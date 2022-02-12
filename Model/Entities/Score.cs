using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class Score
    {
        public int ScoreId { get; set; }
        public string ModelComponentGuid { get; set; }
        public string FormElementGuid { get; set; }
        //public string OrgObjGuid { get; set; }
        public string EntityGuid { get; set; }
        public int EntityType { get; set; }
        public string ActivityGuid { get; set; }
        public double? OriginalScore { get; set; }
        public double? ConvertionScore { get; set; }
        public string ModelComponentComment { get; set; }
        public int? Status { get; set; }
        public string FormGuid { get; set; }

        public virtual Activity ActivityGu { get; set; }
        public virtual FormElement FormElementGu { get; set; }
        public virtual Form FormGu { get; set; }
        public virtual ModelComponent ModelComponentGu { get; set; }
        //public virtual OrganizationObject OrgObjGu { get; set; }
        public virtual Score ScoreNavigation { get; set; }
        public virtual FormStatus StatusNavigation { get; set; }
        public virtual Score InverseScoreNavigation { get; set; }
        public virtual EntityType EntityTypeId { get; set; }
    }
}
