using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class V_RM_DTL
    {
        [Key]
        public int ID { get; set; }
        public int COMP_CODE { get; set; }
        public int GDW_CODE { get; set; }
        public string GDW_NAME { get; set; }
        public int ITEM_CODE { get; set; }
        public string ITEM_NAME { get; set; }
        public int ARTICAL_CODE { get; set; }
        public string ARTICAL_NAME { get; set; }
        public int SIZE_CODE { get; set; }
        public string SIZE_NAME { get; set; }
        public decimal STK_QTY_IN { get; set; }
        public decimal STK_QTY_OUT { get; set; }
        public decimal STK_QTY { get; set; }
        [NotMapped]
        public bool CHK { get; set; }
        [NotMapped]
        public decimal Issue_Qty { get; set; }
    }
}
