using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    public class SizeController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public SizeController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Size_Master()
        {
            return View(dbContext.Size_Master.ToList());
        }
        [HttpGet]
        public IActionResult AddSize()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SAVESize(Size_Master objSize)
        {
            if (ModelState.IsValid)
            {
                objSize.INS_DATE = DateTime.Now;
                objSize.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Size_Master.Add(objSize);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Size_Master");
            }
            else
            {
                return View("Size_Master");
            }
        }
        [HttpGet]
        public IActionResult EditSize(int id)
        {
            return View(dbContext.Size_Master.Find(id));
        }

        [HttpPost]
        public IActionResult EditSize(Size_Master objSize)
        {
            objSize.UDT_DATE = DateTime.Now;
            objSize.UDT_UID = userManager.GetUserName(HttpContext.User);
            dbContext.Size_Master.Update(objSize);
            dbContext.SaveChanges();
            return RedirectToAction("Size_Master");

        }
        [HttpGet]
        public IActionResult DeleteSize(int ID)
        {
            var data = dbContext.Size_Master.Find(ID);
            dbContext.Size_Master.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Size_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Size_Master;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-Sizes");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "NAME";
                worksheet.Cell(currentRow, 2).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 3).Value = "INSERT UID";
                worksheet.Cell(currentRow, 4).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 5).Value = "UPDATE UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.NAME;
                    worksheet.Cell(currentRow, 2).Value = Data.INS_DATE;
                    worksheet.Cell(currentRow, 3).Value = Data.INS_UID;
                    worksheet.Cell(currentRow, 4).Value = Data.UDT_DATE;
                    worksheet.Cell(currentRow, 5).Value = Data.UDT_UID;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Sizes.xlsx");
                }
            }
        }
    }
}