using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.MongoDb
{
    public class Biblio : JsonBase
    {
        public Biblio()
        {
            this.Title = new List<Title>();
            this.Contributor = new List<Contributor>();
            this.Organization = new List<string>();
            this.Date = new List<Date>();
            this.Language = string.Empty;
            this.Subject = string.Empty;
        }
        public List<Title> Title { get; private set; }
        public List<Contributor> Contributor { get; private set; }
        public List<string> Organization { get; private set; }
        public List<Date> Date { get; private set; }
        public string Subject { get; set; }
        public string Language { get; set; }
    }
}
