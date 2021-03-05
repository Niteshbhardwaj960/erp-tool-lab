using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Models;

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
            V_PODetails PO = new V_PODetails();
            PO.AccDropDown = ACClists();
            return View(PO);
        }
        [HttpPost]
        public IActionResult GateEntry_Master(List<string> ckec)
        {
            List<V_GateEntryDetail> li = new List<V_GateEntryDetail>();
            List<V_GateEntryDetail> lli = new List<V_GateEntryDetail>();
            foreach (var order in ckec)
            {
                li = dbContext.V_GateEntryDetail.Where(o => o.ORDER_NO == Convert.ToInt32(order)).ToList();
                foreach (var item in li)
                {
                    lli.Add(item);
                }
            }
            //return View("GateEntry", li);
            return View("GateEntry", lli);
        }
        public List<SelectListItem> ACClists()
        {
            var AccList = (from ACC in dbContext.V_PODetails
                            select new SelectListItem()
                            {
                                Text = ACC.ACC_CODE,
                                Value = ACC.ACC_CODE,
                            }).ToList();

            AccList.Insert(0, new SelectListItem()
            {
                Text = "Select Account",
                Value = string.Empty,
                Selected = true
            });
            return AccList;
        }

        public JsonResult GetGrdData(string accid,string work)
        {
            var grddata = dbContext.V_PODetails.Where(x => x.ACC_CODE == accid).ToList();
            return Json(grddata);
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