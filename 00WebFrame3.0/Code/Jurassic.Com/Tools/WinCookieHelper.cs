using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Runtime.InteropServices;
using System.IO;

namespace Jurassic.Com.Tools
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// Winform的Cookie操作帮助类
    /// </summary>
    public class WinCookieHelper
    {
        /// <summary>
        /// WebClient设置cookie!
        /// </summary>
        /// <param name="wc"></param>
        public static void SetWebClient(WebClient wc, string url, string cookieName, string cookieValue)
        {
            wc.Headers.Add("Cookie", String.Format("{0}={1};", cookieName, cookieValue));
            // 注意，这里是Cookie，不是Set-Cookie
            byte[] re = wc.UploadData(url, new byte[0]);
            System.Text.UTF8Encoding converter = new System.Text.UTF8Encoding();
            string str = converter.GetString(re);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lpszUrlName"></param>
        /// <param name="lbszCookieName"></param>
        /// <param name="lpszCookieData"></param>
        /// <returns></returns>
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
         static extern bool InternetSetCookie(string lpszUrlName, string lbszCookieName, string lpszCookieData);

        /// <summary>
        /// 设置HttpWebRequest的Cookie值(用&号和=号隔开 ）
        /// </summary>
        /// <param name="hreq"></param>
        /// <param name="cookieStr"></param>
        public static void SetToRequest(HttpWebRequest hreq, string cookieStr)
        {
            if (String.IsNullOrEmpty(cookieStr)) return; 
            //在WebBrowser中登录cookie保存在WebBrowser.Document.Cookie中    
            CookieContainer myCookieContainer = new CookieContainer();

            //String 的Cookie　要转成　Cookie型的　并放入CookieContainer中
            string[] cookstr = cookieStr.Split('&');

            foreach (string str in cookstr)
            {
                string[] cookieNameValue = str.Split('=');
                Cookie ck = new Cookie(cookieNameValue[0].Trim(), cookieNameValue[1].Trim());
               // if (hreq .RequestUri.Host == "localhost")
               // ck.Domain =  hreq.RequestUri.DnsSafeHost;//必须写对
                myCookieContainer.Add(ck);
            }

            //自己创建的CookieContainer
            hreq.CookieContainer = myCookieContainer;
        }
    }
}
