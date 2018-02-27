using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Jurassic.Com.Tools;
using Jurassic.CommonModels.Organization;
using System.Reflection;
using Jurassic.CommonModels.EntityBase;
using System.Data.Entity;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 根据部门和用户从属关系授权的数据筛选查询类
    /// 筛选方法：
    /// 当一个实体类中有部门ID外键属性时，并且此属性标记为AuthByDeptId=true，
    /// 则该部门领导能看到所有该部门和下属部门的记录
    /// 普通员工只能看到部门ID属于自己的并且人员ID（标记为AuthByUserId=true)指向自己的记录。
    /// </summary>
    /// <typeparam name="T">要查询的业务实体类型</typeparam>
    public class DeptUserAuthQuery<T>
        where T : class, IId<int>
    {
        ICurrentDepartment _currDept; //当_currDept为空时，不能判断以部门ID为依据的权限和领导的权限
        ModelRule _modelRule;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="getDeptId">获取当前用户部门ID的委托</param>
        public DeptUserAuthQuery()
        {
            _currDept = SiteManager.Get<ICurrentDepartment>();
            _modelRule = ModelRule.Get<T>();
        }

        /// <summary>
        /// 将查询再次通过数据权限进行过滤
        /// </summary>
        /// <param name="query">传入的查询</param>
        /// <returns>过滤后的查询</returns>
        public IQueryable<T> GetQuery(IQueryable<T> query)
        {
            var userId = AppManager.Instance.GetCurrentUserId().ToInt();
            var deptId = _currDept.DeptId;
            var user = Organization.OrgManager.mOrganizationManager.GetDepUserQuery()
            .FirstOrDefault(u => u.UserId == userId && u.DepId == deptId);
            var exp = (user != null && user.IsLeader == 1) ? GetLeaderExp() : GetEmployeeExp();
            if (exp == null)
            {
                return query;
            }
            else
            {
                return query.Where(exp);
            }
        }

        Expression<Func<T, bool>> GetLeaderExp()
        {
            if (_currDept == null)
            {
                return null;
            }
            Expression exp = null;
            ParameterExpression pModel = Expression.Parameter(typeof(T), "my");
            var userIdRules = _modelRule.SingleRules.Where(r => r.AuthByUser);
            var deptIdRules = _modelRule.SingleRules.Where(r => r.AuthByDept);
            MethodInfo containsMethod = typeof(List<int>).GetMethod("Contains");
            var dept = Organization.OrgManager.mOrganizationManager.GetDepInfo(_currDept.DeptId);
            var orgNode = dept.OrgNode;

            if (!deptIdRules.IsEmpty())
            {
                var downDeptIds = Organization.OrgManager.mOrganizationManager.GetDepList()
                       .Where(dep => dep.OrgNode.StartsWith(orgNode))
                       .Select(u => u.DepId.Value).ToList();
                var constList = Expression.Constant(downDeptIds);

                foreach (var r in deptIdRules)
                {
                    Expression mModel = Expression.Property(pModel, r.Name);
                    if (r.Property.PropertyType.IsNullableType())
                    {
                        mModel = Expression.Convert(mModel, typeof(int));
                    }
                    var containsCall = Expression.Call(constList, containsMethod, mModel);
                    //var lambda = Expression.Lambda<Func<T, bool>>(containsCall, pModel);
                    exp = (exp == null) ? (Expression)containsCall : Expression.Or(exp, containsCall);
                }
            }
            //如果有部门ID属性，则不再判断人员
            else if (!userIdRules.IsEmpty())
            {
                var downUserIds = Organization.OrgManager.mOrganizationManager.GetDepUserQuery()
                    .Where(v => v.OrgNode.StartsWith(dept.OrgNode))
                    .Select(u => u.UserId.Value).Distinct().ToList();
                var constList = Expression.Constant(downUserIds);

                foreach (var r in userIdRules)
                {
                    Expression mModel = Expression.Property(pModel, r.Name);
                    if (r.Property.PropertyType.IsNullableType())
                    {
                        mModel = Expression.Convert(mModel, typeof(int));
                    }
                    var containsCall = Expression.Call(constList, containsMethod, mModel);
                    exp = (exp == null) ? (Expression)containsCall : Expression.Or(exp, containsCall);
                }
            }

            if (exp == null)
            {
                return null;
            }
            var lambda = Expression.Lambda<Func<T, bool>>(exp, pModel);
            return lambda;
        }

        Expression<Func<T, bool>> GetEmployeeExp()
        {
            Expression exp = null;
            ParameterExpression pModel = Expression.Parameter(typeof(T), "my");
            var userIdRules = _modelRule.SingleRules.Where(r => r.AuthByUser);
            var deptIdRules = _modelRule.SingleRules.Where(r => r.AuthByDept);
            if (!userIdRules.IsEmpty())
            {
                foreach (var r in userIdRules)
                {
                    var constId = Expression.Constant(AppManager.Instance.GetCurrentUserId().ToInt());
                   Expression mModel = Expression.Property(pModel, r.Name);
                   
                    if (r.Property.PropertyType.IsNullableType())
                    {
                        mModel = Expression.Convert(mModel, typeof(int));
                    }
                    // model = >model.UserId == currentUserId;
                    var equalExp = Expression.Equal(mModel, constId);
                    exp = (exp == null) ? (Expression)equalExp : Expression.Or(exp, equalExp);
                }
            }
            //先注释掉，也就是当人员ID指向自己时，不再判断部门是不是自己的部门
            //if (!deptIdRules.IsEmpty() && _currDept != null)
            //{
            //    foreach (var r in deptIdRules)
            //    {
            //        var constId = Expression.Constant(_currDept.DeptId);
            //        MemberExpression mModel = Expression.Property(pModel, r.Name);
            //        var equalExp = Expression.Equal(mModel, constId);
            //        exp = (exp == null) ? (Expression)equalExp : Expression.Or(exp, equalExp);
            //    }
            //}
            if (exp == null)
            {
                return null;
            }
            var lambda = Expression.Lambda<Func<T, bool>>(exp, pModel);
            return lambda;
        }
    }
}
