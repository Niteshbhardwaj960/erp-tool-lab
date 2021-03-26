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
            List<JobWorkIssueDet> jwDetailList = new List<JobWorkIssueDet>();
            jwViewModel.JobWorkIssHeader = GetJWHeader();
            jwDetailList.Add(GetJWDetails());
            jwViewModel.JobWorkIssueDetails = jwDetailList; 
            return View(jwViewModel);
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
            jwHeader.companyDropDown = companyList;
            jwHeader.accDropDown = accList;
            return jwHeader;
        }

        public JobWorkIssueDet GetJWDetails()
        {
            JobWorkIssueDet jwDetail = new JobWorkIssueDet();
            //var itemList = (from items in dbContext.Item_Master
            //                select new SelectListItem()
            //                {
            //                    Text = items.NAME,
            //                    Value = Convert.ToString(items.ID),
            //                }).ToList();

            //itemList.Insert(0, new SelectListItem()
            //{
            //    Text = "Select",
            //    Value = string.Empty,
            //    Selected = true
            //});
            //jwDetail.GetItems = itemList;
            return jwDetail;
        }
    }
}