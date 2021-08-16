using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    [Authorize(Roles = "RawMaterial , Admin")]
    public class RawMaterialController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public RawMaterialController(
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
            DateTime date = DateTime.Now;
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
        public IActionResult RM_Master()
        {
            RawMaterialDTL rawMaterialDTL = new RawMaterialDTL();
            List<V_RM_DTL> v_rm_dtl = new List<V_RM_DTL>();           
            foreach(var rm in dbContext.V_RM_DTL.AsNoTracking().ToList())
            {
                var gdw = dbContext.Godown_Master.Where(g => g.ID == rm.GDW_CODE && g.SALE_TAG == "1").FirstOrDefault();
                if(gdw != null)
                {
                    v_rm_dtl.Add(rm);
                }
            }
            rawMaterialDTL.V_RM_DTLs = v_rm_dtl;
            rawMaterialDTL.CUTDropDown = CUTlists();
            rawMaterialDTL.Doc_Dates = DateTime.Now;
            rawMaterialDTL.Doc_Fins = GetFinYear();
            return View(rawMaterialDTL);
        }
        [HttpPost]
        public IActionResult RM_Master(RawMaterialDTL rawMaterialDTL)
        {           
            foreach (var order in rawMaterialDTL.V_RM_DTLs)
            {
                if (order.Issue_Qty <= 0 && order.CHK == true)
                {
                   rawMaterialDTL.error = "Issue Qty should be greater than 0";
                }
            }

            if (rawMaterialDTL.error == null)
            {
                int Doc_Number = dbContext.RM_HDR
                .Where(x => x.Doc_FN_Year == rawMaterialDTL.Doc_Fins)
                .Select(p => Convert.ToInt32(p.Doc_No)).DefaultIfEmpty(0).Max();
                int RM_HDR_PK;
                List<RM_DTL> RMDList = new List<RM_DTL>();
                rawMaterialDTL.RM_HDRs.Doc_No = Doc_Number + 1;
                rawMaterialDTL.RM_HDRs.Doc_Date = rawMaterialDTL.Doc_Dates;
                rawMaterialDTL.RM_HDRs.Doc_FN_Year = rawMaterialDTL.Doc_Fins;
                rawMaterialDTL.RM_HDRs.INS_DATE = DateTime.Now;
                rawMaterialDTL.RM_HDRs.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.RM_HDR.Add(rawMaterialDTL.RM_HDRs);
                List<StockDTL_Model> StkDTL = new List<StockDTL_Model>();
                dbContext.SaveChanges();

                RM_HDR_PK = rawMaterialDTL.RM_HDRs.ID;
                foreach (var order in rawMaterialDTL.V_RM_DTLs)
                {
                    if (order.CHK == true)
                    {
                        RMDList.Add(new RM_DTL()
                        {
                            RM_HDR_FK = RM_HDR_PK,
                            INS_DATE = DateTime.Now,
                            INS_UID = userManager.GetUserName(HttpContext.User),
                            GDW_Code = order.GDW_CODE,
                            ITEM_Code = order.ITEM_CODE,
                            ITEM_NAME = order.ITEM_NAME,
                            ARTICAL_Code = order.ARTICAL_CODE,
                            SIZE_Code = order.SIZE_CODE,
                            ISSUE_QTY = order.Issue_Qty,
                        });
                        StkDTL.Add(new StockDTL_Model()
                        {
                            INS_DATE = DateTime.Now,
                            INS_UID = userManager.GetUserName(HttpContext.User),
                            COMP_CODE = 0,
                            Tran_Table = "RM Entry",
                            Tran_Table_PK = order.ID,
                            GDW_CODE = Convert.ToInt32(order.GDW_CODE),
                            Item_Code = Convert.ToInt32(order.ITEM_CODE),
                            Artical_CODE = Convert.ToInt32(order.ARTICAL_CODE),
                            Size_Code = Convert.ToInt32(order.SIZE_CODE),
                            Stk_Qty_OUT = Convert.ToInt32(order.Issue_Qty),
                        });

                        //decimal BalQty;
                        //var result = dbContext.StockDTL_Models.SingleOrDefault(b => b.ID == order.ID);
                        //if (result != null)
                        //{
                        //    BalQty = order.Issue_Qty + result.Stk_Qty_OUT;
                        //    result.Stk_Qty_OUT = Convert.ToInt32(BalQty);
                        //}

                    }
                }
                foreach (var item in RMDList)
                {
                    dbContext.RM_DTL.Add(item);
                    dbContext.SaveChanges();
                }
                foreach (var item in StkDTL)
                {
                    dbContext.StockDTL_Models.Add(item);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("RM_Detail");
            }
            else {
                rawMaterialDTL.CUTDropDown = CUTlists();
                return View("RM_Master", rawMaterialDTL);
            }
        }
        [HttpGet]
        public IActionResult RM_Detail()
        {
            List<RM_HDR> RM_HDRs = new List<RM_HDR>();
            RM_HDRs = dbContext.RM_HDR.AsNoTracking().OrderByDescending(r =>Convert.ToString(r.Doc_No)).ToList();
            return View(RM_HDRs);
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
        [HttpGet]
        public IActionResult EditRM(int id)
        {
            RawMaterialDTL rawMaterialDTL = new RawMaterialDTL();
            rawMaterialDTL.RM_HDRs = dbContext.RM_HDR.Where(r => r.ID == id).FirstOrDefault();
            var RM_DTL_LSTs = dbContext.RM_DTL.Where(l => l.RM_HDR_FK == id).AsNoTracking().ToList();
            foreach (var RMModel in RM_DTL_LSTs.ToList())
            {
                //RMModel. = dbContext.PODetail_Master.
                //                           Where(x => x.POD_PK == gateDetailModel.POD_FK).
                //                           Select(y => y.QTY).FirstOrDefault();
                RMModel.ITEM_NAME = dbContext.Item_Master.
                                          Where(x => x.ID == RMModel.ITEM_Code).
                                          Select(y => y.NAME).FirstOrDefault();
                RMModel.ARTICAL_NAME = dbContext.Artical_Master.
                                          Where(x => x.ID == RMModel.ARTICAL_Code).
                                          Select(y => y.NAME).FirstOrDefault();
                RMModel.GDW_NAME = dbContext.Godown_Master.
                                          Where(x => x.ID == RMModel.GDW_Code).
                                          Select(y => y.NAME).FirstOrDefault();
                RMModel.SIZE_NAME = dbContext.Size_Master.
                                          Where(x => x.ID == RMModel.SIZE_Code).
                                          Select(y => y.NAME).FirstOrDefault();
            }
            rawMaterialDTL.RM_DTL_LST = RM_DTL_LSTs;
            return View("RM_Edit", rawMaterialDTL);
        }
        [HttpPost]
        public IActionResult SAVERM(RawMaterialDTL RawMaterialDTLs)
        {
            foreach (var order in RawMaterialDTLs.RM_DTL_LST)
            {
                if (order.ISSUE_QTY <= 0 && order.CHK == true)
                {
                    RawMaterialDTLs.error = "Issue Qty should be greater than 0";
                }
            }

            if (RawMaterialDTLs.error == null)
            {             
                RawMaterialDTLs.RM_HDRs.UDT_DATE = DateTime.Now;
                RawMaterialDTLs.RM_HDRs.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.RM_HDR.Update(RawMaterialDTLs.RM_HDRs);
                dbContext.SaveChanges();
                foreach (var RMDetailModel in RawMaterialDTLs.RM_DTL_LST.ToList())
                {
                    var result = dbContext.RM_DTL.SingleOrDefault(b => b.ID == RMDetailModel.ID);
                    if (result != null)
                    {
                        result.UDT_DATE = DateTime.Now;
                        result.UDT_UID = userManager.GetUserName(HttpContext.User);
                        result.ISSUE_QTY = RMDetailModel.ISSUE_QTY;
                        dbContext.SaveChanges();
                    }
                    var resultStk = dbContext.StockDTL_Models.SingleOrDefault(b => b.Tran_Table_PK == RMDetailModel.ID && b.Tran_Table == "RM Entry");
                    if (resultStk != null)
                    {
                        resultStk.UDT_DATE = DateTime.Now;
                        resultStk.UDT_UID = userManager.GetUserName(HttpContext.User);
                        resultStk.Stk_Qty_OUT = RMDetailModel.ISSUE_QTY;
                        resultStk.GDW_CODE = RMDetailModel.GDW_Code;
                        dbContext.SaveChanges();
                    }
                }
                return RedirectToAction("RM_Detail");
            }
            else
            {
                return View("RM_Edit", RawMaterialDTLs);
            }
        }
        [HttpGet]
        public IActionResult ActionRM(int id)
        {
            RawMaterialDTL rawMaterialDTL = new RawMaterialDTL();
            rawMaterialDTL.RM_HDRs = dbContext.RM_HDR.Where(r => r.ID == id).FirstOrDefault();
            var RM_DTL_LSTs = dbContext.RM_DTL.Where(l => l.RM_HDR_FK == id).AsNoTracking().ToList();
            foreach (var RMModel in RM_DTL_LSTs.ToList())
            {
                //RMModel. = dbContext.PODetail_Master.
                //                           Where(x => x.POD_PK == gateDetailModel.POD_FK).
                //                           Select(y => y.QTY).FirstOrDefault();
                RMModel.ITEM_NAME = dbContext.Item_Master.
                                          Where(x => x.ID == RMModel.ITEM_Code).
                                          Select(y => y.NAME).FirstOrDefault();
                RMModel.ARTICAL_NAME = dbContext.Artical_Master.
                                          Where(x => x.ID == RMModel.ARTICAL_Code).
                                          Select(y => y.NAME).FirstOrDefault();
                RMModel.GDW_NAME = dbContext.Godown_Master.
                                          Where(x => x.ID == RMModel.GDW_Code).
                                          Select(y => y.NAME).FirstOrDefault();
                RMModel.SIZE_NAME = dbContext.Size_Master.
                                          Where(x => x.ID == RMModel.SIZE_Code).
                                          Select(y => y.NAME).FirstOrDefault();
            }
            rawMaterialDTL.RM_DTL_LST = RM_DTL_LSTs;
            return View("RM_View", rawMaterialDTL);
        }
        [HttpGet]
        public IActionResult DeleteRM(int ID)
        {
            var HDRdata = dbContext.RM_HDR.Where(D => D.ID == ID).FirstOrDefault();
            var cuttingreciept = dbContext.Cutting_Receipt.Where(c => c.CUTTING_ORDER_FK == HDRdata.Cutting_Order_FK).FirstOrDefault();
            if (cuttingreciept == null)
            {                
                dbContext.RM_HDR.Remove(HDRdata);
                dbContext.SaveChanges();
                var data = dbContext.RM_DTL.Where(D => D.RM_HDR_FK == ID).ToList();
                foreach (var Datas in data)
                {
                    dbContext.RM_DTL.Remove(Datas);
                }
                dbContext.SaveChanges();
                return RedirectToAction("RM_Detail");
            }
            else
            {
                ViewBag.Message = string.Format("Can not delete entry. Record present in Cutting Receipt.");
                return View("RM_Detail", dbContext.RM_HDR.ToList());
            }
            
        }
    }
}