using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class EmpSalViewModel
    {
        public List<Emp_Sal> emp_Sals { get; set; }

        public DateTime? FilterMonth { get; set; }

        public string Emp_Type { get; set; }

        public Emp_Sal emp_sal { get; set; }

        public List<Emp_Sal_PC_Cont_Dtl> emp_sal_dtl { get; set; }

    }
}
