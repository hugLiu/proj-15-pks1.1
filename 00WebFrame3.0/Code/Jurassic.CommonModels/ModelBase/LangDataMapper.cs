using Jurassic.AppCenter;
using Jurassic.CommonModels.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Data.Entity;
using System.Reflection;
using Jurassic.AppCenter.Resources;
using System.Diagnostics;
using Jurassic.CommonModels.Articles;
using System.Web;
using Jurassic.Com.Tools;

namespace Jurassic.CommonModels.ModelBase
{
    /// <summary>
    /// 设置支持多语言属性的EF映射规则
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TModel"></typeparam>
    public class LangDataMapper<TModel, TEntity>
        where TModel : class, IId<Int32>
        where TEntity : class, IId<Int32>
    {
        IModelEntityConverter<TModel, TEntity> _converter;
        /// <summary>
        /// ctor
        /// </summary>
        public LangDataMapper()
        {
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            string entityName = typeof(TEntity).Name;
            ModelRule modelRule = ModelRule.Get<TModel>();
            ModelRule entityRule = ModelRule.Get<TEntity>();
            string lang = ResHelper.CurrentCultureName.ToLower();
            _converter = GetConverter<TModel, TEntity>();
            IMappingExpression<TModel, TEntity> expm2t = null;
            IMappingExpression<TEntity, TModel> expt2m = null;
            Mapper.Initialize(cfg =>
            {
                expm2t = cfg.CreateMap<TModel, TEntity>();
                expt2m = cfg.CreateMap<TEntity, TModel>();
            });
            if (_converter != null)
            {
                expt2m.ProjectUsing(_converter.EntityToModel);
                // expm2t.ConstructUsing(_converter.ModelToEntity.Compile()); 不知为何这句无效
            }
            else
            {
                if (typeof(IMultiLanguage).IsAssignableFrom(typeof(TEntity)))
                {
                    RefHelper.CallMethod(this, "MapperLangProperties", new Type[] { typeof(TModel), typeof(TEntity) });
                }
                foreach (var r in modelRule.CollectionRules)
                {
                    if (r.Attr.EntityType != null && Mapper.Configuration.FindTypeMapFor(r.ModelType, r.Attr.EntityType) == null)
                    {
                        Mapper.Initialize(cfg =>
                        {
                            cfg.CreateMap(r.Attr.EntityType, r.ModelType);
                            cfg.CreateMap(r.ModelType, r.Attr.EntityType);
                        });
                    }
                    expt2m.ForMember(r.Name, opt => opt.Ignore());
                }
                foreach (var r in entityRule.CollectionRules)
                {
                    expm2t.ForMember(r.Name, opt => opt.Ignore());
                }
            }
            //Debug.WriteLine("Mapper Elapsed == " + sw.ElapsedMilliseconds);
        }

        /// <summary>
        /// 用AutoMapper映射出多语言字段属性, 此方法被反射调用
        /// </summary>
        /// <typeparam name="TM">业务实体类型</typeparam>
        /// <typeparam name="TE">数据实体类型</typeparam>
        private void MapperLangProperties<TM, TE>()
                where TE : class, IMultiLanguage, IId<int>
        {
            ModelRule rule = ModelRule.Get<TM>();
            foreach (var r in rule.SingleRules.Where(r1 => r1.DataType == ExtDataType.MultiLanguage))
            {
                string propName = r.Name;
                string typeName = typeof(TE).Name;

                Mapper.Initialize(cfg =>
                {
                    cfg.CreateMap<TE, TM>()
                    .ForMember(propName, opt => opt.MapFrom(src => src.LangTexts
                    .FirstOrDefault(lt => lt.Name == propName && lt.BillType == typeName
                    && lt.Language.Equals(ResHelper.CurrentCultureName, StringComparison.OrdinalIgnoreCase)).Text));
                });
            }
        }

        private IModelEntityConverter<TModelItem, TEntityItem> GetConverter<TModelItem, TEntityItem>()
            where TModelItem : class
            where TEntityItem : class
        {
            var converter = SiteManager.Get<IModelEntityConverter<TModelItem, TEntityItem>>();
            if (converter == null && typeof(TModelItem) == typeof(TEntityItem))
            {
                converter = SiteManager.Get<SameEntityModelConverter<TModelItem, TEntityItem>>();
            }
            return converter;
        }

        /// <summary>
        /// 将业务实体对象转换为数据实体对象
        /// </summary>
        /// <param name="t">业务实体对象</param>
        /// <returns>数据实体对象</returns>
        public TEntity ToEntity(TModel t)
        {
            if (_converter == null)
            {
                return Mapper.Map<TModel, TEntity>(t);
            }
            else
            {
                return _converter.ModelToEntity.Compile()(t);
            }
        }

        /// <summary>
        /// 保存多语言的文本信息
        /// </summary>
        /// <param name="dataService">当前的数据服务</param>
        /// <param name="e">数据实体对象</param>
        /// <param name="t">业务实体对象</param>
        public void SaveLanguages(EFAuditDataService<TEntity> dataService, TEntity e, TModel t)
        {
            var me = e as IMultiLanguage;
            if (me == null) return;
            ModelRule modelRule = ModelRule.Get<TModel>();
            string entityType = typeof(TEntity).Name;
            var langs = ResHelper.GetUsedCultureNames().Select(l => l.ToLower()).ToArray();
            if (me.LangTexts == null)
            {
                me.LangTexts = dataService.GetContext().Set<Sys_DataLanguage>().Where(d => d.BillId == e.Id && d.BillType == entityType).ToList();
            }
            var sysLangs = me.LangTexts;
            foreach (var rule in modelRule.SingleRules.Where(r => r.DataType == ExtDataType.MultiLanguage))
            {
                //先处理主文本框name不带语言后缀的表单值,表示是当前语言
                string langVal = HttpContext.Current.Request.Form[rule.Name];
                string currentLang = ResHelper.CurrentCultureName.ToLower();
                var langEntity = sysLangs.FirstOrDefault(l => l.Language == currentLang);
                if (langEntity == null)
                {
                    langEntity = new Sys_DataLanguage
                    {
                        BillId = e.Id,
                        BillType = entityType,
                        Language = currentLang,
                        Name = rule.Name,
                        Text = langVal
                    };
                    dataService.MarkState(langEntity, EntityState.Added);
                }

                else if (langVal != langEntity.Text)
                {
                    langEntity.Text = langVal;
                    dataService.MarkState(langEntity, EntityState.Modified);
                }

                RefHelper.SetValue(e, rule.Name, langVal);

                //再处理下拉列表中的文本框name带语言后缀的表单值
                foreach (var lang in langs.Where(l => l != currentLang))
                {
                    langVal = HttpContext.Current.Request.Form[rule.Name + "-" + lang];
                    langEntity = sysLangs.FirstOrDefault(l => l.Language == lang);
                    if (langEntity == null)
                    {
                        langEntity = new Sys_DataLanguage
                        {
                            BillId = e.Id,
                            BillType = entityType,
                            Language = lang,
                            Name = rule.Name,
                            Text = langVal
                        };
                        dataService.MarkState(langEntity, EntityState.Added);
                    }
                    else if (langVal != langEntity.Text)
                    {
                        langEntity.Text = langVal;
                        dataService.MarkState(langEntity, EntityState.Modified);
                    }
                }
            }
        }
    }
}
