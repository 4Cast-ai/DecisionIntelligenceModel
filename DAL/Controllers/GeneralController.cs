using System.Collections.Generic;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Dal.Services;
using Infrastructure.Auth;
using Model.Data;
using Model.Entities;
using System.Threading.Tasks;
using MeasuringUnitData = Model.Data.MeasuringUnitData;
using RollupMethodInfo = Model.Data.RollupMethodInfo;
using CalenderRollupData = Model.Data.CalenderRollupData;

namespace Dal.Controllers
{
    [Route("api/DalApi/[controller]")]
    [EnableCors(PolicyTypes.ApiCorsPolicy)]
    [ApiController]
    public class GeneralController : ControllerBaseAction
    {
        private readonly GeneralService _generalService;

        public GeneralController(GeneralService general)
        {
            _generalService = general;
        }

        #region Get

        [HttpGet("GetOrgObjActivityTemplates")]
        public async Task<IActionResult> GetOrgObjActivityTemplates([FromQuery]string org_obj_guid)
        {
            List<ActivityTemplateDataInfo> result = await _generalService.Get_Org_Obj_Activity_Templates(org_obj_guid);
            return await _generalService.OkResult(result);
        }

        [HttpGet("GetOrgObjActivities")]
        public async Task<IActionResult> GetOrgObjActivities([FromQuery]string org_obj_guid)
        {
            List<ActivityDetails> result = await _generalService.Get_Org_Obj_Activities(org_obj_guid);
            return await _generalService.OkResult(result);
        }
        [HttpGet("GetOrgObjActivitiesForFiller")]
        public async Task<IActionResult> GetOrgObjActivitiesForFiller([FromQuery] string org_obj_guid)
        {
            List<ActivityDetails> result = await _generalService.Get_Org_Obj_Activities_For_Filler(org_obj_guid);
            return await _generalService.OkResult(result);
        }

        //[HttpGet("GetFormsDataForFiller")]
        //public async Task<IActionResult> GetFormsDataForFiller([FromQuery] string org_obj_guid)
        //{
        //    List<FormData> result = await _generalService.Get_Forms_Data_For_Filler(org_obj_guid);
        //    return await _generalService.OkResult(result);
        //}

        #endregion Get

        #region Description

        [HttpGet("GetDescription")]
        public async Task<IActionResult> GetDescription([FromQuery]int descriptionGuid)
        {
            List<DescriptionsData> result = await _generalService.GetDiscription(descriptionGuid);
            return await _generalService.OkResult(result);
        }

        [HttpPost("AddDescription")]
        public async Task<IActionResult> AddDescription(Description description)
        {
            bool result = await _generalService.AddDescription(description);
            return await _generalService.OkResult(result);
        }

        [HttpPost("DeleteDescription")]
        public async Task<IActionResult> DeleteDescription(Description description)
        {
            bool result = await _generalService.DeleteDescription(description);
            return await _generalService.OkResult(result);
        }

        #endregion

        #region Maps

        [HttpPost("LinkOrg_Model_Polygon")]
        public async Task<IActionResult> LinkOrg_Model_Polygon(OrgModelPolygon data)
        {
            bool result = await _generalService.LinkOrg_Model_Polygon(data);
            return await _generalService.OkResult(result);
        }

        [HttpGet("GetLinkOrg_Model_Polygon")]
        public async Task<IActionResult> GetLinkOrg_Model_Polygon(string model_guid, string org_obj_guid)
        {
            OrgModelPolygon result = await _generalService.GetOrg_Model_Polygon(model_guid, org_obj_guid);
            return await _generalService.OkResult(result);
        }

        #endregion

        #region General

        [HttpGet("GetMeasuring")]
        public async Task<IActionResult> GetMeasuring()
        {
            List<MeasuringUnitData> result = await _generalService.GetMeasuringUnit();
            return await _generalService.OkResult(result);
        }

        [HttpGet("GetRollup_Method")]
        public async Task<IActionResult> GetRollup_Method()
        {
            List<RollupMethodInfo> result = await _generalService.GetRollupMethod();
            return await _generalService.OkResult(result);
        }

        [HttpGet("GetCalender_Rollup")]
        public async Task<IActionResult> GetCalender_Rollup()
        {
            List<CalenderRollupData> result = await  _generalService.GetCalenderRollup();
            return await _generalService.OkResult(result);
        }
        [HttpGet("GetGenderList")]
        public async Task<IActionResult> GetGenderList()
        {
            List<Gender> result = await _generalService.GetGenderList();
            return await _generalService.OkResult(result);
        }
        [HttpGet("GetStatusList")]
        public async Task<IActionResult> GetStatusList()
        {
            List<Status> result = await _generalService.GetStatusList();
            return await _generalService.OkResult(result);
        }
        #endregion General
    }
}
