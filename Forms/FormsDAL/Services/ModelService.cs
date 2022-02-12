using Infrastructure;
using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Services;
using Model.Data;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConvertionTableData = Model.Data.ConvertionTableData;
using ModelComponentSource = Model.Data.ModelComponentSource;
using ModelComponentStatus = Model.Data.ModelComponentStatus;
using FormsDal.Contexts;

namespace FormsDal.Services
{
    public class ModelService : BaseService<FormsManageDBContext>
    {
        private GeneralService _generalService => this.GetChildService<GeneralService>();
        //private ReportService _reportService => this.GetChildService<ReportService>();

        private readonly Serilog.ILogger _logger = GeneralContext.GetService<Serilog.ILogger>();

        private readonly IAppConfig _config = GeneralContext.GetService<IAppConfig>();

        public async Task<List<object>> GetModels(string model_guid)
        {
            //select model component that their source is model or model_root
            List<object> ModelsResult = await (from mc in DbContext.ModelComponent
                                               join m_s in DbContext.ModelStructure on mc.ModelComponentGuid equals m_s.ModelComponentGuid into structure
                                               from ms in structure.DefaultIfEmpty()
                                               where (mc.Source == (int)ModelComponentSource.model || mc.Source == (int)ModelComponentSource.model_root) &&
                                               (ms == null || ms.ModelComponentType == null || ms.ModelComponentType == (int)ModelComponentTypes.copy) &&
                                               (string.IsNullOrEmpty(model_guid) || mc.ModelComponentGuid == model_guid)
                                               orderby mc.Name
                                               select new
                                               {
                                                   model_guid = mc.ModelComponentGuid,
                                                   model_modified_date = mc.ModifiedDate,
                                                   model_name = mc.Name,
                                                   model_status = DbUtils.TranslateStatusFromDB(mc.Status.HasValue ? mc.Status.Value.ToString() : string.Empty),
                                                   model_description = mc.ModelDescriptionText,
                                                   source = mc.Source,
                                                   model_type = mc.Source == (int)ModelComponentSource.model
                                                       ? "base" : mc.Source == (int)ModelComponentSource.model_root
                                                           ? "operational" : string.Empty,
                                                   model_component_type = ms != null ? ms.ModelComponentType : null,
                                                   status = mc.Status,
                                                   descriptions = DbContext.ModelDescription.Where(d => d.ModelComponentGuid == mc.ModelComponentGuid).Select(d => new DescriptionsData { description_guid = d.DescriptionGuid.Value, name = d.DescriptionGu.Name }).ToList()
                                               }).ToListAsync<object>();

            return await Task.FromResult(ModelsResult);
        }

        public async Task<object> GetModelComponentTree(string model_component_guid)
        {
            ModelComponentTreeData model_component_tree = new ModelComponentTreeData
            {
                data = new ModelData()
            };

            var model_component = await GetModel(model_component_guid);
            if (model_component != null)
            {
                model_component_tree.data = Mapper.Map<ModelData>(model_component.MC);

                if (model_component_tree.data == null)
                    return model_component_tree;

                await SetNodePropertiesAsync(model_component_tree);

                ModelStructure model_structure = model_component.MS;

                if (model_structure != null)
                {
                    model_component_tree.data.parent_guid = model_structure.ModelComponentParentGuid;
                    model_component_tree.data.model_component_type = model_structure.ModelComponentType;
                }

                //model_component_tree.data.model_orgs_list = DbContext.OrganizationObjectConnection
                //                                            .Where(ooc => ooc.ModelComponentGuid == model_component_guid)
                //                                            .Select(ooc => ooc.OrgObjGuid).ToList();

                List<string> unitsGuids = DbContext.Unit.Where(ooc => ooc.DefaultModelGuid == model_component_guid)
                                                           .Select(ooc => ooc.UnitGuid).ToList();

                List<string> personsGuids = DbContext.Person.Where(ooc => ooc.JobtitleGuid == model_component_guid)
                                                          .Select(ooc => ooc.PersonGuid).ToList();

                List<string> modelDescEntitiesGuids = (from md in DbContext.ModelDescription
                                                       join ed in DbContext.EntityDescription on md.DescriptionGuid equals ed.DescriptionGuid
                                                       where md.ModelComponentGuid == model_component_guid
                                                       select ed.EntityGuid).ToList();

                model_component_tree.data.model_units_list = unitsGuids.Union(personsGuids).Union(modelDescEntitiesGuids).ToList();
                //model_component_tree.data.model_orgs_list = DbContext.Unit
                //                                           .Where(ooc => ooc.DefaultModelGuid == model_component_guid)
                //                                           .Select(ooc => ooc.UnitGuid).ToList();

                await UpdateTree(model_component_tree);
            }
            else
            {
                await SetNodePropertiesAsync(model_component_tree);
            }

            return model_component_tree;
        }

        public async Task UpdateTree(ModelComponentTreeData model_component_tree)
        {
            var children = await GetModelChildren(model_component_tree.data.model_component_guid);

            if (children == null || !children.Any())
                return;

            model_component_tree.children = new List<ModelComponentTreeData>();
            ModelComponentTreeData childTree;

            foreach (var child in children)
            {
                childTree = new ModelComponentTreeData();
                childTree.data = Mapper.Map<ModelData>(child.MC);

                await SetNodePropertiesAsync(childTree);

                childTree.data.parent_guid = model_component_tree.data.model_component_guid;
                childTree.data.model_component_type = model_component_tree.data.model_component_type;

                ModelStructure model_structure = child.MS;

                if (model_structure != null)
                {
                    childTree.data.model_component_type = model_structure.ModelComponentType;
                }

                model_component_tree.children.Add(childTree);

                await UpdateTree(childTree);
            }
        }

        public async Task<string> GetOriginName(string modelComponentGuid)
        {
            string originName = string.Empty;
            var model_structure = DbContext.ModelStructure.FirstOrDefault(ms => ms.ModelComponentGuid == modelComponentGuid);

            if (model_structure != null)
            {
                if (model_structure.ModelComponentType.HasValue && (model_structure.ModelComponentType == (int)ModelComponentTypes.copyWithSource || model_structure.ModelComponentType == (int)ModelComponentTypes.reference))
                {
                    ModelStructure originStructure = DbContext.ModelStructure.Where(ms => ms.ModelComponentGuid == model_structure.ModelComponentOrigionGuid).FirstOrDefault();
                    if (originStructure != null)
                    {
                        ModelComponent mc = FindModelParentRoot(originStructure.ModelComponentParentGuid);
                        originName = mc != null ? mc.Name : null;
                    }
                    else
                    {
                        ModelComponent originComponent = DbContext.ModelComponent.Where(mc => mc.ModelComponentGuid == model_structure.ModelComponentOrigionGuid).FirstOrDefault();
                        originName = originComponent != null ? originComponent.Name : null;
                    }
                }
            }

            return await Task.FromResult(originName);
        }

        public async Task<ModelComponent> GetFatherModel(string model_component_guid)
        {
            ModelComponent m = await DbContext.ModelStructure
                .Where(ms => ms.ModelComponentGuid == model_component_guid)
                .Select(ms => ms.ModelComponentParentGu)
                .FirstOrDefaultAsync();
            return m ?? null;
        }

        public async Task SetNodePropertiesAsync(ModelComponentTreeData modelComponentTree)
        {
            if (!string.IsNullOrEmpty(modelComponentTree.data.model_component_guid))
            {
                modelComponentTree.data.description_list = await GetModelDescription(modelComponentTree.data.model_component_guid);
                modelComponentTree.data.convertion_table = await GetConvertionDetails(modelComponentTree.data.model_component_guid);
            }
            else
            {
                modelComponentTree.data.description_list = new List<DescriptionsData>();
                modelComponentTree.data.convertion_table = new List<Model.Data.ConvertionTableData>();
            }

            modelComponentTree.data.measuring_unit = await _generalService.GetMeasuringUnit();
            modelComponentTree.data.calender_rollup = await _generalService.GetCalenderRollup();
            modelComponentTree.data.rollup_method = await _generalService.GetRollupMethod();
        }

        public async Task<string[]> GetModelList(string[] modelChildGuids, string[] totalResults = null)
        {
            totalResults = totalResults ?? new string[] { };

            string[] children = await DbContext.ModelStructure
                .Where(ms => modelChildGuids.Contains(ms.ModelComponentParentGuid))
                .OrderBy(ms => ms.ModelComponentGu.ModelComponentOrder)
                .Select(ms => ms.ModelComponentGuid)
                .ToArrayAsync();

            if (children == null || !children.Any())
                return totalResults;

            totalResults = totalResults.Concat(children).ToArray();
            return await GetModelList(children.ToArray(), totalResults);
        }

        public async Task<Dictionary<string, CalculateTreeData>> GetFlatModel(string[] model_guid_list, string org_obj_guid, List<ThresholdData> allModelThresholds = null, Dictionary<string, CalculateTreeData> result = null, bool isFirstLoop = true)
        {
            result = result ?? new Dictionary<string, CalculateTreeData>();

            //add root
            if (isFirstLoop)
            {
                string g = model_guid_list[0];
                allModelThresholds = await GetAllModelThresholds(g, true);


                CalculateTreeData root = DbContext.ModelComponent
                                .Where(mc => mc.ModelComponentGuid == model_guid_list[0])
                                .Select(mc => new CalculateTreeData(mc, mc.ModelStructureModelComponentGu.FirstOrDefault())).FirstOrDefault();

                root.polygonGuid = DbContext.OrgModelPolygon
                             .Where(mc => mc.ModelComponentGuid == model_guid_list[0] && mc.UnitGuid == org_obj_guid)
                             .Select(mc => mc).FirstOrDefault()?.PolygonGuid;

                KeyValuePair<string, CalculateTreeData> rootPair = new KeyValuePair<string, CalculateTreeData>(model_guid_list[0], root);

                await SetExtraModelData(rootPair, allModelThresholds);

                result.Add(rootPair.Key, rootPair.Value);
            }

            var children = DbContext.ModelStructure
                           .Where(ms => model_guid_list.Contains(ms.ModelComponentParentGuid))
                           .OrderBy(ms => ms.ModelComponentGu.ModelComponentOrder)
                           .Select(ms => new KeyValuePair<string, CalculateTreeData>(ms.ModelComponentGu.ModelComponentGuid, new CalculateTreeData(ms.ModelComponentGu, ms)));

            if (children == null || !children.Any())
                return result;

            result = result.Concat(children).ToDictionary(x => x.Key, x => x.Value);

            foreach (var model in result)
            {
                await SetExtraModelData(model, allModelThresholds);
            }

            return await GetFlatModel(children.Select(c => c.Key).ToArray(), org_obj_guid, allModelThresholds, result, false);
        }

