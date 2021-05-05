using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebERP.Models
{
    public class SalesViewModel
    {
        public SalesHeader SalesHeader { get; set; }
        public List<SalesDetail> SaleDetails { get; set; }
    }

    public class SalesCreateFilter
    {
        [NotMapped]
        [Required(ErrorMessage = "Please select Godown Name")]
        public int GodownCode { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please select Item Name")]
        public int ItemCode { get; set; }

        [NotMapped]
        public List<SelectListItem> GodownDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> ItemDropDown { get; set; }
    }
}
