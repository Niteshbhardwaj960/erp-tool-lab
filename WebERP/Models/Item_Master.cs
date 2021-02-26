using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Item_Master
    {
        [NotMapped]
        public List<SelectListItem> UOMDropDown { get; set; }
        public int ID { get; set; }
        [Required(ErrorMessage = "Name is Required Field")]
        public string NAME { get; set; }
        public int UOM_CODE  { get; set; }
        public int HSN_CODE  { get; set; }
        public int MIN_STOCK { get; set; }
        public int MAX_STOCK { get; set; }
        public string ACTIVE_TAG { get; set; }
        public string REMARKS { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string UOM_Name { get; set; }
    }
}
