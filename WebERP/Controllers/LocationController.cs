using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WebERP.Models.Location;
using WebERP.Models;
using System.Diagnostics;
using WebERP.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace WebERP.Controllers
{
    [Authorize(Roles = "Admin")]
    public class LocationController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext dbContext;

        public LocationController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager,ApplicationDbContext context)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            this.dbContext = context;
        }       

        public IActionResult LocationDetails(LocationViewModel locViewModel)
        {
            if (locViewModel == null)
            {
                locViewModel = new LocationViewModel
                {
                    ActiveTab = Tab.Country
                };
            }
            return View(locViewModel);
        }

        public IActionResult SwitchToTabs(string tabname)
        {
            var vm = new LocationViewModel();
            switch (tabname)
            {
                case "Country":
                    vm.ActiveTab = Tab.Country;
                    break;
                case "State":
                    vm.ActiveTab = Tab.State;
                    break;
                case "City":
                    vm.ActiveTab = Tab.City;
                    break;
                default:
                    vm.ActiveTab = Tab.Country;
                    break;
            }

            return RedirectToAction(nameof(LocationController.LocationDetails),vm);

        }

        #region Country CRUD
        [HttpGet]
        public ActionResult AddCountry()
        {            
            return View();
        }
        
        [HttpPost]        
        public ActionResult AddCountry(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                country.Ins_Date = DateTime.Now;
                country.Ins_Uid = userManager.GetUserName(HttpContext.User);
                dbContext.Countries.Add(country);
                dbContext.SaveChanges();
                return RedirectToAction("LocationDetails");
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditCountry(int Id)
        {
            var countryDetails = dbContext.Countries.Find(Id);
            if (countryDetails == null)
            {
                return RedirectToAction("LocationDetails");
            }
            return View(countryDetails);            
        }

        [HttpPost]
        public ActionResult EditCountry(CountryModel country)
        {
            if (ModelState.IsValid)
            {
                country.Upd_Date = DateTime.Now;
                country.Upd_Uid = userManager.GetUserName(HttpContext.User);
                dbContext.Countries.Update(country);
                dbContext.SaveChanges();
                return RedirectToAction("LocationDetails");
            }
            return View(country);
        }

        public ActionResult Delete(int Id)
        {
            if (Id > 0)
            {
                var countryId = dbContext.Countries.Where(x => x.Id == Id).FirstOrDefault();
                if (countryId != null)
                {
                    dbContext.Entry(countryId).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                }
            }
            return RedirectToAction("LocationDetails");
        }
        #endregion

        #region State CRUD
        [HttpGet]
        public ActionResult AddState()
        {
            StateModel stateModel = new StateModel();
            var countryList = (from country in dbContext.Countries
                               select new SelectListItem()
                               {
                                   Text = country.Name,
                                   Value = country.Id.ToString(),
                               }).ToList();

            countryList.Insert(0, new SelectListItem()
            {
                Text = "Select Country",
                Value = string.Empty,
                Selected = true
            });
            stateModel.countryDropDown = countryList;
            return View(stateModel);
        }

        [HttpPost]
        public ActionResult AddState(StateModel state)
        {
            var vm = new LocationViewModel();            
            if (ModelState.IsValid)
            {
                state.CountryCode = dbContext.Countries.Where(x => x.Id == state.CountryId).Select(s => s.CountryCode).FirstOrDefault();
                state.Ins_Date = DateTime.Now;
                state.Ins_Uid = userManager.GetUserName(HttpContext.User);
                dbContext.States.Add(state);
                dbContext.SaveChanges();
                vm.ActiveTab = Tab.State;
                return RedirectToAction(nameof(LocationController.LocationDetails), vm);
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditState(int Id)
        {
            var stateDetails = dbContext.States.Find(Id);
            if (stateDetails == null)
            {
                return RedirectToAction("LocationDetails");
            }
            var countryList = (from country in dbContext.Countries
                               select new SelectListItem()
                               {
                                   Text = country.Name,
                                   Value = country.Id.ToString(),
                               }).ToList();                     
            countryList.Insert(0, new SelectListItem()
            {
                Text = "Select Country",
                Value = string.Empty               
            });
            foreach (var item in countryList.Where(s => s.Value == Convert.ToString(stateDetails.CountryId)))
            {
                item.Selected = true;
            }
            stateDetails.countryDropDown = countryList;
            return View(stateDetails);
        }

        [HttpPost]
        public ActionResult EditState(StateModel state)
        {           
            var vm = new LocationViewModel();
            if (ModelState.IsValid)
            {
                state.CountryCode = dbContext.Countries.Where(x => x.Id == state.CountryId).Select(s => s.CountryCode).FirstOrDefault();
                state.Upd_Date = DateTime.Now;
                state.Upd_Uid = userManager.GetUserName(HttpContext.User);
                dbContext.States.Update(state);
                dbContext.SaveChanges();
                vm.ActiveTab = Tab.State;
                return RedirectToAction(nameof(LocationController.LocationDetails), vm);
            }
            return View(state);
        }

        public ActionResult DeleteState(int Id)
        {
            if (Id > 0)
            {
                var stateId = dbContext.States.Where(x => x.Id == Id).FirstOrDefault();
                if (stateId != null)
                {
                    dbContext.Entry(stateId).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                }
            }
            var vm = new LocationViewModel();
            vm.ActiveTab = Tab.State;
            return RedirectToAction(nameof(LocationController.LocationDetails), vm);
        }
        #endregion

        #region City CRUD
        [HttpGet]
        public ActionResult AddCity()
        {
            CityModel cityModel = new CityModel();
            var stateList = (from state in dbContext.States
                               select new SelectListItem()
                               {
                                   Text = state.Name,
                                   Value = state.Id.ToString(), 
                               }).ToList();

            stateList.Insert(0, new SelectListItem()
            {
                Text = "Select State",
                Value = string.Empty,
                Selected = true
            });
            cityModel.stateDropDown = stateList;
            return View(cityModel);           
        }

        [HttpPost]
        public ActionResult AddCity(CityModel city)
        {
            var vm = new LocationViewModel();
            if (ModelState.IsValid)
            {                                
                city.Ins_Date = DateTime.Now;
                city.Ins_Uid = userManager.GetUserName(HttpContext.User);
                dbContext.Cities.Add(city);
                dbContext.SaveChanges();
                vm.ActiveTab = Tab.City;
                return RedirectToAction(nameof(LocationController.LocationDetails), vm);
            }
            return View();
        }

        [HttpGet]
        public ActionResult EditCity(int Id)
        {
            var cityDetails = dbContext.Cities.Find(Id);
            if (cityDetails == null)
            {
                return RedirectToAction("LocationDetails");
            }
            var stateList = (from state in dbContext.States
                               select new SelectListItem()
                               {
                                   Text = state.Name,
                                   Value = state.Id.ToString(),
                               }).ToList();
            stateList.Insert(0, new SelectListItem()
            {
                Text = "Select State",
                Value = string.Empty
            });
            foreach (var item in stateList.Where(s => s.Value == Convert.ToString(cityDetails.StateId)))
            {
                item.Selected = true;
            }
            cityDetails.stateDropDown = stateList;
            return View(cityDetails);
        }

        [HttpPost]
        public ActionResult EditCity(CityModel city)
        {
            var vm = new LocationViewModel();
            if (ModelState.IsValid)
            {
                city.Upd_Date = DateTime.Now;
                city.Upd_Uid = userManager.GetUserName(HttpContext.User);
                dbContext.Cities.Update(city);
                dbContext.SaveChanges();
                vm.ActiveTab = Tab.City;
                return RedirectToAction(nameof(LocationController.LocationDetails), vm);
            }
            return View(city);
        }

        public ActionResult DeleteCity(int Id)
        {
            if (Id > 0)
            {
                var cityId = dbContext.Cities.Where(x => x.Id == Id).FirstOrDefault();
                if (cityId != null)
                {
                    dbContext.Entry(cityId).State = EntityState.Deleted;
                    dbContext.SaveChanges();
                }
            }
            var vm = new LocationViewModel();
            vm.ActiveTab = Tab.City;
            return RedirectToAction(nameof(LocationController.LocationDetails), vm);
        }
        #endregion




































        //// GET: Location
        //[HttpGet]
        //public ActionResult AddCountry()
        //{
        //    //countryModel country = new countryModel();
        //    //return PartialView("_addCountryModal",country);
        //    return View();
        //}

        // GET: Location
        //[HttpPost]
        //public IActionResult AddCountry(string countrycode,string countryname)
        //{            
        //    LocationViewModel country = new LocationViewModel();
        //    country.CountryCode = countrycode;
        //    country.Name = countryname;
        //    country.Ins_Date = DateTime.Now;            
        //    country.Ins_Uid = userManager.GetUserName(HttpContext.User);

        //    dbContext.Countries.Add(country);
        //    dbContext.SaveChangesAsync();
        //    return RedirectToAction("LocationDetails");
        //}

        //[HttpPost]
        //public IActionResult AddState(string selectCountryCode, string statecode, string statename)
        //{
        //    stateModel state = new stateModel();
        //    state.CountryId = dbContext.Countries.Where(x => x.CountryCode == selectCountryCode).Select(s => s.Id).FirstOrDefault();
        //    state.CountryCode = selectCountryCode;
        //    state.StateCode = statecode;
        //    state.Name = statename;
        //    state.Ins_Date = DateTime.Now;
        //    state.Ins_Uid = userManager.GetUserName(HttpContext.User);
        //    state.Upd_Date = DateTime.Now;
        //    state.Upd_Uid = userManager.GetUserName(HttpContext.User);
        //    dbContext.States.Add(state);
        //    dbContext.SaveChangesAsync();
        //    return RedirectToAction("LocationDetails");
        //}

        //[HttpPost]
        //public IActionResult AddCity(string selectStateCode, string citycode, string cityname)
        //{
        //    cityModel city = new cityModel();
        //    city.StateId = dbContext.States.
        //                      Where(x => x.StateCode == selectStateCode).
        //                      Select(s => s.Id).FirstOrDefault();
        //    city.StateCode = selectStateCode;
        //    city.StateCode = citycode;
        //    city.Name = cityname;
        //    city.Ins_Date = DateTime.Now;
        //    city.Ins_Uid = userManager.GetUserName(HttpContext.User);
        //    city.Upd_Date = DateTime.Now;
        //    city.Upd_Uid = userManager.GetUserName(HttpContext.User);
        //    dbContext.Cities.Add(city);
        //    dbContext.SaveChangesAsync();
        //    return RedirectToAction("LocationDetails");
        //}

        //[HttpGet]
        //public IActionResult LocationDetails()
        //{
        //    //locationVM
        //    locationViewModel locationView = new locationViewModel();
        //    var cnlst = dbContext.Countries.ToList();
        //    var cilst = dbContext.Cities.ToList();
        //    var stlst = dbContext.States.ToList();

        //    if(cnlst != null)
        //        locationView.countryList = dbContext.Countries.ToList();
        //    if (stlst != null)
        //        locationView.stateList = dbContext.States.ToList();
        //    if (cilst != null)
        //        locationView.cityList = dbContext.Cities.ToList();

        //    var countryList = (from country in dbContext.Countries
        //                        select new SelectListItem()
        //                        {
        //                            Text = country.Name,
        //                            Value = country.CountryCode.ToString(),
        //                        }).ToList();

        //    countryList.Insert(0, new SelectListItem()
        //    {
        //        Text = "Select Country",
        //        Value = string.Empty,
        //        Selected = true
        //    });

        //    var stateList = (from state in dbContext.States
        //                       select new SelectListItem()
        //                       {
        //                           Text = state.Name,
        //                           Value = state.StateCode.ToString(),
        //                       }).ToList();

        //    stateList.Insert(0, new SelectListItem()
        //    {
        //        Text = "Select State",
        //        Value = string.Empty,
        //        Selected = true
        //    });

        //    var cityList = (from city in dbContext.Cities
        //                       select new SelectListItem()
        //                       {
        //                           Text = city.Name,
        //                           Value = Convert.ToString(city.Id)
        //                       }).ToList();

        //    cityList.Insert(0, new SelectListItem()
        //    {
        //        Text = "Select City",
        //        Value = string.Empty,
        //        Selected = true
        //    });

        //    locationView.countryDropDown = countryList;
        //    locationView.stateDropDown = stateList;
        //    locationView.cityDropDown = cityList;
        //    return View(locationView);
        //}

        //// GET: Location/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

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