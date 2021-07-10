using System;
using System.Collections.Generic;
using System.Globalization;
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
            return date.ToString("dd/MM/yyyy");
        }
        public static DateTime DateFormatDate(string ParamDate)
        {
            CultureInfo culture = new CultureInfo("en-IN");
            DateTime dateTimeObj = Convert.ToDateTime(ParamDate, culture);
            return Convert.ToDateTime(dateTimeObj.ToString("dd/MM/yyyy"));
            //// var dt = Convert.ToString(ParamDate);
            //DateTime date = DateTime.Parse(ParamDate);
            //return Convert.ToDateTime(date.ToString("dd/MM/yyyy"));
        }
        public static string DateFormatDDMMYYYY(DateTime ParamDate)
        {
            DateTimeFormatInfo usDtfi = new CultureInfo("en-US", false).DateTimeFormat;
            DateTimeFormatInfo ukDtfi = new CultureInfo("en-GB", false).DateTimeFormat;
            var result = Convert.ToDateTime("12/01/2011", usDtfi).ToString(ukDtfi.ShortDatePattern);
            return result;
        }
    }
}
