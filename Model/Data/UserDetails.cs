using Model.Entities;
using System;
using System.Collections.Generic;

namespace Model.Data

{
    [Serializable]
    public class UserDetails
    {
        public string UserId { get; set; }
        public string UserGuid { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserBusinessPhone { get; set; }
        public string UserMobilePhone { get; set; }
        public string UserNotes { get; set; }
        public string JobTitleGuid { get; set; }
        public string JobTitle { get; set; }
        public string UnitName { get; set; }
        public string UnitGuid { get; set; }
        public int UserAdminPermission { get; set; }

        public string UserAdminPermissionText { get; set; }

        public string UserStatus { get; set; }
        public string UserCreateDate { get; set; }
        public string UserMail { get; set; }
        public int UserStatusSaveCode { get; set; }
        public int UserType { get; set; }
        public string UserImg { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleNameText { get; set; }
        public int Language { get; set; }
        public string OrgGuid { get; set; }
        public string OrgName { get; set; }
        public int? PersonalNumber { get; set; }
        public  UserPreference userPreference { get; set; }


        public virtual List<RolePermissionsInfo> RolePermissions { get; set; }
        public virtual List<RoleItems> RoleItems { get; set; }

        public override string ToString()
        {
            return UserName;
        }
        //public string Permission_Type_Name { get; set; }
        //public string Permission_Type_Guid { get; set; }
    }
}
