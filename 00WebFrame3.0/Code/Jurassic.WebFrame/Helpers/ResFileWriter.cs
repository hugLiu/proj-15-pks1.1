using Jurassic.Com.Tools;
using Jurassic.CommonModels;
using Ninject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Jurassic.AppCenter.Resources;
using System.Collections;

namespace Jurassic.WebFrame
{
    /// <summary>
    /// 将所有程序集中打包的资源如*.cshtml,*.css,*.js,*.png.*.jpg等文件拷贝到网站运行目录下
    /// 同时在网站结束时删除这些文件。
    /// </summary>
    public class ResFileWriter
    {
        /// <summary>
        /// 清除从打包的资源生成的文件,但是在文件后期被修改后不再删除
        /// </summary>
        public void ClearResFiles()
        {
            FileInfo logfi = new FileInfo(logFile);
            //删除以前写过的文件，凡是比日志文件更旧的文件都被删除。
            if (logfi.Exists)
            {
                DateTime logTime = logfi.LastWriteTime;
                string[] fileNameStrs = File.ReadAllLines(logFile);
                DeleteFiles(fileNameStrs, logTime);
            }
            else
            {
                // ClearResFilesAll();
            }
        }

        private void DeleteFiles(IEnumerable<string> fileNameStrs, DateTime logTime)
        {
            foreach (string fn in fileNameStrs)
            {
                FileInfo fi = new FileInfo(fn);
                if (fi.Exists && fi.LastWriteTime <= logTime)
                {
                    fi.Delete();
                }
            }
            foreach (string fn in fileNameStrs)
            {
                DirectoryInfo di = new FileInfo(fn).Directory;
                if (!di.Exists) continue;
               // TestEmpty(di);
            }
        }

        private void ClearResFilesAll()
        {
            List<string> phyicialFiles = new List<string>();
            foreach (var ns in MvcApplication.Assemblys)
            {
                var reader = ResHelper.GetGResourcesReader(ns);
                if (reader == null) continue;
                reader.Each(kv =>
                {
                    DictionaryEntry d = (DictionaryEntry)kv;
                    if (forbiddenTypes.Any(ft => d.Key.ToString().EndsWith(ft, StringComparison.OrdinalIgnoreCase)))
                    {
                        return;
                    }

                    string path = Path.Combine(rootPath, d.Key.ToString());
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                    {
                        phyicialFiles.Add(fi.FullName);
                    }
                });

                reader.Close();
            }

            DeleteFiles(phyicialFiles, DateTime.Now.AddDays(1));
        }

        private bool TestEmpty(DirectoryInfo di)
        {
            if (!di.Exists) return true;
            foreach (var d in di.GetDirectories())
            {
                if (!TestEmpty(d))
                {
                    return false;
                }
            }
            if (di.GetFileSystemInfos().Length == 0)
            {
                var pDi = di.Parent;
                di.Delete();
                while (pDi != null)
                {
                    if (TestEmpty(pDi))
                    {
                        pDi = pDi.Parent;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        private static string rootPath = System.Web.Hosting.HostingEnvironment.MapPath("~");
        private static string logFile = Path.Combine(rootPath ?? "", "App_Data", "ResViews.txt");
        //private static string[] fileTypes = { ".cshtml", ".jpg", ".png", ".gif", ".ico", ".js", ".css","*.xml", "*.html", "*.htm", "*. };
        private static string[] forbiddenTypes = { ".resx" };

        /// <summary>
        /// 将所有引用的程序集中的资源写回物理文件
        /// </summary>
        public void WriteResFiles()
        {
            if (rootPath.IsEmpty())
            {
                return;
            }
            ClearResFiles();

            List<string> phyicialFiles = new List<string>();
            foreach (var ns in MvcApplication.Assemblys)
            {
                var reader = ResHelper.GetGResourcesReader(ns);
                if (reader == null) continue;
                reader.Each(kv =>
                {
                    DictionaryEntry d = (DictionaryEntry)kv;
                    if (forbiddenTypes.Any(ft => d.Key.ToString().EndsWith(ft, StringComparison.OrdinalIgnoreCase)))
                    {
                        return;
                    }

                    string path = Path.Combine(rootPath, d.Key.ToString());
                    FileInfo fi = new FileInfo(path);
                    if (!fi.Exists)
                    {
                        if (!fi.Directory.Exists)
                        {
                            fi.Directory.Create();
                        }
                        Stream stream = d.Value as Stream;
                        IOHelper.WriteStreamToFile(stream, path);
                        phyicialFiles.Add(path);
                    }
                });

                reader.Close();
            }
            //将写过的文件路径名记入一个文本文件
            File.WriteAllLines(logFile, phyicialFiles.ToArray());
            FileInfo logInfo = new FileInfo(logFile);
        }
    }
}