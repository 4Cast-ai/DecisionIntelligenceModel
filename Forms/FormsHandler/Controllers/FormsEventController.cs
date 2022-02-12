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
using System.Collections.Generic;


namespace FormsHandler.Controllers
{
    [Route("api/FormsHandlerApi/[controller]")]
    [ApiController]
    public class FormsEventController : GeneralControllerBase
    {
        
      
        [HttpGet("IsConnected")]
        [SwaggerOperation(Summary = "", Description = "IsConnected")]
        public async Task<string> CheckConnection()
        {
            string url = $"FormsGeneral/IsConnected";            
            var result = await DBGate.GetAsync<string>(url);
            return result;
        }

        [HttpPost("CreateEvent")]
        [SwaggerOperation(Summary = "", Description = "Create Event")]
        public async Task<bool> CreateEvent(FormsDataObject eventData)
        {
            var result = await DBGate.PostAsync<bool>("Event/CreateEvent", eventData);
            return result;
        }

        [HttpPost("UpdateEvent")]
        [SwaggerOperation(Summary = "", Description = "Update Event")]
        public async Task<IActionResult> UpdateEvent(FormsDataObject eventData)
        {
            var result = await DBGate.PostAsync<IActionResult>("Event/UpdateEvent", eventData);
            return result;
        }

        [HttpPost("EndEvent")]
        [SwaggerOperation(Summary = "", Description = "End Event")]
        public async Task<List<Score>> EndEvent(string activity_guid)
        {
            var result = await DBGate.PostAsync<List<Score>>("Event/EndEvent", activity_guid);
            return result;
        }

        [HttpPost("CloseDB")]
        [SwaggerOperation(Summary = "", Description = "Close data base")]
        public async Task<bool> CloseDB(int FormsDBID)
        {
            var result = await DBGate.PostAsync<bool>("Event/CloseDB", FormsDBID);
            return result;
        }
    }
}