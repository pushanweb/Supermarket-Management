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

        public IList<Invoice> Invoice { get; set; } = default!;

        public async Task OnGetAsync()
        {
            /*if (HttpContext.Session.GetString("username") == null)
            {
                await _signInManager.SignOutAsync();
            }*/

            var invoices = from i in Context.Invoice
                           select i;

            var isManager = User.IsInRole(Constants.InvoiceManagersRole);

            var currentUserId = UserManager.GetUserName(User);

            if (!isManager)
            {
                invoices = invoices.Where(i => i.CreatorId == currentUserId);
            }

            Invoice = await invoices.Include(p => p.Product).ToListAsync();
        }
    }
}
