using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperMarketManagementSystem.Authorization;
using SuperMarketManagementSystem.Data;
using SuperMarketManagementSystem.Models;

namespace SuperMarketManagementSystem.Pages.Invoices
{
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) :
            base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
        public Invoice Invoice { get; set; } = default!;
        [BindProperty]
        public Product Product { get; set; } = default!;

        public IActionResult OnGet()
        {
            var isManager = User.IsInRole(Constants.InvoiceManagersRole);
            if (isManager) return Forbid();
            ViewData["ProductId"] = new SelectList(Context.Product, "ProductId", "ProductName");
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
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
            
            Invoice.QtyLeft = Product.Quantity;

            Invoice.InvoiceAmount = Invoice.QuantitySold * Product.Price;

            if (Product.Quantity < 0) return Forbid();

            Context.Attach(Product).State = EntityState.Modified;

            Context.Invoice.Add(Invoice);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
