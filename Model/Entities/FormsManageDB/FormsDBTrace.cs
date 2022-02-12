using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class FormsDBTrace
    {
        public FormsDBTrace()
        {
            UserGus = new HashSet<FormsUser>();
        }

        public int FormsDBID { get; set; }
        public string FormsDBName { get; set; } = null!;
        public int FormsStatusID { get; set; }
        public int FormsTypeID { get; set; }
        public DateTime InsertDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string ActivityGuid { get; set; } = null!;

        public virtual FormsStatus FormsStatus { get; set; } = null!;
        public virtual FormsType FormsType { get; set; } = null!;

        public virtual ICollection<FormsUser> UserGus { get; set; }
    }
}
