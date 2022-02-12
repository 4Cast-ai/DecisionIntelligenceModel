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

namespace FormsHandler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(PolicyTypes.ApiAuthPolicy)]
    [EnableCors(PolicyTypes.ApiCorsPolicy)]
    public class FormsAdminController : GeneralControllerBase
    {

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
                    dbUser = await DBGate.GetAsync<UserDetails>($"FormsGeneral/GetUserDetails?userName={userName}&password={password}");
                }

                //if (dbUser != null && dbUser.UserStatus == "activate" && dbUser.UserType != (int)UserTypes.HR)
                if (dbUser != null)
                {
                    //Random ran = new Random();
                    //int index = ran.Next(1, 50);
                    //dbUser.UserImg = $"https://randomuser.me/api/portraits/men/{index}.jpg";

                    dbUser.UserImg = ""; //TODO:get form db           
                    dbUser.Language = language;

                    //if (dbUser.RoleId != (int)RoleTypes.Anonymous)
                    //{
                    //    //dbUser.OrgName = DBGate.Get<Unit>($"Organization/GetUnitByGuid?UnitGuid={dbUser.UnitGuid}")?.unit_name ?? string.Empty;
                    //}

                    // create app user
                    FormsHandler.Models.AppUser appUser = FormsHandler.Models.AppUser.Create(dbUser);

                    var key = GeneralContext.GetConfig<IAuthOptions>().KEY;
                    var token = Util.CreateToken(appUser.ToStringJson(), key);

                    GeneralContext.Logger.Warning($"user {dbUser.UserName} authorized");
                    return Ok(new { data = token });
                }
            }

            GeneralContext.Logger.Warning($"user {dbUser?.UserName ?? userName} unauthorized");
            return Unauthorized();
        }
    }
}