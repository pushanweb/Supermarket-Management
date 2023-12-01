using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperMarketManagementSystem.Models;

namespace SuperMarketManagementSystem.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<SuperMarketManagementSystem.Models.Category>? Category { get; set; }

    public DbSet<SuperMarketManagementSystem.Models.Product>? Product { get; set; }

    public DbSet<SuperMarketManagementSystem.Models.Invoice>? Invoice { get; set; }
}
