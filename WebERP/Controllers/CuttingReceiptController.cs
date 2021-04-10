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
            cuttingReceiptViewModel.DOc_Dates = DateTime.Now;
            cuttingReceiptViewModel.Fin_Years = GetFinYear();
            return View(cuttingReceiptViewModel);
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
            return View("Cut_Recpt_Master", cuttingReceiptViewModel);
        }
        [HttpPost]
        public IActionResult SAVECR(CuttingReceiptViewModel cuttingReceiptViewModel)
        {
            var result = dbContext.Cutting_Receipt.SingleOrDefault(b => b.ID == cuttingReceiptViewModel.cutting_Receipt.ID);
            if (result != null)
            {
                cuttingReceiptViewModel.cutting_Receipt.UDT_DATE = DateTime.Now;
                cuttingReceiptViewModel.cutting_Receipt.UDT_UID = userManager.GetUserName(HttpContext.User);
                result.RECEIPT_QTY = cuttingReceiptViewModel.cutting_Receipt.RECEIPT_QTY;
                dbContext.SaveChanges();
            }
            return RedirectToAction("CuttingReceiptDetail");
        }
        [HttpGet]
        public IActionResult ActionCR(int id)
        {
            CuttingReceiptViewModel cuttingReceiptViewModel = new CuttingReceiptViewModel();
            cuttingReceiptViewModel.cutting_Receipt = dbContext.Cutting_Receipt.Where(r => r.ID == id).FirstOrDefault();
            cuttingReceiptViewModel.Type = "View";
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