using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class Emp_Sal
    {
        [Key]
        public int ID { get; set; }
        public int COMP_CODE { get; set; }
        public DateTime? SAL_MONTH { get; set; }
        public int SAL_TYPE { get; set; }
        public int EMP_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public decimal SALARY { get; set; }
        public decimal SHIFT_HRS { get; set; }
        public decimal PAY_DAYS { get; set; }
        public decimal OT_HRS { get; set; }
        public decimal ERN_SAL { get; set; }
        public decimal ERN_OT { get; set; }
        public decimal ADVANCE_AMOUNT{ get; set; }
        public decimal PAYABAL_SALARY { get; set; }
        public decimal RF_SAL { get; set; }
        public decimal NET_PAY_SAL { get; set; }
        public decimal PAID_SAL { get; set; }
        public decimal PAID_DATE { get; set; }
        public decimal PAID_USER { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string GDW_NAME { get; set; }
    }
}
