using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PKS.Data
{
    /// <summary>仓储数据访问接口</summary>
    public interface IRepository<TEntity>
    {
        /// <summary>增加</summary>
        void Add(TEntity entity, bool submit = true);

        /// <summary>
        /// 批量增加
        /// </summary>
        /// <param name="entity">集合</param>
        /// <param name="submit"></param>
        void AddRange(IEnumerable<TEntity> entity, bool submit = true);

        /// <summary>增加</summary>
        Task AddAsync(TEntity entity);
        /// <summary>更新</summary>
        void Update(TEntity entity, bool submit = true);
        /// <summary>是否变化</summary>
        bool IsModified(TEntity entity);
        /// <summary>
        /// 删除
        /// </summary>
        void Delete(TEntity entity, bool submit = true);
        /// <summary>
        /// 删除
        /// </summary>
        Task DeleteAsync(TEntity entity);
        /// <summary>
        /// 删除
        /// </summary>
        void DeleteByKey(TEntity entity, bool submit = true);

        /// <summary>
        /// 删除
        /// </summary>
        void DeleteList(Expression<Func<TEntity, bool>> whereExpr);

        /// <summary>
        /// 删除
        /// </summary>
        void DeleteList(IEnumerable<TEntity> entities);

         /// <summary>
         /// 判断是否存在
         /// </summary>
         /// <param name="whereExpr"></param>
         /// <returns></returns>
        bool Exist(Expression<Func<TEntity, bool>> whereExpr);
        /// <summary>
        /// 获取记录数
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        int Count(Expression<Func<TEntity, bool>> whereExpr);
        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        List<TEntity> GetAll();
        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetQuery();
        /// <summary>
        /// 查找实体对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity Find(object key);
        /// <summary>
        /// 查找实体对象
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <returns></returns>
        TEntity Find(Expression<Func<TEntity, bool>> whereExpr);

        /// <summary>
        /// 查找实体对象列表
        /// </summary>
        /// <param name="whereExpr"></param>
        /// <param name="orderbyExpr"></param>
        /// <param name="orderDirection"></param>
        /// <returns></returns>
        IEnumerable<TEntity> FindList<TKey>(Expression<Func<TEntity, bool>> whereExpr, Expression<Func<TEntity, TKey>> orderbyExpr, int orderDirection);

        /// <summary>
        /// 查找实体对象列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereExpr"></param>
        /// <param name="selectExpr"></param>
        /// <param name="orderbyExpr"></param>
        /// <param name="orderDirection"></param>
        /// <param name="returnCount"></param>
        /// <returns></returns>
        IEnumerable<TResult> FindList<TResult, TKey>(Expression<Func<TEntity, bool>> whereExpr, Expression<Func<TEntity, TResult>> selectExpr, Expression<Func<TResult, TKey>> orderbyExpr, int orderDirection, int returnCount = -1);

        /// <summary>
        /// 分页查找实体对象列表
        /// </summary>
        IEnumerable<TEntity> FindListByPage<TKey>(Expression<Func<TEntity, bool>> whereExpr, Expression<Func<TEntity, TKey>> orderbyExpr, int orderDirection, int pageSize, int pageNo, out int recordCount);
        /// <summary>
        /// 分页查找实体对象列表
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereExpr"></param>
        /// <param name="selectExpr"></param>
        /// <param name="orderbyExpr"></param>
        /// <param name="orderDirection"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNo"></param>
        /// <param name="recordCount"></param>
        /// <returns></returns>
        IEnumerable<TResult> FindListByPage<TResult, TKey>(Expression<Func<TEntity, bool>> whereExpr, Expression<Func<TEntity, TResult>> selectExpr, Expression<Func<TResult, TKey>> orderbyExpr, int orderDirection, int pageSize, int pageNo, out int recordCount);

        /// <summary>
        /// 获得字段名关联的存储字段名
        /// </summary>
        string GetStoreFieldName(string fieldName);
        /// <summary>
        /// 获得自己及子实体层次集合
        /// </summary>
        List<TEntity> GetChildren(object[] ids, string parentFieldName, string where = null);
        /// <summary>
        /// 获得自己及父实体层次集合
        /// </summary>
        List<TEntity> GetParents(object[] ids, string parentFieldName, string where = null);
        /// <summary>
        /// 提交保存所有变更操作
        /// </summary>
        void Submit();
        /// <summary>
        /// 提交保存所有变更操作
        /// </summary>
        Task SubmitAsync();
        /// <summary>
        /// </summary>
        void BeginTrans();
        /// <summary>
        /// </summary>
        void EndTrans();
        /// <summary>
        /// </summary>
        void RollbackTrans();
    }
}
