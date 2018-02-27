using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.MongoDb
{
    public class TagManagerModel
    {
        public string Id { get; set; }
        public string IIid { get; set; }
        public string TaskLogInfoId { get; set; }
        public string AdapterId { get; set; }
        public string AdapterName { get; set; }
        public string DataType { get; set; }
        public string NatureKey { get; set; }
        public string StartDate { get; set; }
        public string Contributor { get; set; }
        public string PRJ { get; set; }
        public string PT { get; set; }
        public string BO { get; set; }
        public string BP { get; set; }
        public string BF { get; set; }
	    public string Subject { get; set; }
	    public string Title { get; set; }
        public string Date { get; set; }
        public string Language { get; set; }
        public string Format { get; set; }
        public string Description { get; set; }
        public double CompletelyRate { get; set; }
        public int OperationState { get; set; }

		//Walt add--20151208
	    public string BD { get; set; }
	    public string GN { get; set; }


		//Walt add--20160630
		//添加BA BT DS TL属性维护
		public string BA { get; set; }
		public string BT { get; set; }
		public string DS { get; set; }
		public string TL { get; set; }

		//总数据量
		public long total { get; set; }
    }

	public class TagManagerPageModel
	{
		public long total { get; set; }
		public List<TagManagerModel> data { get; set; }
	}
}
