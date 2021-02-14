using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models.Location
{
    public class CityModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required(ErrorMessage = "Please select state name")]
        public int StateId { get; set; }
        public string CityCode { get; set; }
        public DateTime? Ins_Date { get; set; }
        public string Ins_Uid { get; set; }
        public DateTime? Upd_Date { get; set; }
        public string Upd_Uid { get; set; }

        [NotMapped]
        public List<SelectListItem> stateDropDown { get; set; }
        //public StateModel State { get; set; }
    }
}
