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
    [Authorize(Roles = "MFG , Admin")]
    public class MFGReceiptController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public MFGReceiptController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult MFG_Receipt_Master()
        {
            MFGReceiptViewModel mFGReceiptViewModel = new MFGReceiptViewModel();
            mFGReceiptViewModel.Type = "Add";
            mFGReceiptViewModel.CUTDropDown = CUTlists();
            mFGReceiptViewModel.EMPDropDown = EMPlists();
            mFGReceiptViewModel.CONEMPDropDown = CoEmplists();
            return View(mFGReceiptViewModel);
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
        public List<SelectListItem> EMPlists()
        {
            var empList = (from emp in dbContext.Employee_Masters.Where(e => e.EMP_TYPE == "S").ToList()
                           select new SelectListItem()
                           {
                               Text = emp.EMP_NAME,
                               Value = emp.EMP_CODE.ToString(),
                           }).ToList();

            empList.Insert(0, new SelectListItem()
            {
                Text = "Select Employee",
                Value = string.Empty,
                Selected = true
            });
            return empList;
        }
        public List<SelectListItem> CoEmplists()
        {
            var CutList = (from emp in dbContext.Employee_Masters.Where(e => e.EMP_TYPE == "C").ToList()
                           select new SelectListItem()
                           {
                               Text = emp.EMP_NAME,
                               Value = emp.EMP_CODE.ToString(),
                           }).ToList();

            CutList.Insert(0, new SelectListItem()
            {
                Text = "Select Contract Employee",
                Value = string.Empty,
                Selected = true
            });
            return CutList;
        }
        [HttpGet]
        public IActionResult MFGReceiptDetail()
        {
            List<MGF_RECEIPT> mGF_RECEIPTs = new List<MGF_RECEIPT>();
            mGF_RECEIPTs = dbContext.MGF_RECEIPT.AsNoTracking().ToList();
            return View("MFG_Details", mGF_RECEIPTs);
        }
        [HttpPost]
        public IActionResult MFG_Receipt_Master(MFGReceiptViewModel mFGReceiptViewModel, string EMPDropDown, string CONEMPDropDown)
        {
            int Doc_Number = dbContext.MGF_RECEIPT
                .Where(x => x.DOC_FINYEAR == mFGReceiptViewModel.MGF_RECEIPTs.DOC_FINYEAR)
                .Select(p => Convert.ToInt32(p.DOC_NO)).DefaultIfEmpty(0).Max();
            mFGReceiptViewModel.MGF_RECEIPTs.DOC_NO = Doc_Number + 1;
            mFGReceiptViewModel.MGF_RECEIPTs.INS_DATE = DateTime.Now;
            mFGReceiptViewModel.MGF_RECEIPTs.EMP_CODE = Convert.ToInt32(EMPDropDown);
            mFGReceiptViewModel.MGF_RECEIPTs.CONT_EMP_CODE = Convert.ToInt32(CONEMPDropDown);
            mFGReceiptViewModel.MGF_RECEIPTs.INS_UID = userManager.GetUserName(HttpContext.User);
            dbContext.MGF_RECEIPT.Add(mFGReceiptViewModel.MGF_RECEIPTs);
            dbContext.SaveChanges();
            return RedirectToAction("MFGReceiptDetail");
        }
        [HttpGet]
        public IActionResult EditMFG(int id)
        {
            MFGReceiptViewModel mFGReceiptViewModel = new MFGReceiptViewModel();
            mFGReceiptViewModel.MGF_RECEIPTs = dbContext.MGF_RECEIPT.Where(r => r.ID == id).FirstOrDefault();
            mFGReceiptViewModel.Type = "Edit";
            mFGReceiptViewModel.CONEMPDropDown = CoEmplists();
            mFGReceiptViewModel.EMPDropDown = EMPlists();
            return View("MFG_Receipt_Master", mFGReceiptViewModel);
        }
        [HttpPost]
        public IActionResult SAVEMFG(MFGReceiptViewModel mFGReceiptViewModel)
        {
            var result = dbContext.MGF_RECEIPT.SingleOrDefault(b => b.ID == mFGReceiptViewModel.MGF_RECEIPTs.ID);
            if (result != null)
            {
                result.UDT_DATE = DateTime.Now;
                result.UDT_UID = userManager.GetUserName(HttpContext.User);
                result.EMP_CODE = mFGReceiptViewModel.MGF_RECEIPTs.EMP_CODE;
                result.CONT_EMP_CODE = mFGReceiptViewModel.MGF_RECEIPTs.CONT_EMP_CODE;
                result.RECEIPT_QTY = mFGReceiptViewModel.MGF_RECEIPTs.RECEIPT_QTY;
                dbContext.SaveChanges();
            }
            return RedirectToAction("MFGReceiptDetail");
        }
        [HttpGet]
        public IActionResult ActionMFG(int id)
        {
            MFGReceiptViewModel mFGReceiptViewModel = new MFGReceiptViewModel();
            mFGReceiptViewModel.MGF_RECEIPTs = dbContext.MGF_RECEIPT.Where(r => r.ID == id).FirstOrDefault();
            mFGReceiptViewModel.Type = "View";
            mFGReceiptViewModel.CONEMPDropDown = CoEmplists();
            mFGReceiptViewModel.EMPDropDown = EMPlists();
            return View("MFG_Receipt_Master", mFGReceiptViewModel);
        }
        [HttpGet]
        public IActionResult DeleteMFG(int ID)
        {
            var data = dbContext.MGF_RECEIPT.Where(D => D.ID == ID).FirstOrDefault();
            dbContext.MGF_RECEIPT.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("MFGReceiptDetail");
        }
    }
}