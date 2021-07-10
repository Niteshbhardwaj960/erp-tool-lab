using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Employee_Advance
    {
        public int ID { get; set; }
        public int COMP_CODE { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode =true,DataFormatString ="{0:yyyy-mm-dd}")]
        public DateTime? DOC_DATE { get; set; }
        public string EMP_TYPE { get; set; }
        public int EMP_CODE { get; set; }
        public string EMP_FATHER { get; set; }
        public string EMP_MOB_NO { get; set; }
        public string EMP_DEP { get; set; }
        public DateTime? SAL_YYYYMM { get; set; }
        public int SAL_YYYYMM_BRK { get; set; }
        public decimal ADV_AMOUNT { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public string Emp_Name { get; set; }
        [NotMapped]
        public string Emp_Sal_Type { get; set; }
        [NotMapped]
        public List<SelectListItem>EMPDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> DepDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> SalDropDown { get; set; }
    }
}
