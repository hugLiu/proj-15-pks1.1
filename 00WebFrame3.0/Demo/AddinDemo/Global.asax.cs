using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
//using System.Web.Optimization;
using System.Web.Routing;
using Jurassic.Com.Tools;
using System.Globalization;
using Jurassic.AppCenter.Resources;
using Jurassic.AppCenter.Logs;
using Ninject;
using Jurassic.CommonModels.Organization;
using Jurassic.CommonModels;
using Jurassic.CommonModels.EFProvider;
using Jurassic.WebFrame;
using AddinDemo.Models;
using System.Data.Entity;
using AutoMapper;

namespace AddinDemo
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    /// <summary>
    /// 应用程序必须继承MvcApplication
    /// </summary>
    public class Application1 : MvcApplication
    {
    }
}