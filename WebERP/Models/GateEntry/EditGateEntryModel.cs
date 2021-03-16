using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class EditGateEntryModel
    {
        public List<GateEntry.GateEntryDetail> EditGateEntryDetails { get; set; }

        public Gate_HDR Gate_HDRs { get; set; }

        public List<SelectListItem> GoDownDropDown;

        public string GWDERROR { get; set; }

    }
}
