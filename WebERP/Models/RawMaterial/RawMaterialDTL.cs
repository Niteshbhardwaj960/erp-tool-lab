using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class RawMaterialDTL
    {
        public List<V_CuttingDetail> cuttingDetails { get; set; }

        public List<V_RM_DTL> V_RM_DTLs { get; set; }

        public RM_HDR RM_HDRs { get; set; }

        public RM_DTL RM_DTL { get; set; }

        public List<SelectListItem> CUTDropDown { get; set; }

        public List<RM_DTL> RM_DTL_LST { get; set; }

        public DateTime? Doc_Dates { get; set; }

        public string Doc_Fins { get; set; }
    }
}
