using Microsoft.AspNetCore.Mvc.Rendering;
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
        public string Item_UOM { get; set; }
        public int Art_Name { get; set; }
        public int Size_Name { get; set; }
        public int Proc_Name { get; set; }
        public int CHL_NO { get; set; }
        public DateTime? CHL_DATE { get; set; }
        public string Bill_NO { get; set; }
        public DateTime? Bill_Date { get; set; }
        public decimal Stk_Qty { get; set; }
        public int Stk_UOM { get; set; }
        public decimal Fin_Qty { get; set; }
        public int Fin_UOM { get; set; }
        public int GDW_NO { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        public string Remarks { get; set; }
        public string FIN_YEAR { get; set; }
        public DateTime? DOC_DATE { get; set; }
        public string ACC_NAME { get; set; }
        public string Doc_No { get; set; }
        [NotMapped]
        public Decimal PO_QTY { get; set; }
        [NotMapped]
        public Decimal BAL_QTY { get; set; }
        [NotMapped]
        public string ITEM_NAMEs {get; set;}
        [NotMapped]
        public string ART_NAMEs { get; set; }
        [NotMapped]
        public string PROC_NAMEs { get; set; }
        [NotMapped]
        public string SIZE_NAMEs { get; set; }
        [NotMapped]
        public string UOM_NAME { get; set; }
        [NotMapped]
        public bool CHK { get; set; }
        [NotMapped]
        public string GDW_NAME { get; set; }
    }
}
