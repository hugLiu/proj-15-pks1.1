using HtmlAgilityPack;
using PKS.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PKS.DbServices.WEditor
{
    public class WEditorService : AppService, IPerRequestAppService
    {
        /// <summary>
        /// 解析html
        /// </summary>
        /// <param name="serverUrl">服务器路径</param>
        /// <param name="locationHtmlFilePath">本地html文件绝对路径</param>
        /// <returns></returns>
        public string AnalysisHtml(string serverUrl, string locationHtmlFilePath)
        {
            string htmlStr = string.Empty;

            if (string.IsNullOrEmpty(locationHtmlFilePath))
            {
                throw new ArgumentNullException("htmlFilePath参数为空");
            }

            var doc = new HtmlDocument();
            doc.Load(locationHtmlFilePath, Encoding.GetEncoding("GB2312"));
            HtmlNode docNode = doc.DocumentNode;
            var bodyNode = docNode.SelectSingleNode("//body");

            //查询图片
            var imgNodeList = bodyNode.SelectNodes("//img");
            if (imgNodeList != null)
            {
                foreach (var item in imgNodeList)
                {
                    string absolutePath = ImgSrcConvertAbsolutePath(item, serverUrl);
                }
            }

            htmlStr = bodyNode.InnerHtml.Trim();

            //doc.Save(@"F:\aaaa.html");
            return htmlStr;
        }
        private string ImgSrcConvertAbsolutePath(HtmlNode node, string serverUrl)
        {
            string absolutePath = string.Empty;

            string srcStr = node.GetAttributeValue("src", "");
            if (string.IsNullOrEmpty(srcStr)) return absolutePath;


            //FileInfo fi = new FileInfo(filePath);
            //var directoryName = fi.DirectoryName + "/"+ srcStr;
            //absolutePath = "../../upload/image/20171212/" + srcStr;
            
            absolutePath = serverUrl + srcStr;
            node.SetAttributeValue("src", absolutePath);

            return absolutePath;
        }


        private string ImageConvertBase64(HtmlNode node, string filePath)
        {
            string base64Str = string.Empty;
            string srcStr = node.GetAttributeValue("src", "");
            if (string.IsNullOrEmpty(srcStr)) return base64Str;

            string format = string.Empty;
            string tempImagePath = GetTempImageFilePath(filePath, srcStr, out format);
            if (string.IsNullOrEmpty(tempImagePath)) return base64Str;

            string imgBase64Str = ImageToBase64String(tempImagePath);
            if (string.IsNullOrEmpty(imgBase64Str)) return base64Str;

            base64Str = string.Format("data:image/{0};base64,{1}", format, imgBase64Str);
            node.SetAttributeValue("src", base64Str);

            return base64Str;
        }


        /// <summary>
        /// 获取当前文件对应的临时图片存储路径
        /// </summary>
        /// <param name="filePath">当前html文件路径</param>
        /// <param name="src">图片名称</param>
        /// <param name="format">图片格式</param>
        /// <returns></returns>
        private string GetTempImageFilePath(string filePath, string src, out string format)
        {
            string imagePath = string.Empty;

            //扩展名处理
            var extensionName = src.Substring(src.LastIndexOf("."), (src.Length - src.LastIndexOf(".")));
            switch (extensionName.ToLower())
            {
                case ".jpeg":
                case ".jpg":
                    format = "jpeg";
                    break;
                case ".gif":
                    format = "gif";
                    break;
                case ".png":
                    format = "png";
                    break;
                default:
                    format = string.Empty;
                    break;
            }
            if (string.IsNullOrEmpty(format))
            {
                return string.Empty;
            }


            FileInfo fi = new FileInfo(filePath);
            var directoryName = fi.DirectoryName + "\\";
            //directoryName = System.IO.Path.GetDirectoryName(filePath);
            imagePath = directoryName + src;
            return imagePath;
        }

        /// <summary>
        /// 图片转成Base64字符串
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        private string ImageToBase64String(string imagePath)
        {
            string imageBase64Str = string.Empty;

            if (string.IsNullOrEmpty(imagePath))
            {
                throw new ArgumentNullException("图片地址为空！");
            }

            FileInfo file = new FileInfo(imagePath);
            using (var stream = file.OpenRead())
            {
                byte[] buffer = new byte[file.Length];
                stream.Read(buffer, 0, Convert.ToInt32(file.Length));
                imageBase64Str = Convert.ToBase64String(buffer);
                stream.Close();
            }

            return imageBase64Str;
        }

    }
}
