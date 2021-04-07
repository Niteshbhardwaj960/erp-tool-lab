using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Account , Admin")]
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
            Account_Master am = new Account_Master();
            am.cityDropDown = Citylists();
            return View(am);
        }

        public JsonResult Getstatelist(string ctid)
        {
            CityModel City = new CityModel();
            City = dbContext.Cities.Find(Convert.ToInt32(ctid));
            var Stateid = City.StateId;

            StateModel St = new StateModel();
            St = dbContext.States.Find(Convert.ToInt32(Stateid));
            var Countryid = St.CountryId;

            CountryModel CtModel = new CountryModel();
            CtModel = dbContext.Countries.Find(Convert.ToInt32(Countryid));
            var Countyname = CtModel.Name;

            var StateList = (from state in dbContext.States.Where(x => x.Id == Convert.ToInt32(Stateid)).ToList()
                             select new SelectListItem()
                             {
                                 Text = state.Name + '/' + Countyname,
                                 Value = state.Name + '/' + Countyname
                             }).ToList();
            return Json(StateList);
        }
        public JsonResult GetCitylist(int sid)
        {
            var cityList = (from city in dbContext.Cities.Where(x => x.StateId == Convert.ToInt32(sid)).ToList()
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
            return Json(cityList);
        }
        [HttpPost]
        public async Task<IActionResult> SAVEAccount(Account_Master objAccount)
        {
            var NAME = dbContext.Account_Masters.FirstOrDefault(x => x.NAME == objAccount.NAME);

            if (NAME != null)
            {
                ModelState.AddModelError("NAME", "Name Already Exists.");
            }
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
                objAccount.cityDropDown = Citylists();
                return View("AddAccount",objAccount);
            }
        }
        [HttpGet]
        public IActionResult ActionAccount(int id)
        {

            Account_Master objAccount = new Account_Master();
            objAccount = dbContext.Account_Masters.Find(id);
            objAccount.Type = "Action";
            objAccount.stateDropDown = Statelists(objAccount.State_Code);
            objAccount.cityDropDown = Citylists();
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
            objAccount.stateDropDown = Statelists(objAccount.State_Code);
            objAccount.cityDropDown = Citylists();
            dbContext.Account_Masters.Update(objAccount);
            dbContext.SaveChanges();
            return View(objAccount);
        }

        [HttpPost]
        public IActionResult EditAccount(Account_Master objAccount)
        {
            if (ModelState.IsValid)
            {
                objAccount.UDT_DATE = DateTime.Now;
                objAccount.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Account_Masters.Update(objAccount);
                dbContext.SaveChanges();
                return RedirectToAction("Account_Master");
            }
            else
            {
                return View(objAccount);
            }
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

        public List<SelectListItem> Countrylists()
        {
            var countryList = (from country in dbContext.Countries
                               select new SelectListItem()
                               {
                                   Text = country.Name,
                                   Value = country.Id.ToString(),
                               }).ToList();

            countryList.Insert(0, new SelectListItem()
            {
                Text = "Select Country",
                Value = string.Empty,
                Selected = true
            });
            return countryList;
        }
        public List<SelectListItem> Statelists(string cid)
        {
            List<SelectListItem> StateList = new List<SelectListItem>();
            StateList.Insert(0, new SelectListItem()
            {
                Text = cid,
                Value = cid,
                Selected = true
            });
            return StateList;
        }
        public List<SelectListItem> Citylists()
        {
            var cityList = (from city in dbContext.Cities
                            select new SelectListItem()
                            {
                                Text = city.Name,
                                Value = city.Id.ToString(),
                            }).ToList();

            cityList.Insert(0, new SelectListItem()
            {
                Text = "Select City",
                Value = string.Empty,
                Selected = true
            });
            return cityList;
        }
    }
}