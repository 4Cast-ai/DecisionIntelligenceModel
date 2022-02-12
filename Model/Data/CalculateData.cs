using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class CalculateData
    {
        public string report_guid { get; set; }
        public ReportTypes report_type { get; set; }
        public string model_component_guid { get; set; }
        public UnitDataInfo org_obj { get; set; }
        public DateTime? calculate_date { get; set; }
        public List<string> activity_list { get; set; }
        public List<string> comment_list { get; set; }
        public List<string> focus_list { get; set; }
        public bool calc_ref { get; set; }
        public List<string> candidatesList { get; set; }
        public string user_guid { get; set; }
        public string report_name { get; set; }
        public string candidateID { get; set; }

        public CalculateData()
        {
            this.activity_list = new List<string>();
            this.comment_list = new List<string>();
            this.focus_list = new List<string>();
        }

        public CalculateData(string report_guid, ReportTypes report_type, string model_component_guid, UnitDataInfo org_obj, DateTime? calculate_date, List<string> activities_list, List<string> comment_list, List<string> focus_list, bool calc_ref, List<string> candidatesList)
        {
            this.report_guid = report_guid;
            this.report_type = report_type;
            this.model_component_guid = model_component_guid;
            this.org_obj = org_obj;
            this.calculate_date = calculate_date;
            this.activity_list = activities_list;
            this.comment_list = comment_list;
            this.focus_list = focus_list;
            this.calc_ref = calc_ref;
            this.candidatesList = candidatesList;
            this.candidateID = candidatesList.Count > 0 ? candidatesList[0] : "";
        }

        public CalculateData(ReportData report_data, UnitDataInfo org_obj, DateTime? calc_date, List<string> _candidatesList = null)
        {
            this.report_guid = report_data.report_guid;
            this.report_type = report_data.report_type;
            this.model_component_guid = report_data.model_component_guid;
            this.org_obj = org_obj;
            this.org_obj.union_guid = report_data.union_guid;
            this.calculate_date = calc_date;
            this.activity_list = report_data.activity_list;
            this.comment_list = report_data.comment_list;
            this.focus_list = report_data.focus_list;
            this.calc_ref = report_data.calc_ref;
            this.candidatesList = _candidatesList;
            this.candidateID = candidatesList.Count > 0 ? candidatesList[0] : "";
        }

        public CalculateData(CalculateData cd)
        {
            this.report_guid = cd.report_guid;
            this.report_type = cd.report_type;
            this.model_component_guid = cd.model_component_guid;
            this.org_obj = cd.org_obj;
            this.calculate_date = cd.calculate_date;
            this.activity_list = cd.activity_list;
            this.comment_list = cd.comment_list;
            this.focus_list = cd.focus_list;
            this.calc_ref = cd.calc_ref;
            this.candidatesList = cd.candidatesList;
            this.user_guid = cd.user_guid;
            this.report_name = cd.report_name;
            this.candidateID = cd.candidateID;
        }
    }
}
