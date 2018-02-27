using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Configuration;
using System.ComponentModel;
using System.IO;
using System.Web.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Jurassic.Com.Tools
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// 邮件发送的帮助类
    /// </summary>
    public class SMTPMail
    {
        /// <summary>
        /// 发件人
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public String To { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        public string CC { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 附件文件
        /// </summary>
        public String[] Files { get; set; }

        /// <summary>
        /// 邮件服务器
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 发件人账号的密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 默认发件人
        /// </summary>
        public string DefaultFrom { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 创建一个邮件发送类
        /// </summary>
        public SMTPMail() { GetAccount(); }


        /// <summary>
        /// 异步发送时发送完成事件委托
        /// </summary>
        public AsyncCompletedEventHandler SendCompleted { get; set; }

        /// <summary>
        /// 邮件异步发送时传送给发送完成时事件的对象
        /// </summary>
        public object UserState { get; set; }

        /// <summary>
        /// 获取账号信息
        /// </summary>
        void GetAccount()
        {
            StringSpliter hp = new StringSpliter(ConfigurationManager.AppSettings["MailSettings"], ";", "=");

            Server = hp["Server"];
            Port = hp["Port"].ToInt();
            UserName = hp["UserName"];
            Password = hp["Password"];
            DefaultFrom = hp["From"];

            if (Port == 0) Port = 25;
        }

        /// <summary>
        /// 根据发送地址等信息，构造新的邮件工具类
        /// </summary>
        /// <param name="from">发送地址</param>
        /// <param name="to">到达地址</param>
        /// <param name="subject">标题</param>
        /// <param name="body">正文</param>
        /// <param name="files">附件文件名列表</param>
        public SMTPMail(string from, string to, string cc, string subject, string body, params string[] files)
            : this()
        {
            int idx = DefaultFrom.IndexOf('<');
            if ((from ?? "").IndexOf('@') > 0)
            {
                From = from;
            }
            else if (idx > 0)
            {
                From = from + DefaultFrom.Substring(idx);
            }
            To = to;
            CC = cc;
            Subject = subject;
            Body = body;
            Files = files;
        }

        /// <summary>
        /// 生成SMTPMail,根据发送到,主题和内容
        /// </summary>
        /// <param name="to">发送目的邮件地址</param>
        /// <param name="subject">主题</param>
        /// <param name="body">邮件正文</param>
        public SMTPMail(string to, string subject, string body)
            : this()
        {
            From = DefaultFrom;
            To = to;
            Subject = subject;
            Body = body;
            Files = null;
        }

        /// <summary>
        /// 生成邮件信息对象
        /// </summary>
        /// <returns>邮件信息对象</returns>
        protected System.Net.Mail.MailMessage MakeMessage()
        {
            //MailMessage message = new MailMessage(From, To, Subject, Body);
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new MailAddress(From);
            string[] emails = To.Replace(',', ';').Split(';');
            foreach (string email in emails)
            {
                if (!string.IsNullOrEmpty(email) && CommOp.IsEmail(email))
                {
                    message.To.Add(email);
                }
            }
            CC = CC ?? "";
            emails = CC.Replace(',', ';').Split(';');
            foreach (string email in emails)
            {
                if (!string.IsNullOrEmpty(email) && CommOp.IsEmail(email))
                {
                    message.CC.Add(email);
                }
            }

            message.Subject = Subject;
            //message.SubjectEncoding = Encoding.UTF8;
            message.Body = Body;
            //message.BodyEncoding = Encoding.UTF8;

            //添加文件附件
            if (!Files.IsEmpty())
            {
                foreach (string file in Files)
                {
                    Attachment att = new Attachment(file);

                    //att.NameEncoding = Encoding.UTF8;
                    //att.Name = new FileInfo(file).Name;

                    message.Attachments.Add(att);
                }
            }
            message.IsBodyHtml = true;
            return message;
        }

        /// <summary>
        /// 异步发送邮件
        /// </summary>
        public void SendAsync()
        {
            System.Net.Mail.MailMessage message = MakeMessage();
            ErrorMessage = String.Empty;

            SmtpClient client = new SmtpClient(Server);
            client.Port = Port;
            client.Credentials = new NetworkCredential(UserName, Password);
            client.SendCompleted += new SendCompletedEventHandler(client_SendCompleted);

            client.DeliveryMethod = SmtpDeliveryMethod.Network;

            try
            {
                client.SendAsync(message, UserState);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        //// <summary>
        /// 组织邮件内容并同步发送邮件
        /// </summary>
        public void Send()
        {
            System.Net.Mail.MailMessage message = MakeMessage();
            ErrorMessage = String.Empty;
            SmtpClient client = new SmtpClient(Server);
            client.Port = Port;
            client.Credentials = new NetworkCredential(UserName, Password);
            try
            {
                client.Send(message);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
            finally
            {
                foreach (Attachment att in message.Attachments)
                {
                    att.Dispose();
                }
            }
        }

        /// <summary>
        /// 在Send()方法有问题时，用这个方法替换
        /// </summary>
        public void SendWebMail()
        {
            System.Web.Mail.MailMessage mail = new System.Web.Mail.MailMessage();
            mail.From = From;
            mail.To = String.Join(";", To.Replace(',', ';').Split(';'));
            mail.Body = Body;
            mail.Cc = CC;
            mail.Subject = Subject;
            mail.Priority = System.Web.Mail.MailPriority.High;
            mail.BodyFormat = System.Web.Mail.MailFormat.Html;
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", Port);
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", UserName);
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", Password);
            if (Files != null)
            {
                foreach (string file in Files)
                {
                    MailAttachment MyAttachment = new MailAttachment(file);
                    mail.Attachments.Add(MyAttachment);
                }
            }
            System.Web.Mail.SmtpMail.SmtpServer = Server;

            try
            {
                System.Web.Mail.SmtpMail.Send(mail);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.GetType().Name + ":" + ex.Message;
            }
        }

        /// <summary>
        /// 异步发信时发完以后的操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void client_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (SendCompleted != null)
            {
                SendCompleted(sender, e);
            }
        }
    }
}