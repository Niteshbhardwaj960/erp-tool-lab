using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public EmployeeController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Employee_Master()
        {

            return View(dbContext.Employee_Masters.ToList());
        }
        [HttpGet]
        public IActionResult AddEmployee()
        {
            Employee_Master obj = new Employee_Master();
            obj.DepDropDown = DepLists();
            obj.Type = "Add";
            return View(obj);
        }
        [HttpPost]
        public async Task<IActionResult> SAVEEmployee(Employee_Master objEmp)
        {
            var NAME = dbContext.Employee_Masters.FirstOrDefault(x => x.NAME == objEmp.EMP_NAME);
           
            if (NAME != null)
            {
                ModelState.AddModelError("Employee NAME", "Name Already Exists.");
            }
            if (ModelState.IsValid)
            {
                int Emp_No = dbContext.Employee_Masters
                .Select(p => Convert.ToInt32(p.EMP_CODE)).DefaultIfEmpty(0).Max();
                objEmp.EMP_CODE = Emp_No + 1;
                objEmp.Dep_Name = objEmp.DEP_CODE.ToString();
                objEmp.INS_DATE = DateTime.Now;
                objEmp.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Employee_Masters.Add(objEmp);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Employee_Master");
            }
            else
            {
                objEmp.Type = "Add";
                objEmp.DepDropDown = DepLists();
                return View("AddEmployee", objEmp);
            }
        }

        [HttpGet]
        public IActionResult ActionEmployee(int id)
        {
            Employee_Master obj = new Employee_Master();
            obj = dbContext.Employee_Masters.Find(id);
            obj.DepDropDown = DepLists();
            obj.Type = "Action";
            dbContext.Employee_Masters.Update(obj);
            dbContext.SaveChanges();
            return View("AddEmployee", obj);
        }
        [HttpGet]
        public IActionResult EditEmployee(int id)
        {
            Employee_Master obj = new Employee_Master();
            obj = dbContext.Employee_Masters.Find(id);
            obj.DepDropDown = DepLists();
            obj.Type = "Edit";
            dbContext.Employee_Masters.Update(obj);
            dbContext.SaveChanges();
            return View("AddEmployee", obj);
        }

        [HttpPost]
        public IActionResult EditEmployee(Employee_Master obj)
        {
            if (ModelState.IsValid)
            {
                obj.UDT_DATE = DateTime.Now;
                obj.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Employee_Masters.Update(obj);
                dbContext.SaveChanges();
                return RedirectToAction("Employee_Master");
            }
            else
            {
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult DeleteEmployee(int ID)
        {
            var data = dbContext.Employee_Masters.Find(ID);
            dbContext.Employee_Masters.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Employee_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Employee_Masters;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-Employee");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Employee Code";
                worksheet.Cell(currentRow, 2).Value = "Employee NAME";
                worksheet.Cell(currentRow, 3).Value = "Department";
                worksheet.Cell(currentRow, 4).Value = "Employee Type";
                worksheet.Cell(currentRow, 5).Value = "Employee Father's Name";
                worksheet.Cell(currentRow, 6).Value = "Employee Mobile No";
                worksheet.Cell(currentRow, 7).Value = "Employee Phone No";
                worksheet.Cell(currentRow, 8).Value = "Employee DOJ";
                worksheet.Cell(currentRow, 9).Value = "Employee Salary";
                worksheet.Cell(currentRow, 10).Value = "Active Tag";
                worksheet.Cell(currentRow, 11).Value = "Remarks";
                worksheet.Cell(currentRow, 12).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 13).Value = "INSERT UID";
                worksheet.Cell(currentRow, 14).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 15).Value = "UPDATE UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    string EMPTYPE ="";
                    string Active = "";
                    if (Data.EMP_TYPE == "S")
                    {
                        EMPTYPE = "Salary";
                    }
                    if (Data.EMP_TYPE == "P")
                    {
                        EMPTYPE = "Pc Rate";
                    }
                    if (Data.EMP_TYPE == "C")
                    {
                        EMPTYPE = "Contractor";
                    }
                    if (Data.active_tag == "1")
                    {
                        Active = "Yes";
                    }
                    else 
                    {
                        Active = "No";
                    }
                    worksheet.Cell(currentRow, 1).Value = Data.EMP_CODE;
                    worksheet.Cell(currentRow, 2).Value = Data.EMP_NAME;
                    worksheet.Cell(currentRow, 3).Value = Data.DEP_CODE;
                    worksheet.Cell(currentRow, 4).Value = EMPTYPE;
                    worksheet.Cell(currentRow, 5).Value = Data.Emp_Father_Name;
                    worksheet.Cell(currentRow, 6).Value = Data.emp_mobile_no1;
                    worksheet.Cell(currentRow, 7).Value = Data.emp_mobile_no2;
                    worksheet.Cell(currentRow, 8).Value = Data.emp_doj;
                    worksheet.Cell(currentRow, 9).Value = Data.emp_salary;
                    worksheet.Cell(currentRow, 10).Value = Active;
                    worksheet.Cell(currentRow, 11).Value = Data.Remarks;
                    worksheet.Cell(currentRow, 12).Value = Data.INS_DATE;
                    worksheet.Cell(currentRow, 13).Value = Data.INS_UID;
                    worksheet.Cell(currentRow, 14).Value = Data.UDT_DATE;
                    worksheet.Cell(currentRow, 15).Value = Data.UDT_UID;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Employee.xlsx");
                }
            }
        }
        public List<SelectListItem> DepLists()
        {
            var DepList = (from Dep in dbContext.Department_Masters
                           select new SelectListItem()
                           {
                               Text = Dep.NAME,
                               Value = Dep.ID.ToString(),
                           }).ToList();

            DepList.Insert(0, new SelectListItem()
            {
                Text = "Select Department",
                Value = string.Empty,
                Selected = true
            });
            return DepList;
        }
    }
}