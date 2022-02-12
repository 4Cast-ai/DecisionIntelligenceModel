using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class ActivityContainerData
    {
        public ActivityDetails ActivityDetails { get; set; }
        public List<ActivityTemplateDataInfo> ActivityTypesList { get; set; }
    }
}
