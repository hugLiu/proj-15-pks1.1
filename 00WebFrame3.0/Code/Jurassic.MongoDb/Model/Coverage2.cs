namespace Jurassic.MongoDb
{
    /// <summary>
    /// 用于从数据源到Mongodb的数据转化
    /// </summary>
    public class Coverage2:JsonBase
    {
        public Coverage2()
        {
            this.Region = string.Empty;
            this.Datum = string.Empty;
            this.Scale = string.Empty;
        }
        public string Region { get; set; }
        public string Datum { get; set; }
        public string Scale { get; set; }

        /// <summary>
        /// 如果是SQL SERVER数据库的空间类型SQLGeoMetry,直接将该类型转化为WKT串
        /// </summary>
        public string Spatial { get; set; }
    }
}
