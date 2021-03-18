using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class RM_DTL
    {
        public int ID { get; set; }
        public int RM_HDR_FK { get; set; }
        public int GDW_Code { get; set; }
        public int ITEM_Code { get; set; }
        public int ARTICAL_Code { get; set; }
        public int BRAND_Code { get; set; }
        public int SIZE_Code { get; set; }
        public decimal ISSUE_QTY { get; set; }
        public decimal ORDER_QTY { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
    }
}
