using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models.PurchasingOrder
{
    public class POHeaderModel
    {
        [Key]
        public int POH_PK { get; set; }
        public int COMP_CODE { get; set; }
        public DateTime ORDER_DATE { get; set; }
        public string ORDER_FINYEAR { get; set; }
        public int ORDER_NO { get; set; }
        public int ACC_CODE { get; set; }

        [StringLength(100)]
        public string REMARKS { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }

        [NotMapped]
        public ICollection<PODetailModel> PODetailItemList { get; set; }

    }
}
