using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    public class TermController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public TermController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Term_Master()
        {
            return View(dbContext.Term_Master.ToList());
        }
        [HttpGet]
        public IActionResult AddTerm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SAVETerm(Term_Master objTerm)
        {
            if (ModelState.IsValid)
            {
                objTerm.INS_DATE = DateTime.Now;
                objTerm.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Term_Master.Add(objTerm);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Term_Master");
            }
            else
            {
                return View("Term_Master");
            }
        }
    }
}