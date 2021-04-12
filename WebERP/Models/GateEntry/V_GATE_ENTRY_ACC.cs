using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class V_GATE_ENTRY_ACC
    {
        [Key]
        public string TBL_TYPE { get; set; }
        public int ACC_CODE { get; set; }
        public string ACC_NAME { get; set; }
    }
}
