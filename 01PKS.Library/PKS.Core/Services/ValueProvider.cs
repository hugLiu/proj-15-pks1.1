using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PKS.Services
{
    /// <summary>值提供者接口</summary>
    public interface IValueProvider
    {
        /// <summary>获得值</summary>
        object GetValue(object context);
    }

    /// <summary>值提供者</summary>
    public abstract class ValueProvider : IValueProvider
    {
        /// <summary>获得值</summary>
        public abstract object GetValue(object context);
    }

    /// <summary>Guid值提供者</summary>
    public class GuidValueProvider : ValueProvider
    {
        /// <summary>获得值</summary>
        public override object GetValue(object context)
        {
            return Guid.NewGuid();
        }
    }
}
