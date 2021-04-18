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
        public IActionResult CreateSaleOrder()
        {
            SalesViewModel saleViewModel = new SalesViewModel();
            List<V_RM_DTL> viewStockMaster = new List<V_RM_DTL>();
            List<SalesDetail> saleDetailList = new List<SalesDetail>();
            saleViewModel.SalesHeader = GetSaleHeader();
            viewStockMaster = dbContext.V_RM_DTL.AsNoTracking().ToList();
            saleDetailList = GetJWDetails(viewStockMaster);
            saleViewModel.SaleDetails = saleDetailList;
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
            saleHeader.agentaccDropDown = agentacclist;
            saleHeader.companyDropDown = companyList;
            saleHeader.accDropDown = accList;
            saleHeader.DOC_DATE = DateTime.Now;
            saleHeader.DOC_FINYEAR = Convert.ToString(Helper.GetFinYear());
            return saleHeader;
        }

        public List<SalesDetail> GetJWDetails(List<V_RM_DTL> stockMasterList)
        {
            List<SalesDetail> saleDetailList = new List<SalesDetail>();
            decimal grandQTotal = 0;
            foreach (var stock in stockMasterList)
            {
                saleDetailList.Add(new SalesDetail()
                {
                    GODOWN_CODE = stock.GDW_CODE,
                    ITEM_CODE = stock.ITEM_CODE,
                    ARTICAL_CODE = stock.ARTICAL_CODE,
                    SIZE_CODE = stock.SIZE_CODE,
                    GODOWN_NAME = stock.GDW_NAME,
                    ITEM_NAME = stock.ITEM_NAME,
                    ARTICAL_NAME = stock.ARTICAL_NAME,
                    SIZE_NAME = stock.SIZE_NAME,
                    SALE_QTY = stock.STK_QTY,
                   
                });
                grandQTotal += stock.STK_QTY;
            }
            ViewBag.grandQTotal = grandQTotal;
            return saleDetailList;
        }

    }
}