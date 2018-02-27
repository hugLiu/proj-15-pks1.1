using AddinDemo.Models;
using AutoMapper;
using Jurassic.AppCenter;
using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Jurassic.CommonModels.Articles;
using Jurassic.CommonModels.EFProvider;
using Jurassic.CommonModels.EntityBase;
using Jurassic.CommonModels.ModelBase;
using Jurassic.WebFrame;
using Jurassic.WebQuery;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace AddinDemo.Controllers
{
    /// <summary>
    /// 利用高级数据组件，主从关系的数据展示及增删改
    /// </summary>
    public class SuppliersController : AdvDataController<SupplierModel, Supplier>
    {
        public SuppliersController()
        {
            SiteManager.Kernel.Rebind<DbContext, ModelContext>().To<PersonContext>().WithPropertyValue("Schema", "");
        }

        public override ActionResult Index()
        {
            return base.Index();
        }

        //如果要添加额外的条件，可以使用此方法
        //public override JsonResult GetData()
        //{
        //    var jr = base.GetData();
        //    IQueryable<SupplierModel> query = jr.Data as IQueryable<SupplierModel>;

        //    jr.Data = query.Where(t => t.Level >= SupplierLevel.Medium);
        //    return jr;
        //}
        public override JsonResult GetUserDefineList(string prop)
        {

            if (prop == "ContacterId")
            {
                ModelDataService<PersonModel, Person> persons = SiteManager.Get<ModelDataService<PersonModel, Person>>();
                var linked = persons.GetQuery()
                    .Select(p => new
                    {
                        id = p.Id,
                        text = p.Name
                    });

                return Json(linked, JsonRequestBehavior.AllowGet);
            }
            return base.GetUserDefineList(prop);
        }

        protected override void BeforeShowing(SupplierModel t)
        {
            t.Products = t.Products.OrderBy(p => p.Price).ToList();
        }

        public override void BeforeShowingPage(Pager<SupplierModel> pagedData)
        {
            foreach (var model in pagedData)
            {
                model.Name = model.Name + ".Doctor";
            }

        }

        public override ActionResult Edit(int id = 0)
        {
            return base.Edit(id);
        }
    }

    /// <summary>
    /// 测试数据的初始化类
    /// </summary>
    class SupplierProductDataInit
    {
        ModelDataService<SupplierModel, Supplier> _provider;
        public SupplierProductDataInit(ModelDataService<SupplierModel, Supplier> dataProvider)
        {
            _provider = dataProvider;
        }

        List<SupplierModel> _models = new List<SupplierModel>
        {
            new SupplierModel{
                 Id = 1,
                Address = "湖北武汉",
                 Email = "123@456.com",
                 CreaterId = 2,
                  Name = "S1",
                  Tel = "5332239",
                  Products = new List<ProductModel>
                  {
                      new ProductModel{ Id = 1, Name = "TV", Price = 3000, Unit = "台"},
                      new ProductModel{ Id = 2, Name = "PC", Price = 5500, Unit = "台"},
                  }
            },
                new SupplierModel{
                 Id = 2,
                Address = "中国北京",
                 Email = "123@456.com",
                  Name = "S2",
                   Tel = "6213546",
                            CreaterId = 2,
                  Level = SupplierLevel.Medium,
       Products = new List<ProductModel>
                  {
                      new ProductModel{ Id = 3, Name = "CAR", Price = 120000, Unit = "辆"},
                      new ProductModel{ Id = 4, Name = "BUS", Price = 1500000, Unit = "台"},
                    new ProductModel{ Id = 5, Name = "PLANE", Price = 8000000, Unit = "架"},
          }
            },
            new SupplierModel{
                 Id = 3,
                Address = "福建厦门",
                  CreaterId = 2,
              Email = "123@456.com",
                  Name = "S3",
                   Tel = "3421579",
                  Products = new List<ProductModel>
                  {
                      new ProductModel{ Id = 6, Name = "APPLE", Price = 8, Unit = "斤"},
                      new ProductModel{ Id = 7, Name = "ORANGE", Price = 4, Unit = "斤"},
                      new ProductModel{ Id = 8, Name = "PEAL", Price = 3, Unit = "斤"},
                      new ProductModel{ Id = 9, Name = "POTATO", Price = 2, Unit = "斤"},
                  }
            },
            new SupplierModel{
                 Id = 4,
                          CreaterId = 2,
               Address = "河北石家庄",
                 Email = "123@456.com",
                  Name = "S4",
                   Tel = "4135279",
                    Level = SupplierLevel.Large
            },
        };

        public void InitData()
        {
            if (_provider.GetQuery().Count() == 0)
            {
                _provider.Add(_models);
            }
        }
    }
}
