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
    public class TermController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public TermController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Term_Master()
        {
            return View(dbContext.Term_Master.ToList());
        }
        [HttpGet]
        public IActionResult AddTerm()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SAVETerm(Term_Master objTerm)
        {
            var NAME = dbContext.Term_Master.FirstOrDefault(x => x.NAME == objTerm.NAME);

            if (NAME != null)
            {
                ModelState.AddModelError("NAME", "Name Already Exists.");
            }
            if (ModelState.IsValid)
            {
                objTerm.INS_DATE = DateTime.Now;
                objTerm.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Term_Master.Add(objTerm);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Term_Master");
            }
            else
            {
                return View("AddTerm",objTerm);
            }
        }
        [HttpGet]
        public IActionResult ActionTerm(int id)
        {
            Term_Master objTerm = new Term_Master();
            objTerm = dbContext.Term_Master.Find(id);
            objTerm.Type = "Action";
            dbContext.Term_Master.Update(objTerm);
            dbContext.SaveChanges();
            return View("EditTerm", objTerm);
        }
        [HttpGet]
        public IActionResult EditTerm(int id)
        {
            Term_Master objTerm = new Term_Master();
            objTerm = dbContext.Term_Master.Find(id);
            objTerm.Type = "Edit";
            dbContext.Term_Master.Update(objTerm);
            dbContext.SaveChanges();
            return View(objTerm);
        }
       
        [HttpPost]
        public IActionResult EditTerm(Term_Master objTerm)
        {
            if (ModelState.IsValid)
            {
                objTerm.UDT_DATE = DateTime.Now;
                objTerm.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Term_Master.Update(objTerm);
                dbContext.SaveChanges();
                return RedirectToAction("Term_Master");
            }
            else
            {
                return View(objTerm);
            }
        }
        [HttpGet]
        public IActionResult DeleteTerm(int ID)
        {
            var data = dbContext.Term_Master.Find(ID);
            dbContext.Term_Master.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Term_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Term_Master;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-Terms");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "NAME";
                worksheet.Cell(currentRow, 2).Value = "Purchase Order";
                worksheet.Cell(currentRow, 3).Value = "Sales Order";
                worksheet.Cell(currentRow, 4).Value = "ACTIVE TAG";
                worksheet.Cell(currentRow, 5).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 6).Value = "INSERT UID";
                worksheet.Cell(currentRow, 7).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 8).Value = "UPDATE UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.NAME;
                    worksheet.Cell(currentRow, 2).Value = Data.PO;
                    worksheet.Cell(currentRow, 3).Value = Data.SAL_Order;
                    worksheet.Cell(currentRow, 4).Value = Data.ACTIVE_TAG;
                    worksheet.Cell(currentRow, 5).Value = Data.INS_DATE;
                    worksheet.Cell(currentRow, 6).Value = Data.INS_UID;
                    worksheet.Cell(currentRow, 7).Value = Data.UDT_DATE;
                    worksheet.Cell(currentRow, 8).Value = Data.UDT_UID;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Terms.xlsx");
                }
            }
        }
    }
}