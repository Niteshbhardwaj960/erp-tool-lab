using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class StockDTL_Model
    {
        [Key]
        public int ID { get; set; }
        public int COMP_CODE { get; set; }
        public string Tran_Table { get; set; }
        public int Tran_Table_PK { get; set; }
        [Required(ErrorMessage = "Gowdown Name is Required Field")]
        public int GDW_CODE { get; set; }
        [Required(ErrorMessage = "Item Name is Required Field")]
        public int Item_Code { get; set; }
        public int Artical_CODE { get; set; }
        public int Size_Code { get; set; }
        public decimal Stk_Qty_IN { get; set; }
        public decimal Stk_Qty_OUT { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string Type { get; set; }
    }
}
