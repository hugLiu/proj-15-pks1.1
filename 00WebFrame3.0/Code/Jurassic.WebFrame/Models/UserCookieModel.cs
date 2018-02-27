
using System.Collections.Generic;
using Jurassic.AppCenter;

namespace Jurassic.WebFrame.Models
{
    public enum WidgetPlace
    {
        Left = 0,
        Right = 1
    }

    public class WidgetModel
    {
        public WidgetPlace WidgetColumn { get; set; }
        public string WidgetId { get; set; }
        public string WidgetTitle { get; set; }
        public string WidgetShowCloseButton { get; set; }
        public string WidgetBody { get; set; }
        public string WidgetHeight { get; set; }
        public string RenderUrl { get; set; }
        public string RenderAction { get; set; }
        public string RenderController { get; set; }
        public string Order { get; set; }
    }

    public class UserCookieModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PanelWidth { get; set; }
        public string PanelHeight { get; set; }
        public string PanelIncision { get; set; }

        public IList<WidgetModel> Widgets { get; set; }
    }
}