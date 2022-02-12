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
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.Annotations;
using Infrastructure.Controllers;

namespace FormsDal.Controllers
{
    [Route("api/DalApi/[controller]")]
    //[EnableCors(PolicyTypes.ApiCorsPolicy)]
    [ApiController]
    public class FormsDBController : GeneralControllerBase
    {
        private readonly FormsDBServices _formsDBServices;

        public FormsDBController(FormsDBServices FormsDBServices)
        {
            _formsDBServices = FormsDBServices;
        }

        #region Post

        [HttpPost("CreateEvent")]
        [SwaggerOperation(Summary = "", Description = "CreateEvent")]
        public async Task<bool> CreateEvent([FromBody]FormsDataObject eventData)
        {
            int i = 1;
            //var created = await _formsDBServices.CreateFormsSurveyDB("FormsDynamicDB_Maya07");
            var created = await _formsDBServices.CreateEvent(eventData);
            var result = await _formsDBServices.OkResult(created);
            return created;
        }

        #endregion Get

    }
}
