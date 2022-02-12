using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public class TrendData
    {
        public string model_component_guid { get; set; }
        public string name { get; set; }
        public double? score { get; set; }
        public double? convertion_score { get; set; }
        public string calculated_date { get; set; }
        public bool is_expired { get; set; }
        public int score_level { get; set; }
        public string form_name { get; set; }
        public string form_guid { get; set; }
    }

    public class TrendDataComparer : IEqualityComparer<TrendData>
    {
        public bool Equals(TrendData x, TrendData y)
        {
            return x.calculated_date.Substring(0, 8) == y.calculated_date.Substring(0, 8);
        }

        public int GetHashCode(TrendData obj)
        {
            return obj.calculated_date.GetHashCode();
        }
    }
}
