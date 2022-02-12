using Model.Entities;
using System;

namespace Model.Data
{
    [Serializable]
    public class ActivityData
    {
        public string activity_guid { get; set; }
        //public string activity_group_guid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string activity_template_guid { get; set; }
        public bool AnonymousEvaluation { get; set; }
        public string create_date { get; set; }
        //public string org_obj_guid { get; set; }
        public string[] Users { get; set; }
        //public int entity_guid_type { get; set; }
        //public string entity_guid { get; set; }
        //public string[] EstimateUnits { get; set; }
        //public string[] EstimatePersons { get; set; }
        public ActivityData(Activity activity)
        {
            this.activity_guid = activity.ActivityGuid;
            //this.activity_group_guid = activity.ActivityGroupGuid;
            this.name = activity.Name;
            this.description = activity.Description;
            this.start_date = activity.StartDate;
            this.end_date = activity.EndDate;
            this.activity_template_guid = activity.ActivityTemplateGuid;
            this.create_date = activity.CreateDate;
            this.AnonymousEvaluation = activity.AnonymousEvaluation;
            //this.org_obj_guid = activity.OrgObjGuid;
            this.Users = activity.Users;
            //this.entity_guid_type = activity.EntityGuidType;
            //this.entity_guid = activity.EntityGuid;
            //this.EstimateUnits = activity.EstimateUnits;
            //this.EstimatePersons = activity.EstimatePersons;
        }
    }
}
