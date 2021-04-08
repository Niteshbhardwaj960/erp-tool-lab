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

        [HttpGet]
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
            int jwNewId;
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
                foreach (var jwDetailStaticUpd in jwModel.JobWorkIssueDetails)
                {
                    jwDetailStaticUpd.JWH_FK = jwNewId;
                    jwDetailStaticUpd.INS_DATE = DateTime.Now;
                    jwDetailStaticUpd.INS_UID = userManager.GetUserName(HttpContext.User);                   
                }
                foreach (var jwDetailModel in jwModel.JobWorkIssueDetails)
                {
                    dbContext.JobWorkIssue_Details.Add(jwDetailModel);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("JobWorkGrid");
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

            foreach (var item in companyList.Where(s => s.Value == Convert.ToString(jwHeaderList.COMP_CODE)))
            {
                item.Selected = true;
            }

            foreach (var item in accList.Where(s => s.Value == Convert.ToString(jwHeaderList.ACC_CODE)))
            {
                item.Selected = true;
            }
            jwHeader.companyDropDown = companyList;
            jwHeader.accDropDown = accList;
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
            int jwHdrId;
            jwEditModel.JobWorkIssHeader.UDT_DATE = DateTime.Now;
            jwEditModel.JobWorkIssHeader.UDT_UID = userManager.GetUserName(HttpContext.User);         
            dbContext.JobWorkIssue_Header.Update(jwEditModel.JobWorkIssHeader);
            dbContext.SaveChanges();

            jwHdrId = jwEditModel.JobWorkIssHeader.JWH_PK;
            if (jwHdrId != 0)
            {
                foreach (var jwDetailStaticUpd in jwEditModel.JobWorkIssueDetails)
                {
                    jwDetailStaticUpd.JWH_FK = jwHdrId;
                    jwDetailStaticUpd.UDT_DATE = DateTime.Now;
                    jwDetailStaticUpd.UDT_UID = userManager.GetUserName(HttpContext.User);
                }
                foreach (var jwDetailModel in jwEditModel.JobWorkIssueDetails)
                {
                    dbContext.JobWorkIssue_Details.Update(jwDetailModel);
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("JobWorkGrid");
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
            if (id > 0)
            {
                var jwHeader = dbContext.JobWorkIssue_Header.Where(x => x.JWH_PK == id).FirstOrDefault();
                if (jwHeader != null)
                {
                    dbContext.Entry(jwHeader).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                }
                dbContext.JobWorkIssue_Details.RemoveRange(dbContext.JobWorkIssue_Details.Where(x => x.JWH_FK == id));
                dbContext.SaveChanges();
            }
            return RedirectToAction("JobWorkGrid");
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