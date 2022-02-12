using System;

namespace Model.Data
{
    [Serializable]
    public class UnitDetails
    {
        public string Unit_Guid { get; set; }
        public string Unit_Type_Guid { get; set; }
        public string Unit_Name { get; set; }
        public string Unit_Type_Name { get; set; }
        public string Unit_Dsec { get; set; }
        public float Unit_ID { get; set; }
        public string Unit_Parent_Guid { get; set; }
        public bool is_estimate_unit { get; set; }
        public string Unit_Polygon_Guid { get; set; }
        public string Polygon_Name { get; set; }
        public string Visible_Unit_Guid { get; set; }
        public string Unit_Create_Date { get; set; }


        public int organization_type { get; set; }
        public int unit_or_echelon { get; set; }
        public Nullable<int> force_type { get; set; }
        public int rate_type { get; set; }
        public Nullable<int> unit_type_definition { get; set; }

    }
}
