using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using PKS.Utils;

namespace PKS.Models
{
    /// <summary>树节点接口</summary>
    public interface ITreeNode
    {
        /// <summary>Id</summary>
        int Id { get; }
        /// <summary>文本</summary>
        string Text { get; }
        /// <summary>父节点Id</summary>
        int? ParentId { get; set; }
    }

    /// <summary>树节点接口</summary>
    public interface ITreeModel : ITreeNode
    {
        /// <summary>父节点</summary>
        ITreeModel Parent { get; set; }
        /// <summary>子节点集合</summary>
        List<ITreeModel> Children { get; set; }
    }

    /// <summary>树节点</summary>
    [JsonObject(NamingStrategyType = typeof(LowerCaseNamingStrategy))]
    public class TreeNode : ITreeNode
    {
        /// <summary>构造函数</summary>
        public TreeNode() { }
        /// <summary>Id</summary>
        public int Id { get; set; }
        /// <summary>父节点Id</summary>
        public int? ParentId { get; set; }
        /// <summary>父节点</summary>
        [JsonIgnore]
        public TreeNode Parent { get; set; }
        /// <summary>文本</summary>
        public string Text { get; set; }
        /// <summary>子节点集合</summary>
        public List<TreeNode> Children { get; set; }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }

    /// <summary>树节点扩展</summary>
    public static class TreeNodeExtension
    {
        /// <summary>转换为树节点</summary>
        public static TreeNode ToTreeNode<T>(this T model) where T : ITreeNode
        {
            return new TreeNode()
            {
                Id = model.Id,
                ParentId = model.ParentId,
                Text = model.Text
            };
        }
        /// <summary>转换为树节点集合</summary>
        public static IEnumerable<TreeNode> ToTreeNodes<T>(this IEnumerable<T> models) where T : ITreeNode
        {
            var nodes = models.Select(ToTreeNode).ToList();
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var children = nodes.FindAll(e => e != node && e.ParentId.HasValue && e.ParentId.Value == node.Id);
                if (children.Count > 0)
                {
                    children.ForEach(e => e.Parent = node);
                    node.Children = children;
                }
            }
            return nodes.Where(e => e.Parent == null).ToList();
        }
        /// <summary>生成树节点集合</summary>
        public static List<T> BuildTree<T>(this IList<T> nodes) where T : class, ITreeModel
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                var node = nodes[i];
                var children = nodes.Where(e => e != node && e.ParentId.HasValue && e.ParentId.Value == node.Id).Cast<ITreeModel>().ToList();
                if (children.Count > 0)
                {
                    children.ForEach(e => e.Parent = node);
                    node.Children = children;
                }
            }
            return nodes.Where(e => e.Parent == null).ToList();
        }
    }
}