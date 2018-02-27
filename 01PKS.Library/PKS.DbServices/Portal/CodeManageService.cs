using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.DbServices.Portal.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices
{
    /// <summary>
    /// 码表维护服务
    /// </summary>
    public class CodeManageService : AppService, IPerRequestAppService
    {
        private readonly IRepository<PKS_Code> _codeRepository;
        private readonly IRepository<PKS_CodeType> _codeTypeRepository;

        public CodeManageService(IRepository<PKS_Code> codeRepository, 
                                 IRepository<PKS_CodeType> codeTypeRepository)
        {
            _codeRepository = codeRepository;
            _codeTypeRepository = codeTypeRepository;
        }

        public List<CodeTypeModel> GetAllCodeType()
        {
            var codeTypes = _codeTypeRepository.GetQuery();
            if (codeTypes == null)
                return new List<CodeTypeModel>();
            return codeTypes.Select(t => new CodeTypeModel
            {
                Id = t.Id,
                Code = t.Code,
                Title = t.Title,
            }).ToList();
        }

        public List<CodeModel> GetCodesByType(string type)
        {
            var result = new List<CodeModel>();
            var codeType = _codeTypeRepository.GetQuery()
                .FirstOrDefault(t => t.Title == type);
            if (codeType != null)
            {
                var query = _codeRepository.GetQuery()
                    .Where(t => t.CodeTypeId == codeType.Id)
                    .Select(t => new CodeModel
                    {
                        Id = t.Id,
                        CodeTypeId = t.CodeTypeId,
                        Code = t.Code,
                        Title = t.Title
                    });
                if (query != null) result.AddRange(query);
            }
            return result;
        }

        public List<CodeModel> GetCodes(int? codeTypeId)
        {
            var currentCodes = new List<CodeModel>();
            var allCodes = _codeRepository.GetQuery();
            if (allCodes == null)
                return currentCodes;
            if(codeTypeId == null)
            {
                currentCodes = allCodes.Select(t => new CodeModel
                {
                    Id = t.Id,
                    CodeTypeId = t.CodeTypeId,
                    Code = t.Code,
                    Title = t.Title
                }).ToList();
            }
            else
            {
                var nowCode = allCodes.Where(t => t.CodeTypeId == codeTypeId);
                currentCodes = nowCode?.Select(t => new CodeModel
                {
                    Id = t.Id,
                    CodeTypeId = t.CodeTypeId,
                    Code = t.Code,
                    Title = t.Title
                }).ToList();
            }
            return currentCodes;
        }

        public void UpdateCode(List<CodeModel> models, string userName)
        {
            foreach(var model in models)
            {
                if (model._state == EnumNodeState.added.ToString())
                {
                    var entity = new PKS_Code()
                    {
                        Code = model.Code,
                        Title = model.Title,
                        CodeTypeId = model.CodeTypeId,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = userName,
                        UpdatedDate = DateTime.Now
                    };
                    _codeRepository.Add(entity);
                }
                if (model._state == EnumNodeState.modified.ToString())
                {
                    var entity = _codeRepository.GetQuery().FirstOrDefault(t => t.Id == model.Id);
                    if (entity == null) return;
                    entity.Code = model.Code;
                    entity.Title = model.Title;
                    entity.CodeTypeId = model.CodeTypeId;
                    entity.UpdatedBy = userName;
                    entity.UpdatedDate = DateTime.Now;
                    _codeRepository.Update(entity);

                }
                if (model._state == EnumNodeState.removed.ToString())
                {
                    var entity = _codeRepository.GetQuery().FirstOrDefault(t => t.Id == model.Id);
                    if (entity == null) return;
                    _codeRepository.Delete(entity);
                }
            }
        }

        public void UpdateCodeType(List<CodeTypeModel> models, string userName)
        {
            foreach (var model in models)
            {
                if (model._state == EnumNodeState.added.ToString())
                {
                    var entity = new PKS_CodeType()
                    {
                        Code = model.Code,
                        Title = model.Title,
                        CreatedBy = userName,
                        CreatedDate = DateTime.Now,
                        UpdatedBy = userName,
                        UpdatedDate = DateTime.Now
                    };
                    _codeTypeRepository.Add(entity);
                }
                if (model._state == EnumNodeState.modified.ToString())
                {
                    var entity = _codeTypeRepository.GetQuery().FirstOrDefault(t => t.Id == model.Id);
                    if (entity == null) return;
                    entity.Code = model.Code;
                    entity.Title = model.Title;
                    entity.UpdatedBy = userName;
                    entity.UpdatedDate = DateTime.Now;
                    _codeTypeRepository.Update(entity);

                }
                if (model._state == EnumNodeState.removed.ToString())
                {
                    var entity = _codeTypeRepository.GetQuery().FirstOrDefault(t => t.Id == model.Id);
                    if (entity == null) return;
                    _codeRepository.DeleteList(t => t.CodeTypeId == entity.Id);
                    _codeTypeRepository.Delete(entity);
                }
            }
        }
    }
}
