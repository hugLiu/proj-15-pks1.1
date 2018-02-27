using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq.Expressions;

namespace PKS.Utils
{
    /// <summary>反射工具</summary>
    public static class ReflectUtil
    {
        #region 标志成员
        /// <summary>实例成员标志</summary>
        public static readonly BindingFlags InstanceFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        /// <summary>静态成员标志</summary>
        public static readonly BindingFlags StaticFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        #endregion

        #region 类型反射方法
        /// <summary>根据类型名称创建实例</summary>
        public static object CreateInstanceFromName(this string typeName)
        {
            return CreateInstance(Type.GetType(typeName));
        }
        /// <summary>创建强类型的实例</summary>
        public static T CreateInstance<T>(this Type type)
        {
            return (T)CreateInstance(type);
        }
        /// <summary>创建实例</summary>
        public static object CreateInstance(this Type type)
        {
            return Activator.CreateInstance(type);
        }
        /// <summary>获得基础类型的类型代码</summary>
        public static TypeCode GetTypeCode(this Type type)
        {
            return Type.GetTypeCode(type);
        }
        /// <summary>获得父类型集合</summary>
        public static List<Type> GetParentTypes(Type type, Type terminalType = null)
        {
            List<Type> types = new List<Type>();
            if (terminalType == null) terminalType = typeof(object);
            Type baseType = type;
            while (type != null && type != terminalType)
            {
                types.Add(baseType);
                baseType = type.BaseType;
            }
            return types;
        }
        /// <summary>获得类型所在的程序集</summary>
        public static Assembly GetHostedAssembly(this Type type)
        {
            return Assembly.GetAssembly(type);
        }
        /// <summary>
        ///     判断指定类型是否为数值类型
        /// </summary>
        /// <param name="type">要检查的类型</param>
        /// <returns>是否是数值类型</returns>
        public static bool IsNumeric(this Type type)
        {
            return type == typeof(Byte)
                   || type == typeof(Int16)
                   || type == typeof(Int32)
                   || type == typeof(Int64)
                   || type == typeof(SByte)
                   || type == typeof(UInt16)
                   || type == typeof(UInt32)
                   || type == typeof(UInt64)
                   || type == typeof(Decimal)
                   || type == typeof(Double)
                   || type == typeof(Single);
        }
        #endregion

        #region 字段信息反射
        /// <summary>反射获得类型字段信息</summary>
        public static FieldInfo GetFieldInfo(object instance, string fieldName)
        {
            return GetFieldInfo(instance.GetType(), fieldName, false);
        }
        /// <summary>反射获得枚举值的字段信息</summary>
        public static FieldInfo GetFieldInfo(Enum value)
        {
            return GetFieldInfo(value.GetType(), value.ToString(), true);
        }
        /// <summary>反射获得字段信息</summary>
        public static FieldInfo GetFieldInfo(Type type, string fieldName, bool IsStatic = false)
        {
            var flags = IsStatic ? StaticFlags : InstanceFlags;
            return type.GetField(fieldName, flags);
        }
        /// <summary>反射获得字段值</summary>
        public static object GetFieldValue(object instance, string fieldName)
        {
            var fieldInfo = GetFieldInfo(instance, fieldName);
            return fieldInfo.GetValue(instance);
        }
        /// <summary>反射获得字段值</summary>
        public static T GetFieldValue<T>(object instance, string fieldName)
        {
            return (T)GetFieldValue(instance, fieldName);
        }
        /// <summary>反射设置字段值</summary>
        public static void SetFieldValue(object instance, string fieldName, object value)
        {
            var fieldInfo = GetFieldInfo(instance, fieldName);
            fieldInfo.SetValue(instance, value);
        }
        /// <summary>根据字符串反射设置字段值</summary>
        public static void SetFieldValueFromString(object instance, string fieldName, string value)
        {
            var fieldInfo = GetFieldInfo(instance, fieldName);
            if (fieldInfo == null) return;
            object value2 = value.ChangeTo(fieldInfo.FieldType);
            fieldInfo.SetValue(instance, value2);
        }
        #endregion

