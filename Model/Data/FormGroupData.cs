using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class FormGroupData
    {
        public string title { get; set; }
        public string model_component_guid { get; set; }
        public string form_element_guid { get; set; }
        public int? order { get; set; }
        public int? metric_status { get; set; }
        public bool? metric_required { get; set; }
        public int? metric_measuring_unit { get; set; }
        public int? metric_rollup_method { get; set; }
        public bool? metric_not_display_if_irrelevant { get; set; }
        public bool? metric_form_irrelevant { get; set; }
        public bool? metric_show_origion_value { get; set; }
        public string professional_instruction { get; set; }
        public int? form_element_type { get; set; }
        public List<string> connected_model_guid { get; set; }
        public double? score { get; set; }
        public string comment { get; set; }
        public bool showConverTableFlag { get; set; }
        public int source { get; set; }


        public List<FormItemData> items { get; set; }

        public FormGroupData()
        {
            this.items = new List<FormItemData>();
        }

        public FormGroupData(string title, List<FormItemData> items)
        {
            this.title = title;           
        }
        public FormGroupData(FormItemData data, List<FormItemData> items)
        {
            this.title = data.title;
            this.items = items;
            this.model_component_guid = data.model_component_guid;
            this.form_element_guid = data.form_element_guid;
            this.order = data.order;
            this.metric_status = data.metric_status;
            this.metric_required = data.metric_required;
            this.metric_measuring_unit = data.metric_measuring_unit;
            this.metric_rollup_method = data.metric_rollup_method;
            this.metric_not_display_if_irrelevant = data.metric_not_display_if_irrelevant;
            this.metric_form_irrelevant = data.metric_form_irrelevant;
            this.metric_show_origion_value = data.metric_show_origion_value;
            this.professional_instruction = data.professional_instruction;
            this.form_element_type = data.form_element_type;
            this.connected_model_guid = data.connected_model_guid;
            this.score = data.score;
            this.comment = data.comment;
            this.showConverTableFlag = data.showConverTableFlag;
            this.source = data.source;
        }
    }
}
