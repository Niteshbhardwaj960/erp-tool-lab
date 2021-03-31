using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Employee_Master
    {
        [Key]
        public int ID { get; set; }
        public int COMP_CODE { get; set; }
        [Required(ErrorMessage = "Employee Code is Required Field")]
        public int EMP_CODE { get; set; }
        [Required(ErrorMessage = "Employee Name is Required Field")]
        public string EMP_NAME { get; set; }
        [Required(ErrorMessage = "Department Name is Required Field")]
        public int DEP_CODE { get; set; }
        [Required(ErrorMessage = "Employee Type is Required Field")]
        public string EMP_TYPE { get; set; }
        [Required(ErrorMessage = "Employee Father Name is Required Field")]
        public string Emp_Father_Name { get; set; }
        [Required(ErrorMessage = "Mobile No is Required Field")]
        public string emp_mobile_no1 { get; set; }
        public string emp_mobile_no2 { get; set; }
        [Required(ErrorMessage = "Employee Date of Joining is Required Field")]
        public DateTime? emp_doj { get; set; }
        [Required(ErrorMessage = "Salary is Required Field")]
        public int emp_salary { get; set; }
        public string Dep_Name { get; set; }
        public string active_tag { get; set; }
        public string Remarks { get; set; }
        public string NAME { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string Type { get; set; }
        [NotMapped]
        public List<SelectListItem> DepDropDown;
    }
}
