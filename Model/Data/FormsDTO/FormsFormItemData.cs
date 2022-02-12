using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public class FormsFormItemData
    {
        public string model_component_guid { get; set; }
        public string form_element_guid { get; set; }
        public string title { get; set; }
        public int? order { get; set; }
        public bool? metric_required { get; set; }
        public int? metric_measuring_unit { get; set; }
        public bool? metric_form_irrelevant { get; set; }
        public string professional_instruction { get; set; }
        public int? form_element_type { get; set; }
        public List<string> connected_model_guid { get; set; }
        public double? score { get; set; }
        public string comment { get; set; }
        public bool showConverTableFlag { get; set; }
        public List<ConvertionTableData> convertion_table { get; set; }
        public int source { get; set; }
        public List<FormsFormItemData> children { get; set; }
    }
}
