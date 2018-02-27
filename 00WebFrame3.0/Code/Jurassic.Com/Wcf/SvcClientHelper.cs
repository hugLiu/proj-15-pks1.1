using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Jurassic.Com.Wcf
{
    /// <summary>
    /// WCF的客户端代理生成帮助类
    /// </summary>
    public class SvcClientHelper
    {
        /// <summary>
        /// 生成新的WCF服务代理,如果需要新的代理时使用此方法
        /// </summary>
        /// <typeparam name="T">服务端接口类型</typeparam>
        /// <param name="bindingName"></param>
        /// <param name="wcfUrl"></param>
        /// <returns>客户端代理</returns>
        public T CreateClient<T>(string bindingName, string wcfUrl)
        {
            EndpointAddress address = new EndpointAddress(wcfUrl);
            ChannelFactory<T> factory = new ChannelFactory<T>(bindingName, address);
            T client = factory.CreateChannel();
            return client;
        }
        public T CreateClient<T>(string bindingName, string wcfUrl, string userName, string password)
        {
            EndpointAddress address = new EndpointAddress(wcfUrl);
            WSHttpBinding binding = new WSHttpBinding(bindingName);
            ChannelFactory<T> factory = new ChannelFactory<T>(binding, address);
            factory.Credentials.UserName.UserName = userName;
            factory.Credentials.UserName.Password = password;
            T client = factory.CreateChannel();
            return client;
        }

        public T CreateClient<T>(string bindingName, string wcfUrl, string userName, string password, string identityName)
        {
            //绑定
            //WSHttpBinding binding = new WSHttpBinding();
            //binding.Security.Message.ClientCredentialType = MessageCredentialType.UserName;
            WSHttpBinding binding = new WSHttpBinding(bindingName);


            //终结点“标识”属性
            Uri myUri = new Uri(wcfUrl);
            DnsEndpointIdentity ei = new DnsEndpointIdentity(identityName); //"ParkingServer"
            EndpointAddress address = new EndpointAddress(myUri, ei);

            //创建通道工厂
            ChannelFactory<T> factory = new ChannelFactory<T>(binding, address);

            factory.Credentials.UserName.UserName = userName; // "admin";
            factory.Credentials.UserName.Password = password; // "123456";

            //安全证书

            ClientCredentials ccs = new ClientCredentials();

            ccs.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            //自动义X509证书验证器  "Jurassic.AppCenter.Wcf.Client.CustomUserValidator,Jurassic.AppCenter.Wcf.Client";
            CustomCertificateValidator cuv = new CustomCertificateValidator();
            ccs.ServiceCertificate.Authentication.CustomCertificateValidator = cuv;

            ccs.UserName.UserName = userName; // "admin";
            ccs.UserName.Password = password; // "123456";
             
            //增加安全行为配置
            factory.Endpoint.Behaviors.Clear();
            factory.Endpoint.Behaviors.Add(ccs);


            //创建
            T client = factory.CreateChannel();

            
            return client;
        }

    }
}
