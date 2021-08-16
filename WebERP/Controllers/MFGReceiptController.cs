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
        public IActionResult MFG_Receipt_Master()
        {
            MFGReceiptViewModel mFGReceiptViewModel = new MFGReceiptViewModel();
            mFGReceiptViewModel.Type = "Add";
            mFGReceiptViewModel.CUTDropDown = CUTlists();
            mFGReceiptViewModel.EMPDropDown = EMPlists();
            mFGReceiptViewModel.DOC_DATES = DateTime.Now;
            mFGReceiptViewModel.DOC_FINYEARS = GetFinYear();
            mFGReceiptViewModel.CONEMPDropDown = CoEmplists();
            mFGReceiptViewModel.PROCDropDown = PROClists();
            return View(mFGReceiptViewModel);
        }
        public List<SelectListItem> CUTlists()
        {
            var CutList = (from Cut in dbContext.V_CuttingDetail.Where(e => e.ORDER_STATUS == "1").ToList()
                           select new SelectListItem()
                           {
                               Text = Cut.DOC_NO.ToString() + '/' + Cut.EMP_NAME + '/' + Cut.Item_NAME + '/' + Cut.ARTICAL_NAME + '/' + Cut.SIZE_NAME + '/' + Cut.PROC_NAME,
                               Value = Cut.EMP_NAME + '^' + Cut.Item_NAME + '^' + Cut.ARTICAL_NAME + '^' + Cut.SIZE_NAME + '^' + Cut.PROC_NAME + '^' + Cut.ID + '^' + Cut.DOC_NO.ToString() + '^' + Cut.ORDER_QTY,
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
                               Text = emp.EMP_NAME + " / " + emp.EMP_CODE,
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
        public List<SelectListItem> PROClists()
        {
            var PROCList = (from PROC in dbContext.Process_Master.Where(aa => aa.NAME.ToUpper() != "CUTTING").ToList()
                           select new SelectListItem()
                           {
                               Text = PROC.NAME,
                               Value = PROC.ID.ToString(),
                           }).ToList();
            PROCList.Insert(0, new SelectListItem()
            {
                Text = "Select Process",
                Value = string.Empty,
                Selected = true
            });
            return PROCList;
        }
        public List<SelectListItem> CoEmplists()
        {
            var CutList = (from emp in dbContext.Employee_Masters.Where(e => e.EMP_TYPE == "C").ToList()
                           select new SelectListItem()
                           {
                               Text = emp.EMP_NAME + " / " + emp.EMP_CODE,
                               Value = emp.EMP_CODE.ToString(),
                           }).ToList();

            CutList.Insert(0, new SelectListItem()
            {
                Text = "NA",
                Value = "0",
                Selected = true
            });
            return CutList;
        }
        [HttpGet]
        public IActionResult MFGReceiptDetail()
        {
            List<MGF_RECEIPT> mGF_RECEIPTs = new List<MGF_RECEIPT>();
            mGF_RECEIPTs = dbContext.MGF_RECEIPT.AsNoTracking().ToList();
            foreach(var mGF in mGF_RECEIPTs)
            {
                mGF.PROC_NAME = dbContext.Process_Master.Where(m => m.ID == mGF.PROC_CODE).Select(mm => mm.NAME).FirstOrDefault();
            }
            return View("MFG_Details", mGF_RECEIPTs);
        }
        [HttpPost]
        public IActionResult MFG_Receipt_Master(MFGReceiptViewModel mFGReceiptViewModel, int EMP_CODE, int CONT_EMP_CODE, int PROC_CODE)
        {
            if (mFGReceiptViewModel.MGF_RECEIPTs.RECEIPT_QTY <= 0 || mFGReceiptViewModel.MGF_RECEIPTs.RECEIPT_QTY == null)
            {
                mFGReceiptViewModel.error = "Value Receipt Qty should be greater than 0";
            }
            if (mFGReceiptViewModel.MGF_RECEIPTs.RECEIPT_QTY > mFGReceiptViewModel.MGF_RECEIPTs.ORDER_QTY)
            {
                mFGReceiptViewModel.errorOrder = "Recipt Qty can not be greater than cutting Order Qty";
            }
            if (mFGReceiptViewModel.error == null && mFGReceiptViewModel.errorOrder == null)
            {
                var emps = dbContext.Employee_Masters.Where(e => e.EMP_CODE == EMP_CODE).Select(ee => ee.EMP_NAME).FirstOrDefault();
                //var cemps = dbContext.Employee_Masters.Where(e => e.EMP_CODE == mFGReceiptViewModel.MGF_RECEIPTs.CONT_EMP_CODE).Select(ee => ee.EMP_NAME).FirstOrDefault();
                int Doc_Number = dbContext.MGF_RECEIPT
                    .Where(x => x.DOC_FINYEAR == mFGReceiptViewModel.DOC_FINYEARS)
                    .Select(p => Convert.ToInt32(p.DOC_NO)).DefaultIfEmpty(0).Max();
                mFGReceiptViewModel.MGF_RECEIPTs.DOC_NO = Doc_Number + 1;
                mFGReceiptViewModel.MGF_RECEIPTs.INS_DATE = DateTime.Now;
                mFGReceiptViewModel.MGF_RECEIPTs.EMP_CODE = EMP_CODE;
                mFGReceiptViewModel.MGF_RECEIPTs.CONT_EMP_CODE = CONT_EMP_CODE;
                mFGReceiptViewModel.MGF_RECEIPTs.EMP_NAME = emps;
                mFGReceiptViewModel.MGF_RECEIPTs.DOC_DATE = mFGReceiptViewModel.DOC_DATES;
                mFGReceiptViewModel.MGF_RECEIPTs.DOC_FINYEAR = mFGReceiptViewModel.DOC_FINYEARS;
                mFGReceiptViewModel.MGF_RECEIPTs.INS_UID = userManager.GetUserName(HttpContext.User);
                mFGReceiptViewModel.MGF_RECEIPTs.PROC_CODE = PROC_CODE;
                dbContext.MGF_RECEIPT.Add(mFGReceiptViewModel.MGF_RECEIPTs);
                dbContext.SaveChanges();

                if(dbContext.Process_Master.Where(p => p.ID == PROC_CODE).Select(m => m.NAME).FirstOrDefault() == "Packing")
                {
                    StockDTL_Model StkDTL = new StockDTL_Model();
                    var gdwcode = dbContext.Godown_Master.Where(g => g.NAME == "Ready Stock").Select(m => m.ID).FirstOrDefault();
                    StkDTL.INS_DATE = DateTime.Now;
                    StkDTL.INS_UID = userManager.GetUserName(HttpContext.User);
                    StkDTL.COMP_CODE = 0;
                    StkDTL.Tran_Table = "Process Receipt Entry";
                    StkDTL.Tran_Table_PK = mFGReceiptViewModel.MGF_RECEIPTs.ID;
                    StkDTL.GDW_CODE = gdwcode;
                    StkDTL.Item_Code = dbContext.Item_Master.Where(i => i.NAME == mFGReceiptViewModel.MGF_RECEIPTs.ITEM_NAME).Select(n => n.ID).FirstOrDefault();
                    StkDTL.Artical_CODE = dbContext.Artical_Master.Where(ar => ar.NAME == mFGReceiptViewModel.MGF_RECEIPTs.ART_NAME).Select(na => na.ID).FirstOrDefault();
                    StkDTL.Size_Code = dbContext.Size_Master.Where(s => s.NAME == mFGReceiptViewModel.MGF_RECEIPTs.SIZE_NAME).Select(sn => sn.ID).FirstOrDefault();
                    StkDTL.Stk_Qty_IN = mFGReceiptViewModel.MGF_RECEIPTs.RECEIPT_QTY;
                    dbContext.StockDTL_Models.Add(StkDTL);
                    dbContext.SaveChanges();
                }
                return RedirectToAction("MFGReceiptDetail");
            }
            else
            {
                mFGReceiptViewModel.Type = "Add";
                mFGReceiptViewModel.CUTDropDown = CUTlists();
                mFGReceiptViewModel.EMPDropDown = EMPlists();
                mFGReceiptViewModel.CONEMPDropDown = CoEmplists();
                mFGReceiptViewModel.PROCDropDown = PROClists();
                return View(mFGReceiptViewModel);
            }
        }
        [HttpGet]
        public IActionResult EditMFG(int id)
        {
            MFGReceiptViewModel mFGReceiptViewModel = new MFGReceiptViewModel();
            mFGReceiptViewModel.MGF_RECEIPTs = dbContext.MGF_RECEIPT.Where(r => r.ID == id).FirstOrDefault();
            mFGReceiptViewModel.Type = "Edit";
            mFGReceiptViewModel.CONEMPDropDown = CoEmplists();
            mFGReceiptViewModel.EMPDropDown = EMPlists();
            mFGReceiptViewModel.PROCDropDown = PROClists();
            return View("MFG_Receipt_Master", mFGReceiptViewModel);
        }
        [HttpPost]
        public IActionResult SAVEMFG(MFGReceiptViewModel mFGReceiptViewModel,int EMP_CODE,int CONT_EMP_CODE,int PROC_CODE)
        {
            var order_Qty = dbContext.Cutting_Orders.Where(c => c.DOC_NO == Convert.ToInt32(mFGReceiptViewModel.MGF_RECEIPTs.CUT_DOC_NO)).Select(l => l.ORDER_QTY).FirstOrDefault();
            if (mFGReceiptViewModel.MGF_RECEIPTs.RECEIPT_QTY <= 0 || mFGReceiptViewModel.MGF_RECEIPTs.RECEIPT_QTY == null)
            {
                mFGReceiptViewModel.error = "Value Receipt Qty should be greater than 0";
            }
            if (mFGReceiptViewModel.MGF_RECEIPTs.RECEIPT_QTY > order_Qty)
            {
                mFGReceiptViewModel.errorOrder = "Recipt Qty can not be greater than cutting Order Qty";
            }
            if (mFGReceiptViewModel.error == null && mFGReceiptViewModel.errorOrder == null)
            {
                var result = dbContext.MGF_RECEIPT.SingleOrDefault(b => b.ID == mFGReceiptViewModel.MGF_RECEIPTs.ID);
                var emps = dbContext.Employee_Masters.Where(e => e.EMP_CODE == EMP_CODE).Select(ee => ee.EMP_NAME).FirstOrDefault();

                if (result != null)
                {
                    result.UDT_DATE = DateTime.Now;
                    result.UDT_UID = userManager.GetUserName(HttpContext.User);
                    result.EMP_CODE = EMP_CODE;
                    result.CONT_EMP_CODE = CONT_EMP_CODE;
                    result.PROC_CODE = PROC_CODE;
                    result.EMP_NAME = emps;
                    result.CUT_DOC_NO = mFGReceiptViewModel.MGF_RECEIPTs.CUT_DOC_NO;
                    result.RECEIPT_QTY = mFGReceiptViewModel.MGF_RECEIPTs.RECEIPT_QTY;
                    dbContext.SaveChanges();
                }
                return RedirectToAction("MFGReceiptDetail");
            }
            else
            {
                mFGReceiptViewModel.Type = "Edit";
                mFGReceiptViewModel.CONEMPDropDown = CoEmplists();
                mFGReceiptViewModel.EMPDropDown = EMPlists();
                mFGReceiptViewModel.PROCDropDown = PROClists();
                return View("MFG_Receipt_Master", mFGReceiptViewModel);
            }
        }
        [HttpGet]
        public IActionResult ActionMFG(int id)
        {
            MFGReceiptViewModel mFGReceiptViewModel = new MFGReceiptViewModel();
            mFGReceiptViewModel.MGF_RECEIPTs = dbContext.MGF_RECEIPT.Where(r => r.ID == id).FirstOrDefault();
            mFGReceiptViewModel.Type = "Action";
            mFGReceiptViewModel.CONEMPDropDown = CoEmplists();
            mFGReceiptViewModel.EMPDropDown = EMPlists();
            mFGReceiptViewModel.PROCDropDown = PROClists();
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