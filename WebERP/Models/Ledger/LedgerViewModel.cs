using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class LedgerViewModel
    {
        public List<V_LEDGER> v_LEDGERs { get; set; }

        public List<SelectListItem> ddlAcc { get; set; }

        public List<SelectListItem> ddlFin { get; set; }

        public DateTime? FromDate { get; set; }
       
        public DateTime? To_Date { get; set; }
       
        public string Doc_FIN_Years { get; set; }

        public string Acc_Codes { get; set; }

        public decimal CR_Amounts { get; set; }

        public decimal DR_Amounts { get; set; }

        public string type { get; set; }
    }
}
