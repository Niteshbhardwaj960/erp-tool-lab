using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Term_Master
    {
        public int ID { get; set; }
        [Required]
        public string NAME { get; set; }
        public string PO { get; set; }
        public string SAL_Order { get; set; }
        public string ACTIVE_TAG { get; set; }
        public DateTime INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
    }
}
