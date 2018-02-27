using Jurassic.CommonModels.ModelBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AddinDemo.Models
{
    public class HonorConverter : IModelEntityConverter<HonorModel, Honor>
    {
        public System.Linq.Expressions.Expression<Func<HonorModel, Honor>> ModelToEntity
        {
            get
            {
                return p => new Honor
                {
                    GetDate = p.GetDate,
                    HonorName = p.HonorName,
                    Id = p.Id,
                };
            }
        }

        public System.Linq.Expressions.Expression<Func<Honor, HonorModel>> EntityToModel
        {
            get
            {
                return p => new HonorModel
                {
                    GetDate = p.GetDate,
                    HonorName = p.HonorName,
                    Id = p.Id,
                };
            }
        }
    }
}