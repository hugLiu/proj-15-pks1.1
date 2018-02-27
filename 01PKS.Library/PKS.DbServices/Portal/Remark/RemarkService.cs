using System;
using System.Collections.Generic;
using System.Linq;
using PKS.Core;
using PKS.Data;
using PKS.DbModels.Portal;
using PKS.DbServices.Portal.Remark.Model;

namespace PKS.DbServices.Portal.Remark
{
    /// <summary>
    /// 评论服务
    /// </summary>
    public class RemarkService:AppService,IPerRequestAppService
    {
        public IRepository<PKS_Remark> RemarkContent;
        public RemarkService(IRepository<PKS_Remark> remarkContent)
        {
            RemarkContent = remarkContent;
        }

        /// <summary>
        /// 查询评论列表
        /// </summary>
        /// <param name="iiid"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Pagination<RemarkModel> QueryRemark(string iiid, int index, int size, string filter, int userId)
        {
            var remarkList = from e in RemarkContent.GetQuery()
                where e.IIId == iiid
                select e;
            var count = remarkList.Count();

            var pageData = new Pagination<RemarkModel>();
            pageData.total = count;
            IEnumerable<PKS_Remark> list;
            if (filter == "Newest")
            {
                list = from a in remarkList.ToList()
                    where (DateTime.Now.Subtract(a.CreatedDate)).Days < 1
                    select a;

            }
            else if (filter == "My")
            {
                list = from b in remarkList
                    where b.UserId == userId
                    select b;
            }
            else
            {
                list = remarkList;
            }
            var modelList = list.Select(item => new RemarkModel
            {
                Id = item.Id,
                IIId = item.IIId,
                Remark = item.Remark,
                UserId = item.UserId,
                CreatedBy = item.CreatedBy,
                CreatedDate = item.CreatedDate,
                LastUpdatedBy = item.LastUpdatedBy,
                LastUpdatedDate = item.LastUpdatedDate,
                UserPhotoUrl = string.Empty
            });
            // TODO: 修改点赞数据获取方法
            var result = modelList.OrderByDescending(o => o.CreatedDate).Skip(index).Take(size).ToList();
            var remarkThumbupRepository = GetService<IRepository<PKS_Remark_Thumbup>>();
            for (int i = 0; i < result.Count; i++)
            {
                var remarkid = result[i].Id;
                var thumbupsQuery = remarkThumbupRepository.GetQuery()
                        .Where(item => item.RemarkId == remarkid)
                        .Select(item => new RemarkThumbupModel()
                        {
                            Id = item.Id,
                            RemarkId = item.RemarkId,
                            UserId = item.UserId,
                            CreatedBy = item.CreatedBy,
                            LastUpdatedBy = item.LastUpdatedBy

                        });
                result[i].Thumbups = thumbupsQuery.ToList();
            }
            pageData.data = result;
            return pageData;
        }

        /// <summary>
        /// 发表评论
        /// </summary>
        /// <param name="model"></param>
        public void AddRemark(RemarkModel model)
        {
            if (model != null)
            {
                PKS_Remark addModel = new PKS_Remark();
                addModel.Remark = model.Remark;
               // addModel.CreatedBy = UserProfileContent.Find(o => o.UserId == model.UserId).UserName;
              
                addModel.IIId = model.IIId;
                addModel.Remark = model.Remark;
                addModel.UserId = model.UserId;
                addModel.CreatedBy = model.CreatedBy;
                addModel.CreatedDate = model.CreatedDate;
                addModel.LastUpdatedBy = model.LastUpdatedBy;
                addModel.LastUpdatedDate = model.LastUpdatedDate;
                RemarkContent.Add(addModel);
            }
        }
        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="model"></param>
        public void DeleteRemark(int id)
        {
            RemarkContent.DeleteList(item=>item.Id== id);
        }
        /// <summary>
        /// 赞
        /// </summary>
   
        /// <param name="thumbup"></param>
        /// <param name="praise"></param>
        public void PraiseRemark(PKS_Remark_Thumbup thumbup, bool praise)
        {
            var repository = GetService<IRepository<PKS_Remark_Thumbup>>();
            var exists = repository.Exist(item => item.RemarkId == thumbup.RemarkId && item.UserId == thumbup.UserId);
            if (praise)
            {
               if(!exists)
                    repository.Add(thumbup);
            }
            else
                if(exists)
                repository.DeleteList(item=>item.RemarkId== thumbup.RemarkId && item.UserId== thumbup.UserId);
        }
    }
}
