using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    public class AccountController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public AccountController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Account_Master()
        {
            return View(dbContext.Account_Masters.ToList());
        }
        [HttpGet]
        public IActionResult AddAccount()
        {
            var countryList = (from country in dbContext.Countries
                               select new SelectListItem()
                               {
                                   Text = country.Name,
                                   Value = country.CountryCode.ToString(),
                               }).ToList();

            countryList.Insert(0, new SelectListItem()
            {
                Text = "Select Country",
                Value = string.Empty,
                Selected=true
            });
            var StateList = (from state in dbContext.States
                             select new SelectListItem()
                             {
                                 Text = state.Name,
                                 Value = state.StateCode.ToString(),
                             }).ToList();

            StateList.Insert(0, new SelectListItem()
            {
                Text = "Select State",
                Value = string.Empty,
                Selected = true
            });
            var cityList = (from city in dbContext.Cities
                            select new SelectListItem()
                            {
                                Text = city.Name,
                                Value = Convert.ToString(city.Id)
                            }).ToList();

            cityList.Insert(0, new SelectListItem()
            {
                Text = "Select City",
                Value = string.Empty,
                Selected = true
            });
            Account_Master am = new Account_Master();
            am.countryDropDown = countryList;
            am.stateDropDown = StateList;
            am.cityDropDown = cityList;
            return View(am);
        }

        public ActionResult Getstatelist(int cid)
        {
            var StateList = (from state in dbContext.States
                             select new SelectListItem()
                             {
                                 Text = state.Name,
                                 Value = state.StateCode.ToString(),
                             }).ToList();

            StateList.Insert(0, new SelectListItem()
            {
                Text = "Select State",
                Value = string.Empty,
                Selected = true
            });
            return View(StateList);
        }

        [HttpPost]
        public async Task<IActionResult> SAVEAccount(Account_Master objAccount)
        {
            if (ModelState.IsValid)
            {
                objAccount.INS_DATE = DateTime.Now;
                objAccount.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Account_Masters.Add(objAccount);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Account_Master");
            }
            else
            {
                return View("AddAccount");
            }
        }
        [HttpGet]
        public IActionResult ActionAccount(int id)
        {

            Account_Master objAccount = new Account_Master();
            objAccount = dbContext.Account_Masters.Find(id);
            objAccount.Type = "Action";
            dbContext.Account_Masters.Update(objAccount);
            dbContext.SaveChanges();
            return View("EditAccount",objAccount);
        }
        [HttpGet]
        public IActionResult EditAccount(int id)
        {
            Account_Master objAccount = new Account_Master();
            objAccount = dbContext.Account_Masters.Find(id);
            objAccount.Type = "Edit";
            dbContext.Account_Masters.Update(objAccount);
            dbContext.SaveChanges();
            return View(objAccount);
        }

        [HttpPost]
        public IActionResult EditAccount(Account_Master objAccount)
        {
            objAccount.UDT_DATE = DateTime.Now;
            objAccount.UDT_UID = userManager.GetUserName(HttpContext.User);
            dbContext.Account_Masters.Update(objAccount);
            dbContext.SaveChanges();
            return RedirectToAction("Account_Master");

        }
        [HttpGet]
        public IActionResult DeleteAccount(int ID)
        {
            var data = dbContext.Account_Masters.Find(ID);
            dbContext.Account_Masters.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Account_Master");
        }

        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Account_Masters;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-Accounts");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "NAME";
                worksheet.Cell(currentRow, 3).Value = "ADDRESS 1";
                worksheet.Cell(currentRow, 4).Value = "ADDRESS 2";
                worksheet.Cell(currentRow, 5).Value = "CITY CODE";
                worksheet.Cell(currentRow, 6).Value = "PIN CODE";
                worksheet.Cell(currentRow, 7).Value = "MOBILE NO";
                worksheet.Cell(currentRow, 9).Value = "EMAIL ID";
                worksheet.Cell(currentRow, 10).Value = "PH NO";
                worksheet.Cell(currentRow, 11).Value = "GST NO";
                worksheet.Cell(currentRow, 12).Value = "GST REGD TAG"; 
                worksheet.Cell(currentRow, 13).Value = "ACTIVE TAG";
                worksheet.Cell(currentRow, 14).Value = "REMARKS";
                worksheet.Cell(currentRow, 15).Value = "OPENING BAL";
                worksheet.Cell(currentRow, 16).Value = "OPENING BAL TAG";
                worksheet.Cell(currentRow, 17).Value = "ACCOUNT TYPE";
                worksheet.Cell(currentRow, 18).Value = "CREDIT LIMIT";
                worksheet.Cell(currentRow, 19).Value = "CREDIT DAYS"; 
                worksheet.Cell(currentRow, 20).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 21).Value = "INSERT UID";
                worksheet.Cell(currentRow, 22).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 23).Value = "UPDATE UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.NAME;
                    worksheet.Cell(currentRow, 3).Value = Data.ADD1;
                    worksheet.Cell(currentRow, 4).Value = Data.ADD2;
                    worksheet.Cell(currentRow, 5).Value = Data.CITY_CODE;
                    worksheet.Cell(currentRow, 6).Value = Data.PIN_CODE;
                    worksheet.Cell(currentRow, 7).Value = Data.MOBILE_NO;
                    worksheet.Cell(currentRow, 9).Value = Data.EMAIL_ID;
                    worksheet.Cell(currentRow, 10).Value = Data.PH_NO;
                    worksheet.Cell(currentRow, 11).Value = Data.GST_NO;
                    worksheet.Cell(currentRow, 12).Value = Data.GST_REGD_TAG;
                    worksheet.Cell(currentRow, 13).Value = Data.ACTIVE_TAG;
                    worksheet.Cell(currentRow, 14).Value = Data.REMARKS;
                    worksheet.Cell(currentRow, 15).Value = Data.OP_BAL;
                    worksheet.Cell(currentRow, 16).Value = Data.OP_BAL_TAG;
                    worksheet.Cell(currentRow, 17).Value = Data.ACC_TYPE;
                    worksheet.Cell(currentRow, 18).Value = Data.CR_LIMIT;
                    worksheet.Cell(currentRow, 19).Value = Data.CR_DAYS;
                    worksheet.Cell(currentRow, 20).Value = Data.INS_DATE;
                    worksheet.Cell(currentRow, 21).Value = Data.INS_UID;
                    worksheet.Cell(currentRow, 22).Value = Data.UDT_DATE;
                    worksheet.Cell(currentRow, 23).Value = Data.UDT_UID;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Accounts.xlsx");
                }
            }
        }
    }
}