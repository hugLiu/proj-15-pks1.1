using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    public interface IIdNameParentId<TId> : IIdNameBase<TId>
    {
        /// <summary>
        /// 用于表示父对象的ID
        /// </summary>
        TId ParentId { get; set; }
    }
}
