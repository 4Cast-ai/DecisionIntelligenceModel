using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class Roles
    {
        public Roles()
        {
            User = new HashSet<User>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public string UnitGuid { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateUserId { get; set; }
        public string Status { get; set; }

        public virtual Unit UnitGu { get; set; }
        //public virtual User UpdateUser { get; set; }
        public virtual ICollection<User> User { get; set; }
    }
}
