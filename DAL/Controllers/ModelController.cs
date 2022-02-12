using Dal;
using Dal.Services;
using Infrastructure.Auth;
using Model.Data;
using Model.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConvertionTableData = Model.Data.ConvertionTableData;
using ModelComponentStatus = Model.Data.ModelComponentStatus;

namespace Controllers
{
    [Route("api/DalApi/[controller]")]
    [EnableCors(PolicyTypes.ApiCorsPolicy)]
    [ApiController]
    public class ModelController : ControllerBaseAction
    {
        private readonly ModelService _modelsService;

        public ModelController(ModelService modelService)
        {
            _modelsService = modelService;
        }

        [HttpGet("GetModels")]
        public async Task<IActionResult> GetModels([FromQuery] string model_guid)
        {
            List<object> result = await _modelsService.GetModels(model_guid);
            return await _modelsService.OkResult(result);
        }

        [HttpGet("GetModelData")]
        public async Task<IActionResult> GetModelData([FromQuery] string model_guid)
        {
            object result = await _modelsService.GetModelComponentTree(model_guid);
            return await _modelsService.OkResult(result);
        }

        [HttpGet("GetFlatModel")]
        public async Task<IActionResult> GetFlatModel([FromQuery] string model_component_guid, [FromQuery] string org_obj_guid)
        {
            Dictionary<string, CalculateTreeData> result = await _modelsService.GetFlatModel(new string[] { model_component_guid }, org_obj_guid);
            return await _modelsService.OkResult(result);
        }

        [HttpGet("GetOriginName")]
        public async Task<IActionResult> GetOriginName([FromQuery] string model_component_guid)
        {
            string result = await _modelsService.GetOriginName(model_component_guid);
            return await _modelsService.OkResult(result);
        }

        
        [HttpGet("GetFlatModelData")]
        public async Task<IActionResult> GetFlatModelData([FromQuery] string model_component_guid, [FromQuery] string stopMCGuid, [FromQuery] bool showOriginGuid)
        {
            List<FlatModelData> result = await _modelsService.GetFlatModelData(model_component_guid, stopMCGuid, showOriginGuid);
            return await _modelsService.OkResult(result);
        }

        [HttpGet("GetOriginCondition")]
        public async Task<List<Data>> GetOriginCondition()
        {
            var result = await _modelsService.GetOriginCondition();
            return result;
        }

        [HttpGet("GetDestinationCondition")]
        public async Task<List<Data>> GetDestinationCondition()
        {
            var result = await _modelsService.GetDestinationCondition();
            return result;
        }

        [HttpGet("GetThresholdLevels")]
        public async Task<List<Data>> GetThresholdLevels()
        {
            var result = await _modelsService.GetThresholdLevels();
            return result;
        }

        [HttpPost("SaveModelComponent")]
        public async Task<IActionResult> SaveModelComponent([FromQuery] string user_guid, [FromQuery] string model_component_parent_guid, [FromBody] ModelData model_data, [FromQuery] bool interfaceFlag)
        {
            string result = await _modelsService.SaveModelComponent(user_guid, model_component_parent_guid, model_data, interfaceFlag);
            return await _modelsService.OkResult(result);
        }

        [HttpPost("SaveModelTree")]
        public async Task<IActionResult> SaveModelTree([FromQuery] string user_guid, [FromBody] ModelComponentTreeData model_data_tree)
        {
            bool result = await _modelsService.SaveModelTree(user_guid, null, model_data_tree);
            return await _modelsService.OkResult(result);
        }

        [HttpPost("SaveConvertionTable")]
        public async Task<IActionResult> SaveConvertionTable([FromQuery] string model_component_guid, [FromBody] List<ConvertionTableData> convertion_table)
        {
            bool result = _modelsService.SaveConvertionTable(model_component_guid, convertion_table);
            return await _modelsService.OkResult(result);
        }

