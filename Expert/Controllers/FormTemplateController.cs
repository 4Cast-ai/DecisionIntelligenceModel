using Infrastructure.Controllers;
using Infrastructure.Extensions;
using Model.Data;
using Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Expert.Controllers
{
    [Route("api/ExpertApi/[controller]")]
    [ApiController]
    public class FormTemplateController : GeneralControllerBase
    {
        [HttpPost("SaveFormTemplate")]
        [SwaggerOperation(Summary = "", Description = "SaveFormTemplate")]
        public async Task<string> SaveFormTemplate([FromBody] (FormTemplateDataInfo form_template, List<FormItemData> form_items_list, List<string> activities_list) data)
        {
            string result = await DBGate.PostAsync<string>("form/SaveFormTemplate", data);
            return result;
        }

        [HttpGet("GetFormTemplates")]
        [SwaggerOperation(Summary = "", Description = "GetFormTemplates")]
        public async Task<List<FormTemplateDataInfo>> GetFormTemplates()
        {
            var result = await DBGate.GetAsync<List<FormTemplateDataInfo>>("form/GetFormTemplates");
            return result;
        }

        [HttpGet("GetFormTemplateDetails")]
        [SwaggerOperation(Summary = "", Description = "GetFormTemplateDetails")]
        public async Task<FormTemplateData> GetFormTemplateDetails(string form_template_guid)
        {
            string url = $"form/GetFormTemplateDetails?form_template_guid={form_template_guid}";
            var result = await DBGate.GetAsync<FormTemplateData>(url);
            return result;
        }

        [HttpGet("GetFormTemplateItems")]
        [SwaggerOperation(Summary = "", Description = "GetFormTemplateItems")]
        public async Task<List<FormItemData>> GetFormTemplateItems(string form_template_guid)
        {
            string url = $"form/GetFormTemplateItems?form_template_guid={form_template_guid}";
            var result = await DBGate.GetAsync<List<FormItemData>>(url);
            return result;
        }

        [HttpGet("DeleteFormTemplate")]
        [SwaggerOperation(Summary = "", Description = "DeleteFormTemplate")]
        public async Task<bool> DeleteFormTemplate(string form_template_guid)
        {
            string url = $"form/DeleteFormTemplate?form_template_guid={form_template_guid}";
            bool result = await DBGate.GetAsync<bool>(url);
            return result;
        }
    }
}
