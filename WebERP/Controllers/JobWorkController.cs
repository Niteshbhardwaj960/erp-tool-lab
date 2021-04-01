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
            jbEditView.JobWorkIssHeader = dbContext.JobWorkIssue_Header.Where(r => r.JWH_PK == id).FirstOrDefault();
            var jwListDet = dbContext.JobWorkIssue_Details.Where(l => l.JWH_FK == id).AsNoTracking().ToList();
            foreach (var jwDetailItem in jwListDet.ToList())
            {
                //RMModel. = dbContext.PODetail_Master.
                //                           Where(x => x.POD_PK == gateDetailModel.POD_FK).
                //                           Select(y => y.QTY).FirstOrDefault();
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
            }
            jbEditView.JobWorkIssueDetails = jwListDet;
            return View();
        }

        [HttpPost]
        public ActionResult EditJobWork(JobWorkViewModel jwEditModel)
        {
            return RedirectToAction("JobWorkGrid");
        }

        [HttpGet]
        public ActionResult ShowJobWork(int id)
        {
            return View();
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