using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebERP.Data;
using WebERP.Models;
using ClosedXML.Excel;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using WebERP.Helpers;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Brand , Admin")]
    public class BrandController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public BrandController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Brand_Master()
        {
            ViewBag.Message = null;
            return View(dbContext.Brand_Master.ToList());
        }
        [HttpGet]
        public IActionResult AddBrand()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SAVEBrand(Brand_Master objBrand)
        {
            var NAME = dbContext.Brand_Master.FirstOrDefault(x => x.NAME == objBrand.NAME);

            if (NAME != null)
            {
                ModelState.AddModelError("NAME", "Name Already Exists.");
            }
            if (ModelState.IsValid)
            {
                objBrand.INS_DATE = DateTime.Now;
                objBrand.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Brand_Master.Add(objBrand);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Brand_Master");
            }
            else
            {
                return View("ADDBrand",objBrand);
            }
        }
        [HttpGet]
        public IActionResult ActionBrand(int id)
        {
            Brand_Master objBrand = new Brand_Master();
            objBrand = dbContext.Brand_Master.Find(id);
            objBrand.Type = "Action";
            dbContext.Brand_Master.Update(objBrand);
            dbContext.SaveChanges();
            return View("EditBrand", objBrand);
        }
        [HttpGet]
        public IActionResult EditBrand(int id)
        {
            Brand_Master objBrand = new Brand_Master();
            objBrand = dbContext.Brand_Master.Find(id);
            objBrand.Type = "Edit";
            dbContext.Brand_Master.Update(objBrand);
            dbContext.SaveChanges();
            return View(objBrand);
        }
        
        [HttpPost]
        public IActionResult EditBrand(Brand_Master objBrand)
        {
            if (ModelState.IsValid)
            {
                objBrand.UDT_DATE = DateTime.Now;
                objBrand.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Brand_Master.Update(objBrand);
                dbContext.SaveChanges();
                return RedirectToAction("Brand_Master");
            }
            else
            {
                return View(objBrand);
            }
        }
        [HttpGet]
        public IActionResult DeleteBrand(int ID)
        {
            var data = dbContext.Brand_Master.Find(ID);
            dbContext.Brand_Master.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Brand_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Brand_Master;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-Brands");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "NAME";
                worksheet.Cell(currentRow, 2).Value = "Abbreviation";
                worksheet.Cell(currentRow, 3).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 4).Value = "INSERT UID";
                worksheet.Cell(currentRow, 5).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 6).Value = "UPDATE UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.NAME;
                    worksheet.Cell(currentRow, 2).Value = Data.ABV;
                    worksheet.Cell(currentRow, 3).Value = Data.INS_DATE;
                    worksheet.Cell(currentRow, 4).Value = Data.INS_UID;
                    worksheet.Cell(currentRow, 5).Value = Data.UDT_DATE;
                    worksheet.Cell(currentRow, 6).Value = Data.UDT_UID;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Brands.xlsx");
                }
            }
        }


    }
}