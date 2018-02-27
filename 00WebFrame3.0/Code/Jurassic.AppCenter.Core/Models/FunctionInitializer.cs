using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using Jurassic.Com.Tools;
using System.Text.RegularExpressions;
using Jurassic.AppCenter.Logs;

namespace Jurassic.AppCenter.Models
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 扫描程序集，初始化权限数据
    /// 新的扫描结果对照已有的功能列表，保证原有功能的ID不变
    /// </summary>
    public class FunctionInitializer
    {
        JTree<AppFunction> mFuncTrees = new JTree<AppFunction>(new AppFunction());
        List<AppFunction> mOldList;
        DataManager<AppFunction> mFunctionMgr = AppManager.Instance.FunctionManager;

        /// <summary>
        /// 建立一个扫描DLL获取所有功能的对象
        /// 新的扫描结果对照已有的功能列表，保证原有对象的ID不变
        /// </summary>
        public FunctionInitializer()
        {
            mOldList = mFunctionMgr.GetAll().ToList();
        }

        /// <summary>
        /// 根据程序集列表，反射生成系统功能列表
        /// </summary>
        /// <param name="arrPaths">程序集所在路径名称列表</param>
        public void InitFunctions(IEnumerable<string> arrPaths)
        {
            List<Assembly> allAsses = arrPaths
                .Select(dllPath => Assembly.LoadFrom(dllPath)).ToList();

            var selfAss = this.GetType().Assembly;

            if (!allAsses.Any(ass => ass.FullName == selfAss.FullName))
            {
                allAsses.Insert(0, selfAss);
            }

            foreach (var ass in allAsses)
            {
                var menu = GetMenu(ass);
                AppFunction assFunc = new AppFunction()
                {
                    Name = menu.Name,
                    ControllerName = ass.GetName().Name,
                    Ord = menu.Ord,
                    Id = menu.Id,
                    Visible = menu.Visible,
                    Location = GetAssUrl(ass),
                    AuthType = menu.AuthType
                };
                var root = mFuncTrees.Add(assFunc);
                GetFunctions(root, ass);
                //对该程序集，如果一个功能都没有被加入
                if (root.Length == 0)
                {
                    mFuncTrees.Remove(root);
                }
            }

            mFunctionMgr.Clear();
            AddToMgr(mFuncTrees);
        }

        string CreateId(JTree<AppFunction> tree)
        {
            if (tree.Parent == null)
            {
                throw new JException("Root Can not have a Id");
            }
            Regex mIdPattern = new Regex(String.Format(@"^{0}(\d*)$", tree.Parent.Node.Id));

            int maxId = 0;
            foreach (var t in tree.Parent.Children)
            {
                if (t.Node.Id.IsEmpty()) continue;

                var match = mIdPattern.Match(t.Node.Id);
                if (match != null)
                {
                    maxId = Math.Max(maxId, match.Groups[1].Value.ToInt());
                }
            }
            return tree.Parent.Node.Id + (maxId + 1).ToString("00");
        }

        private void AddToMgr(JTree<AppFunction> tree)
        {
            if (tree.Node.Id.IsEmpty() && tree != mFuncTrees)
            {
                tree.Node.Id = CreateId(tree);
            }
            if (tree != mFuncTrees)
            {
                mFunctionMgr.Add(tree.Node);
            }

            int ord = 0;
            foreach (var child in tree.Children.OrderBy(c => c.Node.Ord).ThenBy(c => c.Node.Id))
            {
                child.Node.Ord = ord++;
                child.Node.ParentId = tree.Node.Id;
                AddToMgr(child);
            }
        }

        private string GetAssUrl(Assembly ass)
        {
            return ass == this.GetType().Assembly ? "/AppCenter" : "/";
        }

        /// <summary>
        /// 根据程序集反射出所有的需权限的控制器和功能
        /// </summary>
        /// <param name="rootTree">程序集根结点</param>
        /// <param name="assembly">程序集</param>
        void GetFunctions(JTree<AppFunction> rootTree, Assembly assembly)
        {
            var functionInfos =
            (from t in assembly.GetTypes()
             where !t.IsAbstract && t.Name.EndsWith("Controller")
             from m in t.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
             where !m.IsSpecialName
             select new MvcFuncInfo()
             {
                 Controller = t,
                 Action = m,
                 ControllerName = t.Name.Substring(0, t.Name.Length - 10),
                 ActionName = m.Name,
                 Parameters = m.GetParameters()
             }).ToList();

            List<AppFunction> functionList = new List<AppFunction>();

            foreach (var groupInfo in functionInfos.GroupBy(f => f.Controller))
            {
                var menu = GetMenu(groupInfo.Key, rootTree.Node.AuthType, rootTree.Node.LogType);

                //生成Controller作为二级的功能项
                AppFunction parent = new AppFunction()
                {
                    Name = menu.Name,
                    Location = menu.AreaName + "/" + groupInfo.First().ControllerName,
                    ControllerName = groupInfo.Key.FullName,
                    Visible = menu.Visible,
                    AuthType = menu.AuthType,
                    LogType = menu.LogType,
                    Ord = menu.Ord,
                };

                if (parent.AuthType == JAuthType.Ignore) continue;

                parent.LocationSamples = parent.Location;
                ReDefineId(menu, parent);
                var parentTree = rootTree.Add(parent);

                //生成Controller下面的Action作为三级的功能项
                foreach (var info in groupInfo)
                {
                    info.AuthType = parent.AuthType;
                    //提取Attributes
                    GetAttributes(info);

                    menu = GetMenu(info);
                    AppFunction child = new AppFunction()
                    {
                        Name = menu.Name,
                        Location = menu.AreaName + info.ToString(),
                        ParentId = parent.Id,
                        Visible = menu.Visible,

                        Parameters = info.Parameters.Select(p =>
                        {
                            var pa = new AppParameter
                                {
                                    Name = p.Name,
                                };
                            if (p.ParameterType.Name.StartsWith("Int"))
                            {
                                pa.ValuePattern = @"\d+";
                            }
                            return pa;
                        }).ToList(),
                        Method = info.HasPost ? WebMethod.POST : WebMethod.GET,
                        Ord = menu.Ord,
                        AuthType = menu.AuthType,
                        LogType = menu.LogType,
                        ControllerName = parent.ControllerName,
                        ActionName = info.ActionName
                    };

                    if (child.AuthType == JAuthType.Ignore) continue;

                    ReDefineId(menu, child);

                    if (child.Method == WebMethod.GET)
                    {
                        MakeRegTail(child, info);
                    }
                    else
                    {
                        MakePostRegTail(child, info);
                    }
                    parentTree.Add(child);
                }
            }
        }

        private void GetAttributes(MvcFuncInfo funcInfo)
        {
            foreach (var attr in funcInfo.Action.GetCustomAttributes(false))
            {
                if (attr.GetType().Name == "HttpPostAttribute")
                {
                    funcInfo.HasPost = true;
                    return;
                }
            }
        }

        void ReDefineId(JAuthAttribute menu, AppFunction func)
        {
            var fOld = mOldList.FirstOrDefault(f => f.IsTheSame(func));

            if (fOld != null)
            {
                func.Id = fOld.Id;
                return;
            }

            if (menu.Id.IsEmpty()) return;

            var existsFunc = mFuncTrees.FirstOrDefault(f => f.Id == menu.Id);
            if (existsFunc != null)
            {
                throw new ArgumentException(
                    String.Format("不能为功能'{0}'分配Id={1}, 已有相同ID的功能:'{2}'",
                    func.Name, menu.Id, existsFunc.Name));
            }

            func.Id = menu.Id;
        }

        /// <summary>
        /// 生成POST的Action的URL规则
        /// </summary>
        /// <param name="child"></param>
        /// <param name="info"></param>
        private void MakePostRegTail(AppFunction child, MvcFuncInfo info)
        {
            child.LocationSamples = child.Location + Environment.NewLine + child.Location + "/123";
            child.RegTail = "(/\\d*)?";
        }

        /// <summary>
        /// 生成GET的Action的URL规则
        /// </summary>
        /// <param name="child"></param>
        /// <param name="info"></param>
        private void MakeRegTail(AppFunction child, MvcFuncInfo info)
        {
            ParameterInfo pi = info.Parameters
                .FirstOrDefault(p => p.Name.Equals("id", StringComparison.OrdinalIgnoreCase));

            if (pi == null) return;
            if (!pi.ParameterType.Name.Contains("Int"))
            {
                return;
            }
            else if (!CommOp.IsEmpty(pi.DefaultValue))
            {
                child.LocationSamples = child.Location + Environment.NewLine + child.Location + "/123";
                child.RegTail = "(/\\d*)?";
            }
            else
            {
                child.LocationSamples = child.Location + "/123";
                child.RegTail = "/\\d+";
            }
        }

        JAuthAttribute GetMenu(Type controllerType, JAuthType defaultAuthType, JLogType defaultLogType)
        {
            string controllerName = controllerType.Name.Substring(0, controllerType.Name.Length - 10);
            var attr = controllerType.GetCustomAttributes(false)
                  .FirstOrDefault(co => co is JAuthAttribute) as JAuthAttribute;

            if (attr == null)
            {
                attr = new JAuthAttribute(defaultAuthType, controllerName);
                attr.LogType = defaultLogType;
            }
            if (attr.Name.IsEmpty()) attr.Name = controllerName;

            if (controllerType.Assembly == this.GetType().Assembly)
            {
                attr.AreaName = "/AppCenter";
            }
            return attr;
        }

        JAuthAttribute GetMenu(MvcFuncInfo info)
        {
            var attr = info.Action.GetCustomAttributes(false)
                  .FirstOrDefault(co => co is JAuthAttribute) as JAuthAttribute;
            attr = attr ?? new JAuthAttribute(info.AuthType, info.Action.Name);
            if (attr.Name.IsEmpty()) attr.Name = info.Action.Name;
            if (info.Action.DeclaringType.Assembly == this.GetType().Assembly)
            {
                attr.AreaName = "/AppCenter";
            }
            return attr;
        }

        JAuthAttribute GetMenu(Assembly ass)
        {
            var displayAttr = ass.GetCustomAttributes(false)
                  .FirstOrDefault(co => co is JAuthAttribute) as JAuthAttribute;
            var attr = displayAttr ?? new JAuthAttribute(AppManager.Instance.DefaultAuthType);

            if (attr.Name.IsEmpty()) attr.Name = ass.GetName().FullName.Split(',')[0];
            if (ass == this.GetType().Assembly)
            {
                attr.AreaName = "/AppCenter";
            }
            return attr;
        }

        /// <summary>
        /// Formats the name of the controller,remove all of the namespace information from the controller names
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        private static string FormatControllerName(string typeName)
        {
            return typeName.Substring(typeName.LastIndexOf('.') + 1);
        }

    }
}