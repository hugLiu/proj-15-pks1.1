using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.CommonModels.Articles;

namespace Jurassic.CommonModels.FileRepository
{
    public class ResourceCatalogInfo
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        public string Name { get; set; }


        /// <summary>
        /// 目录所有者ID
        /// </summary>
        public int OwnerId { get; set; }

        /// <summary>
        /// 目录所有者类型
        /// </summary>
        public CatalogOwnerType OwnerType { get; set; }
    }
}