        public async Task SetExtraModelData(KeyValuePair<string, CalculateTreeData> model, List<ThresholdData> allModelThresholds)
        {
            if (model.Value.data.model_data.model_component_type.HasValue &&
                    (model.Value.data.model_data.model_component_type.Value == (int)ModelComponentTypes.copyWithSource ||
                        model.Value.data.model_data.model_component_type.Value == (int)ModelComponentTypes.reference))
            {
                model.Value.data.model_data.base_origin_model_component_guid = GetModelOriginGuidUntilCopy(model.Key);
                model.Value.data.model_data.allUpOrigins = (await GetOriginsForThreshold(new string[] { model.Key })).ToArray();
            }
            if (model.Value.data.model_data.model_component_type.HasValue &&
                    model.Value.data.model_data.model_component_type.Value == (int)ModelComponentTypes.reference)
            {
                model.Value.data.model_data.origin_threshold_list = allModelThresholds.Where(t => model.Value.data.model_data.allUpOrigins.Contains(t.ModelComponentOriginGuid)).Select(x => { x.IsReference = x.ModelComponentOriginGuid != model.Key; return x; }).ToList();
                model.Value.data.model_data.threshold_list = allModelThresholds.Where(t => model.Value.data.model_data.allUpOrigins.Contains(t.ModelComponentDestinationGuid)).Select(x => { x.IsReference = x.ModelComponentDestinationGuid != model.Key; return x; }).ToList();
            }
            else
            {
                model.Value.data.model_data.origin_threshold_list = allModelThresholds.Where(t => t.ModelComponentOriginGuid == model.Key).ToList();
                model.Value.data.model_data.threshold_list = allModelThresholds.Where(t => t.ModelComponentDestinationGuid == model.Key).ToList();
            }
        }

        public async Task<List<ThresholdData>> GetAllModelThresholds(string modelComponentGuid, bool withOrigin = false)
        {
            List<Threshold> res = await GetModelAffectsThresholds(modelComponentGuid, withOrigin, true);
            List<ThresholdData> allModelThresholds = Mapper.Map<List<ThresholdData>>(res);
            return allModelThresholds;
        }

        public async Task<List<FlatModelData>> GetFlatModelData(string model_component_guid, string stopMCGuid, bool showOriginGuid = true, int level = 1, List<FlatModelData> modelComponentList = null)
        {
            if (modelComponentList == null)
            {
                modelComponentList = new List<FlatModelData>();
                string modelName = DbContext.ModelComponent.Find(model_component_guid).Name;
                modelComponentList.Add(new FlatModelData() { id = model_component_guid, text = modelName, level = level });
            }

            if (stopMCGuid != model_component_guid)
            {
                level += 1;
                var children = DbContext.ModelStructure
                               .Where(ms => ms.ModelComponentParentGuid == model_component_guid)
                               .Select(ms => new FlatModelData()
                               {
                                   id = showOriginGuid && ms.ModelComponentType.HasValue && (ms.ModelComponentType == (int)ModelComponentTypes.reference || ms.ModelComponentType == (int)ModelComponentTypes.copyWithSource) ? ms.ModelComponentOrigionGuid : ms.ModelComponentGuid,
                                   text = ms.ModelComponentGu.Name,
                                   level = level,
                                   source = ms.ModelComponentGu.Source,
                                   isRef = ms.ModelComponentType == (int)ModelComponentTypes.reference ? true : false
                               })
                               .ToList();

                foreach (var child in children)
                {
                    modelComponentList.Add(child);
                    await GetFlatModelData(child.id, stopMCGuid, showOriginGuid, level, modelComponentList);
                }
            }
            else
            {
                modelComponentList.RemoveAt(modelComponentList.Count - 1);
            }

            return modelComponentList;
        }

        public async Task<List<Data>> GetOriginCondition()
        {
            List<Data> results = new List<Data>();
            GeneralContext.Cache.TryGetValue(Constants.OriginCondition, out results);

            if (results == null)
            {
                results = await DbContext.ThresholdOriginCondition.Select(x => new Data() { id = x.OriginConditionId.ToString(), text = x.Name }).ToListAsync();
                GeneralContext.Cache.Set(Constants.OriginCondition, results, GeneralContext.CacheEntryOptions);
            }

            return results;
        }

        public async Task<List<Data>> GetDestinationCondition()
        {
            List<Data> results = new List<Data>();
            GeneralContext.Cache.TryGetValue(Constants.DestinationCondition, out results);

            if (results == null)
            {
                results = await DbContext.ThresholdDestinationCondition.Select(x => new Data() { id = x.DestinationConditionId.ToString(), text = x.Name }).ToListAsync();
                GeneralContext.Cache.Set(Constants.DestinationCondition, results, GeneralContext.CacheEntryOptions);
            }

            return results;
        }

        public async Task<List<Data>> GetThresholdLevels()
        {
            List<Data> results = new List<Data>();
            GeneralContext.Cache.TryGetValue(Constants.ThresholdLevels, out results);

            if (results == null)
            {
                results = await DbContext.ThresholdLevels.Select(x => new Data() { id = x.LevelId.ToString(), text = x.Name }).ToListAsync();
                GeneralContext.Cache.Set(Constants.ThresholdLevels, results, GeneralContext.CacheEntryOptions);
            }

            return results;
        }

        public string GetModelOriginGuid(string model_component_guid)
        {
            string origin_guid = null;

            string origin = DbContext.ModelStructure.Where(ms => ms.ModelComponentGuid == model_component_guid).Select(ms => ms.ModelComponentOrigionGuid).FirstOrDefault();
            if (origin == null)
            {
                return model_component_guid;
            }
            else
            {
                origin_guid = GetModelOriginGuid(origin);
            }

            return origin_guid;
        }

        public string GetModelOriginGuidUntilCopy(string model_component_guid)
        {
            string origin_guid = null;

            var origin = DbContext.ModelStructure.Where(ms => ms.ModelComponentGuid == model_component_guid).FirstOrDefault();
            if (origin == null || origin.ModelComponentOrigionGuid == null)
            {
                return model_component_guid;
            }
            else
            {
                if (origin.ModelComponentType == (int)ModelComponentTypes.copyWithSource)
                {
                    return model_component_guid;
                }
                else
                {
                    origin_guid = GetModelOriginGuidUntilCopy(origin.ModelComponentOrigionGuid);
                }
            }

            return origin_guid;
        }

        public async Task<string> SaveModelComponent(string user_guid, string model_component_parent_guid, ModelData model_data, bool interfaceFlag = false)
        {
            string result = null;

            try
            {
                _logger.Information("start SaveModelComponent Information");

                //if model_data.model_component_guid not exist
                if (model_data != null && string.IsNullOrEmpty(model_data.model_component_guid))
                {
                    //create new model_component
                    result = CreateModelComponent(user_guid, model_component_parent_guid, model_data, null, null, interfaceFlag);

                    //find parent guid references
                    List<string> parentReferences = DbContext.ModelStructure.Where(ms => ms.ModelComponentType == (int)ModelComponentTypes.reference && ms.ModelComponentOrigionGuid == model_component_parent_guid).Select(ms => ms.ModelComponentGuid).ToList();

                    if (parentReferences != null && parentReferences.Count > 0)
                    {
                        foreach (var pr in parentReferences)
                        {
                            CreateModelComponent(user_guid, pr, model_data, result, (int)ModelComponentTypes.reference, interfaceFlag);
                        }
                    }
                }
                else
                {
                    //update existing model_component
                    result = UpdateModelComponent(user_guid, model_data);

                    List<ModelStructure> coppied_model_component_list = await GetConnectModels(model_data.model_component_guid, true);

                    foreach (var coppied_model_component in coppied_model_component_list)
                    {
                        ModelData md = new ModelData(model_data);
                        md.model_component_guid = coppied_model_component.ModelComponentGuid;
                        md.convertion_table = model_data.convertion_table;
                        //do update recursive
                        UpdateModelComponent(user_guid, md, true);
                    }
                }
            }
            catch (Exception ex)
            {
                string err = ex.ToString();// ex.GetApiMessageInfo();
                _logger.Error(err);
            }

            return await Task.FromResult(result);
        }

        public async Task<bool> SaveModelTree(string user_guid, string parent_guid, ModelComponentTreeData model_data_tree)
        {
            string new_guid = await SaveModelComponent(user_guid, parent_guid, model_data_tree.data);

            if (new_guid == string.Empty)
                return false;

            foreach (var child in model_data_tree.children)
            {
                await SaveModelTree(user_guid, new_guid, child);
            }

            return true;
        }

