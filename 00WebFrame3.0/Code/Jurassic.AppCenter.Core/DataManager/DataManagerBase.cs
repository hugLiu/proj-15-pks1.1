using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.Tools;

namespace Jurassic.AppCenter
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 统一的数据管理类，提供对类型T数据列表的增删改。
    /// <typeparam name="T">要管理的数据类型</typeparam>
    /// <typeparam name="TId">数据的主键类型</typeparam>
    /// </summary>
    public class DataManagerBase<T, TId> where T : class, IIdNameBase<TId>
    {
        private IList<T> mDataList;

        private Type mDataType;
        /// <summary>
        /// 获取该DataManager的实际数据类型，它应该是T或T的子类
        /// </summary>
        public Type DataType
        {
            get
            {
                if (mDataType == null)
                {
                    mDataType = typeof(T);
                }
                return mDataType;
            }
        }

        /// <summary>
        ///  在从数据持久层获取数据以后的操作
        /// </summary>
        public Func<IEnumerable<T>, IEnumerable<T>> AfterGetData { get; set; }

        /// <summary>
        /// 在保存数据到数据持久层之前的操作
        /// </summary>
        public Action<T> BeforeSave { get; set; }

        /// <summary>
        /// 在成功保存数据之后的操作
        /// </summary>
        public Action<T> AfterSaved { get; set; }

        /// <summary>
        /// 该管理器所管理的数据集合。
        /// </summary>
        IList<T> DataList
        {
            get
            {
                if (mDataList == null)
                {
                    var data = Provider.GetData();
                    if (AfterGetData != null)
                    {
                        data = AfterGetData(data);
                        mDataType = AfterGetData.Method.ReturnType.GetElementType()
                            ?? AfterGetData.Method.ReturnType
                            .GetGenericArguments()
                            .FirstOrDefault(t => typeof(T).IsAssignableFrom(t));
                    }
                    else
                    {
                        mDataType = RefHelper.GetElementType(data);
                    }
                    if (data is IList<T>)
                    {
                        mDataList = (IList<T>)data;
                    }
                    else
                    {
                        mDataList = data.ToList();
                    }
                }
                return mDataList;
            }
        }

        /// <summary>
        /// 元素的总数
        /// </summary>
        public int Count
        {
            get { return DataList.Count; }
        }

        /// <summary>
        /// 获取所有元素
        /// </summary>
        /// <returns></returns>
        public virtual IList<T> GetAll()
        {
            return DataList;
        }

        private IDataProvider<T> mProvider;
        /// <summary>
        /// 来自外部的数据存取提供接口
        /// </summary>
        public virtual IDataProvider<T> Provider
        {
            get
            {
                if (mProvider == null)
                {
                    mProvider = new DataProvider<T>();
                }
                return mProvider;
            }
            set
            {
                mProvider = value;
            }
        }

        /// <summary>
        /// 创建一个T类型的实例
        /// </summary>
        /// <returns>创建的对象</returns>
        public virtual T Create()
        {
            //由于外部的对象可能是T的子类，所以这里不能用new T()
            return (T)Activator.CreateInstance(DataType);
        }

        /// <summary>
        /// 创建一个DataManager
        /// </summary>
        public DataManagerBase()
        {
        }

        /// <summary>
        /// 根据ID获取对像，找不到返回null
        /// </summary>
        /// <param name="id"></param>
        /// <returns>对象T</returns>
        public T GetById(TId id)
        {
            if (id == null) return null;
            return DataList.FirstOrDefault(d => id.Equals(d.Id));
        }

        /// <summary>
        /// 根据名称获取对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns>对象T</returns>
        public T GetByName(string name)
        {
            name = name.ToStr();
            if (name.IsEmpty()) return null;
            return DataList.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="t"></param>
        /// <returns>修改成功与否</returns>
        public bool Change(T t)
        {
            var old = DataList.FirstOrDefault(d => d.Id.Equals(t.Id));
            if (old == null)
            {
                throw new JException(t.Name + "Not found。");
            }
            old.Name = t.Name;
            if (!CheckValidName(old))
            {
                throw new JException(t.Name + " duplicated。");
            }

            if (BeforeSave != null)
            {
                BeforeSave(t);
            }
            if (Provider.Change(t) > 0)
            {
                DataList[DataList.IndexOf(old)] = t;
                if (AfterSaved != null)
                {
                    AfterSaved(t);
                }
                return true;
            }
            return false;
        }

        ///// <summary>
        ///// 修改缓存变量 by_zjf
        ///// </summary>
        ///// <param name="t">数据对象类型</param>
        ///// <returns>返回修改后的缓存变量</returns>
        //public IList<T> ChangeCached(T t)
        //{
        //    var old = DataList.FirstOrDefault(d => d.Id.Equals(t.Id));
        //    if (old == null)
        //    {
        //        return null;
        //    }

        //    DataList[DataList.IndexOf(old)] = t;
        //    return DataList;
        //}

        /// <summary>
        /// 判断实现了IUniqueName接口对象的名称是否重复
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        bool CheckValidName(T t)
        {
            var exists = GetById(t.Id);
            if (exists != null && exists != t)
            {
                throw new JException("Duplicate ID:" + exists.Id);
            }
            if (!(t is IUniqueName)) return true;
            var other = GetByName(t.Name);
            return (other == null || other == t);
        }

        /// <summary>
        /// 新增对象
        /// </summary>
        /// <param name="t"></param>
        public void Add(T t)
        {
            if (!CheckValidName(t))
            {
                throw new JException(t.Name + "的名称重复。");
            }

            if (BeforeSave != null)
            {
                BeforeSave(t);
            }
            if (Provider.Add(t) > 0)
            {
                DataList.Add(t);
                if (AfterSaved != null)
                {
                    AfterSaved(t);
                }
            }
            if (t.Id == null)
            {
                throw new JException("ID is empty");
                //t.Id = CreateId();
            }
        }

        /// <summary>
        /// 删除对象t
        /// </summary>
        /// <param name="t"></param>
        /// <returns>删除成功与否</returns>
        public bool Remove(T t)
        {
            return Remove(t.Id);
        }

        /// <summary>
        /// 根据对象ID删除对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns>删除成功与否</returns>
        public bool Remove(TId id)
        {
            var old = DataList.FirstOrDefault(d => d.Id.Equals(id));
            if (old != null && Provider.Delete(old) > 0)
            {
                var r = DataList.Remove(old);
                if (AfterSaved != null)
                {
                    AfterSaved(old);
                }
                return r;
            }
            return false;
        }

        /// <summary>
        /// 清空数据缓存，强制重新从执久层获取
        /// </summary>
        public virtual void Clear()
        {
            DataList.Clear();
            //wangmin add 2015-09-01
            mDataList = null;
        }

        /// <summary>
        /// 如果内部存储数据的List支持ICanSave接口，则保存全部数据
        /// 这是为CachedListT而准备的
        /// </summary>
        public virtual void Save()
        {
            if (DataList is ICanSave)
            {
                ((ICanSave)DataList).Save();
            }
        }
    }
}
