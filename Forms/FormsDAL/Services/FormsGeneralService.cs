using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Infrastructure;
using Infrastructure.Core;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Model.Data;
using Model.Entities;
using FormsDal.Contexts;

namespace FormsDal.Services
{
    public class FormsGeneralService : BaseService
    {

        #region Get

        public async Task<string> IsConnected()
        {

            var results = "Test";
                //await DbContext.FormsStatuses.Select(x => x).ToListAsync();
            

            return results;
        }
        public async Task<UserDetails> GetUserDetails(string userName, string password)
        {
            string EncryptUserPassword = string.Empty;
            EncryptUserPassword = Util.Encrypt(password, AuthOptions.PASSWORDKEY);

            UserDetails userDetails = null;
           
                userDetails = new UserDetails
                {
                    UserId = "111111",
                    UserGuid = Util.CreateGuid().ToString(),
                    Password = EncryptUserPassword,
                    UserFirstName = "administrator",
                    UserLastName = "administrator",
                    UserBusinessPhone = string.Empty,
                    UserMobilePhone = string.Empty,
                    UserNotes = string.Empty,
                    JobTitleGuid = string.Empty,
                    JobTitle = string.Empty,
                    UnitName = string.Empty,
                    UnitGuid = string.Empty,
                    OrgGuid = string.Empty,
                   /* UserAdminPermissionText = GetConvertUserPremission((int)UserEnumPremission.system_administrator).JobTitleName.Trim(),*/
                    UserStatus = "active",
                    UserName = "administrator",
                    UserAdminPermission = (int)UserEnumPremission.system_administrator, // set in user pres code 8 = system_administrator add by moshe

                    UserCreateDate = string.Empty,
                    UserMail = string.Empty,
                    RoleId = 0
                };
           
                //userDetails = DbContext.User
                //               .Where(u => u.UserName == userName && u.UserPassword == EncryptUserPassword)
                //               .Select(u => new UserDetails
                //               {
                //                   UserId = u.UserGuid,
                //                   UserGuid = u.UserGuid,
                //                   Password = u.UserPassword,
                //                   UserFirstName = u.UserFirstName ?? string.Empty,
                //                   UserLastName = u.UserLastName ?? string.Empty,
                //                   UserBusinessPhone = u.UserBusinessPhone ?? string.Empty,
                //                   UserMobilePhone = u.UserMobilePhone ?? string.Empty,
                //                   UserNotes = u.UserNotes ?? string.Empty,
                //                   JobTitleGuid = u.JobTitleGuid ?? string.Empty,
                //                   UnitName = u.UnitGu == null ? string.Empty : u.UnitGu.UnitName,
                //                   UnitGuid = u.UnitGuid ?? string.Empty,
                //                   OrgGuid = u.UnitGuid ?? string.Empty,
                //                   UserAdminPermission = u.UserAdminPermission,
                //                   UserStatus = u.UserStatus ?? string.Empty,
                //                   UserName = u.UserName ?? string.Empty,
                //                   UserCreateDate = u.UserCreateDate ?? string.Empty,
                //                   UserMail = u.UserEmail ?? string.Empty,
                //                   RoleId = u.RoleId,
                //                   RoleName = u.Role.RoleName,
                //                   RoleNameText = u.Role.RoleName,
                //                   RolePermissions = DbContext.RolePermissions
                //                                    .Include(rp => rp.PermissionType)
                //                                    .Where(rp => rp.RoleId == u.RoleId)
                //                                    .Select(x =>
                //                                        new RolePermissionsInfo(x, u.Role.RoleName, x.PermissionType.PermissionTypeName)).ToList(),
                //                   RoleItems = DbContext.RoleItems.Where(r => r.RoleId == u.RoleId).ToList(),

                //                   userPreference = DbContext.UserPreference
                //                                .Where(rp => rp.UserGuid == u.UserGuid).FirstOrDefault()


                //               })
                // .FirstOrDefault();
            

            if (userDetails != null)
                userDetails.OrgName = userDetails.UnitName;

            return await Task.FromResult(userDetails);
        }

        #endregion Get

    }
}