        public string CreateModelComponent(string user_guid, string model_component_parent_guid, ModelData model_data, string origin_guid = null, int? sub_type = null, bool interfaceFlag = false)
        {
            string guid = string.Empty;

            //Save to Model_Component table
            ModelComponent model_component = new ModelComponent();

            model_component.Name = model_data.name;
            model_component.ProfessionalInstruction = model_data.professional_instruction;
            model_component.ModelDescriptionText = model_data.model_description_text;
            model_component.Source = model_data.source;
            model_component.MetricMeasuringUnit = model_data.metric_measuring_unit;
            model_component.MetricRollupMethod = model_data.metric_rollup_method;
            model_component.MetricCalenderRollup = model_data.metric_calender_rollup;
            model_component.MetricRequired = model_data.metric_required.HasValue ? model_data.metric_required.Value : false;
            model_component.MetricFormula = model_data.metric_formula;
            model_component.MetricIsVisible = model_data.metric_is_visible.HasValue ? model_data.metric_is_visible.Value : true;
            model_component.MetricNotDisplayIfIrrelevant = model_data.metric_not_display_if_irrelevant.HasValue ? model_data.metric_not_display_if_irrelevant.Value : false;
            model_component.MetricExpiredPeriod = !string.IsNullOrEmpty(model_data.metric_expired_period) ? model_data.metric_expired_period : "m12";
            model_component.MetricExpiredPeriodSecondary = !string.IsNullOrEmpty(model_data.metric_expired_period_secondary) ? model_data.metric_expired_period_secondary : "m36";
            model_component.MetricCommentObligationLevel = model_data.metric_comment_obligation_level.HasValue ? model_data.metric_comment_obligation_level.Value : 0;
            model_component.MetricGradualDecreasePrecent = model_data.metric_gradual_decrease_precent.HasValue ? model_data.metric_gradual_decrease_precent.Value : 0;
            model_component.MetricGradualDecreasePeriod = model_data.metric_gradual_decrease_period.HasValue ? model_data.metric_gradual_decrease_period.Value : 0;
            model_component.MetricMinimumFeeds = model_data.metric_minimum_feeds.HasValue ? model_data.metric_minimum_feeds.Value : 0;
            model_component.ShowOrigionValue = model_data.show_origion_value.HasValue ? model_data.show_origion_value.Value : false;
            model_component.Weight = model_data.weight.HasValue ? model_data.weight.Value : 100;//Set to given value
            model_component.CalcAsSum = model_data.calcAsSum;
            model_component.GroupChildren = model_data.groupChildren;

            model_component.TemplateType = model_data.TemplateType.HasValue ? model_data.TemplateType.Value : 0;
            if (model_component.Source == (int)ModelComponentSource.metric)
            {
                model_component.MetricSource = model_data.metric_source.HasValue ? model_data.metric_source.Value : 1;
            }
            else
            {
                model_component.MetricSource = null;
            }

            model_component.ModelComponentGuid = Util.CreateGuid().ToString();//Create new guid
            model_component.CreateDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
            model_component.ModifiedDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
            model_component.ModifiedUserGuid = user_guid;//Set current user

            //if model_component set to 'model-root'
            if (model_component.Source == (int)ModelComponentSource.model_root && sub_type != (int)ModelComponentTypes.reference && sub_type != (int)ModelComponentTypes.copyWithSource)
            {
                model_component.Status = (int)ModelComponentStatus.draft;//Set status to draft
            }
            else
            {
                model_component.Status = (int)ModelComponentStatus.active;//Set status to active
            }

            if (!string.IsNullOrEmpty(model_component_parent_guid))
            {
                //get model_component count from existing hierarchy
                var model_structure_by_parent = DbContext.ModelStructure.Where(ms => ms.ModelComponentParentGuid == model_component_parent_guid).Select(ms => ms.ModelComponentGuid);
                model_component.ModelComponentOrder = model_structure_by_parent.Count() + 1;//Set the order to model_structure_by_parent_count+1
                                                                                            //get parent model component
                var parent_model_component = DbContext.ModelComponent.Where(mc => mc.ModelComponentGuid == model_component_parent_guid).FirstOrDefault();
            }
            else
            {
                model_component.ModelComponentOrder = 1;//Set to default value
            }

            DbContext.ModelComponent.Add(model_component);
            DbContext.SaveChanges();

            //Save to Model_Structure table
            if (!string.IsNullOrEmpty(model_component_parent_guid))
            {
                ModelStructure model_structure = new ModelStructure();

                model_structure.ModelComponentGuid = model_component.ModelComponentGuid;
                model_structure.ModelComponentParentGuid = model_component_parent_guid;

                if (!string.IsNullOrEmpty(origin_guid) && sub_type != null)
                {
                    if (sub_type == (int)ModelComponentTypes.copy)
                        model_structure.ModelComponentOrigionGuid = null;// origin_guid;//GetModelOriginGuid(origin_guid);
                    else if (sub_type == (int)ModelComponentTypes.copyWithSource)
                        model_structure.ModelComponentOrigionGuid = origin_guid;
                    else if (sub_type == (int)ModelComponentTypes.reference && model_data.model_component_type == (int)ModelComponentTypes.copyWithSource)
                    {
                        model_structure.ModelComponentOrigionGuid = origin_guid;
                    }
                    else
                    {
                        model_structure.ModelComponentOrigionGuid = GetModelOriginGuidUntilCopy(origin_guid);
                    }

                    var origins = string.Empty;
                    var exist_ms = DbContext.ModelStructure.FirstOrDefault(x => x.ModelComponentGuid == origin_guid);
                    if (exist_ms != null && exist_ms.AllOrigins != null)
                    {
                        origins = exist_ms.AllOrigins;
                    }

                    model_structure.AllOrigins = origins + origin_guid + ";";
                    model_structure.ModelComponentType = model_data.model_component_type == (int)ModelComponentTypes.reference ? (int)ModelComponentTypes.reference : sub_type;
                }

                DbContext.ModelStructure.Add(model_structure);
                DbContext.SaveChanges();
            }

            //Save to Model_Description table
            if (model_data.description_list != null && model_data.description_list.Count > 0)
            {
                foreach (DescriptionsData description in model_data.description_list)
                {
                    ModelDescription model_description = new ModelDescription();

                    model_description.ModelComponentGuid = model_component.ModelComponentGuid;
                    model_description.DescriptionGuid = description.description_guid;

                    DbContext.ModelDescription.Add(model_description);
                }

                DbContext.SaveChanges();
            }

            //Save to Convertion_tables table
            if (model_data.convertion_table != null && model_data.convertion_table.Count > 0)
            {
                for (int i = 0; i < model_data.convertion_table.Count; i++)
                {
                    Model.Entities.ConvertionTable convertion_item = new Model.Entities.ConvertionTable();

                    convertion_item.ModelComponentGuid = model_component.ModelComponentGuid;
                    convertion_item.LevelId = model_data.convertion_table[i].level_id;
                    convertion_item.StartRange = model_data.convertion_table[i].start_range;
                    convertion_item.EndRange = model_data.convertion_table[i].end_range;
                    convertion_item.ConversionTableFinalScore = model_data.convertion_table[i].conversion_table_final_score;
                    convertion_item.ConversionTableCreateDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
                    convertion_item.ConversionTableModifiedDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
                    convertion_item.ConversionTableStatus = "draft";
                    convertion_item.StartRangeScoreDisplayed = null;
                    convertion_item.EndRangeScoreDisplayed = null;
                    convertion_item.ConversionTableScoreOrder = "0";

                    DbContext.ConvertionTable.Add(convertion_item);
                }

                DbContext.SaveChanges();
            }
            else
            {
                //Add default valus to Convertion_tables
                List<ConvertionTableData> convertion_data = new List<ConvertionTableData>();

                if (model_data.metric_measuring_unit.HasValue)
                {
                    convertion_data = GetDefaultConvertionTable(model_data.metric_measuring_unit.Value, model_component.ModelComponentGuid);//selected measuring_unit
                }
                else if ((model_data.source == (int)ModelComponentSource.model || model_data.source == (int)ModelComponentSource.model_root) && model_data.metric_rollup_method != 1)
                {
                    if (interfaceFlag)
                    {
                        convertion_data = GetDefaultConvertionTable(5, model_component.ModelComponentGuid);//qualitative1
                    }
                    else
                    {
                        convertion_data = GetDefaultConvertionTable(3, model_component.ModelComponentGuid);//precentage
                    }
                }
                else
                {
                    convertion_data = GetDefaultConvertionTable(3, model_component.ModelComponentGuid);//precentage
                }

                //var data = Mapper.Map<List<Convertion_table>>(convertion_data);
                var data = convertion_data.Select(x => new Model.Entities.ConvertionTable()
                {
                    ModelComponentGuid = x.model_component_guid,
                    LevelId = x.level_id,
                    StartRange = x.start_range,
                    EndRange = x.end_range,
                    ConversionTableModifiedDate = x.conversion_table_modified_date,
                    ConversionTableStatus = x.conversion_table_status,
                    ConversionTableCreateDate = x.conversion_table_create_date,
                    StartRangeScoreDisplayed = x.start_range_score_displayed,
                    EndRangeScoreDisplayed = x.end_range_score_displayed,
                    ConversionTableScoreOrder = x.conversion_table_score_order,
                    ConversionTableFinalScore = x.conversion_table_final_score
                }).ToList();

                DbContext.ConvertionTable.AddRange(data);
                DbContext.SaveChanges();
            }

            //save to Organization_Object_Connection
            //SaveOrganizationsObjectConnectionToModel(model_data.model_orgs_list, model_component.ModelComponentGuid);

            guid = model_component.ModelComponentGuid;

            return guid;
        }

