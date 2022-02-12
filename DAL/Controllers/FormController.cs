using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Infrastructure.Auth;
using Infrastructure.Helpers;
using Model.Data;
using Model.Entities;
using Dal.Services;

namespace Dal.Controllers
{
    [Route("api/DalApi/[controller]")]
    [EnableCors(PolicyTypes.ApiCorsPolicy)]
    [ApiController]
    public class FormController : ControllerBaseAction
    {
        private readonly FormService _formService;
        private readonly FormTemplateService _formTemplateService;
        
        public FormController(FormService formService, FormTemplateService formTemplateService)
        {
            _formService = formService;
            _formTemplateService = formTemplateService;
        }

        #region Form_Template

        [HttpPost("SaveFormTemplate")]
        public async Task<IActionResult> SaveFormTemplate([FromBody] (FormTemplateDataInfo form_template, List<FormItemData> form_items_list, List<string> activities_list) data)
        {
            var formTemplate = Mapper.Map<FormTemplate>(data.form_template);

            string result = await _formTemplateService.SaveFormTemplate(formTemplate, data.form_items_list, data.activities_list);
            return await _formTemplateService.OkResult(result);
        }

        [HttpGet("GetFormTemplates")]
        public async Task<IActionResult> GetFormTemplates()
        {
            List<FormTemplateDataInfo> result = await _formTemplateService.GetAllFormTemplates();
            return await _formTemplateService.OkResult(result);
        }

        [HttpGet("GetFormTemplateDetails")]
        public async Task<IActionResult> GetFormTemplateDetails([FromQuery]string form_template_guid)
        {
            FormTemplateData result = await _formTemplateService.GetFormTemplateDetails(form_template_guid);
            return await _formTemplateService.OkResult(result);
        }

        [HttpGet("GetFormTemplateItems")]
        public async Task<IActionResult> GetFormTemplateItems(string form_template_guid)
        {
            List<FormItemData> result = await _formTemplateService.GetFormItemsList(form_template_guid);
            return await _formTemplateService.OkResult(result); ;
        }

        [HttpGet("DeleteFormTemplate")]
        public async Task<IActionResult> DeleteFormTemplate([FromQuery]string form_template_guid)
        {
            bool result = await _formTemplateService.DeleteFormTemplate(form_template_guid);
            return await _formTemplateService.OkResult(result);
        }

        #endregion Form_Template

        #region Form

        [HttpGet("GetFormDetails")]
        public async Task<IActionResult> GetFormDetails([FromQuery]string form_guid)
        {
            FormData result = await _formService.GetFormDetails(form_guid);
            return await _formTemplateService.OkResult(result);
        }

        [HttpPost("SaveFormScore")]
        public async Task<IActionResult> SaveFormScore([FromBody] FormData data)
        {
            bool result = await _formService.SaveFormScore(data);
            return await _formTemplateService.OkResult(result);
        }

        #endregion Form
    }
}
