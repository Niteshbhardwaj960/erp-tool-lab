﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class ProcessRate_Master
    {
        [NotMapped]
        public List<SelectListItem> UOMDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> ProcDropDown { get; set; }
        [NotMapped]
        public List<SelectListItem> ArticalDropDown { get; set; }
        [NotMapped]
        public string UOM_Name { get; set; }
        [NotMapped]
        public string Artical_Name { get; set; }
        [NotMapped]
        public string Proc_Name { get; set; }
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "Process Name is Required Field")]
        public string Proc_Code { get; set; }
        [Required(ErrorMessage = "Artical Name is Required Field")]
        public string Artical_Code { get; set; }
        [Required(ErrorMessage = "Rate is Required Field")]
        public string Rate { get; set; }
        [Required(ErrorMessage = "Commercial Rate is Required Field")]
        public string Comm_Rate { get; set; }
        [Required(ErrorMessage = "UOM Name is Required Field")]
        public string UOM_Code { get; set; }
        public DateTime? From_DATE { get; set; }
        public DateTime? To_DATE { get; set; }
        public DateTime? INS_DATE { get; set; }
        public string INS_UID { get; set; }
        public DateTime? UDT_DATE { get; set; }
        public string UDT_UID { get; set; }
        [NotMapped]
        public string Type { get; set; }
    }
}
