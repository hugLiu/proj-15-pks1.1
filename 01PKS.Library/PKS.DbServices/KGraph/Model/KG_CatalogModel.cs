using Newtonsoft.Json;
using PKS.DbModels;
using PKS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PKS.DbServices.Models
{
    /// <summary>ͼ��ģ��</summary>
    [Serializable]
    public class KG_CatalogModel : KG_CatalogNode, ITreeModel
    {
        /// <summary>���ڵ�</summary>
        [JsonIgnore]
        public ITreeModel Parent { get; set; }
        /// <summary>�ӽڵ㼯��</summary>
        public List<ITreeModel> Children { get; set; }
    }
}
