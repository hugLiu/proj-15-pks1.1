using System.ComponentModel;

namespace PKS.SubmissionTool.Index
{
    /// <summary>索引提交窗体接口</summary>
    public interface IIndexSubmissionForm
    {
        /// <summary>处理进度事件</summary>
        void HandleProgressChanged(object sender, ProgressChangedEventArgs e);
        /// <summary>填充成果</summary>
        void PopuldateProducts(SubmissionProduct[] products);
        /// <summary>刷新成果状态</summary>
        void RefreshProductStatus(SubmissionProduct product, string status, string error);
    }
}