        #region 方法信息反射
        /// <summary>反射调用实例方法</summary>
        public static object InvokeMethod(object instance, string methodName, params object[] args)
        {
            return instance.GetType().InvokeMember(methodName, InstanceFlags | BindingFlags.InvokeMethod, null, instance, args);
        }
        /// <summary>反射调用静态方法</summary>
        public static object InvokeMethod(Type type, string methodName, params object[] args)
        {
            return type.InvokeMember(methodName, BindingFlags.InvokeMethod | StaticFlags | BindingFlags.FlattenHierarchy, null, null, args);
        }
        /// <summary>反射调用静态方法</summary>
        public static T InvokeMethod<T>(Type type, string methodName, params object[] args)
        {
            return (T)InvokeMethod(type, methodName, args);
        }
        /// <summary>创建静态方法委托</summary>
        public static Action<T> CreateActionDelegate<T>(Type type, string methodName)
        {
            var methodInfo = type.GetMethod(methodName, StaticFlags, null, new Type[] { typeof(T) }, null);
            Action<object, object[]> actionDelegate = CreateActionDelegate(methodInfo);
            return parameter => actionDelegate(null, new object[] { parameter });
        }
        /// <summary>创建静态方法委托</summary>
        public static Action<T> CreateActionDelegate<T>(object instance, string methodName)
        {
            var methodInfo = instance.GetType().GetMethod(methodName, InstanceFlags, null, new Type[] { typeof(T) }, null);
            Action<object, object[]> actionDelegate = CreateActionDelegate(methodInfo);
            return parameter => actionDelegate(instance, new object[] { parameter });
        }
        /// <summary>创建方法委托</summary>
        private static Action<object, object[]> CreateActionDelegate(MethodInfo methodInfo)
        {
            // parameters to execute
            ParameterExpression parametersParameter = Expression.Parameter(typeof(object[]), "parameters");
            // build parameter list
            List<Expression> parameterExpressions = new List<Expression>();
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                // (Ti)parameters[i]
                BinaryExpression valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(valueObj, paramInfos[i].ParameterType);
                parameterExpressions.Add(valueCast);
            }
            // non-instance for static method, or ((TInstance)instance)
            ParameterExpression instanceParameter = null;
            Expression instanceCast = null;
            if (!methodInfo.IsStatic)
            {
                instanceParameter = Expression.Parameter(typeof(object), "instance");
                instanceCast = Expression.Convert(instanceParameter, methodInfo.ReflectedType);
            }
            // static invoke or ((TInstance)instance).Method
            MethodCallExpression methodCall = Expression.Call(instanceCast, methodInfo, parameterExpressions);
            // ((TInstance)instance).Method((T0)parameters[0], (T1)parameters[1], ...)
            Expression<Action<object, object[]>> lambda = Expression.Lambda<Action<object, object[]>>(methodCall, instanceParameter, parametersParameter);
            return lambda.Compile();
        }
        /// <summary>创建静态方法委托</summary>
        public static Func<T, TResult> CreateFuncDelegate<T, TResult>(Type type, string methodName)
        {
            MethodInfo methodInfo = type.GetMethod(methodName, StaticFlags);
            Func<object, object[], object> funcDelegate = CreateFuncDelegate(methodInfo);
            return parameter => (TResult)funcDelegate(null, new object[] { parameter });
        }
        /// <summary>创建方法委托</summary>
        private static Func<object, object[], object> CreateFuncDelegate(MethodInfo methodInfo)
        {
            // parameters to execute
            ParameterExpression instanceParameter = Expression.Parameter(typeof(object), "instance");
            ParameterExpression parametersParameter = Expression.Parameter(typeof(object[]), "parameters");
            // build parameter list
            List<Expression> parameterExpressions = new List<Expression>();
            ParameterInfo[] paramInfos = methodInfo.GetParameters();
            for (int i = 0; i < paramInfos.Length; i++)
            {
                // (Ti)parameters[i]
                BinaryExpression valueObj = Expression.ArrayIndex(parametersParameter, Expression.Constant(i));
                UnaryExpression valueCast = Expression.Convert(valueObj, paramInfos[i].ParameterType);
                parameterExpressions.Add(valueCast);
            }
            // non-instance for static method, or ((TInstance)instance)
            Expression instanceCast = methodInfo.IsStatic ? null : Expression.Convert(instanceParameter, methodInfo.ReflectedType);
            // static invoke or ((TInstance)instance).Method
            MethodCallExpression methodCall = Expression.Call(instanceCast, methodInfo, parameterExpressions);
            // ((TInstance)instance).Method((T0)parameters[0], (T1)parameters[1], ...)
            UnaryExpression castMethodCall = Expression.Convert(methodCall, typeof(object));
            Expression<Func<object, object[], object>> lambda = Expression.Lambda<Func<object, object[], object>>(castMethodCall, instanceParameter, parametersParameter);
            return lambda.Compile();
        }
        #endregion

