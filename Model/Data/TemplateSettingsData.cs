using System;

namespace Model.Data
{
    [Serializable]
    public class TemplateSettingsData
    {
        public int TemplateType { get; set; }
        public string TemplateName { get; set; }
        public int ModelLevel { get; set; }
        public int NumOfChildInLevel2 { get; set; }
        public int NumOfChildInLevel3 { get; set; }
        public int NumOfChildInLevel4 { get; set; }

    }
}
