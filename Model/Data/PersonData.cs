using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Data
{
    public class PersonData
    {
        public string PersonGuid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Id { get; set; }
        public int PersonNumber { get; set; }
        public string UnitGuid { get; set; }
        public string UnitName{ get; set; }

        public string DirectManagerGuid { get; set; }
        public string DirectManagerName { get; set; }
        public string professionalManagerGuid { get; set; }
        public string professionalManagerName { get; set; }
        public string JobtitleGuid { get; set; }
        public string JobtitleName { get; set; }
        public int Gender { get; set; }
        public string GenderName { get; set; }
        public DateTime BeginningOfWork { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string Phone1 { get; set; }
        public string Phone2 { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ManagedUnitGuid { get; set; }
        public string ManagedUnitName { get; set; }
        public int? State { get; set; }
        public string StateName { get; set; }
        public int? Country { get; set; }
        public string CountryName { get; set; }
        public string ZipCode { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int StatusGuid { get; set; }
        public string StatusName { get; set; }
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
     
        public int[] DescriptionsGuids { get; set; }
        public DescriptionsData[] DescriptionsData { get; set; }
        public string[] ActivityTemplatesGuids { get; set; }
        public ActivityTemplateDataInfo[] ActivityTemplatesData { get; set; }
        public string[] ModelsGuids { get; set; }
        public ModelData[] ModelsData { get; set; }

    }
}
