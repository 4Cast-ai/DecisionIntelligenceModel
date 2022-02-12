using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ActivityTemplate
    {
        public ActivityTemplate()
        {
            Activity = new HashSet<Activity>();
            AtInFt = new HashSet<AtInFt>();
            ActivityTemplateDescription = new HashSet<ActivityTemplateDescription>();
        }

        public string ActivityTemplateGuid { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int EntityType { get; set; }
        public string CreateDate { get; set; }
        public string ProfessionalRecommendations { get; set; }
        public bool WithinTimeRange { get; set; }
        public bool? SubmitOnlyOnce { get; set; }

        public virtual EntityType EntityTypeId { get; set; }

        public virtual ICollection<Activity> Activity { get; set; }
        public virtual ICollection<AtInFt> AtInFt { get; set; }
        public virtual ICollection<ActivityTemplateDescription> ActivityTemplateDescription { get; set; }

        
    }
}
