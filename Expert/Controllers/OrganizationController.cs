using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using Infrastructure.Controllers;
using Model.Entities;
using Model.Data;
using Infrastructure.Extensions;
using Swashbuckle.AspNetCore.Annotations;
using Expert.Models;
using System.Net;
using System;

namespace Expert.Controllers
{
    [Route("api/ExpertApi/[controller]")]
    [ApiController]
    public class OrganizationController : GeneralControllerBase
    {
        

        //[HttpGet("GetOrganizationTree")]
        //[SwaggerOperation(Summary = "", Description = "GetOrganizationTree")]
        //public async Task<OrgObjTree> GetOrganizationTree()
        //{
        //    string url = "organization/GetOrganizationTree";
        //    var result = await DBGate.GetAsync<OrgObjTree>(url);
        //    return result;
        //}
        [HttpGet("GetUnit")]
        [SwaggerOperation(Summary = "", Description = "GetUnit")]
        public async Task<List<UnitData>> GetUnit([FromQuery] string unitGuid)
        {
            string url = "organization/GetUnit?unitGuid=" + unitGuid;
            var result = await DBGate.GetAsync<List<UnitData>>(url);
            return result;
        }

        [HttpGet("GetUnitByAT")]
        [SwaggerOperation(Summary = "", Description = "GetUnitByAT")]
        public async Task<List<UnitData>> GetUnitByAT([FromQuery] string activityTemplateGuid)
        {
            string url = "organization/GetUnitByAT?activityTemplateGuid=" + activityTemplateGuid;
            var result = await DBGate.GetAsync<List<UnitData>>(url);
            return result;
        }

        [HttpGet("GetUnitPeoples")]
        [SwaggerOperation(Summary = "", Description = "GetUnitPeoples")]
        public async Task<List<PersonData>> GetUnitPeoples([FromQuery] string unitGuid)
        {
            string url = "organization/GetUnitPeoples?unitGuid=" + unitGuid;
            var result = await DBGate.GetAsync<List<PersonData>>(url);
            return result;
        }

        [HttpGet("GetPerson")]
        [SwaggerOperation(Summary = "", Description = "GetPerson")]
        public async Task<List<PersonData>> GetPerson([FromQuery] string personGuid)
        {
            string url = "organization/GetPerson?personGuid=" + personGuid;
            var result = await DBGate.GetAsync<List<PersonData>>(url);
            return result;
        }
        //[HttpGet("UpdateOrganizationTreeConnections")]
        //[SwaggerOperation(Summary = "", Description = "UpdateOrganizationTreeConnections")]
        //public async Task<OrganizationObjectData> UpdateOrganizationTreeConnections()
        //{
        //    string url = "organization/UpdateOrganizationTreeConnections";
        //    var result = await DBGate.GetAsync<OrganizationObjectData>(url);
        //    return result;
        //}

        [HttpGet("GetOrgObjConnection")]
        [SwaggerOperation(Summary = "", Description = "GetOrgObjConnection")]
        public async Task<EntityConnection> GetOrgObjConnection([FromQuery] string orgObjGuid)
        {
            string url = "organization/GetOrgObjConnection?orgObjGuid=" + orgObjGuid;
            var result = await DBGate.GetAsync<EntityConnection>(url);
            return result;
        }

        [HttpPost("SaveUnit")]
        [SwaggerOperation(Summary = "", Description = "SaveUnit")]
        public async Task<IActionResult> SaveUnit([FromBody] UnitData organization_object)
        {
            string url = "organization/SaveUnit";
            string guid = await DBGate.PostAsync<string>(url, organization_object);
            return Ok(new { Guid = guid ?? "" });

        }

        [HttpPost("SavePerson")]
        [SwaggerOperation(Summary = "", Description = "SavePerson")]
        public async Task<IActionResult> SavePerson([FromBody] PersonData person)
        {
            string url = "organization/SavePerson";
            string guid = await DBGate.PostAsync<string>(url, person);
            return Ok(new { Guid = guid ?? "" });

        }

