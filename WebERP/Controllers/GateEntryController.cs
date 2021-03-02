using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Models;
using WebERP.Models.PurchasingOrder;

namespace WebERP.Controllers
{
    public class GateEntryController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public GateEntryController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult GateEntry_Master()
        {
            return View(dbContext.PODetailModel.ToList());
        }
        [HttpPost]
        public IActionResult GateEntry_Master(List<string> checboxlist)
        {
            //foreach (var item in checboxlist)
            //{
            //    ViewBag.NewData<PODetailModel>  = dbContext.PODetailModel.Find(Convert.ToInt32(item));
            //}
            return View("GateEntry", ViewBag.NewData);
        }
        //[HttpGet]
        //public IActionResult GateEntry(SelectedModel obj)
        //{
        //    return View("GateEntry", obj);
        //}

        //[HttpPost]
        //public IActionResult ADD(List<int> data)
        //{
        //    ViewBag.Message = "Selected Items:\\n";
        //    PODetailModel obj = new PODetailModel();
        //    List<SelectListItem> items = new List<SelectListItem>();
        //    foreach (var item in data)
        //    {
        //        ViewBag.message = dbContext.PODetailModel.Find(item);                
        //    }
        //    return View("GateEntry",data);
        //}
    }
}