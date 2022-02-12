using Model.Entities;
using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class CalculateMCData
    {
        public string model_component_guid { get; set; }
        public DateTime? generate_calculate_date { get; set; }
        public int? model_component_type { get; set; }
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
        public double? father_weight { get; set; }
        public double? grandfather_weight { get; set; }
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
        public bool? calcAsSum { get; set; }
        public bool? groupChildren { get; set; }
        public bool is_reference { get; set; }
        public bool is_copy { get; set; }
        public bool is_copy_with_source { get; set; }
        public bool start_build_tree { get; set; }
        public string origin_model_component_guid { get; set; }
        public string base_origin_model_component_guid { get; set; }
        public string[] allUpOrigins { get; set; }
        public string origin_model_component_name { get; set; }
        public int score_level { get; set; }
        public bool is_weakness { get; set; }
        public bool is_origin_threshold { get; set; }
        public bool is_threshold_activated { get; set; }
        public List<ModelComponentData> comment_list { get; set; }
        public List<ModelComponentData> focus_list { get; set; }
        public List<ModelComponentData> weaknesses_list { get; set; }
        public List<ThresholdData> threshold_list { get; set; }
        public List<ThresholdData> origin_threshold_list { get; set; }
        public List<TrendData> trend_list { get; set; }


        public CalculateMCData()
        {
            this.comment_list = new List<ModelComponentData>();
            this.focus_list = new List<ModelComponentData>();
            this.weaknesses_list = new List<ModelComponentData>();
            this.threshold_list = new List<ThresholdData>();
            this.origin_threshold_list = new List<ThresholdData>();
            this.trend_list = new List<TrendData>();
        }

        public CalculateMCData(ModelComponent mc, ModelStructure ms)
        {
            this.model_component_guid = mc.ModelComponentGuid;
            this.name = mc.Name;
            this.professional_instruction = mc.ProfessionalInstruction;
            this.model_description_text = mc.ModelDescriptionText;
            this.source = mc.Source;
            this.metric_source = mc.MetricSource;
            this.status = mc.Status;
            this.model_component_order = mc.ModelComponentOrder;
            this.weight = mc.Weight;
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
            this.calcAsSum = mc.CalcAsSum;
            this.groupChildren = mc.GroupChildren;

            if (ms != null)
            {
                this.model_component_type = ms.ModelComponentType;
                this.parent_guid = ms.ModelComponentParentGuid;
                this.is_reference = ms.ModelComponentType == (int)ModelComponentTypes.reference ? true : false;
                this.is_copy = ms.ModelComponentType == (int)ModelComponentTypes.copy ? true : false;
                this.is_copy_with_source = ms.ModelComponentType == (int)ModelComponentTypes.copyWithSource ? true : false;
                this.origin_model_component_guid = ms.ModelComponentType.HasValue && (ms.ModelComponentType == (int)ModelComponentTypes.reference || ms.ModelComponentType == (int)ModelComponentTypes.copyWithSource) ? ms.ModelComponentOrigionGuid : mc.ModelComponentGuid;
            }

            this.comment_list = new List<ModelComponentData>();
            this.focus_list = new List<ModelComponentData>();
            this.weaknesses_list = new List<ModelComponentData>();
            this.threshold_list = new List<ThresholdData>();
            this.origin_threshold_list = new List<ThresholdData>();
            this.trend_list = new List<TrendData>();
        }

        public CalculateMCData(CalculateMCData md)
        {
            this.model_component_guid = md.model_component_guid;
            this.generate_calculate_date = md.generate_calculate_date;
            this.model_component_type = md.model_component_type;
            this.parent_guid = md.parent_guid;
            this.parent_model_name = md.parent_model_name;

            this.name = md.name;
            this.professional_instruction = md.professional_instruction;
            this.model_description_text = md.model_description_text;
            this.source = md.source;
            this.metric_source = md.metric_source;
            this.status = md.status;
            this.model_component_order = md.model_component_order;
            this.weight = md.weight;
            this.father_weight = md.weight;
            this.grandfather_weight = md.weight;
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
            this.calcAsSum = md.calcAsSum;
            this.groupChildren = md.groupChildren;
            this.is_reference = md.is_reference;
            this.is_copy = md.is_copy;
            this.is_copy_with_source = md.is_copy_with_source;
            this.origin_model_component_guid = md.origin_model_component_guid;
            this.base_origin_model_component_guid = md.base_origin_model_component_guid;
            this.allUpOrigins = md.allUpOrigins;
            this.origin_model_component_name = md.origin_model_component_name;
            this.score_level = md.score_level;
            this.is_weakness = md.is_weakness;
            this.comment_list = md.comment_list;
            this.focus_list = md.focus_list;
            this.weaknesses_list = md.weaknesses_list;
            this.threshold_list = md.threshold_list;
            this.origin_threshold_list = md.origin_threshold_list;
            this.trend_list = md.trend_list;
            this.start_build_tree = md.start_build_tree;
            this.is_origin_threshold = md.is_origin_threshold;
            this.is_threshold_activated = md.is_threshold_activated;
        }
    }
}
