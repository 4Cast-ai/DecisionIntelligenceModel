using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    //[Serializable]
    public class EventDataObject
    {
        public string ActivityGuid { get; set; }
        public string ActivityName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsLimited { get; set; }
        public bool CanSubmitOnce { get; set; }
        public bool IsAnonymous { get; set; }
        public List<Participant> ParticipantList { get; set; }
        public List<FormEvent> Form { get; set; }
    }
   
}
