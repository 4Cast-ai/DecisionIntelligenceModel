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

namespace Expert.Controllers
{
    [Route("api/ExpertApi/[controller]")]
    [ApiController]
    public class GeneralController : GeneralControllerBase
    {
        [HttpGet("GetGenderList")]
        [SwaggerOperation(Summary = "", Description = "GetGenderList")]
        public async Task<List<Gender>> GetGenderList()
        {
            string url = $"General/GetGenderList";
            var result = await DBGate.GetAsync<List<Gender>>(url);
            return result;
        }

        [HttpGet("GetStatusList")]
        [SwaggerOperation(Summary = "", Description = "GetStatusList")]
        public async Task<List<Status>> GetStatusList()
        {
            string url = $"General/GetStatusList";
            var result = await DBGate.GetAsync<List<Status>>(url);
            return result;
        }
    }
}