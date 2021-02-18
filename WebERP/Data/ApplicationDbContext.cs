using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebERP.Models;
//using WebERP.Models.Location;
using WebERP.Models.Location;

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
        public DbSet<Process_Master> Process_Master { get; set; }
        public DbSet<ProcessRate_Master> ProcessRate_Master { get; set; }
        public DbSet<Godown_Master> Godown_Master { get; set; }
    }

}
