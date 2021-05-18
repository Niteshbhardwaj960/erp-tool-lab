using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
namespace WebERP.Models
{
    public class Artical_Merge_DTL
    {
        [Key]
        public int ID { get; set; }
        public int HDR_FK { get; set; }
        public int GDW_CODE { get; set; }
        public int ITEM_CODE { get; set; }
        public int ARTICAL_CODE { get; set; }
        public int SIZE_CODE { get; set; }
        public decimal STK_QTY_OUT { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string GDW_NAME { get; set; }
        [NotMapped]
        public string ITEM_NAME { get; set; }
        [NotMapped]
        public string ARTICAL_NAME { get; set; }
        [NotMapped]
        public string SIZE_NAME { get; set; }
    }
}
