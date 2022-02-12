using System;

namespace Model.Data
{
    [Serializable]
    public class FormTextAreaData
    {
        public string FormGuid { get; set; }
        public string FormTemplateElementGuid { get; set; }
        public string FormTextAreaContent { get; set; }
    }
}
