using System;

namespace Model.Data
{
    [Serializable]
    public class EditingUserData
    {
        public string ModelGuid { get; set; }
        public string UserGuid { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserName { get; set; }
        public bool isEditable { get; set; }

        public override string ToString()
        {
            return "המודל נעול לעריכה ע''י " + UserFirstName + " " + UserLastName + "(" + UserName + ")";
        }
    }
}
