using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PKS.Core
{
    /// <summary>�쳣����</summary>
    public class ExceptionModel
    {
        /// <summary>�쳣����</summary>
        public string Code { get; set; }
        /// <summary>�쳣��Ϣ(�����û�)</summary>
        public string Message { get; set; }
        /// <summary>�쳣��ϸ(���������)</summary>
        public string Details { get; set; }
    }
}