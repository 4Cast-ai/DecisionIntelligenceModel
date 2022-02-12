using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormsUser
    {
        public FormsUser()
        {
            FormsDBs = new HashSet<FormsDBTrace>();
        }

        public string UserGuid { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserBusinessPhone { get; set; }
        public string UserMobilePhone { get; set; }
        public string UserNotes { get; set; }
        public string UserModifiedDate { get; set; }
        public string UserStatus { get; set; }
        public string UserCreateDate { get; set; }
        public string UserEmail { get; set; }
        public int UserTypeId { get; set; }

        public virtual FormsUserType UserType { get; set; } = null!;

        public virtual ICollection<FormsDBTrace> FormsDBs { get; set; }
    }
}
