using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebERP.Data;
using WebERP.Helpers;
using WebERP.Models;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Department , Admin")]
    public class DepartmentController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public DepartmentController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Department_Master()
        {

            return View(dbContext.Department_Masters.ToList());
        }
        [HttpGet]
        public IActionResult AddDepartment()
        {
            Department_Master obj = new Department_Master();            
            obj.Type = "Add";
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> SAVEDepartment(Department_Master objDep)
        {
            var NAME = dbContext.Department_Masters.FirstOrDefault(x => x.NAME == objDep.NAME);

            if (NAME != null)
            {
                ModelState.AddModelError("NAME", "Name Already Exists.");
            }
            if (ModelState.IsValid)
            {
                objDep.INS_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                objDep.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Department_Masters.Add(objDep);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Department_Master");
            }
            else
            {
                return View("AddDepartment", objDep);
            }
        }

        [HttpGet]
        public IActionResult ActionDepartment(int id)
        {
            Department_Master obj = new Department_Master();
            obj = dbContext.Department_Masters.Find(id);
            obj.Type = "Action";
            dbContext.Department_Masters.Update(obj);
            dbContext.SaveChanges();
            return View("AddDepartment", obj);
        }
        [HttpGet]
        public IActionResult EditDepartment(int id)
        {
            Department_Master obj = new Department_Master();
            obj = dbContext.Department_Masters.Find(id);
            obj.Type = "Edit";
            dbContext.Department_Masters.Update(obj);
            dbContext.SaveChanges();
            return View("AddDepartment", obj);
        }

        [HttpPost]
        public IActionResult EditDepartment(Department_Master obj)
        {
            if (ModelState.IsValid)
            {
                obj.UDT_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                obj.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Department_Masters.Update(obj);
                dbContext.SaveChanges();
                return RedirectToAction("Department_Master");
            }
            else
            {
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult DeleteDepartment(int ID)
        {
            var data = dbContext.Department_Masters.Find(ID);
            dbContext.Department_Masters.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Department_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Department_Masters;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-Department");
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
                        "Department.xlsx");
                }
            }
        }
    }
}