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
using WebERP.Models;

namespace WebERP.Controllers
{
    [Authorize(Roles = "CuttingReceipt , Admin")]
    public class CuttingReceiptController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public CuttingReceiptController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        public int GetFinYear()
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
            return Convert.ToInt32(FinYear);
        }
        [HttpGet]
        public IActionResult Cut_Recpt_Master()
        {
            CuttingReceiptViewModel cuttingReceiptViewModel = new CuttingReceiptViewModel();
            cuttingReceiptViewModel.Type = "Add";
            cuttingReceiptViewModel.CUTDropDown = CUTlists();
            cuttingReceiptViewModel.GDWDropDown = GDWlists();
            cuttingReceiptViewModel.DOc_Dates = DateTime.Now;
            cuttingReceiptViewModel.Fin_Years = GetFinYear();
            return View(cuttingReceiptViewModel);
        }
        public List<SelectListItem> GDWlists()
        {
            var gdwList = (from gdw in dbContext.Godown_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = gdw.NAME,
                               Value = gdw.ID.ToString(),
                           }).ToList();

            gdwList.Insert(0, new SelectListItem()
            {
                Text = "Select Godown",
                Value = string.Empty,
                Selected = true
            });
            return gdwList;
        }
        public List<SelectListItem> CUTlists()
        {
            var CutList = (from Cut in dbContext.V_CuttingDetail.Where(e => e.ORDER_STATUS == "1").ToList()
                           select new SelectListItem()
                           {
                               Text = Cut.DOC_NO.ToString() + '/' + Cut.EMP_NAME + '/' + Cut.Item_NAME + '/' + Cut.ARTICAL_NAME + '/' + Cut.SIZE_NAME + '/' + Cut.PROC_NAME,
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
        [HttpPost]
        public IActionResult Cut_Recpt_Master(CuttingReceiptViewModel cuttingReceiptViewModel)
        {
            StockDTL_Model StkDTL = new StockDTL_Model();
            int Doc_Number = dbContext.Cutting_Receipt
                .Where(x => x.DOC_FINYEAR == cuttingReceiptViewModel.Fin_Years)
                .Select(p => Convert.ToInt32(p.DOC_NO)).DefaultIfEmpty(0).Max();
            cuttingReceiptViewModel.cutting_Receipt.DOC_NO = Doc_Number + 1;
            cuttingReceiptViewModel.cutting_Receipt.INS_DATE = DateTime.Now;
            cuttingReceiptViewModel.cutting_Receipt.DOC_DATE = cuttingReceiptViewModel.DOc_Dates;
            cuttingReceiptViewModel.cutting_Receipt.DOC_FINYEAR = cuttingReceiptViewModel.Fin_Years;
            cuttingReceiptViewModel.cutting_Receipt.INS_UID = userManager.GetUserName(HttpContext.User);
            dbContext.Cutting_Receipt.Add(cuttingReceiptViewModel.cutting_Receipt);
            dbContext.SaveChanges();
            StkDTL.INS_DATE = DateTime.Now;
            StkDTL.INS_UID = userManager.GetUserName(HttpContext.User);
            StkDTL.COMP_CODE = 0;
            StkDTL.Tran_Table = "Cutting Receipt Entry";
            StkDTL.Tran_Table_PK = cuttingReceiptViewModel.cutting_Receipt.ID;
            StkDTL.GDW_CODE = cuttingReceiptViewModel.cutting_Receipt.GDW_CODE;
            StkDTL.Item_Code = dbContext.Item_Master.Where(i => i.NAME == cuttingReceiptViewModel.cutting_Receipt.ITEM_NAME).Select(n => n.ID).FirstOrDefault();
            StkDTL.Artical_CODE = dbContext.Artical_Master.Where(ar => ar.NAME == cuttingReceiptViewModel.cutting_Receipt.ART_NAME).Select(na => na.ID).FirstOrDefault();
            StkDTL.Size_Code = dbContext.Size_Master.Where(s => s.NAME == cuttingReceiptViewModel.cutting_Receipt.SIZE_NAME).Select(sn => sn.ID).FirstOrDefault();
            StkDTL.Stk_Qty_IN = cuttingReceiptViewModel.cutting_Receipt.RECEIPT_QTY;
            dbContext.StockDTL_Models.Add(StkDTL);
            dbContext.SaveChanges();
            return RedirectToAction("CuttingReceiptDetail");
        }
        [HttpGet]
        public IActionResult CuttingReceiptDetail()
        {
            List<Cutting_Receipt> cutting_Receipts = new List<Cutting_Receipt>();
            cutting_Receipts = dbContext.Cutting_Receipt.AsNoTracking().ToList();
            return View(cutting_Receipts);
        }
        [HttpGet]
        public IActionResult EditCR(int id)
        {
            CuttingReceiptViewModel cuttingReceiptViewModel = new CuttingReceiptViewModel();
            cuttingReceiptViewModel.cutting_Receipt = dbContext.Cutting_Receipt.Where(r => r.ID == id).FirstOrDefault();
            cuttingReceiptViewModel.Type = "Edit";
            cuttingReceiptViewModel.GDWDropDown = GDWlists();
            return View("Cut_Recpt_Master", cuttingReceiptViewModel);
        }
        [HttpPost]
        public IActionResult SAVECR(CuttingReceiptViewModel cuttingReceiptViewModel)
        {
            var result = dbContext.Cutting_Receipt.SingleOrDefault(b => b.ID == cuttingReceiptViewModel.cutting_Receipt.ID);
            if (result != null)
            {
                result.UDT_DATE = DateTime.Now;
                result.UDT_UID = userManager.GetUserName(HttpContext.User);
                result.RECEIPT_QTY = cuttingReceiptViewModel.cutting_Receipt.RECEIPT_QTY;
                result.GDW_CODE = cuttingReceiptViewModel.cutting_Receipt.GDW_CODE;
                dbContext.SaveChanges();
            }
            var resultStk = dbContext.StockDTL_Models.SingleOrDefault(b => b.Tran_Table_PK == cuttingReceiptViewModel.cutting_Receipt.ID && b.Tran_Table == "Cutting Receipt Entry");
            if (resultStk != null)
            {
                resultStk.UDT_DATE = DateTime.Now;
                resultStk.UDT_UID = userManager.GetUserName(HttpContext.User);
                resultStk.Stk_Qty_IN = cuttingReceiptViewModel.cutting_Receipt.RECEIPT_QTY;
                resultStk.GDW_CODE = cuttingReceiptViewModel.cutting_Receipt.GDW_CODE;
                dbContext.SaveChanges();
            }
            return RedirectToAction("CuttingReceiptDetail");
        }
        [HttpGet]
        public IActionResult ActionCR(int id)
        {
            CuttingReceiptViewModel cuttingReceiptViewModel = new CuttingReceiptViewModel();
            cuttingReceiptViewModel.cutting_Receipt = dbContext.Cutting_Receipt.Where(r => r.ID == id).FirstOrDefault();
            cuttingReceiptViewModel.Type = "Action";
            cuttingReceiptViewModel.GDWDropDown = GDWlists();
            return View("Cut_Recpt_Master", cuttingReceiptViewModel);
        }
        [HttpGet]
        public IActionResult DeleteCR(int ID)
        {
            var CRdata = dbContext.Cutting_Receipt.Where(D => D.ID == ID).FirstOrDefault();
            dbContext.Cutting_Receipt.Remove(CRdata);
            dbContext.SaveChanges();
            return RedirectToAction("CuttingReceiptDetail");
        }
    }
}