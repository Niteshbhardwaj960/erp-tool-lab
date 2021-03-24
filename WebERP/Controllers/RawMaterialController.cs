using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
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
        [HttpGet]
        public IActionResult RM_Master()
        {
            RawMaterialDTL rawMaterialDTL = new RawMaterialDTL();
            rawMaterialDTL.V_RM_DTLs = dbContext.V_RM_DTL.AsNoTracking().ToList();
            rawMaterialDTL.CUTDropDown = CUTlists();
            return View(rawMaterialDTL);
        }
        [HttpPost]
        public IActionResult RM_Master(RawMaterialDTL rawMaterialDTL)
        {
            int Doc_Number = dbContext.RM_HDR
                .Where(x => x.Doc_FN_Year == rawMaterialDTL.RM_HDR.Doc_FN_Year)
                .Select(p => Convert.ToInt32(p.Doc_No)).DefaultIfEmpty(0).Max();
            int RM_HDR_PK;
            List<RM_DTL> RMDList = new List<RM_DTL>();
            rawMaterialDTL.RM_HDR.Doc_No = Doc_Number + 1;
            rawMaterialDTL.RM_HDR.INS_DATE = DateTime.Now;
            rawMaterialDTL.RM_HDR.INS_UID = userManager.GetUserName(HttpContext.User);
            dbContext.RM_HDR.Add(rawMaterialDTL.RM_HDR);
            dbContext.SaveChanges();

            RM_HDR_PK = rawMaterialDTL.RM_HDR.ID;
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
                        ARTICAL_Code = order.ARTICAL_CODE,
                        SIZE_Code = order.SIZE_CODE,
                        ISSUE_QTY = order.Issue_Qty,
                    });
                    decimal BalQty;
                    var result = dbContext.StockDTL_Models.SingleOrDefault(b => b.ID == order.ID);
                    if (result != null)
                    {
                        BalQty = order.Issue_Qty + result.Stk_Qty_OUT;
                        result.Stk_Qty_OUT = Convert.ToInt32(BalQty);
                    }

                }
            }
            foreach (var item in RMDList)
            {
                dbContext.RM_DTL.Add(item);
                dbContext.SaveChanges();
            }
            return RedirectToAction("RM_Detail");
        }
        [HttpGet]
        public IActionResult RM_Detail()
        {
            List<RM_HDR> RM_HDRs = new List<RM_HDR>();
            RM_HDRs = dbContext.RM_HDR.AsNoTracking().ToList();
            return View(RM_HDRs);
        }
        public List<SelectListItem> CUTlists()
        {
            var CutList = (from Cut in dbContext.V_CuttingDetail.Where(e => e.ORDER_STATUS == "1").ToList()
                           select new SelectListItem()
                           {
                               Text =Cut.DOC_NO + '/' + Cut.EMP_NAME + '/' + Cut.Item_NAME + '/' + Cut.ARTICAL_NAME + '/' + Cut.SIZE_NAME + '/' + Cut.PROC_NAME,
                               Value = Cut.EMP_NAME + '/' + Cut.Item_NAME + '/' + Cut.ARTICAL_NAME + '/' + Cut.SIZE_NAME + '/' + Cut.PROC_NAME + '/' + Cut.ID,
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
            rawMaterialDTL.RM_HDR = dbContext.RM_HDR.Where(r => r.ID == id).FirstOrDefault();
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
            rawMaterialDTL.RM_DTL_LST= RM_DTL_LSTs;
            return View("RM_Edit", rawMaterialDTL);
        }
        [HttpPost]
        public IActionResult SAVERM(RawMaterialDTL RawMaterialDTLs)
        {
            if (ModelState.IsValid)
            {
                RawMaterialDTLs.RM_HDR.UDT_DATE = DateTime.Now;
                RawMaterialDTLs.RM_HDR.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.RM_HDR.Update(RawMaterialDTLs.RM_HDR);
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
                }
                return RedirectToAction("RM_Detail");
            }
            else
            {
                return View(RawMaterialDTLs);
            }
        }
        [HttpGet]
        public IActionResult ActionRM(int id)
        {
            RawMaterialDTL rawMaterialDTL = new RawMaterialDTL();
            rawMaterialDTL.RM_HDR = dbContext.RM_HDR.Where(r => r.ID == id).FirstOrDefault();
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
    }
}