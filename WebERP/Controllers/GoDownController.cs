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
using WebERP.Models.GateEntry;

namespace WebERP.Controllers
{
    [Authorize(Roles = "GoDown , Admin")]
    public class GoDownController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public GoDownController(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public IActionResult GoDown_Master()
        {
            List<Godown_Master> gd = new List<Godown_Master>();
            gd = dbContext.Godown_Master.ToList();
            foreach (var obj in gd)
            {
                if (obj.SALE_TAG == "0")
                {
                    obj.SALE_TAG = "Yes";
                }
                else
                {
                    obj.SALE_TAG = "No";
                }
                if (obj.GO_DOWN_TYPE == "1") { obj.GO_DOWN_TYPE = "Raw Material"; }
                else if (obj.GO_DOWN_TYPE == "2") { obj.GO_DOWN_TYPE = "Semi Finished"; }
                else if (obj.GO_DOWN_TYPE == "3") { obj.GO_DOWN_TYPE = "Finished"; }
                else if (obj.GO_DOWN_TYPE == "4") { obj.GO_DOWN_TYPE = "General"; }
                else if (obj.GO_DOWN_TYPE == "5") { obj.GO_DOWN_TYPE = "Waste"; }
                else if (obj.GO_DOWN_TYPE == "6") { obj.GO_DOWN_TYPE = "Outside"; }
                else if (obj.GO_DOWN_TYPE == "7") { obj.GO_DOWN_TYPE = "Consumeable"; }
            }
            return View(gd);
        }
        [HttpGet]
        public IActionResult Add_GoDown()
        {
            Godown_Master obj = new Godown_Master();
            obj.Type = "Add";
            return View("Add_GoDown", obj);
        }
        [HttpPost]
        public async Task<IActionResult> SAVEGoDown(Godown_Master objGoDown)
        {
            var NAME = dbContext.Godown_Master.FirstOrDefault(x => x.NAME == objGoDown.NAME);

            if (NAME != null)
            {
                ModelState.AddModelError("NAME", "Name Already Exists.");
            }
            if (ModelState.IsValid)
            {
                objGoDown.INS_DATE = DateTime.Now;
                objGoDown.INS_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Godown_Master.Add(objGoDown);
                var result = await dbContext.SaveChangesAsync();
                return RedirectToAction("GoDown_Master");
            }
            else
            {
                objGoDown.Type = "Add";
                return View("Add_GoDown", objGoDown);
            }
        }
        [HttpGet]
        public IActionResult ActionGoDown(int id)
        {
            Godown_Master obj = new Godown_Master();
            obj = dbContext.Godown_Master.Find(id);
            obj.Type = "Action";
            dbContext.Godown_Master.Update(obj);
            dbContext.SaveChanges();
            return View("Add_GoDown", obj);
        }
        [HttpGet]
        public IActionResult EditGoDown(int id)
        {
            Godown_Master obj = new Godown_Master();
            obj = dbContext.Godown_Master.Find(id);
            obj.Type = "Edit";
            dbContext.Godown_Master.Update(obj);
            dbContext.SaveChanges();
            return View("Add_GoDown", obj);
        }

        [HttpPost]
        public IActionResult EditGoDown(Godown_Master obj)
        {
            if (ModelState.IsValid)
            {
                obj.UDT_DATE = DateTime.Now;
                obj.UDT_UID = userManager.GetUserName(HttpContext.User);
                dbContext.Godown_Master.Update(obj);
                dbContext.SaveChanges();
                return RedirectToAction("Godown_Master");
            }
            else
            {
                return View(obj);
            }
        }
        [HttpGet]
        public IActionResult DeleteGoDown(int ID)
        {
            var data = dbContext.Godown_Master.Find(ID);
            dbContext.Godown_Master.Remove(data);
            dbContext.SaveChanges();
            return RedirectToAction("Godown_Master");
        }
        [HttpGet]
        public IActionResult Excel()
        {
            var ComData = dbContext.Godown_Master;

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("List-GoDowns");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "NAME";
                worksheet.Cell(currentRow, 2).Value = "INSERT DATE";
                worksheet.Cell(currentRow, 3).Value = "INSERT UID";
                worksheet.Cell(currentRow, 4).Value = "UPDATE DATE";
                worksheet.Cell(currentRow, 5).Value = "UPDATE UID";

                foreach (var Data in ComData)
                {
                    currentRow++;
                    worksheet.Cell(currentRow, 1).Value = Data.NAME;
                    worksheet.Cell(currentRow, 2).Value = Data.INS_DATE;
                    worksheet.Cell(currentRow, 3).Value = Data.INS_UID;
                    worksheet.Cell(currentRow, 4).Value = Data.UDT_DATE;
                    worksheet.Cell(currentRow, 5).Value = Data.UDT_UID;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "GoDowns.xlsx");
                }
            }
        }

        [HttpGet]
        public IActionResult GoDownDetails()
        {
            List<StockDTL_Model> STK = new List<StockDTL_Model>();
            STK = dbContext.StockDTL_Models.ToList();
            return View(STK);
        }

        [HttpGet]
        public IActionResult GoDownStock()
        {
            Models.EditGateEntryModel GED = new Models.EditGateEntryModel();           
            GED.GoDownDropDown = GoDownList();
            GED.EditGateEntryDetails = dbContext.gateEntryDetails.Where(G => G.GDW_NO == 0).ToList();
            foreach( var entry in GED.EditGateEntryDetails)
            {
                var itemName = dbContext.Item_Master.Where(e => e.ID == entry.Item_Name).Select(ee => ee.NAME).FirstOrDefault();

                entry.ITEM_NAMEs = itemName;
            }
            return View(GED);
        }
        public List<SelectListItem> GoDownList()
        {
            var GDWList = (from GDW in dbContext.Godown_Master.ToList()
                           select new SelectListItem()
                           {
                               Text = GDW.NAME,
                               Value = Convert.ToString(GDW.ID),
                           }).ToList();

            //GDWList.Insert(0, new SelectListItem()
            //{
            //    Text = "Select Godown",
            //    Value = string.Empty,
            //    Selected = true
            //});
            return GDWList;
        }

        [HttpPost]
        public IActionResult GoDownStock(EditGateEntryModel EditGateEntryModels, string GDWCODE)
        {
            //if (GDWCODE == null)
            //{
            //    TempData["err"] = "Please Select Godown ";
            //    return RedirectToAction("GoDownStock");
            //}
            //else {
            //    //if (ModelState.IsValid)
            //    //{
                List<StockDTL_Model> StkDTL = new List<StockDTL_Model>();
                List<int> ID = new List<int>();
                foreach (var stk in EditGateEntryModels.EditGateEntryDetails)
                {
                    if (stk.CHK == true)
                    {
                        StkDTL.Add(new StockDTL_Model()
                        {
                            INS_DATE = DateTime.Now,
                            INS_UID = userManager.GetUserName(HttpContext.User),
                            COMP_CODE = 0,
                            Tran_Table = "Gate Entry",
                            Tran_Table_PK = stk.ID,
                            GDW_CODE = Convert.ToInt32(GDWCODE),
                            Item_Code = stk.Item_Name,
                            Artical_CODE = 0,
                            Size_Code = 0,
                            Stk_Qty_IN = stk.Stk_Qty,
                            Stk_Qty_OUT = 0
                        });
                        ID.Add(stk.ID);
                    }
                }
                foreach (var item in StkDTL)
                {
                    dbContext.StockDTL_Models.Add(item);
                    dbContext.SaveChanges();
                }
                foreach (var item in ID)
                {
                    var result = dbContext.gateEntryDetails.SingleOrDefault(b => b.ID == item);
                    if (result != null)
                    {
                        result.GDW_NO = Convert.ToInt32(GDWCODE);
                        dbContext.SaveChanges();
                    }                                      
                }
                return RedirectToAction("GoDownStock");
            //}
        }
    }
}