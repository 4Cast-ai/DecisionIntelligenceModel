using System.Collections.Generic;

namespace Model.Entities
{
    public partial class Gender
    {
        public Gender()
        {
            PersonGender = new HashSet<Person>();
        }
        public int GenderId { get; set; }
        public string GenderName { get; set; }

        public virtual ICollection<Person> PersonGender { get; set; }
    }
}