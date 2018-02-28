namespace Jurassic.GF.DataGatherTool
{
    partial class DataBaseSetting
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.wzdMain = new DevExpress.XtraWizard.WizardControl();
            this.wpgWelcome = new DevExpress.XtraWizard.WelcomeWizardPage();
            this.wpgDBType = new DevExpress.XtraWizard.WizardPage();
            this.rdbSqlServer = new System.Windows.Forms.RadioButton();
            this.rdbOracle = new System.Windows.Forms.RadioButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.wpgConnectParams = new DevExpress.XtraWizard.WizardPage();
            this.txtDabaseName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl6 = new DevExpress.XtraEditors.LabelControl();
            this.btnTest = new DevExpress.XtraEditors.SimpleButton();
            this.txtPassword = new DevExpress.XtraEditors.TextEdit();
            this.txtUserName = new DevExpress.XtraEditors.TextEdit();
            this.txtServerName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.wzdMain)).BeginInit();
            this.wzdMain.SuspendLayout();
            this.wpgDBType.SuspendLayout();
            this.wpgConnectParams.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDabaseName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // wzdMain
            // 
            this.wzdMain.CancelText = "取消";
            this.wzdMain.Controls.Add(this.wpgWelcome);
            this.wzdMain.Controls.Add(this.wpgDBType);
            this.wzdMain.Controls.Add(this.wpgConnectParams);
            this.wzdMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wzdMain.FinishText = "完成";
            this.wzdMain.Location = new System.Drawing.Point(0, 0);
            this.wzdMain.Name = "wzdMain";
            this.wzdMain.NextText = "下一步";
            this.wzdMain.Pages.AddRange(new DevExpress.XtraWizard.BaseWizardPage[] {
            this.wpgWelcome,
            this.wpgDBType,
            this.wpgConnectParams});
            this.wzdMain.PreviousText = "上一步";
            this.wzdMain.Size = new System.Drawing.Size(541, 428);
            this.wzdMain.Text = "";
            this.wzdMain.CancelClick += new System.ComponentModel.CancelEventHandler(this.wzdMain_CancelClick);
            this.wzdMain.FinishClick += new System.ComponentModel.CancelEventHandler(this.wzdMain_FinishClick);
            // 
            // wpgWelcome
            // 
            this.wpgWelcome.IntroductionText = "欢迎使用采集工具数据库设置向导，此向导将帮助您完成采集工具数据库的设置。\r\n\r\n要使用此向导，请确保数据库服务器已经安装Oracle 11g或者SqlServer" +
    "2012。";
            this.wpgWelcome.Name = "wpgWelcome";
            this.wpgWelcome.ProceedText = "按“下一步”键，继续";
            this.wpgWelcome.Size = new System.Drawing.Size(324, 295);
            this.wpgWelcome.Text = "欢迎使用采集工具数据库设置向导！";
            // 
            // wpgDBType
            // 
            this.wpgDBType.Controls.Add(this.rdbSqlServer);
            this.wpgDBType.Controls.Add(this.rdbOracle);
            this.wpgDBType.Controls.Add(this.labelControl1);
            this.wpgDBType.DescriptionText = "选择要连接的数据库类型，请确保选择的数据库系统已经安装并运行。";
            this.wpgDBType.Name = "wpgDBType";
            this.wpgDBType.Size = new System.Drawing.Size(509, 283);
            this.wpgDBType.Text = "第一步：选择数据库类型";
            // 
            // rdbSqlServer
            // 
            this.rdbSqlServer.AutoSize = true;
            this.rdbSqlServer.Location = new System.Drawing.Point(81, 131);
            this.rdbSqlServer.Name = "rdbSqlServer";
            this.rdbSqlServer.Size = new System.Drawing.Size(113, 16);
            this.rdbSqlServer.TabIndex = 1;
            this.rdbSqlServer.Text = "SQL Server 2012";
            this.rdbSqlServer.UseVisualStyleBackColor = true;
            this.rdbSqlServer.CheckedChanged += new System.EventHandler(this.rdbSqlServer_CheckedChanged);
            // 
            // rdbOracle
            // 
            this.rdbOracle.AutoSize = true;
            this.rdbOracle.Checked = true;
            this.rdbOracle.Location = new System.Drawing.Point(81, 109);
            this.rdbOracle.Name = "rdbOracle";
            this.rdbOracle.Size = new System.Drawing.Size(83, 16);
            this.rdbOracle.TabIndex = 1;
            this.rdbOracle.TabStop = true;
            this.rdbOracle.Text = "Oracle 11g";
            this.rdbOracle.UseVisualStyleBackColor = true;
            this.rdbOracle.CheckedChanged += new System.EventHandler(this.rdbOracle_CheckedChanged);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(81, 66);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(136, 14);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "请选择数据库服务器类型:";
            // 
            // wpgConnectParams
            // 
            this.wpgConnectParams.AllowNext = false;
            this.wpgConnectParams.Controls.Add(this.txtDabaseName);
            this.wpgConnectParams.Controls.Add(this.labelControl6);
            this.wpgConnectParams.Controls.Add(this.btnTest);
            this.wpgConnectParams.Controls.Add(this.txtPassword);
            this.wpgConnectParams.Controls.Add(this.txtUserName);
            this.wpgConnectParams.Controls.Add(this.txtServerName);
            this.wpgConnectParams.Controls.Add(this.labelControl5);
            this.wpgConnectParams.Controls.Add(this.labelControl4);
            this.wpgConnectParams.Controls.Add(this.labelControl3);
            this.wpgConnectParams.Controls.Add(this.labelControl2);
            this.wpgConnectParams.DescriptionText = "";
            this.wpgConnectParams.Name = "wpgConnectParams";
            this.wpgConnectParams.Size = new System.Drawing.Size(509, 283);
            this.wpgConnectParams.Text = "第二步：设置数据库连接参数";
            // 
            // txtDabaseName
            // 
            this.txtDabaseName.Location = new System.Drawing.Point(133, 139);
            this.txtDabaseName.Name = "txtDabaseName";
            this.txtDabaseName.Size = new System.Drawing.Size(273, 20);
            this.txtDabaseName.TabIndex = 1;
            // 
            // labelControl6
            // 
            this.labelControl6.Location = new System.Drawing.Point(67, 142);
            this.labelControl6.Name = "labelControl6";
            this.labelControl6.Size = new System.Drawing.Size(60, 14);
            this.labelControl6.TabIndex = 5;
            this.labelControl6.Text = "数据库名：";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(331, 242);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "测试连接";
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(133, 195);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Properties.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(273, 20);
            this.txtPassword.TabIndex = 3;
            // 
            // txtUserName
            // 
            this.txtUserName.Location = new System.Drawing.Point(133, 169);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(273, 20);
            this.txtUserName.TabIndex = 2;
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(133, 109);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(273, 20);
            this.txtServerName.TabIndex = 0;
            // 
            // labelControl5
            // 
            this.labelControl5.Location = new System.Drawing.Point(91, 198);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(36, 14);
            this.labelControl5.TabIndex = 1;
            this.labelControl5.Text = "密码：";
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(79, 172);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(48, 14);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "用户名：";
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(79, 112);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(48, 14);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "服务器：";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(79, 66);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(346, 14);
            this.labelControl2.TabIndex = 0;
            this.labelControl2.Text = "设定数据库的连接参数，按下“测试连接”可以检测参数是否有效。";
            // 
            // DataBaseSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 428);
            this.Controls.Add(this.wzdMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DataBaseSetting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据源设置向导";
            ((System.ComponentModel.ISupportInitialize)(this.wzdMain)).EndInit();
            this.wzdMain.ResumeLayout(false);
            this.wpgDBType.ResumeLayout(false);
            this.wpgDBType.PerformLayout();
            this.wpgConnectParams.ResumeLayout(false);
            this.wpgConnectParams.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDabaseName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassword.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUserName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtServerName.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraWizard.WizardControl wzdMain;
        private DevExpress.XtraWizard.WelcomeWizardPage wpgWelcome;
        private DevExpress.XtraWizard.WizardPage wpgDBType;
        private DevExpress.XtraWizard.WizardPage wpgConnectParams;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.RadioButton rdbSqlServer;
        private System.Windows.Forms.RadioButton rdbOracle;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtPassword;
        private DevExpress.XtraEditors.TextEdit txtUserName;
        private DevExpress.XtraEditors.TextEdit txtServerName;
        private DevExpress.XtraEditors.SimpleButton btnTest;
        private DevExpress.XtraEditors.TextEdit txtDabaseName;
        private DevExpress.XtraEditors.LabelControl labelControl6;
    }
}

