using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Company
    {        
        public int ID { get; set; }
        [Required]
        public string NAME { get; set; }
        public string ABV { get; set; }
        [Required]
        public string ADD1 { get; set; }
        public string ADD2 { get; set; }
        public string CITY_CODE { get; set; }
        [Required]
        public string PIN_CODE { get; set; }
        [Required]
        public string MOBILE_NO { get; set; }
        public string URL { get; set; }
        [Required][EmailAddress]
        public string EMAIL_ID { get; set; }
        public string PH_NO { get; set; }
        public string FAX_NO { get; set; }
        public string LST_NO { get; set; }
        public DateTime LST_DATE { get; set; }
        public string CST_NO { get; set; }
        public DateTime CST_DAT { get; set; }
        public string TIN_NO { get; set; }
        [Required]
        public string GST_NO { get; set; }
        public string ECC_NO { get; set; }
        public string SERVICE_TAX_NO { get; set; }
        [Required]
        public string PAN_NO { get; set; }
        public string IFSC_CODE { get; set; }
        public string TDS_NO { get; set; }
        public string ESI_NO { get; set; }
        public string PF_NO { get; set; }
        public string MSME_NO { get; set; }
        public string LOGO_NAME { get; set; }
        public string BANK_NAME { get; set; }
        public string BANK_ACC_NO { get; set; }
        public string BANK_BRANCH { get; set; }
        public string ACTIVE_TAG { get; set; }
        public string REMARKS { get; set; }
        public DateTime INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime UDT_DATE { get; set; }
        public string UDT_UID { get; set; }

    }
}
