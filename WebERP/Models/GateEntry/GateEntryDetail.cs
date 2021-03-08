using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models.GateEntry
{
    public class GateEntryDetail
    {
        public int ID { get; set; }
        public int GH_FK { get; set; }
        public int POD_FK { get; set; }
        public int JW_FK { get; set; }
        public int Order_No { get; set; }
        public int Item_Name { get; set; }
        public int Item_UOM { get; set; }
        public string CHL_NO { get; set; }
        public DateTime? CHL_DATE { get; set; }
        public string Bill_NO { get; set; }
        public DateTime? Bill_Date { get; set; }
        public int Stk_Qty { get; set; }
        public int Stk_UOM  { get; set; }
        public int Fin_Qty { get; set; }
        public int Fin_UOM { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        public string Remarks { get; set; }
        public string FIN_YEAR { get; set; }
        public DateTime? DOC_DATE { get; set; }
    }
}
