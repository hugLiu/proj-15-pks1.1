using Jurassic.GF.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.GF.Interface
{
    public interface IObjTypeProperty
    {
        /// <summary>
        /// 根据对象类型ID获取对象类型的参数集信息
        /// </summary>
        /// <returns></returns>
        List<ObjTypePropertyModel> GetObjPropertyByBOTID(string BOTID);
    }
}
