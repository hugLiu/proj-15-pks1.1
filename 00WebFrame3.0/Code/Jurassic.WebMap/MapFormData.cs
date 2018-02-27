using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace Jurassic.WebMap
{
    public class MapFormData
    {
        public string Id { get; set; }

        public string JsMapName { get; set; }

        public string TextBoxId { get; set; }

        public string HiddenId { get; set; }

        public string Width { get; set; }

        public string Height { get; set; }

        public MapAddress Address { get; set; }

        /// <summary>
        /// 是否允许手工定位
        /// </summary>
        public bool AllowManualPoint { get; set; }


        public MapFormData()
        {
            this.Address = new MapAddress();
        }
    }
}