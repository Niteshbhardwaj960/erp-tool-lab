using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class RM_HDR
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Company Code is Required Field")]
        public int Comp_Code { get; set; }
        public DateTime? Doc_Date { get; set; }
        public string Doc_FN_Year { get; set; }
        public int Doc_No { get; set; }
        public int Cutting_Order_FK { get; set; }
        public string EMP_NAME { get; set; }
        public string CUTTING_ORDER_NO { get; set; }
        public string ITEM_NAME { get; set; }
        public string ART_NAME { get; set; }
        public string SIZE_NAME { get; set; }
        public string PROC_NAME { get; set; }
        public string Remarks { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }       
    }
} 
