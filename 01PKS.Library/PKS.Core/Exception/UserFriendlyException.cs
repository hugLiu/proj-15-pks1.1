using System;
using System.Runtime.Serialization;

namespace PKS.Core
{
    /// <summary>用户友好异常</summary>
    /// <remarks>可显示给用户的异常</remarks>
    [Serializable]
    [DataContract]
    public class UserFriendlyException : ApplicationException
    {
        /// <summary>构造函数</summary>
        public UserFriendlyException() { }
        /// <summary>构造函数</summary>
        public UserFriendlyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
        /// <summary>构造函数</summary>
        public UserFriendlyException(string code, string message, string friendlyMessage)
            : base(message)
        {
            this.Code = code;
            this.FriendlyMessage = friendlyMessage;
        }
        /// <summary>构造函数</summary>
        public UserFriendlyException(Exception innerException, string code, string message, string friendlyMessage)
            : base(message, innerException)
        {
            this.Code = code;
            this.FriendlyMessage = friendlyMessage;
        }
        /// <summary>构造函数</summary>
        public UserFriendlyException(string code, string message, string friendlyMessage, Exception innerException)
            : base(message, innerException)
        {
            this.Code = code;
            this.FriendlyMessage = friendlyMessage;
        }

        /// <summary>构造函数</summary>
        public UserFriendlyException(string code, string friendlyMessage, Exception innerException)
            : base(innerException.Message, innerException)
        {
            this.Code = code;
            this.FriendlyMessage = friendlyMessage;
        }
        /// <summary>异常编码</summary>
        public string Code { get; set; }
        /// <summary>给用户参考的异常信息</summary>
        public string FriendlyMessage { get; set; }
    }
}