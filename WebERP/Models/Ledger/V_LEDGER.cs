using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class V_LEDGER
    {
        [Key]
        public int COMP_CODE { get; set; }
        public int ACC_CODE { get; set; }
        public string ACC_NAME { get; set; }
        public string DOC_FN_YEAR { get; set; }
        public int DOC_NO { get; set; }
        public string DOC_DATE { get; set; }
        public string DOC_TYPE { get; set; }
        public string REMARKS {get; set; }
        public decimal DR_AMOUNT { get; set; }
        public decimal CR_AMOUNT { get; set; }
        [NotMapped]
        public decimal BAL_AMOUNT { get; set; }
    }
}
