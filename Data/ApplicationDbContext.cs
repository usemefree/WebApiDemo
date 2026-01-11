using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Models;

namespace WebApiDemo.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> option) : base(option)
    {

    }

    public DbSet<Stock> Stocks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Portfolio> portfolios { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Portfolio>(xy => xy.HasKey(p => new { p.AppUserId, p.StockId }));

        builder.Entity<Portfolio>()
            .HasOne(o => o.AppUser)
            .WithMany(m => m.Portfolios)
            .HasForeignKey(f => f.AppUserId);

        builder.Entity<Portfolio>()
            .HasOne(o => o.Stock)
            .WithMany(m => m.Portfolios)
            .HasForeignKey(f => f.StockId);

        List<IdentityRole> roles = new List<IdentityRole>()
        {
            new IdentityRole
            {
                Name="Admin",
                NormalizedName="ADMIN"
            },
             new IdentityRole
            {
                Name="user",
                NormalizedName="USER"
            }
        };
        builder.Entity<IdentityRole>().HasData(roles);
    }
}
