using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// web页面提交请求的方法
    /// </summary>
    public enum WebMethod
    {
        GET,
        POST,
        PUT,
        DELETE,
        OPTIONS,
        PATCH,
    }

    /// <summary>
    /// 授权类型
    /// </summary>
    public enum JAuthType
    {
        /// <summary>
        /// 需要角色权限
        /// </summary>
        NeedAuth =0,

        /// <summary>
        /// 所有登录用户可访问
        /// </summary>
        AllUsers =1,

        /// <summary>
        /// 所有人可访问
        /// </summary>
        EveryOne =2,

        /// <summary>
        /// 所有人可访问, 并且扫描程序会忽略掉此方法的权限判断。<br />
        /// 也不会在功能树中出现
        /// </summary>
        Ignore=3,

        /// <summary>
        /// 所有人都不可访问
        /// </summary>
        Forbidden=4,
    }

    /// <summary>
    /// 功能项出现的方式 
    /// </summary>
    [Flags]
    public enum VisibleType
    {
        /// <summary>
        /// 表示既不在权限控制菜单也不在用户菜单显示
        /// </summary>
        None = 0,

        /// <summary>
        /// 表示在用户菜单显示
        /// </summary>
        Menu = 1,

        /// <summary>
        /// 在权限菜单显示
        /// </summary>
        Role = 2,

        /// <summary>
        /// 同时在权限菜单和用户菜单显示
        /// </summary>
        All = 3,

        /// <summary>
        /// 在顶部显示
        /// </summary>
        Top=4,
        
        /// <summary>
        /// 此菜单项点击后会形成一个容器
        /// </summary>
        Container = 8,

        /// <summary>
        /// 作为按钮显示
        /// </summary>
        Button = 16,

        /// <summary>
        /// 表示是一个按钮组的第一个按钮
        /// </summary>
        GroupBegin = 32,

        /// <summary>
        /// 表示是快捷访问工具栏
        /// </summary>
        QuckAccessBar = 64,

        /// <summary>
        /// 大尺寸按钮
        /// </summary>
        LargeSize = 128,

        /// <summary>
        /// 服务
        /// </summary>
        IsService = 256,

        /// <summary>
        /// 默认提交按钮
        /// </summary>
        DefaultButton = 512,

        /// <summary>
        /// 首页部件
        /// </summary>
        Widget = 1024
    }
}
