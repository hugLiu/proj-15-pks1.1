using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Jurassic.WebFrame.Providers
{
    /// <remarks></remarks>
    /// <summary>
    /// 安全令牌实体类 
    /// </summary>
    public class AccessToken
    {
        /// <summary>
        /// 
        /// </summary>
        public AccessToken() { }

        #region 通讯验证字段
        /// <summary>
        /// 服务端返回的安全令牌
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 安全令牌类型
        /// </summary>
        public string token_type { get; set; }

        /// <summary>
        /// 安全令牌有效时间
        /// </summary>
        public string expires_in { get; set; }

        /// <summary>
        /// 服务端返回的用于刷新安全令牌的刷新Key
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 是否成功状态
        /// true=成功 false=失败
        /// </summary>
        public bool isSuccessStatusCode { get; set; }

        /// <summary>
        /// 服务端返回的状态编码
        /// </summary>
        public string statusCode { get; set; }

        /// <summary>
        /// 返回的状态消息
        /// </summary>
        public string reasonMessage { get; set; }

        /// <summary>
        /// 安全令牌发布日期
        /// </summary>
        public DateTime? issued { get; set; }

        /// <summary>
        /// 安全令牌有效期截止日期
        /// </summary>
        public DateTime? expires { get; set; }

        #endregion

    }
}
