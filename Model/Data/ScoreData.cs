using Model.Entities;
using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class ScoreData
    {
        public string model_component_guid { get; set; }
        public string form_element_guid { get; set; }
        public string form_element_title { get; set; }
        //public string org_obj_guid { get; set; }
        //public string org_obj_name { get; set; }
        public string entity_guid { get; set; }
        public int entity_type { get; set; }
        public string entity_name { get; set; }
        public string activity_guid { get; set; }
        public double? original_score { get; set; }
        public double? convertion_score { get; set; }
        public double? calculated_score { get; set; }
        public int? score_level { get; set; }
        public string calculated_date { get; set; }
        public string model_component_comment { get; set; }
        public DateTime activity_end_date { get; set; }
        public bool? is_expired { get; set; }
        public int calender_rollup { get; set; }
        public string form_guid { get; set; }
        public List<ConvertionTableData> convertion_table { get; set; }

        public int formType { get; set; }
        public string AverageScore { get; set; }
        public int? EvaluatingCount { get; set; }
        public string CandidateUnit { get; set; }
        public string CandidateRole { get; set; }
        public string CandidateRank { get; set; }
        public string TextAnswerQuestion { get; set; }
        public string TextAnswerSummary { get; set; }

        public ScoreData() { }

        public ScoreData(Score score, Activity activity)
        {
            this.model_component_guid = score.ModelComponentGuid;
            this.form_element_guid = score.FormElementGuid;
            this.form_element_title = score.FormElementGu != null ? score.FormElementGu.FormElementTitle : null;
            //this.org_obj_guid = score.OrgObjGuid;
            //this.unit_guid = score.ubTODO
            this.activity_guid = score.ActivityGuid;
            this.original_score = score.OriginalScore;
            this.convertion_score = score.ConvertionScore;
            this.model_component_comment = score.ModelComponentComment;
            this.activity_end_date = activity != null ? ConvertStringToDate(activity.EndDate) : DateTime.Now.AddDays(-1);
            this.form_guid = score.FormGuid;
            this.convertion_table = new List<ConvertionTableData>();
        }
        public ScoreData(Score score, Activity activity, string orgName) : this(score, activity)
        {
            //this.org_obj_name = orgName;TODO
        }

        //public ScoreData(Score score, Activity activity, OrganizationObjectData organization)
        //: this(score, activity)
        //{
        //    //this.org_obj_name = organization.name; TODO
        //}

        public ScoreData(CalculateScore score)
        {
            this.model_component_guid = score.ModelComponentGuid;
            this.form_element_guid = score.FormElementGuid;
            //this.org_obj_guid = score.OrgObjGuid;
            //this.un TODO
            this.activity_guid = score.ActivityGuid;
            this.original_score = score.OriginalScore;
            this.calculated_score = score.CalculatedScore;
            this.calculated_date = score.CalculatedDate;
            this.convertion_score = score.ConvertionScore;
            this.model_component_comment = score.ModelComponentComment;
            this.form_guid = score.FormGuid;
            this.convertion_table = new List<ConvertionTableData>();
        }

        public ScoreData(ScoreData itemS)
        {
            this.model_component_guid = itemS.model_component_guid;
            this.form_element_guid = itemS.form_element_guid;
            this.form_element_title = itemS.form_element_title;
            //this.org_obj_guid = itemS.org_obj_guid;TODO
            //this.org_obj_name = itemS.org_obj_name;
            this.activity_guid = itemS.activity_guid;
            this.original_score = itemS.original_score;
            this.convertion_score = itemS.convertion_score;
            this.calculated_score = itemS.calculated_score;
            this.score_level = itemS.score_level;
            this.calculated_date = itemS.calculated_date;
            this.model_component_comment = itemS.model_component_comment;
            this.activity_end_date = itemS.activity_end_date;
            this.is_expired = itemS.is_expired;
            this.calender_rollup = itemS.calender_rollup;
            this.form_guid = itemS.form_guid;
            this.convertion_table = itemS.convertion_table;
            this.formType = itemS.formType;
            this.AverageScore = itemS.AverageScore;
            this.EvaluatingCount = itemS.EvaluatingCount;
            this.CandidateUnit = itemS.CandidateUnit;
            this.CandidateRole = itemS.CandidateRole;
            this.CandidateRank = itemS.CandidateRank;
            this.TextAnswerQuestion = itemS.TextAnswerQuestion;
            this.TextAnswerSummary = itemS.TextAnswerSummary;
        }

        public static DateTime ConvertStringToDate(string strDate)
        {
            DateTime retDate;

            int Year;
            int Month;
            int Day;
            int Hour;
            int Minute;
            int Second;

            try
            {
                Year = int.Parse(strDate.Substring(0, 4));
                Month = int.Parse(strDate.Substring(4, 2));
                Day = int.Parse(strDate.Substring(6, 2));
                Hour = int.Parse(strDate.Substring(8, 2));
                Minute = int.Parse(strDate.Substring(10, 2));
                Second = int.Parse(strDate.Substring(12, 2));

                retDate = new DateTime(Year, Month, Day, Hour, Minute, Second);

                return retDate;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
