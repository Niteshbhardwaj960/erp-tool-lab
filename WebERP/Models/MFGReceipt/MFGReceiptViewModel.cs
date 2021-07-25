using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class MFGReceiptViewModel
    {
        public List<V_CuttingDetail> cuttingDetails { get; set; }

        public MGF_RECEIPT MGF_RECEIPTs { get; set; }

        public List<SelectListItem> CUTDropDown { get; set; }
        public List<SelectListItem> EMPDropDown { get; set; }
        public List<SelectListItem> CONEMPDropDown { get; set; }
        public List<SelectListItem> PROCDropDown { get; set; }
        [NotMapped]
        public string error { get; set; }
        [NotMapped]
        public string errorOrder { get; set; }
        public string Type { get; set; }
        public int DOC_FINYEARS { get; set; }
        public DateTime? DOC_DATES { get; set; }
    }
}
