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
using Microsoft.AspNetCore.Authorization;
using WebERP.Helpers;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Item , Admin")]
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
            var UOM_list = dbContext.Item_Master.ToList();
            foreach (var item in UOM_list)
            {
                item.UOM_Name = dbContext.UOM_MASTER.Where(s => s.ID == item.UOM_CODE).Select(s => s.NAME).FirstOrDefault();
            }
            return View(UOM_list);
        }
        [HttpGet]
        public IActionResult AddItem()
        {
            Item_Master am = new Item_Master();
            am.UOMDropDown = UOMlists();
            return View(am);
        }
        
        [HttpPost]
        public async Task<IActionResult> SAVEItem(Item_Master objItem)
        {
            var NAME = dbContext.Item_Master.FirstOrDefault(x => x.NAME == objItem.NAME);

            if (NAME != null)
            {
                ModelState.AddModelError("NAME", "Name Already Exists.");
            }
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
                objItem.UOMDropDown = UOMlists();
                return View("AddItem",objItem);
            }
        }
        public IActionResult ActionItem(int id)
        {
            var objItem = dbContext.Item_Master.Find(id);
            objItem.Type = "Action";
            objItem.UOMDropDown = UOMlists();
            return View("EditItem", objItem);
        }
        [HttpGet]
        public IActionResult EditItem(int id)
        {            
            var objItem = dbContext.Item_Master.Find(id);
            objItem.Type = "Edit";
            objItem.UOMDropDown = UOMlists();
            return View(objItem);
        }
        [HttpPost]
        public IActionResult EditItem(Item_Master objItem)
        {
            if (ModelState.IsValid)
            {
                objItem.UDT_DATE = DateTime.Now;
                objItem.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Item_Master.Update(objItem);
                dbContext.SaveChanges();
                return RedirectToAction("Item_Master");
            }
            else
            {
                return View(objItem);
            }
        }
        [HttpGet]
        public IActionResult DeleteItem(int ID)
        {
            var duplPurchaseOrder = dbContext.PODetail_Master.Where(p => p.ITEM_CODE == ID).FirstOrDefault();
            var duplCutting = dbContext.Cutting_Orders.Where(p => p.ITEM_CODE == ID).FirstOrDefault();
         
            var duplJobWork = dbContext.JobWorkIssue_Details.Where(p => p.ITEM_CODE == ID).FirstOrDefault();            
            var duplSaleInv = dbContext.SalesHeader.Where(p => p.ItemCode == ID).FirstOrDefault();

            if (duplJobWork == null && duplCutting == null && duplPurchaseOrder == null && duplSaleInv == null)
            {
                var data = dbContext.Item_Master.Find(ID);
                dbContext.Item_Master.Remove(data);
                dbContext.SaveChanges();
            }
            else
            {
                var ItemMaster = dbContext.Item_Master.ToList();
                ViewBag.Message = string.Format("Can not delete entry. Record present either in Purchase order OR Payment OR JobWork OR Sale Invoice.");
                ViewBag.Color = "red";
                return View("Item_Master", ItemMaster);
            }
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

    }
}