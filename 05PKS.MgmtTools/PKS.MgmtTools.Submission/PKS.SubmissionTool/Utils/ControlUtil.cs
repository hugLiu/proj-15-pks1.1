using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface;
using Jurassic.AppCenter.SmartClient.Infrastructure.Interface.Services;
using Microsoft.Practices.CompositeUI;
using PKS.Utils;
using PKS.Core;

namespace PKS.SubmissionTool
{
    /// <summary>控件工具</summary>
    public static class ControlUtil
    {
        /// <summary>开始异步执行</summary>
        public static void BeginExecuteAsync<TView>(this Presenter<TView> presenter, string operation, Action beginAction, Action<bool> endAction)
        {
            var form = presenter.View.As<Form>();
            var controls = new List<Control>();
            DisableControls(form, controls);
            var splash = presenter.WorkItem.Services.Get<ISplashService>();
            var state = new List<object> { form, presenter.WorkItem, splash, endAction, controls };
            beginAction.BeginInvoke(EndExecuteAsync, state);
            splash.Show($"正在{operation},请稍候...");
        }
        /// <summary>禁用控件</summary>
        private static void DisableControls(Control control, List<Control> controls)
        {
            if (control.Controls.Count > 0)
            {
                foreach (Control child in control.Controls)
                {
                    DisableControls(child, controls);
                }
            }
            else if (control is Button || control is CheckBox)
            {
                if (control.Enabled)
                {
                    control.Enabled = false;
                    controls.Add(control);
                }
            }
            else if (control is TextBox)
            {
                var textBox = control.As<TextBox>();
                if (textBox.Enabled && !textBox.ReadOnly)
                {
                    textBox.ReadOnly = true;
                    controls.Add(control);
                }
            }
        }
        /// <summary>启用控件</summary>
        private static void EnableControls(List<Control> controls)
        {
            foreach (var control in controls)
            {
                if (control is TextBox)
                {
                    control.As<TextBox>().ReadOnly = false;
                }
                else
                {
                    control.Enabled = true;
                }
            }
        }
        /// <summary>结束异步执行</summary>
        private static void EndExecuteAsync(IAsyncResult ar)
        {
            var result = (AsyncResult)ar;
            var beginAction = result.AsyncDelegate.As<Action>();
            var state = ar.AsyncState.As<List<object>>();
            try
            {
                beginAction.EndInvoke(ar);
                state.Add(null);
            }
            catch (Exception ex)
            {
                ModuleBootstrapper.Error(beginAction.Target, beginAction.Method.Name, ex);
                state.Add(ex);
            }
            var form = state[0].As<Form>();
            if (form.InvokeRequired)
            {
                Action<object> endActionSync = EndExecuteSync;
                form.BeginInvoke(endActionSync, state);
                return;
            }
            EndExecuteSync(state);
        }
        /// <summary>结束异步执行</summary>
        private static void EndExecuteSync(object asyncState)
        {
            var state = asyncState.As<List<object>>();
            var index = 0;
            var form = state[index++].As<Form>();
            var workItem = state[index++].As<WorkItem>();
            var splash = state[index++].As<ISplashService>();
            var endAction = state[index++].As<Action<bool>>();
            var controls = state[index++].As<List<Control>>();
            var exAction = state[index++].As<Exception>();
            splash.Hide();
            try
            {
                endAction(exAction == null);
                if (exAction != null) workItem.Alert(exAction.GetFullMessage());
            }
            catch (Exception ex)
            {
                ModuleBootstrapper.Error(endAction.Target, endAction.Method.Name, ex);
                workItem.Alert(ex.GetFullMessage());
                if (exAction != null) workItem.Alert(exAction.GetFullMessage());
            }
            finally
            {
                splash.Hide();
                EnableControls(controls);
            }
        }
        /// <summary>验证文本框值</summary>
        public static bool Validate<TView>(this Presenter<TView> presenter, TextBox textbox, ref string value, string errorMessage)
        {
            var txtValue = textbox.Text.Trim();
            if (txtValue.Length == 0)
            {
                presenter.WorkItem.Alert(errorMessage);
                textbox.Focus();
                return false;
            }
            value = txtValue;
            return true;
        }
        /// <summary>验证文本框对应的数值</summary>
        public static bool ValidateInt<TView>(this Presenter<TView> presenter, TextBox textbox, ref int value, string title)
        {
            var txtValue = textbox.Text.Trim();
            if (txtValue.Length == 0)
            {
                presenter.WorkItem.Alert(title + "不允许为空!");
                textbox.Focus();
                return false;
            }
            var result = 0;
            if (!int.TryParse(txtValue, out result))
            {
                presenter.WorkItem.Alert(title + "中包含了无效字符!");
                textbox.Focus();
                return false;
            }
            value = result;
            return true;
        }
        /// <summary>浏览文件夹</summary>
        public static bool BrowseFolder<TView>(this Presenter<TView> presenter, TextBox textbox)
        {
            using (var browser = new FolderBrowserDialog())
            {
                var folder = textbox.Text.Trim();
                if (folder.Length > 0) browser.SelectedPath = folder;
                browser.ShowNewFolderButton = true;
                if (browser.ShowDialog() != DialogResult.OK) return false;
                textbox.Text = browser.SelectedPath;
                return true;
            }
        }
        /// <summary>打开成果文件夹</summary>
        public static void OpenFolder(this TextBox textbox)
        {
            var folder = textbox.Text.Trim();
            if (folder.Length > 0) Process.Start("explorer.exe", folder);
        }
        /// <summary>浏览文件</summary>
        public static bool BrowseFile<TView>(this Presenter<TView> presenter, TextBox textbox)
        {
            using (var browser = new OpenFileDialog())
            {
                var file = textbox.Text.Trim();
                if (file.Length > 0) browser.InitialDirectory = Path.GetDirectoryName(file);
                //browser.CheckFileExists = true;
                if (browser.ShowDialog() != DialogResult.OK) return false;
                textbox.Text = browser.FileName;
                return true;
            }
        }
        /// <summary>打开文件</summary>
        public static void ShellOpenFile(this TextBox textbox)
        {
            var file = textbox.Text.Trim();
            if (file.Length == 0) return;
            var process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = file;
            process.Start();
        }
        /// <summary>打开文件所在位置</summary>
        public static void OpenFileBrowser(this TextBox textbox)
        {
            var file = textbox.Text.Trim();
            if (file.Length > 0) Process.Start("explorer.exe", "/select, \"" + file + "\"");
        }
    }
}
