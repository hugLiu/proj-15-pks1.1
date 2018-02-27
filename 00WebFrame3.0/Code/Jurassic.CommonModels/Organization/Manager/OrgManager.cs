using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jurassic.CommonModels.Organization
{
    public class OrgManager
    {

        /// <summary>
        /// 组织机构数据访问对象
        /// </summary>
        private static OrganizationManager organizationManager;
        public static OrganizationManager mOrganizationManager
        {
            get
            {
                if (organizationManager == null)
                {
                    organizationManager = SiteManager.Get<OrganizationManager>();
                }
                return organizationManager;
            }
        }


       


    }
}
