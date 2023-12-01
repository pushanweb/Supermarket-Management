using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuperMarketManagementSystem.Authorization;
using SuperMarketManagementSystem.Data;
using SuperMarketManagementSystem.Models;

namespace SuperMarketManagementSystem.Pages.Products
{
    public class DeleteModel : DI_BasePageModel
    {
        public DeleteModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) :
            base(context, authorizationService, userManager)
        {
        }

        [BindProperty]
      public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);
            if (!isManager) return Forbid();

            if (id == null || Context.Product == null)
            {
                return NotFound();
            }

            var product = await Context.Product.FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
            {
                return NotFound();
            }
            else 
            {
                Product = product;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || Context.Product == null)
            {
                return NotFound();
            }
            var product = await Context.Product.FindAsync(id);

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);
            if (!isManager) return Forbid();

            if (product != null)
            {
                Product = product;
                Context.Product.Remove(Product);
                await Context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
