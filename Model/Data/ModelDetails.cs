using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class ModelDetails
    {
        public string model_guid { get; set; }
        public string model_id { get; set; }
        public string model_status { get; set; }
        public int source { get; set; }
        public int? model_component_type { get; set; }

        public string model_name { get; set; }
        public int? status { get; set; }
        public string model_description { get; set; }
        public string model_type { get; set; }
        public string model_component_sub_type { get; set; }
        public string model_modified_date { get; set; }

        public List<DescriptionsData> descriptions { get; set; }

        public ModelDetails()
        {
            descriptions = new List<DescriptionsData>();
        }
    }
}
