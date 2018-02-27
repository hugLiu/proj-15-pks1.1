using System;

namespace PKS.Core
{
    /// <summary>异常编码</summary>
    public enum ExceptionCodes
    {
        /// <summary>缺少参数值</summary>
        MissingParameterValue,
        /// <summary>参数解析失败</summary>
        ParameterParsingFailed,
        /// <summary>无效的枚举值</summary>
        InvalidEnumValue,
        /// <summary>操作未授权</summary>
        OperationUnauthorized,
        /// <summary>操作处理失败</summary>
        OperationProcessingFailed,
        /// <summary>参数不合法</summary>
        InvalidPagerParameter,
        /// <summary>登录失败</summary>
        LoginFailed,
    }
}
