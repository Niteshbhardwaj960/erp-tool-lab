using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    public class BrandController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public BrandController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Brand_Master()
        {
            return View(dbContext.Brand_Master.ToList());
        }
        [HttpGet]
        public IActionResult AddBrand()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SAVEBrand(Brand_Master objBrand)
        {
            if (ModelState.IsValid)
            {
                objBrand.INS_DATE = DateTime.Now;
                objBrand.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Brand_Master.Add(objBrand);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Brand_Master");
            }
            else
            {
                return View("Brand_Master");
            }
        }
    }
}