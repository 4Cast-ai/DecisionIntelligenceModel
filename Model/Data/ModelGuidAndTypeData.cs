using System;

namespace Model.Data
{
    [Serializable]
    public class ModelGuidAndTypeData
    {
        public string model_component_guid { get; set; }
        public string model_component_parent_guid { get; set; }
        public int? model_component_type { get; set; }

        public ModelGuidAndTypeData()
        {

        }

        public ModelGuidAndTypeData(string modelComponentGuid, string modelComponentParentGuid = null, int? modelComponentType = null)
        {
            this.model_component_guid = modelComponentGuid;
            this.model_component_parent_guid = modelComponentParentGuid;
            this.model_component_type = modelComponentType;
        }
    }
}
