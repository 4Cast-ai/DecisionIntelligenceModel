using System;

namespace Model.Data
{
    [Serializable]
    public class FormControlDetails
    {
        public string metric_guid { get; set; }
        public string metric_name { get; set; }
        public string model_guid { get; set; }
        public string model_name { get; set; }
        public string metric_measuring_unit { get; set; }

        public override string ToString()
        {
            return "Metric_Guid=" + metric_guid;
        }
    }
}
