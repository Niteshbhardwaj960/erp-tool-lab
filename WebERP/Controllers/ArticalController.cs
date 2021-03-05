using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Identity;
using WebERP.Data;
using WebERP.Models;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebERP.Controllers
{
    public class ArticalController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;
        
        public ArticalController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Artical_Master()
        {
            var Artical_list = dbContext.Artical_Master.ToList();
            foreach (var item in Artical_list)
            {
                item.Brand_Name = dbContext.Brand_Master.Where(s => s.ID == item.BRAND_CODE).Select(s => s.NAME).FirstOrDefault();
            }
            return View(Artical_list);
        }

        [HttpGet]
        public IActionResult AddArtical(int ID, string Type)
        {
            Artical_Master am = new Artical_Master();
            am.brandDropDown = Brandlists();
            return View(am);
        }

        [HttpPost]
        public async Task<IActionResult> SAVEArtical(Artical_Master objArtical)
        {
            var NAME = dbContext.Artical_Master.FirstOrDefault(x => x.NAME == objArtical.NAME);

            if (NAME != null)
            {
                ModelState.AddModelError("NAME", "Name Already Exists.");
            }

            if (ModelState.IsValid)
            {
                objArtical.INS_DATE = DateTime.Now;
                objArtical.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Artical_Master.Add(objArtical);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Artical_Master");
            }
            else
            {
                objArtical.brandDropDown = Brandlists();
                return View("ADDArtical", objArtical);
            }
        }
        [HttpGet]
        public IActionResult ActionArtical(int id)
        {        
            var objArtical = dbContext.Artical_Master.Find(id);
            objArtical.brandDropDown = Brandlists();
            objArtical.Type = "Action";
            return View("EditArtical", objArtical);
        }
        [HttpGet]
        public IActionResult EditArtical(int id)
        {
            var objArtical = dbContext.Artical_Master.Find(id);
            objArtical.brandDropDown = Brandlists();            
            objArtical.Type = "Edit";
            return View("EditArtical", objArtical);
        }

        [HttpPost]
        public IActionResult EditArtical(Artical_Master objArtical)
        {
            if (ModelState.IsValid)
            {
                objArtical.UDT_DATE = DateTime.Now;
                objArtical.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Artical_Master.Update(objArtical);
                dbContext.SaveChanges();
                return RedirectToAction("Artical_Master");
            }
            else
            {
                return View("EditArtical",objArtical);
            }
        }
        [HttpGet]
        public IActionResult DeleteArtical(int ID)
        {
            var data = dbContext.Artical_Master.Find(ID);
            dbContext.Artical_Master.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Artical_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Artical_Master;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-Articals");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "NAME";
                worksheet.Cell(currentRow, 2).Value = "Brand Code";
                worksheet.Cell(currentRow, 3).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 4).Value = "INSERT UID";
                worksheet.Cell(currentRow, 5).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 6).Value = "UPDATE UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.NAME;
                    worksheet.Cell(currentRow, 2).Value = Data.BRAND_CODE;
                    worksheet.Cell(currentRow, 3).Value = Data.INS_DATE;
                    worksheet.Cell(currentRow, 4).Value = Data.INS_UID;
                    worksheet.Cell(currentRow, 5).Value = Data.UDT_DATE;
                    worksheet.Cell(currentRow, 6).Value = Data.UDT_UID;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Aticals.xlsx");
                }
            }
        }
        public List<SelectListItem> Brandlists()
        {
            var BrandList = (from Brand in dbContext.Brand_Master
                             select new SelectListItem()
                             {
                                 Text = Brand.NAME,
                                 Value = Brand.ID.ToString(),
                             }).ToList();

            BrandList.Insert(0, new SelectListItem()
            {
                Text = "Select Brand",
                Value = string.Empty,
                Selected = true
            });
            return BrandList;
        }
    }
}