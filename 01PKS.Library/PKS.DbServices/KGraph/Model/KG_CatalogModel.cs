using Newtonsoft.Json;
using PKS.DbModels;
using PKS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PKS.DbServices.Models
{
    /// <summary>图谱模型</summary>
    [Serializable]
    public class KG_CatalogModel : KG_CatalogNode, ITreeModel
    {
        /// <summary>父节点</summary>
        [JsonIgnore]
        public ITreeModel Parent { get; set; }
        /// <summary>子节点集合</summary>
        public List<ITreeModel> Children { get; set; }
    }
}
