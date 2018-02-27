using Jurassic.Com.Tools;
using Jurassic.CommonModels.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.FileRepository
{
    /// <summary>
    /// 文件类型帮助类
    /// </summary>
    public class FileExtensionTypeHelper
    {
        /// <summary>
        /// 获取文件类型
        /// </summary>
        /// <param name="stream">文件流</param>
        /// <returns></returns>
        public static FileExtensionType GetFileExtensionType(Stream stream)
        {
            
            stream.Position = 0;
            BinaryReader reader = new BinaryReader(stream);
            
            string fileclass = "";
            try
            {

                for (int i = 0; i < 2; i++)
                {
                    fileclass += reader.ReadByte().ToString();
                }

            }
            catch (Exception)
            {
                stream.Position = 0;
                return FileExtensionType.NONE;
            }

            stream.Position = 0;

            if (Enum.IsDefined(typeof(FileExtensionType), int.Parse(fileclass)))
            {
                int d = (int)Enum.Parse(typeof(FileExtensionType), fileclass);
                return (FileExtensionType)d;
            }
            else
            {
                return FileExtensionType.NONE;
            }
            
        }
        /// <summary>
        /// 获得文件缩略图流
        /// </summary>
        /// <param name="id">文件信息ID</param>
        /// <param name="thumbnailSize">缩略图大小</param>
        /// <returns>缩略图流</returns>
        public static Stream GetThumbnail(Stream stream, Size thumbnailSize)
        {
            Stream targetStream = null;
            
            if (stream == null)
            {
                return null;
            }
            Image originImg = null;
            var d = FileExtensionTypeHelper.GetFileExtensionType(stream);
            if (d == FileExtensionType.BMP || d == FileExtensionType.GIF || d == FileExtensionType.JPG || d == FileExtensionType.PNG)
            {
                originImg = Image.FromStream(stream);
            }
            else if (d == FileExtensionType.PDF)
            {
                originImg = Resource.pdf;
            }
            else if (d == FileExtensionType.XLS || d == FileExtensionType.XLSX)
            {
                originImg = Resource.excel;
            }
            else if (d == FileExtensionType.DOC || d == FileExtensionType.DOCX)
            {
                originImg = Resource.word;
            }
            else
            {
                originImg = Resource.unknown;
            }
            targetStream = DrawingHelper.GetThumbnailStream(originImg, thumbnailSize.Width, thumbnailSize.Height);
            return targetStream;
        }


    }
    public class ThumbnailSize
    {
        /// <summary>
        /// 大
        /// </summary>
        public static Size Large
        {
            get
            {
                return new Size(128, 128);
            }
        }
        /// <summary>
        /// 大中
        /// </summary>
        public static Size LargeMiddle
        {
            get
            {
                return new Size(96, 96);
            }
        }
        /// <summary>
        /// 中
        /// </summary>
        public static Size Middle
        {
            get
            {
                return new Size(64, 64);
            }
        }
        /// <summary>
        /// 中小
        /// </summary>
        public static Size MiddleSmall
        {
            get
            {
                return new Size(48, 48);
            }
        }
        /// <summary>
        /// 小
        /// </summary>
        public static Size Small
        {
            get
            {
                return new Size(32, 32);
            }
        }
    }
    public enum FileExtensionType
    {
        NONE = 0,
        JPG = 255216,
        GIF = 7173,
        BMP = 6677,
        PNG = 13780,
        COM = 7790,
        EXE = 7790,
        DLL = 7790,
        RAR = 8297,
        ZIP = 8075,
        XML = 6063,
        HTML = 6033,
        ASPX = 239187,
        CS = 117115,
        JS = 119105,
        TXT = 210187,
        SQL = 255254,
        BAT = 64101,
        BTSEED = 10056,
        RDP = 255254,
        PSD = 5666,
        PDF = 3780,
        CHM = 7384,
        LOG = 70105,
        REG = 8269,
        HLP = 6395,
        DOC = 208207,
        XLS = 8075,
        DOCX = 208207,
        XLSX = 8075,
    }

}


