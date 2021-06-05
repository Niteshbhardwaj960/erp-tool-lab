using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class RMRViewModel
    {
        public List<V_RM_ISSUE> v_RM_ISSUEs { get; set; }

        public List<RMR_DTL> RMR_DTL_LIST { get; set; }

        public RMR_HDR RMR_HDR { get; set; }

        public RMR_DTL RMR_DTL { get; set; }

        [NotMapped]
        public DateTime? Doc_Dates { get; set; }

        [NotMapped]
        public string Doc_Fins { get; set; }

        [NotMapped]
        public List<SelectListItem> CUTDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> GDWDropDown { get; set; }
    }
}
