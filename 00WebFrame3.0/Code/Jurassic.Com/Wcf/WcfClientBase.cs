using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using Jurassic.Com.Tools;
using Jurassic.Com.Wcf;

namespace Jurassic.Com.Wcf
{
    /// <summary>
    /// 对ClientBase<TChannel>做一个最简单的包装，形成通信工厂的基类
    /// </summary>
    /// <typeparam name="TChannel"></typeparam>
    public class WcfClientBase<TChannel> : ClientBase<TChannel> where TChannel : class
    {


        //public WcfClientBase()
        //{
        //}

        //public WcfClientBase(string endpointConfigurationName) :
        //    base(endpointConfigurationName)
        //{
        //}

        //public WcfClientBase(string endpointConfigurationName, string remoteAddress) :
        //    base(endpointConfigurationName, remoteAddress)
        //{
        //}

        //public WcfClientBase(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) :
        //    base(endpointConfigurationName, remoteAddress)
        //{
        //}

        //public WcfClientBase(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) :
        //    base(binding, remoteAddress)
        //{
        //}

        /// <summary>
        /// 直接给调用者返回ClientBase<TChannel>的Channel代理对象
        /// </summary>
        /// <returns>TChannel代理对象</returns>
        public TChannel GetClientInstance()
        {
            TChannel tc = base.Channel;
            return tc;
        }

        /// <summary>
        /// 为ClientBase<TChannel>追加Authentication认证用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        public void AppendClientCredentials(string userName, string password)
        {

            if (userName.IsEmpty() || password == null)
            {
                return;
            }

            //安全证书

            ClientCredentials ccs = new ClientCredentials();

            ccs.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            //自动义X509证书验证器,继承的父类是 System.IdentityModel.Selectors.X509CertificateValidator
            CustomCertificateValidator cuv = new CustomCertificateValidator();
            ccs.ServiceCertificate.Authentication.CustomCertificateValidator = cuv;

            ccs.UserName.UserName = userName;
            ccs.UserName.Password = password;
            
            //增加安全行为配置
            this.Endpoint.Behaviors.Clear();
            this.Endpoint.Behaviors.Add(ccs);


        }

    }
}