        [HttpPost("DeleteUnit")]
        [SwaggerOperation(Summary = "", Description = "DeleteUnit")]
        public async Task<IActionResult> DeleteUnit([FromBody] List<string> unit_guid_list)
        {
            bool result = await DBGate.PostAsync<bool>("organization/DeleteUnit", unit_guid_list);
            return Ok(new { Success = result });

        }

        [HttpPost("DeletePerson")]
        [SwaggerOperation(Summary = "", Description = "DeletePerson")]
        public async Task<IActionResult> DeletePerson([FromBody] List<string> persons_guid_list)
        {
            bool result = await DBGate.PostAsync<bool>("organization/DeletePerson", persons_guid_list);
            return Ok(new { Success = result });

        }
       
        //[HttpPost("ApplyProperties")]
        //[SwaggerOperation(Summary = "", Description = "ApplyProperties")]
        //public async Task<bool> ApplyProperties([FromBody] (OrganizationObjectData obj, List<string> guid_list) data)
        //{
        //    bool result = await DBGate.PostAsync<bool>("organization/UpdateApplyProperties", data);
        //    return result;
        //}


        //[HttpPost("UpdatePermissionUnits")]
        //[SwaggerOperation(Summary = "", Description = "UpdatePermissionUnits")]
        //public async Task<IActionResult> UpdatePermissionUnits([FromQuery] Guid ownerUnit, [FromBody] string[] units)
        //{
        //    string url = $"organization/UpdatePermissionUnits?ownerUnit=" + ownerUnit;
        //    HttpStatusCode result = await DBGate.PostAsync<HttpStatusCode>(url, units);
        //    return Ok(result);
        //}

        //[HttpPost("ImportPersonalUnit")]
        //[DisableRequestSizeLimit]
        //[SwaggerOperation(Summary = "", Description = "ImportPersonalUnit")]
        //public async Task<IActionResult> ImportPersonalUnit(List<PersonalUnitData> allPersonalUnits)
        //{
        //    string url = $"organization/ImportPersonalUnit";
        //    var mapData = Mapper.Map<List<PersonalUnit>>(allPersonalUnits);
        //    int result = await DBGate.PostAsync<int>(url, mapData);
        //    return Ok(result);
        //}

        [HttpGet("GetOrganizationUnion")]
        [SwaggerOperation(Summary = "", Description = "GetOrganizationUnion")]
        public async Task<List<OrganizationUnionData>> GetOrganizationUnion()
        {
            string url = "organization/GetOrganizationUnion";
            var result = await DBGate.GetAsync<List<OrganizationUnion>>(url);
            var res = Mapper.Map<List<OrganizationUnionData>>(result);
            return res;
        }

        [HttpGet("GetOrganizationUnionDetails")]
        [SwaggerOperation(Summary = "", Description = "GetOrganizationUnionDetails")]
        public async Task<OrganizationUnionTreeData> GetOrganizationUnionDetails([FromQuery] string organizationUnionGuid)
        {
            var result = await DBGate.GetAsync<OrganizationUnionTreeData>("organization/GetOrganizationUnionDetails?organizationUnionGuid=" + organizationUnionGuid);
            return result;
        }

        [HttpGet("DeleteOrganizationUnion")]
        [SwaggerOperation(Summary = "", Description = "DeleteOrganizationUnion")]
        public async Task<bool> DeleteOrganizationUnion([FromQuery] string OrganizationUnionGuid)
        {
            bool result = await DBGate.GetAsync<bool>("organization/DeleteOrganizationUnion?OrganizationUnionGuid="+ OrganizationUnionGuid);
            return result;
        }

        [HttpPost("SaveOrganizationUnion")]
        [SwaggerOperation(Summary = "", Description = "SaveOrganizationUnion")]
        public async Task<string> SaveOrganizationUnion([FromBody] (OrganizationUnionData ouData, string ouJsonTree) data)
        {
            string result = await DBGate.PostAsync<string>("organization/SaveOrganizationUnion", data);
            return result;
        }

     
    }
}
