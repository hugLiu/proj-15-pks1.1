using Jurassic.CommonModels.Schedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Jurassic.WebTemplate
{
    public class TempMenuExtInfoService : IMenuExtInfoService
    {
        public IEnumerable<MenuExtInfo> GetMenuExtInfos(int userId)
        {
            yield return new MenuExtInfo
            {
                BadgeText = "new",
                MenuId = "stock-list"
            };
            yield return new MenuExtInfo
            {
                BadgeText = DateTime.Now.Second.ToString(),
                MenuId = "suppliers-list"
            };

            yield return new MenuExtInfo
            {
                BadgeText = "new",
                MenuId = "addindemo"
            };

            yield return new MenuExtInfo
            {
                MenuId = "messageNumDemo",
                BadgeText = DateTime.Now.Second.ToString()
            };

            yield return new MenuExtInfo
            {
                MenuId = "signalr-demo",
                BadgeText = DateTime.Now.Millisecond.ToString()
            };
        }
    }
}