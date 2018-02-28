using System.Threading.Tasks;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface;
using PKS.Utils;

namespace PKS.SubmissionTool.Index
{
    /// <summary>索引提交Presenter</summary>
    public class frmIndexSubmissionPresenter : Presenter<IIndexSubmissionForm>
    {
        #region 加载和关闭
        /// <summary>
        /// This method is a placeholder that will be called by the view when it has been loaded.
        /// </summary>
        public override void OnViewReady()
        {
            base.OnViewReady();
            this.Service = ModuleBootstrapper.Get<IndexSubmissionService>();
        }
        /// <summary>
        /// Close the view
        /// </summary>
        public override void OnCloseView()
        {
            base.CloseView();
        }
        #endregion

        #region 数据成员
        /// <summary>服务</summary>
        internal IndexSubmissionService Service { get; set; }
        /// <summary>配置</summary>
        internal IndexSubmissionConfig Config
        {
            get { return this.Service.Config; }
        }
        #endregion

        #region 访问方法
        /// <summary>异步加载</summary>
        public void LoadAsync()
        {
            LoadAsync(null);
        }
        /// <summary>异步加载</summary>
        public Task LoadAsync(string configFile)
        {
            this.Service.Initialize(configFile);
            return Utility.CompletedTask;
        }
        /// <summary>是否批量提交</summary>
        public bool IsParallelSubmit { get; set; }
        /// <summary>批量提交</summary>
        public void BatchSubmit()
        {
            this.Service.BatchSubmit(this, this.IsParallelSubmit, false);
        }
        /// <summary>是否能批量提交失败重试</summary>
        public bool CanRetryBatchSubmit()
        {
            var context = this.Service.Context;
            return context != null && !context.FailureValues.IsEmpty;
        }
        /// <summary>批量提交失败重试</summary>
        public void RetryBatchSubmit()
        {
            this.Service.BatchSubmit(this, this.IsParallelSubmit, true);
        }
        /// <summary>合并上下文</summary>
        public MergeContext MergeContext { get; set; }
        /// <summary>开始合并</summary>
        public void BeginMerge()
        {
            this.Service.Merge(this, this.MergeContext);
        }
        #endregion
    }
}
