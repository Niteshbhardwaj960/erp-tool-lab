using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebERP.Models
{
    public class POTermsModel
    {
        [Key]
        public int POT_PK { get; set; }

        [ForeignKey("POHeaderModel")]
        public int POH_FK { get; set; }

        public virtual POHeaderModel POHeaderModel { get; set; }
        public int TERMS_CODE { get; set; }

        [StringLength(100)]
        public string REMARKS { get; set; }

        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }


        [NotMapped]
        public string TERMS_NAME { get; set; }
        [NotMapped]
        public List<SelectListItem> termDropDown { get; set; }
    }
}
