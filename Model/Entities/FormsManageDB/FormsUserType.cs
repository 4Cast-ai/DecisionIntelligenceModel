using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormsUserType
    {
        public FormsUserType()
        {
            FormsUsers = new HashSet<FormsUser>();
        }

        public int UserTypeId { get; set; }
        public string TypeName { get; set; } = null!;

        public virtual ICollection<FormsUser> FormsUsers { get; set; }
    }
}
