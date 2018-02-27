using System;
using System.Collections.Generic;
using System.Data.Entity;
using CacheManager.Core;
using PKS.Utils;

namespace PKS.Data
{
    /// <summary>实体变化项</summary>
    public class EntityChangedItem<T>
    {
        /// <summary>变化状态</summary>
        public EntityState State { get; set; }
        /// <summary>变化实体</summary>
        public T Entity { get; set; }

    }

    /// <summary>实体变化事件数据接口</summary>
    public interface IEntityChangedEventArgs
    {
        /// <summary>加入变化实体</summary>
        void Add(EntityState state, object entity);
    }

    /// <summary>实体变化事件数据</summary>
    public class EntityChangedEventArgs<T> : EventArgs, IEntityChangedEventArgs
        where T : class 
    {
        /// <summary>变化实体</summary>
        public List<EntityChangedItem<T>> Items { get; set; } = new List<EntityChangedItem<T>>();

        /// <summary>加入变化实体</summary>
        public void Add(EntityState state, object entity)
        {
            this.Items.Add(new EntityChangedItem<T>() { State = state, Entity = entity.As<T>() });
        }
    }
}
