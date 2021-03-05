using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class V_GateEntryDetail
    {
        [Key]
        public int ORDER_NO { get; set; }
        public int POH_PK { get; set; }
        public int COMP_CODE { get; set; }
        public string ORDER_FINYEAR { get; set; }
        public DateTime ORDER_DATE { get; set; }
        public String ACC_CODE { get; set; }
        public int POD_PK { get; set; }
        public Decimal QTY { get; set; }
        public string QTY_UOM { get; set; }
        public int Gate_Entry_Qty { get; set; }
        public Decimal BAL_QTY { get; set; }
        public string ITEM_NAME { get; set; }
    }
}
