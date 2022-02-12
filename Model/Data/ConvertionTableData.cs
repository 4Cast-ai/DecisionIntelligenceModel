using System;

namespace Model.Data
{
    [Serializable]
    public class ConvertionTableData
    {
        public string model_component_guid { get; set; }
        public double level_id { get; set; }
        public double? start_range { get; set; }
        public double? end_range { get; set; }
        public string conversion_table_modified_date { get; set; }
        public string conversion_table_status { get; set; }
        public string conversion_table_create_date { get; set; }
        public double? start_range_score_displayed { get; set; }
        public double? end_range_score_displayed { get; set; }
        public string conversion_table_score_order { get; set; }
        public double? conversion_table_final_score { get; set; }

        public ConvertionTableData()
        {

        }

        public ConvertionTableData(Model.Entities.ConvertionTable ct)
        {
            this.model_component_guid = ct.ModelComponentGuid;
            this.level_id = ct.LevelId;
            this.start_range = ct.StartRange;
            this.end_range = ct.EndRange;
            this.conversion_table_modified_date = ct.ConversionTableModifiedDate;
            this.conversion_table_status = ct.ConversionTableStatus;
            this.conversion_table_create_date = ct.ConversionTableCreateDate;
            this.start_range_score_displayed = ct.StartRangeScoreDisplayed;
            this.end_range_score_displayed = ct.EndRangeScoreDisplayed;
            this.conversion_table_score_order = ct.ConversionTableScoreOrder;
            this.conversion_table_final_score = ct.ConversionTableFinalScore;
        }
    }
}
