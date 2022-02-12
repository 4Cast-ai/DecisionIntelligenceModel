using Model.Entities;
using System;

namespace Model.Data
{
    [Serializable]
    public class ModelAndStructure
    {
        public ModelComponent MC { get; set; }
        public ModelStructure MS { get; set; }

        public ModelAndStructure(ModelComponent mc, ModelStructure ms)
        {
            this.MC = mc;
            this.MS = ms;
        }
    }
}
