using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebERP.Models
{
    public class PODetailModel
    {

        [Key]
        public int POD_PK { get; set; }

        [ForeignKey("POHeaderModel")]
        public int POH_FK { get; set; }

        public virtual POHeaderModel POHeaderModel { get; set; }

        public int ITEM_CODE { get; set; }

        [RegularExpression(@"^\d+\.\d{0,3}$")]
        [Range(0, 999999999999.999)]
        public decimal QTY { get; set; }

        public int QTY_UOM { get; set; }

        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999.99)]
        public decimal RATE { get; set; }

        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999.99)]
        public decimal DISC_PER { get; set; }

        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999.99)]
        public decimal DISC_RATE { get; set; }

        [DataType(DataType.Currency)]
        [RegularExpression(@"^\d+\.\d{0,2}$")]
        [Range(0, 9999999999999.99)]
        public decimal NET_RATE { get; set; }

        public int RATE_UOM { get; set; }

        public DateTime DELV_DATE { get; set; }

        [StringLength(3)]
        public string POD_PK_STATUS { get; set; }
        [StringLength(100)]
        public string REMARKS { get; set; }
        public string APPROVED_UID { get; set; }
        public DateTime? APPROVED_DATE { get; set; }

        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        
    }
}
