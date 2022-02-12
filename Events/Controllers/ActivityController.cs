using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Infrastructure.Controllers;
using Infrastructure.Extensions;
using Model.Data;
using Model.Entities;
using Events.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Events.Controllers
{
    [Route("api/EventsApi/[controller]")]
    [ApiController]
    public class ActivityController : GeneralControllerBase
    {
        [HttpGet("GetOrgObjActivityTemplates")]
        [SwaggerOperation(Description = "Get OrgObj activity templates")]
        public async Task<List<ActivityTemplateDataInfo>> GetOrgObjActivityTemplates(string org_obj_guid)
        {
            string url = $"General/GetOrgObjActivityTemplates?org_obj_guid={org_obj_guid}";
            var result = await DBGate.GetAsync<List<ActivityTemplateDataInfo>>(url);
            return result;
        }

        [HttpGet("GetOrgObjActivities")]
        [SwaggerOperation(Description = "Get OrgObj activities")]
        public async Task<List<ActivityDetails>> GetOrgObjActivities(string org_obj_guid)
        {
            string url = $"General/GetOrgObjActivities?org_obj_guid={org_obj_guid}";
            var result = await DBGate.GetAsync<List<ActivityDetails>>(url);
            return result;
        }
        
        [HttpGet("GetOrgObjActivitiesForFiller")]
        [SwaggerOperation(Description = "Get OrgObj activities")]
        public async Task<List<ActivityDetails>> GetOrgObjActivitiesForFiller(string org_obj_guid)
        {
            string url = $"General/GetOrgObjActivitiesForFiller?org_obj_guid={org_obj_guid}";
            var result = await DBGate.GetAsync<List<ActivityDetails>>(url);
            return result;
        }

        
       //[HttpGet("GetFormsDataForFiller")]
       // [SwaggerOperation(Description = "Get OrgObj activities")]
       // public async Task<List<FormData>> GetFormsDataForFiller(string org_obj_guid)
       // {
       //     string url = $"General/GetFormsDataForFiller?org_obj_guid={org_obj_guid}";
       //     var result = await DBGate.GetAsync<List<FormData>>(url);
       //     return result;
       // }

        //[HttpGet("GetMultiActivities")]
        //[SwaggerOperation(Description = "Get Multi Activities")]
        //public async Task<List<MultiActivityData>> GetMultiActivities()
        //{
        //    string url = $"Activity/GetMultiActivities";
        //    var result = await DBGate.GetAsync<List<MultiActivityData>>(url);
        //    return result;
        //}

        //[HttpGet("GetUnitsMultiActivities")]
        //[SwaggerOperation(Description = "Get Units Multi Activities")]
        //public async Task<List<MultiActivityData>> GetUnitsMultiActivities(string orgObjGuid)
        //{
        //    string url = $"Activity/GetUnitsMultiActivities?orgObjGuid={orgObjGuid}";
        //    var result = await DBGate.GetAsync<List<MultiActivityData>>(url);
        //    return result;
        //}
        [HttpGet("GetActivity")]
        [SwaggerOperation(Description = "Get activity")]
        public async Task<ActivityDetails> GetActivity(string activity_guid)
        {
            string url = $"Activity/GetActivity?activity_guid={activity_guid}";
            var result = await DBGate.GetAsync<ActivityDetails>(url);
            return result;
        }

        [HttpPost("SaveActivity")]
        [SwaggerOperation(Description = "Save activity")]
        public async Task<string> SaveActivity([FromBody](ActivityDetails activity_data, List<Participant> participants)[] data)
        {
            string result = await DBGate.PostAsync<string>("Activity/SaveActivity", data);
            return result;
        }

        //[HttpPost("SaveMultiActivity")]
        //[SwaggerOperation(Description = "Save Multi Activity")]
        //public async Task<string> SaveMultiActivity([FromBody] MultiActivityData data)
        //{
        //    //var mapRes = Mapper.Map<List<MultiActivity>>(data);
        //    MultiActivity ma = new MultiActivity();
        //    ma.ActivityGroupGuid = data.activity_group_guid;
        //    //ma.OrgObjGuidList = string.Join(";", data.org_obj_guid_list);
        //    ma.Units = data.units;
        //    ma.Persons = data.persons;
        //    ma.Name = data.name;
        //    ma.Description = data.description;
        //    ma.ActivityTemplateGuid = data.activity_template.activity_template_guid;
        //    ma.StartDate = data.start_date;
        //    ma.EndDate = data.end_date;
        //    //ma.EstimatesGuids = string.Join(";", data.estimates_guids);
        //    //ma.EstimatesNames = string.Join(";", data.estimates_names);
        //    ma.EstimateUnits = data.estimateUnits;
        //    ma.EstimatePersons = data.estimatePersons;
        //    string result = await DBGate.PostAsync<string>("Activity/SaveMultiActivity", ma);
        //    return result;
        //}

        //[HttpGet("GetMultiActivityItems")]
        //[SwaggerOperation(Description = "Get Multi Activity Items")]TODO activityconections
        //public async Task<List<ActivityDetails>> GetMultiActivityItems(string activity_group_guid)
        //{
        //    string url = $"Activity/GetMultiActivityItems?activity_group_guid={activity_group_guid}";
        //    var result = await DBGate.GetAsync<List<ActivityDetails>>(url);
        //    return result;
        //}

        [HttpGet("DeleteMultiActivityData")]
        [SwaggerOperation(Description = "Delete Multi Activity Data")]
        public async Task<bool> DeleteMultiActivityData(string activity_group_guid)
        {
            string url = $"Activity/DeleteMultiActivityData?activity_group_guid={activity_group_guid}";
            bool result = await DBGate.GetAsync<bool>(url);
            return result;
        }

        [HttpPost("DeleteActivity")]
        [SwaggerOperation(Description = "Delete activity")]
        public async Task<bool> DeleteActivity(string activity_guid)
        {
            string url = $"Activity/DeleteActivity?activity_guid={activity_guid}";
            bool result = await DBGate.GetAsync<bool>(url);
            return result;
        }

        [HttpGet("DeleteItemFromMultiActivity")]
        [SwaggerOperation(Description = "DeleteItemFromMultiActivity")]
        public async Task<bool> DeleteItemFromMultiActivity(string orgObjGuid, string activityGroupGuid)
        {
            string url = $"Activity/DeleteItemFromMultiActivity?orgObjGuid={orgObjGuid}&activityGroupGuid={activityGroupGuid}";
            bool result = await DBGate.GetAsync<bool>(url);
            return result;
        }

        [HttpPost("DeleteMultiActivity")]
        [SwaggerOperation(Description = "Delete Multi Activity")]
        public async Task<bool> DeleteMultiActivity([FromBody] (string org_obj_guid, string activity_group_guid)[] data)
        {
            bool result = await DBGate.PostAsync<bool>("Activity/DeleteMultiActivity", data);
            return result;
        }

        [HttpPost("UploadFile")]
        [DisableRequestSizeLimit]
        [SwaggerOperation(Description = "Upload File")]
        public async Task<string> UploadFile([FromBody] ActivityFileData data)
        {
            string url = $"Activity/UploadFile";
            string result = await DBGate.PostAsync<string>(url, data);
            return result;
        }

        [HttpGet("GetActivityFiles")]
        [SwaggerOperation(Description = "Get Activity Files")]
        public async Task<List<ActivityFileData>> GetActivityFiles(string activityGuid)
        {
            string url = $"Activity/GetActivityFiles?activityGuid={activityGuid}";
            var result = await DBGate.GetAsync<List<ActivityFile>>(url);
            var mapRes = Mapper.Map<List<ActivityFileData>>(result);
            return mapRes;
        }

        [HttpPost("DeleteActivityFile")]
        [SwaggerOperation(Description = "Delete Activity File")]
        public async Task<bool> DeleteActivityFile(string[] activityFileGuids)
        {
            string url = $"activity/DeleteActivityFile";
            bool result = await DBGate.PostAsync<bool>(url, activityFileGuids);
            return result;
        }

       
    }
}
