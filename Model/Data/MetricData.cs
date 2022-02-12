using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class MetricData
    {


        private bool _HasChildren;
        private bool _leaf;

        public bool HasChildren
        {
            get { return _HasChildren; }
            set { _HasChildren = value; }
        }

        public bool leaf
        {
            get { return _leaf; }
            set { _leaf = value; }
        }

        public List<MetricCnvData> Metric_Convertion_Table { get; set; }

        private string TranslateMetricMeasuringUnitToDB(string MetricMeasuringUnit)
        {
            if (!string.IsNullOrEmpty(MetricMeasuringUnit))
            {
                switch (MetricMeasuringUnit)
                {
                    case "בוצע/לא בוצע":
                        return "binary";
                    case "כמותי":
                        return "quantitative";
                    case "איכותי(1-5)":
                        return "qualitative";
                    case "אחוזים":
                        return "percentage";
                    default:
                        break;
                }
            }
            return "";
        }
        private string TranslateMetricRollupMethodToDB(string MetricRollupMethod)
        {
            if (!string.IsNullOrEmpty(MetricRollupMethod))
            {
                switch (MetricRollupMethod)
                {
                    case "ממוצע משוקלל":
                        return "weighted_average";
                    case "ממוצע":
                        return "average";
                    case "הגדול ביותר":
                        return "maximum";
                    case "הקטן ביותר":
                        return "minimum";
                    case "סכום":
                        return "sum";
                    case "נוסחת חישוב":
                        return "formula";

                    case "נוסחת חישוב X":
                        return "formulaX";

                    case "נוסחת חישוב תקן":
                        return "formulaT";

                    default:
                        break;
                }
            }
            return "";
        }

        private string TranslateMetricCalenderRollupToDB(string MetricCalenderRollup)
        {
            if (!string.IsNullOrEmpty(MetricCalenderRollup))
            {
                switch (MetricCalenderRollup)
                {
                    case "אחרון":
                        return "last";
                    case "מצטבר":
                        return "cumulative";
                    case "אחרון קובע":
                        return "last_set";
                    case "ממוצע":
                        return "average";
                    case "סכום":
                        return "sum";
                    case "הגדול ביותר":
                        return "biggest";
                    case "הקטן ביותר":
                        return "smallest";
                    default:
                        break;
                }
            }
            return "";
        }

        private string TranslateMetricSourceToDB(string MetricSource)
        {
            if (!string.IsNullOrEmpty(MetricSource))
            {
                switch (MetricSource)
                {
                    case "מדד מוזן מטופס":
                        return "form";
                    case "מדד מחושב":
                        return "calculated";
                    case "מדד מוזן ממערכת":
                        return "data_source";

                    case "מדד ענף":
                        return "branch";

                    default:
                        break;
                }
            }
            return "";
        }

        public void SetData(MetricsDetails Data)
        {
            //if (Data.Metric_Is_Reference == true)
            //    _metric_name = Data.metric_name + "(הפנייה)";
            //else if (Data.Metric_Is_Shortcut == true)
            //    _metric_name = Data.metric_name + "(קיצור דרך)";
            //else
            //    _metric_name = Data.metric_name;

            is_master_branch = (bool)Data.is_master_branch;
            DisplayName = Data.metric_display_name;


            _metric_name = Data.metric_name;




            _metric_guid = Data.Metric_Guid;
            _metric_status = Data.Metric_Status;
            _metric_professional_instruction = Data.Metric_Professional_Instruction;
            _metric_source = Data.Metric_Source;
            _metric_measuring_unit = Data.Metric_Measuring_Unit;
            _metric_rollup_method = Data.Metric_Rollup_Method;
            _metric_calender_rollup = Data.Metric_Calender_Rollup;
            _Metric_Formula = Data.Metric_Formula;




            _Metric_Expired_Period = Data.Metric_Expired_Period;

            _Metric_Catalog_Id = Data.Metric_Catalog_Id;
            _Metric_Obligation_Level = (float)Data.Metric_Obligation_Level;

            In_Level_Order = Data.In_Level_Order;
            _Metric_Model = Data.Metric_Model;
            _Metric_Model_Guid = Data.Metric_Model_Guid;
            _Metric_Is_Reference = (bool)Data.Metric_Is_Reference;

            _In_Level_Order = Data.In_Level_Order;
            _Referenced_From_Model = Data.Referenced_From_Model;
            _Metric_weight = (float)Data.Metric_weight;

            Metric_Rollup_Method_Display = Data.Metric_Rollup_Method_Display;
            _Metric_Source_Display = Data.Metric_Source_Display;

            _Metric_Guid_Father = Data.Metric_Guid_Father;
            _Metric_IsThreshold = Data.Metric_IsThreshold;
            _KPI = Data.KPI;
            _Myid = Data.Metric_Guid + "@" + Data.Metric_Guid_Father + "@" + Data.Referenced_From_Model;
            // _Myid = Data.Metric_Guid + "@" + Data.Metric_Guid_Father;
            metric_not_display_if_irrelevant = (bool)Data.metric_not_display_if_irrelevant;
            if (Data.Metric_Source == "מדד ענף")
            {
                Metric_Minimum_Feeds2 = (float)Data.Metric_Minimum_Feeds;
                Metric_Rollup_Method_Display2 = Data.Metric_Rollup_Method_Display;
            }
            else if (Data.Metric_Source == "מדד מחושב")
            {
                Metric_Minimum_Feeds = (float)Data.Metric_Minimum_Feeds;
                Metric_Measuring_Unit_Display3 = Data.Metric_Measuring_Unit_Display;
            }
            else if (Data.Metric_Source != "מדד מוזן מטופס")
            {
                Metric_Measuring_Unit_Display2 = Data.Metric_Measuring_Unit_Display;
                Metric_Calender_Rollup_Display2 = Data.Metric_Calender_Rollup_Display;
                Metric_Required2 = (bool)Data.Metric_Required;
                Metric_Visible2 = (bool)Data.Metric_Visible;
                if (Data.Metric_Expired_Period.Length > 1)
                {
                    PeriodLatter2 = Data.Metric_Expired_Period.Substring(0, 1);
                    PeriodNumber2 = int.Parse(Data.Metric_Expired_Period.Substring(1));
                }
                Metric_gradual_Decrease_Precent2 = (float)Data.Metric_gradual_Decrease_Precent;
                Metric_Gradual_Decrease_Period2 = (float)Data.Metric_Gradual_Decrease_Period;
                if (Metric_gradual_Decrease_Precent > 0)
                { Isdecrease2 = true; }
            }
            else
            {
                Metric_Measuring_Unit_Display = Data.Metric_Measuring_Unit_Display;
                Metric_Calender_Rollup_Display = Data.Metric_Calender_Rollup_Display;
                Metric_Required = (bool)Data.Metric_Required;
                Metric_Visible = (bool)Data.Metric_Visible;
                if (Data.Metric_Expired_Period.Length > 1)
                {
                    PeriodLatter = Data.Metric_Expired_Period.Substring(0, 1);
                    PeriodNumber = int.Parse(Data.Metric_Expired_Period.Substring(1));
                }
                Metric_gradual_Decrease_Precent = (float)Data.Metric_gradual_Decrease_Precent;
                Metric_Gradual_Decrease_Period = (float)Data.Metric_Gradual_Decrease_Period;
                if (Metric_gradual_Decrease_Precent > 0)
                { Isdecrease = true; }
            }

            HasChildren = (bool)Data.HasChildren;
            leaf = !HasChildren;

        }



        public MetricsDetails GetData()
        {

            MetricsDetails ans = new MetricsDetails();
            ans.Metric_Source = TranslateMetricSourceToDB(_metric_source);

            ans.metric_name = _metric_name;
            ans.Metric_Catalog_Id = _Metric_Catalog_Id;
            ans.Metric_Guid = _metric_guid;
            ans.Metric_Professional_Instruction = _metric_professional_instruction;
            ans.Metric_Obligation_Level = _Metric_Obligation_Level;
            ans.metric_not_display_if_irrelevant = metric_not_display_if_irrelevant;
            ans.Metric_Is_Reference = Metric_Is_Reference;
            ans.Referenced_From_Model = Referenced_From_Model;

            if (ans.Metric_Source == "branch")
            {
                ans.Metric_Measuring_Unit = TranslateMetricMeasuringUnitToDB("אחוזים");
                ans.Metric_Minimum_Feeds = Metric_Minimum_Feeds2;
                ans.Metric_Rollup_Method = TranslateMetricRollupMethodToDB(_Metric_Rollup_Method_Display2);
            }
            else if (ans.Metric_Source == "calculated")
            {
                ans.Metric_Measuring_Unit = TranslateMetricMeasuringUnitToDB(Metric_Measuring_Unit_Display3);
                ans.Metric_Minimum_Feeds = _Metric_Minimum_Feeds;
                ans.Metric_Rollup_Method = TranslateMetricRollupMethodToDB(_Metric_Rollup_Method_Display);
                ans.Metric_Formula = _Metric_Formula;
            }
            else if (ans.Metric_Source != "form")
            {
                ans.Metric_Measuring_Unit = TranslateMetricMeasuringUnitToDB(Metric_Measuring_Unit_Display2);
                ans.Metric_Calender_Rollup = TranslateMetricCalenderRollupToDB(Metric_Calender_Rollup_Display2);
                ans.Metric_Required = Metric_Required2;
                ans.Metric_Expired_Period = PeriodLatter2 + PeriodNumber2;
                ans.Metric_gradual_Decrease_Precent = Metric_gradual_Decrease_Precent2;
                ans.Metric_Gradual_Decrease_Period = Metric_Gradual_Decrease_Period2;
                ans.Metric_Visible = Metric_Visible2;
            }
            else
            {
                ans.Metric_Measuring_Unit = TranslateMetricMeasuringUnitToDB(Metric_Measuring_Unit_Display);
                ans.Metric_Calender_Rollup = TranslateMetricCalenderRollupToDB(Metric_Calender_Rollup_Display);
                ans.Metric_Required = Metric_Required;
                ans.Metric_Expired_Period = PeriodLatter + PeriodNumber;
                ans.Metric_gradual_Decrease_Precent = _Metric_gradual_Decrease_Precent;
                ans.Metric_Gradual_Decrease_Period = _Metric_Gradual_Decrease_Period;
                ans.Metric_Visible = Metric_Visible;
            }
            ans.Metric_weight = _Metric_weight;
            ans.Metric_Guid_Father = _Metric_Guid_Father;
            ans.Metric_Model = _Metric_Model;

            return ans;

        }


        public bool is_master_branch { get; set; }
        string _metric_name;
        string _metric_guid;
        string _metric_status;
        string _metric_professional_instruction;
        string _metric_source;
        string _metric_measuring_unit;
        string _metric_rollup_method;
        string _metric_calender_rollup;
        string _Metric_Formula;
        float _Metric_Gradual_Decrease_Period;
        float _Metric_gradual_Decrease_Precent;
        string _Metric_Expired_Period;
        float _Metric_Minimum_Feeds;
        string _Metric_Catalog_Id;
        float _Metric_Obligation_Level;
        bool _Metric_Visible;
        bool _Metric_Required;
        string _Metric_Model;
        string _Metric_Model_Guid;
        bool _Metric_Is_Reference;
        int _In_Level_Order;
        string _Referenced_From_Model;
        float _Metric_weight;
        string _Metric_Rollup_Method_Display;
        string _Metric_Measuring_Unit_Display;
        string _Metric_Source_Display;
        string _Metric_Calender_Rollup_Display;
        string _Metric_Guid_Father;
        string _Metric_IsThreshold;
        string _KPI;
        string _Myid;
        float _Metric_Minimum_Feeds2;
        string _Metric_Rollup_Method_Display2;
        /* 
         * ListConvention
         * */
        public string KPI { get { return _KPI; } set { _KPI = value; } }
        public string Myid { get { return _Myid; } set { _Myid = value; } }
        public string metric_name { get { return _metric_name; } set { _metric_name = value; } }
        public string metric_guid { get { return _metric_guid; } set { _metric_guid = value; } }
        public string metric_status { get { return _metric_status; } set { _metric_status = value; } }
        public string metric_professional_instruction { get { return _metric_professional_instruction; } set { _metric_professional_instruction = value; } }
        public string metric_source { get { return _metric_source; } set { _metric_source = value; } }
        public string metric_measuring_unit { get { return _metric_measuring_unit; } set { _metric_measuring_unit = value; } }
        public string metric_rollup_method { get { return _metric_rollup_method; } set { _metric_rollup_method = value; } }
        public string metric_calender_rollup { get { return _metric_calender_rollup; } set { _metric_calender_rollup = value; } }
        public string Metric_Formula { get { return _Metric_Formula; } set { _Metric_Formula = value; } }
        public float Metric_Gradual_Decrease_Period { get { return _Metric_Gradual_Decrease_Period; } set { _Metric_Gradual_Decrease_Period = value; } }
        public float Metric_gradual_Decrease_Precent { get { return _Metric_gradual_Decrease_Precent; } set { _Metric_gradual_Decrease_Precent = value; } }
        public string Metric_Expired_Period { get { return _Metric_Expired_Period; } set { _Metric_Expired_Period = value; } }
        public float Metric_Minimum_Feeds { get { return _Metric_Minimum_Feeds; } set { _Metric_Minimum_Feeds = value; } }
        public string Metric_Catalog_Id { get { return _Metric_Catalog_Id; } set { _Metric_Catalog_Id = value; } }
        public float Metric_Obligation_Level { get { return _Metric_Obligation_Level; } set { _Metric_Obligation_Level = value; } }
        public bool Metric_Visible { get { return _Metric_Visible; } set { _Metric_Visible = value; } }
        public bool Metric_Required { get { return _Metric_Required; } set { _Metric_Required = value; } }
        public string Metric_Model { get { return _Metric_Model; } set { _Metric_Model = value; } }
        public string Metric_Model_Guid { get { return _Metric_Model_Guid; } set { _Metric_Model_Guid = value; } }
        public bool Metric_Is_Reference { get { return _Metric_Is_Reference; } set { _Metric_Is_Reference = value; } }
        public int In_Level_Order { get { return _In_Level_Order; } set { _In_Level_Order = value; } }
        public string Referenced_From_Model { get { return _Referenced_From_Model; } set { _Referenced_From_Model = value; } }
        public float Metric_weight { get { return _Metric_weight; } set { _Metric_weight = value; } }
        public string Metric_Rollup_Method_Display { get { return _Metric_Rollup_Method_Display; } set { _Metric_Rollup_Method_Display = value; } }
        public string Metric_Measuring_Unit_Display { get { return _Metric_Measuring_Unit_Display; } set { _Metric_Measuring_Unit_Display = value; } }
        public string Metric_Source_Display { get { return _Metric_Source_Display; } set { _Metric_Source_Display = value; } }
        public string Metric_Calender_Rollup_Display { get { return _Metric_Calender_Rollup_Display; } set { _Metric_Calender_Rollup_Display = value; } }
        public string Metric_Guid_Father { get { return _Metric_Guid_Father; } set { _Metric_Guid_Father = value; } }
        public string Metric_IsThreshold { get { return _Metric_IsThreshold; } set { _Metric_IsThreshold = value; } }

        //OJ 5-6-2014
        // only for view 
        public string PeriodLatter { get; set; }

        public int PeriodNumber { get; set; }
        public bool Isdecrease = false;
        public string DisplayName { get; set; }

        public float Metric_Minimum_Feeds2 { get { return _Metric_Minimum_Feeds2; } set { _Metric_Minimum_Feeds2 = value; } }
        public string Metric_Rollup_Method_Display2 { get { return "ממוצע משוקלל"; } set { _Metric_Rollup_Method_Display2 = "ממוצע משוקלל"; } }

        public string Metric_Measuring_Unit_Display2 { get; set; }
        public string Metric_Measuring_Unit_Display3 { get; set; }
        public string Metric_Calender_Rollup_Display2 { get; set; }
        public bool Metric_Required2 { get; set; }
        public bool Metric_Visible2 { get; set; }
        public string PeriodLatter2 { get; set; }
        public bool Isdecrease2 = false;
        public int PeriodNumber2 { get; set; }
        public float Metric_gradual_Decrease_Precent2 { get; set; }
        public float Metric_Gradual_Decrease_Period2 { get; set; }
        public bool metric_not_display_if_irrelevant { get; set; }

        //********************
    }
}
