using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ConvertionTable
    {
        public string ModelComponentGuid { get; set; }
        public double LevelId { get; set; }
        public double? StartRange { get; set; }
        public double? EndRange { get; set; }
        public string ConversionTableModifiedDate { get; set; }
        public string ConversionTableStatus { get; set; }
        public string ConversionTableCreateDate { get; set; }
        public double? StartRangeScoreDisplayed { get; set; }
        public double? EndRangeScoreDisplayed { get; set; }
        public string ConversionTableScoreOrder { get; set; }
        public double? ConversionTableFinalScore { get; set; }

        public virtual ModelComponent ModelComponentGu { get; set; }
    }
}
