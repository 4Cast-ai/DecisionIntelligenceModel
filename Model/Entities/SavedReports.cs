using System;
using System.Collections.Generic;

namespace Model.Entities
{
    public partial class SavedReports
    {
        public SavedReports()
        {
        }

        public string ReportGuid { get; set; }
        public string UserGuid { get; set; }
        public string CandidateUserGuid { get; set; }

        public string ModelComponentGuid { get; set; }
        public string Name { get; set; }
        public string CalculatedDates { get; set; }
        public bool IsWatch { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsSecondary { get; set; }
        public int ReportType { get; set; }
        public int Order { get; set; }
        public int TemplateType { get; set; }
        public bool IsDefined { get; set; }
        public string UnionGuid { get; set; }


        public virtual ModelComponent ModelComponentGu { get; set; }
        public virtual ReportType ReportTypeNavigation { get; set; }
        public virtual User UserGu { get; set; }
      
    }
}
