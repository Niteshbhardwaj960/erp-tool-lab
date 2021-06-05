using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class V_RM_ISSUE
    {
        [Key]
        //public int ID { get; set; }
        public int Cutting_Order_FK { get; set; }
        public int ITEM_Code { get; set; }
        public string ITEM_NAME { get; set; }
        public int ARTICAL_Code { get; set; }
        public string ART_NAME { get; set; }
        public decimal ISSUE_QTY { get; set; }
        public int SIZE_Code { get; set; }
        public string SIZE_NAME { get; set; }
        [NotMapped]
        public DateTime? Doc_Dates { get; set; }
        [NotMapped]
        public string Doc_Fins { get; set; }
        [NotMapped]
        public bool CHK { get; set; }
        [NotMapped]
        public decimal return_qty { get; set; }
        [NotMapped]
        public int GDW_Code { get; set; }
    }
}
