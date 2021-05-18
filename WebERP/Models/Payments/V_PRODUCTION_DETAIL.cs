using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class V_PRODUCTION_DETAIL
    {
        [Key]
        public int comp_code { get; set; }
        public int emp_code { get; set; }
        public int proc_code { get; set; }
        public string Emp_type { get; set; }
        public int item_code { get; set; }
        public int artical_code { get; set; }
        public int size_code { get; set; }
        public decimal prod_qty { get; set; }
        public string proc_rate { get; set; }
        public decimal Artical_Amount { get; set; }
        public DateTime? DOC_DATE { get; set; }
    }
}
