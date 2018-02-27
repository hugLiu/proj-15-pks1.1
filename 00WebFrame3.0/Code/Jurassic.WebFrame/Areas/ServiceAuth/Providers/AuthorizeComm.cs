using Jurassic.CommonModels.ServerAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;

namespace Jurassic.WebFrame.Providers
{
    public class AuthorizeComm
    {
        /// <summary>
        /// 获取通讯的安全令牌
        /// 注:通过文件头信息传递账号与验证信息,如果通过Body传递账号以及验证信息需要改变服务端获取数据的方式
        /// </summary>
        /// <param name="urlString">远程服务器地址.例:http://www.xxx.com/api</param>
        /// <param name="clientId">客户组账号ID</param>
        /// <param name="clientSecret">客户授权Key</param>
        /// <returns>返回获取安全令牌的对象</returns>
        public static AccessToken GetAccessTokenHeaders(string urlString, string clientId, string clientSecret)
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(urlString);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
               "Basic",
               Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret)));

            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "client_credentials");

            HttpResponseMessage res = _httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters)).Result;

            AccessToken accessTokenModel = new AccessToken();
            if (res.IsSuccessStatusCode)
            { 
                accessTokenModel = res.Content.ReadAsAsync<AccessToken>().Result;
            }
            accessTokenModel.isSuccessStatusCode = res.IsSuccessStatusCode;
            accessTokenModel.statusCode = res.StatusCode.ToString();
            accessTokenModel.reasonMessage = res.ReasonPhrase;
            return accessTokenModel;
        }

        /// <summary>
        /// 刷新通讯的安全令牌
        /// 注:通过文件头信息传递账号与验证信息,如果通过Body传递账号以及验证信息需要改变服务端获取数据的方式
        /// </summary>
        /// <param name="urlString">远程服务器地址.例:http://www.xxx.com/api</param>
        /// <param name="clientId">客户组账号ID</param>
        /// <param name="clientSecret">客户授权Key</param>
        /// <param name="refreshToken">服务端刷新Key</param>
        /// <returns>返回获取安全令牌的对象</returns>
        public static AccessToken RefreshToKen(string urlString, string clientId, string clientSecret, string refreshToken)
        {
            HttpClient _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(urlString);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
               "Basic",
               Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret)));

            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "refresh_token");
            parameters.Add("refresh_token", refreshToken);

            HttpResponseMessage res = _httpClient.PostAsync("/token", new FormUrlEncodedContent(parameters)).Result;
            AccessToken accessTokenModel = new AccessToken();
            if (res.IsSuccessStatusCode)
            {
                accessTokenModel = res.Content.ReadAsAsync<AccessToken>().Result;
            }
            accessTokenModel.isSuccessStatusCode = res.IsSuccessStatusCode;
            accessTokenModel.statusCode = res.StatusCode.ToString();
            accessTokenModel.reasonMessage = res.ReasonPhrase;
            return accessTokenModel;
        }

        /// <summary>
        /// 通过客户组Id查询该客户授权的数据节点
        /// </summary>
        /// <param name="clientId">客户组账号ID</param>
        /// <returns></returns>
        public static List<ViDataAuth> GetAuthdataByClientId(string clientId)
        {
            return ApiManager.mServerAuthManager.GetDataNodesByClientId(clientId);
        }
         
        /// <summary>
        /// 获取该客户的数据节点是否授权
        /// </summary>
        /// <param name="clientId">客户组账号ID</param>
        /// <param name="dataNodeID">数据节点ID</param>
        /// <returns></returns>
        public static bool IsAuthData(string clientId, string dataNodeID)
        {
            return ApiManager.mServerAuthManager.IsAuthData(clientId, dataNodeID);
        }

        /// <summary>
        /// 获取该客户的服务节点是否授权
        /// </summary>
        /// <param name="clientId">客户组账号ID</param>
        /// <param name="actionName">服务名称</param>
        /// <param name="serviceFullName">服务完成方法名称(包含命名空间名称)</param>
        /// <returns></returns>
        public static bool IsAuthService(string clientId, string actionName, string serviceFullName)
        {
            return ApiManager.mServerAuthManager.IsAuthService(clientId, actionName, serviceFullName);
        }

        /// <summary>
        /// 通过客户组Id查询该客户授权的服务节点
        /// </summary>
        /// <param name="clientId">客户组编码ID</param>
        /// <returns></returns>
        public static List<ViServiceAuth> GetServiceByClientId(string clientId)
        {
            return ApiManager.mServerAuthManager.GetServiceByClientId(clientId);
        }


    }
}