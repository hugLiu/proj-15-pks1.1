using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.Standard.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices
{
    public class StandardService : AppService, IPerRequestAppService
    {
        private readonly IRepository<PKS_STANDARD_EXTERNAL> _standardRepository;

        public StandardService(IRepository<PKS_STANDARD_EXTERNAL> standardRepository)
        {
            _standardRepository = standardRepository;
        }

        public List<StandardModel> GetStandards()
        {
            var result = new List<StandardModel>();
            var query = _standardRepository.GetQuery()
                .OrderByDescending(t => t.CREATEDDATE)
                .Select(t => new StandardModel
                {
                    Id = t.Id,
                    Name = t.NAME,
                    Type = t.TYPE,
                    Url = t.URL,
                    CreatedBy = t.CREATEDBY,
                    CreatedDate = t.CREATEDDATE
                });
            if (query != null) result.AddRange(query);
            return result;
        }

        public List<StandardModel> AddStandards(List<StandardModel> models, string name)
        {
            var result = new List<StandardModel>();
            foreach(var model in models)
            {
                var entity = _standardRepository.GetQuery()
                    .FirstOrDefault(t => t.NAME == model.Name);
                if (entity != null)
                {
                    entity.URL = model.Url;
                    entity.TYPE = model.Type;
                    entity.LASTUPDATEDBY = name;
                    entity.LASTUPDATEDDATE = DateTime.Now;
                    _standardRepository.Update(entity);
                }
                else
                {
                    entity = new PKS_STANDARD_EXTERNAL
                    {
                        NAME = model.Name,
                        URL = model.Url,
                        TYPE = model.Type,
                        CREATEDBY = name,
                        CREATEDDATE = DateTime.Now,
                        LASTUPDATEDBY = name,
                        LASTUPDATEDDATE = DateTime.Now
                    };
                    _standardRepository.Add(entity);
                }
                
                result.Add(new StandardModel
                {
                    Id = entity.Id,
                    Name = entity.NAME,
                    Type = entity.TYPE,
                    Url = entity.URL,
                    CreatedBy = entity.CREATEDBY,
                    CreatedDate = entity.CREATEDDATE
                });
            }
            return result;
        }

        public StandardModel UpdateStandard(StandardModel data, string name)
        {
            var entity = _standardRepository.GetQuery()
                .FirstOrDefault(t => t.Id == data.Id);
            if (entity == null) return null;
            entity.NAME = data.Name;
            entity.TYPE = data.Type;
            entity.URL = data.Url;
            entity.LASTUPDATEDBY = name;
            entity.LASTUPDATEDDATE = DateTime.Now;
            _standardRepository.Update(entity);
            return new StandardModel
            {
                Id = entity.Id,
                Name = entity.NAME,
                Type = entity.TYPE,
                Url = entity.URL,
                CreatedBy = entity.CREATEDBY,
                CreatedDate = entity.CREATEDDATE
            };
        }

        public void RemoveStandards(List<int> ids)
        {
            foreach(var id in ids)
            {
                _standardRepository.DeleteList(t => t.Id == id);
            }
        }
    }
}
