using Cafe.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cafe.Persistance.EntityFramework
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Waiter> Waiters { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<Barista> Baristas { get; set; }

        public DbSet<Cashier> Cashiers { get; set; }

        public DbSet<Table> Tables { get; set; }

        public DbSet<ToGoOrder> ToGoOrders { get; set; }

        public DbSet<ToGoOrderMenuItem> ToGoOrderMenuItems { get; set; }

        public DbSet<MenuItem> MenuItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<ToGoOrderMenuItem>()
                .HasOne(x => x.Order)
                .WithMany(o => o.OrderedItems)
                .HasForeignKey(x => x.OrderId);

            builder
                .Entity<Table>()
                .HasIndex(t => t.Number)
                .IsUnique();

            base.OnModelCreating(builder);
        }
    }
}
