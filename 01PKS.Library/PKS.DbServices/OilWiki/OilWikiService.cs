using PKS.Core;
using PKS.Data;
using PKS.DbModels.OilWiki;
using PKS.DbServices.OilWiki.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.OilWiki
{
    public class OilWikiService : AppService, IPerRequestAppService
    {
        private readonly IRepository<PKS_OILWIKI_DOMAIN> _domainRepository;
        private readonly IRepository<PKS_OILWIKI_CATALOG> _catalogTypeRepository;
        private readonly IRepository<PKS_OILWIKI_ENTRY> _entryRepository;
        private readonly IRepository<PKS_OILWIKI_ALIASENTRY> _aliasEntryRepository;
        private readonly IRepository<PKS_OILWIKI_RELATEDENTRY> _relatedRepository;

        public OilWikiService(IRepository<PKS_OILWIKI_DOMAIN> domainRepository,
                              IRepository<PKS_OILWIKI_CATALOG> catalogTypeRepository,
                              IRepository<PKS_OILWIKI_ENTRY> entryRepository,
                              IRepository<PKS_OILWIKI_ALIASENTRY> aliasEntryRepository,
                              IRepository<PKS_OILWIKI_RELATEDENTRY> relatedRepository)
        {
            _domainRepository = domainRepository;
            _catalogTypeRepository = catalogTypeRepository;
            _entryRepository = entryRepository;
            _aliasEntryRepository = aliasEntryRepository;
            _relatedRepository = relatedRepository;
        }
        /// <summary>
        /// 获取所有目录
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PKS_OILWIKI_CATALOG> GetCatalog()
        {
            return _catalogTypeRepository.GetQuery().OrderBy(m => m.ORDERNUMBER).ToList();
        }

        public IEnumerable<PKS_OILWIKI_CATALOG> GetCatalogAndEntry(int entryNum = 3)
        {
            return _catalogTypeRepository.GetQuery().OrderBy(m => m.ORDERNUMBER).ToList();
            //return _catalogTypeRepository.GetQuery().OrderBy(m => m.ORDERNUMBER).Select(n => new PKS_OILWIKI_CATALOG
            //{
            //    Id = n.Id,
            //    CODE = n.CODE,
            //    NAME = n.NAME,
            //    DESCRIPTION = n.DESCRIPTION,
            //    IMAGEURL = n.IMAGEURL,
            //    LEVELNUMBER = n.LEVELNUMBER,
            //    ORDERNUMBER = n.ORDERNUMBER,
            //    KMD = n.KMD,
            //    PARENTID = n.PARENTID,
            //    DOMAINID = n.DOMAINID//,
            //    //PKS_OILWIKI_ENTRY = (ICollection<PKS_OILWIKI_ENTRY>)n.PKS_OILWIKI_ENTRY.Take(entryNum)
            //});
        }

        /// <summary>
        /// 根据目录Id获取目录
        /// </summary>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public PKS_OILWIKI_CATALOG GetCatalogById(int catalogId)
        {
            return _catalogTypeRepository.Find(c => c.Id == catalogId);
        }

        /// <summary>
        /// 根据查询字符串获取搜索的词条
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        public IEnumerable<PKS_OILWIKI_ENTRY> SearchEntry(string queryString = "")
        {
            Func<PKS_OILWIKI_ENTRY, bool> exp = e => true;
            if (queryString.Length > 0)
            {
                exp = e => e.NAME.Contains(queryString);
            }
            return _entryRepository.GetQuery().Where(exp);
        }

        /// <summary>
        /// 根据词条名称获取词条记录
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public PKS_OILWIKI_ENTRY GetEntryByName(string name)
        {
            return _entryRepository.Find(e => e.NAME == name);
        }
        /// <summary>
        /// 根据目录Id获取目录下词条列表
        /// </summary>
        /// <param name="catalogId"></param>
        /// <returns></returns>
        public IEnumerable<PKS_OILWIKI_ENTRY> GetEntryByCatalogId(int catalogId)
        {
            return _entryRepository.GetQuery().Where(e => e.CATALOGID == catalogId);
        }

        /// <summary>
        /// 根据词条Id获取词条数据详情
        /// </summary>
        /// <param name="entryId"></param>
        /// <returns></returns>
        public EntryDetails GetEntryById(int entryId)
        {
            return (from e in _entryRepository.GetQuery()
                    join c in _catalogTypeRepository.GetQuery()
                    on e.CATALOGID equals c.Id
                    where e.Id == entryId
                    select new EntryDetails
                    {
                        Id = e.Id,
                        Name = e.NAME,
                        Contents = e.CONTENTS,
                        Author = e.AUTHOR,
                        Source = e.SOURCE,
                        Image = e.IMAGE,
                        ParentCatalogId =e.CATALOGID, //e.PKS_OILWIKI_CATALOG.Id,
                        ParentCatalogName =c.NAME,// e.PKS_OILWIKI_CATALOG.NAME,
                        //RootCatalogName = c.NAME,
                        AliasEntry = e.PKS_OILWIKI_ALIASENTRY.Select(r => new RelatedEntry { EntryName = r.PKS_OILWIKI_ENTRY.NAME, EntryId = r.Id }).ToList(),
                        RelatedEntry = e.PKS_OILWIKI_RELATEDENTRY.Select(r => new RelatedEntry { EntryName = r.PKS_OILWIKI_ENTRY1.NAME, EntryId = r.Id }).ToList()

                    }).FirstOrDefault();

        }

        public IEnumerable<EntryDTO> GetEntry(int catalogId)
        {
            return _entryRepository.GetQuery().Where(e => e.CATALOGID == catalogId).Select(
                  e => new EntryDTO
                  {
                      ParentCatalogId = e.PKS_OILWIKI_CATALOG.PARENTID == null ? 0 : e.PKS_OILWIKI_CATALOG.PARENTID,
                      CatalogId = catalogId,
                      Id = e.Id,
                      Name = e.NAME,
                      Contents = e.CONTENTS,
                      Source = e.SOURCE,
                      Image = e.IMAGE
                      //AliasEntry = e.PKS_OILWIKI_ALIASENTRY.Select(a => a.NAME).ToList(),
                      //RelatedEntry = e.PKS_OILWIKI_RELATEDENTRY.Select(r => new RelatedEntry { EntryName = r.PKS_OILWIKI_ENTRY.NAME, EntryId = r.Id }).ToList()
                  });
        }

        public bool EntryNameExists(int id, string name)
        {
            var entitys = 0;
            if (id == 0)
            {
                entitys = _entryRepository.GetQuery().Where(e => e.NAME == name).Count();
            }
            else
            {
                entitys = _entryRepository.GetQuery().Where(e => e.NAME == name && e.Id != id).Count();
            }

            return entitys > 0;
        }


        public IEnumerable<EntryDTO> FindEntryListByPage(Expression<Func<PKS_OILWIKI_ENTRY, bool>> whereExpr, int pageSize, int pageNo, out int recordCount)
        {
            return _entryRepository.FindListByPage(whereExpr,
                e => new EntryDTO
                {
                    ParentCatalogId = e.PKS_OILWIKI_CATALOG.PARENTID == null ? 0 : e.PKS_OILWIKI_CATALOG.PARENTID,
                    CatalogId = e.CATALOGID,
                    Id = e.Id,
                    Name = e.NAME,
                    EnglishName = e.ENGLISHNAME,
                    Contents = e.CONTENTS,
                    Author = e.AUTHOR,
                    Source = e.SOURCE,
                    Image = e.IMAGE,
                    CreatedBy = e.CREATEDBY,
                    CreatedDate = e.CREATEDDATE,
                    LastUpdatedBy = e.LASTUPDATEDBY,
                    LastUpdatedDate = e.LASTUPDATEDDATE
                }, e => e.Id, 0, pageSize, pageNo, out recordCount);
        }

        public IEnumerable<EntryDetails> FindAllEntryList()
        {
            var query = (from e in _entryRepository.GetQuery()
                join c in _catalogTypeRepository.GetQuery()
                on e.CATALOGID equals c.Id
                select new EntryDetails
                {
                    Id = e.Id,
                    Name = e.NAME,
                    Contents = e.CONTENTS,
                    Source = e.SOURCE,
                    Image = e.IMAGE,
                    Author=e.AUTHOR,
                    ParentCatalogId = e.CATALOGID, 
                    ParentCatalogName = c.NAME
                });
            return query.ToList();
        }

        public void DeleteEntry(int entryId)
        {
            _aliasEntryRepository.DeleteList(e => e.ENTRYID == entryId);
            _relatedRepository.DeleteList(e => e.ENTRYID == entryId);
            _entryRepository.DeleteList(e => e.Id == entryId);
        }

        public void UpdateEntry(EntryDTO entry)
        {
            var entryModel = _entryRepository.GetQuery().FirstOrDefault(e => e.Id == entry.Id);

            if (entryModel != null)
            {
                entryModel.Id = entry.Id;
                entryModel.NAME = entry.Name;
                entryModel.ENGLISHNAME = entry.EnglishName;
                entryModel.CATALOGID = entry.CatalogId;
                entryModel.AUTHOR = entry.Author;
                entryModel.SOURCE = entry.Source;
                entryModel.IMAGE = entry.Image;
                entryModel.CONTENTS = entry.Contents;
                entryModel.CREATEDBY = entry.CreatedBy;
                entryModel.CREATEDDATE = entry.CreatedDate;
                entryModel.LASTUPDATEDBY = entry.LastUpdatedBy;
                entryModel.LASTUPDATEDDATE = entry.LastUpdatedDate;
                _entryRepository.Update(entryModel);
            }
            else
            {
                entryModel = new PKS_OILWIKI_ENTRY
                {
                    NAME = entry.Name,
                    ENGLISHNAME = entry.EnglishName,
                    CATALOGID = entry.CatalogId,
                    AUTHOR = entry.Author,
                    SOURCE = entry.Source,
                    CONTENTS = entry.Contents,
                    IMAGE = entry.Image,
                    CREATEDBY = entry.CreatedBy,
                    CREATEDDATE = entry.CreatedDate,
                    LASTUPDATEDBY = entry.LastUpdatedBy,
                    LASTUPDATEDDATE = entry.LastUpdatedDate
                };
                _entryRepository.Add(entryModel);
            }

            _aliasEntryRepository.DeleteList(e => e.ENTRYID == entryModel.Id);
            var aliasList = new List<PKS_OILWIKI_ALIASENTRY>();
            if (entry.AliasEntry != null)
            {
                foreach (var item in entry.AliasEntry)
                {
                    aliasList.Add(new PKS_OILWIKI_ALIASENTRY { ENTRYID = entryModel.Id, NAME = item });
                }
                _aliasEntryRepository.AddRange(aliasList, false);
            }
            _aliasEntryRepository.Submit();


            _relatedRepository.DeleteList(e => e.ENTRYID == entryModel.Id);
            var relatedList = new List<PKS_OILWIKI_RELATEDENTRY>();
            if (entry.RelatedEntry != null)
            {
                foreach (var item in entry.RelatedEntry)
                {
                    relatedList.Add(new PKS_OILWIKI_RELATEDENTRY { RELATEDENTRYID = item, ENTRYID = entryModel.Id });
                }
                _relatedRepository.AddRange(relatedList, false);
            }
            _relatedRepository.Submit();

        }

        public int AddEntry(EntryDTO entry)
        {
            var newEntry = new PKS_OILWIKI_ENTRY
            {
                NAME = entry.Name,
                ENGLISHNAME = entry.EnglishName,
                CATALOGID = entry.CatalogId,
                AUTHOR = entry.Author,
                SOURCE = entry.Source,
                IMAGE = entry.Image,
                CONTENTS = entry.Contents,
                CREATEDBY = entry.CreatedBy,
                CREATEDDATE = entry.CreatedDate,
                LASTUPDATEDBY = entry.LastUpdatedBy,
                LASTUPDATEDDATE = entry.LastUpdatedDate
            };
            _entryRepository.Add(newEntry);

            //_aliasEntryRepository.DeleteList(e => e.ENTRYID == newEntry.Id);
            //entry.AliasEntry?.ForEach(a => _aliasEntryRepository.Add(new PKS_OILWIKI_ALIASENTRY { NAME = a, ENTRYID = newEntry.Id }, false));

            //_relatedRepository.DeleteList(e => e.ENTRYID == newEntry.Id);
            //entry.RelatedEntry?.ForEach(a => _relatedRepository.Add(new PKS_OILWIKI_RELATEDENTRY { RELATEDENTRYID = a, ENTRYID = newEntry.Id }, false));

            //_aliasEntryRepository.Submit();
            //_relatedRepository.Submit();


            _aliasEntryRepository.DeleteList(e => e.ENTRYID == newEntry.Id);
            var aliasList = new List<PKS_OILWIKI_ALIASENTRY>();
            if (entry.AliasEntry != null)
            {
                foreach (var item in entry.AliasEntry)
                {
                    aliasList.Add(new PKS_OILWIKI_ALIASENTRY { ENTRYID = newEntry.Id, NAME = item });
                }
                _aliasEntryRepository.AddRange(aliasList, false);
            }
            _aliasEntryRepository.Submit();


            _relatedRepository.DeleteList(e => e.ENTRYID == newEntry.Id);
            var relatedList = new List<PKS_OILWIKI_RELATEDENTRY>();
            if (entry.RelatedEntry != null)
            {
                foreach (var item in entry.RelatedEntry)
                {
                    relatedList.Add(new PKS_OILWIKI_RELATEDENTRY { RELATEDENTRYID = item, ENTRYID = newEntry.Id });
                }
                _relatedRepository.AddRange(relatedList, false);
            }
            _relatedRepository.Submit();

            return newEntry.Id;
        }


        public int AddCatalog(PKS_OILWIKI_CATALOG model)
        {
            _catalogTypeRepository.Add(model);
            return model.Id;
        }

        public void UpdateCatalog(PKS_OILWIKI_CATALOG model)
        {
            _catalogTypeRepository.Update(model);
        }

        public void DeleteCatalog(List<PKS_OILWIKI_CATALOG> models)
        {
            var modelArray = from a in models
                             orderby a.LEVELNUMBER descending
                             select a;
            foreach (var item in models)
            {
                _catalogTypeRepository.DeleteByKey(item);
            }
        }

        public bool CatalogNameExists(int id, string name)
        {
            var entitys = 0;
            if (id == 0)
                _catalogTypeRepository.GetQuery().Where(e => e.NAME == name).Count();
            else
                _catalogTypeRepository.GetQuery().Where(e => e.NAME == name && e.Id != id).Count();

            return entitys > 0;
        }


        public IEnumerable<PKS_OILWIKI_ALIASENTRY> GetAliasEntrys(int entryId)
        {
            var list = _aliasEntryRepository.GetQuery().Where(e => e.ENTRYID == entryId).Select(c => c).ToList();
            return list;
        }

        public IEnumerable<RelatedEntry> GetRelateEntrys(int entryId)
        {
            var list = _relatedRepository.GetQuery().Where(e => e.ENTRYID == entryId).Select(c => new RelatedEntry
            {
                EntryId = c.RELATEDENTRYID,
                EntryName = c.PKS_OILWIKI_ENTRY1.NAME
            }).ToList();

            return list;
        }

    }
}
