using System.Collections.Generic;

namespace PKS.SZXT.Core.DTO
{
    public class HistogramSetting
    {
        public string Title { get; set; }
        public bool Smooth { get; set; }
        public string DefaultChart { get; set; }
        public List<string> Chart { get; set; }
        public List<string> Legend { get; set; }
        public string YAxisCaption { get; set; }
        public string XAxisField { get; set; }

    }
}