/// <summary>  
/// 图像文件的类型  
/// </summary>  
//public enum ImageType
//{
//    None = 0,
//    BMP = 0x4D42,
//    JPG = 0xD8FF,
//    GIF = 0x4947,
//    PCX = 0x050A,
//    PNG = 0x5089,
//    PSD = 0x4238,
//    RAS = 0xA659,
//    SGI = 0xDA01,
//    TIFF = 0x4949
//}

//public FileExtensionType GetFileType(string key)
//{
//    FileExtensionType ret = FileExtensionType.NONE;
//    FileInfo dd = new FileInfo("");

//    //string path = @"D:\Sheet1.doc";
//    //System.IO.FileStream fs = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
//    System.IO.Stream fs = innerGetFile(key);
//    System.IO.BinaryReader r = new System.IO.BinaryReader(fs);
//    string bx = "";
//    byte buffer;
//    try
//    {
//        buffer = r.ReadByte();
//        bx = buffer.ToString();
//        buffer = r.ReadByte();
//        bx += buffer.ToString();
//    }
//    catch (Exception exc)
//    {
//        Console.WriteLine(exc.Message);
//    }
//    r.Close();
//    fs.Close();

//    KeyValuePair<string, string> findedType = FilesTypeDict.First(e => e.Value == "");


//    return ret;
//}
//private static Dictionary<string, string> FilesTypeDict = new Dictionary<string, string>();

//private static Dictionary<string, byte[]> ImageHeader = new Dictionary<string, byte[]>();
//private static Dictionary<string, object> FilesHeader = new Dictionary<string, object>();
//private static void FileTypeInit() 
//{ 
//    ImageHeader.Add("gif", new byte[] { 71, 73, 70, 56, 57, 97 }); 
//    ImageHeader.Add("bmp", new byte[] { 66, 77 }); 
//    ImageHeader.Add("jpg", new byte[] { 255, 216, 255 }); 
//    ImageHeader.Add("png", new byte[] { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 }); 
//    FilesHeader.Add("pdf", new byte[] { 37, 80, 68, 70, 45, 49, 46, 53 }); 
//    FilesHeader.Add("docx", new object[] { new byte[] { 80, 75, 3, 4, 20, 0, 6, 0, 8, 0, 0, 0, 33 }, new Regex(@"word/_rels/document\.xml\.rels", RegexOptions.IgnoreCase) }); 
//    FilesHeader.Add("xlsx", new object[] { new byte[] { 80, 75, 3, 4, 20, 0, 6, 0, 8, 0, 0, 0, 33 }, new Regex(@"xl/_rels/workbook\.xml\.rels", RegexOptions.IgnoreCase) }); 
//    FilesHeader.Add("pptx", new object[] { new byte[] { 80, 75, 3, 4, 20, 0, 6, 0, 8, 0, 0, 0, 33 }, new Regex(@"ppt/_rels/presentation\.xml\.rels", RegexOptions.IgnoreCase) }); 
//    FilesHeader.Add("doc", new object[] { new byte[] { 208, 207, 17, 224, 161, 177, 26, 225 }, new Regex(@"microsoft( office)? word(?![\s\S]*?microsoft)", RegexOptions.IgnoreCase) }); 
//    FilesHeader.Add("xls", new object[] { new byte[] { 208, 207, 17, 224, 161, 177, 26, 225 }, new Regex(@"microsoft( office)? excel(?![\s\S]*?microsoft)", RegexOptions.IgnoreCase) }); 
//    FilesHeader.Add("ppt", new object[] { new byte[] { 208, 207, 17, 224, 161, 177, 26, 225 }, new Regex(@"c.u.r.r.e.n.t. .u.s.e.r(?![\s\S]*?[a-z])", RegexOptions.IgnoreCase) }); 
//    FilesHeader.Add("avi", new byte[] { 65, 86, 73, 32 }); 
//    FilesHeader.Add("mpg", new byte[] { 0, 0, 1, 0xBA }); 
//    FilesHeader.Add("mpeg", new byte[] { 0, 0, 1, 0xB3 }); 
//    FilesHeader.Add("rar", new byte[] { 82, 97, 114, 33, 26, 7 }); 
//    FilesHeader.Add("zip", new byte[] { 80, 75, 3, 4 }); }

