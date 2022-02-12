using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using Infrastructure.Controllers;
using Infrastructure.Extensions;
using Model.Data;
using Model.Entities;

namespace Expert.Controllers
{
    [Route("api/ExpertApi/[controller]")]
    [ApiController]
    public class ActivityTemplateController : GeneralControllerBase
    {
        [HttpPost("SaveActivityTemplate")]
        [SwaggerOperation(Summary = "", Description = "SaveActivityTemplate")]
        public async Task<string> SaveActivityTemplate(ActivityTemplateData data)
        {
            string result = await DBGate.PostAsync<string>("activity/SaveActivityTemplate", data);
            return result;
        }

        [HttpGet("GetActivityTemplates")]
        [SwaggerOperation(Summary = "", Description = "GetActivityTemplates")]
        public async Task<List<ActivityTemplateDataInfo>> GetActivityTemplates()
        {
            string url = "activity/GetActivityTemplates";
            var result = await DBGate.GetAsync<List<ActivityTemplateDataInfo>>(url);
            return result;
        }

        [HttpGet("GetActivityTemplateDetails")]
        [SwaggerOperation(Summary = "", Description = "GetActivityTemplateDetails")]
        public async Task<ActivityTemplateData> GetActivityTemplateDetails(string activity_template_guid)
        {
            string url = $"activity/GetActivityTemplateDetails?activity_template_guid={activity_template_guid}";
            ActivityTemplateData result = await DBGate.GetAsync<ActivityTemplateData>(url);
            return result;
        }

        [HttpGet("DeleteActivityTemplate")]
        [SwaggerOperation(Summary = "", Description = "DeleteActivityTemplate")]
        public async Task<bool> DeleteActivityTemplate(string activity_template_guid)
        {
            string url = $"activity/DeleteActivityTemplate?activity_template_guid={activity_template_guid}";
            bool success = await DBGate.GetAsync<bool>(url);
            return success;
        }

        [HttpGet("GetActivityTemplateFormTemplates")]
        [SwaggerOperation(Summary = "", Description = "GetActivityTemplateFormTemplates")]
        public async Task<List<object>> GetActivityTemplateFormTemplates(string activity_template_guid)
        {
            string url = $"activity/GetActivityTemplateFormTemplates?activity_template_guid={activity_template_guid}";
            List<object> res = await DBGate.GetAsync<List<object>>(url);
            return res;
        }

        [HttpPost("GetOrganizationsFormScores")]
        [SwaggerOperation(Summary = "", Description = "GetOrganizationsFormScores")]
        public async Task<List<FormItemDataMulti>> GetOrganizationsFormScores(string formTemplateGuid, string activityGroupGuid, [FromBody] string[] orgList)
        {
            string url = $"activity/GetOrganizationsFormScores?formTemplateGuid={formTemplateGuid}&activityGroupGuid={activityGroupGuid}";
            var result = await DBGate.PostAsync<List<FormItemDataMulti>>(url, orgList);
            return result;
        }

        [HttpPost("SaveOrganizationsFormScores")]
        [SwaggerOperation(Summary = "", Description = "SaveOrganizationsFormScores")]
        public async Task<bool> SaveOrganizationsFormScores([FromBody] List<FormItemDataMulti> formItemsScores)
        {
            string url = $"activity/SaveOrganizationsFormScores";
            bool success = await DBGate.PostAsync<bool>(url, formItemsScores);
            return success;
        }

    }
}