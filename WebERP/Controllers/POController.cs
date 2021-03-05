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
            poDetailList.Add(GetPODetails());
            poViewModel.PODetails = poDetailList;
            return View(poViewModel);
        }

        public POHeaderModel GetPOHeader()
        {
            POHeaderModel poHeader = new POHeaderModel();
            var companyList = (from company in dbContext.Companies
                               select new SelectListItem()
                               {
                                   Text = company.ID.ToString() + " - " + company.NAME,
                                   Value = company.ID.ToString(),
                               }).ToList();

            companyList.Insert(0, new SelectListItem()
            {
                Text = "Select Company",
                Value = string.Empty,
                Selected = true
            });

            var accList = (from acc in dbContext.Account_Masters
                           select new SelectListItem()
                           {
                               Text = acc.ID.ToString() + " - " + acc.NAME,
                               Value = acc.ID.ToString(),
                           }).ToList();

            accList.Insert(0, new SelectListItem()
            {
                Text = "Select Acc",
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
                                   Value = items.ID.ToString(),
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
            return View(dbContext.POHeader_Master.ToList());
        }
            // GET: PO/Details/5
            public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PO/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PO/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(PODetails));
            }
            catch
            {
                return View();
            }
        }

        // GET: PO/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PO/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(PODetails));
            }
            catch
            {
                return View();
            }
        }

        // GET: PO/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PO/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(PODetails));
            }
            catch
            {
                return View();
            }
        }
    }
}