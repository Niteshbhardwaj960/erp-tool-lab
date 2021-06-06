using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebERP.Data;
using WebERP.Helpers;
using WebERP.Models;

namespace WebERP.Controllers
{
    [Authorize(Roles = "ProcessRate , Admin")]
    public class ProcessRateController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public ProcessRateController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult ProcessRate_Master()
        {
            var UOM_list = dbContext.ProcessRate_Master.ToList();
            foreach (var item in UOM_list)
            {
                item.UOM_Name = dbContext.UOM_MASTER.Where(s => s.ID == Convert.ToInt64(item.UOM_Code)).Select(s => s.NAME).FirstOrDefault();
                item.Artical_Name = dbContext.Artical_Master.Where(s => s.ID == Convert.ToInt64(item.Artical_Code)).Select(s => s.NAME).FirstOrDefault();
                item.Proc_Name = dbContext.Process_Master.Where(s => s.ID == Convert.ToInt64(item.Proc_Code)).Select(s => s.NAME).FirstOrDefault();
            }
            return View(UOM_list);
        }
        [HttpGet]
        public IActionResult AddProcessRate()
        {
            ProcessRate_Master obj = new ProcessRate_Master();
            obj.UOMDropDown = UOMlists();
            obj.ArticalDropDown = Articallists();
            obj.ProcDropDown = Processlists();
            obj.Type = "Add";
            return View("AddProcessRate", obj);
        }
        [HttpPost]
        public async Task<IActionResult> SAVEProcessRate(ProcessRate_Master objProcessRate)
        {
            var NAME = dbContext.ProcessRate_Master.FirstOrDefault(x => x.Proc_Code == objProcessRate.Proc_Code);

            if (NAME != null)
            {
                ModelState.AddModelError("Proc_Code", "Proc Name Already Exists.");
            }
            if (ModelState.IsValid)
            {
                objProcessRate.INS_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                objProcessRate.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.ProcessRate_Master.Add(objProcessRate);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("ProcessRate_Master");
            }
            else
            {
                objProcessRate.Type = "Add";
                objProcessRate.UOMDropDown = UOMlists();
                return View("ADDProcessRate",objProcessRate);
            }
        }
        [HttpGet]
        public IActionResult ActionProcessRate(int id)
        {
            var obj = dbContext.ProcessRate_Master.Find(id);
            obj.Type = "Action";
            obj.UOMDropDown = UOMlists();
            obj.ArticalDropDown = Articallists();
            obj.ProcDropDown = Processlists();
            return View("AddProcessRate", obj);
        }
        [HttpGet]
        public IActionResult EditProcessRate(int id)
        {
           var obj = dbContext.ProcessRate_Master.Find(id);
            obj.Type = "Edit";
            obj.UOMDropDown = UOMlists();
            obj.ArticalDropDown = Articallists();
            obj.ProcDropDown = Processlists();
            return View("AddProcessRate", obj);
        }

        [HttpPost]
        public IActionResult EditProcessRate(ProcessRate_Master obj)
        {
            if (ModelState.IsValid)
            {
                obj.UDT_DATE = Helper.DateFormatDate(Convert.ToString(DateTime.Now));
                obj.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.ProcessRate_Master.Update(obj);
                dbContext.SaveChanges();
                return RedirectToAction("ProcessRate_Master");
            }
            else
            {
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult DeleteProcessRate(int ID)
        {
            var data = dbContext.ProcessRate_Master.Find(ID);
            dbContext.ProcessRate_Master.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("ProcessRate_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.ProcessRate_Master;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-ProcessRate");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Process Name";
                worksheet.Cell(currentRow, 2).Value = "Rate";
                worksheet.Cell(currentRow, 3).Value = "UOM Name";
                worksheet.Cell(currentRow, 4).Value = "From Date";
                worksheet.Cell(currentRow, 5).Value = "To Date";
                worksheet.Cell(currentRow, 6).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 7).Value = "INSERT UID";
                worksheet.Cell(currentRow, 8).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 9).Value = "UPDATE UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.Proc_Code;
                    worksheet.Cell(currentRow, 2).Value = Data.Rate;
                    worksheet.Cell(currentRow, 3).Value = Data.UOM_Code;
                    worksheet.Cell(currentRow, 4).Value = Data.From_DATE;
                    worksheet.Cell(currentRow, 5).Value = Data.To_DATE;
                    worksheet.Cell(currentRow, 6).Value = Data.INS_DATE;
                    worksheet.Cell(currentRow, 7).Value = Data.INS_UID;
                    worksheet.Cell(currentRow, 8).Value = Data.UDT_DATE;
                    worksheet.Cell(currentRow, 9).Value = Data.UDT_UID;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "ProcessRate.xlsx");
                }
            }
        }
        public List<SelectListItem> UOMlists()
        {
            var UOMList = (from UOM in dbContext.UOM_MASTER
                           select new SelectListItem()
                           {
                               Text = UOM.NAME,
                               Value = UOM.ID.ToString(),
                           }).ToList();

            UOMList.Insert(0, new SelectListItem()
            {
                Text = "Select UOM",
                Value = string.Empty,
                Selected = true
            });
            return UOMList;
        }
        public List<SelectListItem> Processlists()
        {
            var ProcessList = (from UOM in dbContext.Process_Master
                           select new SelectListItem()
                           {
                               Text = UOM.NAME,
                               Value = UOM.ID.ToString(),
                           }).ToList();

            ProcessList.Insert(0, new SelectListItem()
            {
                Text = "Select Process",
                Value = string.Empty,
                Selected = true
            });
            return ProcessList;
        }
        public List<SelectListItem> Articallists()
        {
            var ArticalList = (from UOM in dbContext.Artical_Master
                               select new SelectListItem()
                               {
                                   Text = UOM.NAME,
                                   Value = UOM.ID.ToString(),
                               }).ToList();

            ArticalList.Insert(0, new SelectListItem()
            {
                Text = "Select Artical",
                Value = string.Empty,
                Selected = true
            });
            return ArticalList;
        }
    }
}