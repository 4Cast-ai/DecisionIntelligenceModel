using System;
using System.Collections.Generic;

namespace Model.Data
{
    [Serializable]
    public partial class ActivityFileData
    {
        public string ActivityFileGuid { get; set; }
        public string ActivityGuid { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }
    }
}
