using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebERP.Models.Location;

namespace WebERP.Models
{
    public class LocationViewModel
    {        
        public Tab ActiveTab { get; set; }
    }

    public enum Tab
    {
        Country,
        State,
        City
    }


    //public ICollection<CountryModel> countryList { get; set; }
    //public ICollection<StateModel> stateList { get; set; }
    //public ICollection<CityModel> cityList { get; set; }
    //public string countryId { get; set; }
    //public List<SelectListItem> countryDropDown { get; set; }
    //public string stateId { get; set; }
    //public List<SelectListItem> stateDropDown { get; set; }
    //public string cityId { get; set; }
    //public List<SelectListItem> cityDropDown { get; set; }

}
