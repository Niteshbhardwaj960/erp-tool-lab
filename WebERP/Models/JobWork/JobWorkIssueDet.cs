using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace WebERP.Models
{
    public class JobWorkIssueDet
    {
        [Key]
        public int JWD_PK { get; set; }

        [ForeignKey("JobWorkIssueHdr")]
        public int JWH_FK { get; set; }

        public virtual JobWorkIssueHdr JobWorkIssueHdr { get; set; }
       
        public int GODOWN_CODE { get; set; }

        [NotMapped]
        public string ITEM_NAME { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> GetItems { get; set; }

        public int ITEM_CODE { get; set; }

        public int ARTICAL_CODE { get; set; }

        public int SIZE_CODE { get; set; }

        public int HSN_CODE { get; set; }

        public int PROC_CODE { get; set; }

        [RegularExpression(@"^\d+\.\d{0,3}$")]
        [Range(0, 999999999999.999)]
        public decimal QTY { get; set; }

        public string QTY_UOM { get; set; }

        [StringLength(100)]
        public string REMARKS { get; set; }

        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
    }
}
