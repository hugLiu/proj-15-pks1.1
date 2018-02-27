/*
 *主要是保存在缓存库 主库的InfoItem模型
 *区别于InfoItem模型  保持Coverage2中的Spatial为string类型
 *walt add
 *2016-08-16
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jurassic.MongoDb
{
    public class InfoItem2
    {
        public InfoItem2()
        {
            this.IndexedDate = default(DateTime);
            this.Usage = new Usage();
            this.Biblio = new Biblio();
            this.Context = new Context();
			this.Description = new List<Description>();
            this.Coverage = new Coverage2();
            this.UMD = new List<UMD>();
            this.CR = new List<CR>();
            this.TB = new List<string>();
            this.FT = new List<string>();
        }

        //信息项唯一标识
        public string IIid { get; set; }

        //被索引时间
        public DateTime IndexedDate { get; set; }

        //用法
        public Usage Usage { get; set; }

        //文献
        public Biblio Biblio { get; set; }

        //上下文
        public Context Context { get; set; }

        //描述
		public List<Description> Description { get; set; }

        //覆盖
        public Coverage2 Coverage { get; set; }

        //用户定义元数据
        public List<UMD> UMD { get; set; }

        //交叉引用       
        public List<CR> CR { get; set; }

        //缩略图
        public List<string> TB { get; set; }

        //全文
        public List<string> FT { get; set; }
    }
}
