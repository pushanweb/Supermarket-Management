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

namespace SuperMarketManagementSystem.Pages.Invoices
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
        public Invoice Invoice { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || Context.Invoice == null)
            {
                return NotFound();
            }

            var invoice = await Context.Invoice.FirstOrDefaultAsync(m => m.InvoiceId == id);

            if (invoice == null)
            {
                return NotFound();
            }
            else
            {
                Invoice = invoice;
            }

            var isCreator = await AuthorizationService.AuthorizeAsync(
                User, Invoice, InvoiceOperations.Delete);

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            if (isCreator.Succeeded == false && !isManager)
                return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {

            Invoice = await Context.Invoice.FindAsync(id);

            if (Invoice == null)
            {
                return NotFound();
            }

            var isCreator = await AuthorizationService.AuthorizeAsync(
                User, Invoice, InvoiceOperations.Delete);

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            if (isCreator.Succeeded == false && !isManager)
                return Forbid();

            Context.Invoice.Remove(Invoice);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
