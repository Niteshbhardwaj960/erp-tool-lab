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
    [Authorize(Roles = "JobWork , Admin")]
   public class JobWorkController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public JobWorkController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }

        public IActionResult JobWorkGrid()
        {
            var JWGridList = dbContext.JobWorkIssue_Header.ToList();
            foreach (var item in JWGridList)
            {
                item.COMP_NAME = dbContext.Companies.
                                    Where(x => x.ID == item.COMP_CODE).
                                    Select(y => y.NAME).FirstOrDefault();
                item.ACC_NAME = dbContext.Account_Masters.
                                        Where(x => x.ID == item.ACC_CODE).
                                        Select(y => y.NAME).FirstOrDefault();
            }
            return View(JWGridList);
        }

        [HttpGet] //Get JobWork
        public ActionResult CreateJobWork()
        {
            JobWorkViewModel jwViewModel = new JobWorkViewModel();
            List<V_RM_DTL> viewStockMaster = new List<V_RM_DTL>();
            List<JobWorkIssueDet> jwDetailList = new List<JobWorkIssueDet>();
            jwViewModel.JobWorkIssHeader = GetJWHeader();
            viewStockMaster = dbContext.V_RM_DTL.AsNoTracking().ToList();
            jwDetailList = GetJWDetails(viewStockMaster);            
            jwViewModel.JobWorkIssueDetails = jwDetailList; 
            return View(jwViewModel);
        }

        [HttpPost]
        public ActionResult CreateJobWork(JobWorkViewModel jwModel)
        {
            ViewBag.DidValidationFail = "";
            var chkcount = 0;
            if (jwModel.JobWorkIssHeader.ACC_CODE == 0)
            {
                ViewBag.DidValidationFail = "Yes";
            }
            foreach (var jwDetailModel in jwModel.JobWorkIssueDetails)
            {
                if (jwDetailModel.chk == true)
                {
                    if (jwDetailModel.PROC_CODE == 0 || jwDetailModel.QTY == 0 || jwDetailModel.QTY == null)
                    {
                        ViewBag.DidValidationFail = "Yes";
                    }
                    chkcount = 1;
                }
            }

            if (ViewBag.DidValidationFail == "" && chkcount == 1)
            {
                int jwNewId;
                List<StockDTL_Model> StkDTL = new List<StockDTL_Model>();
                var maxOrderNum = dbContext.JobWorkIssue_Header
                    .Where(x => x.DOC_FINYEAR == jwModel.JobWorkIssHeader.DOC_FINYEAR)
                    .Select(p => p.DOC_NO).DefaultIfEmpty(0).Max();

                jwModel.JobWorkIssHeader.INS_DATE = DateTime.Now;
                jwModel.JobWorkIssHeader.INS_UID = userManager.GetUserName(HttpContext.User);
                jwModel.JobWorkIssHeader.DOC_NO = maxOrderNum + 1;
                jwModel.JobWorkIssHeader.DOC_DATE = jwModel.JobWorkIssHeader.DOC_DATE.Date;
                dbContext.JobWorkIssue_Header.Add(jwModel.JobWorkIssHeader);
                dbContext.SaveChanges();

                jwNewId = jwModel.JobWorkIssHeader.JWH_PK;
                if (jwNewId != 0)
                {
                    //foreach (var jwDetailStaticUpd in jwModel.JobWorkIssueDetails)
                    //{
                    //    jwDetailStaticUpd.JWH_FK = jwNewId;
                    //    jwDetailStaticUpd.INS_DATE = DateTime.Now;
                    //    jwDetailStaticUpd.INS_UID = userManager.GetUserName(HttpContext.User);                   
                    // }
                    foreach (var jwDetailModel in jwModel.JobWorkIssueDetails)
                    {
                        if (jwDetailModel.chk == true)
                        {
                            if (jwDetailModel.PROC_CODE <= 0 || jwDetailModel.QTY <= 0 || jwDetailModel.QTY == null)
                            {
                                ViewBag.DidValidationFail = "Yes";
                            }
                            jwDetailModel.JWH_FK = jwNewId;
                            jwDetailModel.INS_DATE = DateTime.Now;
                            jwDetailModel.INS_UID = userManager.GetUserName(HttpContext.User);
                            dbContext.JobWorkIssue_Details.Add(jwDetailModel);
                            dbContext.SaveChanges();
                            StkDTL.Add(new StockDTL_Model()
                            {
                                INS_DATE = DateTime.Now,
                                INS_UID = userManager.GetUserName(HttpContext.User),
                                COMP_CODE = 0,
                                Tran_Table = "Job Work",
                                Tran_Table_PK = jwDetailModel.JWD_PK,
                                GDW_CODE = Convert.ToInt32(jwDetailModel.GODOWN_CODE),
                                Item_Code = Convert.ToInt32(jwDetailModel.ITEM_CODE),
                                Artical_CODE = Convert.ToInt32(jwDetailModel.ARTICAL_CODE),
                                Size_Code = Convert.ToInt32(jwDetailModel.SIZE_CODE),
                                Stk_Qty_OUT = Convert.ToInt32(jwDetailModel.QTY),
                            });
                        }
                    }
                    foreach (var item in StkDTL)
                    {
                        dbContext.StockDTL_Models.Add(item);
                        dbContext.SaveChanges();

                    }
                }
                return RedirectToAction("JobWorkGrid");
            }
            else
            {
                ViewBag.DidValidationFail = "Yes";
                jwModel.JobWorkIssHeader = GetJWHeader();
                jwModel.JobWorkIssueDetails = GetJWDetails(dbContext.V_RM_DTL.AsNoTracking().ToList());
                return View(jwModel);
            }            
        }

        [HttpGet]
        public ActionResult EditJobWork(int id)
        {
            JobWorkViewModel jbEditView = new JobWorkViewModel();
            JobWorkIssueHdr jwHeader = new JobWorkIssueHdr();
            
            var jwHeaderList = dbContext.JobWorkIssue_Header.Find(id);
            var companyList = (from company in dbContext.Companies
                               select new SelectListItem()
                               {
                                   Text = company.ID.ToString() + " - " + company.NAME,
                                   Value = Convert.ToString(company.ID)
                               }).ToList();

            var accList = (from acc in dbContext.Account_Masters
                           select new SelectListItem()
                           {
                               Text = acc.NAME, /*acc.ID.ToString() + " - " + acc.NAME,*/
                               Value = Convert.ToString(acc.ID),
                           }).ToList();

            var procHdrList = (from items in dbContext.Process_Master
                            select new SelectListItem()
                            {
                                Text = items.NAME, /*items.ID.ToString() + " - " + items.NAME,*/
                                Value = Convert.ToString(items.ID),
                            }).ToList();

            procHdrList.Insert(0, new SelectListItem()
            {
                Text = "Select",
                Value = "0",
                Selected=true
            });
            
            jwHeader.companyDropDown = companyList;
            jwHeader.accDropDown = accList;
            jwHeader.ACC_CODE = jwHeaderList.ACC_CODE;
            jwHeader.DOC_DATE = jwHeaderList.DOC_DATE;
            jwHeader.DOC_FINYEAR = jwHeaderList.DOC_FINYEAR;
            jwHeader.DOC_NO = jwHeaderList.DOC_NO;
            jwHeader.REMARKS = jwHeaderList.REMARKS;
            jwHeader.JWH_PK = jwHeaderList.JWH_PK;
            jwHeader.INS_DATE = jwHeaderList.INS_DATE;
            jwHeader.INS_UID = jwHeaderList.INS_UID;
            jwHeader.GetProcess = procHdrList;
            jbEditView.JobWorkIssHeader = jwHeader;
            var jwListDet = dbContext.JobWorkIssue_Details.Where(l => l.JWH_FK == id).AsNoTracking().ToList();           

            foreach (var jwDetailItem in jwListDet.ToList())
            {
                
                jwDetailItem.ITEM_NAME = dbContext.Item_Master.
                                          Where(x => x.ID == jwDetailItem.ITEM_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                jwDetailItem.ARTICAL_NAME = dbContext.Artical_Master.
                                          Where(x => x.ID == jwDetailItem.ARTICAL_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                jwDetailItem.GODOWN_NAME = dbContext.Godown_Master.
                                          Where(x => x.ID == jwDetailItem.GODOWN_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                jwDetailItem.SIZE_NAME = dbContext.Size_Master.
                                          Where(x => x.ID == jwDetailItem.SIZE_CODE).
                                          Select(y => y.NAME).FirstOrDefault();               
                var procList = (from items in dbContext.Process_Master
                                select new SelectListItem()
                                {
                                    Text = items.NAME, /*items.ID.ToString() + " - " + items.NAME,*/
                                    Value = Convert.ToString(items.ID),
                                }).ToList();                
                procList.Insert(0, new SelectListItem()
                {
                    Text = "Select",
                    Value = "0"                    
                });

                foreach (var item in procList.Where(s => s.Value == Convert.ToString(jwDetailItem.PROC_CODE)))
                {
                    item.Selected = true;
                }
                jwDetailItem.GetProcess = procList;
            }
            jbEditView.JobWorkIssueDetails = jwListDet;
            return View(jbEditView);
        }

        [HttpPost]
        public ActionResult EditJobWork(JobWorkViewModel jwEditModel)
        {
            ViewBag.DidValidationFail = "";
            var chkcount = 0;
            if (jwEditModel.JobWorkIssHeader.ACC_CODE == 0)
            {
                ViewBag.DidValidationFail = "Yes";
            }
            foreach (var jwDetailModel in jwEditModel.JobWorkIssueDetails)
            {
                if (jwDetailModel.chk == true)
                {
                    if (jwDetailModel.PROC_CODE <= 0 || jwDetailModel.QTY <= 0 || jwDetailModel.QTY == null)
                    {
                        ViewBag.DidValidationFail = "Yes";
                    }
                    chkcount = 1;
                }
            }

            if (ViewBag.DidValidationFail == "" && chkcount == 1)
            {
                int jwHdrId;
                var hdrresponse = dbContext.JobWorkIssue_Header.Where(a => a.JWH_PK == jwEditModel.JobWorkIssHeader.JWH_PK).FirstOrDefault();
                hdrresponse.UDT_DATE = DateTime.Now;
                hdrresponse.UDT_UID = userManager.GetUserName(HttpContext.User);
                hdrresponse.ACC_CODE = jwEditModel.JobWorkIssHeader.ACC_CODE;
                hdrresponse.DOC_DATE = jwEditModel.JobWorkIssHeader.DOC_DATE;
                hdrresponse.DOC_FINYEAR = jwEditModel.JobWorkIssHeader.DOC_FINYEAR;
                hdrresponse.REMARKS = jwEditModel.JobWorkIssHeader.REMARKS;
                dbContext.SaveChanges();

                jwHdrId = jwEditModel.JobWorkIssHeader.JWH_PK;
                if (jwHdrId != 0)
                {
                    foreach (var jwDetailModel in jwEditModel.JobWorkIssueDetails)
                    {
                        if (jwDetailModel.chk == true)
                        {
                            var dltresponse = dbContext.JobWorkIssue_Details.Where(a => a.JWD_PK == jwDetailModel.JWD_PK).FirstOrDefault();
                            dltresponse.UDT_DATE = DateTime.Now;
                            dltresponse.UDT_UID = userManager.GetUserName(HttpContext.User);
                            dltresponse.HSN_CODE = jwDetailModel.HSN_CODE;
                            dltresponse.PROC_CODE = jwDetailModel.PROC_CODE;
                            dltresponse.QTY = jwDetailModel.QTY;
                            dltresponse.REMARKS = jwDetailModel.REMARKS;
                            dbContext.SaveChanges();

                            var stkResponse = dbContext.StockDTL_Models.Where(a => a.Tran_Table_PK == jwDetailModel.JWD_PK && a.Tran_Table == "Job Work").FirstOrDefault();
                            stkResponse.Stk_Qty_OUT = jwDetailModel.QTY;
                            stkResponse.UDT_DATE = DateTime.Now;
                            stkResponse.UDT_UID = userManager.GetUserName(HttpContext.User);
                            dbContext.SaveChanges();
                        }
                    }
                }
                return RedirectToAction("JobWorkGrid");
            }
            else
            {
                ViewBag.DidValidationFail = "Yes";
                var accList = (from acc in dbContext.Account_Masters
                               select new SelectListItem()
                               {
                                   Text = acc.NAME, /*acc.ID.ToString() + " - " + acc.NAME,*/
                                   Value = Convert.ToString(acc.ID),
                               }).ToList();

                var procHdrList = (from items in dbContext.Process_Master
                                   select new SelectListItem()
                                   {
                                       Text = items.NAME, /*items.ID.ToString() + " - " + items.NAME,*/
                                       Value = Convert.ToString(items.ID),
                                   }).ToList();

                procHdrList.Insert(0, new SelectListItem()
                {
                    Text = "Select",
                    Value = "0",
                    Selected = true
                });
                jwEditModel.JobWorkIssHeader.accDropDown = accList;
                foreach (var jwDetailItem in jwEditModel.JobWorkIssueDetails.ToList())
                {
                    jwDetailItem.GetProcess = procHdrList;
                }
                return View(jwEditModel);
            }
        }

        [HttpGet]
        public ActionResult ShowJobWork(int id)
        {
            JobWorkViewModel jwShowView = new JobWorkViewModel();
            JobWorkIssueHdr jwHeader = new JobWorkIssueHdr();

            var jwHeaderList = dbContext.JobWorkIssue_Header.Find(id);
            jwHeader.COMP_NAME = dbContext.Companies.
                                         Where(x => x.ID == jwHeaderList.COMP_CODE).
                                         Select(y => y.NAME).FirstOrDefault();
            jwHeader.ACC_NAME = dbContext.Account_Masters.
                                     Where(x => x.ID == jwHeaderList.ACC_CODE).
                                     Select(y => y.NAME).FirstOrDefault();
            jwHeader.DOC_DATE = jwHeaderList.DOC_DATE;
            jwHeader.DOC_FINYEAR = jwHeaderList.DOC_FINYEAR;
            jwHeader.DOC_NO = jwHeaderList.DOC_NO;
            jwHeader.REMARKS = jwHeaderList.REMARKS;
            jwHeader.JWH_PK = jwHeaderList.JWH_PK;
            jwShowView.JobWorkIssHeader = jwHeader;
            var jwListDet = dbContext.JobWorkIssue_Details.Where(l => l.JWH_FK == id).AsNoTracking().ToList();

            foreach (var jwDetailItem in jwListDet.ToList())
            {

                jwDetailItem.ITEM_NAME = dbContext.Item_Master.
                                          Where(x => x.ID == jwDetailItem.ITEM_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                jwDetailItem.ARTICAL_NAME = dbContext.Artical_Master.
                                          Where(x => x.ID == jwDetailItem.ARTICAL_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                jwDetailItem.GODOWN_NAME = dbContext.Godown_Master.
                                          Where(x => x.ID == jwDetailItem.GODOWN_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                jwDetailItem.SIZE_NAME = dbContext.Size_Master.
                                          Where(x => x.ID == jwDetailItem.SIZE_CODE).
                                          Select(y => y.NAME).FirstOrDefault();
                jwDetailItem.PROC_NAME = dbContext.Process_Master.
                                          Where(x => x.ID == jwDetailItem.PROC_CODE).
                                          Select(y => y.NAME).FirstOrDefault();               
            }
            jwShowView.JobWorkIssueDetails = jwListDet;
            return View(jwShowView);
        }

        [HttpGet]
        public ActionResult DeleteJobWork(int id)
        {
            ViewBag.jwDelete = "";
            var jwpk = dbContext.gateEntryDetails.Where(a => a.JW_FK == id).FirstOrDefault();

            if (jwpk == null)
            {
                var detail = dbContext.JobWorkIssue_Details.Where(D => D.JWD_PK == id).FirstOrDefault();
                if (id > 0)
                {                   
                    var detailcount = dbContext.JobWorkIssue_Details.Where(D => D.JWH_FK == detail.JWH_FK).ToList();
                    dbContext.JobWorkIssue_Details.Remove(detail);
                    dbContext.SaveChanges();

                    var dtlstk = dbContext.StockDTL_Models.Where(D => D.Tran_Table == "Job Work" && D.Tran_Table_PK == detail.JWD_PK).FirstOrDefault();
                    dbContext.StockDTL_Models.Remove(dtlstk);
                    dbContext.SaveChanges();

                    if (detailcount.Count() == 1)
                    {
                        var HDRdata = dbContext.JobWorkIssue_Header.Where(D => D.JWH_PK == detail.JWH_FK).FirstOrDefault();
                        dbContext.JobWorkIssue_Header.Remove(HDRdata);
                        dbContext.SaveChanges();
                        return RedirectToAction("JobWorkGrid");
                    }                    
                }
                return RedirectToAction("EditJobWork", new { id = detail.JWH_FK });
            }
            else
            {
               ViewBag.jwDelete = "Yes";
                var jwhdrpk = dbContext.JobWorkIssue_Details.Where(a => a.JWD_PK == id).Select(b => b.JWH_FK).FirstOrDefault();
                JobWorkViewModel jbEditView = new JobWorkViewModel();
                JobWorkIssueHdr jwHeader = new JobWorkIssueHdr();

                var jwHeaderList = dbContext.JobWorkIssue_Header.Find(jwhdrpk);
                var companyList = (from company in dbContext.Companies
                                   select new SelectListItem()
                                   {
                                       Text = company.ID.ToString() + " - " + company.NAME,
                                       Value = Convert.ToString(company.ID)
                                   }).ToList();

                var accList = (from acc in dbContext.Account_Masters
                               select new SelectListItem()
                               {
                                   Text = acc.NAME, /*acc.ID.ToString() + " - " + acc.NAME,*/
                                   Value = Convert.ToString(acc.ID),
                               }).ToList();

                var procHdrList = (from items in dbContext.Process_Master
                                   select new SelectListItem()
                                   {
                                       Text = items.NAME, /*items.ID.ToString() + " - " + items.NAME,*/
                                       Value = Convert.ToString(items.ID),
                                   }).ToList();

                procHdrList.Insert(0, new SelectListItem()
                {
                    Text = "Select",
                    Value = "0",
                    Selected = true
                });

                jwHeader.companyDropDown = companyList;
                jwHeader.accDropDown = accList;
                jwHeader.ACC_CODE = jwHeaderList.ACC_CODE;
                jwHeader.DOC_DATE = jwHeaderList.DOC_DATE;
                jwHeader.DOC_FINYEAR = jwHeaderList.DOC_FINYEAR;
                jwHeader.DOC_NO = jwHeaderList.DOC_NO;
                jwHeader.REMARKS = jwHeaderList.REMARKS;
                jwHeader.JWH_PK = jwHeaderList.JWH_PK;
                jwHeader.INS_DATE = jwHeaderList.INS_DATE;
                jwHeader.INS_UID = jwHeaderList.INS_UID;
                jwHeader.GetProcess = procHdrList;
                jbEditView.JobWorkIssHeader = jwHeader;
                var jwListDet = dbContext.JobWorkIssue_Details.Where(l => l.JWH_FK == jwhdrpk).AsNoTracking().ToList();

                foreach (var jwDetailItem in jwListDet.ToList())
                {

                    jwDetailItem.ITEM_NAME = dbContext.Item_Master.
                                              Where(x => x.ID == jwDetailItem.ITEM_CODE).
                                              Select(y => y.NAME).FirstOrDefault();
                    jwDetailItem.ARTICAL_NAME = dbContext.Artical_Master.
                                              Where(x => x.ID == jwDetailItem.ARTICAL_CODE).
                                              Select(y => y.NAME).FirstOrDefault();
                    jwDetailItem.GODOWN_NAME = dbContext.Godown_Master.
                                              Where(x => x.ID == jwDetailItem.GODOWN_CODE).
                                              Select(y => y.NAME).FirstOrDefault();
                    jwDetailItem.SIZE_NAME = dbContext.Size_Master.
                                              Where(x => x.ID == jwDetailItem.SIZE_CODE).
                                              Select(y => y.NAME).FirstOrDefault();
                    var procList = (from items in dbContext.Process_Master
                                    select new SelectListItem()
                                    {
                                        Text = items.NAME, /*items.ID.ToString() + " - " + items.NAME,*/
                                        Value = Convert.ToString(items.ID),
                                    }).ToList();
                    procList.Insert(0, new SelectListItem()
                    {
                        Text = "Select",
                        Value = "0"
                    });

                    foreach (var item in procList.Where(s => s.Value == Convert.ToString(jwDetailItem.PROC_CODE)))
                    {
                        item.Selected = true;
                    }
                    jwDetailItem.GetProcess = procList;
                }
                jbEditView.JobWorkIssueDetails = jwListDet;
                return View("EditJobWork", jbEditView);
            }
        }

        public JobWorkIssueHdr GetJWHeader()
        {
            JobWorkIssueHdr jwHeader = new JobWorkIssueHdr();
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

            var procList = (from items in dbContext.Process_Master
                            select new SelectListItem()
                            {
                                Text = items.ID.ToString() + " - " + items.NAME,
                                Value = Convert.ToString(items.ID),
                            }).ToList();

            procList.Insert(0, new SelectListItem()
            {
                Text = "Select",
                Value = "0",
                Selected = true
            });
            jwHeader.GetProcess = procList;
            jwHeader.companyDropDown = companyList;
            jwHeader.accDropDown = accList;
            jwHeader.DOC_DATE = DateTime.Now;
            jwHeader.DOC_FINYEAR = Convert.ToString(Helper.GetFinYear());
            return jwHeader;
        }

        public List<JobWorkIssueDet> GetJWDetails(List<V_RM_DTL> stockMasterList)
        {
            List<JobWorkIssueDet> jwDetailList = new List<JobWorkIssueDet>();
            var procList = (from items in dbContext.Process_Master
                            select new SelectListItem()
                            {
                                Text = items.ID.ToString() + " - " + items.NAME,
                                Value = Convert.ToString(items.ID),
                            }).ToList();

            procList.Insert(0, new SelectListItem()
            {
                Text = "Select",
                Value = "0",
                Selected = true
            });
            foreach (var stock in stockMasterList)
            {
                jwDetailList.Add(new JobWorkIssueDet()
                {
                    GODOWN_CODE = stock.GDW_CODE,
                    ITEM_CODE = stock.ITEM_CODE,
                    ARTICAL_CODE = stock.ARTICAL_CODE,
                    SIZE_CODE = stock.SIZE_CODE,
                    GODOWN_NAME = stock.GDW_NAME,
                    ITEM_NAME = stock.ITEM_NAME,
                    ARTICAL_NAME = stock.ARTICAL_NAME,
                    SIZE_NAME = stock.SIZE_NAME,
                    QTY_UOM = Convert.ToString(stock.STK_QTY),
                    GetProcess = procList
                });
            }            
            return jwDetailList;
        }
    }
}