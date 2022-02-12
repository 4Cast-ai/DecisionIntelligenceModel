using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Model.Data;
using Infrastructure.Controllers;
using Infrastructure.Extensions;
using Model.Entities;

namespace Expert.Controllers
{
    [Route("api/ExpertApi/[controller]")]
    [ApiController]
    public class ModelController : GeneralControllerBase
    {
        [HttpGet("GetModels")]
        [SwaggerOperation(Summary = "", Description = "GetModels")]
        public async Task<List<object>> GetModels(string model_guid)
        {
            string url = "model/GetModels";

            if (!string.IsNullOrEmpty(model_guid))
                url += "?model_guid=" + model_guid;

            var result = await DBGate.GetAsync<List<object>>(url);
            return result;
        }

        [HttpGet("GetModelData")]
        [SwaggerOperation(Summary = "", Description = "GetModelData")]
        public async Task<object> GetModelData(string model_guid)
        {
            string url = $"model/GetModelData?model_guid={model_guid}";
            var result = await DBGate.GetAsync<object>(url);
            return result;
        }

        [HttpGet("GetFlatModel")]
        [SwaggerOperation(Summary = "", Description = "GetFlatModel")]
        public async Task<List<ModelData>> GetFlatModel(string model_component_guid)
        {
            string url = $"model/GetFlatModel?model_component_guid={model_component_guid}";
            var result = await DBGate.GetAsync<List<ModelData>>(url);
            return result;
        }

        [HttpGet("GetOriginName")]
        [SwaggerOperation(Summary = "", Description = "GetOriginName")]
        public async Task<string> GetOriginName([FromQuery] string model_component_guid)
        {
            string url = $"model/GetOriginName?model_component_guid={model_component_guid}";
            var result = await DBGate.GetAsync<string>(url);
            return result;
        }

        [HttpGet("GetFlatModelData")]
        [SwaggerOperation(Summary = "", Description = "GetFlatModelData")]
        public async Task<List<FlatModelData>> GetFlatModelData(string model_component_guid, string stopMCGuid, bool showOriginGuid)
        {
            string url = $"model/GetFlatModelData?model_component_guid={model_component_guid}&stopMCGuid={stopMCGuid}&showOriginGuid={showOriginGuid}";
            var result = await DBGate.GetAsync<List<FlatModelData>>(url);
            return result;
        }

        [HttpGet("GetOriginCondition")]
        [SwaggerOperation(Summary = "", Description = "GetOriginCondition")]
        public async Task<List<Data>> GetOriginCondition()
        {
            string url = $"model/GetOriginCondition";
            var result = await DBGate.GetAsync<List<Data>>(url);
            return result;
        }

        [HttpGet("GetDestinationCondition")]
        [SwaggerOperation(Summary = "", Description = "GetDestinationCondition")]
        public async Task<List<Data>> GetDestinationCondition()
        {
            string url = $"model/GetDestinationCondition";
            var result = await DBGate.GetAsync<List<Data>>(url);
            return result;
        }

        [HttpGet("GetThresholdLevels")]
        [SwaggerOperation(Summary = "", Description = "GetThresholdLevels")]
        public async Task<List<Data>> GetThresholdLevels()
        {
            string url = $"model/GetThresholdLevels";
            var result = await DBGate.GetAsync<List<Data>>(url);
            return result;
        }

        [HttpGet("CanActivateModel")]
        [SwaggerOperation(Summary = "", Description = "CanActivateModel")]
        public async Task<CanActivateModelData> CanActivateModel(string model_component_guid, bool activate)
        {
            string url = $"model/CanActivateModel?model_component_guid={model_component_guid}&activate={activate}";
            var result = await DBGate.GetAsync<CanActivateModelData>(url);
            return result;
        }

        [HttpGet("ChangeModelComponentStatus")]
        [SwaggerOperation(Summary = "", Description = "ChangeModelComponentStatus")]
        public async Task<bool> ChangeModelComponentStatus(string model_component_guid, int status)
        {
            string url = $"model/ChangeModelComponentStatus?model_component_guid={model_component_guid}&status={status}";
            return await DBGate.GetAsync<bool>(url);
        }

        [HttpGet("DragAndDropModel")]
        [SwaggerOperation(Summary = "", Description = "DragAndDropModel")]
        public async Task<bool> DragAndDropModel([FromQuery] string originObjguid, [FromQuery] string destinationObjguid, [FromQuery] int dragAndDropCode)
        {
            string url = "model/DragAndDropModel";

            if (!string.IsNullOrEmpty(originObjguid) && !string.IsNullOrEmpty(destinationObjguid))
                url += $"?originObjguid={originObjguid}&destinationObjguid={destinationObjguid}&dragAndDropCode={dragAndDropCode}";

            bool success = await DBGate.GetAsync<bool>(url);
            return success;
        }

        [HttpPost("SaveModelComponent")]
        [SwaggerOperation(Summary = "", Description = "SaveModelComponent")]
        public async Task<List<string>> SaveModelComponent([FromQuery] string user_guid, [FromQuery] string model_component_parent_guid, [FromBody] List<ModelData> modelDataList)
        {

            string guid;
            List<string> guid_list = new List<string>();
            var url = "";
            if (modelDataList != null && modelDataList.Count > 0)
            {
                foreach (ModelData modelDataItem in modelDataList)
                {
                    url = $"model/SaveModelComponent?user_guid={user_guid}&model_component_parent_guid={model_component_parent_guid}";
                    guid = await DBGate.PostAsync<string>(url, modelDataItem);
                    guid_list.Add(guid);
                }
            }

            return guid_list;






            //string url = $"model/SaveModelComponent?user_guid={user_guid}&model_component_parent_guid={model_component_parent_guid}";
            //string result = await DBGate.PostAsync<string>(url, model_data);
            //return result;
        }

