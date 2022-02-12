using Model.Entities;
using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class ModelData
    {
        public string model_component_guid { get; set; }
        public int? model_component_type { get; set; }
        public string model_component_origin_name { get; set; }
        public string parent_guid { get; set; }
        public string parent_model_name { get; set; }
        public string name { get; set; }
        public string professional_instruction { get; set; }
        public string model_description_text { get; set; }
        public int source { get; set; }
        public int? metric_source { get; set; }
        public int? status { get; set; }
        public int? model_component_order { get; set; }
        public double? weight { get; set; }
        public string create_date { get; set; }
        public string modified_date { get; set; }
        public string modified_user_guid { get; set; }
        public bool? metric_required { get; set; }
        public int? metric_measuring_unit { get; set; }
        public int? metric_rollup_method { get; set; }
        public int? metric_calender_rollup { get; set; }
        public string metric_formula { get; set; }
        public bool? metric_is_visible { get; set; }
        public bool? metric_not_display_if_irrelevant { get; set; }
        public string metric_expired_period { get; set; }
        public string metric_expired_period_secondary { get; set; }
        public double? metric_comment_obligation_level { get; set; }
        public double? metric_gradual_decrease_precent { get; set; }
        public double? metric_gradual_decrease_period { get; set; }
        public double? metric_minimum_feeds { get; set; }
        public bool? show_origion_value { get; set; }

        public int? TemplateType { get; set; }
        public bool? calcAsSum { get; set; }
        public bool? groupChildren { get; set; }


        //more data
       
        public List<DescriptionsData> description_list { get; set; }
        public List<ConvertionTableData> convertion_table { get; set; }
        public List<MeasuringUnitData> measuring_unit { get; set; }
        public List<CalenderRollupData> calender_rollup { get; set; }
        public List<RollupMethodInfo> rollup_method { get; set; }
        public List<string> model_units_list { get; set; }
        public ModelData()
        {

        }

        public ModelData(ModelComponent mc)
        {
            this.model_component_guid = mc.ModelComponentGuid;
            this.name = mc.Name;
            this.professional_instruction = mc.ProfessionalInstruction;
            this.model_description_text = mc.ModelDescriptionText;
            this.source = mc.Source;
            this.status = mc.Status;
            this.model_component_order = mc.ModelComponentOrder;
            this.weight = mc.Weight;
            this.create_date = mc.CreateDate;
            this.modified_date = mc.ModifiedDate;
            this.modified_user_guid = mc.ModifiedUserGuid;
            this.metric_required = mc.MetricRequired;
            this.metric_measuring_unit = mc.MetricMeasuringUnit;
            this.metric_rollup_method = mc.MetricRollupMethod;
            this.metric_calender_rollup = mc.MetricCalenderRollup;
            this.metric_formula = mc.MetricFormula;
            this.metric_is_visible = mc.MetricIsVisible;
            this.metric_not_display_if_irrelevant = mc.MetricNotDisplayIfIrrelevant;
            this.metric_expired_period = mc.MetricExpiredPeriod;
            this.metric_expired_period_secondary = mc.MetricExpiredPeriodSecondary;
            this.metric_comment_obligation_level = mc.MetricCommentObligationLevel;
            this.metric_gradual_decrease_precent = mc.MetricGradualDecreasePrecent;
            this.metric_gradual_decrease_period = mc.MetricGradualDecreasePeriod;
            this.metric_minimum_feeds = mc.MetricMinimumFeeds;
            this.show_origion_value = mc.ShowOrigionValue;
            this.metric_source = mc.MetricSource;
            this.TemplateType = mc.TemplateType;
            this.calcAsSum = mc.CalcAsSum;
            this.groupChildren = mc.GroupChildren;
        }

        public ModelData(ModelData md)
        {
            this.model_component_guid = md.model_component_guid;
            this.model_component_type = md.model_component_type;
            this.model_component_origin_name = md.model_component_origin_name;
            this.parent_guid = md.parent_guid;
            this.parent_model_name = md.parent_model_name;
            this.name = md.name;
            this.professional_instruction = md.professional_instruction;
            this.model_description_text = md.model_description_text;
            this.source = md.source;
            this.status = md.status;
            this.model_component_order = md.model_component_order;
            this.weight = md.weight;
            this.create_date = md.create_date;
            this.modified_date = md.modified_date;
            this.modified_user_guid = md.modified_user_guid;
            this.metric_required = md.metric_required;
            this.metric_measuring_unit = md.metric_measuring_unit;
            this.metric_rollup_method = md.metric_rollup_method;
            this.metric_calender_rollup = md.metric_calender_rollup;
            this.metric_formula = md.metric_formula;
            this.metric_is_visible = md.metric_is_visible;
            this.metric_not_display_if_irrelevant = md.metric_not_display_if_irrelevant;
            this.metric_expired_period = md.metric_expired_period;
            this.metric_expired_period_secondary = md.metric_expired_period_secondary;
            this.metric_comment_obligation_level = md.metric_comment_obligation_level;
            this.metric_gradual_decrease_precent = md.metric_gradual_decrease_precent;
            this.metric_gradual_decrease_period = md.metric_gradual_decrease_period;
            this.metric_minimum_feeds = md.metric_minimum_feeds;
            this.show_origion_value = md.show_origion_value;
            this.metric_source = md.metric_source;
            this.TemplateType = md.TemplateType;
            this.calcAsSum = md.calcAsSum;
            this.groupChildren = md.groupChildren;
        }
    }
}
