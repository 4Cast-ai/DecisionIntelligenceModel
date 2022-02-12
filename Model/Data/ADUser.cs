using System;

namespace Model.Data
{
    public class ADUser
    {
        public int Language { get; set; }
        public string Token { get; set; }
        public string UserGuid { get; set; }
        public string UserID { get; set; }
        public string UserFullName { get; set; }
        public string UserImg { get; set; }
        public string UserJobTitle { get; set; }
        public DateTime UserLastSignIn { get; set; }
    }
}
