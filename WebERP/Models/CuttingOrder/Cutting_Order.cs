using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Cutting_Order
    {
        public int ID { get; set; }
        public int COMP_CODE { get; set; }
        public DateTime? DOC_DATE { get; set; }
        public int DOC_FINYEAR { get; set; }
        public int DOC_NO { get; set; }
        [Required(ErrorMessage ="Required Field")]
        public int EMP_CODE { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public int CONT_EMP_CODE { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public int ITEM_CODE { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public int ARTICAL_CODE { get; set; }
        [Required(ErrorMessage = "Required Field")]
        public int SIZE_CODE { get; set; }
        public int PROC_CODE { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public int ORDER_QTY { get; set; }
        [RegularExpression(@"\d+(\.\d{1,3})?", ErrorMessage = "Upto 3 decimal place is allowed")]
        public decimal AVG_PC_WEIGHT { get; set; }
        public decimal WASTAGE_PER { get; set; }
        public string ORDER_STATUS { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public int EMP_NAME { get; set; }
        [NotMapped]
        public int CONT_EMP_NAME { get; set; }
        [NotMapped]
        public int ITEM_NAME { get; set; }
        [NotMapped]
        public int ARTICAL_NAME { get; set; }
        [NotMapped]
        public int SIZE_NAME { get; set; }
        [NotMapped]
        public int PROC_NAME { get; set; }
        [NotMapped]
        public List<SelectListItem> EmpDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> ContEmpDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> ItemDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> ArticalDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> SizeDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> ProcDropDown { get; set; }
        [NotMapped]
        public string Type { get; set; }
    }
}
