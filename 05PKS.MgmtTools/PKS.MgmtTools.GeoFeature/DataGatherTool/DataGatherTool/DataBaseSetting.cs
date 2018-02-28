using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using DataGatherTool;
using System.Configuration;
using System.IO;
using System.Security.AccessControl;

namespace Jurassic.GF.DataGatherTool
{
    public partial class DataBaseSetting : Form
    {
        #region 私有成员
        private DataBaseType _dbType;
        private DataBaseSetup _setupObject = new DataBaseSetup();
        private string connStr;
        #endregion

        #region 委托
        public delegate void AsyncDelegate(int callDuration, out int threadId);
        public delegate void SetProgressDelegate(SetupStepEventArgs e);
        #endregion

        public DataBaseSetting()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 尝试连接到数据库
        /// </summary>
        private bool TryConnectDataBase()
        {
            try
            {
                _setupObject.ServerName = txtServerName.Text;
                _setupObject.UserName = txtUserName.Text;
                _setupObject.PassWord = txtPassword.Text;
                _setupObject.DataBaseName = txtDabaseName.Text;
                _setupObject.DbType = _dbType;

                _setupObject.TestConn(out connStr);

                MessageBox.Show("使用" + txtUserName.Text + "连接到数据库" + txtServerName.Text + "成功。", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "连接数据库失败", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
        }



        #region 向导页中控件的事件
        /// <summary>
        /// 选中了rdbOracle的事件
        /// </summary>
        private void rdbOracle_CheckedChanged(object sender, EventArgs e)
        {
            wpgDBType.AllowNext = rdbOracle.Checked;
            _dbType = DataBaseType.Oracle;
        }

        /// <summary>
        /// 选中了rdbSqlServer的事件
        /// </summary>
        private void rdbSqlServer_CheckedChanged(object sender, EventArgs e)
        {
            wpgDBType.AllowNext = rdbSqlServer.Checked;
            _dbType = DataBaseType.SqlServer;
        }

        /// <summary>
        /// txtServerName的TextChanged事件
        /// </summary>
        private void txtServerName_TextChanged(object sender, EventArgs e)
        {
            bool rlt = (txtServerName.Text != "" &&
                        txtUserName.Text != "");

            wpgConnectParams.AllowNext = rlt;
            btnTest.Enabled = rlt;
        }

        /// <summary>
        /// txtUserName的TextChanged事件
        /// </summary>
        private void txtUserName_TextChanged(object sender, EventArgs e)
        {
            bool rlt = (txtServerName.Text != "" &&
                        txtUserName.Text != "");

            wpgConnectParams.AllowNext = rlt;
            btnTest.Enabled = rlt;
        }
        #endregion

        #region 事件





        private void btnTest_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            wpgConnectParams.AllowNext = TryConnectDataBase();
            Cursor = Cursors.Default;
        }


        private void wzdMain_CancelClick(object sender, CancelEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// 完成保存数据库连接到配置文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void wzdMain_FinishClick(object sender, CancelEventArgs e)
        {
            string dbSource = string.Empty;
            if (rdbOracle.Checked)
            {
                dbSource = "Jurassic.GF.Server.Oracle";
            }
            else
            {
                dbSource = "Jurassic.GF.Server.SqlServer";
            }
            UpdateConnectionStringsConfig(dbSource, connStr);
        }

        #endregion

        ///<summary> 
        ///更新连接字符串  
        ///</summary> 
        ///<param name="newName">连接字符串名称</param> 
        ///<param name="newConString">连接字符串内容</param> 
        private static void UpdateConnectionStringsConfig(string FileName,
string ConString)
        {
            addpathPower(Application.StartupPath, "EVERYONE", "FullControl");//添加文件夹权限
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            // 如果连接串已存在，首先删除它  
            config.AppSettings.Settings.Remove("GFDAL");
            config.AppSettings.Settings.Remove("GFSqlConn");
            config.AppSettings.Settings.Add("GFDAL", FileName);
            config.AppSettings.Settings.Add("GFSqlConn", ConString);
            //一定要记得保存，写不带参数的config.Save()也可以
            config.Save();
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static void addpathPower(string pathname, string username, string power)        {            DirectoryInfo dirinfo = new DirectoryInfo(pathname);            if ((dirinfo.Attributes & FileAttributes.ReadOnly) != 0)            {                dirinfo.Attributes = FileAttributes.Normal;            }

            //取得访问控制列表
            DirectorySecurity dirsecurity = dirinfo.GetAccessControl();            switch (power)            {                case "FullControl":                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.FullControl, InheritanceFlags.ContainerInherit, PropagationFlags.InheritOnly, AccessControlType.Allow));                    break;                case "ReadOnly":                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Read, AccessControlType.Allow));                    break;                case "Write":                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Write, AccessControlType.Allow));                    break;                case "Modify":                    dirsecurity.AddAccessRule(new FileSystemAccessRule(username, FileSystemRights.Modify, AccessControlType.Allow));                    break;            }            dirinfo.SetAccessControl(dirsecurity);        }
    }
}
