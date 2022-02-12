using System;

namespace Model.Data
{
    [Serializable]
    public class ValidationAnsData
    {
        public string MetricGuid { get; set; }
        public string MetricName { get; set; }
        public string ValidationError { get; set; }
        public string ModelGuid { get; set; }
        public string ModelName { get; set; }
    }
}
