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

    [Authorize(Roles = "Admin , PO")]
    public class POController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public POController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        [HttpGet]
        public ActionResult PODetails()
        {
            POViewModel poViewModel = new POViewModel();
            List<PODetailModel> poDetailList = new List<PODetailModel>();
            poViewModel.POHeader = GetPOHeader();
            poViewModel.POTerms = GetPOTerm();
            poDetailList.Add(GetPODetails());
            poViewModel.PODetails = poDetailList;
            return View(poViewModel);
        }

        [HttpPost]
        public ActionResult PODetails(POViewModel poViewModel)
        {
            int poNewId;
            var maxOrderNum = dbContext.POHeader_Master
                .Where(x => x.ORDER_FINYEAR == poViewModel.POHeader.ORDER_FINYEAR)
                .Select(p => p.ORDER_NO).DefaultIfEmpty(0).Max();

            poViewModel.POHeader.INS_DATE = DateTime.Now;
            poViewModel.POHeader.INS_UID = userManager.GetUserName(HttpContext.User);
            poViewModel.POHeader.ORDER_NO = maxOrderNum + 1;
            dbContext.POHeader_Master.Add(poViewModel.POHeader);
            dbContext.SaveChanges();

            poNewId = poViewModel.POHeader.POH_PK;
            if (poNewId != 0)
            {
                foreach (var pODetailStaticUpd in poViewModel.PODetails)
                {
                    pODetailStaticUpd.POH_FK = poNewId;
                    pODetailStaticUpd.INS_DATE = DateTime.Now;
                    pODetailStaticUpd.INS_UID = userManager.GetUserName(HttpContext.User);
                    pODetailStaticUpd.APPROVED_DATE = DateTime.Now;
                    pODetailStaticUpd.APPROVED_UID = "N/A";
                }

                foreach (var pOTermsStaticUpd in poViewModel.POTerms)
                {
                    pOTermsStaticUpd.POH_FK = poNewId;
                    pOTermsStaticUpd.INS_DATE = DateTime.Now;
                    pOTermsStaticUpd.INS_UID = userManager.GetUserName(HttpContext.User);
                }

                foreach (var pODetailModel in poViewModel.PODetails)
                {
                    dbContext.PODetail_Master.Add(pODetailModel);
                    dbContext.SaveChanges();
                }

                foreach (var pOTermsModel in poViewModel.POTerms)
                {
                    dbContext.POTerm_Master.Add(pOTermsModel);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("POGridDetails");
        }

        public List<POTermsModel> GetPOTerm()
        {
            List<POTermsModel> termsModels = new List<POTermsModel>();

            var termListDropDown = (from term in dbContext.Term_Master.Where(x => x.ACTIVE_TAG == "1")
                          .Where(y => y.PO == "1").ToList()
                                    select new SelectListItem()
                                    {
                                        Text = term.NAME,
                                        Value = Convert.ToString(term.ID)
                                    }).ToList();

            termListDropDown.Insert(0, new SelectListItem()
            {
                Text = "Select Terms",
                Value = string.Empty,
                Selected = true
            });
            termsModels.Add(new POTermsModel() { termDropDown = termListDropDown, REMARKS = "" });
            return termsModels;

        }

        [HttpGet]
        public ActionResult GetQtyUOMList(int itemCode)
        {
            try
            {
                var itemDetails = dbContext.Item_Master.Find(Convert.ToInt32(itemCode));
                var QtyUOMList = (from uomList in dbContext.UOM_MASTER.ToList()
                                  select new SelectListItem()
                                  {
                                      Text = uomList.ABV,
                                      Value = uomList.ID.ToString()                                      
                                  }).ToList();

                foreach (var item in QtyUOMList.Where(s => s.Value == Convert.ToString(itemDetails.UOM_CODE)))
                {
                    item.Selected = true;
                }
                return Json(QtyUOMList, new Newtonsoft.Json.JsonSerializerSettings());
            }
            catch (Exception e)
            {
                string formattedErrMsg = e.Message;
                Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;
                return Json($"<strong>Error! </strong> { formattedErrMsg}", new Newtonsoft.Json.JsonSerializerSettings());
            }

        }

        public POHeaderModel GetPOHeader()
        {
            POHeaderModel poHeader = new POHeaderModel();
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
            poHeader.companyDropDown = companyList;
            poHeader.accDropDown = accList;
            poHeader.ORDER_DATE = DateTime.Now;
            poHeader.ORDER_FINYEAR = Convert.ToString(Helper.GetFinYear());
            return poHeader;
        }

        public PODetailModel GetPODetails()
        {
            PODetailModel poDetail = new PODetailModel();
            var itemList = (from items in dbContext.Item_Master
                            select new SelectListItem()
                            {
                                Text = items.NAME,
                                Value = Convert.ToString(items.ID),
                            }).ToList();

            itemList.Insert(0, new SelectListItem()
            {
                Text = "Select",
                Value = string.Empty,
                Selected = true
            });
            poDetail.GetItems = itemList;
            poDetail.DELV_DATE = DateTime.Now;
            return poDetail;
        }

        [HttpGet]
        public ActionResult POGridDetails()
        {
            var POGridList = dbContext.POHeader_Master.ToList();
            foreach (var item in POGridList)
            {
                item.COMP_NAME = dbContext.Companies.
                                    Where(x => x.ID == item.COMP_CODE).
                                    Select(y => y.NAME).FirstOrDefault();
                item.ACC_NAME = dbContext.Account_Masters.
                                        Where(x => x.ID == item.ACC_CODE).
                                        Select(y => y.NAME).FirstOrDefault();
            }
            return View(POGridList);
        }

        [HttpGet]
        public ActionResult ShowPODetails(int id)
        {
            POViewModel poViewModel = new POViewModel();
            List<PODetailModel> poDetailList = new List<PODetailModel>();
            List<POTermsModel> poTermList = new List<POTermsModel>();

            #region Header Fill
            var POHeaderList = dbContext.POHeader_Master.Find(id);
            POHeaderList.COMP_NAME = dbContext.Companies.
                                    Where(x => x.ID == POHeaderList.COMP_CODE).
                                    Select(y => y.NAME).FirstOrDefault();
            POHeaderList.ACC_NAME = dbContext.Account_Masters.
                                    Where(x => x.ID == POHeaderList.ACC_CODE).
                                    Select(y => y.NAME).FirstOrDefault();
            poViewModel.POHeader = POHeaderList;
            #endregion

            #region Detail Fill
            double grandTotal = 0;
            decimal grandQTotal = 0;
            var PODetailList = dbContext.PODetail_Master.Where(x => x.POH_FK == id).ToList();
            foreach (var poDetailModel in PODetailList)
            {
                poDetailModel.ITEM_NAME = dbContext.Item_Master.
                                           Where(x => x.ID == poDetailModel.ITEM_CODE).
                                           Select(y => y.NAME).FirstOrDefault();
                poDetailModel.RATEUOM_NAME = dbContext.UOM_MASTER.
                                           Where(x => x.ID == poDetailModel.RATE_UOM).
                                           Select(y => y.NAME).FirstOrDefault();
                poDetailModel.QTYUOMNAME = dbContext.UOM_MASTER.
                                           Where(x => x.ID == Convert.ToInt32(poDetailModel.QTY_UOM)).
                                           Select(y => y.NAME).FirstOrDefault();
                poDetailModel.AMOUNT = Convert.ToString(
                                        Convert.ToDouble(poDetailModel.QTY) *
                                        Convert.ToDouble(poDetailModel.NET_RATE));
                grandQTotal += Convert.ToDecimal(poDetailModel.QTY);
                grandTotal += Convert.ToDouble(poDetailModel.AMOUNT);
                if (poDetailModel.POD_PK_STATUS == "A")
                    poDetailModel.POD_PK_STATUS = "Active";
                if (poDetailModel.POD_PK_STATUS == "F")
                    poDetailModel.POD_PK_STATUS = "Finished";
                if (poDetailModel.POD_PK_STATUS == "C")
                    poDetailModel.POD_PK_STATUS = "Cancelled";
                poDetailList.Add(poDetailModel);
            }
            ViewBag.pograndTotal = grandTotal;
            ViewBag.pograndQTotal = grandQTotal;
            poViewModel.PODetails = poDetailList;
            #endregion

            #region Term Fill
            var POTermList = dbContext.POTerm_Master.Where(x => x.POH_FK == id).ToList();
            foreach (var poTermModel in POTermList)
            {
                poTermModel.TERMS_NAME = dbContext.Term_Master.
                                           Where(x => x.ID == poTermModel.TERMS_CODE).
                                           Select(y => y.NAME).FirstOrDefault();

                poTermList.Add(poTermModel);
            }
            poViewModel.POTerms = poTermList;
            #endregion            

            return View(poViewModel);
        }

        public ActionResult DeletePODetails(int id)
        {
            if (id > 0)
            {
                var pOHeader = dbContext.POHeader_Master.Where(x => x.POH_PK == id).FirstOrDefault();
                if (pOHeader != null)
                {
                    dbContext.Entry(pOHeader).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                }
                dbContext.PODetail_Master.RemoveRange(dbContext.PODetail_Master.Where(x => x.POH_FK == id));
                dbContext.SaveChanges();

                dbContext.POTerm_Master.RemoveRange(dbContext.POTerm_Master.Where(x => x.POH_FK == id));
                dbContext.SaveChanges();
            }
            return RedirectToAction("POGridDetails");
        }

        [HttpGet]
        public ActionResult EditPODetails(int id)
        {
            POViewModel poViewModel = new POViewModel();
            POHeaderModel poHeader = new POHeaderModel();
            List<PODetailModel> poDetailList = new List<PODetailModel>();
            List<POTermsModel> poTermList = new List<POTermsModel>();

            #region Header Fill
            var POHeaderList = dbContext.POHeader_Master.Find(id);
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
                               Text = acc.NAME,//acc.ID.ToString() + " - " + acc.NAME,
                               Value = Convert.ToString(acc.ID),
                           }).ToList();

            //accList.Insert(0, new SelectListItem()
            //{
            //    Text = "Select",
            //    Value = string.Empty
            //});

            foreach (var item in companyList.Where(s => s.Value == Convert.ToString(POHeaderList.COMP_CODE)))
            {
                item.Selected = true;
            }

            foreach (var item in accList.Where(s => s.Value == Convert.ToString(POHeaderList.ACC_CODE)))
            {
                item.Selected = true;
            }
            poHeader.companyDropDown = companyList;
            poHeader.accDropDown = accList;
            poHeader.ORDER_DATE = POHeaderList.ORDER_DATE;
            poHeader.ORDER_FINYEAR = POHeaderList.ORDER_FINYEAR;
            poHeader.ORDER_NO = POHeaderList.ORDER_NO;
            poHeader.REMARKS = POHeaderList.REMARKS;
            poHeader.POH_PK = POHeaderList.POH_PK;
            poHeader.INS_DATE = POHeaderList.INS_DATE;
            poHeader.INS_UID = POHeaderList.INS_UID;
            poViewModel.POHeader = poHeader;
            #endregion

            #region Detail Fill
            double grandTotal = 0;
            decimal grandQTotal = 0;

            var PODetailList = dbContext.PODetail_Master.Where(x => x.POH_FK == id).ToList();
            foreach (var poDetailModel in PODetailList)
            {
                var poDet = GetPODetails();
                foreach (var item in poDet.GetItems.
                    Where(s => s.Value == Convert.ToString(poDetailModel.ITEM_CODE)))
                {
                    item.Selected = true;
                }
                poDetailModel.GetItems = poDet.GetItems;
                poDetailModel.AMOUNT = Convert.ToString(
                                        Convert.ToDouble(poDetailModel.QTY) *
                                        Convert.ToDouble(poDetailModel.NET_RATE));
                var delDate = poDetailModel.DELV_DATE.ToShortDateString();
                poDetailModel.DELV_DATE = Convert.ToDateTime(delDate);
                grandQTotal += Convert.ToDecimal(poDetailModel.QTY);
                grandTotal += Convert.ToDouble(poDetailModel.AMOUNT);
                poDetailList.Add(poDetailModel);
            }

            ViewBag.pograndTotal = grandTotal;
            ViewBag.pograndQTotal = grandQTotal;
            poViewModel.PODetails = poDetailList;
            #endregion

            #region Term Fill

            var POTermList = dbContext.POTerm_Master.Where(x => x.POH_FK == id).ToList();
            foreach (var poTermModel in POTermList)
            {
                var termListDropDown = (from term in dbContext.Term_Master.Where(x => x.ACTIVE_TAG == "1")
                         .Where(y => y.PO == "1").ToList()
                                        select new SelectListItem()
                                        {
                                            Text = term.NAME,
                                            Value = Convert.ToString(term.ID)
                                        }).ToList();

                termListDropDown.Insert(0, new SelectListItem()
                {
                    Text = "Select Terms",
                    Value = string.Empty
                });
                poTermModel.termDropDown = termListDropDown;
                foreach (var item in poTermModel.termDropDown.
                    Where(s => s.Value == Convert.ToString(poTermModel.TERMS_CODE)))
                {
                    item.Selected = true;
                }
                poTermList.Add(poTermModel);
            }

            poViewModel.POTerms = poTermList;
            #endregion            

            return View(poViewModel);
        }

        [HttpPost]
        public ActionResult EditPODetails(POViewModel poEditViewModel)
        {
            int poExistId;
            var poViewModelDet = "NotNullDummy";// dbContext.POHeader_Master.Find(poEditViewModel.POHeader.POH_PK);
            if (poViewModelDet != null)
            {
                poEditViewModel.POHeader.UDT_DATE = DateTime.Now;
                poEditViewModel.POHeader.UDT_UID = userManager.GetUserName(HttpContext.User);     
                dbContext.POHeader_Master.Update(poEditViewModel.POHeader);
                dbContext.SaveChanges();

                poExistId = poEditViewModel.POHeader.POH_PK;
                if (poExistId != 0)
                {
                    foreach (var pODetailStaticUpd in poEditViewModel.PODetails)
                    {
                        if (pODetailStaticUpd.POD_PK != 0)
                        {
                            pODetailStaticUpd.UDT_DATE = DateTime.Now;
                            pODetailStaticUpd.UDT_UID = userManager.GetUserName(HttpContext.User);
                        }
                        else
                        {                            
                            pODetailStaticUpd.INS_DATE = DateTime.Now;
                            pODetailStaticUpd.INS_UID = userManager.GetUserName(HttpContext.User);
                        }
                        pODetailStaticUpd.POH_FK = poExistId;
                        pODetailStaticUpd.APPROVED_DATE = DateTime.Now;
                        pODetailStaticUpd.APPROVED_UID = "N/A";
                    }

                    foreach (var pOTermsStaticUpd in poEditViewModel.POTerms)
                    {
                        if (pOTermsStaticUpd.POT_PK != 0)
                        {                            
                            pOTermsStaticUpd.UDT_DATE = DateTime.Now;
                            pOTermsStaticUpd.UDT_UID = userManager.GetUserName(HttpContext.User);
                        }
                        else
                        {                           
                            pOTermsStaticUpd.INS_DATE = DateTime.Now;
                            pOTermsStaticUpd.INS_UID = userManager.GetUserName(HttpContext.User);
                        }
                        pOTermsStaticUpd.POH_FK = poExistId;
                    }

                    foreach (var pODetailModel in poEditViewModel.PODetails)
                    {
                        if (pODetailModel.POD_PK != 0)
                        {
                            dbContext.PODetail_Master.Update(pODetailModel);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            dbContext.PODetail_Master.Add(pODetailModel);
                            dbContext.SaveChanges();
                        }
                    }

                    foreach (var pOTermsModel in poEditViewModel.POTerms)
                    {
                        if (pOTermsModel.POT_PK != 0)
                        {
                            dbContext.POTerm_Master.Update(pOTermsModel);
                            dbContext.SaveChanges();
                        }
                        else
                        {
                            dbContext.POTerm_Master.Add(pOTermsModel);
                            dbContext.SaveChanges();
                        }
                    }
                }
            }
            else
            {

            }
            return RedirectToAction("POGridDetails");
        }
    }
}