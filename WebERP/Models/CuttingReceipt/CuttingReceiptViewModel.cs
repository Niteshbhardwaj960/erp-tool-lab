﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class CuttingReceiptViewModel
    {
        public List<V_CuttingDetail> cuttingDetails { get; set; }

        public Cutting_Receipt cutting_Receipt { get; set; }

        public List<SelectListItem> CUTDropDown { get; set; }

        public string Type { get; set; }
    }
}
