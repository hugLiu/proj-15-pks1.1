using System.Collections.Concurrent;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;
using Microsoft.Practices.CompositeUI;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;
using PKS.WebAPI.Services;

namespace PKS.SubmissionTool.Index
{
    /// <summary>提交上下文</summary>
    public class IndexSubmissionContext
    {
        /// <summary>视图</summary>
        public IIndexSubmissionForm View { get; set; }
        /// <summary>工作项</summary>
        public WorkItem WorkItem { get; set; }
        /// <summary>显示进度消息</summary>
        public ISplashService Splash { get; set; }
        /// <summary>进度事件</summary>
        public event ProgressChangedEventHandler ProgressChanged;
        /// <summary>进度属性</summary>
        public ProgressProperty Progress { get; set; }
        /// <summary>进度值</summary>
        public volatile int ProgressValue;
        /// <summary>重置进度</summary>
        public void ResetProgress()
        {
            this.ProgressValue = -1;
            this.Progress = new ProgressProperty();
            this.Progress.Reset();
        }
        /// <summary>开始进度</summary>
        public void StartProgress(string message, int total)
        {
            this.ProgressValue = -1;
            this.Progress.Message = message;
            this.Progress.MaxValue = total;
            ReportProgress();
            Interlocked.Increment(ref ProgressValue);
            this.Progress.Message = string.Empty;
        }
        /// <summary>下一个进度值</summary>
        public void NextProgress()
        {
            Interlocked.Increment(ref ProgressValue);
            ReportProgress();
        }
        /// <summary>完成进度</summary>
        public void FinishProgress(string message)
        {
            this.ProgressValue = -2;
            this.Progress.Message = message;
            ReportProgress();
        }
        /// <summary>报告进度</summary>
        public void ReportProgress()
        {
            var message = this.Progress.Message;
            if (!message.IsNullOrEmpty()) this.Splash.Show(message);
            var eventSource = this.ProgressChanged;
            if (eventSource != null) eventSource(this, new ProgressChangedEventArgs(this.ProgressValue, this.Progress.Clone()));
        }
        /// <summary>提交集合</summary>
        public ConcurrentQueue<SubmissionProduct> Values { get; set; }
        /// <summary>失败集合</summary>
        public ConcurrentQueue<SubmissionProduct> FailureValues { get; set; }
        /// <summary>应用数据提交服务</summary>
        public IAppDataServiceWrapper AppDataService { get; set; }
        /// <summary>索引数据提交服务</summary>
        public IIndexerService IndexerService { get; set; }
        /// <summary>移动</summary>
        public void Move()
        {
            while (this.Values.Count > 0)
            {
                var value = default(SubmissionProduct);
                if (!this.Values.TryDequeue(out value)) continue;
            }
            while (this.FailureValues.Count > 0)
            {
                var value = default(SubmissionProduct);
                if (!this.FailureValues.TryDequeue(out value)) continue;
                this.Values.Enqueue(value);
            }
        }
    }

    /// <summary>提交成果</summary>
    public class SubmissionProduct
    {
        /// <summary>ID</summary>
        public int ID { get; set; }
        /// <summary>编码</summary>
        public string CharSet { get; set; }
        /// <summary>获得编码</summary>
        public Encoding GetEncoding()
        {
            if (this.CharSet.IsNullOrEmpty()) return Encoding.UTF8;
            return Encoding.GetEncoding(this.CharSet);
        }
        /// <summary>文件</summary>
        public string File { get; set; }
        /// <summary>扩展名</summary>
        public string Ext { get; set; }
        /// <summary>选项</summary>
        public string Options { get; set; }
        /// <summary>绑定列表项</summary>
        public ListViewItem ListViewItem { get; set; }
        /// <summary>索引数据</summary>
        public Metadata IndexData { get; set; }
        /// <summary>应用数据</summary>
        public AppDataSaveRequest AppData { get; set; }
    }
}
