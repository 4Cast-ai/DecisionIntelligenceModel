using Model.Entities;
using System;

namespace Model.Data
{
    [Serializable]
    public class MeasuringUnitData
    {
        public int measuring_unit_id { get; set; }
        public string measuring_unit_name { get; set; }

        public MeasuringUnitData()
        {

        }

        public MeasuringUnitData(MeasuringUnit measuring_unit)
        {
            this.measuring_unit_id = measuring_unit.MeasuringUnitId;
            this.measuring_unit_name = measuring_unit.MeasuringUnitName;
        }
    }
}
