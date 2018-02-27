using PKS.Models;
using System;

namespace PKS.Web
{
    /// <summary>Web操作结果</summary>
    [Serializable]
    public class WebActionResult : OperationResult
    {
        /// <summary>成功应答后的数据</summary>
        public object Data { get; set; }
        /// <summary>错误类型</summary>
        public string ErrorType { get; set; } = "error";
    }
}