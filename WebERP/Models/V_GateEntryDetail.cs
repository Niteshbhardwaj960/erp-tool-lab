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
        public int poh_pk { get; set; }
        public int comp_code { get; set; }
        public string order_finyear { get; set; }
        public int order_no { get; set; }
        public DateTime? order_date { get; set; }
        public string ACC_NAME { get; set; }
        public int ACC_CODE { get; set; }
        public int pod_pk { get; set; }
        public decimal Qty { get; set; }
        public string QTY_UOM { get; set; }
        public string QTY_CODE { get; set; }
        public decimal Gate_Entry_qty { get; set; }
        public decimal Bal_Qty { get; set; }
        public string ITEM_NAME { get; set; }
        public int Item_Code { get; set; }
        public string Remarks { get; set; }
        [NotMapped]
        public int CHL_NO { get; set; }
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
