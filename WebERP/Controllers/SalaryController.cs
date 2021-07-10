using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebERP.Data;
using WebERP.Helpers;
using WebERP.Models;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Generate_Salary , Admin")]
    public class SalaryController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public SalaryController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult Emp_Sal_Detail(DateTime SAL_MONTH, string emmp_type)
        {
            EmpSalViewModel empSalViewModel = new EmpSalViewModel();
            if (SAL_MONTH.Year != 1)
            {
                empSalViewModel.emp_Sals = dbContext.EMP_SAL.AsNoTracking().Where(at => at.SAL_MONTH.Value.Month == SAL_MONTH.Month && at.SAL_MONTH.Value.Year == SAL_MONTH.Year && at.EMP_TYPE == emmp_type).ToList();
                empSalViewModel.FilterMonth = SAL_MONTH;
                empSalViewModel.Emp_Type = emmp_type;
            }
            else
            {
                empSalViewModel.FilterMonth = DateTime.Now;
                empSalViewModel.Emp_Type = emmp_type;
                empSalViewModel.emp_Sals = dbContext.EMP_SAL.AsNoTracking().Where(at => at.SAL_MONTH.Value.Month == DateTime.Now.Month && at.SAL_MONTH.Value.Year == DateTime.Now.Year && at.EMP_TYPE == "S").ToList();
            }
            return View("Emp_Sal_Detail", empSalViewModel);
        }
        [HttpPost]
        public IActionResult Emp_Sal_Detail_Filter(DateTime SAL_MONTH, string Emp_Type)
        {
            EmpSalViewModel empSalViewModel = new EmpSalViewModel();
            empSalViewModel.FilterMonth = SAL_MONTH;
            empSalViewModel.Emp_Type = Emp_Type;
            empSalViewModel.emp_Sals = dbContext.EMP_SAL.AsNoTracking().Where(at => at.SAL_MONTH.Value.Month == SAL_MONTH.Month && at.SAL_MONTH.Value.Year == SAL_MONTH.Year && at.EMP_TYPE == Emp_Type).ToList();
            return View("Emp_Sal_Detail", empSalViewModel);
        }
        [HttpGet]
        public IActionResult Salary_Gen()
        {
            Emp_Sal emp_Sal = new Emp_Sal();
            emp_Sal.SAL_MONTH = DateTime.Now;
            return View("Salary_Gen", emp_Sal);
        }
        [HttpPost]
        public IActionResult Salary_Gen(Emp_Sal emp_Sal)
        {
            var emp_sal_year = emp_Sal.SAL_MONTH.Value.Year;
            var emp_sal_month = emp_Sal.SAL_MONTH.Value.Month;
            var proc_rate_error = "";
           
            if (ModelState.IsValid)
            {
                if (emp_Sal.EMP_TYPE == "S")
                {
                    var delete_sal = dbContext.EMP_SAL.Where(ds => ds.SAL_MONTH.Value.Month == emp_Sal.SAL_MONTH.Value.Month && ds.SAL_MONTH.Value.Year == emp_Sal.SAL_MONTH.Value.Year && ds.PAID_SAL == 0).ToList();
                    dbContext.EMP_SAL.RemoveRange(delete_sal);
                    dbContext.SaveChanges();

                    var EMP_ATT = dbContext.Employee_Attandance.AsNoTracking().Where(at => at.SAL_YYYYMM.Value.Month == emp_Sal.SAL_MONTH.Value.Month && at.SAL_YYYYMM.Value.Year == emp_Sal.SAL_MONTH.Value.Year).ToList();
                    Employee_Master employee_master = new Employee_Master();
                    List<Emp_Sal> EmppSalAdd = new List<Emp_Sal>();

                    var Noofdays = DateTime.DaysInMonth(emp_sal_year, emp_sal_month);
                    if (EMP_ATT.Count != 0)
                    {
                        foreach (var Emp in EMP_ATT)
                        {
                            var DuplicateSal = dbContext.EMP_SAL.Where(ds => ds.PAID_SAL >= 0 && ds.EMP_CODE== Emp.EMP_CODE && ds.INS_DATE.Value.Month == emp_Sal.SAL_MONTH.Value.Month && ds.INS_DATE.Value.Year == emp_Sal.SAL_MONTH.Value.Year).Select(ss => ss.EMP_NAME).FirstOrDefault();
                            if (DuplicateSal == null)
                            {
                                decimal empAdvance = dbContext.Employee_Advance.AsNoTracking().Where(ea => ea.EMP_CODE == Emp.EMP_CODE && ea.SAL_YYYYMM.Value.Month == emp_sal_month && ea.SAL_YYYYMM.Value.Year == emp_sal_year).Select(aa => aa.ADV_AMOUNT).Sum();
                                employee_master = dbContext.Employee_Masters.AsNoTracking().Where(em => em.EMP_CODE == Emp.EMP_CODE).FirstOrDefault();
                                decimal Ern_Sal = Math.Round((employee_master.emp_salary / Noofdays) * Emp.PAY_DAYS, 2);
                                decimal Ern_OT = Math.Round(((employee_master.emp_salary / Noofdays) / 12) * Emp.OT_HRS, 2);
                                decimal Pay_sal = Ern_Sal + Ern_OT - empAdvance;
                                decimal RFF_SAL = (Math.Round(Pay_sal / 50) * 50) - Pay_sal;
                                EmppSalAdd.Add(new Emp_Sal()
                                {
                                    SAL_MONTH = Emp.SAL_YYYYMM,
                                    EMP_CODE = Emp.EMP_CODE,
                                    EMP_TYPE = Emp.EMP_TYPE,
                                    EMP_NAME = employee_master.EMP_NAME,
                                    SALARY = employee_master.emp_salary,
                                    SHIFT_HRS = employee_master.Shift_Hrs,
                                    PAY_DAYS = Emp.PAY_DAYS,
                                    OT_HRS = Emp.OT_HRS,
                                    ERN_SAL = Ern_Sal,
                                    ERN_OT = Ern_OT,
                                    ADVANCE_AMOUNT = empAdvance,
                                    PAYABAL_SALARY = Pay_sal,
                                    RF_SAL = RFF_SAL,
                                    NET_PAY_SAL = Pay_sal + RFF_SAL,
                                    INS_DATE = DateTime.Now,
                                    INS_UID = userManager.GetUserName(HttpContext.User),
                                });
                            }
                        }
                        foreach (var item in EmppSalAdd)
                        {
                            dbContext.EMP_SAL.Add(item);
                            dbContext.SaveChanges();

                        }
                        return RedirectToAction("Emp_Sal_Detail", new { SAL_MONTH = emp_Sal.SAL_MONTH, emmp_type = emp_Sal.EMP_TYPE });
                    }
                    else
                    {
                        ModelState.AddModelError("EMP_TYPE", "Attandance not applied for Month");
                        return View("Salary_Gen", emp_Sal);
                    }
                }
                else
                {
                    List<V_PRODUCTION_DETAIL> EMP_PROD = new List<V_PRODUCTION_DETAIL>();
                    List<Emp_Sal> EmppSalAdd = new List<Emp_Sal>();
                    Emp_Sal_PC_Cont_Dtl Emp_Sal_PC_Cont_Dtl = new Emp_Sal_PC_Cont_Dtl();
                    List<Emp_Sal_PC_Cont_Dtl> emp_Sal_PC_Cont_Dtls = new List<Emp_Sal_PC_Cont_Dtl>();
                    if (emp_Sal.SAL_TYPE == 1)
                    {
                        EMP_PROD = dbContext.V_PRODUCTION_DETAIL.AsNoTracking().Where(at => at.DOC_DATE >= Convert.ToDateTime(emp_Sal.SAL_MONTH.Value.Month.ToString() + "-1-" + emp_Sal.SAL_MONTH.Value.Year.ToString()) && at.DOC_DATE <= Convert.ToDateTime(emp_Sal.SAL_MONTH.Value.Month.ToString() + "-15-" + emp_Sal.SAL_MONTH.Value.Year.ToString())).ToList();

                    }
                    else
                    {
                        EMP_PROD = dbContext.V_PRODUCTION_DETAIL.AsNoTracking().Where(at => at.DOC_DATE >= Convert.ToDateTime(emp_Sal.SAL_MONTH.Value.Month.ToString() + "-16-" + emp_Sal.SAL_MONTH.Value.Year.ToString()) && at.DOC_DATE <= Convert.ToDateTime(emp_Sal.SAL_MONTH.Value.Month.ToString() + "-31-" + emp_Sal.SAL_MONTH.Value.Year.ToString())).ToList();

                    }

                    foreach (var ProcessRate in EMP_PROD)
                    {
                        if (ProcessRate.proc_rate == "0")
                        {
                            proc_rate_error = proc_rate_error + "Add ProcessRate for Artical Name = " + dbContext.Artical_Master.Where(am => am.ID == ProcessRate.artical_code).Select(aam => aam.NAME).FirstOrDefault() + " , ";
                        }
                    }

                    if (proc_rate_error != "")
                    {
                        ModelState.AddModelError("EMP_TYPE", proc_rate_error);
                        return View("Salary_Gen", emp_Sal);
                    }

                    if (EMP_PROD.Count != 0)
                    {
                        var Emp_prod_groupby = EMP_PROD.GroupBy(x => x.emp_code).Select(x => new { emp_code = x.Key, ART_Amount = x.Sum(xx => xx.Artical_Amount) }).ToList();
                        Employee_Master employee_master = new Employee_Master();
                        foreach (var Emp in Emp_prod_groupby)
                        {
                            decimal empAdvance = dbContext.Employee_Advance.AsNoTracking().Where(ea => ea.EMP_CODE == Emp.emp_code && ea.SAL_YYYYMM.Value.Month == emp_sal_month && ea.SAL_YYYYMM.Value.Year == emp_sal_year).Select(aa => aa.ADV_AMOUNT).Sum();
                            employee_master = dbContext.Employee_Masters.AsNoTracking().Where(em => em.EMP_CODE == Emp.emp_code).FirstOrDefault();
                            decimal Ern_Sal = Emp.ART_Amount;
                            decimal Ern_OT = 0;
                            decimal Pay_sal = Ern_Sal + Ern_OT - empAdvance;
                            decimal RFF_SAL = (Math.Round(Pay_sal / 50) * 50) - Pay_sal;
                            EmppSalAdd.Add(new Emp_Sal()
                            {
                                SAL_MONTH = emp_Sal.SAL_MONTH,
                                EMP_CODE = Emp.emp_code,
                                EMP_TYPE = "P",
                                EMP_NAME = employee_master.EMP_NAME,
                                SALARY = employee_master.emp_salary,
                                SHIFT_HRS = 0,
                                PAY_DAYS = 0,
                                OT_HRS = 0,
                                ERN_SAL = Ern_Sal,
                                ERN_OT = Ern_OT,
                                ADVANCE_AMOUNT = empAdvance,
                                PAYABAL_SALARY = Pay_sal,
                                RF_SAL = RFF_SAL,
                                NET_PAY_SAL = Pay_sal + RFF_SAL,
                                INS_DATE = DateTime.Now,
                                INS_UID = userManager.GetUserName(HttpContext.User),
                            });
                        }
                        foreach (var item in EmppSalAdd)
                        {
                            dbContext.EMP_SAL.Add(item);

                            dbContext.SaveChanges();
                        }

                        foreach (var Emp_pro in EMP_PROD)
                        {
                            emp_Sal_PC_Cont_Dtls.Add(new Emp_Sal_PC_Cont_Dtl()
                            {
                                ART_AMOUNT = Emp_pro.Artical_Amount,
                                ART_CODE = Emp_pro.artical_code,
                                EMP_SAL_FK = dbContext.EMP_SAL.Where(es => es.EMP_CODE == Emp_pro.emp_code && es.SAL_MONTH.Value.Month == emp_sal_month && es.SAL_MONTH.Value.Year == emp_sal_year).Select(se => se.ID).FirstOrDefault(),
                                PROC_CODE = Emp_pro.proc_code,
                                PRODUCT_QTY = Emp_pro.prod_qty,
                                PRODUCT_RATE = Convert.ToDecimal(Emp_pro.proc_rate),
                                INS_DATE = DateTime.Now,
                                INS_UID = userManager.GetUserName(HttpContext.User),
                            });
                        }
                        foreach (var itemdt in emp_Sal_PC_Cont_Dtls)
                        {
                            dbContext.Emp_Sal_PC_Cont_Dtl.Add(itemdt);
                            dbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("EMP_TYPE", "No data found for selected criteria");
                        return View("Salary_Gen", emp_Sal);
                    }

                    return RedirectToAction("Emp_Sal_Detail", new { SAL_MONTH = emp_Sal.SAL_MONTH, emmp_type = emp_Sal.EMP_TYPE });
                }
            }
            else
            {
                return View("Salary_Gen", emp_Sal);
            }

        }
        [HttpGet]
        public IActionResult PopSal(int id)
        {
            EmpSalViewModel Emp_Sal_PC_Cont_Dtl = new EmpSalViewModel();
            Emp_Sal emp_Sal = new Emp_Sal();
            List<Emp_Sal_PC_Cont_Dtl> emp_Sal_dtls = new List<Emp_Sal_PC_Cont_Dtl>();
            emp_Sal = dbContext.EMP_SAL.Where(e => e.ID == id).FirstOrDefault();
            Emp_Sal_PC_Cont_Dtl.emp_sal = emp_Sal;
            emp_Sal_dtls = dbContext.Emp_Sal_PC_Cont_Dtl.Where(es => es.EMP_SAL_FK.ToString() == id.ToString()).ToList();

            foreach (var dt in emp_Sal_dtls)
            {
                dt.ART_NAME = dbContext.Artical_Master.Where(a => a.ID == dt.ART_CODE).Select(aa => aa.NAME).FirstOrDefault();
                dt.PROC_NAME = dbContext.Process_Master.Where(a => a.ID == dt.PROC_CODE).Select(aa => aa.NAME).FirstOrDefault();
            }
            Emp_Sal_PC_Cont_Dtl.emp_sal_dtl = emp_Sal_dtls;

            return PartialView("_ModalPopSalary", Emp_Sal_PC_Cont_Dtl);
        }
        [HttpPost]
        public IActionResult PaidSal(int IDD, decimal NET_PAY_SAL, DateTime SAL_MONTH, string EMP_TYPE)
        {
            var result = dbContext.EMP_SAL.Where(a => a.ID == IDD).FirstOrDefault();

            if (result != null)
            {
                result.PAID_DATE = DateTime.Now;
                result.PAID_USER = userManager.GetUserName(HttpContext.User);
                result.PAID_SAL = NET_PAY_SAL;
                dbContext.SaveChanges();
            }
            return RedirectToAction("Emp_Sal_Detail", new { SAL_MONTH = SAL_MONTH, emmp_type = EMP_TYPE });
        }
        [HttpPost]
        public IActionResult CloseSal(DateTime SAL_MONTH, string EMP_TYPE)
        {
            return RedirectToAction("Emp_Sal_Detail", new { SAL_MONTH = SAL_MONTH, emmp_type = EMP_TYPE });
        }
    }
}