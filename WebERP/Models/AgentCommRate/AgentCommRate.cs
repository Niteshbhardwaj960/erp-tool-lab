using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class AgentCommRate
    {
        public int ID { get; set; }
        public int ACC_CODE { get; set; }
        public string ACC_NAME { get; set; }
        [Range(.1, int.MaxValue, ErrorMessage = "Please enter a value bigger than 0")]
        public decimal COMM_RATE { get; set; }
        public string QTY_AMOUNT_TAG { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public List<SelectListItem> ACCDropDown { get; set; }
    }
}
