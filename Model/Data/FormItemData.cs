using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class FormItemData
    {
        public string model_component_guid { get; set; }
        public string form_element_guid { get; set; }
        public string title { get; set; }
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
        public List<ConvertionTableData> convertion_table { get; set; }

        public int source { get; set; }
        public List<FormItemData> children { get; set; }

        public FormItemData() { }

        public FormItemData(FormItemData form_item)
        {
            this.model_component_guid = form_item.model_component_guid;
            this.form_element_guid = form_item.form_element_guid;
            this.title = form_item.title;
            this.order = form_item.order;

            this.metric_status = form_item.metric_status;
            this.metric_required = form_item.metric_required;
            this.metric_measuring_unit = form_item.metric_measuring_unit;
            this.metric_rollup_method = form_item.metric_rollup_method;
            this.metric_not_display_if_irrelevant = form_item.metric_not_display_if_irrelevant;
            this.metric_form_irrelevant = form_item.metric_form_irrelevant;
            this.metric_show_origion_value = form_item.metric_show_origion_value;
            this.professional_instruction = form_item.professional_instruction;

            this.form_element_type = form_item.form_element_type;
            this.connected_model_guid = form_item.connected_model_guid;

            this.score = form_item.score;
            this.comment = form_item.comment == null ? string.Empty : form_item.comment;
            this.showConverTableFlag = false;
            this.convertion_table = new List<ConvertionTableData>();
            this.source = form_item.source;
            this.children=form_item.children;
        }
    }

    public class FormItemDataMulti: FormItemData
    {
        public List<FormSsoreBaseData> orgs_scores { get; set; }

        public FormItemDataMulti()
        {
            this.orgs_scores = new List<FormSsoreBaseData>();
        }
    }

    public class FormSsoreBaseData
    {
       public string entity_guid { get; set; }
        public int entity_type { get; set; }
        public string model_component_guid { get; set; }
        public double? original_score { get; set; }
        public string model_component_comment { get; set; }
        public bool metric_form_irrelevant { get; set; }
        public int? status { get; set; }
        public string form_guid { get; set; }
        public string activity_guid { get; set; }
    }
}
