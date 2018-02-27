using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 一般性的树结构
    /// </summary>
    /// <typeparam name="T">作为树结点的数据类型</typeparam>
    public class JTree<T> : IEnumerable<T>
    {
        /// <summary>
        /// 树的结点数据
        /// </summary>
        public T Node { get; set; }

        /// <summary>
        /// 树的父结点
        /// </summary>
        public JTree<T> Parent { get; set; }

        private List<JTree<T>> mChildren;

        /// <summary>
        /// 树的子结点集合
        /// </summary>
        public IEnumerable<JTree<T>> Children
        {
            get { return mChildren; }
        }

        /// <summary>
        /// 直接子元素的个数
        /// </summary>
        public int Length
        {
            get
            {
                return mChildren.Count;
            }
        }

        /// <summary>
        /// 用一个根结点初始化一个树
        /// </summary>
        /// <param name="root">根结点的数据</param>
        public JTree(T root)
            : this()
        {
            Node = root;
        }

        /// <summary>
        /// 初始化一个空树
        /// </summary>
        public JTree()
        {
            mChildren = new List<JTree<T>>();
        }

        /// <summary>
        /// 将一个数据加入树的子结点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public JTree<T> Add(T node)
        {
            JTree<T> child = new JTree<T>(node);

            mChildren.Add(child);
            child.Parent = this;
            return child;
        }

        /// <summary>
        /// 在整树中查找，从树中删除指定数据的结点
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public JTree<T> Remove(T node)
        {
            var treeToRemove = Find(node);
            if (treeToRemove.Parent == null)
            {
                return null;
            }
            treeToRemove.Parent.mChildren.Remove(treeToRemove);
            return treeToRemove;
        }

        /// <summary>
        /// 删除树的第一级子树
        /// </summary>
        /// <param name="child">第一级子树</param>
        /// <returns>被删除的子树</returns>
        public JTree<T> Remove(JTree<T> child)
        {
            return mChildren.Remove(child) ? child : null;
        }

        //public void Remove(T node)
        //{
        //    JTree<T> treeToRemove = null;
        //    foreach (var child in mChildren)
        //    {
        //        if (EqualityComparer<T>.Default.Equals(child.Node, node))
        //        {
        //            treeToRemove = child;
        //            break;
        //        }
        //    }

        //    if (treeToRemove != null)
        //    {
        //        mChildren.Remove(treeToRemove);
        //    }
        //}

        /// <summary>
        /// 在整树中查找指定数据的子树
        /// </summary>
        /// <param name="node">要查找的数据</param>
        /// <returns>找到的子树，找不到返回null</returns>
        public JTree<T> Find(T node)
        {
            if (EqualityComparer<T>.Default.Equals(Node, node))
            {
                return this;
            }
            foreach (var child in mChildren)
            {
                var tree = child.Find(node);
                if (tree != null) return tree;
            }
            return null;
        }

        /// <summary>
        /// 在整树中通过指定条件查找第一个满足条件的子树
        /// </summary>
        /// <param name="istrue">判断条件</param>
        /// <returns>子树</returns>
        public JTree<T> FindTree(Func<T, bool> istrue)
        {
            foreach (var node in GetTempList())
            {
                if (istrue(node))
                {
                    return Find(node);
                }
            }
            return null;
        }

        ///// <summary>
        ///// 通过指定条件查找第一个满足条件的祖先
        ///// </summary>
        ///// <param name="istrue">判断条件</param>
        ///// <returns>祖先树</returns>
        //public JTree<T> FindAncestor(Func<T, bool> istrue)
        //{
        //    while (true)
        //    {
              
        //    }
        //}

        /// <summary>
        /// 在整树中通过指定条件查找所有满足条件的子树
        /// </summary>
        /// <param name="istrue"></param>
        /// <returns></returns>
        public IEnumerable<JTree<T>> FindTrees(Func<T, bool> istrue)
        {
            foreach (var node in GetTempList())
            {
                if (istrue(node))
                {
                    yield return Find(node);
                }
            }
        }

        private List<T> mTempList = null;

        private IEnumerable<T> GetTempList()
        {
            yield return this.Node;
            foreach (JTree<T> tree in mChildren)
            {
                foreach (var node in tree.GetTempList())
                {
                    yield return node;
                }
            }
        }

        /// <summary>
        /// 获取遍历树中所有数据的枚举器
        /// </summary>
        /// <returns>枚举器</returns>
        public IEnumerator<T> GetEnumerator()
        {
            GetTempList();
            return mTempList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
