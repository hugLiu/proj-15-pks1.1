using System.Collections.Generic;
using System.Linq;

namespace Jurassic.PKS.Service.Semantics
{
    /// <summary>
    /// 树结构
    /// </summary>
    public class TreeResult
    {
        /// <summary>
        /// 无参构造
        /// </summary>
        public TreeResult()
        {
            this.TreeItems = new List<TreeItem>();
        }
        /// <summary>
        /// 树节点集合
        /// </summary>
        public IList<TreeItem> TreeItems { get; set; }
        /// <summary>
        /// 树添加多个节点
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IList<TreeItem> collection)
        {
            if (collection != null && collection.Any())
            {
                foreach (var item in collection)
                {
                    this.TreeItems.Add(item);
                }
            }
        }
    }
    /// <summary>
    /// 树节点定义
    /// </summary>
    public class TreeItem
    {
        /// <summary>
        /// 无参构造
        /// </summary>
        public TreeItem() { }
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="id">节点id</param>
        /// <param name="pathTerm">叙词路径</param>
        /// <param name="term">叙词名称</param>
        /// <param name="orderIndex">节点排序</param>
        /// <param name="source">叙词来源</param>
        /// <param name="pid">父id</param>
        public TreeItem(string id, string pathTerm, string term, int orderIndex, string source, string pid)
        {
            Id = id;
            PathTerm = pathTerm;
            Term = term;
            OrderIndex = orderIndex;
            Source = source;
            Pid = pid;
        }
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="id">节点id</param>
        /// <param name="term">节点信息</param>
        /// <param name="pid">父id</param>
        public TreeItem(string id, string term, string pid)
        {
            Id = id;
            PathTerm = string.Empty;
            Term = term;
            OrderIndex = 0;
            Pid = pid;
        }
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="id">节点id</param>
        /// <param name="term">节点信息</param>
        /// <param name="pathTerm">节点路径</param>
        /// <param name="pid">父id</param>
        public TreeItem(string id, string term, string pathTerm, string pid)
        {
            Id = id;
            PathTerm = pathTerm;
            Term = term;
            OrderIndex = 0;
            Pid = pid;
        }
        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="term">叙词</param>
        public TreeItem(string term)
        {
            Id = string.Empty;
            PathTerm = string.Empty;
            Term = term;
            OrderIndex = 0;
            Pid = string.Empty;
        }
        /// <summary>
        /// 节点id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 叙词信息
        /// </summary>
        public string Term { get; set; }
        /// <summary>
        /// 叙词路径
        /// </summary>
        public string PathTerm { get; set; }
        /// <summary>
        /// 叙词来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 叙词排序
        /// </summary>
        public int? OrderIndex { get; set; }
        /// <summary>
        /// 父id
        /// </summary>
        public string Pid { get; set; }

    }
}
