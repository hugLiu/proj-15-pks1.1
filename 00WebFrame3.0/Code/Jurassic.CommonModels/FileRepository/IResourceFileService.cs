using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.FileRepository
{
    public interface IResourceFileService
    {
        ResourceFileInfo SaveFile(ResourceFileInfo info);

        ResourceFileInfo GetFile(int fileId);

        ResourceFileInfo GetFile(long size, string fileMD5);

        ResourceFileInfo DeleteFile(int fileId);

        Stream GetFileStream(int fileId);

        Stream GetFileThumbnail(int fileId);

        Stream GetFileThumbnail(int fileId, Size thumbnailSize);


        ResourceCatalogInfo GetCatalogOfFile(int fileId);

        List<ResourceFileInfo> GetFilesInCatalog(int catalogId);


        //ResourceCatalogInfo GetRootCatalog();
        //ResourceCatalogInfo GetCatalog(int catalogId);

        //List<ResourceCatalogInfo> GetChildrenCatalogs(int catalogId);

        //ResourceCatalogInfo SaveCatalog(ResourceCatalogInfo catalog);

        //ResourceCatalogInfo UpdateCatalog(ResourceCatalogInfo catalog);

        //ResourceFileInfo UpdateCatalogOfFile(int fileId, int catalogId);
        //bool DeleteCatalog(int catalogId);


        //查询

    }
}

