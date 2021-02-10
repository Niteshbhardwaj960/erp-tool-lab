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

namespace WebERP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public LocationController(
            RoleManager<IdentityRole> roleManager, 
            UserManager<ApplicationUser> userManager,
            ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }       

        // GET: Location
        [HttpGet]
        public ActionResult AddCountry()
        {
            //countryModel country = new countryModel();
            //return PartialView("_addCountryModal",country);
            return View();
        }

        // GET: Location
        [HttpPost]
        public IActionResult AddCountry(string countrycode,string countryname)
        {            
            countryModel country = new countryModel();
            country.CountryCode = countrycode;
            country.Name = countryname;
            country.Ins_Date = DateTime.Now;            
            country.Ins_Uid = userManager.GetUserName(HttpContext.User);
            
            dbContext.Countries.Add(country);
            dbContext.SaveChangesAsync();
            return RedirectToAction("LocationDetails");
        }

        [HttpPost]
        public IActionResult AddState(string selectCountryCode, string statecode, string statename)
        {
            stateModel state = new stateModel();
            state.CountryId = dbContext.Countries.Where(x => x.CountryCode == selectCountryCode).Select(s => s.Id).FirstOrDefault();
            state.CountryCode = selectCountryCode;
            state.StateCode = statecode;
            state.Name = statename;
            state.Ins_Date = DateTime.Now;
            state.Ins_Uid = userManager.GetUserName(HttpContext.User);
            state.Upd_Date = DateTime.Now;
            state.Upd_Uid = userManager.GetUserName(HttpContext.User);
            dbContext.States.Add(state);
            dbContext.SaveChangesAsync();
            return RedirectToAction("LocationDetails");
        }

        [HttpPost]
        public IActionResult AddCity(string selectStateCode, string citycode, string cityname)
        {
            cityModel city = new cityModel();
            city.StateId = dbContext.States.
                              Where(x => x.StateCode == selectStateCode).
                              Select(s => s.Id).FirstOrDefault();
            city.StateCode = selectStateCode;
            city.StateCode = citycode;
            city.Name = cityname;
            city.Ins_Date = DateTime.Now;
            city.Ins_Uid = userManager.GetUserName(HttpContext.User);
            city.Upd_Date = DateTime.Now;
            city.Upd_Uid = userManager.GetUserName(HttpContext.User);
            dbContext.Cities.Add(city);
            dbContext.SaveChangesAsync();
            return RedirectToAction("LocationDetails");
        }

        [HttpGet]
        public IActionResult LocationDetails()
        {
            //locationVM
            locationViewModel locationView = new locationViewModel();
            var cnlst = dbContext.Countries.ToList();
            var cilst = dbContext.Cities.ToList();
            var stlst = dbContext.States.ToList();

            if(cnlst != null)
                locationView.countryList = dbContext.Countries.ToList();
            if (stlst != null)
                locationView.stateList = dbContext.States.ToList();
            if (cilst != null)
                locationView.cityList = dbContext.Cities.ToList();

            var countryList = (from country in dbContext.Countries
                                select new SelectListItem()
                                {
                                    Text = country.Name,
                                    Value = country.CountryCode.ToString(),
                                }).ToList();

            countryList.Insert(0, new SelectListItem()
            {
                Text = "Select Country",
                Value = string.Empty,
                Selected = true
            });

            var stateList = (from state in dbContext.States
                               select new SelectListItem()
                               {
                                   Text = state.Name,
                                   Value = state.StateCode.ToString(),
                               }).ToList();

            stateList.Insert(0, new SelectListItem()
            {
                Text = "Select State",
                Value = string.Empty,
                Selected = true
            });

            var cityList = (from city in dbContext.Cities
                               select new SelectListItem()
                               {
                                   Text = city.Name,
                                   Value = Convert.ToString(city.Id)
                               }).ToList();

            cityList.Insert(0, new SelectListItem()
            {
                Text = "Select City",
                Value = string.Empty,
                Selected = true
            });

            locationView.countryDropDown = countryList;
            locationView.stateDropDown = stateList;
            locationView.cityDropDown = cityList;
            return View(locationView);
        }

        // GET: Location/Create
        public ActionResult Create()
        {
            return View();
        }

        //// POST: Location/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add insert logic here
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Location/Edit/5
        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //// POST: Location/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add update logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //// GET: Location/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //// POST: Location/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}