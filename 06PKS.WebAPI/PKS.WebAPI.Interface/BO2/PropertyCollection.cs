using System;
using System.Collections.Generic;
using PKS.Utils;

namespace PKS.WebAPI.Models
{
    public class PropertyCollection: Dictionary<string, object>
    {
        public new void Add(string propertyName,object propertyValue)
        {
            if(!this.ValueAllowed(propertyValue))
            {
                throw new InvalidOperationException("值只能为字符串、布尔值、整数或小数及其数组，或者DateTime");
            }

            if (propertyValue is DateTime)
            {
                propertyValue = ((DateTime)propertyValue).ToISODateString();
            }

            if (base.ContainsKey(propertyName))
            {
                base.Remove(propertyName);
            }
            base.Add(propertyName, propertyValue);
        }

        private bool ValueAllowed(object value)
        {
            if (value is string || value is bool || value is int || value is decimal || value is DateTime
             || value is IList<string> || value is IList<bool> || value is IList<int> || value is IList<decimal>)
            {
                return true;
            }
            return false;
        }
    }
}