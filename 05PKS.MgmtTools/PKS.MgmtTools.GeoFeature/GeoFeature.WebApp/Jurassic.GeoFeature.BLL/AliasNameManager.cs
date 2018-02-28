using Jurassic.GeoFeature.Factory;
using Jurassic.GeoFeature.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.GeoFeature.BLL
{
    public class AliasNameManager
    {
        //public bool Exist(AliasNameModel model)
        //{
        //    return ObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").Exist(model);
        //}
        
        //public void Add(AliasNameModel model)
        //{
        //    ObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").Insert(model);
        //}

        //public void Update(AliasNameModel model)
        //{
        //    ObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").Update(model);
        //}

        //public int Delete(AliasNameModel model)
        //{
        //    return ObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").Delete(model);
        //}

        //public IList<AliasNameModel> GetListByID(string id)
        //{
        //    return ObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").GetListByID(id);
        //}

        //public IList<AliasNameModel> GetListByName(string name)
        //{
        //    return ObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").GetListByName(name);
        //}
        //public IList<AliasNameModel> GetList()
        //{
        //    return ObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").GetList();
        //}
        ///// <summary>
        ///// 根据对象ID和应用域确定对象别名
        ///// </summary>
        ///// <param name="boId"></param>
        ///// <param name="appDomain"></param>
        ///// <returns></returns>
        //public IList<AliasNameModel> GetAlisaNameByIDAndAppDomain(string boId, string appDomain)
        //{
        //    return ObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").GetAlisaNameByIDAndAppDomain(boId, appDomain);
        //}

        public int Delete(AliasNameModel model)
        {
            return PrivateObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").Delete(model);
        }

        public bool Exist(AliasNameModel model)
        {
            return PrivateObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").Exist(model);
        }

        public void Add(AliasNameModel model)
        {
            PrivateObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").Insert(model);
        }

        public void Update(AliasNameModel model)
        {
            PrivateObjectCreate<AliasNameModel>.CreatIAlisaName("AliasNameServer").Update(model);
        }
    }
}
