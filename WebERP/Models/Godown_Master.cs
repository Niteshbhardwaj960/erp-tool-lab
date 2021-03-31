using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Godown_Master
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Name is Required Field")]
        public string NAME { get; set; }
        public string ABV { get; set; }
        public string SALE_TAG { get; set; }
        public string GO_DOWN_TYPE { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string Type { get; set; }
    }
}
