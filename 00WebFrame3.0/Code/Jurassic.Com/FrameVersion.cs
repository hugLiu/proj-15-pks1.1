using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.Com
{
    /// <summary>
    /// 标识框架版本的静态类
    /// </summary>
    public sealed class FrameVersion
    {
        public const string Build = "2017";

        public const string CompanyName = "侏罗纪软件";

#if DEBUG
        public const string Configuration = "Debug";
#else
        public const string Configuration = "Release";
#endif
        public const string EndCopyrightYear = "2017";

        public const string MajorMinor = "3.0";

        public const string SatelliteContractVersion = "3.0.0.0";

        public const string Version = "3.0.0.0";
    }
}
