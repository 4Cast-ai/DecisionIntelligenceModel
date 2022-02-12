using Model.Entities;
using Model.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Data
{
    [Serializable]
    public class ActivityDetails
    {
        public string activity_guid { get; set; }
        //public string org_obj_guid { get; set; }
        //public string entity_guid { get; set; }
        //public int entity_guid_type { get; set; }
        //public string activity_group_guid { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public ActivityTemplateDataInfo activity_template { get; set; }
        public bool anonymousEvaluation { get; set; }
        public bool has_files { get; set; }
        //public List<string> estimates_org_list { get; set; }
        public List<FormDetails> form_list { get; set; }
        public string[] users { get; set; }
        //public string[] EstimateUnits { get; set; }
        //public string[] EstimatePersons { get; set; }


        public ActivityDetails()
        {
            this.activity_template = new ActivityTemplateDataInfo();
            //this.estimates_org_list = new List<string>();
            this.form_list = new List<FormDetails>();
        }

        public ActivityDetails(Activity activity, ActivityTemplate at)
            : this()
        {
            this.activity_guid = activity.ActivityGuid;
            //this.org_obj_guid = activity.OrgObjGuid;
            //this.activity_group_guid = activity.ActivityGroupGuid;
            this.name = activity.Name;
            this.description = activity.Description;
            this.start_date = Util.ConvertStringToDate(activity.StartDate);
            this.end_date = Util.ConvertStringToDate(activity.EndDate);
            this.activity_template = new ActivityTemplateDataInfo(at);
            this.anonymousEvaluation = activity.AnonymousEvaluation;
            //this.entity_guid_type = activity.EntityGuidType;
            //this.entity_guid = activity.EntityGuid;
            //this.EstimateUnits = activity.EstimateUnits;
            //this.EstimatePersons = activity.EstimatePersons;

        }

        //public ActivityDetails(Activity activity, ActivityTemplate at, List<string> eoo)
        //    : this(activity, at)
        //{
        //    this.estimates_org_list = eoo;
        //}
    }
}
