using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Cutting_Receipt
    {
        public int ID { get; set; }
        public int COMP_CODE { get; set; }
        public DateTime? DOC_DATE { get; set; }
        public int DOC_FINYEAR { get; set; }
        public int DOC_NO { get; set; }
        public int CUTTING_ORDER_FK { get; set; }
        public decimal RECEIPT_QTY { get; set; }
        public string EMP_NAME { get; set; }
        public string ITEM_NAME { get; set; }
        public string ART_NAME { get; set; }
        public string SIZE_NAME { get; set; }
        public string PROC_NAME { get; set; }
        public int GDW_CODE { get; set; }
        [NotMapped]
        public int ITEM_CODE { get; set; }
        [NotMapped]
        public int ART_CODE { get; set; }
        [NotMapped]
        public int SIZE_CODE { get; set; }
        [NotMapped]
        public int PROC_CODE { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string Type { get; set; }
    }
}
