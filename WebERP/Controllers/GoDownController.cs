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
    public class GoDownController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public GoDownController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult GoDown_Master()
        {
            return View(dbContext.Godown_Master.ToList());
        }
        [HttpGet]
        public IActionResult Add_GoDown()
        {
            Godown_Master obj = new Godown_Master();            
            obj.Type = "Add";
            return View("Add_GoDown", obj);
        }
        [HttpPost]
        public async Task<IActionResult> SAVEGoDown(Godown_Master objGoDown)
        {
            if (ModelState.IsValid)
            {
                objGoDown.INS_DATE = DateTime.Now;
                objGoDown.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Godown_Master.Add(objGoDown);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("GoDown_Master");
            }
            else
            {
                return View("GoDown_Master");
            }
        }
        [HttpGet]
        public IActionResult ActionGoDown(int id)
        {
            Godown_Master obj = new Godown_Master();
            obj = dbContext.Godown_Master.Find(id);
            obj.Type = "Action";
            dbContext.Godown_Master.Update(obj);
            dbContext.SaveChanges();
            return View("Add_GoDown", obj);
        }
        [HttpGet]
        public IActionResult EditGoDown(int id)
        {
            Godown_Master obj = new Godown_Master();
            obj = dbContext.Godown_Master.Find(id);
            obj.Type = "Edit";
            dbContext.Godown_Master.Update(obj);
            dbContext.SaveChanges();
            return View("Add_GoDown", obj);
        }

        [HttpPost]
        public IActionResult EditGoDown(Godown_Master obj)
        {
            obj.UDT_DATE = DateTime.Now;
            obj.UDT_UID = userManager.GetUserName(HttpContext.User);
            dbContext.Godown_Master.Update(obj);
            dbContext.SaveChanges();
            return RedirectToAction("Godown_Master");
        }
        [HttpGet]
        public IActionResult DeleteGoDown(int ID)
        {
            var data = dbContext.Godown_Master.Find(ID);
            dbContext.Godown_Master.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Godown_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Godown_Master;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-GoDowns");
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
                        "GoDowns.xlsx");
                }
            }
        }
    }
}