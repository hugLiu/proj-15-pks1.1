using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using PKS.Core;

namespace PKS.MgmtServices.Converters
{
    /// <summary>缩略图转换器</summary>
    public class ThumbnailConverter : ImageConverter, IThumbnailConverter, ISingletonAppService
    {
        /// <summary>获得默认大小</summary>
        public virtual Size DefaultSize { get { return Size.Empty; } }
    }
}