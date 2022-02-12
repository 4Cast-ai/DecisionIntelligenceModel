using Model.Entities;
using Model.Helpers;
using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class SavedReportDataInfo
    {
        public string report_guid { get; set; }
        public string user_guid { get; set; }
        public string candidate_user_guid { get; set; }

        public string model_component_guid { get; set; }
        public string name { get; set; }
        public string calculated_dates { get; set; }
        public bool is_watch { get; set; }
        public bool is_primary { get; set; }
        public bool is_secondary { get; set; }
        public bool is_model_edit { get; set; }
        public List<DateTime> calculated_date_list { get; set; }
        public List<string> org_obj_guid_list { get; set; }
        public List<string> comment_list { get; set; }
        public List<string> focus_list { get; set; }
        public int report_type { get; set; }
        public int? TemplateType { get; set; }
        public bool IsDefined { get; set; }
        public string union_guid { get; set; }


        public SavedReportDataInfo()
        {

        }

        public SavedReportDataInfo(SavedReports saved_report, List<string> org_obj_guid_list, List<string> comment_list, List<string> focus_list)
        {
            this.report_guid = saved_report.ReportGuid;
            this.user_guid = saved_report.UserGuid;
            this.candidate_user_guid = saved_report.CandidateUserGuid;
            this.model_component_guid = saved_report.ModelComponentGuid;
            this.name = saved_report.Name;
            this.calculated_dates = saved_report.CalculatedDates;
            this.is_watch = saved_report.IsWatch;
            this.is_primary = saved_report.IsPrimary;
            this.is_secondary = saved_report.IsSecondary;
            this.TemplateType = saved_report.TemplateType;
            this.IsDefined = saved_report.IsDefined;
            this.union_guid = saved_report.UnionGuid;

            this.calculated_date_list = GetDatesAsList(this.calculated_dates);
            this.org_obj_guid_list = org_obj_guid_list;
            this.comment_list = comment_list;
            this.focus_list = focus_list;


            this.report_type = saved_report.ReportType;
           

            //TODO:set is model edit
        }

        public List<DateTime> GetDatesAsList(string calculated_dates)
        {
            List<DateTime> res = new List<DateTime>();

            try
            {
                if (!string.IsNullOrEmpty(calculated_dates))
                {
                    string[] dates = calculated_dates.Split(",");

                    foreach (string date in dates)
                    {
                        if (!string.IsNullOrEmpty(date))
                        {
                            res.Add(Util.ConvertStringToDate(date));

                        }
                    }
                }
            }
            catch
            {
                //
            }

            return res;
        }
    }
}
