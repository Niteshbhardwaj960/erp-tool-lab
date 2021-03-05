using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{
    public class V_CITY_DTL
    {
        [Key]
        public int Id { get; set; }
        public string CSC_NAME { get; set; }
    }
}
