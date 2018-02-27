using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Reflection;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Jurassic.Com.Tools
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 用于asp.net的Helper类
    /// </summary>
    public static class WebHelper
    {
        /// <summary>
        /// 获取Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <returns></returns>
        public static string GetCookie(String cookieName)
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                return HttpContext.Current.Request.Cookies[cookieName].Value ?? "";
            }
            return String.Empty;
        }

        /// <summary>
        /// 移除Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        public static void RemoveCookie(String cookieName)
        {
            var cok = HttpContext.Current.Request.Cookies[cookieName];
            if (cok == null)
            {
                return;
            }
            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
            cok.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
            HttpContext.Current.Response.AppendCookie(cok);
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        public static void SetCookie(string cookieName, string value)
        {
            HttpContext.Current.Response.Cookies[cookieName].Value = value;
        }

        /// <summary>
        /// 保存Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="value"></param>
        /// <param name="expireDays"></param>
        public static void SetCookie(string cookieName, string value, int expireDays)
        {
            HttpContext.Current.Response.Cookies[cookieName].Value = value;
            HttpContext.Current.Response.Cookies[cookieName].Expires = DateTime.Now.AddDays(expireDays);
        }

        /// <summary>
        /// 批量保存Cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="values"></param>
        /// <param name="CookiesKeyName"></param>
        public static void SetCookies(string cookieName, string[] values, string CookiesKeyName)
        {
            for (int i = 0; i < values.Length; i++)
            {
                HttpContext.Current.Response.Cookies[cookieName].Values.Add(CookiesKeyName.Split(new char[] { ',' })[i], values[i]);
            }
        }

        /// <summary>
        /// 获取绝对地址
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string TrueUrl(object s)
        {
            return s.ToString().Replace("~/", HttpContext.Current.Request.ApplicationPath + "/").Replace("//", "/");
        }

        /// <summary>
        /// 提供一个文件下载的动作
        /// </summary>
        /// <param name="filePath"></param>
        public static void DownLoadFile(string filePath)
        {
            HttpResponse rsp = HttpContext.Current.Response;

            if (filePath.StartsWith("~/"))
            {
                filePath = HttpContext.Current.Server.MapPath(filePath);
            }

            rsp.ClearHeaders();
            rsp.ContentType = "application/x-msdownload";
            rsp.AddHeader("Content-Disposition", "attachment;filename=" + Path.GetFileName(filePath));

            rsp.TransmitFile(filePath);
        }

        /// <summary>
        /// 请求指定url,并返回结果，如果遇到错误返回空串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetWebResponseText(string url)
        {
            try
            {
                HttpWebRequest smsReq = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = smsReq.GetResponse();
                return GetWebResponseText(response);
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 请求指定url,并返回结果,并不处理错误
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetWebResponseTextCore(string url)
        {
            HttpWebRequest smsReq = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = smsReq.GetResponse();
            return GetWebResponseText(response);
        }

        /// <summary>
        /// 请求指定url,并返回结果,处理错误时返回空串
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetWebResponseText(string url, Encoding encode)
        {
            try
            {
                HttpWebRequest smsReq = (HttpWebRequest)WebRequest.Create(url);
                WebResponse response = smsReq.GetResponse();
                return GetWebResponseText(response, encode);
            }
            catch
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// 返回数据流,并不处理错误
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static string GetWebResponseText(WebResponse response)
        {
            Stream receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream);

            string result = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return result;
        }

        /// <summary>
        /// 返回数据流
        /// </summary>
        /// <param name="response"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string GetWebResponseText(WebResponse response, Encoding encode)
        {
            Stream receiveStream = response.GetResponseStream();

            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encode);

            string result = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return result;
        }

        /// <summary>
        /// 用指定方式请求指定url,并返回结果
        /// </summary>
        /// <param name="url">提交到的URL</param>
        /// <param name="postData">提交的数据</param>
        /// <param name="method">提交方法，默认为POST</param>
        /// <returns>返回结果</returns>
        public static string GetWebResponseText(string url, string postData, string method="POST")
        {
            HttpWebRequest smsReq = (HttpWebRequest)WebRequest.Create(url);
            //将postData一起发送
            //...

            //postData = GetEncodeString(postData);
            smsReq.Method = method;
            //Encoding encoding = new Encoding();
            byte[] byte1 = Encoding.Default.GetBytes(postData);

            smsReq.ContentType = "application/x-www-form-urlencoded";
            smsReq.ContentLength = byte1.Length;

            Stream InputStream = smsReq.GetRequestStream();
            InputStream.Write(byte1, 0, byte1.Length);
            InputStream.Close();

            WebResponse response = smsReq.GetResponse();
            return GetWebResponseText(response);
        }

        /// <summary>
        /// 获取URL编码以后的字符串
        /// </summary>
        /// <param name="bs"></param>
        /// <returns></returns>
        public static String GetEncodeString(byte[] bs)
        {
            string encoded = "";

            if (bs == null) return null;

            if (bs.Length > 100)
            {
                StringBuilder sb = new StringBuilder(bs.Length * 5);
                foreach (byte b in bs)
                {
                    if (b < 128)
                    {
                        sb.Append(b);
                    }
                    else
                    {
                        sb.Append(String.Format("%{0:x}", b));
                    }
                }
                return sb.ToString();
            }
            else
            {
                foreach (byte b in bs)
                {
                    if (b < 128)
                    {
                        encoded += (char)b;
                    }
                    else
                    {
                        encoded += String.Format("%{0:x}", b);
                    }
                }
            }
            return encoded;
        }

        /// <summary>
        /// 获取URL编码以后的字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String GetEncodeString(string s)
        {
            byte[] bs = Encoding.Default.GetBytes(s);
            return GetEncodeString(bs);
        }

        /// <summary>
        /// 从网络URL获取流
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <returns>Stream</returns>
        public static Stream GetStreamFromUrl(string url)
        {
            if (String.IsNullOrEmpty(url)) return null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            return stream;
        }

        /// <summary>
        /// 从ftp服务器下载文件列表
        /// </summary>
        /// <param name="serverUri"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string FtpDisplayFile(Uri serverUri, string userName, string password)
        {
            // The serverUri parameter should start with the ftp:// scheme.
            if (serverUri.Scheme != Uri.UriSchemeFtp)
            {
                return "";
            }
            // Get the object used to communicate with the server.
            WebClient request = new WebClient();

            // This example assumes the FTP site uses anonymous logon.
            request.Credentials = new NetworkCredential(userName, password);
            try
            {
                byte[] newFileData = request.DownloadData(serverUri.ToString());
                string fileString = System.Text.Encoding.UTF8.GetString(newFileData);
                return fileString;
            }
            catch (WebException e)
            {
                return "";
            }
        }

        /// <summary>
        /// 从网络URL获取图片
        /// </summary>
        /// <param name="url">URL地址</param>
        /// <returns>Stream</returns>
        public static Image GetImageFromUrl(string url)
        {
            if (String.IsNullOrEmpty(url)) return null;
            try
            {
                return Image.FromStream(GetStreamFromUrl(url));
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (WebException)
            {
                return null;
            }
        }

        static System.Collections.Generic.Dictionary<string, string> TableOfContent = new System.Collections.Generic.Dictionary<string, string>()
        {
            {"docx", "application/msword"},
            {"doc", "application/msword"},
            {"exe","application/octet-stream"},
            {"tar","application/x-tar"},
            {"gtar", "application/x-gtar"},
            {"css", "text/css"},
            {"html", "text/html"},
            {"htm", "text/html"},
            {"wav", "audio/x-wav"},
            {"bin", "application/octet-stream"},
            {"txt", "text/plain"},
            {"js", "text/javascript"},
        };
        /// <summary>
        /// 获取类型
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        public static string GetContentType(string ext)
        {
            if (TableOfContent.ContainsKey(ext.ToLower()))
            {
                return TableOfContent[ext.ToLower()];
            }
            else return "application/" + ext;

        }

        public static string GetResponeText(this HttpServerUtilityBase server, string path)
        {
            StringBuilder sb =new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            server.Execute("~"+path, sw);
            sw.Close();
            return sb.ToString();
        }

    }
}