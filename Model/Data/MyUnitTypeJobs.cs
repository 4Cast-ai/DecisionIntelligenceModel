using System;

namespace Model.Data
{
    [Serializable]
    public class MyUnitTypeJobs
    {
        public string job_title_guid { get; set; }
        public string job_title_name { get; set; }

        public bool IsSelected { get; set; }
    }
}
