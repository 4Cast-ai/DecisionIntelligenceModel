using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Swashbuckle.AspNetCore.Annotations;
using Infrastructure.Controllers;
using Infrastructure.Interfaces;
using Infrastructure.Extensions;
using Infrastructure.Auth;
using Infrastructure.Core;
using Infrastructure.Enums;
using Infrastructure.Models;
using Model.Data;
using Model.Entities;
using Util = Infrastructure.Helpers.Util;

namespace Expert.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(PolicyTypes.ApiAuthPolicy)]
    [EnableCors(PolicyTypes.ApiCorsPolicy)]
    public class AdminApiController : GeneralControllerBase
    {
        [HttpGet("CheckHealthServices")]
#if DEBUG
        [AllowAnonymous]
#endif
        [SwaggerOperation(Description = "HealthCheck reporting the health of app infrastructure components")]
        public ActionResult<string> CheckHealthServices()
        {
            var GatewayApi = GeneralContext.CreateRestClient(ApiServiceNames.Gateway);
            var result = GatewayApi.Get<object>("/health");
            return Ok(result);
        }

        [HttpGet("CheckDbMigration")]
#if DEBUG
        [AllowAnonymous]
#endif
        [SwaggerOperation(Description = "HealthCheck reporting the health of app infrastructure components")]
        public ActionResult<string> CheckDbMigration()
        {
            var DbGateApi = GeneralContext.CreateRestClient(ApiServiceNames.DalApi);
            var result = DbGateApi.Get<object>("Admin/CheckDbMigration");
            return Ok(result);
        }

        [HttpGet("Login")]
        [AllowAnonymous]
        [SwaggerOperation(Description = "Login with userName and password")]
        public async Task<IActionResult> Login(string userName, string password, int language = 1)
        {
            //string ADGDomainName = Config.GetSettingValue<string>("AD_Domain_Name");
            //string ADGroup = Config.GetSettingValue<string>("AD_Group");
            UserDetails dbUser = null;

            //login to adfs
            bool result = true; //CheckUserInActiveDirectoryGroup(ADGDomainName, ADGroup, userName);

            if (result) // its should be res but until we deploy it its not need to use AD by ohad request.
            {
                //get user details
                if (userName == "Anonymous")
                {
                    dbUser = new UserDetails()
                    {
                        UserId = Guid.NewGuid().ToString(),
                        UserName = "Anonymous",
                        UserFirstName = "יוזר",
                        UserLastName = "אנונימי",
                        RoleId = (int)RoleTypes.Anonymous,
                        UserStatus = "activate"
                    };
                }
                else
                {
                    dbUser = await DBGate.GetAsync<UserDetails>($"User/GetUserDetails?userName={userName}&password={password}");
                }

                if (dbUser != null && dbUser.UserStatus == "activate" && dbUser.UserType != (int)UserTypes.HR)
                {
                    //Random ran = new Random();
                    //int index = ran.Next(1, 50);
                    //dbUser.UserImg = $"https://randomuser.me/api/portraits/men/{index}.jpg";
                    
                    dbUser.UserImg = ""; //TODO:get form db           
                    dbUser.Language = language;

                    if (dbUser.RoleId != (int)RoleTypes.Anonymous)
                    {
                        //dbUser.OrgName = DBGate.Get<Unit>($"Organization/GetUnitByGuid?UnitGuid={dbUser.UnitGuid}")?.unit_name ?? string.Empty;
                    }

                    // create app user
                   Expert.Models.AppUser appUser = Expert.Models.AppUser.Create(dbUser);

                    var key = GeneralContext.GetConfig<IAuthOptions>().KEY;
                    var token = Util.CreateToken(appUser.ToStringJson(), key);

                    GeneralContext.Logger.Warning($"user {dbUser.UserName} authorized");
                    return Ok(new { data = token });
                }
            }

            GeneralContext.Logger.Warning($"user {dbUser?.UserName ?? userName} unauthorized");
            return Unauthorized();
        }

        [HttpGet("LoginAD")]
        [AllowAnonymous]
        [SwaggerOperation(Description = "Login with AD")]
        public async Task<IActionResult> LoginAD(string userName = "", string password = "")
        {
            //string ADGDomainName = Config.GetSettingValue<string>("AD_Domain_Name");
            //string ADGroup = Config.GetSettingValue<string>("AD_Group");
            ADUser adUser = null;
            //login to adfs
            //bool res = CheckUserInActiveDirectoryGroup(ADGDomainName, ADGroup, userName);

            if (true) // its should be res but until we deploy it its not need to use AD by ohad request.
            {
                //login to db
                string url = string.Format("Login?userName={0}&password={1}", userName, password);
                UserDetails dbUser = await DBGate.GetAsync<UserDetails>(url);

                if (dbUser != null)
                {
                    Random ran = new Random();
                    int index = ran.Next(1, 50);

                    string url3 = string.Format("GetJobTitleByGuid?JobTitleGuid={0}", dbUser.JobTitleGuid);
                    string url2 = string.Format("GetUnitByGuid?UnitGuid={0}", dbUser.UnitGuid);
                    Unit unit = await DBGate.GetAsync<Unit>(url2);

                    adUser = new ADUser
                    {
                        UserGuid = dbUser.UserGuid,
                        UserFullName = string.Format("{0} {1}", dbUser.UserFirstName, dbUser.UserLastName),
                        UserImg = "https://randomuser.me/api/portraits/men/" + index + ".jpg",//TODO:get form db
                        UserJobTitle = string.Empty,
                        UserLastSignIn = DateTime.Now//TODO:get from db
                    };

                    return Ok(new { adUser });
                }
            }

            GeneralContext.Logger.Warning($"user {adUser?.UserGuid ?? ""} unauthorized");
            return NotFound();
        }
    }
}