        [HttpPost("SaveConvertionTable")]
        [SwaggerOperation(Summary = "", Description = "SaveConvertionTable")]
        public async Task<bool> SaveConvertionTable([FromQuery] string model_component_guid, [FromBody] object convertion_table)
        {
            string url = $"model/SaveConvertionTable?model_component_guid={model_component_guid}";
            bool result = await DBGate.PostAsync<bool>(url, convertion_table);
            return result;
        }

        [HttpPost("DeleteModelComponent")]
        [SwaggerOperation(Summary = "", Description = "DeleteModelComponent")]
        public async Task<bool> DeleteModelComponent([FromBody] List<ModelGuidAndTypeData> model_component_guid_list, [FromQuery] bool isRoot)
        {
            string url = "model/DeleteModelComponent";
            if (isRoot) url += "?isRoot=" + isRoot;
            bool result = await DBGate.PostAsync<bool>(url, model_component_guid_list);
            return result;
        }

        [HttpPost("AddSubModel")]
        [SwaggerOperation(Summary = "", Description = "AddSubModel")]
        public async Task<bool> AddSubModel(string destination_guid, int sub_type, [FromBody] List<string> model_guid_list)
        {
            string url = $"model/AddSubModel?destination_guid={destination_guid}&sub_type={sub_type}";
            bool result = await DBGate.PostAsync<bool>(url, model_guid_list);
            return result;
        }

        [HttpPost("LinkOrg_Model_Polygon")]
        [SwaggerOperation(Summary = "", Description = "LinkOrg_Model_Polygon")]
        public async Task<bool> LinkOrg_Model_Polygon(OrgModelPolygon data)
        {
            var result = await DBGate.PostAsync<bool>("General/LinkOrg_Model_Polygon", data);
            return result;

        }

        [HttpGet("GetLinkOrg_Model_Polygon")]
        [SwaggerOperation(Summary = "", Description = "GetOrg_Model_Polygon")]
        public async Task<OrgModelPolygonData> GetOrg_Model_Polygon(string model_guid, string org_obj_guid)
        {
            var url = $"General/GetLinkOrg_Model_Polygon?model_guid={model_guid}&org_obj_guid={org_obj_guid}";
            var result = await DBGate.GetAsync<OrgModelPolygon>(url);
            var mapRes = Mapper.Map<OrgModelPolygonData>(result);
            return mapRes;
        }

        [HttpPost("SaveModelThresholds")]
        [SwaggerOperation(Summary = "", Description = "SaveModelThresholds")]
        public async Task<string> SaveModelThresholds([FromBody] ThresholdData data)
        {
            string url = $"model/SaveModelThresholds";
            Threshold mapData = Mapper.Map<Threshold>(data);
            string result = await DBGate.PostAsync<string>(url, mapData);
            return result;
        }

        [HttpPost("DeleteModelThresholds")]
        [SwaggerOperation(Summary = "", Description = "DeleteModelThresholds")]
        public async Task<bool> DeleteModelThresholds([FromBody] List<string> thresholdGuids)
        {
            string url = $"model/DeleteModelThresholds";
            bool result = await DBGate.PostAsync<bool>(url, thresholdGuids);
            return result;
        }

        [HttpGet("GetModelThresholds")]
        [SwaggerOperation(Summary = "", Description = "GetModelThresholds")]
        public async Task<List<ThresholdData>> GetModelThresholds(string modelComponentGuid, bool isRoot)
        {
            var url = $"Model/GetModelThresholds?modelComponentGuid={modelComponentGuid}&isRoot={isRoot}";
            var result = await DBGate.GetAsync<List<Threshold>>(url);
            var mapRes = Mapper.Map<List<ThresholdData>>(result);
            return mapRes;
        }
        [HttpGet("GetTemplateSettings")]
        [SwaggerOperation(Summary = "", Description = "GetTemplateSettings")]
        public async Task<List<TemplateSettings>> GetTemplateSettings()
        {
            var url = $"Model/GetTemplateSettings";
            var result = await DBGate.GetAsync<List<TemplateSettings>>(url);
            //var mapRes = Mapper.Map<List<TemplateSettings>>(result);
            return result;
        }

        [HttpGet("GetAffectedModels")]
        [SwaggerOperation(Summary = "", Description = "GetAffectedModels")]
        public async Task<List<Data>> GetAffectedModels([FromQuery] string modelComponentGuid, [FromQuery] bool isEdit)
        {
            var url = $"Model/GetAffectedModels?modelComponentGuid={modelComponentGuid}&isEdit={isEdit}";
            var result = await DBGate.GetAsync<List<Data>>(url);
            return result;
        }

        //[HttpPost("ImportAmanData")]
        //[DisableRequestSizeLimit]
        //[SwaggerOperation(Summary = "", Description = "ImportAmanData")]
        //public async Task<IActionResult> ImportAmanData(List<AmanCsvFile> allCsvRows)
        //{
        //    string url = $"Model/ImportAmanData";
        //    bool result = await DBGate.PostAsync<bool>(url, allCsvRows);
        //    return Ok(result);
        //}

     
    }
}
