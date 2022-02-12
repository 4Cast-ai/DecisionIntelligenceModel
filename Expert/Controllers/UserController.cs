using System.Net;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Expert.Models;
using Infrastructure.Auth;
using Model.Data;
using Model.Entities;
using Infrastructure.Controllers;
using Infrastructure.Extensions;
using Infrastructure.Helpers;

namespace Expert.Controllers
{
    [Route("api/ExpertApi/[controller]")]
    [EnableCors(PolicyTypes.ApiCorsPolicy)]
    [ApiController]
    public class UserController : GeneralControllerBase
    {
        [HttpGet("GetUserDetails")]
        [SwaggerOperation(Description = "GetUserDetails")]
        public async Task<UserDetails> GetUserDetails([FromQuery] string userName, [FromQuery] string password)
        {
            var result = await DBGate.GetAsync<UserDetails>($"User/GetUserDetails?userName={userName}&password={password}");
            return result;
        }

        [HttpGet("GetUsers")]
        [SwaggerOperation(Description = "GetUsers")]
        public async Task<List<UserDetails>> GetUsers([FromQuery] string userGuid = null)
        {
            var results = await DBGate.GetAsync<List<UserDetails>>($"User/GetUsers?userGuid={userGuid}");
            return results ?? new List<UserDetails>();
        }

        [HttpPost("ActivateUsers")]
        [SwaggerOperation(Description = "ActivateUsers")]
        public async Task<bool> ActivateUsers([FromBody] string[] userGuids)
        {
            var result = await DBGate.PostAsync<bool>($"User/ActivateUsers", userGuids);
            return result;
        }

        [HttpPost("DeactivateUsers")]
        [SwaggerOperation(Description = "DeactivateUsers")]
        public async Task<bool> DeactivateUsers([FromBody] string[] userGuids)
        {
            var result = await DBGate.PostAsync<bool>($"User/DeactivateUsers", userGuids);
            return result;
        }

        [HttpPost("DeleteUsers")]
        [SwaggerOperation(Description = "DeleteUsers")]
        public async Task<bool> DeleteUsers(IEnumerable<string> userGuids)
        {
            var result = await DBGate.PostAsync<bool>($"User/DeleteUsers", userGuids);
            return result;
        }

        [HttpPost("DeleteUser")]
        [SwaggerOperation(Description = "DeleteUser")]
        public async Task<bool> DeleteUser(List<UserDetails> userList)
        {
            var result = await DBGate.PostAsync<bool>($"User/DeleteUser", userList);
            return result;
        }

        [HttpPost("SaveUser")]
        [SwaggerOperation(Summary = "", Description = "SaveUser")]
        public async Task<bool> SaveUser([FromBody] UserDetails userDetails)
        {
            bool createNewUser = string.IsNullOrEmpty(userDetails.UserGuid);
            string path = createNewUser ? $"?createNewUser={createNewUser}" : "";
            int result = await DBGate.PostAsync<int>($"User/SaveUser{path}", userDetails);
            return (HttpStatusCode)result == HttpStatusCode.OK;
        }

        [HttpGet("GetRolePermissions")]
        [SwaggerOperation(Description = "Get user role with permissions")]
        public async Task<List<RolePermissionsInfo>> GetRolePermissions(int? roleId)
        {
            var results = await DBGate.GetAsync<List<RolePermissionsInfo>>($"User/GetRolePermissions?roleId={roleId}");
            return results;
        }

        [HttpGet("GetRoles/{roleId?}")]
        [SwaggerOperation(Description = "Get role by roleId")]
        public async Task<List<RolesInfo>> GetRoles([FromRoute] int? roleId)
        {
            var results = await DBGate.GetAsync<List<RolesInfo>>($"User/GetRoles?roleId={roleId}");
            return results;
        }

        [HttpPost("SendEmails")]
        [SwaggerOperation(Description = "SendEmails")]
        public async Task<bool> SendEmails([FromBody] string[] userGuids)
        {
            var result = await DBGate.PostAsync<bool>($"User/SendEmails", userGuids);
            return result;
        }

        [HttpGet("GetUsersTypes")]
        [SwaggerOperation(Description = "GetUsersTypes")]
        public async Task<List<UserTypeData>> GetUsersTypes()
        {
            var result = await DBGate.GetAsync<List<UserType>>($"User/GetUsersTypes");
            var mapRes = Mapper.Map<List<UserTypeData>>(result);
            return mapRes;
        }

        [HttpPost("TogglePassword")]
        [SwaggerOperation(Description = "Toggle password (encrypt/decrypt)")]
        public async Task<IActionResult> TogglePassword([FromBody] PasswordModel passwordData)
        {
            if (string.IsNullOrEmpty(passwordData.PasswordText))
                return await Task.FromResult(new JsonResult(BadRequest()));

            string result;
            if (passwordData.PasswordText.EndsWith("="))
                result = Util.DecryptPassword(passwordData.PasswordText, AuthOptions.PASSWORDKEY);
            else
                result = Util.EncryptPassword(passwordData.PasswordText, AuthOptions.PASSWORDKEY);

            return await Task.FromResult(new JsonResult(result));
        }


        [HttpPost("SaveUserTheme")]
        [SwaggerOperation(Summary = "", Description = "SaveUserTheme")]
        public async Task<bool> SaveUserTheme([FromBody] UserPreference _userPreference)
        {
            int result = await DBGate.PostAsync<int>($"User/SaveUserPreference", _userPreference);
            return (HttpStatusCode)result == HttpStatusCode.OK;
        }
        [HttpGet("GetUserPreference")]
        [SwaggerOperation(Summary = "", Description = "GetUserPreference")]
        public async Task<UserPreference> GetUserPreference([FromQuery] string userGuid)
        {       
            UserPreference result = await DBGate.GetAsync<UserPreference>($"User/GetUserPreference?userGuid={userGuid}");
            return result;
        }


        
    }
}
