using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface;
using Microsoft.Practices.CompositeUI.SmartParts;
using Microsoft.Practices.ObjectBuilder;
using PKS.Utils;

namespace PKS.SubmissionTool.Index
{
    /// <summary>索引提交窗体</summary>
    [SmartPart]
    public partial class frmIndexSubmission : Form, IIndexSubmissionForm
    {
        #region 数据成员
        /// <summary>构造函数</summary>
        public frmIndexSubmission()
        {
            InitializeComponent();
        }
        /// <summary>Presenter</summary>
        private frmIndexSubmissionPresenter _presenter;
        /// <summary>
        /// Sets the presenter. The dependency injection system will automatically
        /// create a new presenter for you.
        /// </summary>
        [CreateNew]
        public frmIndexSubmissionPresenter Presenter
        {
            private get { return _presenter; }
            set
            {
                _presenter = value;
                _presenter.View = this;
            }
        }
        #endregion

        #region 配置方法
        /// <summary>窗体载入</summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Presenter.OnViewReady();
            this.lvwExcelTemplateVariables.SelectedIndexChanged += lvwExcelTemplateVariables_SelectedIndexChanged;
            this.lvwExcelTemplate.SelectedIndexChanged += lvwExcelTemplate_SelectedIndexChanged;
            this.lvwBatchSubmit.SelectedIndexChanged += lvwBatchSubmit_SelectedIndexChanged;
            this.Presenter.BeginExecuteAsync("加载配置", this.Presenter.LoadAsync, EndLoad);
        }
        /// <summary>结束载入</summary>
        private void EndLoad(bool success)
        {
            var config = this.Presenter.Config;
            if (config == null) return;
            this.txtUserName.Text = config.ApiService.UserName;
            this.txtPasswrod.Text = config.ApiService.Password;
            this.txtApiServiceUrl.Text = config.ApiService.Url;
            this.txtProductFolder.Text = config.Product.Folder;
            this.txtExcelFile.Text = config.Product.ExcelFile;
            this.txtConfigFile.Text = this.Presenter.Service.ConfigFile;
            PopulateVariables(config.Variables);
            PopulateMetadataTags(config.MetadataTags);
        }
        /// <summary>显示操作说明</summary>
        private void btnShowOperationDesc_Click(object sender, EventArgs e)
        {
            new frmShowOperationDesc().ShowDialog(this.Owner);
        }
        /// <summary>加载API配置</summary>
        private void btnLoadFromApi_Click(object sender, EventArgs e)
        {
            if (!ValidateApiServiceConfig()) return;
            this.Presenter.Service.Save();
            this.Presenter.BeginExecuteAsync("从API服务加载", this.Presenter.Service.LoadFromApiService, EndLoadFromApiService);
        }
        /// <summary>结束载入</summary>
        private void EndLoadFromApiService(bool success)
        {
            var config = this.Presenter.Config;
            PopulateMetadataTags(config.MetadataTags);
            if (success) this.Presenter.WorkItem.Alert("从API服务加载成功!");
        }
        /// <summary>验证API配置</summary>
        private bool ValidateApiServiceConfig()
        {
            var config = new XmlApiServiceConfig();
            if (!this.Presenter.Validate(this.txtUserName, ref config.UserName, "请输入用户名称!")) return false;
            if (!this.Presenter.Validate(this.txtPasswrod, ref config.Password, "请输入用户密码!")) return false;
            if (!this.Presenter.Validate(this.txtApiServiceUrl, ref config.Url, "请输入WebAPI站点URL!")) return false;
            this.Presenter.Config.ApiService = config;
            this.Presenter.Service.RefreshVariables_ApiService();
            RefreshVariables(SubmissionConsts.UserName, SubmissionConsts.WebApiUrl);
            return true;
        }
        /// <summary>浏览成果文件夹</summary>
        private void btnBrowseProductFolder_Click(object sender, EventArgs e)
        {
            if (!this.Presenter.BrowseFolder(this.txtProductFolder)) return;
            this.Presenter.Config.Product.Folder = this.txtProductFolder.Text;
            this.Presenter.Service.RefreshVariables_Product(SubmissionConsts.ProductFolder);
            this.Presenter.Service.Save();
            RefreshVariables(SubmissionConsts.ProductFolder);
        }
        /// <summary>打开成果文件夹</summary>
        private void btnOpenProductFolder_Click(object sender, EventArgs e)
        {
            this.txtProductFolder.OpenFolder();
        }
        /// <summary>浏览Excel文件</summary>
        private void btnBrowseExcelFile_Click(object sender, EventArgs e)
        {
            if (!this.Presenter.BrowseFile(this.txtExcelFile)) return;
            this.Presenter.Config.Product.ExcelFile = this.txtExcelFile.Text;
            this.Presenter.Service.RefreshVariables_Product(SubmissionConsts.ExcelFileName);
            this.Presenter.Service.Save();
            RefreshVariables(SubmissionConsts.ExcelFileName);
        }
        /// <summary>打开Excel文件所在的位置</summary>
        private void btnOpenExcelFileBrowser_Click(object sender, EventArgs e)
        {
            this.txtExcelFile.OpenFileBrowser();
        }
        /// <summary>打开Excel文件</summary>
        private void btnOpenExcelFile_Click(object sender, EventArgs e)
        {
            ModuleBootstrapper.TryExecute(this.txtExcelFile.ShellOpenFile);
        }
        /// <summary>浏览配置文件</summary>
        private async void btnBrowseConfigFile_Click(object sender, EventArgs e)
        {
            if (!this.Presenter.BrowseFile(this.txtConfigFile)) return;
            await this.Presenter.LoadAsync(this.txtConfigFile.Text);
            EndLoad(true);
            this.Presenter.WorkItem.Alert("配置加载成功!");
        }
        /// <summary>打开配置文件所在的位置</summary>
        private void btnOpenConfigFileBrowser_Click(object sender, EventArgs e)
        {
            this.txtConfigFile.OpenFileBrowser();
        }
        /// <summary>打开配置文件</summary>
        private void btnOpenConfigFile_Click(object sender, EventArgs e)
        {
            this.txtConfigFile.ShellOpenFile();
        }
        /// <summary>保存配置</summary>
        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            if (!ValidateConfig()) return;
            this.Presenter.WorkItem.Alert("配置保存成功!");
        }
        /// <summary>验证配置</summary>
        private bool ValidateConfig(bool checkExcelFile = false)
        {
            if (!ValidateApiServiceConfig()) return false;
            var product = new XmlProductConfig();
            if (!this.Presenter.Validate(this.txtProductFolder, ref product.Folder, "请选择成果文件夹!")) return false;
            if (!Directory.Exists(product.Folder))
            {
                this.Presenter.WorkItem.Alert($"成果文件夹[{product.Folder}]不存在！");
                this.txtProductFolder.Focus();
                return false;
            }
            if (!this.Presenter.Validate(this.txtExcelFile, ref product.ExcelFile, "请选择Excel文件!")) return false;
            if (checkExcelFile && !File.Exists(product.ExcelFile))
            {
                this.Presenter.WorkItem.Alert($"要提交的Excel文件[{product.ExcelFile}]不存在！");
                this.txtExcelFile.Focus();
                return false;
            }
            this.Presenter.Config.Product = product;
            this.Presenter.Service.RefreshVariables_Product(SubmissionConsts.ProductFolder, SubmissionConsts.ExcelFileName);
            RefreshVariables(SubmissionConsts.ProductFolder, SubmissionConsts.ExcelFileName);
            this.Presenter.Service.Save();
            return true;
        }
        /// <summary>重载配置</summary>
        private async void btnReloadConfig_Click(object sender, EventArgs e)
        {
            await this.Presenter.LoadAsync(null);
            EndLoad(true);
            this.Presenter.WorkItem.Alert("配置重载成功!");
        }
        #endregion

        #region 模板变量方法
        /// <summary>填充变量</summary>
        private void PopulateVariables(List<XmlVariable> variables)
        {
            this.lvwExcelTemplateVariables.BeginUpdate();
            this.lvwExcelTemplateVariables.Items.Clear();
            variables.ForEach(AddVariable);
            this.lvwExcelTemplateVariables.EndUpdate();
        }
        /// <summary>加入一个变量</summary>
        private void AddVariable(XmlVariable variable)
        {
            var lvwItem = this.lvwExcelTemplateVariables.Items.Add(variable.Name, variable.Name, string.Empty);
            lvwItem.SubItems.Add(variable.Title);
            lvwItem.SubItems.Add(variable.BuildIn ? "是" : "否");
            lvwItem.SubItems.Add(variable.Value);
            lvwItem.Tag = variable;
        }
        /// <summary>选择的变量</summary>
        private ListViewItem SelectedVariable { get; set; }
        /// <summary>当选择某个变量时</summary>
        private void lvwExcelTemplateVariables_SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = this.lvwExcelTemplateVariables.SelectedItems;
            if (items.Count == 0) return;
            this.SelectedVariable = items[0];
            var variable = this.SelectedVariable.Tag.As<XmlVariable>();
            this.txtVariableName.Text = variable.Name;
            this.txtVariableName.ReadOnly = variable.BuildIn;
            this.txtVariableTitle.Text = variable.Title;
            this.txtVariableValue.Text = variable.Value;
            this.txtVariableValue.ReadOnly = (variable.Provider != null);
        }
        /// <summary>更新变量</summary>
        private void btnUpdateVariable_Click(object sender, EventArgs e)
        {
            if (this.SelectedVariable == null)
            {
                this.Presenter.WorkItem.Alert("请先选择一个变量!");
                return;
            }
            var oldVariable = this.SelectedVariable.Tag.As<XmlVariable>();
            var newVariable = ValidateVariable(oldVariable);
            if (newVariable == null) return;
            oldVariable.Name = newVariable.Name;
            oldVariable.Title = newVariable.Title;
            oldVariable.Value = newVariable.Value;
            var index = 0;
            this.SelectedVariable.SubItems[index++].Text = newVariable.Name;
            this.SelectedVariable.SubItems[index++].Text = newVariable.Title;
            index++;
            this.SelectedVariable.SubItems[index++].Text = newVariable.Value;
        }
        /// <summary>验证变量</summary>
        private XmlVariable ValidateVariable(XmlVariable oldVariable)
        {
            var newVariable = new XmlVariable();
            if (!this.Presenter.Validate(this.txtVariableName, ref newVariable.Name, "变量名称不允许为空!")) return null;
            if (newVariable.Name.IndexOfAny(new char[] { '{', '}' }) >= 0)
            {
                this.Presenter.WorkItem.Alert("变量名称不允许包含字符'{'和'}'!");
                return null;
            }
            if (oldVariable == null || !newVariable.Name.Equals(oldVariable.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (this.Presenter.Config.Variables.Any(v => v.Name.Equals(newVariable.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    this.Presenter.WorkItem.Alert("变量名称已经存在!");
                    return null;
                }
            }
            if (!this.Presenter.Validate(this.txtVariableTitle, ref newVariable.Title, "变量标题不允许为空!")) return null;
            if (!this.Presenter.Validate(this.txtVariableValue, ref newVariable.Value, "变量值不允许为空!")) return null;
            return newVariable;
        }
        /// <summary>添加变量</summary>
        private void btnAddVariable_Click(object sender, EventArgs e)
        {
            var newVariable = ValidateVariable(null);
            if (newVariable == null) return;
            newVariable.BuildIn = false;
            this.Presenter.Config.Variables.Add(newVariable);
            AddVariable(newVariable);
            //this.Presenter.Service.Save();
        }
        /// <summary>删除变量</summary>
        private void btnDeleteVariable_Click(object sender, EventArgs e)
        {
            var items = this.lvwExcelTemplateVariables.SelectedItems;
            if (items.Count == 0) return;
            var lvwDeletedVariable = items[0];
            var deletedVariable = lvwDeletedVariable.Tag.As<XmlVariable>();
            if (deletedVariable.BuildIn)
            {
                this.Presenter.WorkItem.Alert("内置变量不允许删除!");
                return;
            }
            this.Presenter.Config.Variables.Remove(deletedVariable);
            if (this.SelectedVariable == lvwDeletedVariable) this.SelectedVariable = null;
            this.lvwExcelTemplateVariables.Items.Remove(lvwDeletedVariable);
        }
        /// <summary>刷新变量值</summary>
        private void RefreshVariables(params string[] variableNames)
        {
            this.lvwExcelTemplateVariables.BeginUpdate();
            var variables = this.Presenter.Config.Variables;
            foreach (var variableName in variableNames)
            {
                var lvwItem = this.lvwExcelTemplateVariables.Items.Find(variableName, false).First();
                lvwItem.SubItems[3].Text = variables.First(e => e.Name == variableName).Value;
            }
            this.lvwExcelTemplateVariables.EndUpdate();
        }
        #endregion

        #region Excel模板方法
        /// <summary>填充标签</summary>
        private void PopulateMetadataTags(List<XmlMetadataTag> tags)
        {
            if (tags.IsNullOrEmpty() || tags.First().Refer == null) return;
            this.lvwExcelTemplate.BeginUpdate();
            this.lvwExcelTemplate.Items.Clear();
            foreach (var tag in tags)
            {
                var lvwItem = this.lvwExcelTemplate.Items.Add(tag.Name, tag.Name, string.Empty);
                lvwItem.SubItems.Add(tag.Title);
                lvwItem.SubItems.Add(tag.Enabled ? "是" : "否");
                lvwItem.SubItems.Add(tag.Width.ToString());
                lvwItem.SubItems.Add(tag.DefaultValue);
                lvwItem.SubItems.Add(tag.EnumValues);
                lvwItem.Tag = tag;
            }
            this.lvwExcelTemplate.EndUpdate();
        }
        /// <summary>选择的标签</summary>
        private ListViewItem SelectedTag { get; set; }
        /// <summary>当选择某个标签时</summary>
        private void lvwExcelTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = this.lvwExcelTemplate.SelectedItems;
            if (items.Count == 0) return;
            this.SelectedTag = items[0];
            var tag = this.SelectedTag.Tag.As<XmlMetadataTag>();
            this.txtTagName.Text = tag.Name;
            this.txtTagTitle.Text = tag.Title;
            this.chkTagVisible.Checked = tag.Enabled;
            this.txtTagWidth.Text = tag.Width.ToString();
            this.txtTagDefaultValue.Text = tag.DefaultValue;
            if (tag.EnumValues.Length == 0)
            {
                this.txtTagValues.Clear();
            }
            else
            {
                this.txtTagValues.Lines = tag.GetEnumValues();
            }
        }
        /// <summary>更新标签</summary>
        private void btnUpdateTag_Click(object sender, EventArgs e)
        {
            if (this.SelectedTag == null)
            {
                this.Presenter.WorkItem.Alert("请先选择一个标签!");
                return;
            }
            var oldTag = this.SelectedTag.Tag.As<XmlMetadataTag>();
            var newTag = ValidateTag(oldTag);
            if (newTag == null) return;
            oldTag.Name = newTag.Name;
            //oldTag.Title = newTag.Title;
            oldTag.Enabled = newTag.Enabled;
            oldTag.DefaultValue = newTag.DefaultValue;
            oldTag.EnumValues = newTag.EnumValues;
            newTag.Refer = oldTag.Refer;
            var index = 0;
            this.SelectedTag.SubItems[index++].Text = newTag.Name;
            this.SelectedTag.SubItems[index++].Text = newTag.Title;
            this.SelectedTag.SubItems[index++].Text = newTag.Enabled ? "是" : "否";
            this.SelectedTag.SubItems[index++].Text = newTag.Width.ToString();
            this.SelectedTag.SubItems[index++].Text = newTag.DefaultValue;
            this.SelectedTag.SubItems[index++].Text = newTag.EnumValues;
        }
        /// <summary>验证标签</summary>
        private XmlMetadataTag ValidateTag(XmlMetadataTag oldTag)
        {
            var newTag = new XmlMetadataTag();
            if (!this.Presenter.Validate(this.txtTagName, ref newTag.Name, "标签名称不允许为空!")) return null;
            if (oldTag == null || !newTag.Name.Equals(oldTag.Name, StringComparison.OrdinalIgnoreCase))
            {
                if (this.Presenter.Config.MetadataTags.Any(v => v.Name.Equals(newTag.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    this.Presenter.WorkItem.Alert("标签名称已经存在!");
                    return null;
                }
            }
            newTag.Enabled = this.chkTagVisible.Checked;
            if (!this.Presenter.ValidateInt(this.txtTagWidth, ref newTag.Width, "标签宽度")) return null;
            newTag.DefaultValue = this.txtTagDefaultValue.Text.Trim();
            newTag.EnumValues = string.Join(",", this.txtTagValues.Lines.Select(e => e.Trim()).Distinct().ToArray());
            return newTag;
        }
        /// <summary>上移</summary>
        private void btnTagMoveUp_Click(object sender, EventArgs e)
        {
            var items = this.lvwExcelTemplate.SelectedItems;
            if (items.Count == 0) return;
            var selectedItem = items[0];
            if (selectedItem.Index == 0) return;
            var selectedTag = selectedItem.Tag.As<XmlMetadataTag>();
            var newIndex = selectedItem.Index - 1;
            this.Presenter.Config.MetadataTags.Remove(selectedTag);
            this.Presenter.Config.MetadataTags.Insert(newIndex, selectedTag);
            this.lvwExcelTemplate.Items.Remove(selectedItem);
            this.lvwExcelTemplate.Items.Insert(newIndex, selectedItem);
            selectedItem.EnsureVisible();
        }
        /// <summary>下移</summary>
        private void btnTagMoveDn_Click(object sender, EventArgs e)
        {
            var items = this.lvwExcelTemplate.SelectedItems;
            if (items.Count == 0) return;
            var selectedItem = items[0];
            if (selectedItem.Index == this.lvwExcelTemplate.Items.Count - 1) return;
            var selectedTag = selectedItem.Tag.As<XmlMetadataTag>();
            var newIndex = selectedItem.Index + 1;
            this.Presenter.Config.MetadataTags.Remove(selectedTag);
            this.Presenter.Config.MetadataTags.Insert(newIndex, selectedTag);
            this.lvwExcelTemplate.Items.Remove(selectedItem);
            this.lvwExcelTemplate.Items.Insert(newIndex, selectedItem);
            selectedItem.EnsureVisible();
        }
        /// <summary>生成Excel模板文件</summary>
        private void btnBuildExcelTemplate_Click(object sender, EventArgs e)
        {
            if (!ValidateConfig()) return;
            this.Presenter.BeginExecuteAsync("生成Excel模板文件", this.Presenter.Service.BuildExcelTemplateFile, EndBuildExcelTemplate);
        }
        /// <summary>生成Excel模板文件</summary>
        private void EndBuildExcelTemplate(bool success)
        {
            if (success) this.Presenter.WorkItem.Alert("Excel模板文件生成成功!");
        }
        #endregion

        #region 提交方法
        /// <summary>提交</summary>
        private void btnBatchSubmitIndex_Click(object sender, EventArgs e)
        {
            if (!ValidateConfig(true)) return;
            this.Presenter.IsParallelSubmit = this.chkParallelSubmit.Checked;
            this.Presenter.BeginExecuteAsync("提交索引数据", this.Presenter.BatchSubmit, EndBatchSubmitIndex);
        }
        /// <summary>提交失败重试</summary>
        private void btnRetryBatchSubmit_Click(object sender, EventArgs e)
        {
            if (!this.Presenter.CanRetryBatchSubmit())
            {
                this.Presenter.WorkItem.Alert("没有提交失败的索引数据!");
                return;
            }
            this.Presenter.BeginExecuteAsync("提交上次失败的索引数据", this.Presenter.RetryBatchSubmit, EndBatchSubmitIndex);
        }
        /// <summary>处理进度事件</summary>
        public void HandleProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new ProgressChangedEventHandler(HandleProgressChanged), sender, e);
                return;
            }
            var property = e.UserState.As<ProgressProperty>();
            if (e.ProgressPercentage == -1)
            {
                if (property.MaxValue == -1)
                {
                    this.pbrSubmission.Style = ProgressBarStyle.Marquee;
                    this.pbrSubmission.Maximum = 100;
                    this.pbrSubmission.Value = 0;
                }
                else
                {
                    this.pbrSubmission.Style = ProgressBarStyle.Continuous;
                    this.pbrSubmission.Maximum = property.MaxValue;
                    this.pbrSubmission.Value = 0;
                }
            }
            else if (e.ProgressPercentage > 0)
            {
                this.pbrSubmission.Value = e.ProgressPercentage;
            }
            else
            {
                this.pbrSubmission.Value = 0;
            }
        }
        /// <summary>填充成果</summary>
        public void PopuldateProducts(SubmissionProduct[] products)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<SubmissionProduct[]>(PopuldateProducts), (object)products);
                return;
            }
            this.lvwBatchSubmit.BeginUpdate();
            this.lvwBatchSubmit.Items.Clear();
            foreach (var product in products)
            {
                var name = product.ID.ToString();
                var lvwItem = this.lvwBatchSubmit.Items.Add(name, name, -1);
                lvwItem.SubItems.Add(product.File);
                lvwItem.SubItems.Add("等待提交");
                lvwItem.SubItems.Add(string.Empty);
                product.ListViewItem = lvwItem;
            }
            this.lvwBatchSubmit.EndUpdate();
        }
        /// <summary>刷新产品状态</summary>
        public void RefreshProductStatus(SubmissionProduct product, string status, string error)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<SubmissionProduct, string, string>(RefreshProductStatus), product, status, error);
                return;
            }
            var lvwItem = product.ListViewItem;
            lvwItem.SubItems[2].Text = status;
            lvwItem.SubItems[3].Text = error;
            lvwItem.EnsureVisible();
        }
        /// <summary>选择某个成果时</summary>
        private void lvwBatchSubmit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var items = this.lvwBatchSubmit.SelectedItems;
            if (items.Count == 0) return;
            var lvwItem = items[0];
            this.txtSubmitFailure.Text = lvwItem.SubItems[3].Text;
        }
        /// <summary>提交</summary>
        private void EndBatchSubmitIndex(bool success)
        {
            if (!success) return;
            var message = "索引数据提交完成!" + Environment.NewLine;
            message += this.Presenter.Service.GetSubmissionStat();
            this.Presenter.WorkItem.Alert(message);
        }
        #endregion

        #region 合并方法
        /// <summary>浏览合并源文件</summary>
        private void btnBrowseMergeSourceFile_Click(object sender, EventArgs e)
        {
            this.Presenter.BrowseFile(this.txtMergeSourceFile);
        }
        /// <summary>打开合并源文件所在的位置</summary>
        private void btnOpenMergeSourceFileBrowser_Click(object sender, EventArgs e)
        {
            this.txtMergeSourceFile.OpenFileBrowser();
        }
        /// <summary>打开合并源文件</summary>
        private void btnOpenMergeSourceFile_Click(object sender, EventArgs e)
        {
            this.txtMergeSourceFile.ShellOpenFile();
        }
        /// <summary>合并</summary>
        private void btnMerge_Click(object sender, EventArgs e)
        {
            var context = this.Presenter.MergeContext;
            if (context == null)
            {
                context = this.Presenter.MergeContext = new MergeContext();
            }
            if (!this.Presenter.Validate(this.txtExcelFile, ref context.MergeTargetFile, "请选择Excel文件!")) return;
            if (!File.Exists(context.MergeTargetFile))
            {
                this.Presenter.WorkItem.Alert($"Excel文件[{context.MergeTargetFile}]不存在！");
                this.txtExcelFile.Focus();
                return;
            }
            if (!this.Presenter.Validate(this.txtMergeSourceFile, ref context.MergeSourceFile, "请选择Excel合并源文件!")) return;
            if (!File.Exists(context.MergeSourceFile))
            {
                this.Presenter.WorkItem.Alert($"Excel合并源文件[{context.MergeSourceFile}]不存在！");
                this.txtMergeSourceFile.Focus();
                return;
            }
            if (!this.Presenter.ValidateInt(this.txtMergeSourceFieldRow, ref context.MergeSourceFieldRow, "合并源文件标签行行号")) return;
            if (!this.Presenter.ValidateInt(this.txtMergeSourceDataRow, ref context.MergeSourceDataRow, "合并源文件数据行行号")) return;
            this.Presenter.BeginExecuteAsync("合并Excel文件", this.Presenter.BeginMerge, EndMerge);
        }
        /// <summary>提交</summary>
        private void EndMerge(bool success)
        {
            if (!success) return;
            this.Presenter.WorkItem.Alert("Excel文件合并完成!");
        }
        #endregion
    }
}
