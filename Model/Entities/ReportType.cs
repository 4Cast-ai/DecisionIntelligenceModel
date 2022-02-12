using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class ReportType
    {
        public ReportType()
        {
            SavedReports = new HashSet<SavedReports>();
        }

        public int TypeId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<SavedReports> SavedReports { get; set; }
    }
}
