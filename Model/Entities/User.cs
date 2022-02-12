using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities
{
    public partial class User
    {
        public User()
        {
            Form = new HashSet<Form>();
            FormTemplate = new HashSet<FormTemplate>();
            ModelComponent = new HashSet<ModelComponent>();
            Roles = new HashSet<Roles>();
            SavedReports = new HashSet<SavedReports>();
            FormCourse = new HashSet<Form>();
        }

        public string UserGuid { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserBusinessPhone { get; set; }
        public string UserMobilePhone { get; set; }
        public string UserNotes { get; set; }
        public string UserModifiedDate { get; set; }
        public string UserStatus { get; set; }
        public string JobTitleGuid { get; set; }
        public string UnitGuid { get; set; }
        public int UserAdminPermission { get; set; }
        public string UserCreateDate { get; set; }
        public string UserEmail { get; set; }
        public int RoleId { get; set; }
        public int UserType { get; set; }

        public virtual Unit UnitGu { get; set; }
        public virtual Roles Role { get; set; }
        public virtual UserType UserTypeNavigation { get; set; }
        //public virtual Candidate Candidate { get; set; }
        public virtual UserPreference UserPreference { get; set; }

        //[InverseProperty("Form")]
        public virtual ICollection<Form> Form { get; set; }
        public virtual ICollection<FormTemplate> FormTemplate { get; set; }
        public virtual ICollection<ModelComponent> ModelComponent { get; set; }
        public virtual ICollection<Roles> Roles { get; set; }
        public virtual ICollection<SavedReports> SavedReports { get; set; }
         public virtual ICollection<Form> FormCourse { get; set; }

    }
}
