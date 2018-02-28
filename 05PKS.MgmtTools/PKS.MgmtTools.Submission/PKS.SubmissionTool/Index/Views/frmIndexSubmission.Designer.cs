namespace PKS.SubmissionTool.Index
{
    partial class frmIndexSubmission
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.tabMain = new System.Windows.Forms.TabControl();
            this.tabExcelTemplateVariables = new System.Windows.Forms.TabPage();
            this.pnlExcelTemplateVariables = new System.Windows.Forms.Panel();
            this.splExcelTemplateVariables = new System.Windows.Forms.SplitContainer();
            this.lvwExcelTemplateVariables = new System.Windows.Forms.ListView();
            this.cnVariableName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cnVariableTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cnVariableBuildin = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cnVariableValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnUpdateVariable = new System.Windows.Forms.Button();
            this.btnDeleteVariable = new System.Windows.Forms.Button();
            this.btnAddVariable = new System.Windows.Forms.Button();
            this.txtVariableValue = new System.Windows.Forms.TextBox();
            this.lblVariableValue = new System.Windows.Forms.Label();
            this.txtVariableTitle = new System.Windows.Forms.TextBox();
            this.lblVariableTitle = new System.Windows.Forms.Label();
            this.txtVariableName = new System.Windows.Forms.TextBox();
            this.lblVariableName = new System.Windows.Forms.Label();
            this.tabBuildExcelTemplate = new System.Windows.Forms.TabPage();
            this.pnlBuildExcelTemplate = new System.Windows.Forms.Panel();
            this.splBuildExcelTemplate = new System.Windows.Forms.SplitContainer();
            this.lvwExcelTemplate = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cnTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cnVisible = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cnWidth = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cnDefaultValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cnValues = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtTagValues = new System.Windows.Forms.TextBox();
            this.lblTagValues = new System.Windows.Forms.Label();
            this.txtTagDefaultValue = new System.Windows.Forms.TextBox();
            this.lblTagDefaultValue = new System.Windows.Forms.Label();
            this.lblTagWidthUnit = new System.Windows.Forms.Label();
            this.btnUpdateTag = new System.Windows.Forms.Button();
            this.chkTagVisible = new System.Windows.Forms.CheckBox();
            this.txtTagWidth = new System.Windows.Forms.TextBox();
            this.lblTagWidth = new System.Windows.Forms.Label();
            this.txtTagTitle = new System.Windows.Forms.TextBox();
            this.lblTagTitle = new System.Windows.Forms.Label();
            this.txtTagName = new System.Windows.Forms.TextBox();
            this.lblTagName = new System.Windows.Forms.Label();
            this.pnlBuildExcelTemplateTop = new System.Windows.Forms.Panel();
            this.btnTagMoveDn = new System.Windows.Forms.Button();
            this.btnTagMoveUp = new System.Windows.Forms.Button();
            this.btnBuildExcelTemplate = new System.Windows.Forms.Button();
            this.tabMerge = new System.Windows.Forms.TabPage();
            this.pnlMerge = new System.Windows.Forms.Panel();
            this.btnMerge = new System.Windows.Forms.Button();
            this.txtMergeSourceDataRow = new System.Windows.Forms.TextBox();
            this.lblMergeSourceDataRow = new System.Windows.Forms.Label();
            this.txtMergeSourceFieldRow = new System.Windows.Forms.TextBox();
            this.lblMergeSourceFieldRow = new System.Windows.Forms.Label();
            this.txtMergeNotice = new System.Windows.Forms.TextBox();
            this.btnOpenMergeSourceFile = new System.Windows.Forms.Button();
            this.btnOpenMergeSourceFileBrowser = new System.Windows.Forms.Button();
            this.btnBrowseSourceMergeFile = new System.Windows.Forms.Button();
            this.txtMergeSourceFile = new System.Windows.Forms.TextBox();
            this.lblMergeSourceFile = new System.Windows.Forms.Label();
            this.tabBatchSubmitIndex = new System.Windows.Forms.TabPage();
            this.pnlBatchSubmitIndex = new System.Windows.Forms.Panel();
            this.splBatchSubmit = new System.Windows.Forms.SplitContainer();
            this.lvwBatchSubmit = new System.Windows.Forms.ListView();
            this.chProductIndex = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chProductFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSubmitStatus = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chSubmitFailureMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtSubmitFailure = new System.Windows.Forms.TextBox();
            this.pnlSubmitFailure = new System.Windows.Forms.Panel();
            this.lblSubmitFailure = new System.Windows.Forms.Label();
            this.pbrSubmission = new System.Windows.Forms.ProgressBar();
            this.pnlSubmitTop = new System.Windows.Forms.Panel();
            this.btnRetryBatchSubmit = new System.Windows.Forms.Button();
            this.btnBatchSubmitIndex = new System.Windows.Forms.Button();
            this.grpProduct = new System.Windows.Forms.GroupBox();
            this.btnOpenProductFolder = new System.Windows.Forms.Button();
            this.btnShowOperationDesc = new System.Windows.Forms.Button();
            this.btnReloadConfig = new System.Windows.Forms.Button();
            this.btnBrowseConfigFile = new System.Windows.Forms.Button();
            this.btnOpenConfigFile = new System.Windows.Forms.Button();
            this.btnOpenExcelFile = new System.Windows.Forms.Button();
            this.btnOpenExcelFileBrowser = new System.Windows.Forms.Button();
            this.btnSaveConfig = new System.Windows.Forms.Button();
            this.btnOpenConfigFileBrowser = new System.Windows.Forms.Button();
            this.txtConfigFile = new System.Windows.Forms.TextBox();
            this.lblConfigFile = new System.Windows.Forms.Label();
            this.btnBrowseExcelFile = new System.Windows.Forms.Button();
            this.txtExcelFile = new System.Windows.Forms.TextBox();
            this.lblExcelFile = new System.Windows.Forms.Label();
            this.btnBrowseProductFolder = new System.Windows.Forms.Button();
            this.txtProductFolder = new System.Windows.Forms.TextBox();
            this.lblProductFolder = new System.Windows.Forms.Label();
            this.grpApiService = new System.Windows.Forms.GroupBox();
            this.btnLoadFromApi = new System.Windows.Forms.Button();
            this.txtApiServiceUrl = new System.Windows.Forms.TextBox();
            this.lblApiServiceUrl = new System.Windows.Forms.Label();
            this.txtPasswrod = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.lblUserName = new System.Windows.Forms.Label();
            this.tagToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.chkParallelSubmit = new System.Windows.Forms.CheckBox();
            this.pnlMain.SuspendLayout();
            this.tabMain.SuspendLayout();
            this.tabExcelTemplateVariables.SuspendLayout();
            this.pnlExcelTemplateVariables.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splExcelTemplateVariables)).BeginInit();
            this.splExcelTemplateVariables.Panel1.SuspendLayout();
            this.splExcelTemplateVariables.Panel2.SuspendLayout();
            this.splExcelTemplateVariables.SuspendLayout();
            this.tabBuildExcelTemplate.SuspendLayout();
            this.pnlBuildExcelTemplate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splBuildExcelTemplate)).BeginInit();
            this.splBuildExcelTemplate.Panel1.SuspendLayout();
            this.splBuildExcelTemplate.Panel2.SuspendLayout();
            this.splBuildExcelTemplate.SuspendLayout();
            this.pnlBuildExcelTemplateTop.SuspendLayout();
            this.tabMerge.SuspendLayout();
            this.pnlMerge.SuspendLayout();
            this.tabBatchSubmitIndex.SuspendLayout();
            this.pnlBatchSubmitIndex.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splBatchSubmit)).BeginInit();
            this.splBatchSubmit.Panel1.SuspendLayout();
            this.splBatchSubmit.Panel2.SuspendLayout();
            this.splBatchSubmit.SuspendLayout();
            this.pnlSubmitFailure.SuspendLayout();
            this.pnlSubmitTop.SuspendLayout();
            this.grpProduct.SuspendLayout();
            this.grpApiService.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.BackColor = System.Drawing.Color.SkyBlue;
            this.pnlMain.Controls.Add(this.tabMain);
            this.pnlMain.Controls.Add(this.grpProduct);
            this.pnlMain.Controls.Add(this.grpApiService);
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(1184, 662);
            this.pnlMain.TabIndex = 1;
            // 
            // tabMain
            // 
            this.tabMain.Controls.Add(this.tabExcelTemplateVariables);
            this.tabMain.Controls.Add(this.tabBuildExcelTemplate);
            this.tabMain.Controls.Add(this.tabMerge);
            this.tabMain.Controls.Add(this.tabBatchSubmitIndex);
            this.tabMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMain.ItemSize = new System.Drawing.Size(200, 25);
            this.tabMain.Location = new System.Drawing.Point(0, 218);
            this.tabMain.Name = "tabMain";
            this.tabMain.SelectedIndex = 0;
            this.tabMain.Size = new System.Drawing.Size(1184, 444);
            this.tabMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabMain.TabIndex = 26;
            // 
            // tabExcelTemplateVariables
            // 
            this.tabExcelTemplateVariables.Controls.Add(this.pnlExcelTemplateVariables);
            this.tabExcelTemplateVariables.Location = new System.Drawing.Point(4, 29);
            this.tabExcelTemplateVariables.Name = "tabExcelTemplateVariables";
            this.tabExcelTemplateVariables.Size = new System.Drawing.Size(1176, 411);
            this.tabExcelTemplateVariables.TabIndex = 2;
            this.tabExcelTemplateVariables.Text = "Excel模板变量";
            this.tabExcelTemplateVariables.UseVisualStyleBackColor = true;
            // 
            // pnlExcelTemplateVariables
            // 
            this.pnlExcelTemplateVariables.BackColor = System.Drawing.Color.SkyBlue;
            this.pnlExcelTemplateVariables.Controls.Add(this.splExcelTemplateVariables);
            this.pnlExcelTemplateVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlExcelTemplateVariables.Location = new System.Drawing.Point(0, 0);
            this.pnlExcelTemplateVariables.Name = "pnlExcelTemplateVariables";
            this.pnlExcelTemplateVariables.Size = new System.Drawing.Size(1176, 411);
            this.pnlExcelTemplateVariables.TabIndex = 3;
            // 
            // splExcelTemplateVariables
            // 
            this.splExcelTemplateVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splExcelTemplateVariables.Location = new System.Drawing.Point(0, 0);
            this.splExcelTemplateVariables.Name = "splExcelTemplateVariables";
            // 
            // splExcelTemplateVariables.Panel1
            // 
            this.splExcelTemplateVariables.Panel1.Controls.Add(this.lvwExcelTemplateVariables);
            // 
            // splExcelTemplateVariables.Panel2
            // 
            this.splExcelTemplateVariables.Panel2.Controls.Add(this.btnUpdateVariable);
            this.splExcelTemplateVariables.Panel2.Controls.Add(this.btnDeleteVariable);
            this.splExcelTemplateVariables.Panel2.Controls.Add(this.btnAddVariable);
            this.splExcelTemplateVariables.Panel2.Controls.Add(this.txtVariableValue);
            this.splExcelTemplateVariables.Panel2.Controls.Add(this.lblVariableValue);
            this.splExcelTemplateVariables.Panel2.Controls.Add(this.txtVariableTitle);
            this.splExcelTemplateVariables.Panel2.Controls.Add(this.lblVariableTitle);
            this.splExcelTemplateVariables.Panel2.Controls.Add(this.txtVariableName);
            this.splExcelTemplateVariables.Panel2.Controls.Add(this.lblVariableName);
            this.splExcelTemplateVariables.Size = new System.Drawing.Size(1176, 411);
            this.splExcelTemplateVariables.SplitterDistance = 800;
            this.splExcelTemplateVariables.TabIndex = 2;
            // 
            // lvwExcelTemplateVariables
            // 
            this.lvwExcelTemplateVariables.BackColor = System.Drawing.Color.SkyBlue;
            this.lvwExcelTemplateVariables.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.cnVariableName,
            this.cnVariableTitle,
            this.cnVariableBuildin,
            this.cnVariableValue});
            this.lvwExcelTemplateVariables.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwExcelTemplateVariables.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lvwExcelTemplateVariables.FullRowSelect = true;
            this.lvwExcelTemplateVariables.GridLines = true;
            this.lvwExcelTemplateVariables.HideSelection = false;
            this.lvwExcelTemplateVariables.Location = new System.Drawing.Point(0, 0);
            this.lvwExcelTemplateVariables.MultiSelect = false;
            this.lvwExcelTemplateVariables.Name = "lvwExcelTemplateVariables";
            this.lvwExcelTemplateVariables.Size = new System.Drawing.Size(800, 411);
            this.lvwExcelTemplateVariables.TabIndex = 0;
            this.lvwExcelTemplateVariables.UseCompatibleStateImageBehavior = false;
            this.lvwExcelTemplateVariables.View = System.Windows.Forms.View.Details;
            // 
            // cnVariableName
            // 
            this.cnVariableName.Text = "名称";
            this.cnVariableName.Width = 150;
            // 
            // cnVariableTitle
            // 
            this.cnVariableTitle.Text = "标题";
            this.cnVariableTitle.Width = 250;
            // 
            // cnVariableBuildin
            // 
            this.cnVariableBuildin.Text = "是否内置";
            this.cnVariableBuildin.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.cnVariableBuildin.Width = 80;
            // 
            // cnVariableValue
            // 
            this.cnVariableValue.Text = "变量值";
            this.cnVariableValue.Width = 250;
            // 
            // btnUpdateVariable
            // 
            this.btnUpdateVariable.Location = new System.Drawing.Point(93, 129);
            this.btnUpdateVariable.Name = "btnUpdateVariable";
            this.btnUpdateVariable.Size = new System.Drawing.Size(116, 28);
            this.btnUpdateVariable.TabIndex = 42;
            this.btnUpdateVariable.Text = "更新变量";
            this.btnUpdateVariable.UseVisualStyleBackColor = true;
            this.btnUpdateVariable.Click += new System.EventHandler(this.btnUpdateVariable_Click);
            // 
            // btnDeleteVariable
            // 
            this.btnDeleteVariable.Location = new System.Drawing.Point(93, 224);
            this.btnDeleteVariable.Name = "btnDeleteVariable";
            this.btnDeleteVariable.Size = new System.Drawing.Size(116, 28);
            this.btnDeleteVariable.TabIndex = 41;
            this.btnDeleteVariable.Text = "删除变量";
            this.btnDeleteVariable.UseVisualStyleBackColor = true;
            this.btnDeleteVariable.Click += new System.EventHandler(this.btnDeleteVariable_Click);
            // 
            // btnAddVariable
            // 
            this.btnAddVariable.Location = new System.Drawing.Point(93, 173);
            this.btnAddVariable.Name = "btnAddVariable";
            this.btnAddVariable.Size = new System.Drawing.Size(116, 28);
            this.btnAddVariable.TabIndex = 39;
            this.btnAddVariable.Text = "添加变量";
            this.btnAddVariable.UseVisualStyleBackColor = true;
            this.btnAddVariable.Click += new System.EventHandler(this.btnAddVariable_Click);
            // 
            // txtVariableValue
            // 
            this.txtVariableValue.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtVariableValue.Location = new System.Drawing.Point(93, 97);
            this.txtVariableValue.Name = "txtVariableValue";
            this.txtVariableValue.Size = new System.Drawing.Size(250, 26);
            this.txtVariableValue.TabIndex = 30;
            // 
            // lblVariableValue
            // 
            this.lblVariableValue.AutoSize = true;
            this.lblVariableValue.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVariableValue.Location = new System.Drawing.Point(12, 104);
            this.lblVariableValue.Name = "lblVariableValue";
            this.lblVariableValue.Size = new System.Drawing.Size(51, 12);
            this.lblVariableValue.TabIndex = 29;
            this.lblVariableValue.Text = "变量值:";
            // 
            // txtVariableTitle
            // 
            this.txtVariableTitle.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtVariableTitle.Location = new System.Drawing.Point(93, 55);
            this.txtVariableTitle.Name = "txtVariableTitle";
            this.txtVariableTitle.Size = new System.Drawing.Size(250, 26);
            this.txtVariableTitle.TabIndex = 28;
            // 
            // lblVariableTitle
            // 
            this.lblVariableTitle.AutoSize = true;
            this.lblVariableTitle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVariableTitle.Location = new System.Drawing.Point(12, 62);
            this.lblVariableTitle.Name = "lblVariableTitle";
            this.lblVariableTitle.Size = new System.Drawing.Size(64, 12);
            this.lblVariableTitle.TabIndex = 27;
            this.lblVariableTitle.Text = "变量标题:";
            // 
            // txtVariableName
            // 
            this.txtVariableName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtVariableName.Location = new System.Drawing.Point(93, 23);
            this.txtVariableName.Name = "txtVariableName";
            this.txtVariableName.Size = new System.Drawing.Size(250, 26);
            this.txtVariableName.TabIndex = 26;
            // 
            // lblVariableName
            // 
            this.lblVariableName.AutoSize = true;
            this.lblVariableName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblVariableName.Location = new System.Drawing.Point(12, 30);
            this.lblVariableName.Name = "lblVariableName";
            this.lblVariableName.Size = new System.Drawing.Size(64, 12);
            this.lblVariableName.TabIndex = 25;
            this.lblVariableName.Text = "变量名称:";
            // 
            // tabBuildExcelTemplate
            // 
            this.tabBuildExcelTemplate.Controls.Add(this.pnlBuildExcelTemplate);
            this.tabBuildExcelTemplate.Location = new System.Drawing.Point(4, 29);
            this.tabBuildExcelTemplate.Name = "tabBuildExcelTemplate";
            this.tabBuildExcelTemplate.Padding = new System.Windows.Forms.Padding(3);
            this.tabBuildExcelTemplate.Size = new System.Drawing.Size(1176, 411);
            this.tabBuildExcelTemplate.TabIndex = 0;
            this.tabBuildExcelTemplate.Text = "生成Excel模板";
            this.tabBuildExcelTemplate.UseVisualStyleBackColor = true;
            // 
            // pnlBuildExcelTemplate
            // 
            this.pnlBuildExcelTemplate.BackColor = System.Drawing.Color.SkyBlue;
            this.pnlBuildExcelTemplate.Controls.Add(this.splBuildExcelTemplate);
            this.pnlBuildExcelTemplate.Controls.Add(this.pnlBuildExcelTemplateTop);
            this.pnlBuildExcelTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBuildExcelTemplate.Location = new System.Drawing.Point(3, 3);
            this.pnlBuildExcelTemplate.Name = "pnlBuildExcelTemplate";
            this.pnlBuildExcelTemplate.Size = new System.Drawing.Size(1170, 405);
            this.pnlBuildExcelTemplate.TabIndex = 0;
            // 
            // splBuildExcelTemplate
            // 
            this.splBuildExcelTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splBuildExcelTemplate.Location = new System.Drawing.Point(0, 50);
            this.splBuildExcelTemplate.Name = "splBuildExcelTemplate";
            // 
            // splBuildExcelTemplate.Panel1
            // 
            this.splBuildExcelTemplate.Panel1.Controls.Add(this.lvwExcelTemplate);
            // 
            // splBuildExcelTemplate.Panel2
            // 
            this.splBuildExcelTemplate.Panel2.AutoScroll = true;
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.txtTagValues);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.lblTagValues);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.txtTagDefaultValue);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.lblTagDefaultValue);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.lblTagWidthUnit);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.btnUpdateTag);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.chkTagVisible);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.txtTagWidth);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.lblTagWidth);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.txtTagTitle);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.lblTagTitle);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.txtTagName);
            this.splBuildExcelTemplate.Panel2.Controls.Add(this.lblTagName);
            this.splBuildExcelTemplate.Size = new System.Drawing.Size(1170, 355);
            this.splBuildExcelTemplate.SplitterDistance = 800;
            this.splBuildExcelTemplate.TabIndex = 1;
            // 
            // lvwExcelTemplate
            // 
            this.lvwExcelTemplate.BackColor = System.Drawing.Color.SkyBlue;
            this.lvwExcelTemplate.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.cnTitle,
            this.cnVisible,
            this.cnWidth,
            this.cnDefaultValue,
            this.cnValues});
            this.lvwExcelTemplate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwExcelTemplate.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lvwExcelTemplate.FullRowSelect = true;
            this.lvwExcelTemplate.GridLines = true;
            this.lvwExcelTemplate.HideSelection = false;
            this.lvwExcelTemplate.Location = new System.Drawing.Point(0, 0);
            this.lvwExcelTemplate.MultiSelect = false;
            this.lvwExcelTemplate.Name = "lvwExcelTemplate";
            this.lvwExcelTemplate.Size = new System.Drawing.Size(800, 355);
            this.lvwExcelTemplate.TabIndex = 0;
            this.lvwExcelTemplate.UseCompatibleStateImageBehavior = false;
            this.lvwExcelTemplate.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "名称";
            this.chName.Width = 120;
            // 
            // cnTitle
            // 
            this.cnTitle.Text = "标题";
            this.cnTitle.Width = 150;
            // 
            // cnVisible
            // 
            this.cnVisible.Text = "可见性";
            this.cnVisible.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cnWidth
            // 
            this.cnWidth.Text = "宽度";
            this.cnWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // cnDefaultValue
            // 
            this.cnDefaultValue.Text = "默认值";
            this.cnDefaultValue.Width = 150;
            // 
            // cnValues
            // 
            this.cnValues.Text = "枚举值";
            this.cnValues.Width = 200;
            // 
            // txtTagValues
            // 
            this.txtTagValues.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTagValues.Location = new System.Drawing.Point(99, 175);
            this.txtTagValues.Multiline = true;
            this.txtTagValues.Name = "txtTagValues";
            this.txtTagValues.Size = new System.Drawing.Size(250, 144);
            this.txtTagValues.TabIndex = 57;
            this.tagToolTip.SetToolTip(this.txtTagValues, "一行为一个枚举值,枚举值不允许重复");
            // 
            // lblTagValues
            // 
            this.lblTagValues.AutoSize = true;
            this.lblTagValues.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTagValues.Location = new System.Drawing.Point(18, 189);
            this.lblTagValues.Name = "lblTagValues";
            this.lblTagValues.Size = new System.Drawing.Size(77, 12);
            this.lblTagValues.TabIndex = 56;
            this.lblTagValues.Text = "标签枚举值:";
            // 
            // txtTagDefaultValue
            // 
            this.txtTagDefaultValue.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTagDefaultValue.Location = new System.Drawing.Point(99, 143);
            this.txtTagDefaultValue.Name = "txtTagDefaultValue";
            this.txtTagDefaultValue.Size = new System.Drawing.Size(250, 26);
            this.txtTagDefaultValue.TabIndex = 55;
            this.tagToolTip.SetToolTip(this.txtTagDefaultValue, "如果使用模板变量,模板变量名称必须用\"{}\"括起来.");
            // 
            // lblTagDefaultValue
            // 
            this.lblTagDefaultValue.AutoSize = true;
            this.lblTagDefaultValue.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTagDefaultValue.Location = new System.Drawing.Point(18, 150);
            this.lblTagDefaultValue.Name = "lblTagDefaultValue";
            this.lblTagDefaultValue.Size = new System.Drawing.Size(77, 12);
            this.lblTagDefaultValue.TabIndex = 54;
            this.lblTagDefaultValue.Text = "标签默认值:";
            // 
            // lblTagWidthUnit
            // 
            this.lblTagWidthUnit.AutoSize = true;
            this.lblTagWidthUnit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTagWidthUnit.Location = new System.Drawing.Point(221, 118);
            this.lblTagWidthUnit.Name = "lblTagWidthUnit";
            this.lblTagWidthUnit.Size = new System.Drawing.Size(19, 12);
            this.lblTagWidthUnit.TabIndex = 53;
            this.lblTagWidthUnit.Text = "pt";
            // 
            // btnUpdateTag
            // 
            this.btnUpdateTag.Location = new System.Drawing.Point(99, 325);
            this.btnUpdateTag.Name = "btnUpdateTag";
            this.btnUpdateTag.Size = new System.Drawing.Size(116, 28);
            this.btnUpdateTag.TabIndex = 52;
            this.btnUpdateTag.Text = "更新标签";
            this.btnUpdateTag.UseVisualStyleBackColor = true;
            this.btnUpdateTag.Click += new System.EventHandler(this.btnUpdateTag_Click);
            // 
            // chkTagVisible
            // 
            this.chkTagVisible.AutoSize = true;
            this.chkTagVisible.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkTagVisible.Location = new System.Drawing.Point(99, 85);
            this.chkTagVisible.Name = "chkTagVisible";
            this.chkTagVisible.Size = new System.Drawing.Size(61, 20);
            this.chkTagVisible.TabIndex = 50;
            this.chkTagVisible.Text = "可见";
            this.tagToolTip.SetToolTip(this.chkTagVisible, "不可见的标签在生成的Excel模板文件中宽度为0");
            this.chkTagVisible.UseVisualStyleBackColor = true;
            // 
            // txtTagWidth
            // 
            this.txtTagWidth.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTagWidth.Location = new System.Drawing.Point(99, 111);
            this.txtTagWidth.Name = "txtTagWidth";
            this.txtTagWidth.Size = new System.Drawing.Size(116, 26);
            this.txtTagWidth.TabIndex = 48;
            this.txtTagWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblTagWidth
            // 
            this.lblTagWidth.AutoSize = true;
            this.lblTagWidth.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTagWidth.Location = new System.Drawing.Point(18, 118);
            this.lblTagWidth.Name = "lblTagWidth";
            this.lblTagWidth.Size = new System.Drawing.Size(64, 12);
            this.lblTagWidth.TabIndex = 47;
            this.lblTagWidth.Text = "标签宽度:";
            // 
            // txtTagTitle
            // 
            this.txtTagTitle.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTagTitle.Location = new System.Drawing.Point(99, 53);
            this.txtTagTitle.Name = "txtTagTitle";
            this.txtTagTitle.Size = new System.Drawing.Size(250, 26);
            this.txtTagTitle.TabIndex = 46;
            // 
            // lblTagTitle
            // 
            this.lblTagTitle.AutoSize = true;
            this.lblTagTitle.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTagTitle.Location = new System.Drawing.Point(18, 60);
            this.lblTagTitle.Name = "lblTagTitle";
            this.lblTagTitle.Size = new System.Drawing.Size(64, 12);
            this.lblTagTitle.TabIndex = 45;
            this.lblTagTitle.Text = "标签标题:";
            // 
            // txtTagName
            // 
            this.txtTagName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtTagName.Location = new System.Drawing.Point(99, 21);
            this.txtTagName.Name = "txtTagName";
            this.txtTagName.ReadOnly = true;
            this.txtTagName.Size = new System.Drawing.Size(250, 26);
            this.txtTagName.TabIndex = 44;
            // 
            // lblTagName
            // 
            this.lblTagName.AutoSize = true;
            this.lblTagName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTagName.Location = new System.Drawing.Point(18, 28);
            this.lblTagName.Name = "lblTagName";
            this.lblTagName.Size = new System.Drawing.Size(64, 12);
            this.lblTagName.TabIndex = 43;
            this.lblTagName.Text = "标签名称:";
            // 
            // pnlBuildExcelTemplateTop
            // 
            this.pnlBuildExcelTemplateTop.Controls.Add(this.btnTagMoveDn);
            this.pnlBuildExcelTemplateTop.Controls.Add(this.btnTagMoveUp);
            this.pnlBuildExcelTemplateTop.Controls.Add(this.btnBuildExcelTemplate);
            this.pnlBuildExcelTemplateTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlBuildExcelTemplateTop.Location = new System.Drawing.Point(0, 0);
            this.pnlBuildExcelTemplateTop.Name = "pnlBuildExcelTemplateTop";
            this.pnlBuildExcelTemplateTop.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlBuildExcelTemplateTop.Size = new System.Drawing.Size(1170, 50);
            this.pnlBuildExcelTemplateTop.TabIndex = 0;
            // 
            // btnTagMoveDn
            // 
            this.btnTagMoveDn.Location = new System.Drawing.Point(238, 13);
            this.btnTagMoveDn.Name = "btnTagMoveDn";
            this.btnTagMoveDn.Size = new System.Drawing.Size(116, 28);
            this.btnTagMoveDn.TabIndex = 53;
            this.btnTagMoveDn.Text = "下移";
            this.btnTagMoveDn.UseVisualStyleBackColor = true;
            this.btnTagMoveDn.Click += new System.EventHandler(this.btnTagMoveDn_Click);
            // 
            // btnTagMoveUp
            // 
            this.btnTagMoveUp.Location = new System.Drawing.Point(106, 13);
            this.btnTagMoveUp.Name = "btnTagMoveUp";
            this.btnTagMoveUp.Size = new System.Drawing.Size(116, 28);
            this.btnTagMoveUp.TabIndex = 52;
            this.btnTagMoveUp.Text = "上移";
            this.btnTagMoveUp.UseVisualStyleBackColor = true;
            this.btnTagMoveUp.Click += new System.EventHandler(this.btnTagMoveUp_Click);
            // 
            // btnBuildExcelTemplate
            // 
            this.btnBuildExcelTemplate.Location = new System.Drawing.Point(373, 13);
            this.btnBuildExcelTemplate.Name = "btnBuildExcelTemplate";
            this.btnBuildExcelTemplate.Size = new System.Drawing.Size(148, 28);
            this.btnBuildExcelTemplate.TabIndex = 39;
            this.btnBuildExcelTemplate.Text = "生成Excel模板文件";
            this.btnBuildExcelTemplate.UseVisualStyleBackColor = true;
            this.btnBuildExcelTemplate.Click += new System.EventHandler(this.btnBuildExcelTemplate_Click);
            // 
            // tabMerge
            // 
            this.tabMerge.Controls.Add(this.pnlMerge);
            this.tabMerge.Location = new System.Drawing.Point(4, 29);
            this.tabMerge.Name = "tabMerge";
            this.tabMerge.Size = new System.Drawing.Size(1176, 411);
            this.tabMerge.TabIndex = 3;
            this.tabMerge.Text = "合并Excel文件";
            this.tabMerge.UseVisualStyleBackColor = true;
            // 
            // pnlMerge
            // 
            this.pnlMerge.BackColor = System.Drawing.Color.SkyBlue;
            this.pnlMerge.Controls.Add(this.btnMerge);
            this.pnlMerge.Controls.Add(this.txtMergeSourceDataRow);
            this.pnlMerge.Controls.Add(this.lblMergeSourceDataRow);
            this.pnlMerge.Controls.Add(this.txtMergeSourceFieldRow);
            this.pnlMerge.Controls.Add(this.lblMergeSourceFieldRow);
            this.pnlMerge.Controls.Add(this.txtMergeNotice);
            this.pnlMerge.Controls.Add(this.btnOpenMergeSourceFile);
            this.pnlMerge.Controls.Add(this.btnOpenMergeSourceFileBrowser);
            this.pnlMerge.Controls.Add(this.btnBrowseSourceMergeFile);
            this.pnlMerge.Controls.Add(this.txtMergeSourceFile);
            this.pnlMerge.Controls.Add(this.lblMergeSourceFile);
            this.pnlMerge.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMerge.Location = new System.Drawing.Point(0, 0);
            this.pnlMerge.Name = "pnlMerge";
            this.pnlMerge.Size = new System.Drawing.Size(1176, 411);
            this.pnlMerge.TabIndex = 0;
            // 
            // btnMerge
            // 
            this.btnMerge.Location = new System.Drawing.Point(109, 121);
            this.btnMerge.Name = "btnMerge";
            this.btnMerge.Size = new System.Drawing.Size(116, 28);
            this.btnMerge.TabIndex = 51;
            this.btnMerge.Text = "开始合并";
            this.btnMerge.UseVisualStyleBackColor = true;
            this.btnMerge.Click += new System.EventHandler(this.btnMerge_Click);
            // 
            // txtMergeSourceDataRow
            // 
            this.txtMergeSourceDataRow.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMergeSourceDataRow.Location = new System.Drawing.Point(109, 80);
            this.txtMergeSourceDataRow.Name = "txtMergeSourceDataRow";
            this.txtMergeSourceDataRow.ReadOnly = true;
            this.txtMergeSourceDataRow.Size = new System.Drawing.Size(116, 26);
            this.txtMergeSourceDataRow.TabIndex = 50;
            this.txtMergeSourceDataRow.Text = "3";
            this.txtMergeSourceDataRow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblMergeSourceDataRow
            // 
            this.lblMergeSourceDataRow.AutoSize = true;
            this.lblMergeSourceDataRow.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMergeSourceDataRow.Location = new System.Drawing.Point(8, 87);
            this.lblMergeSourceDataRow.Name = "lblMergeSourceDataRow";
            this.lblMergeSourceDataRow.Size = new System.Drawing.Size(90, 12);
            this.lblMergeSourceDataRow.TabIndex = 49;
            this.lblMergeSourceDataRow.Text = "合并源数据行:";
            // 
            // txtMergeSourceFieldRow
            // 
            this.txtMergeSourceFieldRow.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMergeSourceFieldRow.Location = new System.Drawing.Point(109, 48);
            this.txtMergeSourceFieldRow.Name = "txtMergeSourceFieldRow";
            this.txtMergeSourceFieldRow.ReadOnly = true;
            this.txtMergeSourceFieldRow.Size = new System.Drawing.Size(116, 26);
            this.txtMergeSourceFieldRow.TabIndex = 48;
            this.txtMergeSourceFieldRow.Text = "2";
            this.txtMergeSourceFieldRow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblMergeSourceFieldRow
            // 
            this.lblMergeSourceFieldRow.AutoSize = true;
            this.lblMergeSourceFieldRow.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMergeSourceFieldRow.Location = new System.Drawing.Point(8, 56);
            this.lblMergeSourceFieldRow.Name = "lblMergeSourceFieldRow";
            this.lblMergeSourceFieldRow.Size = new System.Drawing.Size(90, 12);
            this.lblMergeSourceFieldRow.TabIndex = 47;
            this.lblMergeSourceFieldRow.Text = "合并源标签行:";
            // 
            // txtMergeNotice
            // 
            this.txtMergeNotice.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMergeNotice.Location = new System.Drawing.Point(231, 50);
            this.txtMergeNotice.Multiline = true;
            this.txtMergeNotice.Name = "txtMergeNotice";
            this.txtMergeNotice.ReadOnly = true;
            this.txtMergeNotice.Size = new System.Drawing.Size(839, 56);
            this.txtMergeNotice.TabIndex = 46;
            this.txtMergeNotice.Text = "注意:合并是把Excel合并文件中的数据合并到Excel模板文件,合并的依据是Excel模板文件中的成果文件与Excel合并文件中的title标签匹配,匹配的标准" +
    "是忽略其中的空格符号和扩展名;";
            // 
            // btnOpenMergeSourceFile
            // 
            this.btnOpenMergeSourceFile.Location = new System.Drawing.Point(990, 16);
            this.btnOpenMergeSourceFile.Name = "btnOpenMergeSourceFile";
            this.btnOpenMergeSourceFile.Size = new System.Drawing.Size(80, 28);
            this.btnOpenMergeSourceFile.TabIndex = 45;
            this.btnOpenMergeSourceFile.Text = "打开";
            this.btnOpenMergeSourceFile.UseVisualStyleBackColor = true;
            this.btnOpenMergeSourceFile.Click += new System.EventHandler(this.btnOpenMergeSourceFile_Click);
            // 
            // btnOpenMergeSourceFileBrowser
            // 
            this.btnOpenMergeSourceFileBrowser.Location = new System.Drawing.Point(818, 15);
            this.btnOpenMergeSourceFileBrowser.Name = "btnOpenMergeSourceFileBrowser";
            this.btnOpenMergeSourceFileBrowser.Size = new System.Drawing.Size(151, 28);
            this.btnOpenMergeSourceFileBrowser.TabIndex = 44;
            this.btnOpenMergeSourceFileBrowser.Text = "打开文件所在的位置";
            this.btnOpenMergeSourceFileBrowser.UseVisualStyleBackColor = true;
            this.btnOpenMergeSourceFileBrowser.Click += new System.EventHandler(this.btnOpenMergeSourceFileBrowser_Click);
            // 
            // btnBrowseSourceMergeFile
            // 
            this.btnBrowseSourceMergeFile.Location = new System.Drawing.Point(722, 16);
            this.btnBrowseSourceMergeFile.Name = "btnBrowseSourceMergeFile";
            this.btnBrowseSourceMergeFile.Size = new System.Drawing.Size(80, 28);
            this.btnBrowseSourceMergeFile.TabIndex = 43;
            this.btnBrowseSourceMergeFile.Text = "浏览...";
            this.btnBrowseSourceMergeFile.UseVisualStyleBackColor = true;
            this.btnBrowseSourceMergeFile.Click += new System.EventHandler(this.btnBrowseMergeSourceFile_Click);
            // 
            // txtMergeSourceFile
            // 
            this.txtMergeSourceFile.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtMergeSourceFile.Location = new System.Drawing.Point(109, 16);
            this.txtMergeSourceFile.Name = "txtMergeSourceFile";
            this.txtMergeSourceFile.Size = new System.Drawing.Size(607, 26);
            this.txtMergeSourceFile.TabIndex = 42;
            // 
            // lblMergeSourceFile
            // 
            this.lblMergeSourceFile.AutoSize = true;
            this.lblMergeSourceFile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMergeSourceFile.Location = new System.Drawing.Point(8, 24);
            this.lblMergeSourceFile.Name = "lblMergeSourceFile";
            this.lblMergeSourceFile.Size = new System.Drawing.Size(77, 12);
            this.lblMergeSourceFile.TabIndex = 41;
            this.lblMergeSourceFile.Text = "合并源文件:";
            // 
            // tabBatchSubmitIndex
            // 
            this.tabBatchSubmitIndex.BackColor = System.Drawing.Color.SkyBlue;
            this.tabBatchSubmitIndex.Controls.Add(this.pnlBatchSubmitIndex);
            this.tabBatchSubmitIndex.Location = new System.Drawing.Point(4, 29);
            this.tabBatchSubmitIndex.Name = "tabBatchSubmitIndex";
            this.tabBatchSubmitIndex.Padding = new System.Windows.Forms.Padding(3);
            this.tabBatchSubmitIndex.Size = new System.Drawing.Size(1176, 411);
            this.tabBatchSubmitIndex.TabIndex = 1;
            this.tabBatchSubmitIndex.Text = "批量提交";
            // 
            // pnlBatchSubmitIndex
            // 
            this.pnlBatchSubmitIndex.BackColor = System.Drawing.Color.SkyBlue;
            this.pnlBatchSubmitIndex.Controls.Add(this.splBatchSubmit);
            this.pnlBatchSubmitIndex.Controls.Add(this.pbrSubmission);
            this.pnlBatchSubmitIndex.Controls.Add(this.pnlSubmitTop);
            this.pnlBatchSubmitIndex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBatchSubmitIndex.Location = new System.Drawing.Point(3, 3);
            this.pnlBatchSubmitIndex.Name = "pnlBatchSubmitIndex";
            this.pnlBatchSubmitIndex.Size = new System.Drawing.Size(1170, 405);
            this.pnlBatchSubmitIndex.TabIndex = 2;
            // 
            // splBatchSubmit
            // 
            this.splBatchSubmit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splBatchSubmit.Location = new System.Drawing.Point(0, 50);
            this.splBatchSubmit.Name = "splBatchSubmit";
            // 
            // splBatchSubmit.Panel1
            // 
            this.splBatchSubmit.Panel1.Controls.Add(this.lvwBatchSubmit);
            // 
            // splBatchSubmit.Panel2
            // 
            this.splBatchSubmit.Panel2.Controls.Add(this.txtSubmitFailure);
            this.splBatchSubmit.Panel2.Controls.Add(this.pnlSubmitFailure);
            this.splBatchSubmit.Size = new System.Drawing.Size(1170, 332);
            this.splBatchSubmit.SplitterDistance = 800;
            this.splBatchSubmit.TabIndex = 4;
            // 
            // lvwBatchSubmit
            // 
            this.lvwBatchSubmit.BackColor = System.Drawing.Color.SkyBlue;
            this.lvwBatchSubmit.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chProductIndex,
            this.chProductFile,
            this.chSubmitStatus,
            this.chSubmitFailureMessage});
            this.lvwBatchSubmit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvwBatchSubmit.FullRowSelect = true;
            this.lvwBatchSubmit.Location = new System.Drawing.Point(0, 0);
            this.lvwBatchSubmit.Name = "lvwBatchSubmit";
            this.lvwBatchSubmit.ShowGroups = false;
            this.lvwBatchSubmit.Size = new System.Drawing.Size(800, 332);
            this.lvwBatchSubmit.TabIndex = 3;
            this.lvwBatchSubmit.UseCompatibleStateImageBehavior = false;
            this.lvwBatchSubmit.View = System.Windows.Forms.View.Details;
            // 
            // chProductIndex
            // 
            this.chProductIndex.Text = "序号";
            // 
            // chProductFile
            // 
            this.chProductFile.Text = "成果文件";
            this.chProductFile.Width = 480;
            // 
            // chSubmitStatus
            // 
            this.chSubmitStatus.Text = "提交状态";
            this.chSubmitStatus.Width = 80;
            // 
            // chSubmitFailureMessage
            // 
            this.chSubmitFailureMessage.Text = "失败信息";
            this.chSubmitFailureMessage.Width = 100;
            // 
            // txtSubmitFailure
            // 
            this.txtSubmitFailure.BackColor = System.Drawing.Color.SkyBlue;
            this.txtSubmitFailure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSubmitFailure.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSubmitFailure.Location = new System.Drawing.Point(0, 21);
            this.txtSubmitFailure.Multiline = true;
            this.txtSubmitFailure.Name = "txtSubmitFailure";
            this.txtSubmitFailure.Size = new System.Drawing.Size(366, 311);
            this.txtSubmitFailure.TabIndex = 48;
            // 
            // pnlSubmitFailure
            // 
            this.pnlSubmitFailure.Controls.Add(this.lblSubmitFailure);
            this.pnlSubmitFailure.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubmitFailure.Location = new System.Drawing.Point(0, 0);
            this.pnlSubmitFailure.Name = "pnlSubmitFailure";
            this.pnlSubmitFailure.Size = new System.Drawing.Size(366, 21);
            this.pnlSubmitFailure.TabIndex = 47;
            // 
            // lblSubmitFailure
            // 
            this.lblSubmitFailure.AutoSize = true;
            this.lblSubmitFailure.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSubmitFailure.Location = new System.Drawing.Point(3, 5);
            this.lblSubmitFailure.Name = "lblSubmitFailure";
            this.lblSubmitFailure.Size = new System.Drawing.Size(64, 12);
            this.lblSubmitFailure.TabIndex = 46;
            this.lblSubmitFailure.Text = "失败信息:";
            // 
            // pbrSubmission
            // 
            this.pbrSubmission.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbrSubmission.Location = new System.Drawing.Point(0, 382);
            this.pbrSubmission.Name = "pbrSubmission";
            this.pbrSubmission.Size = new System.Drawing.Size(1170, 23);
            this.pbrSubmission.TabIndex = 3;
            // 
            // pnlSubmitTop
            // 
            this.pnlSubmitTop.Controls.Add(this.chkParallelSubmit);
            this.pnlSubmitTop.Controls.Add(this.btnRetryBatchSubmit);
            this.pnlSubmitTop.Controls.Add(this.btnBatchSubmitIndex);
            this.pnlSubmitTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlSubmitTop.Location = new System.Drawing.Point(0, 0);
            this.pnlSubmitTop.Name = "pnlSubmitTop";
            this.pnlSubmitTop.Padding = new System.Windows.Forms.Padding(0, 10, 0, 0);
            this.pnlSubmitTop.Size = new System.Drawing.Size(1170, 50);
            this.pnlSubmitTop.TabIndex = 1;
            // 
            // btnRetryBatchSubmit
            // 
            this.btnRetryBatchSubmit.Location = new System.Drawing.Point(260, 13);
            this.btnRetryBatchSubmit.Name = "btnRetryBatchSubmit";
            this.btnRetryBatchSubmit.Size = new System.Drawing.Size(148, 28);
            this.btnRetryBatchSubmit.TabIndex = 40;
            this.btnRetryBatchSubmit.Text = "开始批量提交失败重试";
            this.btnRetryBatchSubmit.UseVisualStyleBackColor = true;
            this.btnRetryBatchSubmit.Click += new System.EventHandler(this.btnRetryBatchSubmit_Click);
            // 
            // btnBatchSubmitIndex
            // 
            this.btnBatchSubmitIndex.Location = new System.Drawing.Point(106, 13);
            this.btnBatchSubmitIndex.Name = "btnBatchSubmitIndex";
            this.btnBatchSubmitIndex.Size = new System.Drawing.Size(148, 28);
            this.btnBatchSubmitIndex.TabIndex = 39;
            this.btnBatchSubmitIndex.Text = "开始批量提交";
            this.btnBatchSubmitIndex.UseVisualStyleBackColor = true;
            this.btnBatchSubmitIndex.Click += new System.EventHandler(this.btnBatchSubmitIndex_Click);
            // 
            // grpProduct
            // 
            this.grpProduct.Controls.Add(this.btnOpenProductFolder);
            this.grpProduct.Controls.Add(this.btnShowOperationDesc);
            this.grpProduct.Controls.Add(this.btnReloadConfig);
            this.grpProduct.Controls.Add(this.btnBrowseConfigFile);
            this.grpProduct.Controls.Add(this.btnOpenConfigFile);
            this.grpProduct.Controls.Add(this.btnOpenExcelFile);
            this.grpProduct.Controls.Add(this.btnOpenExcelFileBrowser);
            this.grpProduct.Controls.Add(this.btnSaveConfig);
            this.grpProduct.Controls.Add(this.btnOpenConfigFileBrowser);
            this.grpProduct.Controls.Add(this.txtConfigFile);
            this.grpProduct.Controls.Add(this.lblConfigFile);
            this.grpProduct.Controls.Add(this.btnBrowseExcelFile);
            this.grpProduct.Controls.Add(this.txtExcelFile);
            this.grpProduct.Controls.Add(this.lblExcelFile);
            this.grpProduct.Controls.Add(this.btnBrowseProductFolder);
            this.grpProduct.Controls.Add(this.txtProductFolder);
            this.grpProduct.Controls.Add(this.lblProductFolder);
            this.grpProduct.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpProduct.Location = new System.Drawing.Point(0, 58);
            this.grpProduct.Name = "grpProduct";
            this.grpProduct.Size = new System.Drawing.Size(1184, 160);
            this.grpProduct.TabIndex = 25;
            this.grpProduct.TabStop = false;
            this.grpProduct.Text = "成果信息";
            // 
            // btnOpenProductFolder
            // 
            this.btnOpenProductFolder.Location = new System.Drawing.Point(822, 18);
            this.btnOpenProductFolder.Name = "btnOpenProductFolder";
            this.btnOpenProductFolder.Size = new System.Drawing.Size(151, 28);
            this.btnOpenProductFolder.TabIndex = 45;
            this.btnOpenProductFolder.Text = "打开文件夹";
            this.btnOpenProductFolder.UseVisualStyleBackColor = true;
            this.btnOpenProductFolder.Click += new System.EventHandler(this.btnOpenProductFolder_Click);
            // 
            // btnShowOperationDesc
            // 
            this.btnShowOperationDesc.Location = new System.Drawing.Point(113, 116);
            this.btnShowOperationDesc.Name = "btnShowOperationDesc";
            this.btnShowOperationDesc.Size = new System.Drawing.Size(116, 28);
            this.btnShowOperationDesc.TabIndex = 44;
            this.btnShowOperationDesc.Text = "显示操作说明";
            this.btnShowOperationDesc.UseVisualStyleBackColor = true;
            this.btnShowOperationDesc.Click += new System.EventHandler(this.btnShowOperationDesc_Click);
            // 
            // btnReloadConfig
            // 
            this.btnReloadConfig.Location = new System.Drawing.Point(380, 116);
            this.btnReloadConfig.Name = "btnReloadConfig";
            this.btnReloadConfig.Size = new System.Drawing.Size(116, 28);
            this.btnReloadConfig.TabIndex = 43;
            this.btnReloadConfig.Text = "重载配置";
            this.btnReloadConfig.UseVisualStyleBackColor = true;
            this.btnReloadConfig.Click += new System.EventHandler(this.btnReloadConfig_Click);
            // 
            // btnBrowseConfigFile
            // 
            this.btnBrowseConfigFile.Location = new System.Drawing.Point(724, 83);
            this.btnBrowseConfigFile.Name = "btnBrowseConfigFile";
            this.btnBrowseConfigFile.Size = new System.Drawing.Size(80, 28);
            this.btnBrowseConfigFile.TabIndex = 42;
            this.btnBrowseConfigFile.Text = "浏览...";
            this.btnBrowseConfigFile.UseVisualStyleBackColor = true;
            this.btnBrowseConfigFile.Click += new System.EventHandler(this.btnBrowseConfigFile_Click);
            // 
            // btnOpenConfigFile
            // 
            this.btnOpenConfigFile.Location = new System.Drawing.Point(994, 82);
            this.btnOpenConfigFile.Name = "btnOpenConfigFile";
            this.btnOpenConfigFile.Size = new System.Drawing.Size(80, 28);
            this.btnOpenConfigFile.TabIndex = 41;
            this.btnOpenConfigFile.Text = "打开";
            this.btnOpenConfigFile.UseVisualStyleBackColor = true;
            this.btnOpenConfigFile.Click += new System.EventHandler(this.btnOpenConfigFile_Click);
            // 
            // btnOpenExcelFile
            // 
            this.btnOpenExcelFile.Location = new System.Drawing.Point(994, 52);
            this.btnOpenExcelFile.Name = "btnOpenExcelFile";
            this.btnOpenExcelFile.Size = new System.Drawing.Size(80, 28);
            this.btnOpenExcelFile.TabIndex = 40;
            this.btnOpenExcelFile.Text = "打开";
            this.btnOpenExcelFile.UseVisualStyleBackColor = true;
            this.btnOpenExcelFile.Click += new System.EventHandler(this.btnOpenExcelFile_Click);
            // 
            // btnOpenExcelFileBrowser
            // 
            this.btnOpenExcelFileBrowser.Location = new System.Drawing.Point(822, 51);
            this.btnOpenExcelFileBrowser.Name = "btnOpenExcelFileBrowser";
            this.btnOpenExcelFileBrowser.Size = new System.Drawing.Size(151, 28);
            this.btnOpenExcelFileBrowser.TabIndex = 39;
            this.btnOpenExcelFileBrowser.Text = "打开文件所在的位置";
            this.btnOpenExcelFileBrowser.UseVisualStyleBackColor = true;
            this.btnOpenExcelFileBrowser.Click += new System.EventHandler(this.btnOpenExcelFileBrowser_Click);
            // 
            // btnSaveConfig
            // 
            this.btnSaveConfig.Location = new System.Drawing.Point(245, 116);
            this.btnSaveConfig.Name = "btnSaveConfig";
            this.btnSaveConfig.Size = new System.Drawing.Size(116, 28);
            this.btnSaveConfig.TabIndex = 38;
            this.btnSaveConfig.Text = "保存配置";
            this.btnSaveConfig.UseVisualStyleBackColor = true;
            this.btnSaveConfig.Click += new System.EventHandler(this.btnSaveConfig_Click);
            // 
            // btnOpenConfigFileBrowser
            // 
            this.btnOpenConfigFileBrowser.Location = new System.Drawing.Point(822, 83);
            this.btnOpenConfigFileBrowser.Name = "btnOpenConfigFileBrowser";
            this.btnOpenConfigFileBrowser.Size = new System.Drawing.Size(151, 28);
            this.btnOpenConfigFileBrowser.TabIndex = 37;
            this.btnOpenConfigFileBrowser.Text = "打开文件所在的位置";
            this.btnOpenConfigFileBrowser.UseVisualStyleBackColor = true;
            this.btnOpenConfigFileBrowser.Click += new System.EventHandler(this.btnOpenConfigFileBrowser_Click);
            // 
            // txtConfigFile
            // 
            this.txtConfigFile.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtConfigFile.Location = new System.Drawing.Point(113, 84);
            this.txtConfigFile.Name = "txtConfigFile";
            this.txtConfigFile.ReadOnly = true;
            this.txtConfigFile.Size = new System.Drawing.Size(607, 26);
            this.txtConfigFile.TabIndex = 36;
            // 
            // lblConfigFile
            // 
            this.lblConfigFile.AutoSize = true;
            this.lblConfigFile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblConfigFile.Location = new System.Drawing.Point(5, 91);
            this.lblConfigFile.Name = "lblConfigFile";
            this.lblConfigFile.Size = new System.Drawing.Size(64, 12);
            this.lblConfigFile.TabIndex = 35;
            this.lblConfigFile.Text = "配置文件:";
            // 
            // btnBrowseExcelFile
            // 
            this.btnBrowseExcelFile.Location = new System.Drawing.Point(726, 52);
            this.btnBrowseExcelFile.Name = "btnBrowseExcelFile";
            this.btnBrowseExcelFile.Size = new System.Drawing.Size(80, 28);
            this.btnBrowseExcelFile.TabIndex = 34;
            this.btnBrowseExcelFile.Text = "浏览...";
            this.btnBrowseExcelFile.UseVisualStyleBackColor = true;
            this.btnBrowseExcelFile.Click += new System.EventHandler(this.btnBrowseExcelFile_Click);
            // 
            // txtExcelFile
            // 
            this.txtExcelFile.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtExcelFile.Location = new System.Drawing.Point(113, 52);
            this.txtExcelFile.Name = "txtExcelFile";
            this.txtExcelFile.Size = new System.Drawing.Size(607, 26);
            this.txtExcelFile.TabIndex = 33;
            // 
            // lblExcelFile
            // 
            this.lblExcelFile.AutoSize = true;
            this.lblExcelFile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblExcelFile.Location = new System.Drawing.Point(5, 59);
            this.lblExcelFile.Name = "lblExcelFile";
            this.lblExcelFile.Size = new System.Drawing.Size(73, 12);
            this.lblExcelFile.TabIndex = 32;
            this.lblExcelFile.Text = "Excel文件:";
            // 
            // btnBrowseProductFolder
            // 
            this.btnBrowseProductFolder.Location = new System.Drawing.Point(726, 20);
            this.btnBrowseProductFolder.Name = "btnBrowseProductFolder";
            this.btnBrowseProductFolder.Size = new System.Drawing.Size(80, 28);
            this.btnBrowseProductFolder.TabIndex = 31;
            this.btnBrowseProductFolder.Text = "浏览...";
            this.btnBrowseProductFolder.UseVisualStyleBackColor = true;
            this.btnBrowseProductFolder.Click += new System.EventHandler(this.btnBrowseProductFolder_Click);
            // 
            // txtProductFolder
            // 
            this.txtProductFolder.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtProductFolder.Location = new System.Drawing.Point(113, 20);
            this.txtProductFolder.Name = "txtProductFolder";
            this.txtProductFolder.Size = new System.Drawing.Size(607, 26);
            this.txtProductFolder.TabIndex = 30;
            // 
            // lblProductFolder
            // 
            this.lblProductFolder.AutoSize = true;
            this.lblProductFolder.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblProductFolder.Location = new System.Drawing.Point(5, 27);
            this.lblProductFolder.Name = "lblProductFolder";
            this.lblProductFolder.Size = new System.Drawing.Size(77, 12);
            this.lblProductFolder.TabIndex = 29;
            this.lblProductFolder.Text = "成果文件夹:";
            // 
            // grpApiService
            // 
            this.grpApiService.Controls.Add(this.btnLoadFromApi);
            this.grpApiService.Controls.Add(this.txtApiServiceUrl);
            this.grpApiService.Controls.Add(this.lblApiServiceUrl);
            this.grpApiService.Controls.Add(this.txtPasswrod);
            this.grpApiService.Controls.Add(this.lblPassword);
            this.grpApiService.Controls.Add(this.txtUserName);
            this.grpApiService.Controls.Add(this.lblUserName);
            this.grpApiService.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpApiService.Location = new System.Drawing.Point(0, 0);
            this.grpApiService.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.grpApiService.Name = "grpApiService";
            this.grpApiService.Size = new System.Drawing.Size(1184, 58);
            this.grpApiService.TabIndex = 23;
            this.grpApiService.TabStop = false;
            this.grpApiService.Text = "WebAPI服务信息";
            // 
            // btnLoadFromApi
            // 
            this.btnLoadFromApi.Location = new System.Drawing.Point(822, 19);
            this.btnLoadFromApi.Name = "btnLoadFromApi";
            this.btnLoadFromApi.Size = new System.Drawing.Size(80, 28);
            this.btnLoadFromApi.TabIndex = 29;
            this.btnLoadFromApi.Text = "加载";
            this.btnLoadFromApi.UseVisualStyleBackColor = true;
            this.btnLoadFromApi.Click += new System.EventHandler(this.btnLoadFromApi_Click);
            // 
            // txtApiServiceUrl
            // 
            this.txtApiServiceUrl.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtApiServiceUrl.Location = new System.Drawing.Point(542, 20);
            this.txtApiServiceUrl.Name = "txtApiServiceUrl";
            this.txtApiServiceUrl.Size = new System.Drawing.Size(264, 26);
            this.txtApiServiceUrl.TabIndex = 28;
            // 
            // lblApiServiceUrl
            // 
            this.lblApiServiceUrl.AutoSize = true;
            this.lblApiServiceUrl.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblApiServiceUrl.Location = new System.Drawing.Point(435, 27);
            this.lblApiServiceUrl.Name = "lblApiServiceUrl";
            this.lblApiServiceUrl.Size = new System.Drawing.Size(101, 12);
            this.lblApiServiceUrl.TabIndex = 27;
            this.lblApiServiceUrl.Text = "WebAPI站点URL:";
            // 
            // txtPasswrod
            // 
            this.txtPasswrod.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPasswrod.Location = new System.Drawing.Point(313, 20);
            this.txtPasswrod.Name = "txtPasswrod";
            this.txtPasswrod.PasswordChar = '*';
            this.txtPasswrod.Size = new System.Drawing.Size(116, 26);
            this.txtPasswrod.TabIndex = 26;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPassword.Location = new System.Drawing.Point(243, 27);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(64, 12);
            this.lblPassword.TabIndex = 25;
            this.lblPassword.Text = "用户密码:";
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserName.Location = new System.Drawing.Point(113, 20);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(116, 26);
            this.txtUserName.TabIndex = 24;
            // 
            // lblUserName
            // 
            this.lblUserName.AutoSize = true;
            this.lblUserName.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblUserName.Location = new System.Drawing.Point(6, 27);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(64, 12);
            this.lblUserName.TabIndex = 0;
            this.lblUserName.Text = "用户名称:";
            // 
            // chkParallelSubmit
            // 
            this.chkParallelSubmit.AutoSize = true;
            this.chkParallelSubmit.Checked = true;
            this.chkParallelSubmit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkParallelSubmit.Location = new System.Drawing.Point(5, 20);
            this.chkParallelSubmit.Name = "chkParallelSubmit";
            this.chkParallelSubmit.Size = new System.Drawing.Size(72, 16);
            this.chkParallelSubmit.TabIndex = 41;
            this.chkParallelSubmit.Text = "并行提交";
            this.chkParallelSubmit.UseVisualStyleBackColor = true;
            // 
            // frmIndexSubmission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 662);
            this.Controls.Add(this.pnlMain);
            this.Name = "frmIndexSubmission";
            this.Text = "索引提交工具";
            this.pnlMain.ResumeLayout(false);
            this.tabMain.ResumeLayout(false);
            this.tabExcelTemplateVariables.ResumeLayout(false);
            this.pnlExcelTemplateVariables.ResumeLayout(false);
            this.splExcelTemplateVariables.Panel1.ResumeLayout(false);
            this.splExcelTemplateVariables.Panel2.ResumeLayout(false);
            this.splExcelTemplateVariables.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splExcelTemplateVariables)).EndInit();
            this.splExcelTemplateVariables.ResumeLayout(false);
            this.tabBuildExcelTemplate.ResumeLayout(false);
            this.pnlBuildExcelTemplate.ResumeLayout(false);
            this.splBuildExcelTemplate.Panel1.ResumeLayout(false);
            this.splBuildExcelTemplate.Panel2.ResumeLayout(false);
            this.splBuildExcelTemplate.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splBuildExcelTemplate)).EndInit();
            this.splBuildExcelTemplate.ResumeLayout(false);
            this.pnlBuildExcelTemplateTop.ResumeLayout(false);
            this.tabMerge.ResumeLayout(false);
            this.pnlMerge.ResumeLayout(false);
            this.pnlMerge.PerformLayout();
            this.tabBatchSubmitIndex.ResumeLayout(false);
            this.pnlBatchSubmitIndex.ResumeLayout(false);
            this.splBatchSubmit.Panel1.ResumeLayout(false);
            this.splBatchSubmit.Panel2.ResumeLayout(false);
            this.splBatchSubmit.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splBatchSubmit)).EndInit();
            this.splBatchSubmit.ResumeLayout(false);
            this.pnlSubmitFailure.ResumeLayout(false);
            this.pnlSubmitFailure.PerformLayout();
            this.pnlSubmitTop.ResumeLayout(false);
            this.pnlSubmitTop.PerformLayout();
            this.grpProduct.ResumeLayout(false);
            this.grpProduct.PerformLayout();
            this.grpApiService.ResumeLayout(false);
            this.grpApiService.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.GroupBox grpApiService;
        private System.Windows.Forms.Button btnLoadFromApi;
        private System.Windows.Forms.TextBox txtApiServiceUrl;
        private System.Windows.Forms.Label lblApiServiceUrl;
        private System.Windows.Forms.TextBox txtPasswrod;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label lblUserName;
        private System.Windows.Forms.GroupBox grpProduct;
        private System.Windows.Forms.TextBox txtProductFolder;
        private System.Windows.Forms.Label lblProductFolder;
        private System.Windows.Forms.Button btnBrowseProductFolder;
        private System.Windows.Forms.TabControl tabMain;
        private System.Windows.Forms.TabPage tabBuildExcelTemplate;
        private System.Windows.Forms.Panel pnlBuildExcelTemplate;
        private System.Windows.Forms.TabPage tabBatchSubmitIndex;
        private System.Windows.Forms.Button btnBrowseExcelFile;
        private System.Windows.Forms.TextBox txtExcelFile;
        private System.Windows.Forms.Label lblExcelFile;
        private System.Windows.Forms.Panel pnlBatchSubmitIndex;
        private System.Windows.Forms.Button btnOpenConfigFileBrowser;
        private System.Windows.Forms.TextBox txtConfigFile;
        private System.Windows.Forms.Label lblConfigFile;
        private System.Windows.Forms.Button btnSaveConfig;
        private System.Windows.Forms.Panel pnlBuildExcelTemplateTop;
        private System.Windows.Forms.SplitContainer splBuildExcelTemplate;
        private System.Windows.Forms.Button btnOpenExcelFileBrowser;
        private System.Windows.Forms.Button btnOpenExcelFile;
        private System.Windows.Forms.Button btnOpenConfigFile;
        private System.Windows.Forms.Button btnBuildExcelTemplate;
        private System.Windows.Forms.ListView lvwExcelTemplate;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader cnTitle;
        private System.Windows.Forms.ColumnHeader cnVisible;
        private System.Windows.Forms.ColumnHeader cnWidth;
        private System.Windows.Forms.ColumnHeader cnDefaultValue;
        private System.Windows.Forms.ColumnHeader cnValues;
        private System.Windows.Forms.TabPage tabExcelTemplateVariables;
        private System.Windows.Forms.Panel pnlExcelTemplateVariables;
        private System.Windows.Forms.SplitContainer splExcelTemplateVariables;
        private System.Windows.Forms.ListView lvwExcelTemplateVariables;
        private System.Windows.Forms.ColumnHeader cnVariableName;
        private System.Windows.Forms.ColumnHeader cnVariableTitle;
        private System.Windows.Forms.ColumnHeader cnVariableValue;
        private System.Windows.Forms.ColumnHeader cnVariableBuildin;
        private System.Windows.Forms.TextBox txtVariableName;
        private System.Windows.Forms.Label lblVariableName;
        private System.Windows.Forms.Button btnAddVariable;
        private System.Windows.Forms.TextBox txtVariableValue;
        private System.Windows.Forms.Label lblVariableValue;
        private System.Windows.Forms.TextBox txtVariableTitle;
        private System.Windows.Forms.Label lblVariableTitle;
        private System.Windows.Forms.Button btnDeleteVariable;
        private System.Windows.Forms.Button btnUpdateVariable;
        private System.Windows.Forms.Button btnUpdateTag;
        private System.Windows.Forms.CheckBox chkTagVisible;
        private System.Windows.Forms.TextBox txtTagWidth;
        private System.Windows.Forms.Label lblTagWidth;
        private System.Windows.Forms.TextBox txtTagTitle;
        private System.Windows.Forms.Label lblTagTitle;
        private System.Windows.Forms.TextBox txtTagName;
        private System.Windows.Forms.Label lblTagName;
        private System.Windows.Forms.Label lblTagWidthUnit;
        private System.Windows.Forms.TextBox txtTagDefaultValue;
        private System.Windows.Forms.Label lblTagDefaultValue;
        private System.Windows.Forms.Label lblTagValues;
        private System.Windows.Forms.Button btnTagMoveDn;
        private System.Windows.Forms.Button btnTagMoveUp;
        private System.Windows.Forms.Panel pnlSubmitTop;
        private System.Windows.Forms.Button btnBatchSubmitIndex;
        private System.Windows.Forms.Button btnRetryBatchSubmit;
        private System.Windows.Forms.Button btnReloadConfig;
        private System.Windows.Forms.Button btnBrowseConfigFile;
        private System.Windows.Forms.Button btnShowOperationDesc;
        private System.Windows.Forms.Button btnOpenProductFolder;
        private System.Windows.Forms.TextBox txtTagValues;
        private System.Windows.Forms.ToolTip tagToolTip;
        private System.Windows.Forms.SplitContainer splBatchSubmit;
        private System.Windows.Forms.ListView lvwBatchSubmit;
        private System.Windows.Forms.ColumnHeader chProductIndex;
        private System.Windows.Forms.ColumnHeader chProductFile;
        private System.Windows.Forms.ColumnHeader chSubmitStatus;
        private System.Windows.Forms.ColumnHeader chSubmitFailureMessage;
        private System.Windows.Forms.TextBox txtSubmitFailure;
        private System.Windows.Forms.Panel pnlSubmitFailure;
        private System.Windows.Forms.Label lblSubmitFailure;
        private System.Windows.Forms.ProgressBar pbrSubmission;
        private System.Windows.Forms.TabPage tabMerge;
        private System.Windows.Forms.Panel pnlMerge;
        private System.Windows.Forms.Button btnOpenMergeSourceFile;
        private System.Windows.Forms.Button btnOpenMergeSourceFileBrowser;
        private System.Windows.Forms.Button btnBrowseSourceMergeFile;
        private System.Windows.Forms.TextBox txtMergeSourceFile;
        private System.Windows.Forms.Label lblMergeSourceFile;
        private System.Windows.Forms.TextBox txtMergeSourceDataRow;
        private System.Windows.Forms.Label lblMergeSourceDataRow;
        private System.Windows.Forms.TextBox txtMergeSourceFieldRow;
        private System.Windows.Forms.Label lblMergeSourceFieldRow;
        private System.Windows.Forms.TextBox txtMergeNotice;
        private System.Windows.Forms.Button btnMerge;
        private System.Windows.Forms.CheckBox chkParallelSubmit;
    }
}