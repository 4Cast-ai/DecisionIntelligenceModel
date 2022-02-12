using Model.Entities;
using System;

namespace Model.Data
{
    [Serializable]
    public class RolesInfo : Roles
    {
        public RolesInfo() { }

        public RolesInfo(Roles role, string orgName, string userFullName)
        {
            OrgName = orgName;
            RoleId = role.RoleId;
            RoleName = role.RoleName;
            Description = role.Description;
            UnitGuid = role.UnitGuid;
            UpdateDate = role.UpdateDate;
            UpdateUserId = role.UpdateUserId;
            UpdateUserFullName = userFullName;
            Status = role.Status;
            UnitGu = null;
        }
        public string UpdateUserFullName { get; set; }
        public string OrgName { get; set; }
    }
}
