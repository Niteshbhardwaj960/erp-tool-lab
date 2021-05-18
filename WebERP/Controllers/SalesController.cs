using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebERP.Models;
using System.Diagnostics;
using WebERP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebERP.Helpers;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SalesController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public SalesController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        public IActionResult SalesGrid()
        {
            var SalesGridList = dbContext.SalesHeader.ToList();
            foreach (var item in SalesGridList)
            {
                item.COMP_NAME = dbContext.Companies.
                                    Where(x => x.ID == item.COMP_CODE).
                                    Select(y => y.NAME).FirstOrDefault();
                item.ACC_NAME = dbContext.Account_Masters.
                                        Where(x => x.ID == item.ACC_CODE).
                                        Select(y => y.NAME).FirstOrDefault();
            }
            return View(SalesGridList);
        }

        [HttpGet]
        public IActionResult CreateSalesWithFilter()
        {
            SalesCreateFilter filter = new SalesCreateFilter();
            var goDownList = (from Gdw in dbContext.Godown_Master
                               select new SelectListItem()
                               {
                                   Text = Gdw.NAME,
                                   Value = Convert.ToString(Gdw.ID)
                               }).ToList();

            var ItemList = (from item in dbContext.Item_Master
                              select new SelectListItem()
                              {
                                  Text = item.NAME,
                                  Value = Convert.ToString(item.ID)
                              }).ToList();
            filter.GodownDropDown = goDownList;
            filter.ItemDropDown = ItemList;
            return View(filter);
        }

        [HttpPost]
        public IActionResult CreateSalesWithFilter(SalesCreateFilter filter)
        {
            return RedirectToAction("CreateSaleOrder", "Sales", new
            {
                godownCode = filter.GodownCode,
                itemCode = filter.ItemCode                
            });
        }
       
        [HttpGet]
        public IActionResult CreateSaleOrder(int godownCode, int itemCode)
        {
            SalesViewModel saleViewModel = new SalesViewModel();
            List<V_RM_DTL> viewStockMaster = new List<V_RM_DTL>();
            List<SalesDetail> saleDetailList = new List<SalesDetail>();
            saleViewModel.SalesHeader = GetSaleHeader();
            //viewStockMaster = dbContext.V_RM_DTL.AsNoTracking().
            //Where(x => x.GDW_CODE==godownCode && x.ITEM_CODE==itemCode).ToList();
            //saleDetailList = GetSalesDetails(viewStockMaster);
            saleDetailList.Add(GetSalesDetails());
            saleViewModel.SaleDetails = saleDetailList;
            return View(saleViewModel);
        }
        [HttpPost]
        public ActionResult CreateSaleOrder(SalesViewModel salesModel)
        {
            int saleNewId;
            var maxdocNum = dbContext.SalesHeader
                .Where(x => x.DOC_FINYEAR == salesModel.SalesHeader.DOC_FINYEAR)
                .Select(p => p.DOC_NO).DefaultIfEmpty(0).Max();

            salesModel.SalesHeader.INS_DATE = DateTime.Now;
            salesModel.SalesHeader.INS_UID = userManager.GetUserName(HttpContext.User);
            salesModel.SalesHeader.DOC_NO = maxdocNum + 1;
            salesModel.SalesHeader.DOC_DATE = salesModel.SalesHeader.DOC_DATE.Date;
            dbContext.SalesHeader.Add(salesModel.SalesHeader);
            dbContext.SaveChanges();

            saleNewId = salesModel.SalesHeader.SALE_PK;
            if (saleNewId != 0)
            {
                foreach (var salesDetStatic in salesModel.SaleDetails)
                {
                    salesDetStatic.SALE_FK = saleNewId;
                    salesDetStatic.INS_DATE = DateTime.Now;
                    salesDetStatic.INS_UID = userManager.GetUserName(HttpContext.User);
                }
                foreach (var saleDetailModel in salesModel.SaleDetails)
                {
                    dbContext.SalesDetails.Add(saleDetailModel);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("SalesGrid");
        }

        [HttpGet]
        public IActionResult EditSalesOrder(int id)
        {
            SalesViewModel saleViewModel = new SalesViewModel();
            SalesHeader saleHeader = new SalesHeader();
            var salesHeaderList = dbContext.SalesHeader.Find(id);

            var companyList = (from company in dbContext.Companies
                               select new SelectListItem()
                               {
                                   Text = company.ID.ToString() + " - " + company.NAME,
                                   Value = Convert.ToString(company.ID),
                                   Selected = true
                               }).ToList();

            var accList = (from acc in dbContext.Account_Masters
                           select new SelectListItem()
                           {
                               Text = acc.ID.ToString() + " - " + acc.NAME,
                               Value = Convert.ToString(acc.ID),
                           }).ToList();

            accList.Insert(0, new SelectListItem()
            {
                Text = "Select",
                Value = string.Empty
            });

            var agentacclist = (from acc in dbContext.Account_Masters
                                select new SelectListItem()
                                {
                                    Text = acc.ID.ToString() + " - " + acc.NAME,
                                    Value = Convert.ToString(acc.ID),
                                }).ToList();

            agentacclist.Insert(0, new SelectListItem()
            {
                Text = "NA",
                Value = string.Empty
            });

            var taxList = (from tax in dbContext.TAX_MASTER
                           select new SelectListItem()
                           {
                               Text = tax.TAX_NAME,
                               Value = Convert.ToString(tax.ID)
                           }).ToList();

            taxList.Insert(0, new SelectListItem()
            {
                Text = "Select",
                Value = string.Empty
            });

            foreach (var item in taxList.Where(s => s.Value == Convert.ToString(salesHeaderList.TAX_CODE)))
            {
                item.Selected = true;
            }

            foreach (var item in companyList.Where(s => s.Value == Convert.ToString(salesHeaderList.COMP_CODE)))
            {
                item.Selected = true;
            }

            foreach (var item in accList.Where(s => s.Value == Convert.ToString(salesHeaderList.ACC_CODE)))
            {
                item.Selected = true;
            }

            saleHeader.taxDropDown = taxList;
            saleHeader.agentaccDropDown = agentacclist;
            saleHeader.companyDropDown = companyList;
            saleHeader.accDropDown = accList;
            saleHeader.DOC_DATE = salesHeaderList.DOC_DATE;
            saleHeader.DOC_FINYEAR = salesHeaderList.DOC_FINYEAR;
            saleHeader.DOC_NO = salesHeaderList.DOC_NO;
            saleHeader.REMARKS = salesHeaderList.REMARKS;
            saleHeader.SALE_PK = salesHeaderList.SALE_PK;
            saleHeader.INS_DATE = salesHeaderList.INS_DATE;
            saleHeader.INS_UID = salesHeaderList.INS_UID;
            saleHeader.GROSS_AMT = salesHeaderList.GROSS_AMT;
            saleHeader.RF_AMT = salesHeaderList.RF_AMT;
            saleHeader.IGST_PER = salesHeaderList.IGST_PER;
            saleHeader.IGST_AMOUNT = salesHeaderList.IGST_AMOUNT;
            saleHeader.CGST_PER = salesHeaderList.CGST_PER;
            saleHeader.CGST_AMOUNT = salesHeaderList.CGST_AMOUNT;
            saleHeader.SGST_PER = salesHeaderList.SGST_PER;
            saleHeader.SGST_AMOUNT = salesHeaderList.SGST_AMOUNT;
            saleHeader.TAX_AMT = salesHeaderList.TAX_AMT;
            saleHeader.OTH_AMTNAME1 = salesHeaderList.OTH_AMTNAME1;
            saleHeader.OTH_AMT1 = salesHeaderList.OTH_AMT1;
            saleHeader.OTH_AMTNAME2 = salesHeaderList.OTH_AMTNAME2;
            saleHeader.OTH_AMT2 = salesHeaderList.OTH_AMT2;
            saleHeader.NET_AMT = salesHeaderList.NET_AMT;
            saleViewModel.SalesHeader = saleHeader;
            var saleListDet = dbContext.SalesDetails.Where(l => l.SALE_FK == id).AsNoTracking().ToList();
            decimal grandQTotal = 0;
            foreach (var DetailItem in saleListDet.ToList())
            {
                var salesDet = GetSalesDetails();
                foreach (var item in salesDet.GODOWN_LIST.
                   Where(s => s.Value == Convert.ToString(DetailItem.GODOWN_CODE)))
                {
                    item.Selected = true;
                }
                DetailItem.GODOWN_LIST = salesDet.GODOWN_LIST;
                DetailItem.ITEM_NAME = dbContext.Item_Master.
                                          Where(x => x.ID == DetailItem.ITEM_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                DetailItem.ARTICAL_NAME = dbContext.Artical_Master.
                                          Where(x => x.ID == DetailItem.ARTICAL_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                DetailItem.GODOWN_NAME = dbContext.Godown_Master.
                                          Where(x => x.ID == DetailItem.GODOWN_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                DetailItem.SIZE_NAME = dbContext.Size_Master.
                                          Where(x => x.ID == DetailItem.SIZE_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                DetailItem.SALEQTY_UOM = dbContext.UOM_MASTER.
                                         Where(x => x.ID == DetailItem.SALEQTYCODE).
                                         Select(y => y.NAME).FirstOrDefault();
                grandQTotal += Convert.ToDecimal(DetailItem.SALE_QTY);
            }
            ViewBag.grandQTotal = grandQTotal;
            saleViewModel.SaleDetails = saleListDet;
            return View(saleViewModel);
        }

        [HttpPost]
        public ActionResult EditSalesOrder(SalesViewModel saleEditModel)
        {
            int saleHdrId;
            saleEditModel.SalesHeader.UDT_DATE = DateTime.Now;
            saleEditModel.SalesHeader.UDT_UID = userManager.GetUserName(HttpContext.User);
            dbContext.SalesHeader.Update(saleEditModel.SalesHeader);
            dbContext.SaveChanges();

            saleHdrId = saleEditModel.SalesHeader.SALE_PK;
            if (saleHdrId != 0)
            {
                foreach (var saleDetailUpd in saleEditModel.SaleDetails)
                {
                    saleDetailUpd.SALE_FK = saleHdrId;
                    saleDetailUpd.UDT_DATE = DateTime.Now;
                    saleDetailUpd.UDT_UID = userManager.GetUserName(HttpContext.User);
                }
                foreach (var saleDetailModel in saleEditModel.SaleDetails)
                {
                    dbContext.SalesDetails.Update(saleDetailModel);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("SalesGrid ");
        }

        [HttpGet]
        public IActionResult ShowSalesOrder(int id)
        {
            SalesViewModel saleViewModel = new SalesViewModel();
            SalesHeader saleHeader = new SalesHeader();
            var salesHeaderList = dbContext.SalesHeader.Find(id);

            saleHeader.COMP_NAME = dbContext.Companies.
                                          Where(x => x.ID == salesHeaderList.COMP_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
            saleHeader.ACC_NAME = dbContext.Account_Masters.
                                     Where(x => x.ID == salesHeaderList.ACC_CODE).
                                     Select(y => y.NAME).FirstOrDefault();                  
            saleHeader.DOC_DATE = salesHeaderList.DOC_DATE;
            saleHeader.DOC_FINYEAR = salesHeaderList.DOC_FINYEAR;
            saleHeader.DOC_NO = salesHeaderList.DOC_NO;
            saleHeader.GATEOUT_DATE = salesHeaderList.GATEOUT_DATE;
            saleHeader.GATEOUT_UID = salesHeaderList.GATEOUT_UID;
            saleHeader.REMARKS = salesHeaderList.REMARKS;
            saleHeader.SALE_PK = salesHeaderList.SALE_PK;
            saleHeader.INS_DATE = salesHeaderList.INS_DATE;
            saleHeader.INS_UID = salesHeaderList.INS_UID;
            saleHeader.GROSS_AMT = salesHeaderList.GROSS_AMT;
            saleHeader.RF_AMT = salesHeaderList.RF_AMT;
            saleHeader.IGST_PER = salesHeaderList.IGST_PER;
            saleHeader.IGST_AMOUNT = salesHeaderList.IGST_AMOUNT;
            saleHeader.CGST_PER = salesHeaderList.CGST_PER;
            saleHeader.CGST_AMOUNT = salesHeaderList.CGST_AMOUNT;
            saleHeader.SGST_PER = salesHeaderList.SGST_PER;
            saleHeader.SGST_AMOUNT = salesHeaderList.SGST_AMOUNT;
            saleHeader.TAX_NAME = dbContext.TAX_MASTER.
                                    Where(x => x.ID == salesHeaderList.TAX_CODE).
                                    Select(y => y.TAX_NAME).FirstOrDefault();
            saleHeader.TAX_AMT = salesHeaderList.TAX_AMT;
            saleHeader.OTH_AMTNAME1 = salesHeaderList.OTH_AMTNAME1;
            saleHeader.OTH_AMT1 = salesHeaderList.OTH_AMT1;
            saleHeader.OTH_AMTNAME2 = salesHeaderList.OTH_AMTNAME2;
            saleHeader.OTH_AMT2 = salesHeaderList.OTH_AMT2;
            saleHeader.NET_AMT = salesHeaderList.NET_AMT;
            saleViewModel.SalesHeader = saleHeader;
            var saleListDet = dbContext.SalesDetails.Where(l => l.SALE_FK == id).AsNoTracking().ToList();
            decimal grandQTotal = 0;
            foreach (var DetailItem in saleListDet.ToList())
            {

                DetailItem.ITEM_NAME = dbContext.Item_Master.
                                          Where(x => x.ID == DetailItem.ITEM_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                DetailItem.ARTICAL_NAME = dbContext.Artical_Master.
                                          Where(x => x.ID == DetailItem.ARTICAL_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                DetailItem.GODOWN_NAME = dbContext.Godown_Master.
                                          Where(x => x.ID == DetailItem.GODOWN_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                DetailItem.SIZE_NAME = dbContext.Size_Master.
                                          Where(x => x.ID == DetailItem.SIZE_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                DetailItem.SALEQTY_UOM = dbContext.UOM_MASTER.
                                         Where(x => x.ID == DetailItem.SALEQTYCODE).
                                         Select(y => y.NAME).FirstOrDefault();
                grandQTotal += Convert.ToDecimal(DetailItem.SALE_QTY);
            }
            ViewBag.grandQTotal = grandQTotal;
            saleViewModel.SaleDetails = saleListDet;
            return View(saleViewModel);
        }

        public SalesHeader GetSaleHeader()
        {
            SalesHeader saleHeader = new SalesHeader();
            var companyList = (from company in dbContext.Companies
                               select new SelectListItem()
                               {
                                   Text = company.ID.ToString() + " - " + company.NAME,
                                   Value = Convert.ToString(company.ID),
                                   Selected = true
                               }).ToList();

            var accList = (from acc in dbContext.Account_Masters
                           select new SelectListItem()
                           {
                               Text = acc.ID.ToString() + " - " + acc.NAME,
                               Value = Convert.ToString(acc.ID),
                           }).ToList();

            accList.Insert(0, new SelectListItem()
            {
                Text = "Select",
                Value = string.Empty,
                Selected = true
            });
            var agentacclist = accList;

            agentacclist.Insert(0, new SelectListItem()
            {
                Text = "NA",
                Value = string.Empty,
                Selected = true
            });

            var taxList = (from tax in dbContext.TAX_MASTER
                           select new SelectListItem()
                           {
                               Text = tax.TAX_NAME,
                               Value = Convert.ToString(tax.ID)
                           }).ToList();

            taxList.Insert(0, new SelectListItem()
            {
                Text = "Select",
                Value = string.Empty                
            });
            saleHeader.taxDropDown = taxList;
            saleHeader.agentaccDropDown = agentacclist;
            saleHeader.companyDropDown = companyList;
            saleHeader.accDropDown = accList;
            saleHeader.DOC_DATE = DateTime.Now;
            saleHeader.DOC_FINYEAR = Convert.ToString(Helper.GetFinYear());
            return saleHeader;
        }

        public SalesDetail GetSalesDetails() //List<V_RM_DTL> stockMasterList)
        {
            SalesDetail saleDetailList = new SalesDetail();
            //decimal grandQTotal = 0;
            //foreach (var stock in stockMasterList)
            //{
            //    saleDetailList.Add(new SalesDetail()
            //    {
            //        GODOWN_CODE = stock.GDW_CODE,
            //        ITEM_CODE = stock.ITEM_CODE,
            //        ARTICAL_CODE = stock.ARTICAL_CODE,
            //        SIZE_CODE = stock.SIZE_CODE,
            //        GODOWN_NAME = stock.GDW_NAME,
            //        ITEM_NAME = stock.ITEM_NAME,
            //        ARTICAL_NAME = stock.ARTICAL_NAME,
            //        SIZE_NAME = stock.SIZE_NAME,
            //        SALE_QTY = stock.STK_QTY,

            //    });
            //    grandQTotal += stock.STK_QTY;
            //}           
            //ViewBag.grandQTotal = grandQTotal;           
            var godownList = (from stckView in dbContext.V_RM_DTL
                              select new SelectListItem()
                            {
                                Text = stckView.GDW_NAME,
                                Value = Convert.ToString(stckView.GDW_CODE),
                            }).GroupBy(x => x.Text).Select(x => x.First()).ToList();

            godownList.Insert(0, new SelectListItem()
            {
                Text = "Select",
                Value = string.Empty,
                Selected = true
            });
            saleDetailList.GODOWN_LIST = godownList;            
            return saleDetailList;
        }

        [HttpGet]
        public ActionResult GetTaxList(int code)
        {
            try
            {                
                var taxDetails = dbContext.TAX_MASTER.Find(Convert.ToInt32(code));
                var taxRates = new { igst = taxDetails.IGST_PER,
                                     cgst = taxDetails.CGST_PER,
                                     sgst = taxDetails.SGST_PER };
                return Json(taxRates, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception e)
            {
                string formattedErrMsg = e.Message;
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Json($"<strong>Error! </strong> { formattedErrMsg}", new Newtonsoft.Json.JsonSerializerSettings());
            }

        }

        [HttpGet]
        public ActionResult DeleteSalesOrder(int id)
        {
            if (id > 0)
            {
                var saleHeader = dbContext.SalesHeader.Where(x => x.SALE_PK == id).FirstOrDefault();
                if (saleHeader != null)
                {
                    dbContext.Entry(saleHeader).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                }
                dbContext.SalesDetails.RemoveRange(dbContext.SalesDetails.Where(x => x.SALE_FK == id));
                dbContext.SaveChanges();
            }
            return RedirectToAction("SalesGrid");
        }

        [HttpGet]
        public ActionResult GetVMFilter(string mode,int gc, int ic=0, int ac=0)
        {
            try
            {
                IEnumerable<SelectListItem> itemGetList = new List<SelectListItem>();
                if (mode == "G")
                {
                    itemGetList = (from itemList in dbContext.V_RM_DTL.Where(x => x.GDW_CODE == gc)
                                       select new SelectListItem()
                                       {
                                           Text = itemList.ITEM_NAME,
                                           Value = Convert.ToString(itemList.ITEM_CODE)
                                       }).ToList();                   
                }
                if (mode == "I")
                {
                     itemGetList = (from itemList in dbContext.V_RM_DTL.Where(x => x.GDW_CODE == gc && x.ITEM_CODE == ic)
                                       select new SelectListItem()
                                       {
                                           Text = string.IsNullOrEmpty(itemList.ARTICAL_NAME) ? "NA" : itemList.ARTICAL_NAME,
                                           Value = Convert.ToString(itemList.ARTICAL_CODE)
                                       }).ToList();                    
                }
                if (mode == "A")
                {
                    var viewDet = dbContext.V_RM_DTL.Where(x => x.GDW_CODE == gc && x.ITEM_CODE == ic && x.ARTICAL_CODE == ac).ToList();                                       
                    return Json(new{quant = viewDet.Select(x=>x.STK_QTY).FirstOrDefault(),
                                    size = string.IsNullOrEmpty(viewDet.Select(x => x.SIZE_NAME).FirstOrDefault()) ? "NA" : viewDet.Select(x => x.SIZE_NAME).FirstOrDefault(),
                                    sizec = viewDet.Select(x => x.SIZE_CODE).FirstOrDefault()}, new Newtonsoft.Json.JsonSerializerSettings());
                }
                return Json(itemGetList, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception e)
            {
                string formattedErrMsg = e.Message;
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Json($"<strong>Error! </strong> { formattedErrMsg}", new Newtonsoft.Json.JsonSerializerSettings());
            }

        }
    }
}