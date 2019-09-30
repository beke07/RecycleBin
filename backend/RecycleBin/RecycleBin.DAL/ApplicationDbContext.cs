using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using RecycleBin.Model;
using Microsoft.EntityFrameworkCore;
using RecycleBin.DAL.Constants;

namespace RecycleBin.DAL
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationRole>()
                .HasData(
                new ApplicationRole
                {
                    Id = Guid.Parse("6b722742-6b8c-4f96-b38f-8f1d27c8a4d4"),
                    Name = RoleNames.Admin,
                    NormalizedName = RoleNames.Admin.ToUpper()
                }, 
                new ApplicationRole
                {
                    Id = Guid.Parse("41ed1abc-da17-44bf-909c-41b43484ee94"),
                    Name = RoleNames.Customer,
                    NormalizedName = RoleNames.Customer.ToUpper()
                });
        }

        public DbSet<Bin> Bins { get; set; }

        public DbSet<BinReport> BinReports { get; set; }
    }
}
