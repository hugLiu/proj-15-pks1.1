using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.SZXT.IService.ExplorationResearchAchievement
{
    public class UIMetadata
    {
        [JsonProperty("iiid")]
        public string IIId { get; set; }
        [JsonProperty("indexeddate")]
        public DateTime? IndexedDate { get; set; }
        [JsonProperty("showtype")]
        public string ShowType { get; set; }
        [JsonProperty("pageid")]
        public string PageId { get; set; }
        [JsonProperty("dataid")]
        public string DataId { get; set; }
        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("author")]
        public string Author { get; set; }
        [JsonProperty("createddate")]
        public DateTime? CreatedDate { get; set; }
        [JsonProperty("system")]
        public string System { get; set; }
        [JsonProperty("resourceType")]
        public string ResourceType { get; set; }
        [JsonProperty("datasource")]
        public object Datasource { get; set; }
        [JsonProperty("pt")]
        public string PT { get; set; }
        [JsonProperty("pc")]
        public string PC { get; set; }
    }
}
