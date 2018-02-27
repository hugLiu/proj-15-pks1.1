using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace PKS.Utils
{
    /// <summary>Hash工具</summary>
    public static class HashUtil
    {
        /// <summary>生成MD5值</summary>
        public static string ToMD5(this string content)
        {
            var buffer = Encoding.UTF8.GetBytes(content);
            return ToMD5(buffer);
        }

        /// <summary>生成MD5</summary>
        public static string ToMD5(this Stream content)
        {
            var buffer = content.ToByteArray();
            return ToMD5(buffer);
        }

        /// <summary>生成MD5</summary>
        public static string ToMD5(this byte[] buffer)
        {
            var md5 = new MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(buffer);
            return hash.ToHexString();
        }
    }
}