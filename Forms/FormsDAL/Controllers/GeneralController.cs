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
    public class GeneralController : ControllerBaseAction
    {
        private readonly GeneralService _generalService;
        
        public GeneralController(GeneralService general)
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

        #endregion Get

    }
}
