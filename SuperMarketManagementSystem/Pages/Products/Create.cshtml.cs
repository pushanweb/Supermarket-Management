using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SuperMarketManagementSystem.Authorization;
using SuperMarketManagementSystem.Data;
using SuperMarketManagementSystem.Models;

namespace SuperMarketManagementSystem.Pages.Products
{
    public class CreateModel : DI_BasePageModel
    {
        public CreateModel(ApplicationDbContext context,
            IAuthorizationService authorizationService,
            UserManager<IdentityUser> userManager) :
            base(context, authorizationService, userManager)
        {
        }

        public IActionResult OnGet()
        {
            var isManager = User.IsInRole(Constants.InvoiceManagersRole);
            if (!isManager) return Forbid();
            ViewData["CategoryId"] = new SelectList(Context.Category, "CategoryId", "CategoryName");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!isManager) return Forbid();

            Context.Product.Add(Product);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
