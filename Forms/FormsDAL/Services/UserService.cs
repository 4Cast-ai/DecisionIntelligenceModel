using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Model.Data;
using Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace FillerDal.Services
{
    public class UserService : BaseService
    {
        private ModelService _modelService => this.GetChildService<ModelService>();
        private ReportService _reportService => this.GetChildService<ReportService>();

        #region Role/Permissions

        public async Task<List<RolesInfo>> GetRoles(int? roleId = null)
        {
            List<RolesInfo> results = null;

            DbContext.Roles.Include(x => x.OrgObjGu);

            if (roleId.HasValue && roleId > 0 && roleId < 8)
            {
                var roleItems = DbContext.RoleItems.Where(p => p.RoleId == roleId).Select(x => x.RoleItemId).ToList();
                if (roleItems.Any())
                {
                    results = DbContext.Roles.Where(x => roleItems.Contains(x.RoleId))
                               .Select(role => new RolesInfo(role, role.OrgObjGu.Name, ""))
                               .ToList();
                }
            }
            else
            {
                results = DbContext.Roles
                           .Select(role => new RolesInfo(role, role.OrgObjGu.Name, ""))
                           .ToList();
            }
            return await Task.FromResult(results);
        }

        public async Task<List<RolePermissionsInfo>> GetRolePermissions(int? roleId = null)
        {
            var resuls = await (from role in DbContext.Roles.Where(x => roleId.HasValue ? x.RoleId == roleId : true)
                      join rp in DbContext.RolePermissions on role.RoleId equals rp.RoleId
                      join pt in DbContext.PermissionTypes on rp.PermissionTypeId equals pt.PermissionTypeId into rolePermissionsList
                      from x in rolePermissionsList.DefaultIfEmpty()
                      select new RolePermissionsInfo(rp, role.RoleName, x.PermissionTypeName))
                      .ToListAsync();
            return resuls;
        }

        public async Task<List<UserDetails>> GetPermissions()
        {
            var users = DbContext.User
                        .Where(u => u.UserStatus == "active" && (u.UserModifiedDate == null || u.UserModifiedDate == string.Empty))
                        .Select(u => new
                        {
                            UserId = u.UserId,
                            UserGuid = u.UserGuid,
                            Password = u.UserPassword,
                            UserFirstName = u.UserFirstName,
                            UserLastName = u.UserLastName,
                            UserBusinessPhone = u.UserBusinessPhone,
                            UserMobilePhone = u.UserMobilePhone,
                            UserNotes = u.UserNotes,
                            JobTitleGuid = u.JobTitleGuid,
                            UnitName = u.OrgObjGu.Name,
                            UnitGuid = u.OrgObjGuid,
                            UserAdminPermission = u.UserAdminPermission,
                            UserStatus = u.UserStatus,
                            UserName = u.UserName,
                            UserCreateDate = u.UserCreateDate,
                            UserMail = u.UserEmail,
                            u.RoleId,
                            u.Role
                        });

            List<UserDetails> results = new List<UserDetails>();
            foreach (var u in users)
            {
                results.Add(new UserDetails()
                {
                    UserId = u.UserId,
                    UserGuid = u.UserGuid,
                    Password = u.Password,
                    UserFirstName = u.UserFirstName,
                    UserLastName = u.UserLastName,
                    UserBusinessPhone = u.UserBusinessPhone,
                    UserMobilePhone = u.UserMobilePhone,
                    UserNotes = u.UserNotes,
                    JobTitleGuid = u.JobTitleGuid,
                    UnitName = u.UnitName,
                    UnitGuid = u.UnitGuid,
                    UserAdminPermissionText = GetConvertUserPremission(u.UserAdminPermission).JobTitleName.Trim(),
                    UserAdminPermission = u.UserAdminPermission,
                    UserStatus = u.UserStatus,
                    UserName = u.UserName,
                    UserCreateDate = u.UserCreateDate,
                    UserMail = u.UserMail,
                    RoleId = u.RoleId,
                    RoleNameText = u.Role.RoleName
                });
            }

            return await Task.FromResult(results);
        }

        public SystemJobTitles GetConvertUserPremission(int user_admin_permission)
        {
            var sys_job_title = DbContext.SystemJobTitles
                                 .Where(u => u.UserAdminPermission == user_admin_permission)
                                 .FirstOrDefault();
            return sys_job_title;
        }

        #endregion Role/Permissions

        #region Users

        public async Task<List<UserType>> GetUsersTypes()
        {
            var results = await DbContext.UserType.ToListAsync();
            return results;
        }

        public async Task<List<UserDetails>> GetUsers(string userGuid = null)
        {
            List<UserDetails> results = default;
            List<int> roleItems = null;
            if (userGuid != null)
            {
                var user = DbContext.User.FirstOrDefault(u => u.UserGuid == userGuid);
                if (user != null && user.RoleId > 0 && user.RoleId < 8)
                    roleItems = DbContext.RoleItems.Where(p => p.RoleId == user.RoleId).Select(x => x.RoleItemId).ToList();
            }

            results = (from u in DbContext.User
                       join su in DbContext.SystemJobTitles on u.UserAdminPermission equals su.UserAdminPermission
                       where roleItems != null ? roleItems.Contains(u.RoleId) : true
                       select new
                       {
                           u = new UserDetails
                           {
                               UserId = u.UserId,
                               UserGuid = u.UserGuid,
                               Password = u.UserPassword,
                               UserFirstName = u.UserFirstName,
                               UserLastName = u.UserLastName,
                               UserBusinessPhone = u.UserBusinessPhone,
                               UserMobilePhone = u.UserMobilePhone,
                               UserNotes = u.UserNotes,
                               JobTitleGuid = u.JobTitleGuid,
                               UnitName = u.OrgObjGu.Name,
                               UnitGuid = u.OrgObjGuid,
                               OrgGuid = u.OrgObjGuid,
                               UserAdminPermissionText = su.JobTitleName.Trim(),
                               UserAdminPermission = u.UserAdminPermission,
                               UserStatus = u.UserStatus,
                               UserName = u.UserName,
                               UserCreateDate = u.UserCreateDate,
                               UserMail = u.UserEmail,
                               UserType = u.UserType,
                               RoleId = u.RoleId,
                               RoleName = u.Role.RoleName,
                               RoleNameText = u.Role.RoleName,
                               RolePermissions = DbContext.RolePermissions
                                                .Where(rp => rp.RoleId == u.RoleId)
                                                .Select(x => new RolePermissionsInfo(x, u.Role.RoleName, "")).ToList()
                           },
                       })
                        .ToList()
                        .Select(x =>
                        {
                            Random ran = new Random();
                            int index = ran.Next(1, 50);

                            x.u.UserImg = $"https://randomuser.me/api/portraits/men/{index}.jpg"; //TODO:get form db

                            return x.u;
                        })
                      .ToList();

            return await Task.FromResult(results);
        }

        public async Task<UserDetails> GetUserDetails(string userName, string password)
        {
            string EncryptUserPassword = string.Empty;
            EncryptUserPassword = Util.Encrypt(password, AuthOptions.PASSWORDKEY);

            UserDetails userDetails = null;
            if (userName == "administrator" && password == "Gkuser001")  //Dmitriy Code for empty database with User ADMIN
            {
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
                    UserAdminPermissionText = GetConvertUserPremission((int)UserEnumPremission.system_administrator).JobTitleName.Trim(),
                    UserStatus = "active",
                    UserName = "administrator",
                    UserAdminPermission = (int)UserEnumPremission.system_administrator, // set in user pres code 8 = system_administrator add by moshe

                    UserCreateDate = string.Empty,
                    UserMail = string.Empty,
                    RoleId = 0
                };
            }
            else
            {
                userDetails = DbContext.User
                               .Where(u => u.UserName == userName && u.UserPassword == EncryptUserPassword)
                               .Select(u => new UserDetails
                               {
                                   UserId = u.UserGuid,
                                   UserGuid = u.UserGuid,
                                   Password = u.UserPassword,
                                   UserFirstName = u.UserFirstName ?? string.Empty,
                                   UserLastName = u.UserLastName ?? string.Empty,
                                   UserBusinessPhone = u.UserBusinessPhone ?? string.Empty,
                                   UserMobilePhone = u.UserMobilePhone ?? string.Empty,
                                   UserNotes = u.UserNotes ?? string.Empty,
                                   JobTitleGuid = u.JobTitleGuid ?? string.Empty,
                                   UnitName = u.OrgObjGu == null ? string.Empty : u.OrgObjGu.Name,
                                   UnitGuid = u.OrgObjGuid ?? string.Empty,
                                   OrgGuid = u.OrgObjGuid ?? string.Empty,
                                   UserAdminPermission = u.UserAdminPermission,
                                   UserStatus = u.UserStatus ?? string.Empty,
                                   UserName = u.UserName ?? string.Empty,
                                   UserCreateDate = u.UserCreateDate ?? string.Empty,
                                   UserMail = u.UserEmail ?? string.Empty,
                                   RoleId = u.RoleId,
                                   RoleName = u.Role.RoleName,
                                   RoleNameText = u.Role.RoleName,
                                   RolePermissions = DbContext.RolePermissions
                                                    .Include(rp => rp.PermissionType)
                                                    .Where(rp => rp.RoleId == u.RoleId)
                                                    .Select(x =>
                                                        new RolePermissionsInfo(x, u.Role.RoleName, x.PermissionType.PermissionTypeName)).ToList(),
                                   RoleItems = DbContext.RoleItems.Where(r => r.RoleId == u.RoleId).ToList()
                               })
                 .FirstOrDefault();
            }

            if (userDetails != null)
                userDetails.OrgName = userDetails.UnitName;

            return await Task.FromResult(userDetails);
        }

        public async Task<bool> DeleteUsers(IEnumerable<string> userGuids)
        {
            foreach (string userGuid in userGuids)
            {
                var rmList = DbContext.ModelComponent.Where(mc => mc.ModifiedUserGuid == userGuid).Include(mc => mc.ModelStructureModelComponentGu);
                List<ModelGuidAndTypeData> relatedmodels = new List<ModelGuidAndTypeData>();
                foreach (var rm in rmList)
                {
                    var s = rm.ModelStructureModelComponentGu != null ? rm.ModelStructureModelComponentGu.FirstOrDefault() : null;
                    relatedmodels.Add(new ModelGuidAndTypeData(rm.ModelComponentGuid, s != null ? s.ModelComponentParentGuid : null, s != null ? s.ModelComponentType : null));
                }
                await _modelService.DeleteModelComponentAsync(relatedmodels, true);

                var relatedSavedReport = DbContext.SavedReports.Where(sr => sr.UserGuid == userGuid).ToArray();
                foreach (var sr in relatedSavedReport)
                {
                    await _reportService.DeleteSavedReport(sr.ReportGuid);
                }


                var relatedCandidates = DbContext.Candidate.Where(c => c.UserGuid == userGuid);
                DbContext.Candidate.RemoveRange(relatedCandidates);
                DbContext.SaveChanges();

                var user = await DbContext.User.FirstOrDefaultAsync(u => u.UserGuid == userGuid);

                if (user != null)
                    DbContext.User.Remove(user);
            }

            var saveChangesCount = await DbContext.SaveChangesAsync();
            return saveChangesCount > 0;
        }

        public async Task<HttpStatusCode> SaveUser(UserDetails userDetails, bool createNewUser)
        {
            User user = DbContext.User.FirstOrDefault(x => x.UserGuid == userDetails.UserGuid);

            if (user == null && !createNewUser) return HttpStatusCode.Conflict;

                if (createNewUser)
            {
                if (user != null) return HttpStatusCode.AlreadyReported;
                user = new User { UserGuid = Util.CreateGuid().ToString() };
                userDetails.UserGuid = user.UserGuid;
            }

            PopulateUserDetails(user, userDetails, createNewUser);

            if (createNewUser)
            {
                await DbContext.User.AddAsync(user);
            }

            int saveChangeCount = await DbContext.SaveChangesAsync();

            return saveChangeCount > 0 ? HttpStatusCode.OK : HttpStatusCode.NotModified;
        }

        public async Task<List<string>> SaveUserByInterFace(List<UserDetails> userDetails, bool createNewUser)
        {
            List<string> users = new List<string>();
            foreach (var _user in userDetails)
            {
                if (createNewUser)
                {
                    _user.Password = "4cast123456";
                    _user.UserId = _user.UserId;
                    _user.UserName = _user.UserFirstName +_user.UserId;
                    _user.RoleId = 4;
                }

                await SaveUser(_user, createNewUser);

                _user.UserGuid = _user.UserId + "@" + _user.UserGuid;
                users.Add(_user.UserGuid);
            }

            await OkResult();
            return users;

        }

        public async Task<bool> SaveCandidates(List<Candidate> candidates)
        {
            int saveChangeCount = 0;

            if (candidates.Count > 0)
            {
                var oldRows = from c in DbContext.Candidate select c;
                if (oldRows != null)
                    DbContext.Candidate.RemoveRange(oldRows);

                await DbContext.Candidate.AddRangeAsync(candidates);
                saveChangeCount = await DbContext.SaveChangesAsync();

                await OkResult();
            }

            return saveChangeCount > 0;
        }


        public async Task<bool> ActivateUsers(string[] userGuids, bool isActive)
        {
            foreach (var userGuid in userGuids)
            {
                var user = await DbContext.User.FindAsync(userGuid);
                user.UserStatus = isActive ? "activate" : "";
            }

            int countChanges = DbContext.SaveChanges();
            return countChanges > 0;
        }

        public bool SendEmails(string[] userGuids = null)
        {
            bool result;
            try
            {
                result = false;
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
                result = false;
            }
            return result;
        }

        private void PopulateUserDetails(User user, UserDetails userDetails, bool isNewUser)
        {

            if (userDetails.UserAdminPermission == (int)UserEnumPremission.system_administrator || userDetails.UserAdminPermission == (int)UserEnumPremission.Content_manager)
            {
                if (!string.IsNullOrEmpty(userDetails.UserAdminPermissionText))
                {
                    userDetails.UserAdminPermissionText = DbContext.SystemJobTitles
                        .Where(x => x.UserAdminPermission == userDetails.UserAdminPermission)
                        .FirstOrDefault()?.JobTitleName;
                }
            }


            user.UserId = userDetails.UserId;
            user.UserGuid = userDetails.UserGuid;
            user.RoleId = userDetails.RoleId;
            userDetails.UserAdminPermission = userDetails.RoleId;
            user.UserType = userDetails.UserType != 0 ? userDetails.UserType : 1;

            user.UserName = userDetails.UserName;
            user.UserPassword = Util.EncryptPassword(userDetails.Password, AuthOptions.PASSWORDKEY);
            user.UserFirstName = userDetails.UserFirstName ?? string.Empty;
            user.UserLastName = userDetails.UserLastName ?? string.Empty;
            user.UserBusinessPhone = userDetails.UserBusinessPhone ?? string.Empty;
            user.UserMobilePhone = userDetails.UserMobilePhone ?? string.Empty;
            user.UserNotes = userDetails.UserNotes ?? string.Empty;
            user.UserEmail = userDetails.UserMail ?? string.Empty;
            user.UserAdminPermission = userDetails.RoleId;

            if (isNewUser)
            {
                user.UserStatus = "activate";
                user.UserCreateDate = Util.ConvertDateToString(DateTime.Now);
                user.JobTitleGuid = userDetails.JobTitleGuid;
                user.OrgObjGuid = userDetails.UnitGuid;
            }
            else
            {
                user.UserModifiedDate = Util.ConvertDateToString(DateTime.Now);

                // check if changed unit/job
                if ((user.OrgObjGuid != userDetails.UnitGuid) || (user.JobTitleGuid != userDetails.JobTitleGuid))
                {
                    user.JobTitleGuid = userDetails.JobTitleGuid ?? string.Empty;
                    user.OrgObjGuid = userDetails.UnitGuid ?? string.Empty;
                }
            }
        }

        #endregion Users
    }
}