        public List<ConvertionTableData> GetDefaultConvertionTable(int measuring_unit, string model_component_guid)
        {
            List<ConvertionTableData> data = new List<ConvertionTableData>();
            string date = Util.ConvertDateToString(DateTime.Now);

            switch (measuring_unit)
            {
                case 1://quantitative
                    {
                        ConvertionTableData ct1 = new ConvertionTableData
                        {
                            level_id = 1,
                            start_range = 0,
                            end_range = 49,
                            conversion_table_final_score = 0,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct2 = new ConvertionTableData
                        {
                            level_id = 2,
                            start_range = 50,
                            end_range = 59,
                            conversion_table_final_score = 55,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct3 = new ConvertionTableData
                        {
                            level_id = 3,
                            start_range = 60,
                            end_range = 79,
                            conversion_table_final_score = 70,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct4 = new ConvertionTableData
                        {
                            level_id = 4,
                            start_range = 80,
                            end_range = 89,
                            conversion_table_final_score = 85,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct5 = new ConvertionTableData
                        {
                            level_id = 5,
                            start_range = 90,
                            end_range = 100,
                            conversion_table_final_score = 100,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        data.AddRange(new List<ConvertionTableData> { ct1, ct2, ct3, ct4, ct5 });
                        break;
                    }
                case 2://binary
                    {
                        ConvertionTableData ct1 = new ConvertionTableData
                        {
                            level_id = 1,
                            start_range = 0,
                            end_range = 0,
                            conversion_table_final_score = 0,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct2 = new ConvertionTableData
                        {
                            level_id = 5,
                            start_range = 1,
                            end_range = 1,
                            conversion_table_final_score = 100,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        data.AddRange(new List<ConvertionTableData> { ct1, ct2 });
                        break;
                    }
                case 3://percentage
                    {
                        ConvertionTableData ct1 = new ConvertionTableData
                        {
                            level_id = 1,
                            start_range = 0,
                            end_range = 49,
                            conversion_table_final_score = 0,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct2 = new ConvertionTableData
                        {
                            level_id = 2,
                            start_range = 50,
                            end_range = 59,
                            conversion_table_final_score = 0,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct3 = new ConvertionTableData
                        {
                            level_id = 3,
                            start_range = 60,
                            end_range = 79,
                            conversion_table_final_score = 0,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct4 = new ConvertionTableData
                        {
                            level_id = 4,
                            start_range = 80,
                            end_range = 89,
                            conversion_table_final_score = 0,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct5 = new ConvertionTableData
                        {
                            level_id = 5,
                            start_range = 90,
                            end_range = 100,
                            conversion_table_final_score = 0,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        //Convertion_tables ct6 = new Convertion_tables
                        //{
                        //    LevelId = 6,
                        //    StartRange = 101,
                        //    EndRange = 120,
                        //    conversion_table_final_score = 85,
                        //    ModelComponentGuid = model_component_guid,
                        //    ConversionTableCreateDate = Util.ConvertDateToString(DateTime.Now),
                        //    conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                        //    ConversionTableStatus ="draft"
                        //};

                        //Convertion_tables ct7 = new Convertion_tables
                        //{
                        //    LevelId = 7,
                        //    StartRange = 121,
                        //    EndRange = 150,
                        //    conversion_table_final_score = 70,
                        //    ModelComponentGuid = model_component_guid,
                        //    ConversionTableCreateDate = Util.ConvertDateToString(DateTime.Now),
                        //    conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                        //    ConversionTableStatus ="draft"
                        //};

                        //data.AddRange(new List<Convertion_tables> { ct1, ct2, ct3, ct4, ct5, ct6, ct7 });
                        data.AddRange(new List<ConvertionTableData> { ct1, ct2, ct3, ct4, ct5 });
                        break;
                    }
                case 4://qualitative
                    {
                        ConvertionTableData ct1 = new ConvertionTableData
                        {
                            level_id = 1,
                            start_range = 1,
                            end_range = 1,
                            conversion_table_final_score = 0,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct2 = new ConvertionTableData
                        {
                            level_id = 2,
                            start_range = 2,
                            end_range = 2,
                            conversion_table_final_score = 55,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct3 = new ConvertionTableData
                        {
                            level_id = 3,
                            start_range = 3,
                            end_range = 3,
                            conversion_table_final_score = 70,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct4 = new ConvertionTableData
                        {
                            level_id = 4,
                            start_range = 4,
                            end_range = 4,
                            conversion_table_final_score = 85,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct5 = new ConvertionTableData
                        {
                            level_id = 5,
                            start_range = 5,
                            end_range = 5,
                            conversion_table_final_score = 100,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        data.AddRange(new List<ConvertionTableData> { ct1, ct2, ct3, ct4, ct5 });
                        break;
                    }

                case 5://qualitative1-7
                    {
                        ConvertionTableData ct1 = new ConvertionTableData
                        {
                            level_id = 1,
                            start_range = 1,
                            end_range = 1,
                            conversion_table_final_score = 0,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct2 = new ConvertionTableData
                        {
                            level_id = 2,
                            start_range = 2,
                            end_range = 3,
                            conversion_table_final_score = 55,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct3 = new ConvertionTableData
                        {
                            level_id = 3,
                            start_range = 4,
                            end_range = 4,
                            conversion_table_final_score = 70,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct4 = new ConvertionTableData
                        {
                            level_id = 4,
                            start_range = 5,
                            end_range = 6,
                            conversion_table_final_score = 85,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        ConvertionTableData ct5 = new ConvertionTableData
                        {
                            level_id = 5,
                            start_range = 7,
                            end_range = 7,
                            conversion_table_final_score = 100,
                            model_component_guid = model_component_guid,
                            conversion_table_create_date = date,
                            conversion_table_modified_date = date,
                            conversion_table_status = "draft"
                        };

                        data.AddRange(new List<ConvertionTableData> { ct1, ct2, ct3, ct4, ct5 });
                        break;
                    }
            }

            return data;
        }

        public async Task<List<Model.Data.ConvertionTableData>> GetConvertionDetails(string model_component_guid)
        {
            var convertionData = await DbContext.ConvertionTable
                    .Where(c => c.ModelComponentGuid == model_component_guid)
                    .OrderBy(x => x.LevelId).ToListAsync();

            List<ConvertionTableData> list = Mapper.Map<List<ConvertionTableData>>(convertionData);
            return list;
        }

        public string UpdateModelComponent(string user_guid, ModelData model_data, bool is_copied = false)
        {
            string guid = string.Empty;

            //Get Model_Component row by model_component_guid
            var model_component = DbContext.ModelComponent.Find(model_data.model_component_guid);

            //Update model_component
            model_component.Name = model_data.name;
            model_component.ProfessionalInstruction = model_data.professional_instruction;
            model_component.ModelDescriptionText = model_data.model_description_text;
            model_component.Source = model_data.source;
            model_component.MetricMeasuringUnit = model_data.metric_measuring_unit;
            model_component.MetricRollupMethod = model_data.metric_rollup_method;
            model_component.MetricCalenderRollup = model_data.metric_calender_rollup;
            model_component.MetricRequired = model_data.metric_required.HasValue ? model_data.metric_required.Value : false;
            model_component.MetricFormula = model_data.metric_formula;
            model_component.MetricIsVisible = model_data.metric_is_visible.HasValue ? model_data.metric_is_visible.Value : true;
            model_component.MetricNotDisplayIfIrrelevant = model_data.metric_not_display_if_irrelevant.HasValue ? model_data.metric_not_display_if_irrelevant.Value : false;
            model_component.MetricExpiredPeriod = !string.IsNullOrEmpty(model_data.metric_expired_period) ? model_data.metric_expired_period : "m12";
            model_component.MetricExpiredPeriodSecondary = !string.IsNullOrEmpty(model_data.metric_expired_period_secondary) ? model_data.metric_expired_period_secondary : "m36";
            model_component.MetricCommentObligationLevel = model_data.metric_comment_obligation_level.HasValue ? model_data.metric_comment_obligation_level.Value : 0;
            model_component.MetricGradualDecreasePrecent = model_data.metric_gradual_decrease_precent.HasValue ? model_data.metric_gradual_decrease_precent.Value : 0;
            model_component.MetricGradualDecreasePeriod = model_data.metric_gradual_decrease_period.HasValue ? model_data.metric_gradual_decrease_period.Value : 0;
            model_component.MetricMinimumFeeds = model_data.metric_minimum_feeds.HasValue ? model_data.metric_minimum_feeds.Value : 0;
            model_component.ShowOrigionValue = model_data.show_origion_value.HasValue ? model_data.show_origion_value.Value : false;
            model_component.CalcAsSum = model_data.calcAsSum;
            model_component.GroupChildren = model_data.groupChildren;
            model_component.TemplateType = model_data.TemplateType.HasValue ? model_data.TemplateType.Value : 0;

            if (!is_copied)
            {
                model_component.Weight = model_data.weight.HasValue ? model_data.weight.Value : 100;//Set to given value
            }

            if (model_component.Source == (int)ModelComponentSource.metric)
            {
                model_component.MetricSource = model_data.metric_source.HasValue ? model_data.metric_source.Value : 1;
            }
            else
            {
                model_component.MetricSource = null;
            }

            model_component.ModifiedDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
            model_component.ModifiedUserGuid = user_guid;//Set current user

            DbContext.ModelComponent.Update(model_component);
            DbContext.SaveChanges();

            //Delete from Model_Description by model_component_guid
            var descriptions = DbContext.ModelDescription.Where(md => md.ModelComponentGuid == model_data.model_component_guid);

            foreach (var model_description in descriptions)
            {
                DbContext.ModelDescription.Remove(model_description);
            }

            DbContext.SaveChanges();

            //Save to Model_Description table
            if (model_data.description_list != null && model_data.description_list.Count > 0)
            {
                foreach (DescriptionsData description in model_data.description_list)
                {
                    ModelDescription model_description = new ModelDescription();

                    model_description.ModelComponentGuid = model_data.model_component_guid;
                    model_description.DescriptionGuid = description.description_guid;

                    DbContext.ModelDescription.Add(model_description);
                }

                DbContext.SaveChanges();
            }

            //Save to Convertion_tables table
            if (model_data.convertion_table != null && model_data.convertion_table.Count > 0)
            {
                //Delete from Convertion_Table by model_component_guid
                var convertions = DbContext.ConvertionTable.Where(ct => ct.ModelComponentGuid == model_data.model_component_guid);

                foreach (var convertion in convertions)
                {
                    DbContext.ConvertionTable.Remove(convertion);
                }

                DbContext.SaveChanges();

                for (int i = 0; i < model_data.convertion_table.Count; i++)
                {
                    Model.Entities.ConvertionTable convertion_item = new Model.Entities.ConvertionTable();

                    convertion_item.ModelComponentGuid = model_data.model_component_guid;
                    convertion_item.LevelId = model_data.convertion_table[i].level_id;
                    convertion_item.StartRange = model_data.convertion_table[i].start_range;
                    convertion_item.EndRange = model_data.convertion_table[i].end_range;
                    convertion_item.ConversionTableFinalScore = model_data.convertion_table[i].conversion_table_final_score;
                    convertion_item.ConversionTableCreateDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
                    convertion_item.ConversionTableModifiedDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
                    convertion_item.ConversionTableStatus = "draft";
                    convertion_item.StartRangeScoreDisplayed = null;
                    convertion_item.EndRangeScoreDisplayed = null;
                    convertion_item.ConversionTableScoreOrder = "0";

                    DbContext.ConvertionTable.Add(convertion_item);
                }

                DbContext.SaveChanges();
            }

            //save to Organization_Object_Connection
            //SaveOrganizationsObjectConnectionToModel(model_data.model_orgs_list, model_component.ModelComponentGuid);

            guid = model_data.model_component_guid;

            return guid;
        }

        //public void SaveOrganizationsObjectConnectionToModel(List<string> organization_list, string model_component_guid)
        //{
        //    if (organization_list != null && organization_list.Count > 0)
        //    {
        //        //remove current connection for this organizations
        //        var current_orgs_to_model = DbContext.OrganizationObjectConnection.Where(ooc => ooc.ModelComponentGuid == model_component_guid).ToList();
        //        DbContext.OrganizationObjectConnection.RemoveRange(current_orgs_to_model);
        //        DbContext.SaveChanges();

        //        //add connection for this organizations
        //        for (int i = 0; i < organization_list.Count; i++)
        //        {
        //            OrganizationObjectConnection ooc = new OrganizationObjectConnection();
        //            ooc.OrgObjGuid = organization_list[i];
        //            ooc.ModelComponentGuid = model_component_guid;
        //            DbContext.OrganizationObjectConnection.Add(ooc);
        //            DbContext.SaveChanges();
        //        }
        //    }
        //}

        public bool SaveConvertionTable(string model_component_guid, List<Model.Data.ConvertionTableData> convertion_table)
        {
            // Util.LogMessage("SaveConvertionTable", "Start!", System.Diagnostics.TraceEventType.Verbose, null);
            bool res;

            try
            {
                //    
                {
                    //if model_component_guid not exist in Convertion_Tables
                    var convertions = DbContext.ConvertionTable.Where(ct => ct.ModelComponentGuid == model_component_guid);
                    if (convertions == null || !convertions.Any())
                    {
                        //create new Convertion_Tables
                        res = CreateConvertionTable(model_component_guid, convertion_table);
                    }
                    else
                    {
                        //update existing Convertion_Tables
                        res = UpdateConvertionTable(model_component_guid, convertion_table);
                    }
                }

                return res;
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
                return false;
            }
        }

        public bool CreateConvertionTable(string model_component_guid, List<Model.Data.ConvertionTableData> convertion_table)
        {
            // Util.LogMessage("CreateConvertionTable", "Start!", System.Diagnostics.TraceEventType.Verbose, null);

            bool success = false;

            try
            {
                //    
                {
                    //Save to Convertion_tables table
                    if (convertion_table != null && convertion_table.Count > 0)
                    {
                        for (int i = 0; i < convertion_table.Count; i++)
                        {
                            Model.Entities.ConvertionTable convertion_item = new Model.Entities.ConvertionTable();

                            convertion_item.ModelComponentGuid = model_component_guid;
                            convertion_item.LevelId = convertion_table[i].level_id;
                            convertion_item.StartRange = convertion_table[i].start_range;
                            convertion_item.EndRange = convertion_table[i].end_range;
                            convertion_item.ConversionTableFinalScore = convertion_table[i].conversion_table_final_score;
                            convertion_item.ConversionTableCreateDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
                            convertion_item.ConversionTableModifiedDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
                            convertion_item.ConversionTableStatus = "draft";
                            convertion_item.StartRangeScoreDisplayed = null;
                            convertion_item.EndRangeScoreDisplayed = null;
                            convertion_item.ConversionTableScoreOrder = "0";

                            DbContext.ConvertionTable.Add(convertion_item);
                        }

                        DbContext.SaveChanges();
                    }

                    success = true;
                    // Util.LogMessage("CreateConvertionTable", "End! save succeeded!", System.Diagnostics.TraceEventType.Verbose, null);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
            }

            return success;
        }

        public bool UpdateConvertionTable(string model_component_guid, List<Model.Data.ConvertionTableData> convertion_table)
        {
            // Util.LogMessage("UpdateConvertionTable", "Start!", System.Diagnostics.TraceEventType.Verbose, null);

            bool success = false;

            try
            {
                //    
                {
                    //Delete from Convertion_Table by model_component_guid
                    var convertions = DbContext.ConvertionTable.Where(ct => ct.ModelComponentGuid == model_component_guid);

                    foreach (var convertion in convertions)
                    {
                        DbContext.ConvertionTable.Remove(convertion);
                    }

                    DbContext.SaveChanges();
                    // Util.LogMessage("UpdateConvertionTable", "Pass DbContext.Convertion_tables.Remove(convertion);", System.Diagnostics.TraceEventType.Verbose, null);

                    //Save to Convertion_tables table
                    if (convertion_table != null && convertion_table.Count > 0)
                    {
                        for (int i = 0; i < convertion_table.Count; i++)
                        {
                            Model.Entities.ConvertionTable convertion_item = new Model.Entities.ConvertionTable();

                            convertion_item.ModelComponentGuid = model_component_guid;
                            convertion_item.LevelId = convertion_table[i].level_id;
                            convertion_item.StartRange = convertion_table[i].start_range;
                            convertion_item.EndRange = convertion_table[i].end_range;
                            convertion_item.ConversionTableFinalScore = convertion_table[i].conversion_table_final_score;
                            convertion_item.ConversionTableCreateDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
                            convertion_item.ConversionTableModifiedDate = Util.ConvertDateToString(DateTime.Now);//Set the date to now
                            convertion_item.ConversionTableStatus = "draft";
                            convertion_item.StartRangeScoreDisplayed = null;
                            convertion_item.EndRangeScoreDisplayed = null;
                            convertion_item.ConversionTableScoreOrder = "0";

                            DbContext.ConvertionTable.Add(convertion_item);
                        }

                        DbContext.SaveChanges();
                    }

                    success = true;
                    // Util.LogMessage("UpdateConvertionTable", "End! save succeeded!", System.Diagnostics.TraceEventType.Verbose, null);
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
            }

            return success;
        }

        public async Task<bool> DeleteModelComponentAsync(List<ModelGuidAndTypeData> model_component_guid_list, bool isRoot = false)
        {
            foreach (var model_component in model_component_guid_list)
            {
                //loop on all copied models and delete them

                //Get all copied models from Model_Structure
                List<ModelGuidAndTypeData> coppied_model_component_list = (await GetConnectModels(model_component.model_component_guid, false)).Select(ms => new ModelGuidAndTypeData() { model_component_guid = ms.ModelComponentGuid }).ToList();
                //List<ModelGuidAndTypeData> coppied_model_component_list = DbContext.ModelStructure
                //                                             .Where(ms => ms.ModelComponentOrigionGuid == model_component.model_component_guid &&
                //                                                   (ms.ModelComponentType == (int)ModelComponentTypes.copyWithSource ||
                //                                                    ms.ModelComponentType == (int)ModelComponentTypes.reference))
                //                                             .Select(ms => new ModelGuidAndTypeData()
                //                                             {
                //                                                 model_component_guid = ms.ModelComponentGuid,
                //                                                 model_component_parent_guid = ms.ModelComponentParentGuid,
                //                                                 model_component_type = ms.ModelComponentType
                //                                             }).ToList();

                //do delete recursive
                await DeleteModelComponentAsync(coppied_model_component_list);

                //Loop all hierarchy recursive
                await DeleteModelComponentChildren(model_component.model_component_guid);

                //Delete root level
                var model_component_root = await GetModel(model_component.model_component_guid);

                if (model_component_root != null || isRoot)
                {
                    await DeleteModelComponentFromTables(model_component_root.MC);
                }
            }

            return true;
        }

        public async Task DeleteModelComponentChildren(string model_component_guid)
        {
            var children = await GetModelChildren(model_component_guid);
            foreach (var child in children)
            {
                await DeleteModelComponentChildren(child.MC.ModelComponentGuid);
                //Delete from all relevant table
                await DeleteModelComponentFromTables(child.MC);
            }
        }

        public async Task DeleteModelComponentFromTables(ModelComponent MC)
        {
            if (MC != null)
            {
                //remove from Model_Description
                var descriptions = DbContext.ModelDescription.Where(x => x.ModelComponentGuid == MC.ModelComponentGuid);
                DbContext.ModelDescription.RemoveRange(descriptions);
                //DbContext.SaveChanges();

                //remove from Convertion_Tables
                var convertions = DbContext.ConvertionTable.Where(x => x.ModelComponentGuid == MC.ModelComponentGuid);
                DbContext.ConvertionTable.RemoveRange(convertions);
                //DbContext.SaveChanges();

                //remove from Organization_Object_Connections
                //var organizationsConnections = DbContext.OrganizationObjectConnection.Where(x => x.ModelComponentGuid == MC.ModelComponentGuid);
                //DbContext.OrganizationObjectConnection.RemoveRange(organizationsConnections);
                //DbContext.SaveChanges();

                //remove from Threshold
                var thresholds = DbContext.Threshold.Where(x => x.ModelComponentOriginGuid == MC.ModelComponentGuid || x.ModelComponentDestinationGuid == MC.ModelComponentGuid);
                DbContext.Threshold.RemoveRange(thresholds);
                //DbContext.SaveChanges();

                //remove from Saved_report_Connection
                //var saved_report_connection_list = DbContext.SavedReportConnection.Where(x => x.Focus == MC.ModelComponentGuid || x.Comment == MC.ModelComponentGuid);
                //DbContext.SavedReportConnection.RemoveRange(saved_report_connection_list);
                ////DbContext.SaveChanges();

                //remove from Saved_reports
                var saved_reports = DbContext.SavedReports.Where(x => x.ModelComponentGuid == MC.ModelComponentGuid).ToList();
                foreach (var reprot in saved_reports)
                {
                    //await _reportService.DeleteSavedReport(reprot.ReportGuid);
                }

                //remove from Form_Element_Connection
                var formElementConnection = DbContext.FormElementConnection.Where(x => x.ModelComponentGuid == MC.ModelComponentGuid);
                DbContext.FormElementConnection.RemoveRange(formElementConnection);
                //DbContext.SaveChanges();

                //remove from Form_Template_Structure
                var formTemplateStructure = DbContext.FormTemplateStructure.Where(x => x.ModelComponentGuid == MC.ModelComponentGuid);
                DbContext.FormTemplateStructure.RemoveRange(formTemplateStructure);
                //DbContext.SaveChanges();

                //remove from Calculate_Score
                var calculateScore = DbContext.CalculateScore.Where(x => x.ModelComponentGuid == MC.ModelComponentGuid);
                DbContext.CalculateScore.RemoveRange(calculateScore);
                //DbContext.SaveChanges();

                //remove from Score
                var score = DbContext.Score.Where(x => x.ModelComponentGuid == MC.ModelComponentGuid);
                DbContext.Score.RemoveRange(score);
                //DbContext.SaveChanges();

                //remove from Model_Structure
                List<ModelStructure> model_structure_list = DbContext.ModelStructure
                    .Where(ms => ms.ModelComponentGuid == MC.ModelComponentGuid ||
                           ms.ModelComponentParentGuid == MC.ModelComponentGuid ||
                           ms.ModelComponentOrigionGuid == MC.ModelComponentGuid).ToList();

                DbContext.ModelStructure.RemoveRange(model_structure_list);
                //DbContext.SaveChanges();

                //remove from Model_component
                DbContext.ModelComponent.Remove(MC);
                await DbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> DragAndDropModel(string origin_guid, string destination_guid, int dragAndDropCode)
        {
            if (origin_guid == destination_guid)
                return false;

            var o = await GetModel(origin_guid);
            var origin = o.MC;
            var origin_structure = o.MS;

            var d = await GetModel(destination_guid);
            var destination = d.MC;
            var destination_structure = d.MS;

            string origin_parent_guid = origin_structure.ModelComponentParentGuid;

            switch ((DragAndDropType)dragAndDropCode)
            {
                case DragAndDropType.into:
                    {
                        //get destination model children

                        var modelChildren = await GetModelChildren(destination.ModelComponentGuid);
                        var destination_model_children_count = modelChildren.Count;

                        ModelStructure mst = new ModelStructure();
                        mst.ModelComponentGuid = origin.ModelComponentGuid;
                        mst.ModelComponentParentGuid = destination.ModelComponentGuid;

                        DbContext.ModelStructure.Add(mst);
                        DbContext.SaveChanges();

                        //change parent guid
                        DbContext.ModelStructure.Remove(origin_structure);
                        DbContext.SaveChanges();

                        //set to last order by parent destination model children count + 1
                        origin.ModelComponentOrder = destination_model_children_count + 1;
                        DbContext.ModelComponent.Update(origin);
                        DbContext.SaveChanges();


                        //update origin items order
                        var parent_ModelChildren = await GetModelChildren(origin_parent_guid);
                        var parent_origin_model_children = parent_ModelChildren.Select(x => x.MC).ToList();

                        int l = parent_origin_model_children.Count;
                        for (int i = 0; i < l; i++)
                        {
                            parent_origin_model_children[i].ModelComponentOrder = i + 1;

                            DbContext.ModelComponent.Update(parent_origin_model_children[i]);
                            DbContext.SaveChanges();
                        }

                        break;
                    }
                case DragAndDropType.above:
                case DragAndDropType.below:
                    {
                        List<ModelComponent> parent_destination_model_children = new List<ModelComponent>();

                        if (origin_parent_guid != destination_structure.ModelComponentParentGuid)
                        {
                            //get parent destination model children
                            parent_destination_model_children =
                                (await GetModelChildren(destination_structure.ModelComponentParentGuid))?
                                    .Select(x => x.MC)
                                    .ToList();

                            ModelStructure mst = new ModelStructure
                            {
                                ModelComponentGuid = origin_structure.ModelComponentGuid,
                                ModelComponentParentGuid = destination_structure.ModelComponentParentGuid
                            };

                            DbContext.ModelStructure.Add(mst);
                            DbContext.SaveChanges();

                            //change parent guid
                            DbContext.ModelStructure.Remove(origin_structure);
                            DbContext.SaveChanges();

                            //change order
                            parent_destination_model_children.RemoveAll(x => x.ModelComponentGuid == origin.ModelComponentGuid);

                            //change order
                            int position = destination.ModelComponentOrder.HasValue ? destination.ModelComponentOrder.Value : 1;
                            if ((DragAndDropType)dragAndDropCode == DragAndDropType.above)
                                position -= 1;

                            parent_destination_model_children.Insert(position, origin);

                            int length = parent_destination_model_children.Count;
                            for (int i = 0; i < length; i++)
                            {
                                parent_destination_model_children[i].ModelComponentOrder = i + 1;

                                DbContext.ModelComponent.Update(parent_destination_model_children[i]);
                                DbContext.SaveChanges();
                            }
                        }
                        else//same parent
                        {
                            //get parent origin/destination model children
                            var parent_destination_modelChildren = await GetModelChildren(origin_parent_guid);
                            parent_destination_model_children = parent_destination_modelChildren.Select(x => x.MC).ToList();

                            //change order
                            parent_destination_model_children.RemoveAll(x => x.ModelComponentGuid == origin.ModelComponentGuid);

                            int position = destination.ModelComponentOrder.HasValue ? destination.ModelComponentOrder.Value : 1;
                            position -= 1;

                            parent_destination_model_children.Insert(position, origin);

                            int length = parent_destination_model_children.Count;
                            for (int i = 0; i < length; i++)
                            {
                                parent_destination_model_children[i].ModelComponentOrder = i + 1;

                                DbContext.ModelComponent.Update(parent_destination_model_children[i]);
                                DbContext.SaveChanges();
                            }
                        }
                        break;
                    }
            }

            return true;
        }

        public async Task<bool> AddSubModel(string destination_guid, int sub_type, string user_guid, List<string> model_guid_list)
        {
            string new_guid = string.Empty;
            string parent_guid = null;
            Dictionary<string, string> copy_parent = new Dictionary<string, string>();

            //sort array by parents first
            for (int i = model_guid_list.Count - 1; i >= 0; i--)
            {
                ModelAndStructure model_component_and_structure = await GetModel(model_guid_list[i]);

                if (model_component_and_structure != null && model_component_and_structure.MS != null)
                {
                    if (model_guid_list.Contains(model_component_and_structure.MS.ModelComponentParentGuid))
                    {
                        int index = model_guid_list.IndexOf(model_component_and_structure.MS.ModelComponentParentGuid);
                        if (index > i)
                        {
                            string val = model_guid_list[i];
                            model_guid_list.Remove(model_guid_list[i]);//remove
                            model_guid_list.Insert(index, val);//add to end
                        }
                    }
                }
            }

            int idx = 0;

            foreach (string model_component_guid in model_guid_list)
            {
                ModelAndStructure model_component_and_structure = await GetModel(model_component_guid);

                ModelData data = new ModelData(model_component_and_structure.MC);
                data.model_component_type = model_component_and_structure.MS != null ? model_component_and_structure.MS.ModelComponentType : null;
                data.description_list = DbContext.ModelDescription.Where(md => md.ModelComponentGuid == model_component_guid)
                                            .Select(md => new DescriptionsData()
                                            {
                                                description_guid = md.DescriptionGuid.HasValue ? md.DescriptionGuid.Value : 0
                                            }).ToList();
                data.convertion_table = DbContext.ConvertionTable.Where(ct => ct.ModelComponentGuid == model_component_guid)
                                            .Select(md => new ConvertionTableData(md)).ToList();

                if (model_component_and_structure != null && model_component_and_structure.MS != null && model_component_and_structure.MC != null)
                {
                    if (model_guid_list.Contains(model_component_and_structure.MS.ModelComponentParentGuid))
                    {
                        //parent exist in array
                        parent_guid = copy_parent[model_component_and_structure.MS.ModelComponentParentGuid];
                    }
                    else
                    {
                        //parent NOT exist in array
                        parent_guid = destination_guid;
                    }

                    new_guid = CreateModelComponent(user_guid, parent_guid, data, model_component_guid, sub_type);
                    copy_parent.Add(model_component_guid, new_guid);
                }
                else if (idx == 0)
                {
                    parent_guid = destination_guid;
                    new_guid = CreateModelComponent(user_guid, parent_guid, data, model_component_guid, sub_type);
                    copy_parent.Add(model_component_guid, new_guid);
                }

                idx++;
            }

            return true;
        }

        public async Task<ModelAndStructure> GetModel(string model_component_guid)
        {
            var model = await DbContext.ModelComponent.FindAsync(model_component_guid);
            var structure = await DbContext.ModelStructure.FirstOrDefaultAsync(ms => ms.ModelComponentGuid == model_component_guid);
            var result = new ModelAndStructure(model, structure);

            return result;
        }

        public async Task<List<ModelAndStructure>> GetModelChildren(string model_component_guid)
        {
            var results = await (from ms in DbContext.ModelStructure
                                 join mc in DbContext.ModelComponent on ms.ModelComponentGuid equals mc.ModelComponentGuid
                                 where ms.ModelComponentParentGuid == model_component_guid
                                 orderby mc.ModelComponentOrder
                                 select new ModelAndStructure(mc, ms)).ToListAsync();

            return results;
        }

        public async Task<CanActivateModelData> CanActivateModel(string model_component_guid, CanActivateModelData data = null)
        {
            if (data == null)
                data = new CanActivateModelData();

            //get model component
            var model_component = DbContext.ModelComponent.Where(mc => mc.ModelComponentGuid == model_component_guid)
                                                    .Select(mc => new { MC = mc, MS = mc.ModelStructureModelComponentGu.FirstOrDefault() }).FirstOrDefault();

            //check if convertion table exist
            var convertionTable = DbContext.ConvertionTable.FirstOrDefault(ct => ct.ModelComponentGuid == model_component_guid);
            if (convertionTable == null)
            {
                //add convertion table

                //get default by measuring unit
                int measuring = model_component.MC.MetricMeasuringUnit.HasValue ? model_component.MC.MetricMeasuringUnit.Value : 3;
                var ct = GetDefaultConvertionTable(measuring, model_component_guid);
                var ctMapper = ct.Select(x => new Model.Entities.ConvertionTable()
                {
                    ModelComponentGuid = x.model_component_guid,
                    LevelId = x.level_id,
                    StartRange = x.start_range,
                    EndRange = x.end_range,
                    ConversionTableModifiedDate = x.conversion_table_modified_date,
                    ConversionTableStatus = x.conversion_table_status,
                    ConversionTableCreateDate = x.conversion_table_create_date,
                    StartRangeScoreDisplayed = x.start_range_score_displayed,
                    EndRangeScoreDisplayed = x.end_range_score_displayed,
                    ConversionTableScoreOrder = x.conversion_table_score_order,
                    ConversionTableFinalScore = x.conversion_table_final_score
                }).ToList();
                DbContext.ConvertionTable.AddRange(ctMapper);
                DbContext.SaveChanges();
            }

            //get all children
            var children = DbContext.ModelStructure
                            .Where(ms => ms.ModelComponentParentGuid == model_component_guid)
                            .OrderBy(ms => ms.ModelComponentGu.ModelComponentOrder)
                            .Select(ms => new { MC = ms.ModelComponentGu, MS = ms }).ToList();

            //add incorrect weight
            if (children.Count() > 0)
            {
                double sum_weight = children.Select(c => c.MC.Weight).Sum();

                if (sum_weight != 100)
                {
                    data.incorrect_weight_list.Add(new Data() { id = model_component_guid, text = model_component.MC.Name + ":" + sum_weight.ToString() });
                }
            }

            //add not in form
            string guid = model_component_guid;

            if (model_component.MS != null && (model_component.MS.ModelComponentType == (int)ModelComponentTypes.reference || model_component.MS.ModelComponentType == (int)ModelComponentTypes.copyWithSource))
            {
                guid = model_component.MS.ModelComponentOrigionGuid;
            }

            string rootOriginGuid = GetModelOriginGuid(guid);

            var form_item = DbContext.FormTemplateStructure.Where(fts => fts.ModelComponentGuid == guid || fts.ModelComponentGuid == rootOriginGuid).FirstOrDefault();

            List<string> personnelModelMetrics = new List<string> { Constants.RequiredModelGuid, Constants.ActualModelGuid, Constants.CertificatedModelGuid, Constants.RefreshedModelGuid };

            if (form_item == null && model_component.MC.Source == (int)ModelComponentSource.metric && !personnelModelMetrics.Contains(rootOriginGuid))
            {
                data.not_in_form_list.Add(new Data() { id = model_component_guid, text = model_component.MC.Name });
            }

            //loop all children
            foreach (var child in children)
            {
                //add not active model component
                if (child.MC.Status != (int)ModelComponentStatus.active)
                {
                    data.not_active_list.Add(new Data() { id = child.MC.ModelComponentGuid, text = child.MC.Name });
                }

                await CanActivateModel(child.MC.ModelComponentGuid, data);
            }

            return data;
        }

        public async Task<bool> ChangeModelComponentStatus(string model_component_guid, ModelComponentStatus status)
        {
            ModelComponent model_component = DbContext.ModelComponent.Where(mc => mc.ModelComponentGuid == model_component_guid).FirstOrDefault();
            model_component.Status = (int)status;

            DbContext.ModelComponent.Update(model_component);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<DescriptionsData>> GetModelDescription(string model_guid)
        {
            List<DescriptionsData> list = new List<DescriptionsData>();

            //list = await (from md in DbContext.ModelDescription
            //              join d in DbContext.Description on md.DescriptionGuid equals d.DescriptionGuid
            //              where md.ModelComponentGuid == model_guid
            //              select new DescriptionsData()
            //              {
            //                  description_guid = d.DescriptionGuid,
            //                  name = d.Name,
            //                  remark = d.Remark,
            //                  creator = d.Creator,
            //                  modify = d.Modify,
            //                  type_guid = d.TypeGuid,
            //                  creator_user_guid = d.CreatorUserGuid,
            //                  modify_user_guid = d.ModifyUserGuid
            //              })
            //            .ToListAsync();

            list = ((await (from md in DbContext.ModelDescription
                            join d in DbContext.Description on md.DescriptionGuid equals d.DescriptionGuid
                            where md.ModelComponentGuid == model_guid
                            select d).ToArrayAsync()).Select(d => new DescriptionsData()
                            {
                                description_guid = d.DescriptionGuid,
                                name = d.Name,
                                remark = d.Remark,
                                creator = d.Creator,
                                modify = d.Modify,
                                creator_user_guid = d.CreatorUserGuid,
                                modify_user_guid = d.ModifyUserGuid
                            })).ToList();

            return list;
        }

        public async Task<string> SaveModelThresholds(Threshold threshold)
        {
            var existThreshold = DbContext.Threshold.Find(threshold.ThresholdGuid);
            //await UpdateThresholdScore(threshold);
            if (existThreshold == null)
            {
                //create
                threshold.ThresholdGuid = Util.CreateGuid().ToString();
                threshold.CreateDate = Util.ConvertDateToString(DateTime.Now);
                DbContext.Threshold.Add(threshold);
            }
            else
            {
                //update
                existThreshold.ModifiedDate = Util.ConvertDateToString(DateTime.Now);
                existThreshold.ModelComponentOriginGuid = threshold.ModelComponentOriginGuid;
                existThreshold.OriginCondition = threshold.OriginCondition;
                existThreshold.OriginScore = threshold.OriginScore;
                existThreshold.OriginLevel = threshold.OriginLevel;
                existThreshold.IsOriginLevel = threshold.IsOriginLevel;
                existThreshold.ModelComponentDestinationGuid = threshold.ModelComponentDestinationGuid;
                existThreshold.DestinationCondition = threshold.DestinationCondition;
                existThreshold.DestinationScore = threshold.DestinationScore;
                existThreshold.DestinationLevel = threshold.DestinationLevel;
                existThreshold.IsDestinationLevel = threshold.IsDestinationLevel;
                existThreshold.AutoMessage = threshold.AutoMessage;
                existThreshold.FreeMessage = threshold.FreeMessage;
                existThreshold.Recommendations = threshold.Recommendations;
                existThreshold.Explanations = threshold.Explanations;
            }

            await DbContext.SaveChangesAsync();
            return threshold.ThresholdGuid;
        }

        public async Task<bool> DeleteModelThresholds(List<string> thresholdGuids)
        {
            Threshold t;
            foreach (var thresholdGuid in thresholdGuids)
            {
                t = DbContext.Threshold.Find(thresholdGuid);
                DbContext.Threshold.Remove(t);
                DbContext.SaveChanges();
            }

            return await Task.FromResult(true);
        }

        public async Task<List<Threshold>> GetModelAffectsThresholds(string modelComponentGuid, bool withOrigin, bool isRoot)
        {
            List<Threshold> thresholds;

            if (isRoot)
            {
                //get model list with origins
                string[] flatModel = await GetModelList(new string[] { modelComponentGuid });

                if (withOrigin)
                    flatModel = flatModel.Concat(await GetOriginsForThreshold(flatModel)).ToArray();

                thresholds = DbContext.Threshold.Where(t => flatModel.Contains(t.ModelComponentOriginGuid)).ToList();
            }
            else
            {
                thresholds = DbContext.Threshold.Where(t => t.ModelComponentOriginGuid == modelComponentGuid).ToList();
            }

            return await Task.FromResult(thresholds);
        }

        public async Task<List<TemplateSettings>> GetTemplateSettings()
        {
            List<TemplateSettings> templateSettings;
            try
            {
                templateSettings = DbContext.TemplateSettings.Select(x => x).ToList();

            }
            catch (Exception ex)
            {

                throw;
            }



            return await Task.FromResult(templateSettings);
        }

        public async Task<List<ModelStructure>> GetConnectModels(string modelComponentGuid, bool isEdit)
        {
            string originGuid = modelComponentGuid;
            ModelStructure root = null;

            if (isEdit)
            {
                originGuid = GetModelOriginGuid(modelComponentGuid);
                if (originGuid != modelComponentGuid)
                {
                    root = new ModelStructure() { ModelComponentGuid = originGuid, ModelComponentParentGuid = originGuid };
                }
            }

            string[] origins = await GetAllOrigins(new string[] { originGuid });
            List<string> allOrigins = origins.Concat(new string[] { originGuid }).ToList();
            var coppiedList = DbContext.ModelStructure.Where(ms => allOrigins.Contains(ms.ModelComponentOrigionGuid) && ms.ModelComponentGuid != modelComponentGuid).ToList();
            if (root != null)
            {
                coppiedList.Add(root);
            }
            return coppiedList;
        }

        public async Task<List<Data>> GetAffectedModels(string modelComponentGuid, bool isEdit)
        {
            List<Data> affectedModels = new List<Data>();

            List<ModelStructure> coppiedList = await GetConnectModels(modelComponentGuid, isEdit);

            ModelComponent mc;
            Data d;

            foreach (var copy in coppiedList)
            {
                mc = FindModelParentRoot(copy.ModelComponentParentGuid, true);
                d = new Data() { id = mc.ModelComponentGuid, text = mc.Name };
                if (!affectedModels.Exists(x => x.id == d.id && x.text == d.text))
                    affectedModels.Add(d);
            }

            return await Task.FromResult(affectedModels);
        }

        public async Task<string[]> GetAllOrigins(string[] origins, string[] result = null)
        {
            result = result ?? new string[] { };

            string[] affected = await DbContext.ModelStructure.Where(ms => origins.Contains(ms.ModelComponentOrigionGuid))
                .Select(ms => ms.ModelComponentGuid)
               .ToArrayAsync();

            if (affected == null || !affected.Any())
                return result;

            result = result.Concat(affected).ToArray();
            return await GetAllOrigins(affected.ToArray(), result);
        }


        public async Task<List<string>> GetOriginsForThreshold(string[] origins)
        {
            string[] affected = await DbContext.ModelStructure.Where(ms => origins.Contains(ms.ModelComponentGuid))
                .Select(ms => ms.AllOrigins)
               .ToArrayAsync();

            List<string> result = new List<string>();
            result.Add(origins[0]);
            string[] splitOrigins;
            foreach (var item in affected)
            {
                if (item != null)
                {
                    splitOrigins = item.Split(";");
                    foreach (var o in splitOrigins)
                    {
                        if (!string.IsNullOrEmpty(o))
                            result.Add(o);
                    }
                }
            }

            return result;
        }

        public ModelComponent FindModelParentRoot(string guid, bool withoutCopyOrRef = false)
        {
            ModelStructure parentModel = DbContext.ModelStructure.Where(ms => ms.ModelComponentGuid == guid).Include(ms => ms.ModelComponentGu).FirstOrDefault();

            if (parentModel == null)
            {
                ModelComponent model = DbContext.ModelComponent.Where(mc => mc.ModelComponentGuid == guid).FirstOrDefault();
                return model;
            }

            if (withoutCopyOrRef)
            {
                if (parentModel.ModelComponentGu.Source == (int)ModelComponentSource.model_root &&
                    (parentModel.ModelComponentType.HasValue == false ||
                    (parentModel.ModelComponentType.Value != (int)ModelComponentTypes.copyWithSource &&
                    parentModel.ModelComponentType.Value != (int)ModelComponentTypes.reference)))
                {
                    return parentModel.ModelComponentGu;
                }
            }
            else
            {
                if (parentModel.ModelComponentGu.Source == (int)ModelComponentSource.model_root)
                {
                    return parentModel.ModelComponentGu;
                }
            }

            return FindModelParentRoot(parentModel.ModelComponentParentGuid, withoutCopyOrRef);
        }

        //public async Task<bool> ImportAmanData(List<AmanCsvFile> allCsvRows)
        //{
        //    string amanGuid = _config.AmanGuid;
        //    if(string.IsNullOrEmpty(amanGuid))
        //    {
        //        return false;
        //    }

        //    //remove 2 exist models
        //    await DeleteModelComponentAsync(new List<ModelGuidAndTypeData> { new ModelGuidAndTypeData(Constants.AmanModelGuidA), new ModelGuidAndTypeData(Constants.AmanModelGuidB) });

        //    //Create activity template
        //    ActivityTemplate at = new ActivityTemplate();
        //    at.ActivityTemplateGuid = Util.CreateGuid().ToString();
        //    at.CreateDate = Util.ConvertDateToString(DateTime.Now);
        //    at.Name = "כשירות שירותי אמ''ן";

        //    DbContext.ActivityTemplate.Add(at);
        //    DbContext.SaveChanges();

        //    //Create activities by dates
        //    DateTime[] dates = allCsvRows.GroupBy(x => x.open_date).Select(x => x.Key).ToArray();
        //    foreach (var d in dates)
        //    {
        //        string strDate = Util.ConvertDateToString(d);
        //        Activity activity = new Activity();
        //        activity.ActivityGuid = Util.CreateGuid().ToString() + "Aman-" + d.ToShortDateString();
        //        activity.Name = "כשירות שירותי אמ''ן - " + d.ToShortDateString();
        //        activity.StartDate = strDate;
        //        activity.EndDate = activity.EndDate = strDate;
        //        activity.ActivityTemplateGuid = at.ActivityTemplateGuid;
        //        activity.CreateDate = strDate;
        //        //activity.OrgObjGuid = amanGuid;TODO

        //        DbContext.Activity.Add(activity);
        //        DbContext.SaveChanges();
        //    }



        //    //Create option B
        //    await CreateAmanDynamicModel(Constants.AmanModelGuidB, "כשירות שירותי אמ''ן - לפי מערכת", 100, 0, 7, 3, 3);

        //    int idxService = 0, idxSystem = 0, idxHardware = 0;
        //    int servicesCnt, systemsCnt, hardwaresCnt;

        //    DateTime date = allCsvRows.Max(x => x.open_date);
        //    var lastCsvRows = allCsvRows.Where(x => x.open_date == date);

        //    var services = lastCsvRows.GroupBy(x => x.world);
        //    foreach (var service in services)
        //    {
        //        string serviceGuid = Util.CreateGuid().ToString();
        //        servicesCnt = services.Count();
        //        int serviceWeight = idxService == servicesCnt - 1 ? ((100 / servicesCnt) + (100 % servicesCnt)) : (100 / servicesCnt);
        //        await CreateAmanDynamicModel(serviceGuid, service.Key, serviceWeight, idxService, 6, 3, 3, Constants.AmanModelGuidB);

        //        var systems = service.GroupBy(x => x.system_name);

        //        idxSystem = 0;
        //        foreach (var system in systems)
        //        {
        //            string systemGuid = Util.CreateGuid().ToString();
        //            systemsCnt = systems.Count();
        //            int ststemWeight = idxSystem == systemsCnt - 1 ? ((100 / systemsCnt) + (100 % systemsCnt)) : (100 / systemsCnt);
        //            await CreateAmanDynamicModel(systemGuid, system.Key, ststemWeight, idxSystem, 6, 3, 3, serviceGuid);

        //            var hardwares = system.GroupBy(x => x.severity);

        //            idxHardware = 0;
        //            foreach (var hardware in hardwares)
        //            {
        //                string hardwareGuid = Util.CreateGuid().ToString();
        //                hardwaresCnt = hardwares.Count();
        //                int hardwareWeight = idxHardware == hardwaresCnt - 1 ? ((100 / hardwaresCnt) + (100 % hardwaresCnt)) : (100 / hardwaresCnt);
        //                await CreateAmanDynamicModel(hardwareGuid, string.Join(" - ", service.Key, system.Key, hardware.Key), hardwareWeight, idxHardware, 5, 3, null, systemGuid, true);
        //                idxHardware++;

        //                var data = hardware.First();
        //                List<AmanCsvFile> allScores = allCsvRows.Where(x => x.world == data.world && x.system_name == data.system_name && x.severity == data.severity).ToList();
        //                await SaveScores(amanGuid, hardwareGuid, allScores);
        //            }
        //            idxSystem++;
        //        }
        //        idxService++;
        //    }

        //    //connect model to Aman unit
        //    SaveOrganizationsObjectConnectionToModel(new List<string>() { amanGuid }, Constants.AmanModelGuidB);

        //    //Create option A
        //    await CreateAmanDynamicModel(Constants.AmanModelGuidA, @"כשירות שירותי אמ''ן - לפי חומרה", 100, 0, 7, 3, 3);

        //    idxService = idxSystem = idxHardware = 0;

        //    var services2 = lastCsvRows.GroupBy(x => x.world);
        //    foreach (var service in services2)
        //    {
        //        string serviceGuid = Util.CreateGuid().ToString();
        //        servicesCnt = services2.Count();
        //        int serviceWeight = idxService == servicesCnt - 1 ? ((100 / servicesCnt) + (100 % servicesCnt)) : (100 / servicesCnt);
        //        await CreateAmanDynamicModel(serviceGuid, service.Key, serviceWeight, idxService, 6, 3, 3, Constants.AmanModelGuidA);

        //        var hardwares2 = service.GroupBy(x => x.severity);

        //        idxHardware = 0;
        //        foreach (var hardware in hardwares2)
        //        {
        //            string hardwareGuid = Util.CreateGuid().ToString();
        //            hardwaresCnt = hardwares2.Count();
        //            int hardwareWeight = idxHardware == hardwaresCnt - 1 ? ((100 / hardwaresCnt) + (100 % hardwaresCnt)) : (100 / hardwaresCnt);
        //            await CreateAmanDynamicModel(hardwareGuid, hardware.Key, hardwareWeight, idxHardware, 6, 3, 3, serviceGuid);

        //            var systems2 = hardware.GroupBy(x => x.system_name);

        //            idxSystem = 0;
        //            foreach (var system in systems2)
        //            {
        //                string systemGuid = Util.CreateGuid().ToString();
        //                systemsCnt = systems2.Count();
        //                int systemWeight = idxSystem == systemsCnt - 1 ? ((100 / systemsCnt) + (100 % systemsCnt)) : (100 / systemsCnt);
        //                await CreateAmanDynamicModel(systemGuid, string.Join(" - ", service.Key, hardware.Key, system.Key), systemWeight, idxSystem, 5, 3, null, hardwareGuid, true);
        //                idxSystem++;

        //                var data = system.First();
        //                List<AmanCsvFile> allScores = allCsvRows.Where(x => x.world == data.world && x.system_name == data.system_name && x.severity == data.severity).ToList();
        //                await SaveScores(amanGuid, systemGuid, allScores);
        //            }
        //            idxHardware++;
        //        }
        //        idxService++;
        //    }

        //    //connect model to Aman unit
        //    SaveOrganizationsObjectConnectionToModel(new List<string>() { amanGuid }, Constants.AmanModelGuidA);

        //    return true;
        //}

        //public async Task SaveScores(string orgObjGuid, string modelGuid, List<AmanCsvFile> allScores)
        //{
        //    List<Score> savedScores = new List<Score>();
        //    var lastRow = DbContext.Score.OrderByDescending(u => u.ScoreId).FirstOrDefault();
        //    int idx = lastRow != null ? lastRow.ScoreId : 0;

        //    foreach (var score in allScores)
        //    {
        //        string activityGuid = DbContext.Activity.FirstOrDefault(a => a.ActivityGuid.Contains("Aman-" + score.open_date.ToShortDateString()))?.ActivityGuid;

        //        var s = new Score
        //        {
        //            ScoreId = ++idx,
        //            ActivityGuid = activityGuid,
        //            ModelComponentGuid = modelGuid,
        //            //OrgObjGuid = orgObjGuid,TODO
        //            EntityGuid =
        //            OriginalScore = score.percentage,
        //            ConvertionScore = score.percentage,
        //            Status = 4,
        //        };

        //        savedScores.Add(s);
        //    }

        //    DbContext.Score.AddRange(savedScores);
        //    DbContext.SaveChanges();
        //}

        //public async Task CreateAmanDynamicModel(string modelGuid, string modelName, int weight, int order, int source, int measuringUnit, int? rollupMethod, string rootGuid = null, bool showOriginValue = false)
        //{
        //    ModelComponent model_component = new ModelComponent();
        //    model_component.ModelComponentGuid = modelGuid;
        //    model_component.Name = modelName;
        //    model_component.Source = source;
        //    model_component.Status = 2;
        //    model_component.ModelComponentOrder = order;
        //    model_component.Weight = weight;
        //    model_component.CreateDate = Util.ConvertDateToString(DateTime.Now);
        //    model_component.ModifiedDate = Util.ConvertDateToString(DateTime.Now);
        //    model_component.MetricRequired = false;
        //    model_component.MetricMeasuringUnit = source == 5 ? measuringUnit : default(int?);
        //    model_component.MetricRollupMethod = source == 5 ? default(int?) : rollupMethod;
        //    model_component.MetricCalenderRollup = source == 5 ? 2 : default(int?);//last set
        //    model_component.MetricIsVisible = true;
        //    model_component.MetricNotDisplayIfIrrelevant = false;
        //    model_component.MetricExpiredPeriod = "m12";
        //    model_component.MetricCommentObligationLevel = 0;
        //    model_component.MetricGradualDecreasePrecent = 0;
        //    model_component.MetricGradualDecreasePeriod = 0;
        //    model_component.MetricMinimumFeeds = 0;
        //    model_component.ShowOrigionValue = showOriginValue;
        //    model_component.MetricSource = 3;//data_source
        //    model_component.TemplateType = 0;
        //    model_component.CalcAsSum = false;
        //    model_component.MetricExpiredPeriodSecondary = null;
        //    model_component.GroupChildren = false;

        //    //model component
        //    DbContext.ModelComponent.Add(model_component);
        //    DbContext.SaveChanges();

        //    if (!string.IsNullOrEmpty(rootGuid))
        //    {
        //        //model structure
        //        ModelStructure model_structure = new ModelStructure();
        //        model_structure.ModelComponentGuid = model_component.ModelComponentGuid;
        //        model_structure.ModelComponentParentGuid = rootGuid;

        //        DbContext.ModelStructure.Add(model_structure);
        //        DbContext.SaveChanges();
        //    }

        //    //convertion table
        //    List<ConvertionTableData> convertion_data = new List<ConvertionTableData>();
        //    convertion_data = GetDefaultConvertionTable(measuringUnit, model_component.ModelComponentGuid);
        //    List<ConvertionTable> data = Mapper.Map<List<ConvertionTable>>(convertion_data);
        //    //List<ConvertionTable> data = convertion_data.Select(x => new Model.Entities.ConvertionTable()
        //    //{
        //    //    ModelComponentGuid = x.model_component_guid,
        //    //    LevelId = x.level_id,
        //    //    StartRange = x.start_range,
        //    //    EndRange = x.end_range,
        //    //    ConversionTableModifiedDate = x.conversion_table_modified_date,
        //    //    ConversionTableStatus = x.conversion_table_status,
        //    //    ConversionTableCreateDate = x.conversion_table_create_date,
        //    //    StartRangeScoreDisplayed = x.start_range_score_displayed,
        //    //    EndRangeScoreDisplayed = x.end_range_score_displayed,
        //    //    ConversionTableScoreOrder = x.conversion_table_score_order,
        //    //    ConversionTableFinalScore = x.conversion_table_final_score
        //    //}).ToList();

        //    DbContext.ConvertionTable.AddRange(data);
        //    DbContext.SaveChanges();
        //}


    }
}