using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class V_PODetails
    {
        [Key]
        public int POH_PK { get; set; }
        public string ACC_CODE { get; set; }
        public DateTime? ORDER_DATE { get; set; }
        public decimal QTY { get; set; }
        [NotMapped]
        public ICollection<PODet> PODets;
        [NotMapped]
        public List<SelectListItem> AccDropDown { get; set; }
    }
    public class PODet
    {
        public int ORDER_NO { get; set; }
        public string ACC_CODE { get; set; }
        public DateTime? ORDER_DATE { get; set; }
        public decimal QTY { get; set; }
    }
    //public class PODetailsMain
    //{
    //    public ICollection<V_PODetails> PODetails;
    //    public PODetailsDDL PODetailsDDLs { get; set; }
    //}
}
