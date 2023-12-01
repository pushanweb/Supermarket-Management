using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuperMarketManagementSystem.Authorization;
using SuperMarketManagementSystem.Data;
using SuperMarketManagementSystem.Models;

namespace SuperMarketManagementSystem.Pages
{
    public class ReportsModel : DI_BasePageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        public ReportsModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager) :
            base(context, authorizationService, userManager)
        {
            _signInManager = signInManager;
        }

        public IList<Invoice> Invoice { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                await _signInManager.SignOutAsync();
            }
            var invoices = from i in Context.Invoice
                           select i;

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            var currentUserId = UserManager.GetUserName(User);

            if (!isManager)
            {
                return Forbid();
            }
            Invoice = await invoices.Include(p => p.Product).ToListAsync();

            return Page();
        }

        public async Task OnPostAsync(string searchString, double? priceHigh, double? priceLow)
        {
            var invoices = from i in Context.Invoice
                           select i;

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            var currentUserId = UserManager.GetUserName(User);

            if (!isManager)
            {
                invoices = invoices.Where(i => i.CreatorId == currentUserId);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                invoices = invoices.Where(s => s.CreatorId.Contains(searchString));
            }

            if (priceHigh != null || priceLow != null)
            {
                if(priceHigh != null && priceLow != null) invoices = invoices.Where(s => s.InvoiceAmount <= priceHigh && s.InvoiceAmount >= priceLow);
                else if (priceHigh != null) invoices = invoices.Where(s => s.InvoiceAmount <= priceHigh);
                else invoices = invoices.Where(s => s.InvoiceAmount >= priceLow);
            }

            Invoice = await invoices.Include(p => p.Product).ToListAsync();
        }
    }
}

