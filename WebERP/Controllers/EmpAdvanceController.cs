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
            return View("Emp_Adv_Master",employee_Advance);
        }
        [HttpPost]
        public IActionResult Emp_Adv_Master(Employee_Advance employee_Advance)
        {
            employee_Advance.INS_DATE = DateTime.Now;
            employee_Advance.INS_UID = userManager.GetUserName(HttpContext.User);
            dbContext.Employee_Advance.Add(employee_Advance);
            dbContext.SaveChanges();

            return RedirectToAction("Payments_Details");
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
    }
}