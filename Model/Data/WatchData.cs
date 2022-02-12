using System;

namespace Model.Data
{
    [Serializable]
    public class WatchData
    {
        public string report_guid { get; set; }
        public int report_type { get; set; }
        public string model_component_guid { get; set; }
        public string name { get; set; }
        public double? score { get; set; }
        public int score_level { get; set; }
        public int order { get; set; }
    }
}
