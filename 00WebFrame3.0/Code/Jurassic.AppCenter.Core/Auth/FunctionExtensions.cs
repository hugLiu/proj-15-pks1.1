using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 权限判断的扩展类
    /// </summary>
    public static class FunctionExtensions
    {
        /// <summary>
        /// 判断有无下级
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static bool HasChildren(this AppFunction func)
        {
            return AppManager.Instance.FunctionManager.GetAll().Any(f => f.ParentId == func.Id);
        }

        /// <summary>
        /// 获取功能项的直接下级列表
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IEnumerable<AppFunction> GetChildren(this AppFunction func)
        {
            return AppManager.Instance.FunctionManager.GetAll().Where(f => f.ParentId == func.Id)
                .OrderBy(f=>f.Ord);
        }

        /// <summary>
        /// 获取一个权限结点的所有上级和下级以及它本身
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static List<AppFunction> GetFamily(this AppFunction func)
        {
            List<AppFunction> funcs = new List<AppFunction>();
            var f = func;
            while (f != null)
            {
                funcs.Add(f);
                f = AppManager.Instance.FunctionManager.GetById(f.ParentId);
            }

            funcs.AddRange(GetChildren(func));

            return funcs;
        }
    }
}
