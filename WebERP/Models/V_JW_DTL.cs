using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class V_JW_DTL
    {
        [Key]
        public int JWH_PK { get; set; }
        public int COMP_CODE { get; set; }
        public string DOC_FINYEAR { get; set; }
        public int DOC_NO { get; set; }
        public DateTime? DOC_DATE { get; set; }
        public string ACC_NAME { get; set; }
        public int ACC_CODE { get; set; }
        public int JWD_PK { get; set; }
        public decimal QTY { get; set; }
        public string QTY_UOM_NAME { get; set; }
        public int QTY_UOM { get; set; }
        public decimal GATE_ENTRY_QTY { get; set; }
        public decimal BAL_QTY { get; set; }
        public string ITEM_NAME { get; set; }
        public int ITEM_CODE { get; set; }
        public string ARTICAL_NAME { get; set; }
        public int ARTICAL_CODE { get; set; }
        public string SIZE_NAME { get; set; }
        public int SIZE_CODE { get; set; }
        public string GDW_NAME { get; set; }
        public int GDW_CODE { get; set; }
        public int HSN_CODE { get; set; }
        public string HSN_NAME { get; set; }
        public int PROC_CODE { get; set; }
        public string PROC_NAME { get; set; }
        public string Remarks { get; set; }
        [NotMapped]
        public string CHL_NO { get; set; }
        [NotMapped]
        public DateTime? CHL_DATE { get; set; }
        [NotMapped]
        public string Bill_NO { get; set; }
        [NotMapped]
        public DateTime? Bill_Date { get; set; }
        [NotMapped]
        public int Stk_Qty { get; set; }
        [NotMapped]
        public int Stk_UOM { get; set; }
        [NotMapped]
        public int Fin_Qty { get; set; }
        [NotMapped]
        public int Fin_UOM { get; set; }
        [NotMapped]
        public bool? Chart { get; set; }
    }
}
