using PKS.Utils;
using System;

namespace PKS.WebAPI.Models
{
    /// <summary>Token过期参数</summary>
    [Serializable]
    public class TokenExpireSettings
    {
        /// <summary>续期间隔</summary>
        public TimeSpan RenewInterval { get; set; }
        /// <summary>过期间隔</summary>
        public TimeSpan ExpireInterval { get; set; }
        /// <summary>解析</summary>
        public static TokenExpireSettings Parse(string settingsValue)
        {
            var settings = settingsValue.Split(',');
            var result = new TokenExpireSettings
            {
                RenewInterval = TimeSpan.FromMinutes(settings[0].ToInt32()),
                ExpireInterval = TimeSpan.FromMinutes(settings[1].ToInt32())
            };
            return result;
        }
        /// <summary>生成JSON串</summary>
        public override string ToString()
        {
            return this.ToJson();
        }
    }
}