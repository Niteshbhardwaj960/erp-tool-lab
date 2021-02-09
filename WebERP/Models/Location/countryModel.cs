using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{    
    public class locationViewModel
    {
        public ICollection<countryModel> countryList { get; set; }
        public ICollection<stateModel> stateList { get; set; }
        public ICollection<cityModel> cityList { get; set; }

        public string countryId { get; set; }
        public List<SelectListItem> countryDropDown { get; set; }
        public string stateId { get; set; }
        public List<SelectListItem> stateDropDown { get; set; }
        public string cityId { get; set; }
        public List<SelectListItem> cityDropDown { get; set; }

    }
    public class countryModel
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public DateTime Ins_Date { get; set; }
        public string Ins_Uid { get; set; }
        public DateTime Upd_Date { get; set; }
        public string Upd_Uid { get; set; }
        public ICollection<stateModel> States { get; set; }
    }
    public class stateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StateCode { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public DateTime Ins_Date { get; set; }
        public string Ins_Uid { get; set; }
        public DateTime Upd_Date { get; set; }
        public string Upd_Uid { get; set; }
        public countryModel Country { get; set; }
        public ICollection<cityModel> Cities { get; set; }
    }
    public class cityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public DateTime Ins_Date { get; set; }
        public string Ins_Uid { get; set; }
        public DateTime Upd_Date { get; set; }
        public string Upd_Uid { get; set; }
        public stateModel State { get; set; }
    }
}
