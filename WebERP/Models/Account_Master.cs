using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Account_Master
    {
        [NotMapped]
        public List<SelectListItem> countryDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> stateDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> cityDropDown { get; set; }

        public int ID { get; set; }
        [Required]
        public string NAME { get; set; }
       
        [Required]
        public string ADD1 { get; set; }
        public string ADD2 { get; set; }
        [Required]
        public string CITY_CODE { get; set; }
        [Required]
        public string PIN_CODE { get; set; }
        public string GST_REGD_TAG { get; set; }
        public string GST_NO { get; set; }
        [Required]
        public string MOBILE_NO { get; set; }       
        [Required]
        [EmailAddress]
        public string EMAIL_ID { get; set; }
        public string PH_NO { get; set; }
        public string OP_BAL { get; set; }
        public string OP_BAL_TAG { get; set; }
        public string ACC_TYPE { get; set; }
        public string CR_LIMIT { get; set; }        
        public string CR_DAYS { get; set; }        
        public string ACTIVE_TAG { get; set; }
        public string REMARKS { get; set; }
        public DateTime INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
    }
}
