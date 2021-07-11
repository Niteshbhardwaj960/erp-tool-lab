using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebERP.Data;
using WebERP.Helpers;
using WebERP.Models;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Tax , Admin")]
    public class TaxController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public TaxController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult Tax_Detail()
        {
            ViewBag.Message = null;

               List <TAX_MASTER> tAX_MASTER = new List<TAX_MASTER>();

            tAX_MASTER = dbContext.TAX_MASTER.AsNoTracking().ToList();

            return View(tAX_MASTER);
        }
        [HttpGet]
        public IActionResult Tax_Master()
        {
            TAX_MASTER tAX_MASTER = new TAX_MASTER();
            tAX_MASTER.Type = "Add";
            return View(tAX_MASTER);
        }
        [HttpPost]
        public IActionResult Tax_Master(TAX_MASTER tAX_MASTER)
        {
            var NAME = dbContext.TAX_MASTER.FirstOrDefault(x => x.TAX_NAME == tAX_MASTER.TAX_NAME);

            if (NAME != null)
            {
                ModelState.AddModelError("Tax_Name", "Tax Name Already Exists.");
            }
            if (ModelState.IsValid)
            {
                tAX_MASTER.INS_DATE = DateTime.Now;
                tAX_MASTER.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.TAX_MASTER.Add(tAX_MASTER);
                dbContext.SaveChanges();
                return RedirectToAction("Tax_Detail");
            }
            else
            {
                tAX_MASTER.Type = "Add";
                return View("Tax_Master", tAX_MASTER);
            }
        }
        [HttpGet]
        public IActionResult ActionTax(int id)
        {
            TAX_MASTER obj = new TAX_MASTER();
            obj = dbContext.TAX_MASTER.Find(id);
            obj.Type = "Action";
            dbContext.TAX_MASTER.Update(obj);
            dbContext.SaveChanges();
            return View("Tax_Master", obj);
        }
        [HttpGet]
        public IActionResult EditTax(int id)
        {
            TAX_MASTER obj = new TAX_MASTER();
            obj = dbContext.TAX_MASTER.Find(id);
            obj.Type = "Edit";
            dbContext.TAX_MASTER.Update(obj);
            dbContext.SaveChanges();
            return View("Tax_Master", obj);
        }

        [HttpPost]
        public IActionResult EditTax(TAX_MASTER obj)
        {
            if (ModelState.IsValid)
            {
                obj.UDT_DATE = DateTime.Now;
                obj.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.TAX_MASTER.Update(obj);
                dbContext.SaveChanges();
                return RedirectToAction("Tax_Detail");
            }
            else
            {
                obj.Type = "Edit";
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult DeleteTax(int ID)
        {
           var dupl = dbContext.SalesHeader.Where(p => p.TAX_CODE == ID).FirstOrDefault();

            if (dupl == null)
            {
                var data = dbContext.TAX_MASTER.Find(ID);
                dbContext.TAX_MASTER.Remove(data);
                dbContext.SaveChanges();
            }
            else
            {
                var TAX_MASTER = dbContext.TAX_MASTER.ToList();
                ViewBag.Message = string.Format("Can not delete entry. Record present in Sale Invoice.");
                ViewBag.Color = "red";
                return View("Tax_Detail", TAX_MASTER);
            }
            return RedirectToAction("Tax_Detail");
        }
    }
}