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
    public class ProcessController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public ProcessController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Process_Master()
        {
            return View(dbContext.Process_Master.ToList());
        }
        [HttpGet]
        public IActionResult AddProcess()
        {
            Process_Master obj = new Process_Master();
            obj.Type = "Add";
            return View("AddProcess", obj);
        }
        [HttpPost]
        public async Task<IActionResult> SAVEProcess(Process_Master objProcess)
        {
            if (ModelState.IsValid)
            {
                objProcess.INS_DATE = DateTime.Now;
                objProcess.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Process_Master.Add(objProcess);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Process_Master");
            }
            else
            {
                return View("Process_Master");
            }
        }
        [HttpGet]
        public IActionResult ActionProcess(int id)
        {
            Process_Master obj = new Process_Master();
            obj = dbContext.Process_Master.Find(id);
            obj.Type = "Action";
            dbContext.Process_Master.Update(obj);
            dbContext.SaveChanges();
            return View("AddProcess", obj);
        }
        [HttpGet]
        public IActionResult EditProcess(int id)
        {
            Process_Master obj = new Process_Master();
            obj = dbContext.Process_Master.Find(id);
            obj.Type = "Edit";
            dbContext.Process_Master.Update(obj);
            dbContext.SaveChanges();
            return View("AddProcess", obj);
        }

        [HttpPost]
        public IActionResult EditProcess(Process_Master obj)
        {
            obj.UDT_DATE = DateTime.Now;
            obj.UDT_UID = userManager.GetUserName(HttpContext.User);
            dbContext.Process_Master.Update(obj);
            dbContext.SaveChanges();
            return RedirectToAction("Process_Master");
        }
        [HttpGet]
        public IActionResult DeleteProcess(int ID)
        {
            var data = dbContext.Process_Master.Find(ID);
            dbContext.Process_Master.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Process_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Process_Master;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-Process");
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
                        "Process.xlsx");
                }
            }
        }
    }
}