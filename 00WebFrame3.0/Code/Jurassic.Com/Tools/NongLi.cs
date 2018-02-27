using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Jurassic.Com.Tools
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 农历算法
    /// </summary>
    public class NongLi
    {
        static ChineseLunisolarCalendar calendar = new ChineseLunisolarCalendar();

        /// <summary> 
        /// 农历所对应的日期
        /// </summary>
        public DateTime Date { get; set; }

        int leapMonth = 0;

		int year = 0;
        int month = 0;
        int day = 1;


        /// <summary>
        /// 农历的月份,在1-13之间.由于润月的存在,一年可能有13个月
        /// </summary>
        public int Month
        {
            get { return month; }
        }

        /// <summary>
        /// 农历的月中第几天,在1-29,30之间
        /// </summary>
        public int Day
        {
            get { return day; }
        }

        /// <summary>
        /// 农历的第几个月为润月
        /// </summary>
        public int LeapMonth
        {
            get { return leapMonth; }
        }

        /// <summary>
        /// 用指定日期新建一个农历对象
        /// </summary>
        /// <param name="dt"></param>
        public NongLi(DateTime dt)
        {
            Date = dt;
			year = calendar.GetYear(Date);
            month = calendar.GetMonth(Date);
            day = calendar.GetDayOfMonth(Date);
			leapMonth = calendar.GetLeapMonth(year);
        }

        static string[] chnMonthNames = {
       "正月","二月","三月","四月","五月","六月","七月","八月","九月","十月","十一月","十二月"
                                               };
        static string[] chnDayNames = {
      "初一","初二","初三","初四", "初五","初六","初七","初八","初九","初十",
      "十一","十二","十三","十四","十五","十六","十七","十八","十九","二十",
      "廿一","廿二","廿三","廿四","廿五","廿六","廿七","廿八","廿九","三十"
                                      };
        /// <summary>
        /// 农历的月份的中文名称,如果是润月,会在前面加个"润"字
        /// 例:七月,润七月,...
        /// </summary>
        /// <returns></returns>
        public string MonthName()
        {
            string name = String.Empty;
            int m = month;
            if (m == leapMonth)
            {
                name = "润";
            }
			if (m >= leapMonth && leapMonth > 0)
			{
				m--;
			}

            return name + chnMonthNames[m - 1];
        }

        /// <summary>
        /// 农历日的中文名称,如初一,十五等等.
        /// </summary>
        /// <returns></returns>
        public string DayName()
        {
            return chnDayNames[day - 1];
        }

        /// <summary>
        /// 显示农历月日,如八月十五
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return MonthName() + DayName();
        }
    }

    /// <summary>
    /// 星期的汉字枚举
    /// </summary>
    public enum ChnWeek
    {
        星期日 = 0,
        星期一 = 1,
        星期二 = 2,
        星期三 = 3,
        星期四 = 4,
        星期五 = 5,
        星期六 = 6,
    }
}