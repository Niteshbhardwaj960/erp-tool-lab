using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace WebERP.Models
{
    public class Emp_Sal_PC_Cont_Dtl
    {
        [Key]
        public int ID { get; set; }
        public int EMP_SAL_FK { get; set; }
        public int ART_CODE { get; set; }
        public int PROC_CODE { get; set; }
        public decimal PRODUCT_QTY { get; set; }
        public decimal PRODUCT_RATE { get; set; }
        public decimal ART_AMOUNT { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string GDW_NAME { get; set; }
        [NotMapped]
        public string ART_NAME { get; set; }
        [NotMapped]
        public string PROC_NAME { get; set; }
    }
}
