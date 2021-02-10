using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebERP.Models;
//using WebERP.Models.Location;

namespace WebERP.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<countryModel> Countries { get; set; }
        public DbSet<stateModel> States { get; set; }
        public DbSet<cityModel> Cities { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Account_Master> Account_Masters { get; set; }
        public DbSet<Term_Master> Term_Master { get; set; }
        public DbSet<UOM_MASTER> UOM_MASTER { get; set; }
        public DbSet<Brand_Master> Brand_Master { get; set; }
    }

}
