using System;

namespace Model.Data
{
    [Serializable]
    public class MyJobsUnitTypeData
    {
        public string unit_type_guid { get; set; }
        public string unit_type_name { get; set; }
        public bool IsSelected { get; set; }
    }
}
