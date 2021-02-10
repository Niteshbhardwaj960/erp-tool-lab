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
    public class UOMController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public UOMController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult UOM_Master()
        {
            return View(dbContext.UOM_MASTER.ToList());
        }
        [HttpGet]
        public IActionResult AddUOM()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SAVEUOM(UOM_MASTER objUOM)
        {
            if (ModelState.IsValid)
            {
                objUOM.INS_DATE = DateTime.Now;
                objUOM.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.UOM_MASTER.Add(objUOM);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("UOM_Master");
            }
            else
            {
                return View("UOM_Master");
            }
        }
    }
}