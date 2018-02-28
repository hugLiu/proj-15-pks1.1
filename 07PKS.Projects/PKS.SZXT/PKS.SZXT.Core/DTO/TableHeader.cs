namespace PKS.SZXT.Core.DTO
{
    public class TableHeader
    {
        public string Field { get; set; }
        public TableHeaderAlign Align { get; set; }
        public int RowSpan { get; set; }
        public int ColSpan { get; set; }
        public string Title { get; set; }
    }
}
