using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    public class EmpAttandanceController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public EmpAttandanceController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult Emp_Attand_Master()
        {
            Employee_Attandance employee_Attandance = new Employee_Attandance();
            employee_Attandance.Type = "Add";
            employee_Attandance.EMPDropDown = Emplists("S");
            return View("Emp_Attand_Master", employee_Attandance);
        }
        [HttpPost]
        public IActionResult Emp_Attand_Master(Employee_Attandance employee_Attandance)
        {
            employee_Attandance.INS_DATE = DateTime.Today;
            employee_Attandance.INS_UID = userManager.GetUserName(HttpContext.User);
            employee_Attandance.EMP_TYPE = "S";
            dbContext.Employee_Attandance.Add(employee_Attandance);
            dbContext.SaveChanges();

            return RedirectToAction("Emp_Attand_Details");
        }
        [HttpGet]
        public IActionResult Emp_Attand_Details()
        {
            List<Employee_Attandance> employee_Attandance = new List<Employee_Attandance>();
            employee_Attandance = dbContext.Employee_Attandance.ToList();
            foreach (var emp in employee_Attandance.ToList())
            {
                var empname = dbContext.Employee_Masters.Where(e => e.ID == emp.EMP_CODE).Select(s => s.EMP_NAME).FirstOrDefault();
                emp.Emp_Name = empname;
            }
            return View(employee_Attandance);
        }
        [HttpGet]
        public List<SelectListItem> Emplists(string type)
        {
            var Emplist = (from Emp in dbContext.Employee_Masters.Where(C => C.EMP_TYPE == type).ToList()
                           select new SelectListItem()
                           {
                               Text = Emp.EMP_NAME,
                               Value = Emp.ID.ToString(),
                           }).ToList();

            Emplist.Insert(0, new SelectListItem()
            {
                Text = "Select Employee",
                Value = string.Empty,
                Selected = true
            });
            return Emplist;
        }

        [HttpGet]
        public IActionResult ActionEmpAttn(int id)
        {
            Employee_Attandance employee_Attandance = new Employee_Attandance();
            employee_Attandance = dbContext.Employee_Attandance.Find(id);
            employee_Attandance.EMPDropDown = Emplists(employee_Attandance.EMP_TYPE);
            employee_Attandance.Type = "Action";
            dbContext.Employee_Attandance.Update(employee_Attandance);
            return View("Emp_Attand_Master", employee_Attandance);
        }
        [HttpGet]
        public IActionResult EditEmpAttn(int id)
        {
            Employee_Attandance employee_Attandance = new Employee_Attandance();
            employee_Attandance = dbContext.Employee_Attandance.Find(id);
            employee_Attandance.Type = "Edit";
            employee_Attandance.EMPDropDown = Emplists(employee_Attandance.EMP_TYPE);
            dbContext.Employee_Attandance.Update(employee_Attandance);
            dbContext.SaveChanges();
            return View("Emp_Attand_Master", employee_Attandance);
        }

        [HttpPost]
        public IActionResult EditEmpAttn(Employee_Attandance employee_Attandance)
        {
            if (ModelState.IsValid)
            {
                var result = dbContext.Employee_Attandance.SingleOrDefault(b => b.ID == employee_Attandance.ID);
                if (result != null)
                {
                    result.UDT_DATE = DateTime.Today;
                    result.UDT_UID = userManager.GetUserName(HttpContext.User);
                    result.PAY_DAYS = employee_Attandance.PAY_DAYS;
                    result.OT_HRS = employee_Attandance.OT_HRS;
                    result.DOC_DATE = employee_Attandance.DOC_DATE;
                    result.EMP_CODE = employee_Attandance.EMP_CODE;
                    result.EMP_TYPE = "S";
                    result.SAL_YYYYMM = employee_Attandance.SAL_YYYYMM;
                    result.SAL_YYYYMM_BRK = employee_Attandance.SAL_YYYYMM_BRK;
                    dbContext.SaveChanges();
                }
                return RedirectToAction("Emp_Attand_Details");
            }
            else
            {
                return View("Emp_Attand_Master", employee_Attandance);
            }
        }
        [HttpGet]
        public IActionResult DeleteEmpAttn(int ID)
        {
            var data = dbContext.Employee_Attandance.Find(ID);
            dbContext.Employee_Attandance.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Emp_Attand_Details");
        }
    }
}