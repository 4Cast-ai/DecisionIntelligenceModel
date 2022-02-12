using System;

namespace Model.Data
{
    [Serializable]
    public class ConnectedModelData
    {
        public string model_component_guid { get; set; }
        public string model_component_name { get; set; }
        public ModelComponentTypes model_component_type { get; set; }
    }
}
