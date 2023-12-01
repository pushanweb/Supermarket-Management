using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SuperMarketManagementSystem.Data;
using SuperMarketManagementSystem.Models;

namespace SuperMarketManagementSystem.Pages.Products
{
    public class IndexModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly SuperMarketManagementSystem.Data.ApplicationDbContext _context;

        public IndexModel(SuperMarketManagementSystem.Data.ApplicationDbContext context, SignInManager<IdentityUser> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            /*if (HttpContext.Session.GetString("username") == null)
            {
                await _signInManager.SignOutAsync();
            }*/

            if (_context.Product != null)
            {
                Product = await _context.Product
                .Include(p => p.Category).ToListAsync();
            }
        }
        public async Task OnPostAsync(string searchString, int? Category, double? priceHigh, double? priceLow)
        {
            var products = from i in _context.Product
                           select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString));
            }

            if (Category != null)
            {
                products = products.Where(s => s.CategoryId == Category);
            }

            if (priceHigh != null || priceLow !=null)
            {
                if(priceHigh != null && priceLow != null) products = products.Where(s => s.Price <= priceHigh && s.Price >= priceLow);
                else if (priceHigh != null) products = products.Where(s => s.Price <= priceHigh);
                else products = products.Where(s => s.Price >= priceLow);
            }

            Product = await products.Include(p => p.Category).ToListAsync();
        }
    }
}
