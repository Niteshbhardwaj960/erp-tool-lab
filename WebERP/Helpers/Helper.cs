using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebERP.Helpers
{
    public static class Helper
    {
        public static int GetFinYear()
        {
            string FinYear = "";
            DateTime date = DateTime.Now;
            if ((date.Month) == 1 || (date.Month) == 2 || (date.Month) == 3)
            {
                FinYear = (date.Year - 1) + "" + date.Year;
            }
            else
            {
                FinYear = date.Year + "" + (date.Year + 1);
            }
            return Convert.ToInt32(FinYear);
        }
        public static string DateFormat(string ParamDate)
        {
            DateTime date = DateTime.Parse(ParamDate);
            return date.ToString("dd/MMM/yyyy");
        }
    }
}
