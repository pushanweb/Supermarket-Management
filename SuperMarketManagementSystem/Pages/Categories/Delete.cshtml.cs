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

namespace SuperMarketManagementSystem.Pages.Categories
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
      public Category Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var isManager = User.IsInRole(Constants.InvoiceManagersRole);
            if (!isManager) return Forbid();

            if (id == null || Context.Category == null)
            {
                return NotFound();
            }

            var category = await Context.Category.FirstOrDefaultAsync(m => m.CategoryId == id);

            if (category == null)
            {
                return NotFound();
            }
            else 
            {
                Category = category;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || Context.Category == null)
            {
                return NotFound();
            }
            var category = await Context.Category.FindAsync(id);

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);
            if (!isManager) return Forbid();


            if (category != null)
            {
                Category = category;
                Context.Category.Remove(Category);
                await Context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
