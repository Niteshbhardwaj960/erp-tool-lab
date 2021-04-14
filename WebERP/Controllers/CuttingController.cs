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
    [Authorize(Roles = "Cutting , Admin")]
    public class CuttingController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public CuttingController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }
        [HttpGet]
        public IActionResult CuttingDetail()
        {
            List<V_CuttingDetail> cut = new List<V_CuttingDetail>();
            cut = dbContext.V_CuttingDetail.ToList();
            return View(cut);
        }
        public int GetFinYear()
        {
            string FinYear = "";
            DateTime date = DateTime.Now;
            if ((date.Month) == 1 || (date.Month) == 2 || (date.Month) == 3)
            {
                FinYear = (date.Year - 1) + "" + date.Year;
            }
            else
            {
                FinYear = date.Year + "" + (date.Year + 1);
            }
            return Convert.ToInt32(FinYear);
        }
        [HttpGet]
        public IActionResult AddCutting()
        {
            Cutting_Order cut = new Cutting_Order();
            cut.DOC_DATE = DateTime.Now;
            cut.DOC_FINYEAR = GetFinYear();
            cut.EmpDropDown = Emplists();
            int DoC_No = dbContext.Cutting_Orders
                .Select(p => Convert.ToInt32(p.DOC_NO)).DefaultIfEmpty(0).Max();
            cut.ArticalDropDown = Articallists();
            cut.ContEmpDropDown = ContEmplists();
            cut.ItemDropDown = Itemlists();
            cut.ProcDropDown = Proclists();
            cut.SizeDropDown = Sizelists();
            cut.Type = "Add";
            cut.DOC_NO = DoC_No + 1;
            return View(cut);
        }

        [HttpPost]
        public IActionResult SaveCutting(Cutting_Order CuttingOrder)
        {
            if (ModelState.IsValid)
            {
                int DoC_No = dbContext.Cutting_Orders
                .Select(p => Convert.ToInt32(p.DOC_NO)).DefaultIfEmpty(0).Max();
                CuttingOrder.INS_DATE = DateTime.Now;
                CuttingOrder.INS_UID = userManager.GetUserName(HttpContext.User);
                CuttingOrder.DOC_NO = DoC_No + 1;
                dbContext.Cutting_Orders.Add(CuttingOrder);
                dbContext.SaveChanges();
                return RedirectToAction("CuttingDetail");
            }
            else
            {
                CuttingOrder.EmpDropDown = Emplists();
                CuttingOrder.ArticalDropDown = Articallists();
                CuttingOrder.ContEmpDropDown = ContEmplists();
                CuttingOrder.ItemDropDown = Itemlists();
                CuttingOrder.ProcDropDown = Proclists();
                CuttingOrder.SizeDropDown = Sizelists();
                return View("AddCutting", CuttingOrder);
            }
        }

        [HttpGet]
        public IActionResult ActionCutting(int id)
        {
            Cutting_Order cut = new Cutting_Order();
            cut = dbContext.Cutting_Orders.Find(id);
            cut.EmpDropDown = Emplists();
            cut.ArticalDropDown = Articallists();
            cut.ContEmpDropDown = ContEmplists();
            cut.ItemDropDown = Itemlists();
            cut.ProcDropDown = Proclists();
            cut.SizeDropDown = Sizelists();
            cut.Type = "Action";
            return View("AddCutting", cut);
        }
        [HttpGet]
        public IActionResult EditCutting(int id)
        {
            Cutting_Order cut = new Cutting_Order();
            cut = dbContext.Cutting_Orders.Find(id);
            cut.EmpDropDown = Emplists();
            cut.ArticalDropDown = Articallists();
            cut.ContEmpDropDown = ContEmplists();
            cut.ItemDropDown = Itemlists();
            cut.ProcDropDown = Proclists();
            cut.SizeDropDown = Sizelists();
            cut.Type = "Edit";
            return View("AddCutting", cut);
        }

        [HttpPost]
        public IActionResult EditCutting(Cutting_Order obj)
        {
            if (ModelState.IsValid)
            {
                obj.UDT_DATE = DateTime.Now;
                obj.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Cutting_Orders.Update(obj);
                dbContext.SaveChanges();
                return RedirectToAction("CuttingDetail");
            }
            else
            {
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult DeleteCutting(int ID)
        {
            var data = dbContext.Cutting_Orders.Find(ID);
            dbContext.Cutting_Orders.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("CuttingDetail");
        }
        //[HttpGet]
        //public IActionResult Excel()
        //{
        //    var ComData = dbContext.Employee_Masters;

        //    using (var workbook = new XLWorkbook())
        //    {
        //        var worksheet = workbook.Worksheets.Add("List-Employee");
        //        var currentRow = 1;
        //        worksheet.Cell(currentRow, 1).Value = "Employee Code";
        //        worksheet.Cell(currentRow, 2).Value = "Employee NAME";
        //        worksheet.Cell(currentRow, 3).Value = "Department";
        //        worksheet.Cell(currentRow, 4).Value = "Employee Type";
        //        worksheet.Cell(currentRow, 5).Value = "Employee Father's Name";
        //        worksheet.Cell(currentRow, 6).Value = "Employee Mobile No";
        //        worksheet.Cell(currentRow, 7).Value = "Employee Phone No";
        //        worksheet.Cell(currentRow, 8).Value = "Employee DOJ";
        //        worksheet.Cell(currentRow, 9).Value = "Employee Salary";
        //        worksheet.Cell(currentRow, 10).Value = "Active Tag";
        //        worksheet.Cell(currentRow, 11).Value = "Remarks";
        //        worksheet.Cell(currentRow, 12).Value = "INSERT DATE";
        //        worksheet.Cell(currentRow, 13).Value = "INSERT UID";
        //        worksheet.Cell(currentRow, 14).Value = "UPDATE DATE";
        //        worksheet.Cell(currentRow, 15).Value = "UPDATE UID";

        //        foreach (var Data in ComData)
        //        {
        //            currentRow++;
        //            string EMPTYPE = "";
        //            string Active = "";
        //            if (Data.EMP_TYPE == "S")
        //            {
        //                EMPTYPE = "Salary";
        //            }
        //            if (Data.EMP_TYPE == "P")
        //            {
        //                EMPTYPE = "Pc Rate";
        //            }
        //            if (Data.EMP_TYPE == "C")
        //            {
        //                EMPTYPE = "Contractor";
        //            }
        //            if (Data.active_tag == "1")
        //            {
        //                Active = "Yes";
        //            }
        //            else
        //            {
        //                Active = "No";
        //            }
        //            worksheet.Cell(currentRow, 1).Value = Data.EMP_CODE;
        //            worksheet.Cell(currentRow, 2).Value = Data.EMP_NAME;
        //            worksheet.Cell(currentRow, 3).Value = Data.DEP_CODE;
        //            worksheet.Cell(currentRow, 4).Value = EMPTYPE;
        //            worksheet.Cell(currentRow, 5).Value = Data.Emp_Father_Name;
        //            worksheet.Cell(currentRow, 6).Value = Data.emp_mobile_no1;
        //            worksheet.Cell(currentRow, 7).Value = Data.emp_mobile_no2;
        //            worksheet.Cell(currentRow, 8).Value = Data.emp_doj;
        //            worksheet.Cell(currentRow, 9).Value = Data.emp_salary;
        //            worksheet.Cell(currentRow, 10).Value = Active;
        //            worksheet.Cell(currentRow, 11).Value = Data.Remarks;
        //            worksheet.Cell(currentRow, 12).Value = Data.INS_DATE;
        //            worksheet.Cell(currentRow, 13).Value = Data.INS_UID;
        //            worksheet.Cell(currentRow, 14).Value = Data.UDT_DATE;
        //            worksheet.Cell(currentRow, 15).Value = Data.UDT_UID;
        //        }

        //        using (var stream = new MemoryStream())
        //        {
        //            workbook.SaveAs(stream);
        //            var content = stream.ToArray();

        //            return File(
        //                content,
        //                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        //                "Employee.xlsx");
        //        }
        //    }
        //}

        public List<SelectListItem> Emplists()
        {
            var EmpList = (from Emp in dbContext.Employee_Masters.Where(e => e.EMP_TYPE == "P").ToList()
                           select new SelectListItem()
                           {
                               Text = Emp.EMP_NAME,
                               Value = Convert.ToString(Emp.EMP_CODE),
                           }).ToList();

            EmpList.Insert(0, new SelectListItem()
            {
                Text = "Select Employee",
                Value = string.Empty,
                Selected = true
            });
            return EmpList;
        }
        public List<SelectListItem> ContEmplists()
        {
            var EmpList = (from Emp in dbContext.Employee_Masters.Where(e => e.EMP_TYPE == "C").ToList()
                           select new SelectListItem()
                           {
                               Text = Emp.EMP_NAME,
                               Value = Convert.ToString(Emp.EMP_CODE),
                           }).ToList();

            EmpList.Insert(0, new SelectListItem()
            {
                Text = "NA",
                Value = "0",
                Selected = true
            });
            return EmpList;
        }
        public List<SelectListItem> Itemlists()
        {
            var ItemList = (from Item in dbContext.Item_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = Item.NAME,
                               Value = Convert.ToString(Item.ID),
                           }).ToList();

            ItemList.Insert(0, new SelectListItem()
            {
                Text = "Select Item",
                Value = string.Empty,
                Selected = true
            });
            return ItemList;
        }
        public List<SelectListItem> Articallists()
        {
            var ArtList = (from Art in dbContext.Artical_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = Art.NAME,
                               Value = Convert.ToString(Art.ID),
                           }).ToList();

            ArtList.Insert(0, new SelectListItem()
            {
                Text = "Select Artical",
                Value = string.Empty,
                Selected = true
            });
            return ArtList;
        }
        public List<SelectListItem> Sizelists()
        {
            var SizeList = (from Size in dbContext.Size_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = Size.NAME,
                               Value = Convert.ToString(Size.ID),
                           }).ToList();

            SizeList.Insert(0, new SelectListItem()
            {
                Text = "Select Size",
                Value = string.Empty,
                Selected = true
            });
            return SizeList;
        }
        public List<SelectListItem> Proclists()
        {
            var ProcList = (from Proc in dbContext.Process_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = Proc.NAME,
                               Value = Convert.ToString(Proc.ID),
                           }).ToList();

            ProcList.Insert(0, new SelectListItem()
            {
                Text = "Select Process",
                Value = string.Empty,
                Selected = true
            });
            return ProcList;
        }
    }
}