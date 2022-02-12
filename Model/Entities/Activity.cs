using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class Activity
    {
        public Activity()
        {
            CalculateScore = new HashSet<CalculateScore>();
            //EstimatedOrganizationObject = new HashSet<EstimatedOrganizationObject>();
            Form = new HashSet<Form>();
            ActivityFile = new HashSet<ActivityFile>();
            Score = new HashSet<Score>();
            ActivityEntity = new HashSet<ActivityEntity>(); 
        }

        public string ActivityGuid { get; set; }
        //public string ActivityGroupGuid { get; set; }
        public string Name { get; set; }
        public string Name1 { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ActivityTemplateGuid { get; set; }
        public bool AnonymousEvaluation { get; set; }
        public string CreateDate { get; set; }
        //public string OrgObjGuid { get; set; }
        public string[] Users { get; set; }
        public int? Status { get; set; }

        //public string EntityGuid { get; set; }
        //public int EntityGuidType { get; set; }
        //public string[] EstimateUnits { get; set; }
        //public string[] EstimatePersons { get; set; }


        //public virtual EntityType EntityGuidTypeId { get; set; }
        public virtual ActivityTemplate ActivityTemplateGu { get; set; }

        //public virtual OrganizationObject OrgObjGu { get; set; }
        public virtual ICollection<CalculateScore> CalculateScore { get; set; }
        //public virtual ICollection<EstimatedOrganizationObject> EstimatedOrganizationObject { get; set; }
        public virtual ICollection<Form> Form { get; set; }
        public virtual ICollection<ActivityFile> ActivityFile { get; set; }
        public virtual ICollection<Score> Score { get; set; }
        public virtual ICollection<ActivityEntity> ActivityEntity { get; set; }
        public virtual ActivityStatus StatusNavigation { get; set; }


    }
}
