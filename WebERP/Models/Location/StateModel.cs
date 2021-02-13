using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models.Location
{
    public class StateModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string StateCode { get; set; }
        [Required(ErrorMessage = "Please select country name")]
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public DateTime? Ins_Date { get; set; }
        public string Ins_Uid { get; set; }
        public DateTime? Upd_Date { get; set; }
        public string Upd_Uid { get; set; }

        [NotMapped]
        public List<SelectListItem> countryDropDown { get; set; }       
    }
}
