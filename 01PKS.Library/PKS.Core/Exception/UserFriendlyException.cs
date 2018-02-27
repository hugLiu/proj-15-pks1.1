using System;
using System.Runtime.Serialization;

namespace PKS.Core
{
    /// <summary>�û��Ѻ��쳣</summary>
    /// <remarks>����ʾ���û����쳣</remarks>
    [Serializable]
    [DataContract]
    public class UserFriendlyException : ApplicationException
    {
        /// <summary>���캯��</summary>
        public UserFriendlyException() { }
        /// <summary>���캯��</summary>
        public UserFriendlyException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
        /// <summary>���캯��</summary>
        public UserFriendlyException(string code, string message, string friendlyMessage)
            : base(message)
        {
            this.Code = code;
            this.FriendlyMessage = friendlyMessage;
        }
        /// <summary>���캯��</summary>
        public UserFriendlyException(Exception innerException, string code, string message, string friendlyMessage)
            : base(message, innerException)
        {
            this.Code = code;
            this.FriendlyMessage = friendlyMessage;
        }
        /// <summary>���캯��</summary>
        public UserFriendlyException(string code, string message, string friendlyMessage, Exception innerException)
            : base(message, innerException)
        {
            this.Code = code;
            this.FriendlyMessage = friendlyMessage;
        }

        /// <summary>���캯��</summary>
        public UserFriendlyException(string code, string friendlyMessage, Exception innerException)
            : base(innerException.Message, innerException)
        {
            this.Code = code;
            this.FriendlyMessage = friendlyMessage;
        }
        /// <summary>�쳣����</summary>
        public string Code { get; set; }
        /// <summary>���û��ο����쳣��Ϣ</summary>
        public string FriendlyMessage { get; set; }
    }
}