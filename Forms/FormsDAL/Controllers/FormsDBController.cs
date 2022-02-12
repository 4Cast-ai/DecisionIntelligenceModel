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
    public class FormsDBController : ControllerBaseAction
    {
        private readonly FormsDBServices _formsDBServices;

        public FormsDBController(FormsDBServices FormsDBServices)
        {
            _formsDBServices = FormsDBServices;
        }

        #region Get

        [HttpGet("CreateSurvey")]
        public async Task<IActionResult> CreateSurvey()
        {
            var created =  await _formsDBServices.CreateFormsSurveyDB("FormsSurveyDB_Maya02");
            return await _formsDBServices.OkResult(created);
        }

        [HttpPost("CreateEvent")]
        public async Task<bool> CreateEvent([FromBody] EventDataObject eventData)
        {
            var result = true;
            return result;

        }

        #endregion Get

    }
}
