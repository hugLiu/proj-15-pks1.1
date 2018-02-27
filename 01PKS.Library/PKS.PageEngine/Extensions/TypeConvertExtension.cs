using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PKS.PageEngine.Query;

namespace PKS.PageEngine.Extensions
{
    public static class TypeConvertExtension
    {
        public static object ToSimpleType(this object obj, string dataType)
        {
            if (string.IsNullOrWhiteSpace(dataType))
                return obj;
         
            string type = dataType.ToLower();
         
            if (type == "bool" || type == "boolean")
            {
                if (obj == null)
                    return false;
                return Convert.ChangeType(obj,  TypeCode.Boolean);
            }
            if (type == "int" || type == "int32")
            {
                if (obj == null)
                    return 0;
                return Convert.ChangeType(obj, TypeCode.Int32);
            }
            if (type == "decimal")
            {
                if (obj == null)
                    return 0;
                return Convert.ChangeType(obj, TypeCode.Decimal);
            }
            if (type == "double"||type=="number")
            {
                if (obj == null)
                    return 0;
                return Convert.ChangeType(obj, TypeCode.Double);
            }
            if (type == "datetime")
            {
                if (obj == null)
                    return null;
                return Convert.ChangeType(obj, TypeCode.DateTime);
            }

            if (type == "string")
            {
                if (obj == null)
                    return null;
                return Convert.ChangeType(obj, TypeCode.String);
            }
            return obj;
        }

        public static OperationType ToOperationType(this string strOperationType)
        {
            if (string.IsNullOrEmpty(strOperationType))
                return OperationType.Equal;
            if(string.Equals(strOperationType,"eq",StringComparison.OrdinalIgnoreCase))
                return OperationType.Equal;
            if (string.Equals(strOperationType, "contains", StringComparison.OrdinalIgnoreCase))
                return OperationType.Contains;
            if (string.Equals(strOperationType, "in", StringComparison.OrdinalIgnoreCase))
                return OperationType.In;
            if (string.Equals(strOperationType, "gt", StringComparison.OrdinalIgnoreCase))
                return OperationType.GreaterThan;
            if (string.Equals(strOperationType, "gte", StringComparison.OrdinalIgnoreCase))
                return OperationType.GreaterThanEqual;
            if (string.Equals(strOperationType, "lt", StringComparison.OrdinalIgnoreCase))
                return OperationType.LessThan;
            if (string.Equals(strOperationType, "lte", StringComparison.OrdinalIgnoreCase))
                return OperationType.LessThanEqual;
            if (string.Equals(strOperationType, "ne", StringComparison.OrdinalIgnoreCase))
                return OperationType.NotEqual;
            return OperationType.Equal;
        }
    }
}
