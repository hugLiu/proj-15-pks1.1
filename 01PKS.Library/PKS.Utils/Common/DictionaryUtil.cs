using System.Collections.Generic;
using System.Linq;

namespace PKS.Utils
{
    /// <summary>字典工具</summary>
    public static class DictionaryUtil
    {
        /// <summary>
        /// Dictionary根据值排序
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="isDesc">默认降序</param>
        /// <returns></returns>
        public static Dictionary<string, int> SortByValue(this Dictionary<string, int> dic, bool isDesc = true)
        {
            var sortedDic = from objDic in dic orderby objDic.Value ascending select objDic;
            if (isDesc)
            {
                sortedDic = from objDic in dic orderby objDic.Value descending select objDic;
            }
            return sortedDic.ToDictionary(s => s.Key, s => s.Value);
        }
        /// <summary>
        /// Dictionary根据值排序
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="isDesc">默认降序</param>
        /// <returns></returns>
        public static Dictionary<string, long?> SortByValue(this Dictionary<string, long?> dic, bool isDesc = true)
        {
            var sortedDic = from objDic in dic orderby objDic.Value ascending select objDic;
            if (isDesc)
            {
                sortedDic = from objDic in dic orderby objDic.Value descending select objDic;
            }
            return sortedDic.ToDictionary(s => s.Key, s => s.Value);
        }
        /// <summary>
        /// Dictionary根据key排序
        /// </summary>
        /// <param name="dic"></param>
        /// <param name="isDesc">默认降序</param>
        /// <returns></returns>
        public static Dictionary<string, int> SortByKey(this Dictionary<string, int> dic, bool isDesc = true)
        {
            var sortedDic = from objDic in dic orderby objDic.Key ascending select objDic;
            if (isDesc)
            {
                sortedDic = from objDic in dic orderby objDic.Key descending select objDic;
            }
            return sortedDic.ToDictionary(s => s.Key, s => s.Value);
        }
    }
}
