using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WebERP.Models;

namespace WebERP.Models
{
    public class GateEntryViewModel
    {
        public List<V_GateEntryDetail> v_GateEntryDetails { get; set; }

        public Gate_HDR Gate_HDR { get; set; }

        public List<V_JW_DTL> V_JW_DTLs { get; set; }

        public string Worktype { get; set; }

        [NotMapped]
        public string gateEntryValCheck { get; set; }
    }
}
