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
    [Authorize(Roles = "RawMaterialReturn , Admin")]
    public class RMRController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public object StkDTL { get; private set; }

        public RMRController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
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
        public IActionResult RMR_MASTER()
        {
            RMRViewModel rMRViewModel = new RMRViewModel();
            //rMRViewModel.V_RM_DTLs = dbContext.V_RM_DTL.AsNoTracking().ToList();
            rMRViewModel.CUTDropDown = CUTlists();
            rMRViewModel.Doc_Dates = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
            rMRViewModel.Doc_Fins = GetFinYear();
            return View("RMR_MASTER", rMRViewModel);
        }
        [HttpGet]
        public IActionResult RMR_DETAIL()
        {
            List<RMR_HDR> rMR_HDR = new List<RMR_HDR>();
            rMR_HDR = dbContext.RMR_HDR.AsNoTracking().ToList();
            return View(rMR_HDR);
        }
        public List<SelectListItem> CUTlists()
        {
            var CutList = (from Cut in dbContext.V_CuttingDetail.Where(e => e.ORDER_STATUS == "1").ToList()
                           select new SelectListItem()
                           {
                               Text = Cut.DOC_NO.ToString() + '/' + Cut.EMP_NAME + '/' + Cut.Item_NAME + '/' + Cut.ARTICAL_NAME + '/' + Cut.SIZE_NAME + '/' + Cut.PROC_NAME,
                               Value = Cut.EMP_NAME + '^' + Cut.Item_NAME + '^' + Cut.ARTICAL_NAME + '^' + Cut.SIZE_NAME + '^' + Cut.PROC_NAME + '^' + Cut.DOC_NO + '^' + Cut.ID,
                           }).ToList();

            CutList.Insert(0, new SelectListItem()
            {
                Text = "Select Cutting Order",
                Value = string.Empty,
                Selected = true
            });
            return CutList;
        }
        public List<SelectListItem> GDWlists()
        {
            var GDWList = (from GDW in dbContext.Godown_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = GDW.NAME,
                               Value = GDW.ID.ToString(),
                           }).ToList();            
            return GDWList;
        }
        public JsonResult GetRMDetail(string cutid)
        {
            var grddata = dbContext.V_RM_ISSUE.AsNoTracking().Where(j => j.Cutting_Order_FK == Convert.ToInt32(cutid)).ToList();
            return Json(grddata);

        }
        [HttpPost]
        public IActionResult RMR_MASTER(RMRViewModel rMRViewModel)
        {
            rMRViewModel.RMR_HDR.Doc_Date = rMRViewModel.Doc_Dates;
            rMRViewModel.RMR_HDR.Doc_FN_Year = rMRViewModel.Doc_Fins;
            rMRViewModel.GDWDropDown = GDWlists();
            rMRViewModel.v_RM_ISSUEs = dbContext.V_RM_ISSUE.AsNoTracking().Where(j => j.Cutting_Order_FK == Convert.ToInt32(rMRViewModel.RMR_HDR.Cutting_Order_FK)).ToList();
            return View("RMR", rMRViewModel);
        }

        [HttpPost]
        public IActionResult RMR(RMRViewModel rMRViewModel)
        {
            int Doc_Number = dbContext.RMR_HDR
                .Where(x => x.Doc_FN_Year == rMRViewModel.Doc_Fins)
                .Select(p => Convert.ToInt32(p.Doc_No)).DefaultIfEmpty(0).Max();
            int RMR_HDR_PK;
            List<RMR_DTL> RMDList = new List<RMR_DTL>();
            rMRViewModel.RMR_HDR.Doc_No = Doc_Number + 1;
            rMRViewModel.RMR_HDR.Doc_Date = Helper.DateFormatDate(Convert.ToString(rMRViewModel.RMR_HDR.Doc_Date));
            rMRViewModel.RMR_HDR.Doc_FN_Year = rMRViewModel.RMR_HDR.Doc_FN_Year;
            rMRViewModel.RMR_HDR.INS_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
            rMRViewModel.RMR_HDR.INS_UID = userManager.GetUserName(HttpContext.User);
            dbContext.RMR_HDR.Add(rMRViewModel.RMR_HDR);
            List<StockDTL_Model> StkDTL = new List<StockDTL_Model>();
            dbContext.SaveChanges();

            RMR_HDR_PK = rMRViewModel.RMR_HDR.ID;
            foreach (var order in rMRViewModel.v_RM_ISSUEs)
            {
                if (order.CHK)
                {
                    // var RMRISSUE = dbContext.V_RM_ISSUE.Where(r => r.ID = )
                    RMDList.Add(new RMR_DTL()
                    {
                        RM_HDR_FK = RMR_HDR_PK,
                        INS_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now)),
                        INS_UID = userManager.GetUserName(HttpContext.User),
                        GDW_Code = order.GDW_Code,
                        ITEM_Code = order.ITEM_Code,
                        ITEM_NAME = order.ITEM_NAME,
                        ARTICAL_Code = order.ARTICAL_Code,
                        SIZE_Code = order.SIZE_Code,
                        ISSUE_QTY = order.ISSUE_QTY,
                        ORDER_QTY = order.return_qty,
                    });

                }                
            }
            foreach (var item in RMDList)
            {
                dbContext.RMR_DTL.Add(item);
                dbContext.SaveChanges();

                StkDTL.Add(new StockDTL_Model()
                {
                    INS_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now)),
                    INS_UID = userManager.GetUserName(HttpContext.User),
                    COMP_CODE = 0,
                    Tran_Table = "RMR Entry",
                    Tran_Table_PK = item.ID,
                    GDW_CODE = Convert.ToInt32(item.GDW_Code),
                    Item_Code = Convert.ToInt32(item.ITEM_Code),
                    Artical_CODE = Convert.ToInt32(item.ARTICAL_Code),
                    Size_Code = Convert.ToInt32(item.SIZE_Code),
                    Stk_Qty_IN = Convert.ToInt32(item.ORDER_QTY),
                });
            }
            foreach (var item in StkDTL)
            {
                dbContext.StockDTL_Models.Add(item);
                dbContext.SaveChanges();
            }
            return RedirectToAction("RMR_DETAIL");
        }

        [HttpGet]
        public IActionResult RMREDIT(int id)
        {
            RMRViewModel rMRViewModel = new RMRViewModel();
            rMRViewModel.RMR_HDR = dbContext.RMR_HDR.Where(r => r.ID == id).FirstOrDefault();
            rMRViewModel.RMR_HDR.Doc_Date = Helper.DateFormatDate(Convert.ToString(rMRViewModel.RMR_HDR.Doc_Date));
            rMRViewModel.RMR_DTL_LIST = dbContext.RMR_DTL.Where(r => r.RM_HDR_FK == id).ToList();

            foreach(var item in rMRViewModel.RMR_DTL_LIST)
            {
                item.ARTICAL_NAME = dbContext.Artical_Master.Where(a => a.ID == item.ARTICAL_Code).Select(aa =>aa.NAME).FirstOrDefault();
                item.SIZE_NAME = dbContext.Size_Master.Where(a => a.ID == item.SIZE_Code).Select(aa => aa.NAME).FirstOrDefault();
                item.ITEM_NAME = dbContext.Item_Master.Where(a => a.ID == item.ITEM_Code).Select(aa => aa.NAME).FirstOrDefault();
            }
            rMRViewModel.GDWDropDown = GDWlists();            
            return View("RMR_EDIT", rMRViewModel);
        }

        [HttpPost]
        public IActionResult RMREDIT(RMRViewModel rMRViewModel)
        {
            List<StockDTL_Model> StkDTL = new List<StockDTL_Model>();
            dbContext.SaveChanges();           
            foreach (var order in rMRViewModel.RMR_DTL_LIST)
            {
                if (order.CHK)
                {
                    var result = dbContext.RMR_DTL.Where(r => r.ID == order.ID).FirstOrDefault();

                    if (result != null)
                    {
                        result.UDT_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                        result.UDT_UID = userManager.GetUserName(HttpContext.User);           
                        result.GDW_Code = order.GDW_Code;
                        result.ORDER_QTY = order.ORDER_QTY;
                        dbContext.SaveChanges();
                    }

                    var resultstk = dbContext.StockDTL_Models.Where(r => r.Tran_Table_PK == order.ID && r.Tran_Table == "RMR Entry").FirstOrDefault();
                    if (resultstk != null)
                    {
                        resultstk.UDT_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                        resultstk.UDT_UID = userManager.GetUserName(HttpContext.User);
                        resultstk.GDW_CODE = order.GDW_Code;
                        resultstk.Stk_Qty_IN = order.ORDER_QTY;
                        dbContext.SaveChanges();
                    }
                }
            }          
            return RedirectToAction("RMR_DETAIL");
        }
        [HttpGet]
        public IActionResult RMRDelete(int ID)
        {
            var HDRdata = dbContext.RMR_HDR.Where(D => D.ID == ID).FirstOrDefault();
            dbContext.RMR_HDR.Remove(HDRdata);
            dbContext.SaveChanges();
            var data = dbContext.RMR_DTL.Where(D => D.RM_HDR_FK == ID).ToList();
            foreach (var Datas in data)
            {
                dbContext.RMR_DTL.Remove(Datas);
                dbContext.SaveChanges();
                var datastk = dbContext.StockDTL_Models.Where(D => D.Tran_Table_PK == Datas.ID && D.Tran_Table == "RMR Entry").FirstOrDefault();
                dbContext.StockDTL_Models.Remove(datastk);
                dbContext.SaveChanges();
            }        
            return RedirectToAction("RMR_DETAIL");
        }
    }
}