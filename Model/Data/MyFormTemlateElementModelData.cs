using System;

namespace Model.Data
{
    [Serializable]
    public partial class MyFormTemlateElementModelData
    {
        public string form_template_guid { get; set; }
        public string model_guid { get; set; }
        public string metric_guid { get; set; }
        public string form_template_element { get; set; }
        public Nullable<double> element__sort_order_in_metric { get; set; }
        public string form_template_element_guid { get; set; }
        public string ModelName { get; set; }
        public string MetricName { get; set; }
    }
}
