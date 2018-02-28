using PKS.Utils;

namespace PKS.SubmissionTool
{
    /// <summary>进度属性</summary>
    public class ProgressProperty
    {
        /// <summary>消息</summary>
        public string Message { get; set; }
        /// <summary>最大值</summary>
        public int MaxValue { get; set; }
        /// <summary>内容</summary>
        public string Content { get; set; }
        /// <summary>重置</summary>
        public void Reset()
        {
            this.Message = string.Empty;
            this.Content = string.Empty;
            this.MaxValue = -1;
        }
        /// <summary>克隆</summary>
        public ProgressProperty Clone()
        {
            var clone = this.MemberwiseClone();
            return (ProgressProperty)clone;
        }
        /// <summary>生成方法</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}
