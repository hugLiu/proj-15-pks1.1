using System;

namespace PKS.Models
{
    /// <summary>操作结果</summary>
    [Serializable]
    public class OperationResult
    {
        /// <summary>就否成功</summary>
        public bool Succeed { get; set; }
        /// <summary>错误信息</summary>
        public string ErrorMessage { get; set; }
    }
}