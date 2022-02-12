using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Controllers;
using Infrastructure.Extensions;
using Model.Entities;
using Swashbuckle.AspNetCore.Annotations;
using Model.Data;
using Expert.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace Expert.Controllers
{
    [Route("api/ExpertApi/[controller]")]
    [ApiController]
    public class DescriptionController : GeneralControllerBase
    {
        [HttpGet("GetDescription")]
        [SwaggerOperation(Summary = "", Description = "GetDescription")]
        public async Task<List<DescriptionsData>> GetDescription(int descriptionGuid)
        {
            string url = $"General/GetDescription?descriptionGuid={descriptionGuid}";
            var result = await DBGate.GetAsync<List<DescriptionsData>>(url);
            return result;
        }

        [HttpPost("AddDescription")]
        [SwaggerOperation(Summary = "", Description = "AddDescription")]
        public async Task<bool> AddDescription(DescriptionsData description)
        {
            Description d = Mapper.Map<Description>(description);
            var result = await DBGate.PostAsync<bool>("General/AddDescription", d);
            return result;
        }

        [HttpPost("DeleteDescription")]
        [SwaggerOperation(Summary = "", Description = "DeleteDescription")]
        public async Task<bool> DeleteDescription(DescriptionsData description)
        {
            Description d = Mapper.Map<Description>(description);
            var result = await DBGate.PostAsync<bool>("General/DeleteDescription", d);
            return result;
        }
    }
}
