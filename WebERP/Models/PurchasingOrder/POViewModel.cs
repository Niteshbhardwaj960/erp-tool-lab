using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebERP.Models
{
    public class POViewModel
    {
        public POHeaderModel POHeader { get; set; }
        public List<PODetailModel> PODetails { get; set; }
        public List<POTermsModel> POTerms { get; set; }
    }
}
