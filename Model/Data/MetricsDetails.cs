using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Model.Data
{
    [Serializable]
    public class MetricsDetails : INotifyPropertyChanged
    {
        public bool InnerFlag = false;
        public bool? is_master_branch { get; set; }
        public string DisplayName { get; set; }

        public string MetricPrefixLetter { get; set; }

        public bool? metric_not_display_if_irrelevant
        { get; set; }

        private bool? _metric_required;

        public bool? Metric_Required
        {
            get
            {
                return this._metric_required;
            }
            set
            {
                if (this._metric_required != value)
                {
                    this._metric_required = value;
                    this.RaisePropertyChanged("Metric_Required");
                }
            }
        }

        private bool? _metric_reference;
        public bool? Metric_Is_Reference
        {
            get
            {
                return this._metric_reference;
            }
            set
            {
                if (this._metric_reference != value)
                {
                    this._metric_reference = value;
                    this.RaisePropertyChanged("Metric_Is_Reference");
                }
            }
        }

        private bool? _metric_shortcut;
        public bool? Metric_Is_Shortcut
        {
            get
            {
                return this._metric_shortcut;
            }
            set
            {
                if (this._metric_shortcut != value)
                {
                    this._metric_shortcut = value;
                    this.RaisePropertyChanged("Metric_Is_Shortcut");
                }
            }
        }

        private string _referenced_from_model;
        public string Referenced_From_Model
        {
            get
            {
                return this._referenced_from_model;
            }
            set
            {
                if (this._referenced_from_model != value)
                {
                    this._referenced_from_model = value;
                    this.RaisePropertyChanged("Referenced_From_Model");
                }
            }
        }

        private bool? _metric_visible;
        public bool? Metric_Visible
        {
            get
            {
                return this._metric_visible;
            }
            set
            {
                if (this._metric_visible != value)
                {
                    this._metric_visible = value;
                    this.RaisePropertyChanged("Metric_Visible");
                }
            }
        }

        private bool? _metric_copy;
        public bool? Metric_Is_Copy
        {
            get
            {
                return this._metric_copy;
            }
            set
            {
                if (this._metric_copy != value)
                {
                    this._metric_copy = value;
                    this.RaisePropertyChanged("Metric_Is_Copy");
                }
            }
        }

        public bool? HasChildren
        { get; set; }

        private string _metric_guid;
        public string Metric_Guid
        {
            get
            {
                return this._metric_guid;
            }
            set
            {
                if (this._metric_guid != value)
                {
                    this._metric_guid = value;
                    this.RaisePropertyChanged("Metric_Guid");
                }
            }
        }

        private string _metric_name;
        public string metric_name
        {
            get
            {
                return this._metric_name;
            }
            set
            {
                if (this._metric_name != value)
                {
                    this._metric_name = value;
                    this.RaisePropertyChanged("metric_name");
                }
            }
        }

        private string _metric_display_name;
        public string metric_display_name
        {
            get
            {
                return this._metric_display_name;
            }
            set
            {
                if (this._metric_display_name != value)
                {
                    this._metric_display_name = value;
                    this.RaisePropertyChanged("metric_display_name");
                }
            }
        }

        private string _metric_model;
        public string Metric_Model
        {
            get
            {
                return this._metric_model;
            }
            set
            {
                if (this._metric_model != value)
                {
                    this._metric_model = value;
                    this.RaisePropertyChanged("Metric_Model");
                }
            }
        }

        private string _metric_model_guid;
        public string Metric_Model_Guid
        {
            get
            {
                return this._metric_model_guid;
            }
            set
            {
                if (this._metric_model_guid != value)
                {
                    this._metric_model_guid = value;
                    this.RaisePropertyChanged("Metric_Model_Guid");
                }
            }
        }

        private string _metric_source;
        public string Metric_Source
        {
            get
            {
                return this._metric_source;
            }
            set
            {
                if (this._metric_source != value)
                {
                    this._metric_source = value;
                    this.RaisePropertyChanged("Metric_Source");
                }
            }
        }

        private string _metric_source_display;
        public string Metric_Source_Display
        {
            get
            {
                return this._metric_source_display;
            }
            set
            {
                if (this._metric_source_display != value)
                {
                    this._metric_source_display = value;
                    this.RaisePropertyChanged("Metric_Source_Display");
                }
            }
        }

        private string _metric_status;
        public string Metric_Status
        {
            get
            {
                return this._metric_status;
            }
            set
            {
                if (this._metric_status != value)
                {
                    this._metric_status = value;
                    this.RaisePropertyChanged("Metric_Status");
                }
            }
        }

        private string _metric_rollup_method;
        public string Metric_Rollup_Method
        {
            get
            {
                return this._metric_rollup_method;
            }
            set
            {
                if (this._metric_rollup_method != value)
                {
                    this._metric_rollup_method = value;
                    this.RaisePropertyChanged("Metric_Rollup_Method");
                }
            }
        }

        private string _metric_rollup_method_display;
        public string Metric_Rollup_Method_Display
        {
            get
            {
                return this._metric_rollup_method_display;
            }
            set
            {
                if (this._metric_rollup_method_display != value)
                {
                    this._metric_rollup_method_display = value;
                    this.RaisePropertyChanged("Metric_Rollup_Method_Display");
                }
            }
        }

        private string _metric_calender_rollup;
        public string Metric_Calender_Rollup
        {
            get
            {
                return this._metric_calender_rollup;
            }
            set
            {
                if (this._metric_calender_rollup != value)
                {
                    this._metric_calender_rollup = value;
                    this.RaisePropertyChanged("Metric_Calender_Rollup");
                }
            }
        }

        private string _metric_calender_rollup_display;
        public string Metric_Calender_Rollup_Display
        {
            get
            {
                return this._metric_calender_rollup_display;
            }
            set
            {
                if (this._metric_calender_rollup_display != value)
                {
                    this._metric_calender_rollup_display = value;
                    this.RaisePropertyChanged("Metric_Calender_Rollup_Display");
                }
            }
        }

        private string _metric_professional_instruction;
        public string Metric_Professional_Instruction
        {
            get
            {
                return this._metric_professional_instruction;
            }
            set
            {
                if (this._metric_professional_instruction != value)
                {
                    this._metric_professional_instruction = value;
                    this.RaisePropertyChanged("Metric_Professional_Instruction");
                }
            }
        }

        private string _metric_measuring_unit;
        public string Metric_Measuring_Unit
        {
            get
            {
                return this._metric_measuring_unit;
            }
            set
            {
                if (this._metric_measuring_unit != value)
                {
                    this._metric_measuring_unit = value;
                    this.RaisePropertyChanged("Metric_Measuring_Unit");
                }
            }
        }

        private string _metric_measuring_unit_display;
        public string Metric_Measuring_Unit_Display
        {
            get
            {
                return this._metric_measuring_unit_display;
            }
            set
            {
                if (this._metric_measuring_unit_display != value)
                {
                    this._metric_measuring_unit_display = value;
                    this.RaisePropertyChanged("Metric_Measuring_Unit_Display");
                }
            }
        }

        private string _metric_expired_period;
        public string Metric_Expired_Period
        {
            get
            {
                return this._metric_expired_period;
            }
            set
            {
                if (this._metric_expired_period != value)
                {
                    this._metric_expired_period = value;
                    this.RaisePropertyChanged("Metric_Expired_Period");
                }
            }
        }

        private string _metric_description;
        public string Metric_Desc
        {
            get
            {
                return this._metric_description;
            }
            set
            {
                if (this._metric_description != value)
                {
                    this._metric_description = value;
                    this.RaisePropertyChanged("Metric_Desc");
                }
            }
        }

        private string _metric_formula;
        public string Metric_Formula
        {
            get
            {
                return this._metric_formula;
            }
            set
            {
                if (this._metric_formula != value)
                {
                    this._metric_formula = value;
                    this.RaisePropertyChanged("Metric_Formula");
                }
            }
        }

        private float? _metric_gradual_decrease_precent;
        public float? Metric_gradual_Decrease_Precent
        {
            get
            {
                return this._metric_gradual_decrease_precent;
            }
            set
            {
                if (this._metric_gradual_decrease_precent != value)
                {
                    this._metric_gradual_decrease_precent = value;
                    this.RaisePropertyChanged("Metric_gradual_Decrease_Precent");
                }
            }
        }

        private float? _metric_gradual_decrease_period;
        public float? Metric_Gradual_Decrease_Period
        {
            get
            {
                return this._metric_gradual_decrease_period;
            }
            set
            {
                if (this._metric_gradual_decrease_period != value)
                {
                    this._metric_gradual_decrease_period = value;
                    this.RaisePropertyChanged("Metric_Gradual_Decrease_Period");
                }
            }
        }

        private string _metric_catalog_id;
        public string Metric_Catalog_Id
        {
            get
            {
                return this._metric_catalog_id;
            }
            set
            {
                if (this._metric_catalog_id != value)
                {
                    this._metric_catalog_id = value;
                    this.RaisePropertyChanged("Metric_Catalog_Id");
                }
            }
        }

        private float? _metric_minimum_feeds;
        public float? Metric_Minimum_Feeds
        {
            get
            {
                return this._metric_minimum_feeds;
            }
            set
            {
                if (this._metric_minimum_feeds != value)
                {
                    this._metric_minimum_feeds = value;
                    this.RaisePropertyChanged("Metric_Minimum_Feeds");
                }
            }
        }

        private string _metric_threshold;
        public string Metric_IsThreshold
        {
            get
            {
                return this._metric_threshold;
            }
            set
            {
                if (this._metric_threshold != value)
                {
                    this._metric_threshold = value;
                    this.RaisePropertyChanged("Metric_IsThreshold");
                }
            }
        }

        private string kpi;
        public string KPI
        {
            get
            {
                return this.kpi;
            }
            set
            {
                if (this.kpi != value)
                {
                    this.kpi = value;
                    this.RaisePropertyChanged("KPI");
                }
            }
        }

        private float? _metric_weight;
        public float? Metric_weight
        {
            get
            {
                return this._metric_weight;
            }
            set
            {
                if (this._metric_weight != value)
                {
                    this._metric_weight = value;
                    this.RaisePropertyChanged("Metric_weight");
                }
            }
        }

        private string _metric_Guid_Father;
        public string Metric_Guid_Father
        {
            get
            {
                return this._metric_Guid_Father;
            }
            set
            {
                if (this._metric_Guid_Father != value)
                {
                    this._metric_Guid_Father = value;
                    this.RaisePropertyChanged("Metric_Guid_Father");
                }
            }
        }

        private string _metric_Hierarchy;
        public string Metric_Hierarchy
        {
            get
            {
                return this._metric_Hierarchy;
            }
            set
            {
                if (this._metric_Hierarchy != value)
                {
                    this._metric_Hierarchy = value;
                    this.RaisePropertyChanged("Metric_Hierarchy");
                }
            }
        }

        private DateTime _metric_LastUpdDte;
        public DateTime Metric_LastUpdDte
        {
            get
            {
                return this._metric_LastUpdDte;
            }
            set
            {
                if (this._metric_LastUpdDte != value)
                {
                    this._metric_LastUpdDte = value;
                    this.RaisePropertyChanged("Metric_LastUpdDte");
                }
            }
        }

        private List<MetricCnvData> _listConvention;
        public List<MetricCnvData> ListConvention
        {
            get
            {
                return this._listConvention;
            }
            set
            {
                if (this._listConvention != value)
                {
                    this._listConvention = value;
                    this.RaisePropertyChanged("ListConvention");
                }
            }
        }

        private float? _metric_comment_obligation_level;
        public float? Metric_Obligation_Level
        {
            get
            {
                return this._metric_comment_obligation_level;
            }
            set
            {
                if (this._metric_comment_obligation_level != value)
                {
                    this._metric_comment_obligation_level = value;
                    this.RaisePropertyChanged("Metric_Obligation_Level");
                }
            }
        }

        private int _inlevel_order;
        public int In_Level_Order
        {
            get
            {
                return this._inlevel_order;
            }
            set
            {
                if (this._inlevel_order != value)
                {
                    this._inlevel_order = value;
                    this.RaisePropertyChanged("In_Level_Order");
                }
            }
        }

        private int _mylvl;
        public int My_Level
        {
            get
            {
                return this._mylvl;
            }
            set
            {
                if (this._mylvl != value)
                {
                    this._mylvl = value;
                    this.RaisePropertyChanged("My_Level");
                }
            }
        }


        //private bool mark_as_deleted;
        //public bool Mark_As_Deleted
        //{
        //    get
        //    {
        //        return this.mark_as_deleted;
        //    }
        //    set
        //    {
        //        if (this.mark_as_deleted != value)
        //        {
        //            this.mark_as_deleted = value;
        //            this.RaisePropertyChanged("Mark_As_Deleted");
        //        }
        //    }
        //}

        private string _delete_Message;
        public string Delete_Message
        {
            get
            {
                return this._delete_Message;
            }
            set
            {
                if (this._delete_Message != value)
                {
                    this._delete_Message = value;
                    this.RaisePropertyChanged("Delete_Message");
                }
            }
        }

        private MetricDetailsActionEnum metricDetails_Action;
        public MetricDetailsActionEnum MetricDetails_Action
        {
            get
            {
                return this.metricDetails_Action;
            }
            set
            {
                if (this.metricDetails_Action != value)
                {
                    this.metricDetails_Action = value;
                    this.RaisePropertyChanged("MetricDetails_Action");
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }

        public override string ToString()
        {
            return "Metric_Guid=" + Metric_Guid;
        }


        public MetricsDetails Copy()
        {
            return (MetricsDetails)MemberwiseClone();
            //MetricsDetails md = new MetricsDetails();
            //md.KPI = this.KPI;


            //return md;
        }
    }
}
