using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.Services;
using Infrastructure.Auth;
using Model.Data;
using Model.Entities;

namespace Dal.Controllers
{
    [Route("api/DalApi/[controller]")]
    [EnableCors(PolicyTypes.ApiCorsPolicy)]
    [ApiController]
    public class UserController : ControllerBaseAction
    {
        private readonly UserService _userService;

        public UserController(UserService users)
        {
            _userService = users;
        }

        [HttpGet("GetUserDetails")]
        public async Task<UserDetails> GetUserDetails([FromQuery]string userName, [FromQuery]string password)
        {
            var result = await _userService.GetUserDetails(userName, password);
            return result;
        }

        [HttpGet("GetUsers")]
        public async Task<List<UserDetails>> GetUsers([FromQuery]string userGuid = null)
        {
            var results = await _userService.GetUsers(userGuid);
            return results ?? new List<UserDetails>();
        }

        [HttpPost("ActivateUsers")]
        public async Task<IActionResult> ActivateUsers([FromBody]string[] userGuids)
        {
            var result = await _userService.ActivateUsers(userGuids, true);
            return await _userService.OkResult(result);
        }

        [HttpPost("DeactivateUsers")]
        public async Task<IActionResult> DeactivateUsers([FromBody]string[] userGuids)
        {
            var result = await _userService.ActivateUsers(userGuids, false);
            return await _userService.OkResult(result);
        }

        [HttpPost("DeleteUsers")]
        public async Task<IActionResult> DeleteUsers(IEnumerable<string> userGuids)
        {
            var result = await _userService.DeleteUsers(userGuids);
            return await _userService.OkResult(result);
        }

        [HttpPost("SaveUser")]
        public async Task<IActionResult> SaveUser([FromBody] UserDetails userDetails, [FromQuery] bool createNewUSer)
        {
            var result = await _userService.SaveUser(userDetails, createNewUSer);
            return await _userService.OkResult((int)result, result, result.ToString());
        }
        [HttpPost("SaveUserPreference")]
        public async Task<IActionResult> SaveUserPreference([FromBody] UserPreference _userPreference)
        {
            var result = await _userService.SaveUserPreference(_userPreference);
            return await _userService.OkResult((int)result, result, result.ToString());
        }

        [HttpGet("GetUserPreference")]
        public async Task<IActionResult> GetUserPreference([FromQuery] string userGuid)
        {
            var result = await _userService.GetUserPreference(userGuid);
            return await _userService.OkResult(result);
        }




        [HttpPost("SendEmails")]
        public bool SendEmails([FromBody]string[] userGuids)
        {
            return _userService.SendEmails(userGuids);
        }

        [HttpGet("DalGetPermissions")]
        public async Task<List<UserDetails>> GetPermissions()
        {
            var result = await _userService.GetPermissions();
            return result;
        }

        [HttpGet("GetRolePermissions/")]
        public async Task<List<RolePermissionsInfo>> GetRolePermissions([FromQuery]int? roleId)
        {
            var result = await _userService.GetRolePermissions(roleId);
            return result;
        }

        [HttpGet("GetRoles/")]
        public async Task<List<RolesInfo>> GetRoles([FromQuery]int? roleId)
        {
            var result = await _userService.GetRoles(roleId);
            return result ?? new List<RolesInfo>();
        }

        [HttpGet("GetUsersTypes")]
        public async Task<List<UserType>> GetUsersTypes()
        {
            var result = await _userService.GetUsersTypes();
            return result ?? new List<UserType>();
        }

      
    }
}
