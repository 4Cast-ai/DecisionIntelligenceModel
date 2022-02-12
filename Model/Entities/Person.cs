using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entities
{
    public partial class Person
    {
        public Person()
        {
            DirectManager = new HashSet<Person>();
            professionalManager = new HashSet<Person>();
            ManagerUnit = new HashSet<Unit>();
        }
        public string PersonGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; set; }
        public int PersonNumber { get; set; }
        public string UnitGuid { get; set; }
        public string DirectManagerGuid { get; set; }
        public string professionalManagerGuid { get; set; }
        public string JobtitleGuid { get; set; }
        public int Gender { get; set; }
        public DateTime BeginningOfWork { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int? State { get; set; }
        public int? Country { get; set; }
        public string ZipCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Status { get; set; }
        public int ChildrenNum { get; set; }
        public string Degree { get; set; }
        public string Institution { get; set; }
        public string Profession { get; set; }
        public bool? Car { get; set; }
        public string Manufactor { get; set; }
        public string PlateNum { get; set; }
        public bool? EducationFund { get; set; }
        public DateTime? LastSalaryUpdate { get; set; }
        public byte[][] Files { get; set; }
        public byte[] Photo { get; set; }

        public virtual Unit UnitGu { get; set; }
        public virtual Person DirectManagerGu { get; set; }
        public virtual Person professionalManagerGu { get; set; }
        public virtual ModelComponent JobtitleGu { get; set; }
        public virtual Gender GenderGu { get; set; }
        public virtual Status StatusGu { get; set; }

        public virtual ICollection<Person> DirectManager { get; set; }
        public virtual ICollection<Person> professionalManager { get; set; }
        public virtual ICollection<Unit> ManagerUnit { get; set; }
        

    }
}
