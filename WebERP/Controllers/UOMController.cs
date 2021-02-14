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

namespace WebERP.Controllers
{
    public class UOMController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public UOMController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult UOM_Master()
        {
            return View(dbContext.UOM_MASTER.ToList());
        }
        [HttpGet]
        public IActionResult AddUOM()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SAVEUOM(UOM_MASTER objUOM)
        {
            if (ModelState.IsValid)
            {
                objUOM.INS_DATE = DateTime.Now;
                objUOM.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.UOM_MASTER.Add(objUOM);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("UOM_Master");
            }
            else
            {
                return View("UOM_Master");
            }
        }
        [HttpGet]
        public IActionResult EditUOM(int id)
        {
            return View(dbContext.UOM_MASTER.Find(id));
        }

        [HttpPost]
        public IActionResult EditUOM(UOM_MASTER objUOM)
        {
            objUOM.UDT_DATE = DateTime.Now;
            objUOM.UDT_UID = userManager.GetUserName(HttpContext.User);
            dbContext.UOM_MASTER.Update(objUOM);
            dbContext.SaveChanges();
            return RedirectToAction("UOM_MASTER");

        }
        [HttpGet]
        public IActionResult DeleteUOM(int ID)
        {
            var data = dbContext.UOM_MASTER.Find(ID);
            dbContext.UOM_MASTER.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("UOM_MASTER");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.UOM_MASTER;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-UOMs");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "NAME";
                worksheet.Cell(currentRow, 2).Value = "ABBRIVATION";
                worksheet.Cell(currentRow, 4).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 3).Value = "INSERT UID";
                worksheet.Cell(currentRow, 4).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 5).Value = "UPDATE UID";

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
                        "UOMs.xlsx");
                }
            }
        }
    }
}