using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Infrastructure.Core;
using Infrastructure.Interfaces;
using Model.Data;
using Model.Entities;

namespace Infrastructure.Models
{
    public class AppUser : IdentityUser<string>, IAppUser
    {
        private static AppUser instance;

        public List<RolePermissionsInfo> RolePermissions { get; set; }
        public List<RoleItems> RoleItems { get; set; }

        private AppUser()
        { }

        public static AppUser Create(UserDetails user)
        {
            instance = new AppUser(user);
            return instance;
        }

        public static AppUser Get()
        {
            return instance;
        }

        public bool IsLogged
        {
            get { return !string.IsNullOrEmpty(SecurityStamp) && UserName != null; }
        }

        private AppUser(UserDetails user)
        {
            Id = user.UserGuid;
            FirstName = user.UserFirstName;
            LastName = user.UserLastName;
            UserName = user.UserName;
            PasswordHash = user.Password;
            UnitGuid = user.UnitGuid;
            UnitName = user.UnitName;
            RoleId = user.RoleId;
            SecurityStamp = Guid.NewGuid().ToString();

            RolePermissions = user.RolePermissions;
            RoleItems = user.RoleItems;
    }

        public bool IsAuthenticated
        {
            get
            {
                var httpContext = GeneralContext.HttpContext;
                ClaimsPrincipal currentUser = httpContext.User;
                return currentUser.Identity.IsAuthenticated;
            }
        }

        public int UserId { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UnitGuid { get; private set; }
        public string UnitName { get; private set; }
        public string Units { get; set; }
        public string FullName { get; set; }
        public string UserTypeName { get; set; }
        public int RoleId { get; set; }

        public string ToStringJson()
        {
            var userData = new
            {
                UserGuid = Id,
                UserId,
                UnitGuid,
                UserName,
                FirstName,
                LastName, // .ToBase64String(),
                UserFullName = FullName,
                RoleId,
                RolePermissions,
                RoleItems
        };
            var result = JsonConvert.SerializeObject(userData);
            return result;

        }
    }
}
