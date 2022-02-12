using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Dal.Services;
using Infrastructure.Auth;
using Infrastructure.Helpers;
using Model.Data;
using Microsoft.AspNetCore.Http;
using System.IO;
using Model.Entities;

namespace Dal.Controllers
{
    [Route("api/DalApi/[controller]")]
    [EnableCors(PolicyTypes.ApiCorsPolicy)]
    [ApiController]
    public class ActivityController : ControllerBaseAction
    {
        private readonly ActivityService _activityService;
        private readonly ActivityTemplateService _activityTemplateService;

        public ActivityController(ActivityService activityService, ActivityTemplateService activityTemplateService)
        {
            _activityService = activityService;
            _activityTemplateService = activityTemplateService;
        }

        #region Activity_Template

        [HttpPost("SaveActivityTemplate")]
        public async Task<IActionResult> SaveActivityTemplate([FromBody] ActivityTemplateData activity_template_data)
        {
            string result = await _activityTemplateService.SaveActivityTemplate(activity_template_data);
            return await _activityService.OkResult(result);
        }

        [HttpGet("GetActivityTemplates")]
        public async Task<List<ActivityTemplateDataInfo>> GetActivityTemplates()
        {
            var results = await _activityTemplateService.GetActivityTemplates();
            return results;
        }

        [HttpGet("GetActivityTemplateDetails")]
        public async Task<ActivityTemplateData> GetActivityTemplateDetails([FromQuery]string activity_template_guid)
        {
            var results = await _activityTemplateService.GetActivityTemplateDetails(activity_template_guid);
            return results;
        }

        [HttpGet("DeleteActivityTemplate")]
        public async Task<IActionResult> DeleteActivityTemplate([FromQuery]string activity_template_guid)
        {
            var results = await _activityTemplateService.DeleteActivityTemplate(activity_template_guid);
            return await _activityService.OkResult(results);
        }

        [HttpGet("GetActivityTemplateFormTemplates")]
        public async Task<IActionResult> GetActivityTemplateFormTemplates([FromQuery] string activity_template_guid)
        {
            var results = await _activityTemplateService.GetActivityTemplateFormTemplates(activity_template_guid);
            return await _activityService.OkResult(results);
        }

        [HttpPost("GetOrganizationsFormScores")]
        public async Task<List<FormItemDataMulti>> GetOrganizationsFormScores(string formTemplateGuid, string activityGroupGuid, [FromBody] string[] orgList)
        {
            var results = await _activityTemplateService.GetOrganizationsFormScores(formTemplateGuid, activityGroupGuid, orgList);
            return results;
        }

        [HttpPost("SaveOrganizationsFormScores")]
        public async Task<IActionResult> SaveOrganizationsFormScores([FromBody] List<FormItemDataMulti> formItemsScores)
        {
            var result = await _activityTemplateService.SaveOrganizationsFormScores(formItemsScores);
            return await _activityService.OkResult(result);
        }

        #endregion Activity_Template

        #region Activity

        [HttpPost("SaveActivity")]
        public async Task<IActionResult> SaveActivity([FromBody] (ActivityDetails activity_data, List<Participant> participants)[] data)
        {
            string result = string.Empty;

            foreach (var item in data)
            {
                result += await _activityService.SaveActivity(item.activity_data, item.participants); 
            }

            return await _activityService.OkResult(result);
        }

        //[HttpPost("SaveMultiActivity")]
        //public async Task<IActionResult> SaveMultiActivity([FromBody] MultiActivity data)
        //{
        //    var result = await _activityService.SaveMultiActivity(data);
        //    return await _activityService.OkResult(result);
        //}
        
        //[HttpGet("GetMultiActivityItems")]TODO activityconections
        //public async Task<IActionResult> GetMultiActivityItems([FromQuery] string activity_group_guid)
        //{
        //    var result = await _activityService.GetMultiActivityItems(activity_group_guid);
        //    return await _activityService.OkResult(result);
        //}

        //[HttpGet("DeleteMultiActivityData")]
        //public async Task<IActionResult> DeleteMultiActivity([FromQuery] string activity_group_guid)
        //{
        //    var result = await _activityService.DeleteMultiActivityData(activity_group_guid);
        //    return await _activityService.OkResult(result);
        //}

        [HttpGet("DeleteActivity")]
        public async Task<IActionResult> DeleteActivity([FromQuery] string activity_guid)
        {
            var result = await _activityService.DeleteActivity(activity_guid);
            return await _activityService.OkResult(result);
        }

        //[HttpGet("DeleteItemFromMultiActivity")] TODO activityconections
        //public async Task<IActionResult> DeleteItemFromMultiActivity([FromQuery] string orgObjGuid, [FromQuery] string activityGroupGuid)
        //{
        //    var result = await _activityService.DeleteItemFromMultiActivity(orgObjGuid, activityGroupGuid);
        //    return await _activityService.OkResult(result);
        //}

        //[HttpPost("DeleteMultiActivity")]TODO activityconections
        //public async Task<IActionResult> DeleteMultiActivity([FromBody] (string org_obj_guid, string activity_group_guid)[] data)
        //{
        //    bool result = false;

        //    foreach (var item in data)
        //    {
        //        await _activityService.DeleteMultiActivity(item.org_obj_guid, item.activity_group_guid);
        //    }

        //    result = true;
        //    return await _activityService.OkResult(result);
        //}

        //[HttpGet("GetUnitsMultiActivities")]
        //public async Task<List<MultiActivityData>> GetUnitsMultiActivities(string orgObjGuid)
        //{
        //    var result = await _activityService.GetUnitsMultiActivities(orgObjGuid);
        //    return result;
        //}
        //[HttpGet("GetMultiActivities")]
        //public async Task<List<MultiActivityData>> GetMultiActivities()
        //{
        //    var result = await _activityService.GetMultiActivities();
        //    return result;
        //}
        
        [HttpGet("GetActivity")]
        public async Task<ActivityDetails> GetActivity([FromQuery]string activity_guid)
        {
            var result = await _activityService.GetActivity(activity_guid);
            return result;
        }

        [HttpPost("UploadFile")]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile([FromBody] ActivityFileData data)
        {
            var result = await _activityService.SaveActivityFile(data.ActivityGuid, data.FileName, data.Content);
            return await _activityService.OkResult(result);
        }

        [HttpGet("GetActivityFiles")]
        public async Task<List<ActivityFile>> GetActivityFiles([FromQuery] string activityGuid)
        {
            var result = await _activityService.GetActivityFiles(activityGuid);
            return result;
        }

        [HttpPost("DeleteActivityFile")]
        public async Task<IActionResult> DeleteActivityFile([FromBody] string[] activityFileGuids)
        {
            bool result = await _activityService.DeleteActivityFile(activityFileGuids);
            return await _activityService.OkResult(result);
        }

        
        #endregion Activity
    }
}
