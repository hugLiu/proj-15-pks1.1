using System;
using System.Net;
using System.Threading;
using System.IO;

namespace Jurassic.Com.Tools
{
    /// <remarks>王家新, 2014-08-01, 2014-08-01</remarks>
    /// <summary>
    /// FTP上传的帮助类 
    /// </summary>
    public class FtpHelper
    {
        string UserName { get; set; }
        string Password { get; set; }
        string Url { get; set; }
        /// <summary>
        /// 带地址、用户名、密码参数的构造函数
        /// </summary>
        /// <param name="url">FTP地址</param>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        public FtpHelper(string url, string userName, string password)
        {
            Url = url;
            UserName = userName;
            Password = password;
        }
        /// <summary>
        /// 带地址的构造函数
        /// </summary>
        /// <param name="url">FTP地址</param>
        public FtpHelper(string url)
        {
            Url = url;
        }

        /// <summary>
        /// 异步上传，还没写好
        /// </summary>
        /// <param name="target">目标Uri</param>
        /// <param name="fileName">文件名</param>
        public void UploadAsync(Uri target, string fileName)
        {
            // Create a Uri instance with the specified URI string.
            // If the URI is not correctly formed, the Uri constructor
            // will throw an exception.
            ManualResetEvent waitObject;

            FtpState state = new FtpState();
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(target);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            // This example uses anonymous logon.
            // The request is anonymous by default; the credential does not have to be specified. 
            // The example specifies the credential only to
            // control how actions are logged on the server.

            request.Credentials = new NetworkCredential("anonymous", "janeDoe@contoso.com");

            // Store the request in the object that we pass into the
            // asynchronous operations.
            state.Request = request;
            state.FileName = fileName;

            // Get the event to wait on.
            waitObject = state.OperationComplete;

            // Asynchronously get the stream for the file contents.
            request.BeginGetRequestStream(
                new AsyncCallback(EndGetStreamCallback),
                state
            );

            // Block the current thread until all operations are complete.
            waitObject.WaitOne();

            // The operations either completed or threw an exception.
            if (state.OperationException != null)
            {
                throw state.OperationException;
            }
            else
            {
                Console.WriteLine("The operation completed - {0}", state.StatusDescription);
            }
        }

        /// <summary>
        /// 同步上传
        /// </summary>
        /// <param name="fileName">上传的本地文件</param>
        public string Upload(string fileName)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(Url);
            request.Method = WebRequestMethods.Ftp.UploadFile;

            if (!String.IsNullOrEmpty(UserName))
                request.Credentials = new NetworkCredential(UserName, Password);

            // Asynchronously get the stream for the file contents.
            Stream requestStream = request.GetRequestStream();
            FileStream stream = File.OpenRead(fileName);
            const int bufferLength = 2048;
            byte[] buffer = new byte[bufferLength];
            int count = 0;
            int readBytes = 0;
            do
            {
                readBytes = stream.Read(buffer, 0, bufferLength);
                requestStream.Write(buffer, 0, readBytes);
                count += readBytes;
            }
            while (readBytes != 0);

            // IMPORTANT: Close the request stream before sending the request.
            requestStream.Close();

            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream resStream = response.GetResponseStream();
            response.Close();
            string r = response.StatusDescription;
            return r;
            // Signal the main application thread that 
            // the operation is complete.
        }

        private void EndGetStreamCallback(IAsyncResult ar)
        {
            FtpState state = (FtpState)ar.AsyncState;

            Stream requestStream = null;
            // End the asynchronous call to get the request stream.
            try
            {
                requestStream = state.Request.EndGetRequestStream(ar);
                // Copy the file contents to the request stream.
                const int bufferLength = 2048;
                byte[] buffer = new byte[bufferLength];
                int count = 0;
                int readBytes = 0;
                FileStream stream = File.OpenRead(state.FileName);
                do
                {
                    readBytes = stream.Read(buffer, 0, bufferLength);
                    requestStream.Write(buffer, 0, readBytes);
                    count += readBytes;
                }
                while (readBytes != 0);

                // IMPORTANT: Close the request stream before sending the request.
                requestStream.Close();
                // Asynchronously get the response to the upload request.
                state.Request.BeginGetResponse(
                    new AsyncCallback(EndGetResponseCallback),
                    state
                );
            }
            // Return exceptions to the main application thread.
            catch (Exception e)
            {
                Console.WriteLine("Could not get the request stream.");
                state.OperationException = e;
                state.OperationComplete.Set();
                return;
            }

        }

        // The EndGetResponseCallback method  
        // completes a call to BeginGetResponse.
        private static void EndGetResponseCallback(IAsyncResult ar)
        {
            FtpState state = (FtpState)ar.AsyncState;
            FtpWebResponse response = null;
            try
            {
                response = (FtpWebResponse)state.Request.EndGetResponse(ar);
                response.Close();
                state.StatusDescription = response.StatusDescription;
                // Signal the main application thread that 
                // the operation is complete.
                state.OperationComplete.Set();
            }
            // Return exceptions to the main application thread.
            catch (Exception e)
            {
                Console.WriteLine("Error getting response.");
                state.OperationException = e;
                state.OperationComplete.Set();
            }
        }
    }


    /// <summary>
    /// FTP状态类
    /// </summary>
    public class FtpState
    {
        private ManualResetEvent wait;
        private FtpWebRequest request;
        private string fileName;
        private Exception operationException = null;
        string status;
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public FtpState()
        {
            wait = new ManualResetEvent(false);
        }
        /// <summary>
        /// 
        /// </summary>
        public ManualResetEvent OperationComplete
        {
            get { return wait; }
        }
        /// <summary>
        /// 文件传输协议
        /// </summary>
        public FtpWebRequest Request
        {
            get { return request; }
            set { request = value; }
        }
        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        /// <summary>
        /// 执行程序时发生的错误信息
        /// </summary>
        public Exception OperationException
        {
            get { return operationException; }
            set { operationException = value; }
        }
        /// <summary>
        /// 状态描述
        /// </summary>
        public string StatusDescription
        {
            get { return status; }
            set { status = value; }
        }
    }

}
