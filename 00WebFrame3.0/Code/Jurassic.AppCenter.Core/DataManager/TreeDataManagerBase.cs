using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 用于管理线性结构存储的层级结构的数据
    /// </summary>
    /// <typeparam name="T">数据类型</typeparam>
    /// <typeparam name="TId">数据的主键类型</typeparam>
    public class TreeDataManagerBase<T, TId> : DataManagerBase<T, TId>
        where T : class,IIdNameParentId<TId>
    {
        /// <summary>
        /// 获取直接下级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetChildren(TId id)
        {
            return GetAll().Where(cat => cat.ParentId.Equals(id)).ToList();
        }

        /// <summary>
        /// 获取所有后代
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<T> GetDescendant(TId id)
        {
            List<T> descs = new List<T>();

            var children = GetChildren(id);

            foreach (var child in children)
            {
                descs.Add(child);
                descs.AddRange(GetDescendant(child.Id));
            }

            return descs;
        }

        TId GetRootId(string name)
        {
            return GetAll().First(cat => cat.Name == name && cat.ParentId == null).Id;
        }

        /// <summary>
        /// 批量删除结点
        /// </summary>
        /// <param name="idArr"></param>
        /// <returns></returns>
        public List<TId> Delete(params TId[] idArr)
        {
            if (idArr == null) return null;
            List<TId> deleted = new List<TId>();

            foreach (var id in idArr)
            {
                var func = GetById(id);
                var childIds = GetAll()
                    .Where(f => f.ParentId.Equals(id)).Select(f => f.Id);

                //ztree会把半选的父结点也传过来，所以要判断，如果所选父结点的子结点
                //有一个不删除，则父结点不删
                if (childIds.Any(cid => !idArr.Contains(cid)))
                {
                    continue;
                }
                Provider.Delete(func);
                deleted.Add(func.Id);
            }
            return deleted;
        }

        TId parentId;
        List<TId> idArr;
        /// <summary>
        /// 将选中结点移到另一个结点下
        /// </summary>
        /// <param name="ids">要移动的结点ID列表</param>
        /// <param name="pId">要移到的新结点ID，0代表移动到根结点</param>
        public void Move(IEnumerable<TId> ids, TId pId)
        {
            parentId = pId;
            idArr = ids.ToList();
            if (!parentId.IsDefault())
            {
                var func = GetById(parentId);
                if (func == null)
                {
                    throw new JException("结点未找到");
                }
            }
            else
            {
                parentId = default(TId);
            }
            if (idArr.Contains(pId))
            {
                throw new JException("结点不能移动到自身.");
            }
            while (idArr.Count > 0)
            {
                var id = idArr[0];
                CascadeMove(id);
            }
        }

        void CascadeMove(TId id, bool move = true)
        {
            var childIds = GetAll()
                        .Where(f => f.ParentId.EqualTo(id))
                        .Select(f => f.Id).ToList();
            var func = GetById(id);
            idArr.Remove(id);

            //ztree会把半选的父结点也传过来，所以要判断，如果所选父结点的子结点
            //全选，则是对父结点操作，否则是对子结点操作
            if (move && childIds.All(cid => idArr.Contains(cid)))
            {
                func.ParentId = parentId.IsDefault() ? default(TId) : parentId;
                Save(func);
                childIds.Each(cid => CascadeMove(cid, false));
            }
        }

        /// <summary>
        /// 提交修改并返回树结点信息
        /// </summary>
        /// <param name="cat">要修改的功能</param>
        /// <returns>修改后的功能信息Json</returns>
        public T Save(T cat)
        {
            if (cat.Id.IsDefault())
            {
                Add(cat);
            }
            else
            {
                Change(cat);
            }

            return cat;
        }

        /// <summary>
        /// 获取指定栏目到根栏目的层级数
        /// </summary>
        /// <param name="catId"></param>
        /// <param name="rootId"></param>
        /// <returns></returns>
        public int GetDeepth(TId catId, TId rootId)
        {
            T cat = GetById(catId);
            int deepth = 0;
            while (cat.ParentId != null && cat.Id.EqualTo(rootId))
            {
                cat = GetById(cat.ParentId);
                deepth++;
            }
            return deepth;
        }
    }

}
