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
using Microsoft.AspNetCore.Authorization;
using WebERP.Helpers;

namespace WebERP.Controllers
{
    [Authorize(Roles = "GateEntryUser , Admin")]
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
            GateEtryViewModel.V_JW_DTLs = dbContext.V_JW_DTL.AsNoTracking().ToList();
            gate_HDRs.AccDropDown = ACClists("1");
            gate_HDRs.Doc_Date = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
            gate_HDRs.Doc_FN_Year = GetFinYear();
            GateEtryViewModel.Gate_HDR = gate_HDRs;
            return View(GateEtryViewModel);
        }
        public string GetFinYear()
        {
            string FinYear = "";
            DateTime date = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
            if ((date.Month) == 1 || (date.Month) == 2 || (date.Month) == 3)
            {
                FinYear = (date.Year - 1) + "" + date.Year;
            }
            else
            {
                FinYear = date.Year + "" + (date.Year + 1);
            }
            return FinYear;
        }
        [HttpGet]
        public IActionResult Gate_Entry_Details()
        {
            Gate_HDR GED = new Gate_HDR();
            return View(dbContext.Gate_HDR.ToList());
        }
        [HttpPost]
        public IActionResult GateEntry_Master(List<string> ckec,string ddlwork, string FinYear, DateTime doc_Date, string ddlACC, string Remarks)
        {            
            List<V_GateEntryDetail> li = new List<V_GateEntryDetail>();
            List<V_GateEntryDetail> lli = new List<V_GateEntryDetail>();
            List<V_JW_DTL> JWli = new List<V_JW_DTL>();
            List<V_JW_DTL> JWlli = new List<V_JW_DTL>();
            Gate_HDR gate_HDRs = new Gate_HDR();
            int Doc_Number = dbContext.gateEntryDetails
                .Where(x => x.FIN_YEAR == gate_HDRs.Doc_FN_Year)
                .Select(p => Convert.ToInt32(p.Doc_No)).DefaultIfEmpty(0).Max();
            GateEntryViewModel GateEtryViewModel = new GateEntryViewModel();

            if (ddlwork == "1")
            {                
                GateEtryViewModel.v_GateEntryDetails = dbContext.V_GateEntryDetail.AsNoTracking().ToList();

                foreach (var order in ckec)
                {
                    li = dbContext.V_GateEntryDetail.AsNoTracking().Where(o => o.pod_pk == Convert.ToInt32(order)).ToList();
                    foreach (var item in li)
                    {
                        item.CHL_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                        lli.Add(item);
                    }
                }                
                GateEtryViewModel.Worktype = "1";
                GateEtryViewModel.v_GateEntryDetails = lli.ToList();
            }
            else
            {
                GateEtryViewModel.V_JW_DTLs = dbContext.V_JW_DTL.AsNoTracking().ToList();

                foreach (var order in ckec)
                {
                    JWli = dbContext.V_JW_DTL.AsNoTracking().Where(o => o.JWD_PK == Convert.ToInt32(order)).ToList();
                    foreach (var item in JWli)
                    {
                        item.CHL_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                        JWlli.Add(item);
                    }
                }
                GateEtryViewModel.V_JW_DTLs = JWlli.ToList();
                GateEtryViewModel.Worktype = "2";
            }
            gate_HDRs.Acc_Code = ddlACC.ToString();
            gate_HDRs.Acc_Name = dbContext.Account_Masters.Where(a => a.ID.ToString() == ddlACC.ToString()).Select(b => b.NAME).FirstOrDefault();
            gate_HDRs.Doc_Date = doc_Date;
            gate_HDRs.Doc_FN_Year = FinYear;
            gate_HDRs.Remarks = Remarks;
            gate_HDRs.Doc_No = (Doc_Number + 1).ToString();
            GateEtryViewModel.Gate_HDR = gate_HDRs;
            return View("GateEntry", GateEtryViewModel);
        }
        [HttpPost]
        public ActionResult GateEntry(GateEntryViewModel gateEntryViewModels)
        {
            List<GateEntryDetail> GEList = new List<GateEntryDetail>();
            GateEntryDetail li = new GateEntryDetail();
            int GateHdrID;
            string Account_Name;
            string Document_Number;           

            if (gateEntryViewModels.Worktype == "1")
            {
                gateEntryViewModels.Gate_HDR.Type = "Purchase Order";
                gateEntryViewModels.Gate_HDR.INS_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                gateEntryViewModels.Gate_HDR.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Gate_HDR.Add(gateEntryViewModels.Gate_HDR);
                dbContext.SaveChanges();
                Account_Name = gateEntryViewModels.Gate_HDR.Acc_Name;
                GateHdrID = gateEntryViewModels.Gate_HDR.ID;
                Document_Number = gateEntryViewModels.Gate_HDR.Doc_No;
                foreach (var order in gateEntryViewModels.v_GateEntryDetails)
                {
                    GEList.Add(new GateEntryDetail()
                    {
                        POD_FK = order.pod_pk,
                        GH_FK = GateHdrID,
                        INS_DATE = DateTime.Now,
                        INS_UID = userManager.GetUserName(HttpContext.User),
                        Order_No = order.order_no,
                        GDW_NO = 0,
                        Bill_Date = order.Bill_Date,
                        Bill_NO = order.Bill_NO,
                        CHL_NO = order.CHL_NO,
                        CHL_DATE = order.CHL_DATE,
                        Fin_Qty = order.Fin_Qty,
                        Fin_UOM = order.Fin_UOM,
                        Stk_Qty = order.Bal_Qty,
                        Stk_UOM = order.Stk_UOM,
                        Item_Name = order.Item_Code,
                        Item_UOM = order.QTY_CODE,
                        Remarks = order.Remarks,
                        ACC_NAME = Account_Name,
                        Doc_No = Document_Number
                    });
                }
                foreach (var item in GEList)
                {
                    dbContext.gateEntryDetails.Add(item);
                    dbContext.SaveChanges();

                }
            }
            else
            {
                gateEntryViewModels.Gate_HDR.Type = "Job Work";
                gateEntryViewModels.Gate_HDR.INS_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                gateEntryViewModels.Gate_HDR.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Gate_HDR.Add(gateEntryViewModels.Gate_HDR);
                dbContext.SaveChanges();
                Account_Name = gateEntryViewModels.Gate_HDR.Acc_Name;
                GateHdrID = gateEntryViewModels.Gate_HDR.ID;
                Document_Number = gateEntryViewModels.Gate_HDR.Doc_No;
                foreach (var order in gateEntryViewModels.V_JW_DTLs)
                {
                    GEList.Add(new GateEntryDetail()
                    {
                        JW_FK = order.JWD_PK,
                        GH_FK = GateHdrID,
                        INS_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now)),
                        INS_UID = userManager.GetUserName(HttpContext.User),
                        Order_No = order.DOC_NO,
                        GDW_NO = 0,
                        Bill_Date = order.Bill_Date,
                        Bill_NO = order.Bill_NO,
                        CHL_NO = order.CHL_NO,
                        CHL_DATE = order.CHL_DATE,
                        Fin_Qty = order.Fin_Qty,
                        Fin_UOM = order.Fin_UOM,
                        Stk_Qty = order.BAL_QTY,
                        Stk_UOM = order.Stk_UOM,
                        Item_Name = order.ITEM_CODE,
                        Art_Name = order.ARTICAL_CODE,
                        Size_Name = order.SIZE_CODE,
                        Proc_Name = order.PROC_CODE,
                        Item_UOM = order.QTY_UOM_NAME,
                        Remarks = order.Remarks,
                        ACC_NAME = Account_Name,
                        Doc_No = Document_Number
                    });
                }
                foreach (var item in GEList)
                {
                    dbContext.gateEntryDetails.Add(item);
                    dbContext.SaveChanges();

                }
            }
            return RedirectToAction("Gate_Entry_Details");
        }
        [HttpPost]
        public IActionResult AddGateEntry(List<string> ORDER_NO, List<string> CHL_NO, List<DateTime> CHL_DATE, List<string> BILL_NO, List<string> BILL_DATE, List<string> Gate_Entry_Qty, List<string> BAL_QTY, List<string> REMARKS, List<string> ITEM_NAME)
        {
            return View("GateEntry_Master");
        }
        public List<SelectListItem> ACClists(string types)
        {            
            var AccList = (from ACC in dbContext.V_GATE_ENTRY_ACC.AsNoTracking().Where(ac => ac.TBL_TYPE == types).ToList()
                           select new SelectListItem()
                           {
                               Text = ACC.ACC_NAME,
                               Value = Convert.ToString(ACC.ACC_CODE),
                           }).ToList();

            AccList.Insert(0, new SelectListItem()
            {
                Text = "Select Account",
                Value = string.Empty,
                Selected = true
            });
            return AccList;
        }
        public List<SelectListItem> GDWlists()
        {
            var GDWList = (from ACC in dbContext.Godown_Master.AsNoTracking().ToList()
                           select new SelectListItem()
                           {
                               Text = ACC.NAME,
                               Value = Convert.ToString(ACC.ID),
                           }).ToList();

            return GDWList;
        }
        public JsonResult GetGrdData(string accid, string work)
        {
            if (work == "1")
            {
                var grddata = dbContext.V_GateEntryDetail.AsNoTracking().Where(x => x.ACC_CODE == Convert.ToInt32(accid)).ToList();
                return Json(grddata);
            }
            else
            {
                var grddata = dbContext.V_JW_DTL.AsNoTracking().Where(j => j.ACC_CODE == Convert.ToInt32(accid)).ToList();
                return Json(grddata);
            }            
        }

        [HttpGet]
        public IActionResult EditGateEntry(string id)
        {
            PODetailModel PODetailModels = new PODetailModel();
            EditGateEntryModel GateEntryDetails = new EditGateEntryModel();
            GateEntryDetails.GoDownDropDown = GDWlists();
            var GateDetailList = dbContext.gateEntryDetails.AsNoTracking().Where(g => g.Doc_No == id).ToList();
            foreach (var gateDetailModel in GateDetailList.ToList())
            {
                gateDetailModel.PO_QTY = dbContext.PODetail_Master.
                                           Where(x => x.POD_PK == gateDetailModel.POD_FK).
                                           Select(y => y.QTY).FirstOrDefault();
                gateDetailModel.BAL_QTY = gateDetailModel.PO_QTY - gateDetailModel.Stk_Qty;
                gateDetailModel.ITEM_NAMEs = dbContext.Item_Master.
                                          Where(x => x.ID == gateDetailModel.Item_Name).
                                          Select(y => y.NAME).FirstOrDefault();
                gateDetailModel.ART_NAMEs = dbContext.Artical_Master.
                                          Where(x => x.ID == gateDetailModel.Art_Name).
                                          Select(y => y.NAME).FirstOrDefault();
                gateDetailModel.SIZE_NAMEs = dbContext.Size_Master.
                                          Where(x => x.ID == gateDetailModel.Size_Name).
                                          Select(y => y.NAME).FirstOrDefault();
                gateDetailModel.PROC_NAMEs = dbContext.Process_Master.
                                          Where(x => x.ID == gateDetailModel.Proc_Name).
                                          Select(y => y.NAME).FirstOrDefault();
                var uomcode = dbContext.Item_Master.
                                          Where(x => x.ID == gateDetailModel.Item_Name).
                                          Select(y => y.UOM_CODE).FirstOrDefault();
                gateDetailModel.UOM_NAME = dbContext.UOM_MASTER.
                                          Where(x => x.ID == uomcode).
                                          Select(y => y.NAME).FirstOrDefault();
            }
            GateEntryDetails.EditGateEntryDetails = GateDetailList;
            GateEntryDetails.Gate_HDRs = dbContext.Gate_HDR.Where(g => g.Doc_No == id).FirstOrDefault();
            return View(GateEntryDetails);
        }

        [HttpPost]
        public IActionResult EditGateEntry(EditGateEntryModel EditGateEntryModels)
        {
            if (ModelState.IsValid)
            {
                EditGateEntryModels.Gate_HDRs.UDT_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                EditGateEntryModels.Gate_HDRs.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Gate_HDR.Update(EditGateEntryModels.Gate_HDRs);
                dbContext.SaveChanges();
                foreach (var gateDetailModel in EditGateEntryModels.EditGateEntryDetails.ToList())
                {                   
                    var result = dbContext.gateEntryDetails.SingleOrDefault(b => b.ID == gateDetailModel.ID);
                    if (result != null)
                    {
                        result.UDT_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                        result.UDT_UID = userManager.GetUserName(HttpContext.User);
                        result.CHL_NO = gateDetailModel.CHL_NO;
                        result.CHL_DATE = gateDetailModel.CHL_DATE;
                        result.Bill_Date = gateDetailModel.Bill_Date;
                        result.Bill_NO = gateDetailModel.Bill_NO;
                        result.Stk_Qty = gateDetailModel.Stk_Qty;
                        result.GDW_NO = gateDetailModel.GDW_NO;
                        result.Remarks = gateDetailModel.Remarks;
                        dbContext.SaveChanges();
                    }
                }
                return RedirectToAction("Gate_Entry_Details");
            }
            else
            {
                return View(EditGateEntryModels);
            }
        }
        [HttpGet]
        public IActionResult ActionGateEntry(string id)
        {
            PODetailModel PODetailModels = new PODetailModel();
            EditGateEntryModel GateEntryDetails = new EditGateEntryModel();
            GateEntryDetails.GoDownDropDown = GDWlists();
            var GateDetailList = dbContext.gateEntryDetails.AsNoTracking().Where(g => g.Doc_No == id).ToList();
            foreach (var gateDetailModel in GateDetailList.ToList())
            {
                gateDetailModel.PO_QTY = dbContext.PODetail_Master.
                                           Where(x => x.POD_PK == gateDetailModel.POD_FK).
                                           Select(y => y.QTY).FirstOrDefault();
                gateDetailModel.BAL_QTY = gateDetailModel.PO_QTY - gateDetailModel.Stk_Qty;
                gateDetailModel.ITEM_NAMEs = dbContext.Item_Master.
                                          Where(x => x.ID == gateDetailModel.Item_Name).
                                          Select(y => y.NAME).FirstOrDefault();
                gateDetailModel.ART_NAMEs = dbContext.Artical_Master.
                                          Where(x => x.ID == gateDetailModel.Art_Name).
                                          Select(y => y.NAME).FirstOrDefault();
                gateDetailModel.SIZE_NAMEs = dbContext.Size_Master.
                                          Where(x => x.ID == gateDetailModel.Size_Name).
                                          Select(y => y.NAME).FirstOrDefault();
                gateDetailModel.PROC_NAMEs = dbContext.Process_Master.
                                          Where(x => x.ID == gateDetailModel.Proc_Name).
                                          Select(y => y.NAME).FirstOrDefault();
                var uomcode = dbContext.Item_Master.
                                          Where(x => x.ID == gateDetailModel.Item_Name).
                                          Select(y => y.UOM_CODE).FirstOrDefault();
                gateDetailModel.UOM_NAME = dbContext.UOM_MASTER.
                                         Where(x => x.ID == uomcode).
                                         Select(y => y.NAME).FirstOrDefault();
            }
            GateEntryDetails.EditGateEntryDetails = GateDetailList;
            GateEntryDetails.Gate_HDRs = dbContext.Gate_HDR.Where(g => g.Doc_No == id).FirstOrDefault();
            return View(GateEntryDetails);
        }
        [HttpGet]
        public IActionResult DeleteGateEntry(string ID)
        {
            var HDRdata = dbContext.Gate_HDR.Where(D => D.Doc_No == ID).FirstOrDefault();
            dbContext.Gate_HDR.Remove(HDRdata);
            dbContext.SaveChanges();
            var data = dbContext.gateEntryDetails.Where(D => D.Doc_No == ID).ToList();
            foreach (var Datas in data)
            {
                dbContext.gateEntryDetails.Remove(Datas);
            }
            dbContext.SaveChanges();
            return RedirectToAction("Gate_Entry_Details");
        }

    }
}