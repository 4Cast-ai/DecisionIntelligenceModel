using Dal;
using Dal.Services;
using Infrastructure.Auth;
using Infrastructure.Core;
using Infrastructure.Filters;
using Model.Data;
using Model.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Controllers
{
    [Route("api/DalApi/[controller]")]
    [EnableCors(PolicyTypes.ApiCorsPolicy)]
    [ApiController]
    public class OrganizationController : ControllerBaseAction
    {
        private readonly OrganizationService _orgService;

        public OrganizationController(OrganizationService orgService)
        {
            _orgService = orgService;
        }

        //[HttpGet("GetOrganizationType")]
        //public async Task<IActionResult> GetOrganizationType()
        //{
        //    List<OrganizationTypeData> result = await _orgService.GetOrganizationType();
        //    return await _orgService.OkResult(result);
        //}

        //[HttpGet("GetOrganizationTree")]
        //[ApiCache(typeof(OrganizationObjectData), typeof(OrgObjTree), true)]
        //public async Task<IActionResult> GetOrganizationTree()
        //{
        //    var result = await _orgService.GetOrganizationTree();
        //    return await _orgService.OkResult(result ?? new OrgObjTree());
        //}
        [HttpGet("GetUnit")]
        public async Task<IActionResult> GetUnit(string unitGuid)
        {
            List<UnitData> result = await _orgService.GetUnit(unitGuid);
            return await _orgService.OkResult(result);

        }

        [HttpGet("GetUnitByAT")]
        public async Task<IActionResult> GetUnitByAT(string activityTemplateGuid)
        {
            List<UnitData> result = await _orgService.GetUnitByAT(activityTemplateGuid);
            return await _orgService.OkResult(result);

        }

        [HttpGet("GetUnitPeoples")]
        public async Task<IActionResult> GetUnitPeoples(string unitGuid)
        {
            List<PersonData> result = await _orgService.GetUnitPeoples(unitGuid);
            return await _orgService.OkResult(result);

        }
        
        //[HttpGet("GetUnitsList")]
        //public async Task<IActionResult> GetUnitsList()
        //{
        //    List<UnitData> result = await _orgService.GetUnitsList();
        //    return await _orgService.OkResult(result);

        //}
        [HttpGet("GetPerson")]
        public async Task<IActionResult> GetPerson(string personGuid)
        {
            List<PersonData> result = await _orgService.GetPerson(personGuid);
            return await _orgService.OkResult(result);
        }
        //[HttpGet("UpdateOrganizationTreeConnections")]
        //[ApiCache(typeof(OrgObjTree), typeof(OrganizationObjectData), true)]
        //public async Task<IActionResult> UpdateOrganizationTreeConnections()
        //{
        //    var result = await _orgService.UpdateOrganizationTreeConnections();
        //    return await _orgService.OkResult(result ?? new OrganizationObjectData());
        //}

        [HttpGet("GetOrgObjConnection")]
        public async Task<IActionResult> GetOrgObjConnection(string orgObjGuid)
        {
            var result = await _orgService.GetOrgObjConnection(orgObjGuid);
            return await _orgService.OkResult(result);
        }

        [HttpGet("GetUnitByGuid")]
        public async Task<IActionResult> GetUnitByGuid(string UnitGuid)
        {
            var result = await _orgService.GetUnitByGuid(UnitGuid);
            return await _orgService.OkResult(result);
        }

        //[HttpPost("SaveOrganizationObject")]
        //[ApiCache(typeof(OrganizationObjectData))]
        //public async Task<IActionResult> SaveOrganizationObject([FromQuery] string parent_guid, [FromBody] OrganizationObjectData data)
        //{
        //    string result = await _orgService.SaveOrganizationObject(parent_guid, data);
        //    return await _orgService.OkResult(result);
        //}

        [HttpPost("SaveUnit")]
        [ApiCache(typeof(UnitData))]
        public async Task<IActionResult> SaveUnit([FromBody] UnitData data)
        {
            string result = await _orgService.SaveUnit(data);
            return await _orgService.OkResult(result);
        }

        [HttpPost("SavePerson")]
        [ApiCache(typeof(PersonData))]
        public async Task<IActionResult> SavePerson([FromBody] PersonData data)
        {
            string result = await _orgService.SavePerson(data);
            return await _orgService.OkResult(result);
        }

        [HttpPost("DeleteUnit")]
        [ApiCache(typeof(Unit))]
        public async Task<IActionResult> DeleteUnit([FromBody] List<string> units_guid_list)
        {
            bool result = await _orgService.DeleteUnit(units_guid_list);
            return await _orgService.OkResult(result);
        }

        //[HttpPost("DeleteOrganizationOrObject")]
        //[ApiCache(typeof(OrganizationObjectData))]
        //public async Task<IActionResult> DeleteOrganizationOrObject([FromBody] List<string> organization_guid_list)
        //{
        //    bool result = await _orgService.DeleteOrganizationOrObject(organization_guid_list);
        //    return await _orgService.OkResult(result);
        //}

        [HttpPost("DeletePerson")]
        [ApiCache(typeof(Person))]
        public async Task<IActionResult> DeletePerson([FromBody] List<string> persons_guid_list)
        {
            bool result = await _orgService.DeletePerson(persons_guid_list);
            return await _orgService.OkResult(result);
        }

        //[HttpPost("UpdateDragAndDrop")]
        //[ApiCache(typeof(OrganizationObjectData))]
        //public async Task<IActionResult> UpdateDragAndDrop([FromQuery] string dest_org_guid, [FromQuery] string drag_org_guid, [FromBody] List<string> org_children_guid_list)
        //{
        //    bool result = await _orgService.DragAndDrop(dest_org_guid, drag_org_guid, org_children_guid_list);
        //    return await _orgService.OkResult(result);
        //}

        //[HttpPost("UpdateApplyProperties")]
        //[ApiCache(typeof(OrganizationObjectData))]
        //public async Task<IActionResult> UpdateApplyProperties([FromBody] (OrganizationObjectData obj, List<string> guid_list) data)
        //{
        //    bool result = await _orgService.ApplyProperties(data.obj, data.guid_list);
        //    return await _orgService.OkResult(result);
        //}

        //[HttpPost("UpdateDuplicateOrganizationObject")]
        //[ApiCache(typeof(OrganizationObjectData))]
        //public async Task<IActionResult> UpdateDuplicateOrganizationObject([FromBody] OrganizationObjectData obj, bool isRec)
        //{
        //    //TODO:replace dynamic
        //    //var obj = Util.JsonConvert<OrganizationObjectData>(organization_object);
        //    //string result = await _orgService.DuplicateOrganizationObject(obj, isRec);

        //    string result = await _orgService.DuplicateOrganizationObject(obj, isRec);
        //    return await _orgService.OkResult(result);
        //}

        //[HttpGet("GetChildOrgModels")]
        //public async Task<IActionResult> GetChildOrgModels([FromQuery] string org_obj_guid, [FromQuery] string unionGuid)
        //{
        //    var result = await _orgService.GetChildOrgModels(org_obj_guid, unionGuid);
        //    return Ok(result);
        //}

        //[HttpGet("GetOrgName")]
        //public async Task<IActionResult> GetOrgName([FromQuery] string orgObjGuid)
        //{
        //    var result = await _orgService.GetOrgName(orgObjGuid);
        //    return Ok(result);
        //}

        [HttpPost("GetOrgByModel")]
        public async Task<IActionResult> GetOrgByModel([FromBody] (Dictionary<string, List<OrgModels>> orgModelsDict, string orgObjGuid, string modelComponentGuid) data)
        {
            var result = await _orgService.GetOrgByModel(data.orgModelsDict, data.orgObjGuid, data.modelComponentGuid);
            return Ok(result);
        }

        //[HttpPost("UpdatePermissionUnits")]
        //[ApiCache(typeof(OrganizationObjectData))]
        //public async Task<IActionResult> UpdatePermissionUnits([FromQuery] Guid ownerUnit, [FromBody] string[] units)
        //{
        //    HttpStatusCode result = await _orgService.UpdatePermissionUnits(ownerUnit, units);
        //    return await _orgService.OkResult(result);
        //}

        //[HttpPost("ImportPersonalUnit")]
        //[DisableRequestSizeLimit]
        //public async Task<IActionResult> ImportPersonalUnit([FromBody] List<PersonalUnit> allPersonalUnits)
        //{
        //    int result = await _orgService.ImportPersonalUnit(allPersonalUnits);
        //    return await _orgService.OkResult(result);
        //}

        [HttpGet("GetOrganizationUnion")]
        public async Task<IActionResult> GetOrganizationUnion()
        {
            var result = await _orgService.GetOrganizationUnion();
            return Ok(result);
        }

        [HttpGet("GetOrganizationUnionDetails")]
        public async Task<IActionResult> GetOrganizationUnionDetails([FromQuery] string organizationUnionGuid)
        {
            var result = await _orgService.GetOrganizationUnionDetails(organizationUnionGuid);
            return Ok(result);
        }

        [HttpGet("DeleteOrganizationUnion")]
        public async Task<IActionResult> DeleteOrganizationUnion([FromQuery] string OrganizationUnionGuid)
        {
            bool result = await _orgService.DeleteOrganizationUnion(OrganizationUnionGuid);
            return await _orgService.OkResult(result);
        }

        [HttpPost("SaveOrganizationUnion")]
        public async Task<IActionResult> SaveOrganizationUnion([FromBody] (OrganizationUnionData ouData, string ouJsonTree) data)
        {
            string result = await _orgService.SaveOrganizationUnion(data.ouData, data.ouJsonTree);
            return await _orgService.OkResult(result);
        }

  
    }
}
