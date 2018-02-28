using PKS.Models;
using PKS.Web;
namespace PKS.Sooil.Common
{
    public abstract class SoWebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        public string UserName
        {
            get { return this.Context.GetPrincipal().Identity.Name; }

        }

        public bool HasLogin()
        {
            return !string.IsNullOrWhiteSpace(UserName);
        }

        public string LogoutUrl
        {
            get
            {
                return Context.GetSubSystemUrl(PKSSubSystems.Portal) + "/Account/logout";
            }
        }
    }

    public abstract class SoWebViewPage : SoWebViewPage<dynamic>
    {

    }

}