using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class RolePermissions
    {
        public int RoleId { get; set; }
        public int PermissionTypeId { get; set; }

        public virtual PermissionTypes PermissionType { get; set; }
    }
}
