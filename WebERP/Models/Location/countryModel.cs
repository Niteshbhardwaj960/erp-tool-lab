using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Models
{    
    public class countryModel
    {
        public int Id { get; set; }
        public string CountryCode { get; set; }
        public string Name { get; set; }
        public DateTime Ins_Date { get; set; }
        public string Ins_Uid { get; set; }
        public DateTime Upd_Date { get; set; }
        public string Upd_Uid { get; set; }
        public ICollection<stateModel> States { get; set; }
    }
    public class stateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StateCode { get; set; }
        public int CountryId { get; set; }
        public string CountryCode { get; set; }
        public DateTime Ins_Date { get; set; }
        public string Ins_Uid { get; set; }
        public DateTime Upd_Date { get; set; }
        public string Upd_Uid { get; set; }
        public countryModel Country { get; set; }
        public ICollection<cityModel> Cities { get; set; }
    }
    public class cityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public string StateCode { get; set; }
        public DateTime Ins_Date { get; set; }
        public string Ins_Uid { get; set; }
        public DateTime Upd_Date { get; set; }
        public string Upd_Uid { get; set; }
        public stateModel State { get; set; }
    }
}
