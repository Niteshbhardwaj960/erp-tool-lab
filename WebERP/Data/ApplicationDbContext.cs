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
    }

}
