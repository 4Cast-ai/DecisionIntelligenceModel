using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using FormsDal.Services;
using Infrastructure.Auth;
using Model.Data;
using Model.Entities;
using System.Threading.Tasks;
using MeasuringUnitData = Model.Data.MeasuringUnitData;
using RollupMethodInfo = Model.Data.RollupMethodInfo;
using CalenderRollupData = Model.Data.CalenderRollupData;

namespace FormsDal.Controllers
{
    [Route("api/DalApi/[controller]")]
    [EnableCors(PolicyTypes.ApiCorsPolicy)]
    [ApiController]
    public class FormsGeneralController : ControllerBaseAction
    {
        private readonly FormsGeneralService _generalService;
        
        public FormsGeneralController(FormsGeneralService general)
        {
            _generalService = general;
        }

        #region Get

        [HttpGet("IsConnected")]
        public async Task<IActionResult> GetOrgObjActivityTemplates()
        {
            var result = await _generalService.IsConnected();



            return await _generalService.OkResult(result);
        }
        [HttpGet("GetUserDetails")]
        public async Task<UserDetails> GetUserDetails([FromQuery] string userName, [FromQuery] string password)
        {
            var result = await _generalService.GetUserDetails(userName, password);
            return result;
        }

        #endregion Get

    }
}
