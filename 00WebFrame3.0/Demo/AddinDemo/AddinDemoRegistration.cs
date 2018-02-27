using AddinDemo.Models;
using AutoMapper;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.EFProvider;
using Jurassic.CommonModels.ModelBase;
using Jurassic.WebFrame;
using Jurassic.WebQuery;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using Jurassic.AppCenter.Resources;

namespace AddinDemo
{
    public class AdvDemoRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "AddinDemo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //要支持Oralce数据库，请在""中填写Oralce库的Schema名称
            SiteManager.Kernel.Rebind<DbContext, ModelContext>().To<PersonContext>()
              .WithPropertyValue("Schema", "");
            //SiteManager.Kernel.Bind<IModelEntityConverter<PersonModel, Person>>().To<PersonConverter>();
            SiteManager.Kernel.Bind<IModelEntityConverter<EduHistoryModel, EduHistory>>().To<EduHistoryConverter>();
            SiteManager.Kernel.Bind<IModelEntityConverter<HonorModel, Honor>>().To<HonorConverter>();

            SiteManager.Kernel.Bind<ICurrentDepartment>().To<TestDeptIdProvider>();
            Mapper.Initialize(cfg=>cfg.CreateMap<Supplier, SupplierModel>()
                .ForMember(s => s.ContacterName,
                opt => opt.MapFrom(s => s.Contacter.LangTexts.FirstOrDefault(l => l.Name == "Name" && l.BillType == "Person" && l.Language == ResHelper.CurrentCultureName).Text)));
            //SiteManager.Kernel.Bind<IModelEntityConverter<SupplierModel, Supplier>>().To<SupplierConverter>();

            //设定多语言文本框语言图标的显示方式
            SiteManager.Kernel.Bind<LangTextFormData>().ToSelf().WithPropertyValue("IconType", CultureIconType.Text);
        }

        class TestDeptIdProvider : ICurrentDepartment
        {
            public int DeptId
            {
                get { return 28;  }
                set { }
            }
        }
    }
}
