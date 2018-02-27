using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Schedule
{
    public class EventModel
    {
        public int caId { get; set; }
        public string start { get; set; }
        public string end { get; set; }


        public string title { get; set; }

        public string alertBefore { get; set; }

        public string alertTime { get; set; }

        public bool editable { get; set; }

        public bool finished { get; set; }

        public string url { get; set; }

        public bool read { get; set; }
    }
}
