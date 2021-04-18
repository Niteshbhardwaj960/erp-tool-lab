using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class SalesViewModel
    {
        public SalesHeader SalesHeader { get; set; }
        public List<SalesDetail> SaleDetails { get; set; }
    }
}
