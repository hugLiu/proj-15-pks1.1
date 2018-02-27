using Jurassic.CommonModels.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddinDemo.Models
{
    public class EduHistoryConverter : IModelEntityConverter<EduHistoryModel, EduHistory>
    {
        public System.Linq.Expressions.Expression<Func<EduHistoryModel, EduHistory>> ModelToEntity
        {
            get
            {
                return p => new EduHistory
                {
                    EndDate = p.EndDate,
                    Id = p.Id,
                    Remark = p.Remark,
                    SchoolName = p.SchoolName,
                    StartDate = p.StartDate,
                    Special = p.Special,
                    Subject = p.Subject
                };
            }
        }

        public System.Linq.Expressions.Expression<Func<EduHistory, EduHistoryModel>> EntityToModel
        {
            get
            {
                return p => new EduHistoryModel
                {
                    EndDate = p.EndDate,
                    Id = p.Id,
                    Remark = p.Remark,
                    SchoolName = p.SchoolName,
                    StartDate = p.StartDate == null ? default(DateTime) : p.StartDate.Value,
                    Special = p.Special,
                    Subject = p.Subject
                };
            }
        }
    }
}