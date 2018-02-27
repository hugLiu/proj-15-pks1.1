using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Jurassic.Com.Tools
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 从字符串中提取用分隔符分开的属性的操作类
    /// </summary>
    public class StringSpliter
    {
        /// <summary>
        /// 包含属性的字符串
        /// </summary>
        private String _source;

        /// <summary>
        /// 包含属性的字典表
        /// </summary>
        private Dictionary<string, string> propTable;

        /// <summary>
        /// 行分隔符
        /// </summary>
        String _split1 = ";";

        /// <summary>
        /// 行内属性名称和值之间的分隔符
        /// </summary>
        String _split2 = "=";

        /// <summary>
        /// 根据要分析的字符串，第一分隔符和第二分隔符新建一个操作对象
        /// </summary>
        /// <param name="source">待分解的字符串</param>
        /// <param name="split1">第一分隔符</param>
        /// <param name="split2">第二分隔符</param>
        public StringSpliter(String source, string split1, string split2)
        {
            _source = source ?? "";
            _split1 = split1;
            _split2 = split2;

            Str2Hash();
        }

        /// <summary>
        /// 根据指定源字符串创建字符串分割类
        /// </summary>
        /// <param name="source"></param>
        public StringSpliter(String source)
        {
            _source = source ?? "";
            Str2Hash();
        }

        public StringSpliter()
        {
            propTable = new Dictionary<string, string>();
        }

        /// <summary>
        /// 获取属性名获取属性值
        /// </summary>
        /// <param name="propName">属性名</param>
        /// <returns>属性值</returns>
        public string this[string propName]
        {
            get { return GetValue(propName); }
            set { SetValue(propName, value); }
        }

        /// <summary>
        /// 将字符串转成字典表
        /// </summary>
        /// <returns></returns>
        private void Str2Hash()
        {
            if (propTable == null)
                propTable = new Dictionary<string, string>();
            else
                propTable.Clear();
            char s1 = '∥';
            char s2 = '∷';
            String propstr = _source.Replace(_split1, s1.ToString()).Replace(_split2, s2.ToString());

            string[] propArr = propstr.Split(s1);

            foreach (String s in propArr)
            {
                if (s.IsEmpty()) continue;
                string[] sArr = s.Split(new char[] { s2 }, 2);
                if (sArr.Length > 1)
                {
                    SetValue(sArr[0].Trim(), sArr[1]);
                }
                else
                {
                    //因为有些应用需要遍历所有的属性，所以即使没值也要占一个空位
                    SetValue(sArr[0].Trim(), String.Empty);
                }
            }
        }

        /// <summary>
        /// 将字典表转成字符串
        /// </summary>
        /// <returns></returns>
        public override String ToString()
        {
            if (propTable.Count == 1 && propTable.First().Value.IsEmpty())
            {
                return propTable.Keys.First();
            }

            List<string> propStrs = new List<string>();
            foreach (KeyValuePair<string, string> kv in propTable)
            {
                if (kv.Key.IsEmpty()) continue;
                propStrs.Add(kv.Key + _split2 + kv.Value);
            }

            return string.Join(_split1, propStrs);
        }

        /// <summary>
        /// 设置一个属性
        /// </summary>
        /// <param name="propName">属性名称</param>
        /// <param name="o">属性值</param>
        void SetValue(String propName, object o)
        {
            if (o == null)
            {
                propTable.Remove(propName);
            }
            else
            {
                propTable[propName] = CommOp.ToStr(o);
            }
        }

        /// <summary>
        /// 获取某属性名对应的值
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        string GetValue(String propName)
        {
            if (propTable.ContainsKey(propName))
            {
                return propTable[propName];
            }
            return String.Empty;
        }

        public IEnumerable<string> Keys
        {
            get
            {
                return propTable.Keys;
            }
        }
    }
}
