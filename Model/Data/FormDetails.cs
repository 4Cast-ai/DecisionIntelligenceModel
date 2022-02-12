using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Model.Data
{
    [Serializable]
    public class FormDetails
    {
        public string form_guid { get; set; }
        public string form_template_guid { get; set; }
        public string activity_guid { get; set; }
        public string name { get; set; }
        public string approve_user_guid { get; set; }
        public string approve_user_name { get; set; }
        public string approve_date { get; set; }
        public int? status { get; set; }
        //public string org_obj_guid { get; set; }
        //public string org_obj_name { get; set; }
      
       
       
        public string entity_guid { get; set; }
        public int entity_type { get; set; }
        public string entity_name { get; set; }
        public int MyProperty { get; set; }
        public string user_guid{ get; set; }
        public string user_name { get; set; }
        public FormDetails() { }

        public FormDetails(Form form, FormTemplate ft, string name, User us = null)
        {
            this.form_guid = form.FormGuid;
            this.form_template_guid = ft.FormTemplateGuid;
            this.activity_guid = form.ActivityGuid;
            this.name = ft.Name;
            this.approve_user_guid = form.ApproveUserGuid;
            this.approve_date = form.ApproveDate;
            this.status = form.Status;
            this.entity_guid = form.EntityGuid;
            this.entity_type = form.EntityType;
            this.entity_name = name;
            //this.org_obj_guid = form.OrgObjGuid;
            //this.org_obj_name = org_obj.Name;
          
            this.user_guid = us == null ? null: us.UserGuid;
            this.user_name = us == null ? null : us.UserFirstName + " " + us.UserLastName;
        }
        public FormDetails(Form form, FormTemplate ft, string name)
        {
            this.form_guid = form.FormGuid;
            this.form_template_guid = ft.FormTemplateGuid;
            this.activity_guid = form.ActivityGuid;
            this.name = ft.Name;
            this.approve_user_guid = form.ApproveUserGuid;
            this.approve_date = form.ApproveDate;
            this.status = form.Status;
            //this.org_obj_guid = form.OrgObjGuid;
            //this.org_obj_name = org_obj.Name;
            this.entity_guid = form.EntityGuid;
            this.entity_type = form.EntityType;
            this.entity_name = name;

        }
    }
}
