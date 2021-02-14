using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebERP.Models;
using System.Diagnostics;
using WebERP.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public CompanyController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Company()
        { 
            return View(dbContext.Companies.ToList());
        }

        [HttpGet]
        public IActionResult AddCompany()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SAVECompany(Company objCompany)
        {
            using (dbContext)
            {
                dbContext.Companies.Add(objCompany);
                dbContext.SaveChanges();
                Int64 id = objCompany.ID;
            }
            return View(objCompany);
        }
    }
}