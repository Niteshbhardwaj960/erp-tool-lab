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
    [Authorize(Roles = "Payments , Admin")]
    public class PaymentsController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public PaymentsController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult Payments_Master()
        {
            Payments payments = new Payments();
            payments.Type = "Add";
            payments.DOC_DATE = DateTime.Now;
            payments.PAY_DOC_DATE = DateTime.Now;
            payments.DOC_FN_YEAR = GetFinYear();
            payments.ACCDropDown = Acclists();
            payments.CBACCDropDown = CBAcclists("4");
            return View(payments);
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
        [HttpPost]
        public IActionResult Payments_Master(Payments payments)
        {
            if (payments.PAYMENT_MODE == 1)
            {
                ModelState.AddModelError("PAYMENT_MODE", "Please select some value");
            }
            if (payments.CB_ACC_CODE == null || payments.CB_ACC_CODE == 0)
            {
                ModelState.AddModelError("CB_ACC_CODE", "Please select some value");
            }
            if (ModelState.IsValid)
            { 
            int Doc_Number = dbContext.Payments
                .Where(x => x.DOC_FN_YEAR == payments.DOC_FN_YEAR)
                .Select(p => Convert.ToInt32(p.DOC_NO)).DefaultIfEmpty(0).Max();
            
            payments.DOC_NO = Doc_Number + 1;
            payments.INS_DATE = DateTime.Now;
            payments.INS_UID = userManager.GetUserName(HttpContext.User);
            dbContext.Payments.Add(payments);
            dbContext.SaveChanges();
            
            return RedirectToAction("Payments_Details");
            }
            else
            {
                payments.Type = "Add";
                payments.CBACCDropDown = CBAcclists(payments.PAYMENT_MODE.ToString());
                payments.DOC_DATE = DateTime.Now;
                payments.PAY_DOC_DATE = DateTime.Now;
                payments.DOC_FN_YEAR = GetFinYear();
                payments.ACCDropDown = Acclists();
                return View(payments);
            }
        }
        [HttpGet]
        public IActionResult Payments_Details()
        {
            List<Payments> payments = new List<Payments>();
            payments = dbContext.Payments.AsNoTracking().ToList();
            foreach (var pay in payments)
            {
                if (pay.PAYMENT_MODE == 4)
                {
                    pay.PAY_MODE = "Bank";
                }
                else if(pay.PAYMENT_MODE == 5)
                {
                    pay.PAY_MODE = "Cash";
                }
                if (pay.PAYMENT_TAG == 1)
                {
                    pay.PAY_TAG = "Payment";
                }
                else
                {
                    pay.PAY_TAG = "Receipt";
                }
                pay.ACC_NAME = dbContext.Account_Masters.Where(a => a.ID == pay.ACC_CODE).Select(aa => aa.NAME).FirstOrDefault();
                pay.CB_ACC_NAME = dbContext.Account_Masters.Where(a => a.ID == pay.CB_ACC_CODE).Select(aa => aa.NAME).FirstOrDefault();
            }
            return View(payments);
        }
        public List<SelectListItem> Acclists()
        {
            var AccList = (from Acc in dbContext.Account_Masters.ToList()
                           select new SelectListItem()
                           {
                               Text = Acc.NAME,
                               Value = Acc.ID.ToString(),
                           }).ToList();

            AccList.Insert(0, new SelectListItem()
            {
                Text = "Select Account",
                Value = string.Empty,
                Selected = true
            });
            return AccList;
        }
        public List<SelectListItem> CBAcclists(string type)
        {
            var CBAccList = (from Acc in dbContext.Account_Masters.Where(C => C.ACC_TYPE== type).ToList()
                           select new SelectListItem()
                           {
                               Text = Acc.NAME,
                               Value = Acc.ID.ToString(),
                           }).ToList();

            CBAccList.Insert(0, new SelectListItem()
            {
                Text = "Select CB Account",
                Value = string.Empty,
                Selected = true
            });
            return CBAccList;
        }

        [HttpGet]
        public IActionResult ActionPayments(int id)
        {
            Payments payments = new Payments();
            payments = dbContext.Payments.Find(id);
            payments.ACCDropDown = Acclists();
            payments.CBACCDropDown = CBAcclists(payments.PAYMENT_MODE.ToString());
            payments.Type = "Action";
            dbContext.Payments.Update(payments);
            dbContext.SaveChanges();
            return View("Payments_Master", payments);
        }
        [HttpGet]
        public IActionResult EditPayments(int id)
        {
            Payments payments = new Payments();
            payments = dbContext.Payments.Find(id);
            payments.Type = "Edit";
            payments.ACCDropDown = Acclists();
            payments.CBACCDropDown = CBAcclists(payments.PAYMENT_MODE.ToString());
            dbContext.Payments.Update(payments);
            dbContext.SaveChanges();
            return View("Payments_Master", payments);
        }

        [HttpPost]
        public IActionResult EditPayments(Payments payments)
        {
            if (ModelState.IsValid)
            {
                var result = dbContext.Payments.SingleOrDefault(b => b.ID == payments.ID);
                if (result != null)
                {
                    result.UDT_DATE = DateTime.Now;
                    result.UDT_UID = userManager.GetUserName(HttpContext.User);
                    result.CB_ACC_CODE = payments.CB_ACC_CODE;
                    result.ACC_CODE = payments.ACC_CODE;
                    result.AMOUNT = payments.AMOUNT;
                    result.PAYMENT_MODE = payments.PAYMENT_MODE;
                    result.PAYMENT_TAG = payments.PAYMENT_TAG;
                    result.PAY_DOC_DATE = payments.PAY_DOC_DATE;
                    result.PAY_DOC_NO = payments.PAY_DOC_NO;
                    result.REMARKS = payments.REMARKS;
                    dbContext.SaveChanges();
                }              
                return RedirectToAction("Payments_Details");
            }
            else
            {
                payments.Type = "Edit";
                payments.ACCDropDown = Acclists();
                payments.CBACCDropDown = CBAcclists(payments.PAYMENT_MODE.ToString());
                return View("Payments_Master", payments);
            }
        }
        [HttpGet]
        public IActionResult DeletePayments(int ID)
        {
            var data = dbContext.Payments.Find(ID);
            dbContext.Payments.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Payments_Details");
        }
    }
}