using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class ModelComponentTreeData
    {
        public ModelData data { get; set; }

        public List<ModelComponentTreeData> children { get; set; }
    }
}
