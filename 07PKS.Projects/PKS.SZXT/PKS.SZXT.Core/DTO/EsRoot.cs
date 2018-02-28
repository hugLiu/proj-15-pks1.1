namespace PKS.SZXT.Core.Model.EsRawResult
{
    public class EsRoot
    {
        public double? took { get; set; }
        public bool timed_out { get; set; }
        public object _shards { get; set; }
        public EsHit hits { get; set; }
    }
}
