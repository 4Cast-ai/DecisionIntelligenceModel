using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class PermissionTypes
    {
        public PermissionTypes()
        {
            RolePermissions = new HashSet<RolePermissions>();
        }

        public int PermissionTypeId { get; set; }
        public string PermissionTypeName { get; set; }

        public virtual ICollection<RolePermissions> RolePermissions { get; set; }
    }
}
