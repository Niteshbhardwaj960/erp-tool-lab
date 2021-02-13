﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models.Location
{
    public class CountryModel
    {        
        public int Id { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [Required]
        public string Name { get; set; }
        public DateTime? Ins_Date { get; set; }
        public string Ins_Uid { get; set; }
        public DateTime? Upd_Date { get; set; }
        public string Upd_Uid { get; set; }        
       // public ICollection<StateModel> States { get; set; }
    }
}
