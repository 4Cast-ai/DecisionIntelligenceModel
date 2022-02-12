using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities
{
    public partial class Form
    {
        public Form()
        {
            CalculateScore = new HashSet<CalculateScore>();
            Score = new HashSet<Score>();
        }

        public string FormGuid { get; set; }
        public string FormTemplateGuid { get; set; }
        public string ActivityGuid { get; set; }
        public string ApproveUserGuid { get; set; }
        public string ApproveDate { get; set; }
        public int? Status { get; set; }
        //public string OrgObjGuid { get; set; }
        public string EntityGuid { get; set; }
        public int EntityType { get; set; }
        public string UserInCourse { get; set; }

        public virtual Activity ActivityGu { get; set; }
        public virtual User ApproveUserGu { get; set; }
        public virtual FormTemplate FormTemplateGu { get; set; }
        //public virtual OrganizationObject OrgObjGu { get; set; }
        public virtual FormStatus StatusNavigation { get; set; }
        public virtual User UserInCour { get; set; }
        public virtual EntityType EntityTypeId { get; set; }

        public virtual ICollection<CalculateScore> CalculateScore { get; set; }
        public virtual ICollection<Score> Score { get; set; }
    }
}
