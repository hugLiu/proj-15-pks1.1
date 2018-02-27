using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebQuery
{
    /// <summary>
    /// 高级查询管理类
    /// </summary>
    public class AdvQueryManager : IDisposable
    {
        ArticleManager _article;
        public AdvQueryManager(ArticleManager article)
        {
            _article = article;
        }

        /// <summary>
        /// 返回当前用户保存过的当前模型的查询表达式列表
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public IEnumerable<AdvQueryItem> GetUserQuerys(string modelName)
        {
            var arts = GetUserQueryArticle(modelName).ToList();
            return _article.MatchTexts(arts)
            .Select(ca => new AdvQueryItem
            {
                Id = ca.Id,
                Name = ca.Article.Title,
                Nodes = JsonHelper.FromJson<List<AdvQueryNode>>(ca.Article.Text),
            });
        }

        private IQueryable<Base_CatalogArticle> GetUserQueryArticle(string modelName)
        {
            int userId = CommOp.ToInt(AppManager.Instance.GetCurrentUserId());
            int modelNameId = SiteManager.Catalog.GetExtByName(AdvQuery.Query.Id, AdvQuery.Query.ModelName).Id;
            var arts = _article.GetAllAtCatalog(AdvQuery.Query.Id)
                 .Where(ca => ca.Article.EditorId == userId && ca.Article.Exts.Any(ext => ext.CatlogExtId == modelNameId && ext.Value == modelName));
            return arts;
        }

        /// <summary>
        /// 根据搜索项的ID查找对应的搜索项
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public AdvQueryItem GetQueryItemById(int itemId)
        {
            var ca = _article.GetById(itemId);
            return new AdvQueryItem
            {
                Id = ca.Id,
                Name = ca.Article.Title,
                Nodes = JsonHelper.FromJson<List<AdvQueryNode>>(ca.Article.Text),
            };
        }

        /// <summary>
        /// 根据查询名称获取高级查询对象
        /// </summary>
        /// <param name="queryName">查询对象的名称</param>
        /// <param name="modelName">要查询的实体类型全名</param>
        /// <returns></returns>
        public AdvQueryItem GetQueryItemByName(string queryName, string modelName)
        {
            int userId = CommOp.ToInt(AppManager.Instance.GetCurrentUserId());
            var ca = GetUserQueryArticle(modelName).FirstOrDefault(ca1 => ca1.Article.Title.Equals(queryName, StringComparison.OrdinalIgnoreCase));
            _article.MatchTexts(ca.Article);
            return new AdvQueryItem()
            {
                Id = ca.Id,
                Name = ca.Article.Title,
                Nodes = JsonHelper.FromJson<List<AdvQueryNode>>(ca.Article.Text),
            };
        }

        /// <summary>
        /// 保存查询对象，根据是否是新对象决定是插入还是改写
        /// </summary>
        /// <param name="item">查询对象</param>
        public void Save(AdvQueryItem item)
        {
            var ca = GetUserQueryArticle(item.ModelName).FirstOrDefault(ca1 => ca1.Article.Title.Equals(item.Name, StringComparison.OrdinalIgnoreCase));
            if (ca == null)
            {
                ca = _article.CreateByCatalog(AdvQuery.Query.Id);
                ca.Article.State = ArticleState.Published;
            }
            ca.Article.Title = item.Name;
            ca.Article.Text = JsonHelper.ToJson(item.Nodes);
            ca.Article.EditorId = CommOp.ToInt(AppManager.Instance.GetCurrentUserId());
            ca.SetExt(AdvQuery.Query.ModelName, item.ModelName);
            _article.Save(ca);
            item.Id = ca.Id;
        }

        /// <summary>
        /// 去掉不必要的首尾空格
        /// </summary>
        /// <param name="item">高级查询对象</param>
        public void ClearQueryItem(AdvQueryItem item)
        {
            item.Nodes.ForEach(node =>
            {
                node.Expression = CommOp.ToStr(node.Expression);
                node.ExpressionText = CommOp.ToStr(node.ExpressionText);
                node.Value = CommOp.ToStr(node.Value);
                node.ValueText = CommOp.ToStr(node.ValueText);
            });
        }

        /// <summary>
        /// 根据指定的查询表达式和查询类型，查询指定集合
        /// </summary>
        /// <typeparam name="T">泛型集合的实体类型</typeparam>
        /// <param name="q">待查询的IQueryable集合</param>
        /// <param name="querys">前台传递的代表表达式树的json数据</param>
        /// <param name="queryType">查询的种类，当为0时，表示只计算顶级的AND/OR， 当为1时表示要计算整个表达式树</param>
        /// <returns></returns>
        public IQueryable<T> Query<T>(IQueryable<T> q, string querys, int queryType) where T : class
        {
            var queryItem = new AdvQueryItem()
            {
                ModelName = typeof(T).AssemblyQualifiedName,
                Nodes = JsonHelper.FromJson<List<AdvQueryNode>>(querys)
            };
            ClearQueryItem(queryItem);
            if (queryType == 0 && !queryItem.Nodes.IsEmpty())
            {
                queryItem.Nodes = GetPlainNodes(queryItem.Nodes).ToList();
            }
            return Query(q, queryItem) as IQueryable<T>;
        }

        /// <summary>
        /// 只考虑顶级的AND/OR的表达式树
        /// </summary>
        /// <param name="nodes">完整查询表达式结点集合</param>
        /// <returns>只考虑顶级AND/OR之后的结点集合</returns>
        private IEnumerable<AdvQueryNode> GetPlainNodes(IEnumerable<AdvQueryNode> nodes)
        {
            var root = nodes.FirstOrDefault();
            yield return root;
            foreach (var node in nodes.Skip(1).Where(n => n.Type != "Operator"))
            {
                node.ParentId = root.Id;
                yield return node;
            }
        }

        /// <summary>
        /// 根据指定的查询对象名称，查询指定集合
        /// </summary>
        /// <param name="q">查询对象</param>
        /// <param name="querys"></param>
        /// <returns></returns>
        public IQueryable Query(IQueryable q, string querys)
        {
            Type t = q.GetType().GetGenericArguments().FirstOrDefault();
            if (t == null)
            {
                throw new ArgumentException("q必须是泛型的IQueryable");
            }

            var queryItem = new AdvQueryItem()
            {
                ModelName = t.AssemblyQualifiedName,
                Nodes = JsonHelper.FromJson<List<AdvQueryNode>>(querys)
            };
            ClearQueryItem(queryItem);
            return Query(q, queryItem);
        }


        /// <summary>
        /// 根据高级查询对象，查询指定集合
        /// </summary>
        /// <param name="q">待筛选的集合</param>
        /// <param name="queryItem">高级查询对象</param>
        /// <returns></returns>
        public IQueryable Query(IQueryable q, AdvQueryItem queryItem)
        {

            //解析出查询表达式
            Tuple<string, object[]> expr = CalcFinalExpr(queryItem);
            //是否有查询条件
            if (expr == null)
            {
                return q;
            }
            //查询表达式和参数数值
            return q.Where(expr.Item1, expr.Item2);
        }

        /// <summary>
        /// 根据结点数据计算最终条件表达式的值
        /// </summary>
        /// <param name="item">高级查询对象</param>
        /// <returns>二元组</returns>
        Tuple<string, object[]> CalcFinalExpr(AdvQueryItem item)
        {
            //检查查询项数据
            if (item == null || item.Nodes == null)
            {
                return null;
            }
            if (!item.Nodes.Exists(t => t.ParentId == 0))
            {
                return null;
            }

            string whereExp = "";
            IList<object> parameters = new List<object>();

            //查询到根节点。
            AdvQueryNode rootNode = item.Nodes.FirstOrDefault(t => t.ParentId == 0);

            //生成ODT模型的对象
            object odtObj = RefHelper.LoadClass(item.ModelName);

            ProcessQueryNode(rootNode, item, odtObj, ref whereExp, ref parameters);
            //是否有查询条件
            if (parameters.Count == 0)
            {
                return null;
            }

            Tuple<string, object[]> result = new Tuple<string, object[]>(whereExp, parameters.ToArray());
            return result;
        }

        /// <summary>
        /// 处理一个查询定义节点。依次递归子节点循环。
        /// </summary>
        /// <param name="node">当前节点</param>
        /// <param name="item">查询项集合</param>
        /// <param name="odtObj"></param>
        /// <param name="exp">查询表达式</param>
        /// <param name="parameters">查询表达式参数</param>
        private void ProcessQueryNode(AdvQueryNode node, AdvQueryItem item, object odtObj, ref string exp, ref IList<object> parameters)
        {
            if (node == null) return;

            //查询当前节点的子节点
            List<AdvQueryNode> children = item.Nodes.Where(t => t.ParentId == node.Id).ToList();

            if (children.Count == 0)
            {
                //没有子节点，这是一个表达式节点

                if (node.Operator.IsEmpty()) return;

                string p = "@" + parameters.Count;

                if (node.Operator.Trim().ToUpper().Equals("LIKE"))
                {
                    exp = exp + node.Expression.Trim() + ".Contains(" + p + ") ";
                }
                else if (node.Operator.Trim().ToUpper().Equals("NOT LIKE"))
                {
                    exp = exp + " !" + node.Expression.Trim() + ".Contains(" + p + ") ";
                }
                else
                {
                    exp = exp + " (" + node.Expression.Trim() + node.Operator + p + ") ";
                }

                RefHelper.SetValue(odtObj, node.Expression, node.Value);
                parameters.Add(RefHelper.GetValue(odtObj, node.Expression));

                return;
            }
            else
            {
                //有子节点，这是一个逻辑运算符节点

                exp = exp + "(";
                int i = 0;
                foreach (AdvQueryNode child in children)
                {
                    //子节点是逻辑运算符
                    if (child.Type.ToLower() == "operator")
                    {
                        if (!item.Nodes.Exists(t => t.ParentId == child.Id))
                        {
                            continue;
                        }
                    }

                    if (i > 0)
                    {
                        exp = exp + " " + node.Expression + " ";
                    }
                    //递归子节点循环。
                    ProcessQueryNode(child, item, odtObj, ref exp, ref parameters);

                    i++;

                }
                exp = exp + ")";
            }
        }

        public void Delete(int id)
        {
            _article.Delete(id);
        }

        public void Dispose()
        {
            _article.Dispose();
        }
    }
}