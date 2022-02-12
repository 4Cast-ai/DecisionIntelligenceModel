using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Controllers;
using Model.Data;
using Swashbuckle.AspNetCore.Annotations;
using Infrastructure.Extensions;
using System.Threading.Tasks;

namespace Events.Controllers
{
    [Route("api/EventsApi/[controller]")]
    [ApiController]
    public class FormController : GeneralControllerBase
    {
        [HttpGet("GetFormDetails")]
        [SwaggerOperation(Description = "Get form details")]
        public async Task<FormData> GetFormDetails(string form_guid)
        {
            string url = $"Form/GetFormDetails?form_guid={form_guid}";
            var result = await DBGate.GetAsync<FormData>(url);
            return result;
        }

        [HttpPost("SaveFormScore")]
        [SwaggerOperation(Description = "Save Form score")]
        public async Task<bool> SaveFormScore(FormData data)
        {
            bool result = await DBGate.PostAsync<bool>("form/SaveFormScore", data);
            return result;
        }
    }
}