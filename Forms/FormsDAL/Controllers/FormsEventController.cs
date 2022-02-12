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
    public class FormsEventController : ControllerBaseAction
    {
        private readonly FormsEventService _eventService;
        
        public FormsEventController(FormsEventService _event)
        {
            _eventService = _event;
        }

        #region Get

       

        #endregion Get

        [HttpPost("CreateEvent")]
        public async Task<bool> CreateEvent([FromBody] FormsDataObject eventData)
        {
            var result = true;
            return result;
        }

        [HttpPost("UpdateEvent")]
        public async Task<IActionResult> UpdateEvent([FromBody] FormsDataObject eventData)
        {
            //var result = _eventService.OkResult();
            return await _eventService.OkResult();
        }

        [HttpPost("EndEvent")]
        public async Task<List<Score>> EndEvent(string activity_guid)
        {
            //var result = await DBGate.PostAsync<List<Score>>("Event/EndEvent", activity_guid);
            return null;
        }

        [HttpPost("CloseDB")]
        public async Task<bool> CloseDB(int FormsDBID)
        {
            //var result = await DBGate.PostAsync<bool>("Event/CloseDB", FormsDBID);
            return true;
        }


    }
}
