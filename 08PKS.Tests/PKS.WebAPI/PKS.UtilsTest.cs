using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PKS.Utils;

namespace PKS.WebAPI.UnitTest
{
    [TestClass]
    public class ModelJsonTest
    {
        [TestMethod]
        public void MD5_ImplsTest()
        {
            var path = @"..\..\01PKS.Library\PKS.Utils";
            var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
            var md51 = new MD5Cng();
            var md52 = new MD5CryptoServiceProvider();
            foreach (var file in files)
            {
                var content = File.ReadAllBytes(file);
                var md5Value1 = md51.ComputeHash(content).ToHexString();
                var md5Value2 = md51.ComputeHash(content).ToHexString();
                Assert.AreEqual(md5Value1, md5Value2);
            }
        }
    }
}
