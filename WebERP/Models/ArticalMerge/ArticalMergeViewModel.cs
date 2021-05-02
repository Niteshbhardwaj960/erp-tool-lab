using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class ArticalMergeViewModel
    {
        public List<V_RM_DTL> v_STK_DTL { get; set; }

        public Artical_Merge_HDR artical_Merge_HDR { get; set; }

        public List<Artical_Merge_DTL> artical_Merge_DTL { get; set; }
        [NotMapped]
        public List<SelectListItem> GDWDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> ITEMDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> ARTDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> SIZEDropDown { get; set; }
        [NotMapped]
        public decimal AssStockQty { get; set; }

    }
}
