using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class V_GateEntryDetail
    {
        [Key]
        public int POH_PK { get; set; }
        public int ORDER_NO { get; set; }        
        public int COMP_CODE { get; set; }
        public string ORDER_FINYEAR { get; set; }
        public DateTime ORDER_DATE { get; set; }
        public int ACC_CODE { get; set; }
        public string ACC_NAME { get; set; }
        public int POD_PK { get; set; }
        public decimal QTY { get; set; }
        public string QTY_UOM { get; set; }
        public int QTY_CODE { get; set; }
        public int Gate_Entry_Qty { get; set; }
        public decimal BAL_QTY { get; set; }
        public string ITEM_NAME { get; set; }
        public int ITEM_CODE { get; set; }
        public string REMARKS { get; set; }
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
