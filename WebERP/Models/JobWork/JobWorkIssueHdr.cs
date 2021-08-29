using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebERP.Models
{
    public class JobWorkIssueHdr
    {
        [Key]
        public int JWH_PK { get; set; }
        public int COMP_CODE { get; set; }
        public int ACC_CODE { get; set; }

        [NotMapped]
        public List<SelectListItem> companyDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> accDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> accTempDropDown { get; set; }

        public DateTime DOC_DATE { get; set; }
        public string DOC_FINYEAR { get; set; }
        public int DOC_NO { get; set; }

        [StringLength(100)]
        public string REMARKS { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }

        [NotMapped]
        public string COMP_NAME { get; set; }
        [NotMapped]
        public string ACC_NAME { get; set; }
        [NotMapped]
        public ICollection<JobWorkIssueDet> JWDetailItemList { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> GetProcess { get; set; }
    }
}
