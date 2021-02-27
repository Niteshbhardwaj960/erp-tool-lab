using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebERP.Models;

namespace WebERP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<StateModel> States { get; set; }
        public DbSet<CityModel> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Account_Master> Account_Masters { get; set; }
        public DbSet<Term_Master> Term_Master { get; set; }
        public DbSet<UOM_MASTER> UOM_MASTER { get; set; }
        public DbSet<Brand_Master> Brand_Master { get; set; }
        public DbSet<Item_Master> Item_Master { get; set; }
        public DbSet<Artical_Master> Artical_Master { get; set; }
        public DbSet<Size_Master> Size_Master { get; set; }

        public DbSet<POHeaderModel> POHeader_Master { get; set; }
        public DbSet<PODetailModel> PODetail_Master { get; set; }
        public DbSet<POTermsModel> POTerm_Master { get; set; }

        //protected void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<PODetail_Master>().Property(x => x.SnachCount).HasPrecision(16, 3);
        //    modelBuilder.Entity<PODetail_Master>().Property(x => x.MinimumStock).HasPrecision(16, 3);
        //    modelBuilder.Entity<PODetail_Master>().Property(x => x.MaximumStock).HasPrecision(16, 3);
        //}
    }

}
