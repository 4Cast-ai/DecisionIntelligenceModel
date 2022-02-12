using Model.Data;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Events.Models
{
    public class DataModelsInfo
    {
        public ActivityObjInfo ActivityObj { get; set; }

        public class ActivityObjInfo
        {
            public ActivityDetails activity { get; set; }
            public string current_org { get; set; }
            public string activity_template_guid { get; set; }
        }
    }
}
