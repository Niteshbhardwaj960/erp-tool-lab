using System;
using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Models;
using WebERP.Models.GateEntry;
using Microsoft.EntityFrameworkCore;

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
            Gate_HDR gate_HDRs = new Gate_HDR();
            GateEntryViewModel GateEtryViewModel = new GateEntryViewModel();
            GateEtryViewModel.v_GateEntryDetails = dbContext.V_GateEntryDetail.AsNoTracking().ToList();
            gate_HDRs.AccDropDown = ACClists();
            GateEtryViewModel.Gate_HDR = gate_HDRs;
            return View(GateEtryViewModel);
        }
        [HttpGet]
        public IActionResult Gate_Entry_Details()
        {
            GateEntryDetail GED = new GateEntryDetail();
            return View(GED);
        }
        [HttpPost]
        public IActionResult GateEntry_Master(List<string> ckec, string FinYear, DateTime doc_Date, string ddlACC)
        {
            List<V_GateEntryDetail> li = new List<V_GateEntryDetail>();
            List<V_GateEntryDetail> lli = new List<V_GateEntryDetail>();
            Gate_HDR gate_HDRs = new Gate_HDR();
            GateEntryViewModel GateEtryViewModel = new GateEntryViewModel();
            GateEtryViewModel.v_GateEntryDetails = dbContext.V_GateEntryDetail.AsNoTracking().ToList();
         
            foreach (var order in ckec)
            {
                li = dbContext.V_GateEntryDetail.AsNoTracking().Where(o => o.POD_PK == Convert.ToInt32(order)).ToList();
                foreach (var item in li)
                {
                    lli.Add(item);
                }
            }
            gate_HDRs.Acc_Code = ddlACC.ToString();
            gate_HDRs.Doc_Date = doc_Date;
            gate_HDRs.Doc_FN_Year = FinYear;
            GateEtryViewModel.v_GateEntryDetails = lli.ToList();
            GateEtryViewModel.Gate_HDR = gate_HDRs;
            //return View("GateEntry", li);
            return View("GateEntry", GateEtryViewModel);
        }
        [HttpPost]
        public ActionResult GateEntry(List<GateEntryViewModel> gateEntryViewModels)
        {
            List<GateEntryDetail> GEList = new List<GateEntryDetail>();
            GateEntryDetail li = new GateEntryDetail();
            Gate_HDR GH = new Gate_HDR();
            int GateHdrID;
            GH.Doc_Date = DateTime.Now;
            dbContext.Gate_HDR.Add(GH);
            dbContext.SaveChanges();
            GateHdrID = GH.ID;
            foreach (var order in gateEntryViewModels)
            {
                GEList.Add(new GateEntryDetail()
                {
                    //POD_FK = order.POD_PK,
                    //GH_FK = GateHdrID,
                    //INS_DATE = DateTime.Now,
                    //INS_UID = userManager.GetUserName(HttpContext.User),
                    //Order_No = order.POH_PK,
                    //Bill_Date = order.Bill_Date,
                    //Bill_NO = order.Bill_NO,
                    //CHL_NO = order.CHL_NO,
                    //CHL_DATE = order.CHL_DATE,
                    //Fin_Qty = order.Fin_Qty,
                    //Fin_UOM = order.Fin_UOM,
                    //Stk_Qty = order.Gate_Entry_Qty,
                    //Stk_UOM = order.Stk_UOM,
                    //Item_Name = order.ITEM_CODE,
                    //Remarks = order.REMARKS
                });
            }
            foreach (var item in GEList)
            {
                dbContext.gateEntryDetails.Add(item);
                dbContext.SaveChanges();

            }
            return View();
        }
        [HttpPost]
        public IActionResult AddGateEntry(List<string> ORDER_NO, List<string> CHL_NO, List<DateTime> CHL_DATE, List<string> BILL_NO, List<string> BILL_DATE, List<string> Gate_Entry_Qty, List<string> BAL_QTY, List<string> REMARKS, List<string> ITEM_NAME)
        {

            return View("GateEntry_Master");
        }
        public List<SelectListItem> ACClists()
        {
            var AccList = (from ACC in dbContext.Account_Masters.ToList()
                           select new SelectListItem()
                           {
                               Text = ACC.NAME,
                               Value = Convert.ToString(ACC.ID),
                           }).ToList();

            AccList.Insert(0, new SelectListItem()
            {
                Text = "Select Account",
                Value = string.Empty,
                Selected = true
            });
            return AccList;
        }

        public JsonResult GetGrdData(string accid, string work)
        {
            var grddata = dbContext.V_GateEntryDetail.AsNoTracking().Where(x => x.ACC_CODE == Convert.ToInt32(accid)).ToList();
            return Json(grddata);
        }

    }
}