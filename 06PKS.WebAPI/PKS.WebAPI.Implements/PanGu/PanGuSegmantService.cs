using PanGu;
using PanGu.Match;
using System.Collections.Generic;
using System.Linq;

namespace PKS.WebAPI.PanGu
{
    /// <summary>
    /// 盘古分词
    /// </summary>
    public class PanGuSegmantService
    {
        private readonly Segment _segment;
        public PanGuSegmantService()
        {
            _segment = new Segment();
        }

        public List<WordInfo> DoSegment(string sentence)
        {
            return _segment.DoSegment(sentence).ToList();

        }

        public List<WordInfo> DoSegment(string sentence, MatchOptions options)
        {
            return _segment.DoSegment(sentence, options).ToList();
        }

        public List<WordInfo> DoSegment(string sentence, MatchOptions options, MatchParameter parameters)
        {
            return _segment.DoSegment(sentence, options, parameters).ToList();
        }
    }
}
