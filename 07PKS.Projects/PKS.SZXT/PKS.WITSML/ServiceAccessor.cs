using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PKS.WITSML.SeviceReference;
using System.Net;
using System.IO;

namespace PKS.WITSML
{
    public class ServiceAccessor
    {
        const string DEFAULT_URL = "http://10.138.0.142:8010";
        const string DEFAULT_USERNAME = "zljsz";
        const string DEFAULT_PASSWORD = "wiscommon";
        const int DEFAULT_TIMEOUT = 90000;

        private Service service;

        public ServiceAccessor()
            : this(DEFAULT_URL, DEFAULT_USERNAME, DEFAULT_PASSWORD)
        {
        }

        public ServiceAccessor(string url, string userName, string password)
        {
            service = new Service();
            service.Url = url;
            service.Credentials = new NetworkCredential(userName, password);
            service.Timeout = DEFAULT_TIMEOUT;
        }

        public int WMLS_GetFromStore(string typeIn, string queryIn, out string xmlOut, out string suppMsgOut, string optionsIn = "", string capabilitiesIn = "")
        {
            string WMLtypeIn = typeIn.ToString().ToLower();

            return (int)service.WMLS_GetFromStore(WMLtypeIn, queryIn, optionsIn, capabilitiesIn, out xmlOut, out suppMsgOut);
        }

        private void WriteFile(string xmlStr, string type)
        {
            string path = GetUniquePath(type);
            using (var fs = new System.IO.FileStream(path, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.Write(xmlStr);
                    sw.Flush();
                }
            }
            System.Threading.Thread.Sleep(200);
        }
        private string GetUniquePath(string type)
        {
            var dir = new System.IO.DirectoryInfo(@"D:\log");
            var files = dir.GetFiles().Select(f => int.Parse(Path.GetFileNameWithoutExtension(f.Name).Split(new string[] { "_" }, StringSplitOptions.RemoveEmptyEntries)[1]));
            var path = Path.Combine(dir.ToString(), type + "_" + (files.Max(f => f) + 1).ToString() + ".xml");
            return path;
        }


    }
}
