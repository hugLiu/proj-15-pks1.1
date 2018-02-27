using Jurassic.AppCenter.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
namespace Jurassic.AppCenter.AppServices
{
    [ServiceContract]
    public interface IUpdateService
    {
        [OperationContract]
        Stream Download(string updateDir, string fileToDown);

        [OperationContract]
        List<NetFileInfo> GetFilesInfo(string updateDir);
    }
}