        [HttpPost("DeleteModelComponent")]
        public async Task<IActionResult> DeleteModelComponent([FromBody] List<ModelGuidAndTypeData> model_component_guid_list, [FromQuery] bool isRoot)
        {
            bool result = await _modelsService.DeleteModelComponentAsync(model_component_guid_list, isRoot);
            return await _modelsService.OkResult(result);
        }

        [HttpGet("DragAndDropModel")]
        public async Task<IActionResult> DragAndDropModel([FromQuery] string originObjguid, [FromQuery] string destinationObjguid, [FromQuery] int dragAndDropCode)
        {
            bool result = await _modelsService.DragAndDropModel(originObjguid, destinationObjguid, dragAndDropCode);
            return await _modelsService.OkResult(result);
        }

        [HttpPost("AddSubModel")]
        public async Task<IActionResult> AddSubModel([FromQuery] string destination_guid, [FromQuery] int sub_type, [FromQuery] string user_guid, [FromBody] List<string> model_guid_list)
        {
            bool result = await _modelsService.AddSubModel(destination_guid, sub_type, user_guid, model_guid_list);
            return await _modelsService.OkResult(result);
        }

        [HttpGet("CanActivateModel")]
        public async Task<IActionResult> CanActivateModel([FromQuery] string model_component_guid, [FromQuery] bool activate)
        {
            CanActivateModelData result = await _modelsService.CanActivateModel(model_component_guid);

            if (activate)
            {
                if (result.incorrect_weight_list.Count == 0 && result.not_active_list.Count == 0 && result.not_in_form_list.Count == 0)
                {
                    await _modelsService.ChangeModelComponentStatus(model_component_guid, ModelComponentStatus.active);
                }
            }

            return await _modelsService.OkResult(result);
        }

        [HttpGet("ChangeModelComponentStatus")]
        public async Task<IActionResult> ChangeModelComponentStatus([FromQuery] string model_component_guid, [FromQuery] int status)
        {
            bool result = await _modelsService.ChangeModelComponentStatus(model_component_guid, (ModelComponentStatus)status);
            return await _modelsService.OkResult(result);
        }

        [HttpGet("GetDefaultConvertionTable")]
        public async Task<List<ConvertionTableData>> GetDefaultConvertionTable([FromQuery] int measuring_unit, [FromQuery] string model_component_guid)
        {
            var res = _modelsService.GetDefaultConvertionTable(measuring_unit, model_component_guid);
            return await Task.FromResult(res);
        }

        [HttpPost("SaveModelThresholds")]
        public async Task<IActionResult> SaveModelThresholds([FromBody] Threshold data)
        {
            string result = await _modelsService.SaveModelThresholds(data);
            return await _modelsService.OkResult(result);
        }

        [HttpPost("DeleteModelThresholds")]
        public async Task<IActionResult> DeleteModelThresholds([FromBody] List<string> thresholdGuids)
        {
            bool result = await _modelsService.DeleteModelThresholds(thresholdGuids);
            return await _modelsService.OkResult(result);
        }

        [HttpGet("GetModelThresholds")]
        public async Task<List<Threshold>> GetModelThresholds([FromQuery] string modelComponentGuid, [FromQuery] bool isRoot)
        {
            var res = await _modelsService.GetModelAffectsThresholds(modelComponentGuid, false, isRoot);
            return await Task.FromResult(res);
        }


        [HttpGet("GetTemplateSettings")]
        public async Task<List<TemplateSettings>> GetTemplateSettings()
        {
            var res = await _modelsService.GetTemplateSettings();
            return await Task.FromResult(res);
        }

        [HttpGet("GetAffectedModels")]
        public async Task<List<Data>> GetAffectedModels([FromQuery] string modelComponentGuid, [FromQuery] bool isEdit)
        {
            var res = await _modelsService.GetAffectedModels(modelComponentGuid, isEdit);
            return await Task.FromResult(res);
        }

        //[HttpPost("ImportAmanData")]
        //public async Task<IActionResult> ImportAmanData([FromBody] List<AmanCsvFile> allCsvRows)
        //{
        //    bool result = await _modelsService.ImportAmanData(allCsvRows);
        //    return await _modelsService.OkResult(result);
        //}

       
    }
}
