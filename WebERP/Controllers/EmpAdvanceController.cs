using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Models;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Advance , Admin")]
    public class EmpAdvanceController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public EmpAdvanceController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult Emp_Adv_Master()
        {
            Employee_Advance employee_Advance = new Employee_Advance();
            employee_Advance.Type = "Add";
            employee_Advance.EMPDropDown = Emplists("P");
            employee_Advance.SalDropDown = SalType("P");
            employee_Advance.DOC_DATE = DateTime.Now;
            return View("Emp_Adv_Master",employee_Advance);
        }
        [HttpPost]
        public IActionResult Emp_Adv_Master(Employee_Advance employee_Advance)
        {
            employee_Advance.INS_DATE = DateTime.Today;
            employee_Advance.INS_UID = userManager.GetUserName(HttpContext.User); 
            dbContext.Employee_Advance.Add(employee_Advance);
            dbContext.SaveChanges();

            return RedirectToAction("Emp_Adv_Details");
        }
        
        [HttpGet]
        public IActionResult Emp_Adv_Details()
        {
            List<Employee_Advance> employee_Advance = new List<Employee_Advance>();
            employee_Advance = dbContext.Employee_Advance.ToList();
            foreach(var emp in employee_Advance.ToList())
            {
                var empname = dbContext.Employee_Masters.Where(e => e.ID == emp.EMP_CODE).Select(s => s.EMP_NAME).FirstOrDefault();
                emp.Emp_Name = empname;
            }
            return View(employee_Advance);
        }
        [HttpGet]
        public List<SelectListItem> Emplists(string type)
        {
            var Emplist = (from Emp in dbContext.Employee_Masters.Where(C => C.EMP_TYPE == type).ToList()
                             select new SelectListItem()
                             {
                                 Text = Emp.EMP_NAME + "/" + Emp.Emp_Father_Name + "/" + Emp.emp_mobile_no1 + "/" + dbContext.Department_Masters.Where(C => C.ID == Emp.DEP_CODE).Select(ss => ss.NAME).FirstOrDefault(),
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
        public IActionResult ActionEmpAdv(int id)
        {
            Employee_Advance employee_Advance = new Employee_Advance();
            employee_Advance = dbContext.Employee_Advance.Find(id);
            employee_Advance.EMPDropDown = Emplists(employee_Advance.EMP_TYPE);
            employee_Advance.SalDropDown = SalType(employee_Advance.EMP_TYPE);
            employee_Advance.Type = "Action";
            dbContext.Employee_Advance.Update(employee_Advance);
            return View("Emp_Adv_Master", employee_Advance);
        }
        [HttpGet]
        public IActionResult EditEmpAdv(int id)
        {
            Employee_Advance employee_Advance = new Employee_Advance();
            employee_Advance = dbContext.Employee_Advance.Find(id);
            employee_Advance.Type = "Edit";
            employee_Advance.EMPDropDown = Emplists(employee_Advance.EMP_TYPE);
            employee_Advance.SalDropDown = SalType(employee_Advance.EMP_TYPE);
            dbContext.Employee_Advance.Update(employee_Advance);
            dbContext.SaveChanges();
            return View("Emp_Adv_Master", employee_Advance);
        }

        [HttpPost]
        public IActionResult EditEmpAdv(Employee_Advance employee_Advance)
        {
            if (ModelState.IsValid)
            {
                var result = dbContext.Employee_Advance.SingleOrDefault(b => b.ID == employee_Advance.ID);
                if (result != null)
                {
                    result.UDT_DATE = DateTime.Today;
                    result.UDT_UID = userManager.GetUserName(HttpContext.User);
                    result.ADV_AMOUNT = employee_Advance.ADV_AMOUNT;
                    result.DOC_DATE = employee_Advance.DOC_DATE;
                    result.EMP_CODE = employee_Advance.EMP_CODE;
                    result.EMP_TYPE = employee_Advance.EMP_TYPE;
                    result.EMP_FATHER = employee_Advance.EMP_FATHER;
                    result.EMP_MOB_NO = employee_Advance.EMP_MOB_NO;
                    result.EMP_DEP = employee_Advance.EMP_DEP;
                    result.SAL_YYYYMM = employee_Advance.SAL_YYYYMM;
                    result.SAL_YYYYMM_BRK = employee_Advance.SAL_YYYYMM_BRK;
                    dbContext.SaveChanges();
                }
                return RedirectToAction("Emp_Adv_Details");
            }
            else
            {
                return View("Emp_Adv_Master", employee_Advance);
            }
        }
        [HttpGet]
        public List<SelectListItem> SalType(string type)
        {
            List<SelectListItem> Sallist = new List<SelectListItem>();
            if (type == "S")
            {
                Sallist.Insert(0, new SelectListItem()
                {
                    Text = "Full Month",
                    Value = "0",
                    Selected = true
                });
            }
            else
            {
                Sallist.Insert(0, new SelectListItem()
                {
                    Text = "1 to 15",
                    Value = "1",
                    Selected = true
                });
                Sallist.Insert(1, new SelectListItem()
                {
                    Text = "16 to 30",
                    Value = "2",
                    Selected = false
                });
            }
            return Sallist;
        }
        [HttpGet]
        public IActionResult DeleteEmpAdv(int ID)
        {
            var data = dbContext.Employee_Advance.Find(ID);
            dbContext.Employee_Advance.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Emp_Adv_Details");
        }

    }
}