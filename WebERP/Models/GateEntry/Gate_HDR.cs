using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Gate_HDR
    {
        //[NotMapped]
        //public List<SelectListItem> countryDropDown { get; set; }
        //[NotMapped]
        //public List<SelectListItem> stateDropDown { get; set; }
        //[NotMapped]
        //public List<SelectListItem> cityDropDown { get; set; }

        public int ID { get; set; }
        [Required(ErrorMessage = "Company Code is Required Field")]
        public int Comp_Code { get; set; }
        public DateTime? Doc_Date { get; set; }
        public string Doc_FN_Year { get; set; }
        public string Doc_No { get; set; }
        public string Acc_Code { get; set; }
        public string Acc_Name { get; set; }
        public string Remarks { get; set; }
        public string Type { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public List<SelectListItem> AccDropDown { get; set; }
        [NotMapped]
        public string ActionType { get; set; }

    }
}
