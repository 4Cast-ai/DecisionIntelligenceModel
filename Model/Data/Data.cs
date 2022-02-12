using System;

namespace Model.Data
{
    [Serializable]
    public class Data
    {
        public string id { get; set; }
        public string text { get; set; }
    }

    public class FlatModelData : Data
    {
        public int level { get; set; }
        public int source { get; set; }
        public bool isRef { get; set; }
    }
}
