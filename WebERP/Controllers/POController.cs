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

namespace WebERP.Controllers
{
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
                .Where(x=> x.ORDER_FINYEAR == poViewModel.POHeader.ORDER_FINYEAR)
                .Select(p => p.ORDER_NO).DefaultIfEmpty(0).Max();

            poViewModel.POHeader.INS_DATE = DateTime.Now;
            poViewModel.POHeader.INS_UID= userManager.GetUserName(HttpContext.User);
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
            //var termList = dbContext.Term_Master.Where(x => x.ACTIVE_TAG == "1" )
            //                .Where(y=> y.PO =="1").ToList();
            //if (termList.Count() > 0)
            //{               
            //    foreach (var item in termList)
            //    {                   
            //        termsModels.Add(new POTermsModel() { termDropDown = termListDropDown, REMARKS = "" });
            //    }
            //}
            //else
            //{
            //    termsModels.Add(new POTermsModel());
            //}                      
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
                                     Text =uomList.ABV,
                                     Value = uomList.ID.ToString()
                                 }).ToList();

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
                                    Select(y=>y.NAME).FirstOrDefault();
            POHeaderList.ACC_NAME = dbContext.Account_Masters.
                                    Where(x => x.ID == POHeaderList.ACC_CODE).
                                    Select(y => y.NAME).FirstOrDefault();
            poViewModel.POHeader = POHeaderList;
            #endregion

            #region Detail Fill
            var PODetailList = dbContext.PODetail_Master.Where(x => x.POH_FK == id).ToList();
            foreach (var poDetailModel in PODetailList)
            {
                poDetailModel.ITEM_NAME = dbContext.Item_Master.
                                           Where(x => x.ID == poDetailModel.ITEM_CODE).
                                           Select(y => y.NAME).FirstOrDefault();
                poDetailModel.RATEUOM_NAME = dbContext.UOM_MASTER.
                                           Where(x => x.ID == poDetailModel.RATE_UOM).
                                           Select(y => y.NAME).FirstOrDefault();
                if (poDetailModel.POD_PK_STATUS == "A")
                    poDetailModel.POD_PK_STATUS = "Active";
                    if (poDetailModel.POD_PK_STATUS == "F")
                    poDetailModel.POD_PK_STATUS = "Finished";
                if (poDetailModel.POD_PK_STATUS == "C")
                    poDetailModel.POD_PK_STATUS = "Cancelled";
                poDetailList.Add(poDetailModel);
            }
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

    }
}