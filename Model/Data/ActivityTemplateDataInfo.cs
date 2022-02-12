using Model.Entities;
using System;

namespace Model.Data
{
    [Serializable]
    public class ActivityTemplateDataInfo
    {
        public string activity_template_guid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int entity_type { get; set; }
        public string create_date { get; set; }
        public string professional_recommendations { get; set; }
        public bool within_time_range { get; set; }
        public bool? submit_only_once { get; set; }


        public ActivityTemplateDataInfo() { }

        public ActivityTemplateDataInfo(ActivityTemplate activity_template)
        {
            this.activity_template_guid = activity_template.ActivityTemplateGuid;
            this.name = activity_template.Name;
            this.description = activity_template.Description;
            this.entity_type = activity_template.EntityType;
            this.create_date = activity_template.CreateDate;
            this.professional_recommendations = activity_template.ProfessionalRecommendations;
            this.within_time_range = activity_template.WithinTimeRange;    
            this.submit_only_once = activity_template.SubmitOnlyOnce;   
        }
    }
}
