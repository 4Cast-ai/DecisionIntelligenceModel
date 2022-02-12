using Model.Entities;
using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class RolePermissionsInfo : RolePermissions
    {
        public RolePermissionsInfo(RolePermissions rp, string roleName, string permissionTypeName)
        {
            RoleId = rp?.RoleId ?? 0;
            RoleName = roleName ?? "None";
            PermissionTypeId = rp?.PermissionTypeId ?? 0;
            PermissionTypeName = permissionTypeName ?? "None";
        }

        public string RoleName { get; set; }
        public string PermissionTypeName { get; set; }
    }

    [Serializable]
    public class RolePermissionsData : Roles
    {
        public RolePermissionsData() : this(null, null)
        {
        }

        public RolePermissionsData(Roles role, IEnumerable<PermissionTypes> permissions = null)
        {
            if (role != null)
            {
                RoleId = role?.RoleId ?? 0;
                RoleName = role?.RoleName ?? "None";
            }
            Permissions = permissions;
        }

        public IEnumerable<PermissionTypes> Permissions { get; set; }
    }
}
