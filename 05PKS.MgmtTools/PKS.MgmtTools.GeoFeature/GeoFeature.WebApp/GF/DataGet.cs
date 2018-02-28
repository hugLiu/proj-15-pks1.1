using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Jurassic.GeoFeature.Model;

namespace GeoFeature
{
    static class TypeClass
    {
        public static List<TypeClassModel> GetTypeClass()
        {
            List<TypeClassModel> tcl = new List<TypeClassModel>();
            tcl.Add(new TypeClassModel() { ClassId = "111", ClassName = "地质对象" });
            tcl.Add(new TypeClassModel() { ClassId = "222", ClassName = "工程对象" });
            tcl.Add(new TypeClassModel() { ClassId = "333", ClassName = "管理对象" });

            return tcl;
        }
    }
    static class ObjType
    {
        public static List<ObjectTypeModel> GetObjType()
        {
            List<ObjectTypeModel> otl = new List<ObjectTypeModel>();
            otl.Add(new ObjectTypeModel() { Botid = "1", ClassId = "111", FeatureType = "", HasGeometry = "1", IsUserDefin = "0", Bot = "盆地", Shape = "Polygon" });
            otl.Add(new ObjectTypeModel() { Botid = "2", ClassId = "111", FeatureType = "", HasGeometry = "1", IsUserDefin = "0", Bot = "区带", Shape = "Polygon" });
            otl.Add(new ObjectTypeModel() { Botid = "3", ClassId = "111", FeatureType = "", HasGeometry = "1", IsUserDefin = "0", Bot = "油气田", Shape = "Polygon" });
            otl.Add(new ObjectTypeModel() { Botid = "4", ClassId = "111", FeatureType = "", HasGeometry = "1", IsUserDefin = "0", Bot = "井", Shape = "Point" });
            otl.Add(new ObjectTypeModel() { Botid = "5", ClassId = "222", FeatureType = "", HasGeometry = "0", IsUserDefin = "1", Bot = "项目" });

            return otl;
        }

    }
    static class ObjTyptProperty
    {
        public static List<ObjTypePropertyModel> GetObjTypeProperty()
        {
            List<ObjTypePropertyModel> otpl = new List<ObjTypePropertyModel>();
            otpl.Add(new ObjTypePropertyModel() { Botid = "1", Botname = "盆地", Ns = "基础参数", Md = "<PropertySet name=\"基础参数\"><P  n=\"坐标范围\" t=\"String\"> </P>   <P  n=\"盆地面积\" t=\"Decimal\"> </P><P  n=\"盆地类型\" t=\"String\"> </P></PropertySet >" });
            otpl.Add(new ObjTypePropertyModel() { Botid = "2", Botname = "区带", Ns = "基础参数", Md = "<PropertySet name=\"基础参数\">  <P  n=\"类型\" t=\"String\"> </P>   <P  n=\"坐标范围\" t=\"String\"> </P>   </PropertySet >" });
            otpl.Add(new ObjTypePropertyModel() { Botid = "3", Botname = "油气田", Ns = "基础参数", Md = "<PropertySet name=\"基础参数\">  <P  n=\"坐标范围\" t=\"String\"> </P> <P n=\"面积\" t=\"Decimal\"> </P><P  n=\"类型\" t=\"String\"> </P>   <P  n=\"油藏类别\" t=\"String\"> </P>   <P  n=\"储量级别\" t=\"String\"> </P>   <P  n=\"含油面积\" t=\"String\"> </P>   </PropertySet >" });
            otpl.Add(new ObjTypePropertyModel() { Botid = "4", Botname = "井", Ns = "基础参数", Md = "<PropertySet name=\"基础参数\"><P n=\"井名称\" t=\"String\"> </P> <P n=\"井坐标\" t=\"String\"> </P> <P n=\"井类别\" t=\"String\"> </P> <P n=\"井型\" t=\"String\"> </P> <P n=\"井深\" t=\"Decimal\"> </P> <P n=\"补心海拔\" t=\"Decimal\"> </P> <P n=\"完钻日期\" t=\"Date\"> </P> <P n=\"完钻层位\" t=\"String\"> </P> </PropertySet >" });
            otpl.Add(new ObjTypePropertyModel() { Botid = "4", Botname = "井", Ns = "钻井参数", Md = "<PropertySet name=\"钻井参数\"><P n=\"开钻日期\" t=\"Date\"></P><P  n=\"完钻井深\" t=\"Decimal\"></P></PropertySet>" });
            otpl.Add(new ObjTypePropertyModel() { Botid = "4", Botname = "井", Ns = "生产参数", Md = "<PropertySet name=\"生产参数\"><P n=\"日产水\" t=\"Decimal\"></P><P n=\"日产气\" t=\"Decimal\"></P><P n=\"日产油\" t=\"Decimal\"></P></PropertySet>" });
            otpl.Add(new ObjTypePropertyModel() { Botid = "5", Botname = "项目", Ns = "基础参数", Md = "<PropertySet name=\"基础参数\">  <P  n=\"项目名称\" t=\"String\"></P></PropertySet>" });

            return otpl;
        }
    }
}