using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Jurassic.Com.DB
{
    /// <summary>
    /// 识别实体特殊标记的帮助类，根据北京总部成果改写
    /// </summary>
    public class ModelHelper
    {
        /// <summary>
        /// 获取实体类主键字段,此处改为和System.ComponentModel.DataAnnotations的[Key]属性一致
        /// </summary>
        /// <tparam name="t">业务实体类型</param>
        /// <returns></returns>
        public static string GetKeyField(Type t)
        {
            string key = "";
            var name = t.Name;
            foreach (var prop in t.GetProperties())
            {
                foreach (Attribute attr in prop.GetCustomAttributes(true))
                {
                    if (attr is KeyAttribute)
                    {
                        key = prop.Name;
                        return key;
                    }
                }
            }
            return key;
        }

        /// <summary>
        /// 获取实体类主键字段值
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        public static object GetKeyFieldValue<T>(T entity)
        {
            Type objTye = typeof(T);
            PropertyInfo property = objTye.GetProperty(GetKeyField(objTye));
            return property.GetValue(entity, null).ToString();
        }

        public static object GetFieldValue<T>(T entity, string fieldName)
        {
            Type objTye = typeof(T);
            PropertyInfo property = objTye.GetProperty(fieldName);
            return property.GetValue(entity, null).ToString();
        }

        public static Hashtable GetKeyFieldAndValue<T>(T entity)
        {
            Hashtable result = new Hashtable();
            Type objTye = typeof(T);
            string key = GetKeyField(objTye);

            result.Add(key, objTye.GetProperty(key).GetValue(entity, null).ToString());
            return result;
        }
    }
}
