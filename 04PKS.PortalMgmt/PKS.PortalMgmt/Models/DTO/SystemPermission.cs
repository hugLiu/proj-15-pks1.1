using PKS.DbModels;
using PKS.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PKS.PortalMgmt.Models.DTO
{
    public class SystemPermission
    {
        public int Id { get; set; }
        public double? ParentId { get;set; }
        public string Title { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int IsDefault { get; set; }
        public bool IsChecked {get;set;}
        public string PermissionType { get; set; }
    }

    public static class SystemPermissionExtension
    {
        public static PKS_ROLE_PERMISSION ToRolePermission(this SystemPermission sp,WEBPAGES_ROLES role)
        {
            var rp = new PKS_ROLE_PERMISSION();
            rp.RoleId = role.ROLEID;
            rp.PermissionId = sp.Id;
            rp.IsDefault = sp.IsDefault;
            return rp;
        }
    }
}