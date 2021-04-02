using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Employee_Attandance
    {
        public int ID { get; set; }
        public int COMP_CODE { get; set; }
        public DateTime? DOC_DATE { get; set; }
        public string EMP_TYPE { get; set; }
        public int EMP_CODE { get; set; }
        public DateTime? SAL_YYYYMM { get; set; }
        public int SAL_YYYYMM_BRK { get; set; }
        public decimal PAY_DAYS { get; set; }
        public decimal OT_HRS { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string Emp_Name { get; set; }
        [NotMapped]
        public List<SelectListItem> EMPDropDown { get; set; }
    }
}
