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

namespace WebERP.Controllers
{
    public class ItemController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public ItemController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult Item_Master()
        {
            return View(dbContext.Item_Master.ToList());
        }
        [HttpGet]
        public IActionResult AddItem()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SAVEItem(Item_Master objItem)
        {
            if (ModelState.IsValid)
            {
                objItem.INS_DATE = DateTime.Now;
                objItem.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Item_Master.Add(objItem);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("Item_Master");
            }
            else
            {
                return View("Item_Master");
            }
        }
        [HttpGet]
        public IActionResult EditItem(int id)
        {
            return View(dbContext.Item_Master.Find(id));
        }

        [HttpPost]
        public IActionResult EditItem(Item_Master objItem)
        {
            objItem.UDT_DATE = DateTime.Now;
            objItem.UDT_UID = userManager.GetUserName(HttpContext.User);
            dbContext.Item_Master.Update(objItem);
            dbContext.SaveChanges();
            return RedirectToAction("Item_Master");

        }
        [HttpGet]
        public IActionResult DeleteItem(int ID)
        {
            var data = dbContext.Item_Master.Find(ID);
            dbContext.Item_Master.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Item_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Item_Master;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-Items");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "NAME";
                worksheet.Cell(currentRow, 2).Value = "UOM CODE";
                worksheet.Cell(currentRow, 2).Value = "HSN CODE";
                worksheet.Cell(currentRow, 2).Value = "MIN STOCK";
                worksheet.Cell(currentRow, 2).Value = "MAX STOCK";
                worksheet.Cell(currentRow, 2).Value = "ACTIVE TAG";
                worksheet.Cell(currentRow, 2).Value = "REMARKS";
                worksheet.Cell(currentRow, 3).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 4).Value = "INSERT UID";
                worksheet.Cell(currentRow, 5).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 6).Value = "UPDATE UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.NAME;
                    worksheet.Cell(currentRow, 2).Value = Data.UOM_CODE;
                    worksheet.Cell(currentRow, 2).Value = Data.HSN_CODE;
                    worksheet.Cell(currentRow, 2).Value = Data.MIN_STOCK;
                    worksheet.Cell(currentRow, 2).Value = Data.MAX_STOCK;
                    worksheet.Cell(currentRow, 2).Value = Data.ACTIVE_TAG;
                    worksheet.Cell(currentRow, 2).Value = Data.REMARKS;
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
                        "Items.xlsx");
                }
            }
        }
    }
}