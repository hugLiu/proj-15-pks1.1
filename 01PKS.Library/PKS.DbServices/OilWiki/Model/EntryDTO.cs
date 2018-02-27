using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.DbServices.OilWiki.Model
{
    public class EntryDTO
    {
        public int Id { get; set; }
        public int CatalogId { get; set; }
        public int? ParentCatalogId { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public string Contents { get; set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public string Image { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        public List<string> AliasEntry { get; set; }
        public List<int> RelatedEntry { get; set; }
    }

    public class EntryDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Contents { get; set; }
        public string Source { get; set; }
        public string Image { get; set; }
        public int ParentCatalogId { get; set; }
        public string ParentCatalogName { get; set; }

        //public string RootCatalogName { get; set; }
        public List<RelatedEntry> AliasEntry { get; set; }
        public List<RelatedEntry> RelatedEntry { get; set; }
        public string Author { get;  set; }
    }

    public class RelatedEntry
    {
        public int EntryId { get; set; }
        public string EntryName { get; set; }
    }
}
