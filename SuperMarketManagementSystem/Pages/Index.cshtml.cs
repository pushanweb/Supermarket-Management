using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperMarketManagementSystem.Authorization;
using SuperMarketManagementSystem.Data;
using SuperMarketManagementSystem.Models;

namespace SuperMarketManagementSystem.Pages
{
    public class IndexModel : DI_BasePageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public IndexModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager) :
            base(context, authorizationService, userManager)
        {
            _signInManager = signInManager;
        }

        [BindProperty]
        public Invoice Invoice { get; set; } = default!;
        [BindProperty]
        public Product Product { get; set; } = default!;
        public IList<Invoice> IndexInvoice { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            /*if (HttpContext.Session.GetString("username") == null)
            {
                _signInManager.SignOutAsync();
            }*/

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            if (isManager)
            {
                return RedirectToPage("./Invoices/Index");
            }
            
            ViewData["ProductId"] = new SelectList(Context.Product, "ProductId", "ProductName");

            var invoices = from i in Context.Invoice
                           select i;

            var currentUserId = UserManager.GetUserName(User);

            invoices = invoices.Where(i => i.CreatorId == currentUserId);

            IndexInvoice = await invoices.Include(p => p.Product).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Invoice.CreatorId = UserManager.GetUserName(User);

            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                User, Invoice, InvoiceOperations.Create);

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            if (isAuthorized.Succeeded == false || isManager)
                return Forbid();

            var product = await Context.Product.FirstOrDefaultAsync(m => m.ProductId == Invoice.ProductId);
            Product = product;

            Product.Quantity = Product.Quantity - Invoice.QuantitySold;
            Invoice.InvoiceAmount = Invoice.QuantitySold * Product.Price;

            if (Product.Quantity < 0) return Forbid();

            Context.Attach(Product).State = EntityState.Modified;

            Context.Invoice.Add(Invoice);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}