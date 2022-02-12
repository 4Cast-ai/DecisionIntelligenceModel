using System;

namespace Model.Data
{
    [Serializable]
    public class FormTables
    {
        public string FormGuid { get; set; }
        public string FormTemplateElementGuid { get; set; }
        public byte[] FormTableContent { get; set; }
    }
}
