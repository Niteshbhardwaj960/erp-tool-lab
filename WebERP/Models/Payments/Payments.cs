using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Payments
    {
        public int ID { get; set; }
        public int COMP_CODE { get; set; }
        public DateTime? DOC_DATE { get; set; }
        public string DOC_FN_YEAR { get; set; }
        public int DOC_NO { get; set; }
        public int ACC_CODE { get; set; }
        public int CB_ACC_CODE { get; set; }
        public int PAYMENT_TAG { get; set; }
        public int PAYMENT_MODE  { get; set; }
        public int PAY_DOC_NO { get; set; }
        public DateTime? PAY_DOC_DATE { get; set; }
        public decimal AMOUNT { get; set; }
        public string REMARKS { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public List<SelectListItem> ACCDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> CBACCDropDown { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string PAY_MODE { get; set; }
        [NotMapped]
        public string PAY_TAG { get; set; }
    }
}
