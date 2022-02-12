using System;

namespace Model.Data
{
    [Serializable]
    public class CalenderRollupData
    {
        public int calender_rollup_id { get; set; }
        public string calender_rollup_name { get; set; }

        public CalenderRollupData()
        {

        }

        public CalenderRollupData(Model.Entities.CalenderRollup calender_rollup)
        {
            this.calender_rollup_id = calender_rollup.CalenderRollupId;
            this.calender_rollup_name = calender_rollup.CalenderRollupName;
        }
    }
}
