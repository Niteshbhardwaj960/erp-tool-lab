using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class MGF_RECEIPT
    {
        public int ID { get; set; }
        public int COMP_CODE { get; set; }
        public DateTime? DOC_DATE { get; set; }
        public int DOC_FINYEAR { get; set; }
        public int DOC_NO { get; set; }
        public int PROC_CODE { get; set; }
        public int EMP_CODE { get; set; }
        public int CONT_EMP_CODE { get; set; }
        public int CUTTING_ORDER_FK { get; set; }
        public decimal RECEIPT_QTY { get; set; }
        public string EMP_NAME { get; set; }
        public string ITEM_NAME { get; set; }
        public string ART_NAME { get; set; }
        public string SIZE_NAME { get; set; }
        public string PROC_NAME { get; set; }
        public string CUT_DOC_NO { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
    }
}
