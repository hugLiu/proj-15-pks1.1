using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.PageEngine.Param;

namespace PKS.PageEngine.Extensions
{
    public static class PageContextExtension
    {
        public static void AddContextParam(this PageContext context,string key,object value)
        {
            if (context.ContextParams.ContainsKey(key))
            {
                context.ContextParams[key].Value = value;
                if (value != null)
                    context.ContextParams[key].DataType = value.GetType().Name;
                return;
            }
            VParam param = new VParam();
            param.Code = key;
            param.Value = value;
            if (value != null)
                param.DataType = value.GetType().Name;
            param.Name = key;
            context.ContextParams.Add(key,param);
        }

        public static VParam GetContextParam(this PageContext context,string paramName)
        {
            if (context.ContextParams != null && context.ContextParams.ContainsKey(paramName))
                return context.ContextParams[paramName];
            return null;
        }

        public static object GetContextParamValue(this PageContext context,string paramName)
        {
            var vParam = context.GetContextParam(paramName);
            if (vParam == null)
                return null;
            return vParam.Value;
        }


        public static T  GetContextParamValue<T>(this PageContext context,string paramName)
        {
            var vParam = context.GetContextParam(paramName);
            if (vParam == null)
                return default(T);
            var dataType = vParam.DataType.ToLower();
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(Convert.ToString(vParam.Value ?? vParam.DefaultValue), typeof(T));
            }
            if (typeof(T) == typeof(int) || typeof(T) == typeof(Int32))
            {
                return (T)Convert.ChangeType(Convert.ToInt32(vParam.Value ?? vParam.DefaultValue), typeof(T));
            }
            if (typeof(T) == typeof(bool))
            {
                var result = false;
                bool.TryParse(Convert.ToString(vParam.Value ?? vParam.DefaultValue), out result);
                return (T)Convert.ChangeType(result, typeof(T));
            }
            if (typeof(T) == typeof(double))
            {
                var result = 0.00;
                double.TryParse(Convert.ToString(vParam.Value ?? vParam.DefaultValue), out result);
                return (T)Convert.ChangeType(Convert.ToDecimal(vParam.Value ?? vParam.DefaultValue), typeof(T));
            }
            if (typeof(T).IsEnum)
            {
                var enumValue = Convert.ToString(vParam.Value ?? vParam.DefaultValue);

                try
                {
                    return (T)Convert.ChangeType(Enum.Parse(typeof(T), enumValue, true), typeof(T));
                }
                catch
                {
                    return default(T);
                }
            }
            return (T)Convert.ChangeType(vParam.Value ?? vParam.DefaultValue, typeof(T));
        }
    }
}
