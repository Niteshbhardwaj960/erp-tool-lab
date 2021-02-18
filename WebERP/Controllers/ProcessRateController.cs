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
    public class ProcessRateController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public ProcessRateController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult ProcessRate_Master()
        {
            return View(dbContext.ProcessRate_Master.ToList());
        }
        [HttpGet]
        public IActionResult AddProcessRate()
        {
            ProcessRate_Master obj = new ProcessRate_Master();
            obj.Type = "Add";
            return View("AddProcessRate", obj);
        }
        [HttpPost]
        public async Task<IActionResult> SAVEProcessRate(ProcessRate_Master objProcessRate)
        {
            if (ModelState.IsValid)
            {
                objProcessRate.INS_DATE = DateTime.Now;
                objProcessRate.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.ProcessRate_Master.Add(objProcessRate);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("ProcessRate_Master");
            }
            else
            {
                return View("ProcessRate_Master");
            }
        }
        [HttpGet]
        public IActionResult ActionProcessRate(int id)
        {
            ProcessRate_Master obj = new ProcessRate_Master();
            obj = dbContext.ProcessRate_Master.Find(id);
            obj.Type = "Action";
            dbContext.ProcessRate_Master.Update(obj);
            dbContext.SaveChanges();
            return View("AddProcessRate", obj);
        }
        [HttpGet]
        public IActionResult EditProcessRate(int id)
        {
            ProcessRate_Master obj = new ProcessRate_Master();
            obj = dbContext.ProcessRate_Master.Find(id);
            obj.Type = "Edit";
            dbContext.ProcessRate_Master.Update(obj);
            dbContext.SaveChanges();
            return View("AddProcessRate", obj);
        }

        [HttpPost]
        public IActionResult EditProcessRate(ProcessRate_Master obj)
        {
            obj.UDT_DATE = DateTime.Now;
            obj.UDT_UID = userManager.GetUserName(HttpContext.User);
            dbContext.ProcessRate_Master.Update(obj);
            dbContext.SaveChanges();
            return RedirectToAction("ProcessRate_Master");
        }
        [HttpGet]
        public IActionResult DeleteProcessRate(int ID)
        {
            var data = dbContext.ProcessRate_Master.Find(ID);
            dbContext.ProcessRate_Master.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("ProcessRate_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.ProcessRate_Master;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-ProcessRate");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Process Name";
                worksheet.Cell(currentRow, 2).Value = "Rate";
                worksheet.Cell(currentRow, 3).Value = "UOM Name";
                worksheet.Cell(currentRow, 4).Value = "From Date";
                worksheet.Cell(currentRow, 5).Value = "To Date";
                worksheet.Cell(currentRow, 6).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 7).Value = "INSERT UID";
                worksheet.Cell(currentRow, 8).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 9).Value = "UPDATE UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.Proc_Code;
                    worksheet.Cell(currentRow, 2).Value = Data.Rate;
                    worksheet.Cell(currentRow, 3).Value = Data.UOM_Code;
                    worksheet.Cell(currentRow, 4).Value = Data.From_DATE;
                    worksheet.Cell(currentRow, 5).Value = Data.To_DATE;
                    worksheet.Cell(currentRow, 6).Value = Data.INS_DATE;
                    worksheet.Cell(currentRow, 7).Value = Data.INS_UID;
                    worksheet.Cell(currentRow, 8).Value = Data.UDT_DATE;
                    worksheet.Cell(currentRow, 9).Value = Data.UDT_UID;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ProcessRate.xlsx");
                }
            }
        }
    }
}