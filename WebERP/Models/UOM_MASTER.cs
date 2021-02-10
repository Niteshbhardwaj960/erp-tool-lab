using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class UOM_MASTER
    {
        public int ID { get; set; }
        [Required]
        public string NAME { get; set; }
        public string ABV { get; set; }
        public DateTime INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
    }
}
