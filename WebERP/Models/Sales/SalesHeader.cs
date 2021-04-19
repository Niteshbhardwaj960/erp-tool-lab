using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebERP.Models
{
    public class SalesHeader
    {
        [Key]
        public int SALE_PK { get; set; }
       
        public int COMP_CODE { get; set; }
        
        public int ACC_CODE { get; set; }

        [NotMapped]
        public List<SelectListItem> companyDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> accDropDown { get; set; }

        [NotMapped]
        public string COMP_NAME { get; set; }

        [NotMapped]
        public string ACC_NAME { get; set; }

        public DateTime DOC_DATE { get; set; }
        public string DOC_FINYEAR { get; set; }
        public int DOC_NO { get; set; }

        public int AGENTACC_CODE { get; set; }

        [NotMapped]
        public List<SelectListItem> agentaccDropDown { get; set; }

        public DateTime? GATEOUT_DATE { get; set; }
        public string GATEOUT_UID { get; set; }      

        public decimal GROSS_AMT { get; set; }
        public decimal TAX_AMT { get; set; }
        [StringLength(50)]
        public string OTH_AMTNAME1 { get; set; }
        public decimal OTH_AMT1 { get; set; }
        [StringLength(50)]
        public string OTH_AMTNAME2 { get; set; }
        public decimal OTH_AMT2 { get; set; }
        public decimal RF_AMT { get; set; }
        public decimal NET_AMT { get; set; }

        [NotMapped]
        public string TAX_NAME { get; set; }

        public int TAX_CODE { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999.99)]
        public decimal IGST_PER { get; set; }

        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999.99)]
        public decimal IGST_AMOUNT { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999.99)]
        public decimal CGST_PER { get; set; }

        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999.99)]
        public decimal CGST_AMOUNT { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999.99)]
        public decimal SGST_PER { get; set; }

        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999.99)]
        public decimal SGST_AMOUNT { get; set; }

        [StringLength(100)]
        public string REMARKS { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; } 
    }
}
