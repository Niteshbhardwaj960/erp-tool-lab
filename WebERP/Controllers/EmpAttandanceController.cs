using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Helpers;
using WebERP.Models;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Attandance , Admin")]
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
            employee_Attandance.DOC_DATE = DateTime.Now;
            return View("Emp_Attand_Master", employee_Attandance);
        }
        [HttpPost]
        public IActionResult Emp_Attand_Master(Employee_Attandance employee_Attandance)
        {
            employee_Attandance.INS_DATE = DateTime.Now;
            employee_Attandance.INS_UID = userManager.GetUserName(HttpContext.User);
            employee_Attandance.EMP_TYPE = "S";
            dbContext.Employee_Attandance.Add(employee_Attandance);
            dbContext.SaveChanges();

            return RedirectToAction("Emp_Attand_Details");
        }
        [HttpGet]
        public IActionResult Emp_Attand_Details()
        {
            ViewBag.Message = null;
            List<Employee_Attandance> employee_Attandance = new List<Employee_Attandance>();
            employee_Attandance = dbContext.Employee_Attandance.ToList();
            foreach (var emp in employee_Attandance.ToList())
            {
                var empname = dbContext.Employee_Masters.Where(e => e.ID == emp.EMP_CODE).Select(s => s.EMP_NAME).FirstOrDefault();
                emp.Emp_Name = empname;
                if (emp.SAL_YYYYMM_BRK == 0)
                {
                    emp.Emp_Sal_Type = "Full Month";
                }
                if (emp.SAL_YYYYMM_BRK == 1)
                {
                    emp.Emp_Sal_Type = "1 to 15";
                }
                if (emp.SAL_YYYYMM_BRK == 2)
                {
                    emp.Emp_Sal_Type = "16 to 30";
                }
            }
            return View(employee_Attandance);
        }
        [HttpGet]
        public List<SelectListItem> Emplists(string type)
        {
            var Emplist = (from Emp in dbContext.Employee_Masters.Where(C => C.EMP_TYPE == type).ToList()
                           select new SelectListItem()
                           {
                               Text = Emp.EMP_NAME + "/" + Emp.Emp_Father_Name + "/" + Emp.emp_mobile_no1 + "/" + dbContext.Department_Masters.Where(C => C.ID == Emp.DEP_CODE).Select(ss => ss.NAME).FirstOrDefault(),
                               Value = Emp.EMP_CODE.ToString(),
                           }).ToList();

            Emplist.Insert(0, new SelectListItem()
            {
                Text = "Select Employee",
                Value = string.Empty,
                Selected = true
            });
            return Emplist;
        }
        
        public List<SelectListItem> Deplists()
        {
            var Emplist = (from Emp in dbContext.Department_Masters.ToList()
                           select new SelectListItem()
                           {
                               Text = Emp.NAME,
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
        //public string Deplists(string ID)
        //{
        //    var Deplist = dbContext.Department_Masters.Where(d => d.ID.ToString() == ID).Select(e => e.NAME).FirstOrDefault();
        //    return Deplist;
        //}
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
            var emp_code = dbContext.Employee_Attandance.Where(p => p.ID == id).FirstOrDefault();
            var salpaid = dbContext.EMP_SAL.Where(p => p.EMP_CODE == emp_code.EMP_CODE && p.SAL_MONTH.Value.Month == emp_code.SAL_YYYYMM.Value.Month && p.SAL_MONTH.Value.Year == emp_code.SAL_YYYYMM.Value.Year && p.PAID_SAL > 0).FirstOrDefault();

            Employee_Attandance employee_Attandance = new Employee_Attandance();
            employee_Attandance = dbContext.Employee_Attandance.Find(id);
            if (salpaid == null)
            {
                employee_Attandance.paidType = "NonPaid";
            }
            else
            {
                employee_Attandance.paidType = "Paid";
            }
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
                    result.EMP_FATHER = employee_Attandance.EMP_FATHER;
                    result.EMP_MOB_NO = employee_Attandance.EMP_MOB_NO;
                    result.EMP_DEP = employee_Attandance.EMP_DEP;
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
            var empattn = dbContext.Employee_Attandance.Where(p => p.ID == ID).FirstOrDefault();
            var duplsal = dbContext.EMP_SAL.Where(p => p.EMP_CODE == empattn.EMP_CODE).FirstOrDefault();
            var duplpaidsal = dbContext.EMP_SAL.Where(p => p.EMP_CODE == empattn.EMP_CODE && p.PAID_SAL > 0 && p.SAL_MONTH.Value.Month == empattn.SAL_YYYYMM.Value.Month && p.SAL_MONTH.Value.Year == empattn.SAL_YYYYMM.Value.Year).FirstOrDefault();
            var dupladvs = dbContext.Employee_Advance.Where(p => p.EMP_CODE == empattn.EMP_CODE && p.SAL_YYYYMM.Value.Month == empattn.SAL_YYYYMM.Value.Month && p.SAL_YYYYMM.Value.Year == empattn.SAL_YYYYMM.Value.Year).FirstOrDefault();

            if (duplsal == null && dupladvs == null && duplpaidsal == null)
            {
                var data = dbContext.Employee_Attandance.Find(ID);
                dbContext.Employee_Attandance.Remove(data);
                dbContext.SaveChanges();
            }
            else
            {
                var employee_Att = dbContext.Employee_Attandance.ToList();
                foreach (var emp in employee_Att.ToList())
                {
                    var empname = dbContext.Employee_Masters.Where(e => e.ID == emp.EMP_CODE).Select(s => s.EMP_NAME).FirstOrDefault();
                    emp.Emp_Name = empname;
                    if (emp.SAL_YYYYMM_BRK == 0)
                    {
                        emp.Emp_Sal_Type = "Full Month";
                    }
                    if (emp.SAL_YYYYMM_BRK == 1)
                    {
                        emp.Emp_Sal_Type = "1 to 15";
                    }
                    if (emp.SAL_YYYYMM_BRK == 2)
                    {
                        emp.Emp_Sal_Type = "16 to 30";
                    }
                }
                ViewBag.Message = string.Format("Can not delete entry. Record present in Employee Advance or Salary");
                return View("Emp_Attand_Details", employee_Att);
            }            
            return RedirectToAction("Emp_Attand_Details");
        }
    }
}