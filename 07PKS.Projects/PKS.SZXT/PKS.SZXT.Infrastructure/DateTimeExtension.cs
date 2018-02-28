using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.Infrastructure
{
    public static class DateTimeExtension
    {
        public static string ToMonthDay(this DateTime dt)
        {
            return dt.ToString("MM-dd");
        }
        public static string ToEsDate(this DateTime dt)
        {
            return dt.ToString("yyyy-MM-ddTHH:MM:ss.ms");
        }
    }
}
