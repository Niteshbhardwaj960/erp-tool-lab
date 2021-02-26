using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebERP.Models;
using System.Diagnostics;
using WebERP.Data;
using Microsoft.AspNetCore.Authorization;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public CompanyController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult ActionCompany(int id)
        {
            Company objCompany = new Company();
            objCompany = dbContext.Companies.Find(id);
            objCompany.Type = "Action";
            objCompany.countryDropDown = Countrylists();
            objCompany.stateDropDown = Statelists(objCompany.Country_Code);
            objCompany.cityDropDown = Citylists(objCompany.State_Code);
            return View("EditCompany", objCompany);
        }
        [HttpGet]
        public IActionResult Company()
        {
            return View(dbContext.Companies.ToList());
        }

        [HttpGet]
        public IActionResult AddCompany()
        {
            Company objCompany = new Company();
            objCompany.countryDropDown = Countrylists();
            return View(objCompany);
        }
        [HttpPost]
        public async Task<IActionResult> SAVECompany(Company objCompany)
        {
            if (ModelState.IsValid)
            {
                objCompany.INS_DATE = DateTime.Now;
                objCompany.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Companies.Add(objCompany);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Company");
            }
            else
            {
                return View("AddCompany");
            }
        }

        [HttpGet]
        public IActionResult EditCompany(int id)
        {
            Company objCompany = new Company();
            objCompany = dbContext.Companies.Find(id);
            objCompany.Type = "Edit";
            objCompany.countryDropDown = Countrylists();
            objCompany.stateDropDown = Statelists(objCompany.Country_Code);
            objCompany.cityDropDown = Citylists(objCompany.State_Code);
            return View("EditCompany", objCompany);
        }

        [HttpPost]
        public IActionResult EditCompany(Company objCompany)
        {
            objCompany.UDT_DATE = DateTime.Now;
            objCompany.UDT_UID = userManager.GetUserName(HttpContext.User);
            dbContext.Companies.Update(objCompany);
            dbContext.SaveChanges();
            return RedirectToAction("Company");

        }

        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Companies;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-Companies");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "NAME";
                worksheet.Cell(currentRow, 2).Value = "ABV";
                worksheet.Cell(currentRow, 3).Value = "ADD1";
                worksheet.Cell(currentRow, 4).Value = "ADD2";
                worksheet.Cell(currentRow, 5).Value = "CITY_CODE";
                worksheet.Cell(currentRow, 6).Value = "PIN_CODE";
                worksheet.Cell(currentRow, 7).Value = "MOBILE_NO";
                worksheet.Cell(currentRow, 8).Value = "URL";
                worksheet.Cell(currentRow, 9).Value = "EMAIL_ID";
                worksheet.Cell(currentRow, 10).Value = "PH_NO";
                worksheet.Cell(currentRow, 11).Value = "FAX_NO";
                worksheet.Cell(currentRow, 12).Value = "LST_NO";
                worksheet.Cell(currentRow, 13).Value = "LST_DATE";
                worksheet.Cell(currentRow, 14).Value = "CST_NO";
                worksheet.Cell(currentRow, 15).Value = "CST_DAT";
                worksheet.Cell(currentRow, 16).Value = "TIN_NO";
                worksheet.Cell(currentRow, 17).Value = "GST_NO";
                worksheet.Cell(currentRow, 18).Value = "ECC_NO";
                worksheet.Cell(currentRow, 19).Value = "SERVICE_TAX_NO";
                worksheet.Cell(currentRow, 20).Value = "PAN_NO";
                worksheet.Cell(currentRow, 21).Value = "IFSC_CODE";
                worksheet.Cell(currentRow, 22).Value = "TDS_NO";
                worksheet.Cell(currentRow, 23).Value = "ESI_NO";
                worksheet.Cell(currentRow, 24).Value = "PF_NO";
                worksheet.Cell(currentRow, 25).Value = "MSME_NO";
                worksheet.Cell(currentRow, 26).Value = "LOGO_NAME";
                worksheet.Cell(currentRow, 27).Value = "BANK_NAME";
                worksheet.Cell(currentRow, 28).Value = "BANK_ACC_NO";
                worksheet.Cell(currentRow, 29).Value = "BANK_BRANCH";
                worksheet.Cell(currentRow, 30).Value = "ACTIVE_TAG";
                worksheet.Cell(currentRow, 31).Value = "REMARKS";
                worksheet.Cell(currentRow, 32).Value = "INS_DATE";
                worksheet.Cell(currentRow, 33).Value = "INS_UID";
                worksheet.Cell(currentRow, 34).Value = "UDT_DATE";
                worksheet.Cell(currentRow, 35).Value = "UDT_UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.NAME;
                    worksheet.Cell(currentRow, 2).Value = Data.ABV;
                    worksheet.Cell(currentRow, 3).Value = Data.ADD1;
                    worksheet.Cell(currentRow, 4).Value = Data.ADD2;
                    worksheet.Cell(currentRow, 5).Value = Data.CITY_CODE;
                    worksheet.Cell(currentRow, 6).Value = Data.PIN_CODE;
                    worksheet.Cell(currentRow, 7).Value = Data.MOBILE_NO;
                    worksheet.Cell(currentRow, 8).Value = Data.URL;
                    worksheet.Cell(currentRow, 9).Value = Data.EMAIL_ID;
                    worksheet.Cell(currentRow, 10).Value = Data.PH_NO;
                    worksheet.Cell(currentRow, 11).Value = Data.FAX_NO;
                    worksheet.Cell(currentRow, 12).Value = Data.LST_NO;
                    worksheet.Cell(currentRow, 13).Value = Data.LST_DATE;
                    worksheet.Cell(currentRow, 14).Value = Data.CST_NO;
                    worksheet.Cell(currentRow, 15).Value = Data.CST_DAT;
                    worksheet.Cell(currentRow, 16).Value = Data.TIN_NO;
                    worksheet.Cell(currentRow, 17).Value = Data.GST_NO;
                    worksheet.Cell(currentRow, 18).Value = Data.ECC_NO;
                    worksheet.Cell(currentRow, 19).Value = Data.SERVICE_TAX_NO;
                    worksheet.Cell(currentRow, 20).Value = Data.PAN_NO;
                    worksheet.Cell(currentRow, 21).Value = Data.IFSC_CODE;
                    worksheet.Cell(currentRow, 22).Value = Data.TDS_NO;
                    worksheet.Cell(currentRow, 23).Value = Data.ESI_NO;
                    worksheet.Cell(currentRow, 24).Value = Data.PF_NO;
                    worksheet.Cell(currentRow, 25).Value = Data.MSME_NO;
                    worksheet.Cell(currentRow, 26).Value = Data.LOGO_NAME;
                    worksheet.Cell(currentRow, 27).Value = Data.BANK_NAME;
                    worksheet.Cell(currentRow, 28).Value = Data.BANK_ACC_NO;
                    worksheet.Cell(currentRow, 29).Value = Data.BANK_BRANCH;
                    worksheet.Cell(currentRow, 30).Value = Data.ACTIVE_TAG;
                    worksheet.Cell(currentRow, 31).Value = Data.REMARKS;
                    worksheet.Cell(currentRow, 32).Value = Data.INS_DATE;
                    worksheet.Cell(currentRow, 33).Value = Data.INS_UID;
                    worksheet.Cell(currentRow, 34).Value = Data.UDT_DATE;
                    worksheet.Cell(currentRow, 35).Value = Data.UDT_UID;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Companies.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult DeleteCompany(int ID)
        {
            var data = dbContext.Companies.Find(ID);
            dbContext.Companies.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Company");
        }
        public JsonResult Getstatelist(string cid)
        {
            var StateList = (from state in dbContext.States.Where(x => x.CountryId == Convert.ToInt32(cid)).ToList()
                             select new SelectListItem()
                             {
                                 Text = state.Name,
                                 Value = state.Id.ToString(),
                             }).ToList();

            StateList.Insert(0, new SelectListItem()
            {
                Text = "Select State",
                Value = string.Empty,
                Selected = true
            });
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
            var StateList = (from state in dbContext.States.Where(x => x.CountryId == Convert.ToInt32(cid)).ToList()
                             select new SelectListItem()
                             {
                                 Text = state.Name,
                                 Value = state.Id.ToString(),
                             }).ToList();

            StateList.Insert(0, new SelectListItem()
            {
                Text = "Select State",
                Value = string.Empty,
                Selected = true
            });
            return StateList;
        }
        public List<SelectListItem> Citylists(string sid)
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
            return cityList;
        }
    }
}