        #region 属性信息反射
        /// <summary>反射获得属性信息</summary>
        public static PropertyInfo GetPropertyInfo(object instance, string propertyName)
        {
            return GetPropertyInfo(instance.GetType(), propertyName, false);
        }
        /// <summary>反射获得属性信息</summary>
        public static PropertyInfo GetPropertyInfo(Type type, string propertyName, bool IsStatic = false)
        {
            var flags = IsStatic ? StaticFlags : InstanceFlags;
            return type.GetProperty(propertyName, flags);
        }
        /// <summary>反射获得属性值</summary>
        public static object GetPropertyValue(object instance, string propertyName)
        {
            return GetPropertyInfo(instance, propertyName).GetValue(instance, null);
        }
        /// <summary>反射获得属性值</summary>
        public static T GetPropertyValue<T>(object instance, string propertyName)
        {
            return (T)GetPropertyValue(instance, propertyName);
        }
        /// <summary>反射设置属性值</summary>
        public static void SetPropertyValue(object instance, string propertyName, object value)
        {
            var pinfo = GetPropertyInfo(instance, propertyName);
            pinfo.SetValue(instance, value, null);
        }
        /// <summary>根据字符串反射设置属性值</summary>
        public static void SetPropertyValueFromString(object instance, string propertyName, string value)
        {
            var pinfo = GetPropertyInfo(instance, propertyName);
            if (pinfo == null) return;
            object value2 = value.ChangeTo(pinfo.PropertyType);
            pinfo.SetValue(instance, value2, null);
        }
        /// <summary>转换为目标类型</summary>
        public static object ChangeTo(this string value, Type type)
        {
            if (typeof(string) == type) return value;
            if (type.IsEnum) return value.ParseEnum(type);
            return Convert.ChangeType(value, type);
        }
        /// <summary>转换为枚举值</summary>
        public static object ParseEnum(this string value, Type type)
        {
            return Enum.Parse(type, value);
        }
        #endregion

        #region 特性反射
        /// <summary>获得特性数组</summary>
        public static TAttribute[] GetAttributes<TAttribute>(this ICustomAttributeProvider instance, bool inherit = false)
            where TAttribute : Attribute
        {
            var attributeType = typeof(TAttribute);
            return instance.GetCustomAttributes(attributeType, inherit).Cast<TAttribute>().ToArray();
        }
        #endregion

        #region 程序集反射
        /// <summary>创建接口实例</summary>
        public static T CreateInterfaceInstance<T>(this Assembly assembly)
            where T : class
        {
            var typeInterface = typeof(T);
            var typeImplement = assembly.GetTypes().First(type => typeInterface.IsAssignableFrom(type));
            return Activator.CreateInstance(typeImplement).As<T>();
        }
        /// <summary>如果是加载全部，则获得已加载的所有程序集，否则从配置文件中加载</summary>
        public static IEnumerable<Assembly> GetAssemblies(bool loadAll, string sectionName)
        {
            IEnumerable<Assembly> assemblies;
            if (loadAll)
            {
                assemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
            else
            {
                var fileConfig = ConfigurationManager.GetSection(sectionName).As<NameValueCollection>();
                if (fileConfig == null)
                {
                    assemblies = new Assembly[0];
                }
                else
                {
                    assemblies = fileConfig.Keys.Cast<string>().Select(Assembly.Load).ToArray();
                }
            }
            return assemblies;
        }
        #endregion
    }
}
