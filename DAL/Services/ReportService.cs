using Infrastructure.Core;
using Infrastructure.Extensions;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Model.Data;
using Model.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConvertionTableData = Model.Data.ConvertionTableData;
using FormStatus = Model.Data.FormStatus;
using ModelComponentSource = Model.Data.ModelComponentSource;
using ModelComponentStatus = Model.Data.ModelComponentStatus;

namespace Dal.Services
{
    public class ReportService : BaseService
    {
        private ModelService _modelService => this.GetChildService<ModelService>();
        private OrganizationService _organizationService => this.GetChildService<OrganizationService>();

        private readonly Serilog.ILogger _logger = GeneralContext.GetService<Serilog.ILogger>();

        public async Task<Dictionary<string, CalculateTreeData>> FillTreeScores(string report_guid, UnitDataInfo org_obj, Dictionary<string, CalculateTreeData> flatModel, List<string> activities, DateTime? calculate_date, string _candidateGuid = null)
        {
            Dictionary<string, double?> fixWeightModels = new Dictionary<string, double?>();

            foreach (var model in flatModel)
            {
                model.Value.report_guid = report_guid;
                model.Value.data.model_data.generate_calculate_date = calculate_date;

                if (model.Value.data.model_data.is_reference)
                {
                    if (model.Value.data.reference_score_list != null && model.Value.data.reference_score_list.Count > 0)
                    {
                        ModelComponent mc;
                        int scoreLevel;
                        foreach (var reference_score in model.Value.data.reference_score_list)
                        {
                            int? measuring = model.Value.data.model_data.metric_measuring_unit;
                            if (model.Value.data.model_data.metric_gradual_decrease_period.Value > 0 && model.Value.data.model_data.metric_gradual_decrease_precent.Value > 0 &&
                                (model.Value.data.model_data.metric_measuring_unit == 4 || model.Value.data.model_data.metric_measuring_unit == 5 || model.Value.data.model_data.metric_measuring_unit == 2))
                            {
                                model.Value.data.score_data[0].convertion_table = GetPrecentageCT(string.Empty);
                                measuring = 3;
                            }

                            scoreLevel = GetScoreLevel(reference_score.score, measuring, model.Value.data.score_data[0].convertion_table);
                            reference_score.score_level = scoreLevel;
                            if (reference_score.tree.data.model_data.source == (int)ModelComponentSource.model)
                            {
                                mc = _modelService.FindModelParentRoot(reference_score.tree.data.model_data.origin_model_component_guid);
                                if (mc != null)
                                    reference_score.tree.polygonGuid = GetPolyGonByOrgAndModelGuid(mc.ModelComponentGuid, reference_score.org_obj_guid);
                            }
                            else
                            {
                                reference_score.tree.polygonGuid = GetPolyGonByOrgAndModelGuid(model.Value.data.model_data.origin_model_component_guid, reference_score.org_obj_guid);
                            }
                        }

                        model.Value.data.model_data.score_level = GetScoreLevel(model.Value.data.score_data[0].calculated_score, 3, GetConvertionForRef(model.Value.data));
                    }
                    else
                    {
                        flatModel.Remove(model.Key);
                        if (fixWeightModels.ContainsKey(model.Value.data.model_data.parent_guid))
                        {
                            fixWeightModels[model.Value.data.model_data.parent_guid] += model.Value.data.model_data.weight;
                        }
                        else
                        {
                            fixWeightModels.Add(model.Value.data.model_data.parent_guid, model.Value.data.model_data.weight);
                        }
                    }
                }
                else
                {
                    model.Value.data.score_data = await GetScoreData(org_obj, activities, model.Value.data.model_data, calculate_date, _candidateGuid);
                }
            }

            if (fixWeightModels.Count > 0)
            {
                foreach (var model in fixWeightModels)
                {
                    var allChilds = flatModel.Where(x => x.Value.data.model_data.parent_guid == model.Key);
                    double? addWeight = model.Value / allChilds.Count();

                    foreach (var child in allChilds)
                    {
                        child.Value.data.model_data.weight += addWeight;
                    }

                }
            }

            return flatModel;
        }
        public async Task<Dictionary<string, CalculateTreeData>> FillTreeScoresRef(string model_component_guid, UnitDataInfo org_obj, Dictionary<string, List<OrgModels>> orgModels, Dictionary<string, CalculateTreeData> flatModel, List<string> activities, DateTime? calculate_date, string candidateGuid = null)
        {
            //var flatRefModel = await _modelService.GetFlatModel(new[] { model_component_guid }, org_obj.guid);
            var flatRefModel = await GetSubFlatModel(flatModel, model_component_guid);

            foreach (var refModel in flatRefModel)
            {
                refModel.Value.data.model_data.generate_calculate_date = calculate_date;
                if (!refModel.Value.data.model_data.is_reference)
                {
                    refModel.Value.data.score_data = await GetScoreData(org_obj, activities, refModel.Value.data.model_data, calculate_date, candidateGuid);
                }
                else
                {
                    //refModel.Value.data.score_data = await GetModelScoreData(org_obj, activities, refModel.Value.data.model_data, calculate_date, candidateGuid);

                    if (refModel.Value.data.model_data.source != (int)ModelComponentSource.model && refModel.Value.data.model_data.source != (int)ModelComponentSource.model_root)
                    {
                        int? measuring = refModel.Value.data.model_data.metric_measuring_unit;

                        string rootOriginGuid = _modelService.GetModelOriginGuid(refModel.Value.data.model_data.origin_model_component_guid);
                        List<ScoreData> existOrgScore = await GetMetricScoreData(org_obj, activities, refModel.Value.data.model_data, rootOriginGuid);
                        if (existOrgScore == null || existOrgScore.Count == 0 || existOrgScore[0].calculated_score == -1)
                        {
                            //string[] org_obj_children = await _organizationService.GetOrgObjChildren(new string[] { org_obj.guid }, org_obj.union_guid);
                            string[] org_obj_children = {};
                            if (org_obj_children.Length > 0)
                            {
                                List<ScoreData> sl = null;
                                UnitDataInfo childOrg = new UnitDataInfo() { guid = org_obj.guid, name = org_obj.name, org_type = org_obj.org_type };
                                double? sum = 0;
                                double count = 0;
                                foreach (var orgGuid in org_obj_children)
                                {
                                    childOrg.guid = orgGuid;
                                    sl = await GetMetricScoreData(childOrg, activities, refModel.Value.data.model_data, rootOriginGuid);
                                    if (sl != null && sl.Count > 0 && sl[0].calculated_score > -1)
                                    {
                                        sum += sl[0].calculated_score;
                                        count++;
                                    }
                                }

                                existOrgScore = new List<ScoreData>();
                                existOrgScore.Add(sl[0]);
                                //existOrgScore[0].org_obj_guid = org_obj.guid;
                                //existOrgScore[0].org_obj_name = org_obj.name;
                                existOrgScore[0].entity_guid = org_obj.guid;
                                //existOrgScore[0].unit_type = TODO
                                existOrgScore[0].original_score = existOrgScore[0].convertion_score = existOrgScore[0].calculated_score = count > 0 ? sum / count : -1;
                            }
                            else
                            {
                                existOrgScore[0].original_score = existOrgScore[0].convertion_score = existOrgScore[0].calculated_score = existOrgScore[0].calculated_score;
                            }
                        }
                       
                        refModel.Value.data.score_data = existOrgScore;
                        
                        if (refModel.Value.data.model_data.metric_gradual_decrease_period.Value > 0 && refModel.Value.data.model_data.metric_gradual_decrease_precent.Value > 0 &&
                            (refModel.Value.data.model_data.metric_measuring_unit == 4 || refModel.Value.data.model_data.metric_measuring_unit == 5 || refModel.Value.data.model_data.metric_measuring_unit == 2))
                        {
                            refModel.Value.data.score_data[0].convertion_table = GetPrecentageCT(string.Empty);
                            measuring = 3;
                        }

                        refModel.Value.data.model_data.score_level = GetScoreLevel(refModel.Value.data.score_data[0].calculated_score, measuring, refModel.Value.data.score_data[0].convertion_table);
                    }
                    else
                    {
                        refModel.Value.data.score_data = await GetModelScoreData(org_obj, activities, refModel.Value.data.model_data, calculate_date, candidateGuid, true);
                    }
                }

            }

            return flatRefModel;
        }

        private List<ConvertionTableData> GetPrecentageCT(string model_component_guid)
        {
            List<ConvertionTableData> data = new List<ConvertionTableData>();
            ConvertionTableData ct1 = new ConvertionTableData
            {
                level_id = 1,
                start_range = 0,
                end_range = 49,
                conversion_table_final_score = 0,
                model_component_guid = model_component_guid,
                conversion_table_create_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_status = "draft"
            };

            ConvertionTableData ct2 = new ConvertionTableData
            {
                level_id = 2,
                start_range = 50,
                end_range = 59,
                conversion_table_final_score = 0,
                model_component_guid = model_component_guid,
                conversion_table_create_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_status = "draft"
            };

            ConvertionTableData ct3 = new ConvertionTableData
            {
                level_id = 3,
                start_range = 60,
                end_range = 79,
                conversion_table_final_score = 0,
                model_component_guid = model_component_guid,
                conversion_table_create_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_status = "draft"
            };

            ConvertionTableData ct4 = new ConvertionTableData
            {
                level_id = 4,
                start_range = 80,
                end_range = 89,
                conversion_table_final_score = 0,
                model_component_guid = model_component_guid,
                conversion_table_create_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_status = "draft"
            };

            ConvertionTableData ct5 = new ConvertionTableData
            {
                level_id = 5,
                start_range = 90,
                end_range = 100,
                conversion_table_final_score = 0,
                model_component_guid = model_component_guid,
                conversion_table_create_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_modified_date = Util.ConvertDateToString(DateTime.Now),
                conversion_table_status = "draft"
            };

            data.AddRange(new List<ConvertionTableData> { ct1, ct2, ct3, ct4, ct5 });

            return data;
        }

        public async Task<Dictionary<string, CalculateTreeData>> GetSubFlatModel(Dictionary<string, CalculateTreeData> flatModel, string modelComponentGuid)
        {
            Dictionary<string, CalculateTreeData> subFlatModel = new Dictionary<string, CalculateTreeData>();
            List<string> parentsGuids = new List<string>();

            foreach (var model in flatModel)
            {
                if (model.Key == modelComponentGuid || parentsGuids.Contains(model.Value.data.model_data.parent_guid))
                {
                    parentsGuids.Add(model.Key);
                    subFlatModel.Add(model.Key, model.Value);
                }
            }

            return await Task.FromResult(subFlatModel);
        }

        public async Task<bool> SaveCalculateScores(string report_guid, CalculateTreeData calculate_tree)
        {
            _logger.Information("start SaveCalculateScores");

            var allScores = calculate_tree.GetNodeAndDescendants().SelectMany(node => node.data.score_data).ToList();

            _logger.Information("num of scores from calculate_tree: " + allScores.Count);

            List<CalculateScore> newScoresList = new List<CalculateScore>();
            CalculateScore calculate_score;
            int scoreIdentify = 1;

            var lastScore = DbContext.CalculateScore.OrderByDescending(cs => cs.ScoreId).FirstOrDefault();
            if (lastScore != null)
            {
                scoreIdentify = lastScore.ScoreId + 1;
            }

            foreach (var score in allScores)
            {
                calculate_score = new CalculateScore();

                calculate_score.ScoreId = scoreIdentify;
                calculate_score.ReportGuid = report_guid;
                calculate_score.ModelComponentGuid = score.model_component_guid;
                calculate_score.FormElementGuid = score.form_element_guid;
                calculate_score.EntityType = score.entity_type;
                calculate_score.EntityGuid = score.entity_guid;
                calculate_score.ActivityGuid = score.activity_guid;
                calculate_score.OriginalScore = score.original_score;
                calculate_score.ConvertionScore = score.convertion_score;
                calculate_score.CalculatedScore = score.calculated_score;
                calculate_score.CalculatedDate = score.calculated_date;
                calculate_score.ModelComponentComment = score.model_component_comment;
                calculate_score.FormGuid = score.form_guid;

                newScoresList.Add(calculate_score);
                scoreIdentify += 1;
            }

            _logger.Information("num of scores before save: " + newScoresList.Count);

            DbContext.CalculateScore.AddRange(newScoresList);
            DbContext.SaveChanges();

            _logger.Information("end SaveCalculateScores");

            return true;
        }

        public async Task<List<CalculateTreeData>> UpdateCalculateTree(string report_guid, ReportTypes report_type, List<CalculateTreeData> tree_list, DateTime? calculate_date, UnitDataInfo org_obj, List<string> comment_list, List<string> focus_list, bool is_root, bool hasReference)
        {
            if (tree_list != null && tree_list.Count > 0)
            {
                foreach (var tree in tree_list)
                {
                    await FillReportList(report_guid, tree, report_type, org_obj, calculate_date, is_root, comment_list, focus_list, hasReference);

                    //remove metric_not_display_if_irrelevant=true and metric_is_visible=false
                    tree.children.RemoveAll(c => (c.data.score_data[0].calculated_score == -2 && c.data.model_data.metric_not_display_if_irrelevant.HasValue && c.data.model_data.metric_not_display_if_irrelevant.Value) || c.data.model_data.metric_is_visible == false);

                    await UpdateCalculateTree(report_guid, report_type, tree.children, calculate_date, org_obj, comment_list, focus_list, false, hasReference);


                    if ((tree.data.model_data.groupChildren.HasValue && tree.data.model_data.groupChildren.Value == true) ||//groupChildren is selected
                         tree.data.model_data.parent_guid == null)//root level
                    {
                        List<OrgObjScoreData> childGroupRef = new List<OrgObjScoreData>();
                        int refChildsCount = tree.children.Count(c => c.data.model_data.is_reference);
                        if (refChildsCount > 1)
                        {
                            foreach (var child in tree.children)
                            {
                                if (tree.data.model_data.parent_guid != null)
                                {
                                    var originModel = _modelService.FindModelParentRoot(child.data.model_data.origin_model_component_guid);
                                    child.data.model_data.origin_model_component_name = originModel.Name;
                                }

                                if (child.data.reference_score_list != null)
                                    childGroupRef.AddRange(child.data.reference_score_list);
                            }

                            tree.data.reference_score_list = childGroupRef;
                        }
                        else
                        {
                            tree.data.reference_score_list = null;
                        }
                    }

                }
            }

            return tree_list;
        }

        public async Task<string> SaveReportAsJson(dynamic data)
        {
            Model.Entities.SavedReportData saved_reports;
            //string[] s1 = { };
            try
            {
                foreach (var item in data)
                {
                    saved_reports = new Model.Entities.SavedReportData();
                    string reportData = JsonConvert.SerializeObject(item.Value);
                    saved_reports.ReportData = reportData;
                    saved_reports.ReportGuid = data.report_guid?.Value;

                    //check if exist
                    var existing_data = DbContext.SavedReportData.Where(srd => srd.ReportGuid == saved_reports.ReportGuid).FirstOrDefault();

                    if (existing_data == null)
                    {
                        DbContext.SavedReportData.Add(saved_reports);
                    }
                    else
                    {
                        existing_data.ReportData = saved_reports.ReportData;
                        DbContext.SavedReportData.Update(existing_data);
                    }

                    await DbContext.SaveChangesAsync();
                    break;

                }

            }
            catch (Exception ex)
            {


            }

            return data.report_guid?.Value;
        }

        public async Task<List<string>> GetReportByGuid(string report_guid)
        {
            List<string> calculatetrees = new List<string>();

            if (!string.IsNullOrEmpty(report_guid))
            {
                calculatetrees = await DbContext.SavedReportData
                                        .Where(srd => srd.ReportGuid == report_guid && srd.ReportData != null)
                                        .Select(srd => srd.ReportData).ToListAsync();
            }

            return calculatetrees;
        }

        //public async Task<string> GetUserPrimaryReport(string user_guid)
        //{
        //    string calculatetree = string.Empty;

        //    SavedReports saved_report = await DbContext.SavedReports.Where(sr => sr.UserGuid == user_guid && sr.IsPrimary == true).FirstOrDefaultAsync();
        //    if (saved_report != null)
        //    {
        //        string org_obj_guid = await DbContext.SavedReportConnection.Where(src => src.ReportGuid == saved_report.ReportGuid).Select(src => src.OrgObjGuid).FirstOrDefaultAsync();
        //        DateTime calculated_date = Util.ConvertStringToDate(saved_report.CalculatedDates.Split(",")[0]);
        //        var res = GetSavedReport(saved_report);

        //        if (res.Count == 1)
        //        {
        //            calculatetree = res[0];
        //        }
        //    }

        //    return calculatetree;
        //}

        //public async Task<List<string>> GetUserHybridReports(string user_guid)
        //{
        //    List<string> calculatetree = new List<string>();

        //    List<SavedReports> saved_report = await DbContext.SavedReports.Where(sr => sr.UserGuid == user_guid && sr.IsDefined == true).OrderBy(X => X.TemplateType).ToListAsync();
        //    foreach (var rpt in saved_report)
        //    {
        //        if (rpt != null)
        //        {
        //            string org_obj_guid = await DbContext.SavedReportConnection.Where(src => src.ReportGuid == rpt.ReportGuid).Select(src => src.OrgObjGuid).FirstOrDefaultAsync();
        //            DateTime calculated_date = Util.ConvertStringToDate(rpt.CalculatedDates.Split(",")[0]);
        //            var res = GetSavedReport(rpt);

        //            if (res.Count == 1)
        //            {
        //                calculatetree.Add(res[0]);
        //            }
        //        }
        //    }


        //    return calculatetree;
        //}

        public async Task<string> GetUserSecondaryReport(string user_guid)
        {
            string calculatetree = string.Empty;

            SavedReports saved_report = await DbContext.SavedReports.Where(sr => sr.UserGuid == user_guid && sr.IsSecondary == true).FirstOrDefaultAsync();

            if (saved_report != null)
            {
                var jsonTreeDATA = await DbContext.SavedReportData.Where(srd => srd.ReportGuid == saved_report.ReportGuid).Select(srd => srd.ReportData).FirstOrDefaultAsync();
                if (jsonTreeDATA != null)
                {
                    calculatetree = jsonTreeDATA;
                }
            }

            return calculatetree;
        }

        public async Task<List<WatchData>> GetUserWatchReports(string user_guid)
        {
            List<WatchData> watch_report_list = new List<WatchData>();

            List<Model.Entities.SavedReports> saved_report_list = await DbContext.SavedReports.Where(sr => sr.UserGuid == user_guid && sr.IsWatch == true).ToListAsync();

            List<WatchData> watch_data = new List<WatchData>();

            DateTime calculated_date;
            List<string> org_obj_guid = new List<string>();

            foreach (var saved_report in saved_report_list)
            {
                calculated_date = Util.ConvertStringToDate(saved_report.CalculatedDates.Split(",")[0]);

                var convertion_details = await _modelService.GetConvertionDetails(saved_report.ModelComponentGuid);

                watch_data = await DbContext.CalculateScore
                              .Where(s => s.ModelComponentGuid == saved_report.ModelComponentGuid &&
                              s.ReportGuid == saved_report.ReportGuid &&
                              s.CalculatedScore != null)
                              .Select(s => new WatchData()
                              {
                                  report_guid = saved_report.ReportGuid,
                                  report_type = saved_report.ReportType,
                                  model_component_guid = saved_report.ModelComponentGuid,
                                  name = saved_report.Name,
                                  order = saved_report.Order,
                                  score = s.CalculatedScore,
                                  score_level = GetScoreLevel(s.CalculatedScore, s.ModelComponentGu.MetricMeasuringUnit, convertion_details)
                              }).ToListAsync();

                if (watch_data != null)
                {
                    if (watch_data.Count > 1)//its compare report so we need to make score avg
                    {
                        WatchData watch_dataItem = new WatchData();
                        var measuring_unit = await DbContext.ModelComponent.Where(mu => mu.ModelComponentGuid == saved_report.ModelComponentGuid)
                            .Select(mu => mu.MetricMeasuringUnit).FirstOrDefaultAsync();

                        double sum = 0;
                        foreach (var item in watch_data)
                        {
                            sum += (double)item.score;
                        }

                        sum = Convert.ToDouble(string.Format("{0:0.00}", sum / watch_data.Count));

                        watch_dataItem.report_guid = saved_report.ReportGuid;
                        watch_dataItem.report_type = saved_report.ReportType;
                        watch_dataItem.model_component_guid = saved_report.ModelComponentGuid;
                        watch_dataItem.name = saved_report.Name;
                        watch_dataItem.order = saved_report.Order;
                        watch_dataItem.score = sum;
                        watch_dataItem.score_level =
                            GetScoreLevel(sum, measuring_unit,
                                await _modelService.GetConvertionDetails(saved_report.ModelComponentGuid));
                        watch_report_list.Add(watch_dataItem);
                    }
                    else if (watch_data.Count > 0)
                    {
                        watch_report_list.Add(watch_data[0]);

                    }
                }
            }

            watch_report_list = watch_report_list.OrderBy(w => w.order).ToList();

            return watch_report_list;
        }

        public async Task<List<SavedReportDataInfo>> GetUserReports(string user_guid)
        {
            List<SavedReportDataInfo> saved_report = new List<SavedReportDataInfo>();

            List<string> tmp = new List<string>();

            //var cand = await DbContext.Candidate.ToListAsync();


            saved_report = await DbContext.SavedReports
                                  .Where(sr => sr.UserGuid == user_guid)
                                  .OrderBy(sr => sr.Order)
                                  .Select(sr => new SavedReportDataInfo(sr, tmp, tmp, tmp))
                                  .ToListAsync();

            //foreach (var item in saved_report)
            //{
            //    var candidate = cand.Where(x => x.UserGuid == item.candidate_user_guid).Select(x => x).FirstOrDefault();
            //    if (candidate != null)
            //    {
            //        item.candidateInfo = new CandidateDataInfo(candidate);

            //    }
            //    else
            //    {
            //        item.candidateInfo = new CandidateDataInfo();
            //    }

            //}

            saved_report.Reverse();

            return saved_report;
        }

        //public async Task<Model.Data.SavedReportDataInfo> GetSavedReport(string report_guid)
        //{
        //    Model.Data.SavedReportDataInfo saved_report;

        //    saved_report = await DbContext.SavedReports
        //                          .Where(sr => sr.ReportGuid == report_guid)
        //                          .Select(sr => new SavedReportDataInfo(sr,
        //                                                        sr.SavedReportConnection.Where(src => src.OrgObjGuid != null).Select(src => src.OrgObjGuid).ToList(),
        //                                                        sr.SavedReportConnection.Where(src => src.Comment != null).Select(src => src.Comment).ToList(),
        //                                                        sr.SavedReportConnection.Where(src => src.Focus != null).Select(src => src.Focus).ToList(), sr.UserGu.Candidate))
        //                                         .FirstOrDefaultAsync();

        //    return saved_report;
        //}

        public async Task<string> SaveReport(ReportData data)
        {
            SavedReports saved_reports = new SavedReports();

            SetReportData(saved_reports, data);

            if (!IsReportExist(data.report_guid))
            {
                int userReportsCount = DbContext.SavedReports.Count(sr => sr.UserGuid == data.user_guid);

                saved_reports.Order = userReportsCount;

                //New
                DbContext.SavedReports.Add(saved_reports);
                await DbContext.SaveChangesAsync();
            }
            else
            {
                //hybrid reports add by moshe
                //set IsDefiend to Old one to False
                var oldReportIsDefined = DbContext.SavedReports.Where(sr => sr.TemplateType == data.TemplateType && sr.IsDefined == true && sr.UserGuid == data.user_guid).FirstOrDefault();
                if (oldReportIsDefined != null && oldReportIsDefined.ReportGuid != data.report_guid)
                {
                    oldReportIsDefined.IsDefined = false;
                    DbContext.SavedReports.Update(oldReportIsDefined);

                }



                //Update
                var existing_report = DbContext.SavedReports.Where(sr => sr.ReportGuid == data.report_guid).FirstOrDefault();
                existing_report.CalculatedDates = saved_reports.CalculatedDates;
                existing_report.ModelComponentGuid = saved_reports.ModelComponentGuid;
                existing_report.Name = saved_reports.Name;
                existing_report.ReportType = saved_reports.ReportType;
                existing_report.UserGuid = saved_reports.UserGuid;
                existing_report.CandidateUserGuid = saved_reports.CandidateUserGuid;
                existing_report.IsDefined = saved_reports.IsDefined;
                existing_report.UnionGuid = saved_reports.UnionGuid;

                DbContext.SavedReports.Update(existing_report);


                await DbContext.SaveChangesAsync();
            }

            //bool res = await SavedReportConnection(data);


            return data.report_guid;
        }

      
        public async Task<bool> ToggleReportViewType(string report_guid, ReportView exist_report_view, int order)
        {
            SavedReports saved_report = await DbContext.SavedReports.Where(sr => sr.ReportGuid == report_guid).FirstOrDefaultAsync();

            if (saved_report == null)
                return false;

            switch (exist_report_view)
            {
                case ReportView.isPrimary:
                    {
                        if (!saved_report.IsPrimary)
                        {
                            //clear existing primary report
                            await ClearPrimaryOrSecondaryReport(saved_report, ReportView.isPrimary);
                        }

                        saved_report.IsPrimary = !saved_report.IsPrimary;
                        break;
                    }
                case ReportView.isSecondary:
                    {
                        if (!saved_report.IsSecondary)
                        {
                            //clear existing secondary report
                            await ClearPrimaryOrSecondaryReport(saved_report, ReportView.isSecondary);
                        }

                        saved_report.IsSecondary = !saved_report.IsSecondary;
                        break;
                    }
                case ReportView.isWatch:
                    {
                        saved_report.IsWatch = !saved_report.IsWatch;


                        //if remove watch (order =0) - less 1 from all next watches order
                        if (order == 0)
                        {
                            List<Model.Entities.SavedReports> next_saved_report_list = await DbContext.SavedReports.Where(sr => sr.IsWatch == true && sr.Order > saved_report.Order).ToListAsync();
                            foreach (SavedReports next_saved_report in next_saved_report_list)
                            {
                                next_saved_report.Order -= 1;
                                DbContext.SavedReports.Update(next_saved_report);
                            }
                        }

                        saved_report.Order = order;

                        break;
                    }
            }

            DbContext.SavedReports.Update(saved_report);
            await DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteSavedReport(string report_guid)
        {
            ////remove saved_report_connection
            //var saved_report_conn_list = DbContext.SavedReportConnection.Where(srw => srw.ReportGuid == report_guid);
            //DbContext.SavedReportConnection.RemoveRange(saved_report_conn_list);
            ////DbContext.SaveChanges();

            //delete from saved report data
            var saved_report_data = DbContext.SavedReportData.Where(sr => sr.ReportGuid == report_guid);
            DbContext.SavedReportData.RemoveRange(saved_report_data);
            //DbContext.SaveChanges();

            //delete saved report
            var saved_report = DbContext.SavedReports.Where(sr => sr.ReportGuid == report_guid);
            DbContext.SavedReports.RemoveRange(saved_report);

            //delete from SaveReportConectionInterFace
            //var saved_report_con_Interface = DbContext.SavedReportsConnectionInterface.Where(sr => sr.ReportGuidInterFace == report_guid);
            //DbContext.SavedReportsConnectionInterface.RemoveRange(saved_report_con_Interface);

            //saved_report_con_Interface = DbContext.SavedReportsConnectionInterface.Where(sr => sr.ReportGuid == report_guid);
            //DbContext.SavedReportsConnectionInterface.RemoveRange(saved_report_con_Interface);
            await DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<List<ModelData>> GetOrgObjModels(string[] org_obj_guid_list) //TODO may be different lists for units & persons
        {
            List<ModelData> org_obj_model_list = new List<ModelData>();

          
            var list = await (from ooc in DbContext.Unit
                              join mc in DbContext.ModelComponent on ooc.DefaultModelGuid equals mc.ModelComponentGuid
                              join m_s in DbContext.ModelStructure on mc.ModelComponentGuid equals m_s.ModelComponentGuid into structure
                              from ms in structure.DefaultIfEmpty()
                              where ooc.DefaultModelGuid != null &&
                                   org_obj_guid_list.Contains(ooc.UnitGuid) &&
                                   mc.Status == (int)ModelComponentStatus.active &&
                                   mc.Source == (int)ModelComponentSource.model_root &&
                                   (ms == null || !ms.ModelComponentType.HasValue || (ModelComponentTypes)ms.ModelComponentType.Value != ModelComponentTypes.reference)

                              orderby mc.ModelComponentGuid
                              select new ModelData(mc))
                      .ToListAsync();

            int length = list.Count();
            for (int i = 0; i < length; i++)
            {
                int count = list.Where(x => x.model_component_guid == list[i].model_component_guid).Count();
                if (count == org_obj_guid_list.Length)
                {
                    org_obj_model_list.Add(list[i]);
                    i += count - 1;
                }
            }

            return org_obj_model_list;
        }

        public async Task<int> GetScoreLevelValue(double? score, int? metric_measuring_unit, List<ConvertionTableData> convertion_table)
        {
            int level = GetScoreLevel(score, metric_measuring_unit, convertion_table);
            return await Task.FromResult(level);
        }

        #region Private methods

        private async Task<List<ScoreData>> GetScoreData(UnitDataInfo org_obj, List<string> activities, CalculateMCData data, DateTime? calculate_date, string _candidateGuid = null)
        {
            List<ScoreData> scores;

            if (data.source == (int)ModelComponentSource.model || data.source == (int)ModelComponentSource.model_root)
            {
                scores = await GetModelScoreData(org_obj, activities, data, calculate_date, _candidateGuid);
            }
            else
            {
                if (_candidateGuid == null)
                {
                    scores = await GetMetricScoreData(org_obj, activities, data);

                }
                else
                {
                    scores = await GetMetricScoreFromOutSource(_candidateGuid, org_obj, activities, data);//MOSHE TEST
                }
            }

            return scores;
        }

        private List<ConvertionTableData> GetConvertionForRef(CalculateNodeData data)
        {
            //if (data.score_data[0].convertion_table.Count > 0)
            //    return data.score_data[0].convertion_table;
            //else
            return _modelService.GetDefaultConvertionTable(3, data.model_data.model_component_guid);
        }

        private async Task<List<ScoreData>> GetModelScoreData(UnitDataInfo org_obj, List<string> activities, CalculateMCData data, DateTime? calc_date, string _candidateGuid = null,bool isRef = false)
        {
            List<ScoreData> scores = new List<ScoreData>();
            ScoreData score = new ScoreData();

            string activity_guid = null;

            string modelComponentGuid = data.origin_model_component_guid != null ? data.origin_model_component_guid : data.model_component_guid;

            if (activities.Count == 1)
            {
                activity_guid = activities[0];
            }
            if (_candidateGuid == null)
            {
                score = await GetEmptyScore(data.model_component_guid, org_obj, data.metric_measuring_unit, data.generate_calculate_date, activity_guid, modelComponentGuid);

            }
            else
            {
                var _calcScore = await GetMetricScoreFromOutSource(_candidateGuid, org_obj, activities, data);
                score = _calcScore.Count() > 0 ? _calcScore[0] : score;
            }

            //Score_Data score = await GetEmptyScore(data.model_component_guid, org_obj_guid, data.metric_measuring_unit, data.generate_calculate_date, activity_guid, modelComponentGuid);
            scores.Add(score);

            //add comment to model
            DateTime? date = calc_date;

            List<ScoreData> comments = GetAllComments(data, org_obj, activities, date, isRef);
            scores.AddRange(comments);

            //get reference scores for model
            //List<Score_Data> comment_scores = GetCommentReferenceScores(modelComponentGuid, org_obj_guid, activities, data.generate_calculate_date);
            //scores.AddRange(comment_scores);

            return scores;
        }


        private async Task<List<ScoreData>> GetMetricScoreData(UnitDataInfo org_obj, List<string> activities, CalculateMCData data, string rootOriginGuid = null)
        {
            List<ScoreData> scores = new List<ScoreData>();
            IEnumerable<ScoreData> query;

            if (data.is_reference)
            {
                //string[] org_obj_children = await _organizationService.GetOrgObjChildren(new string[] { org_obj.guid }, org_obj.union_guid);

                //get all metric scores from origin guid

                //string rootOriginGuid = _modelService.GetModelOriginGuid(data.origin_model_component_guid);

                query = DbContext.Score
                          .Where(s => (s.ModelComponentGuid == data.origin_model_component_guid || s.ModelComponentGuid == rootOriginGuid) &&
                                s.FormElementGuid == null &&
                                s.Status == (int)FormStatus.Authorized &&
                                s.EntityGuid == org_obj.guid)
                          .Select(s => new ScoreData(s, s.ActivityGu))
                          .AsEnumerable();

                //bool orgExist = query.Any(s => s.org_obj_guid == org_obj.guid);
                //if (orgExist)
                //{
                //    query = query.Where(s => s.org_obj_guid == org_obj.guid);
                //}
            }
            else
            {
                //get all metric scores
                query = DbContext.Score
                          .Where(s => s.ModelComponentGuid == data.origin_model_component_guid &&
                                s.FormElementGuid == null &&
                                s.Status == (int)FormStatus.Authorized &&
                                s.EntityGuid == org_obj.guid &&
                                (activities.Count == 0 || activities.Contains(s.ActivityGuid)))
                          .Select(s => new ScoreData(s, s.ActivityGu, org_obj.name))
                          .AsEnumerable();
            }

            if (data.generate_calculate_date.HasValue)
            {
                //get scores until generate_calculate_date
                query = query.Where(s => s.activity_end_date <= data.generate_calculate_date.Value);
            }

            scores = query.ToList();

            //get all expired scores
            var expired_scores_count = query.Where(s => IsExpired(s.activity_end_date, data.generate_calculate_date, data, org_obj)).Count();

            //get all irrelevant scores
            var irrelevant_scores_count = query.Where(s => s.original_score == -2).Count();

            //get all not inserted scores
            var not_inserted_scores_count = query.Where(s => s.original_score == -1).Count();

            if (scores.Count == expired_scores_count ||//all metric is expired
                     scores.Count == irrelevant_scores_count ||//all metric is irrelevant
                     scores.Count == not_inserted_scores_count ||//all metric is not inserted
                     scores.Count == 0)//metric has no scores
            {
                var last_score = scores.OrderBy(s => s.activity_end_date).Reverse().FirstOrDefault();
                string activity_guid = last_score != null ? last_score.activity_guid : null;
                string form_guid = last_score != null ? last_score.form_guid : null;
                int? score = null;

                if (scores.Count == 0 || scores.Count == not_inserted_scores_count)
                {
                    //metric has no scores - remove all scores and set score to -1
                    scores.Clear();
                    score = -1;
                }
                else if (scores.Count == expired_scores_count)
                {
                    //all metric is expired - remove all scores and set score to 0
                    scores.Clear();
                    score = 0;
                }
                else if (scores.Count == irrelevant_scores_count)
                {
                    //all metric is irrelevant - remove all scores and set score to -2
                    scores.Clear();
                    score = -2;
                }

                //add row with score
                scores.Add(new ScoreData()
                {
                    model_component_guid = data.model_component_guid,
                    //org_obj_guid = org_obj.guid,
                    //org_obj_name = org_obj.name,
                    entity_guid = org_obj.guid,
                    //unit_type =TODO
                    activity_guid = activity_guid,
                    form_guid = form_guid,
                    original_score = score,
                    convertion_score = score,
                    calculated_score = score,
                    score_level = 0,
                    calculated_date = data.generate_calculate_date.HasValue ? Util.ConvertDateToString(data.generate_calculate_date.Value) : string.Empty,
                    convertion_table = await _modelService.GetConvertionDetails(data.origin_model_component_guid)
                });
            }
            else if (scores.Count > 0)
            {
                //trim expired scores
                scores = scores.Where(s => !IsExpired(s.activity_end_date, data.generate_calculate_date, data, org_obj)).ToList();

                //get score after check calender rollup, remove all scores and add the relevant
                List<ScoreData> score_data_list = GetScoreByCalenderRollup(scores, data, data.generate_calculate_date);

                ScoreData score_data = score_data_list?[0];

                //get convertion table for metric score
                score_data.convertion_table = await _modelService.GetConvertionDetails(data.origin_model_component_guid);

                if (data.metric_measuring_unit != 3 &&
                   (data.show_origion_value == null || data.show_origion_value.Value == false))
                {
                    //if metric measuring unit not precentage and not set to origin score - change score by convertion table
                    score_data.convertion_score = score_data.convertion_table.Where(ct => ct.start_range <= score_data.original_score && ct.end_range >= score_data.original_score)
                                                                         .Select(ct => ct.conversion_table_final_score).FirstOrDefault();
                    score_data.calculated_score = score_data.convertion_score;
                }
                else
                {
                    score_data.calculated_score = score_data.original_score;
                }

                //get score after decrease period
                score_data.calculated_score = GetDecreasePeriod(data, score_data, data.generate_calculate_date);

                score_data.score_level = GetScoreLevel(score_data.calculated_score, data.metric_measuring_unit, score_data.convertion_table);

                scores.Clear();
                scores.AddRange(score_data_list);

                //update calc score if node not relevant - if the node has no childs calc service not change it
                if (scores[0].original_score == -1 || scores[0].original_score == -2)
                {
                    scores[0].calculated_score = scores[0].original_score;
                }
            }

            return scores;
        }

        private async Task<ScoreData> GetEmptyScore(string model_component_guid, UnitDataInfo org_obj, int? metric_measuring_unit, DateTime? calculate_date, string activity_guid, string origin_model_component_guid)
        {
            //create empty score for base or root model
            ScoreData model_score = new ScoreData();

            model_score.model_component_guid = model_component_guid;
            //model_score.org_obj_guid = org_obj.guid;
            //model_score.org_obj_name = org_obj.name;TODO
         
            model_score.activity_guid = activity_guid;
            model_score.calculated_date = calculate_date.HasValue ? Util.ConvertDateToString(calculate_date.Value) : string.Empty;
            model_score.convertion_table = await _modelService.GetConvertionDetails(origin_model_component_guid);

            return model_score;
        }

        private List<ScoreData> GetAllComments(CalculateMCData data, UnitDataInfo org_obj, List<string> activities, DateTime? calculate_date, bool isRef = false)
        {
            List<ScoreData> comments_scores = new List<ScoreData>();

            comments_scores = DbContext.Score
                               //.Where(s => (s.ModelComponentGuid == data.model_component_guid) &&
                                .Where(s => (isRef ? s.ModelComponentGuid == data.origin_model_component_guid : s.ModelComponentGuid == data.model_component_guid) &&
                                     !string.IsNullOrEmpty(s.ModelComponentComment) &&
                                     s.EntityGuid == org_obj.guid &&
                                     s.Status == (int)FormStatus.Authorized &&
                                     (activities.Count == 0 || activities.Contains(s.ActivityGuid)))
                               .Include(s => s.FormElementGu)
                               .Select(s => new ScoreData(s, s.ActivityGu, org_obj.name)).AsEnumerable()
                               .Where(s => (s.activity_end_date == null || !IsExpired(s.activity_end_date, calculate_date, data, org_obj))).ToList();

           return comments_scores;

           
        }

        private List<ScoreData> GetScoreByCalenderRollup(List<ScoreData> scores, CalculateMCData model_data, DateTime? calculate_date)
        {
            List<ScoreData> score_list = new List<ScoreData>();
            ScoreData score_data = new ScoreData();

            //switch on metric calender rollup
            switch ((CalenderRollupType)model_data.metric_calender_rollup)
            {
                case CalenderRollupType.smallest:
                    {
                        //get the smallest score value
                        score_data = scores.Where(s => s.original_score != -2).OrderBy(s => s.original_score).FirstOrDefault();
                        break;
                    }
                case CalenderRollupType.biggest:
                    {
                        //get the biggest score value
                        score_data = scores.Where(s => s.original_score != -2).OrderByDescending(s => s.original_score).FirstOrDefault();
                        break;
                    }
                case CalenderRollupType.last:
                    {
                        //get the last relevant inserted score value
                        score_data = scores.Where(s => s.original_score != -2).OrderByDescending(s => s.activity_end_date).FirstOrDefault();
                        break;
                    }
                case CalenderRollupType.last_set:
                    {
                        //get the last inserted score value - even if it irrelevant
                        score_data = scores.OrderByDescending(s => s.activity_end_date).FirstOrDefault();
                        break;
                    }
                case CalenderRollupType.cumulative:
                    {
                        //get sum of all scores
                        double? score = scores.Where(s => s.original_score != -2).Select(s => s.original_score).Sum();
                        score_data = scores.Where(s => s.original_score != -2).OrderByDescending(s => s.activity_end_date).FirstOrDefault();
                        score_data.original_score = score;
                        score_data.convertion_score = score;
                        score_data.calculated_score = score;

                        var comments_list = scores.Where(s => s != score_data &&
                                                              s.original_score != -2 &&
                                                              !string.IsNullOrEmpty(s.model_component_comment)).ToList();

                        score_list.AddRange(comments_list);

                        break;
                    }
                case CalenderRollupType.average:
                    {
                        //get average of all scores
                        IEnumerable<double> avg_scores = scores.Where(s => s.original_score != -2).Select(s => s.original_score.HasValue ? s.original_score.Value : 0);
                        int score = Convert.ToInt32(avg_scores.Sum() / avg_scores.Count());
                        score_data = scores.Where(s => s.original_score != -2).OrderBy(s => s.activity_end_date).Reverse().FirstOrDefault();
                        score_data.original_score = score;
                        score_data.convertion_score = score;
                        score_data.calculated_score = score;

                        var comments_list = scores.Where(s => s != score_data &&
                                                              s.original_score != -2 &&
                                                              !string.IsNullOrEmpty(s.model_component_comment)).ToList();

                        score_list.AddRange(comments_list);

                        break;
                    }
            }

            score_list.Insert(0, score_data);

            foreach (var score in score_list)
            {
                score.calculated_date = calculate_date.HasValue ? Util.ConvertDateToString(calculate_date.Value) : string.Empty;
            }

            return score_list;
        }

        private double? GetDecreasePeriod(CalculateMCData model_data, ScoreData score_data, DateTime? calculate_date)
        {
            double? calculatedScore = score_data.calculated_score;
            //if precent is 0 or period is 0 or score is equal or less then 0 - return the score and not doing anything
            if (model_data.metric_gradual_decrease_precent == 0 || model_data.metric_gradual_decrease_period == 0 || calculatedScore == null || calculatedScore.Value <= 0 || !calculate_date.HasValue)
                return calculatedScore;

            var totalDays = (calculate_date.Value - score_data.activity_end_date).TotalDays;
            int totalMonths = (int)Math.Truncate((totalDays % 365) / 30);
            int NumOfMonthSetting = (int)model_data.metric_gradual_decrease_period.Value;

            if (totalMonths < NumOfMonthSetting)
                return calculatedScore;

            int numOfMonth = totalMonths / NumOfMonthSetting;

            for (int i = 0; i < numOfMonth; i++)
            {
                calculatedScore = (calculatedScore * (100 - model_data.metric_gradual_decrease_precent.Value)) / 100;
            }

            if (calculatedScore < 0)
                calculatedScore = 0;

            return calculatedScore;
        }

        private static bool IsExpired(string activity_end_date_string, DateTime? calculate_date, CalculateMCData data, UnitDataInfo org_obj)
        {
            DateTime activity_end_date = Util.ConvertStringToDate(activity_end_date_string);

            return IsExpired(activity_end_date, calculate_date, data, org_obj);
        }

        private static bool IsExpired(DateTime activity_end_date, DateTime? calculate_date, CalculateMCData data, UnitDataInfo org_obj)
        {
            bool expired = false;

            string finalExpiredPeriod = data.metric_expired_period;
            if (org_obj.org_type.HasValue && (OrganizationObjectType)org_obj.org_type.Value == OrganizationObjectType.Secondary)
            {
                if (data.metric_expired_period_secondary == null)
                {
                    finalExpiredPeriod = "m36";
                }
                else
                {
                    finalExpiredPeriod = data.metric_expired_period_secondary;
                }
            }

            if (calculate_date.HasValue)
            {
                expired = GetExpiredDate(activity_end_date, finalExpiredPeriod) < calculate_date.Value;
            }

            return expired;
        }

        private static DateTime GetExpiredDate(DateTime startDate, string expired)
        {
            if (expired != null && expired != "0")
            {
                if (expired.IndexOf("y") != -1)
                {
                    startDate = startDate.AddYears(Convert.ToInt32(expired.Replace("y", "")));
                }
                else if (expired.IndexOf("m") != -1)
                {
                    startDate = startDate.AddMonths(Convert.ToInt32(expired.Replace("m", "")));
                }
                else if (expired.IndexOf("w") != -1)
                {
                    startDate = startDate.AddDays(7 * Convert.ToInt32(expired.Replace("w", "")));
                }
                else if (expired.IndexOf("d") != -1)
                {
                    startDate = startDate.AddDays(Convert.ToInt32(expired.Replace("d", "")));
                }
            }

            return startDate;
        }

        private bool IsReportExist(string report_guid)
        {
            var report = DbContext.SavedReports.Where(sr => sr.ReportGuid == report_guid).FirstOrDefault();
            if (report != null)
                return true;
            return false;
        }

        private void SetReportData(SavedReports saved_reports, ReportData data)
        {
            saved_reports.ReportGuid = data.report_guid;
            saved_reports.UserGuid = data.user_guid;
            saved_reports.ModelComponentGuid = data.model_component_guid;
            saved_reports.Name = data.report_name;
            saved_reports.IsWatch = false;
            saved_reports.IsPrimary = false;
            saved_reports.IsSecondary = false;
            saved_reports.ReportType = (int)data.report_type;
            saved_reports.Order = data.order;
            saved_reports.CandidateUserGuid = data.candidatesList.Count > 0 ? data.candidatesList[0].Split("@")[0] : null;
            saved_reports.TemplateType = data.TemplateType.HasValue ? data.TemplateType.Value : 0;
            saved_reports.IsDefined = data.IsDefined;
            saved_reports.UnionGuid = data.union_guid != null ? data.union_guid : "1";
            string new_date = string.Empty;

            foreach (DateTime date in data.calculated_date_list)
            {
                new_date += Util.ConvertDateToString(date) + ",";
            }

            saved_reports.CalculatedDates = new_date;
        }

        //private async Task<bool> SavedReportConnection(ReportData data)
        //{
        //    SavedReportConnection src;

        //    //remove report existing connection
        //    var existing_connections = await DbContext.SavedReportConnection.Where(s => s.ReportGuid == data.report_guid).ToListAsync();
        //    DbContext.SavedReportConnection.RemoveRange(existing_connections);
        //    await DbContext.SaveChangesAsync();


        //    //save report org_obj_guid_list
        //    foreach (UnitDataInfo org_obj in data.org_obj_list)
        //    {
        //        src = new SavedReportConnection();

        //        src.ReportGuid = data.report_guid;
        //        src.OrgObjGuid = org_obj.guid;

        //        DbContext.SavedReportConnection.Add(src);
        //    }

        //    //await DbContext.SaveChangesAsync();

        //    //save report comment_list
        //    foreach (string comment in data.comment_list)
        //    {
        //        src = new SavedReportConnection();

        //        src.ReportGuid = data.report_guid;
        //        src.Comment = comment;

        //        DbContext.SavedReportConnection.Add(src);
        //    }

        //    //await DbContext.SaveChangesAsync();

        //    //save report focus_list
        //    foreach (string focus in data.focus_list)
        //    {
        //        src = new SavedReportConnection();

        //        src.ReportGuid = data.report_guid;
        //        src.Focus = focus;

        //        DbContext.SavedReportConnection.Add(src);
        //    }

        //    await DbContext.SaveChangesAsync();
        //    return true;
        //}

        private List<string> GetSavedReport(SavedReports saved_report)
        {
            List<string> trees = new List<string>();

            var jsonTreeDATA = DbContext.SavedReportData
                                .Where(srd => srd.ReportGuid == saved_report.ReportGuid)
                                .Select(srd => srd.ReportData).FirstOrDefault();
            if (jsonTreeDATA != null)
            {
                trees.Add(jsonTreeDATA);
            }

            return trees;
        }

        private async Task FillReportList(string report_guid, CalculateTreeData tree, ReportTypes report_type, UnitDataInfo org_obj, DateTime? calculate_date, bool is_root, List<string> comment_guid_list, List<string> focus_guid_list, bool hasReference)
        {
            if (tree == null)
                return;

            if (string.IsNullOrEmpty(tree.data.score_data[0].entity_name))
                tree.data.score_data[0].entity_name = org_obj.name;

            if (is_root)
            {
                tree.data.model_data.focus_list = GetCalcReportFocus(tree, report_guid, org_obj.guid, focus_guid_list);//Focus
                tree.data.model_data.comment_list = GetNodeReportComment(tree, report_guid, calculate_date, comment_guid_list, hasReference);//Comment
            }

            //comments
            List<ModelComponentData> node_comment = GetModelComponentComment(tree, hasReference);

            tree.data.model_data.comment_list.AddRange(node_comment);


            //Trends
            tree.data.model_data.trend_list = await GetNodeTrends(tree, org_obj, calculate_date);

            //Weaknesses
            tree.data.model_data.weaknesses_list = await GetNodeWeaknesses(report_guid, tree, report_type);
        }

        private async Task<List<TrendData>> GetNodeTrends(CalculateTreeData tree, UnitDataInfo org_obj, DateTime? dateVal)
        {
            List<TrendData> trend_list = new List<TrendData>();

            if (dateVal.HasValue == false)
                return trend_list;

            DateTime date = dateVal.Value;

            if (IsModel(tree.data.model_data.source) || tree.data.model_data.is_reference)
            {
                trend_list = await GetModelTrends(tree, org_obj.guid, date);
            }
            else
            {
                trend_list = await GetMetricTrends(tree, org_obj, date);
            }

            return trend_list;
        }

        private async Task<List<TrendData>> GetModelTrends(CalculateTreeData tree, string org_obj_guid, DateTime date)
        {
            string modelGuid = tree.data.model_data.origin_model_component_guid != null ? tree.data.model_data.origin_model_component_guid : tree.data.model_data.model_component_guid;
            var trend_list = (from cs in DbContext.CalculateScore
                              where cs.ModelComponentGuid == modelGuid &&
                                     cs.CalculatedScore != null &&
                                     cs.EntityGuid == org_obj_guid &&
                                     !string.IsNullOrEmpty(cs.CalculatedDate) && !string.IsNullOrEmpty(cs.CalculatedDate.Trim())
                              //orderby cs.CalculatedDate
                              select new TrendData()
                              {
                                  model_component_guid = cs.ModelComponentGuid,
                                  name = tree.data.model_data.name,
                                  calculated_date = cs.CalculatedDate,
                                  score = cs.CalculatedScore
                              })
                                .AsEnumerable()
                                .Where(cs => date >= Util.ConvertStringToDate(cs.calculated_date));
            //.Distinct(new TrendDataComparer()).Take(5).ToList();

            var data = trend_list.GroupBy(x => x.calculated_date.Substring(0, 8)).Select(x => x.First())
                                 .OrderByDescending(x => Util.ConvertStringToDate(x.calculated_date)).Take(4).Reverse().ToList();

            data.Add(new TrendData()
            {
                model_component_guid = tree.data.model_data.model_component_guid,
                name = tree.data.model_data.name,
                calculated_date = Util.ConvertDateToString(date),
                score = tree.data.score_data.Count > 0 ? tree.data.score_data[0].calculated_score : null
            });

            return await Task.FromResult(data);
        }

        private async Task<List<TrendData>> GetMetricTrends(CalculateTreeData tree, UnitDataInfo org_obj, DateTime date)
        {
            string modelGuid = tree.data.model_data.origin_model_component_guid != null ? tree.data.model_data.origin_model_component_guid : tree.data.model_data.model_component_guid;
            var trend_list = (from cs in DbContext.Score
                              join a in DbContext.Activity on cs.ActivityGuid equals a.ActivityGuid
                              join ff in DbContext.Form on cs.FormGuid equals ff.FormGuid into formjointable
                              from f in formjointable.DefaultIfEmpty()
                              where cs.ModelComponentGuid == modelGuid &&
                                    cs.OriginalScore != null &&
                                    cs.Status == (int)FormStatus.Authorized &&
                                    cs.EntityGuid == org_obj.guid &&
                                    !string.IsNullOrEmpty(a.EndDate) &&
                                    !string.IsNullOrEmpty(a.EndDate.Trim())
                              orderby a.EndDate, cs.ScoreId
                              select new TrendData()
                              {
                                  model_component_guid = cs.ModelComponentGuid,
                                  name = tree.data.model_data.name,
                                  calculated_date = a.EndDate,
                                  score = cs.OriginalScore,
                                  convertion_score = tree.data.model_data.metric_measuring_unit == (int)MeasuringUnitType.percentage ? cs.OriginalScore : cs.ConvertionScore,
                                  score_level = GetScoreLevel(cs.ConvertionScore, tree.data.model_data.metric_measuring_unit, tree.data.score_data[0].convertion_table),
                                  is_expired = IsExpired(a.EndDate, date, tree.data.model_data, org_obj),
                                  form_guid = cs.FormGuid,
                                  form_name = f != null ? DbContext.FormTemplate.FirstOrDefault(ft => ft.FormTemplateGuid == f.FormTemplateGuid).Name : null
                              })
                                .AsEnumerable()
                                .Where(cs => date >= Util.ConvertStringToDate(cs.calculated_date));
            //.Distinct(new TrendDataComparer()).Take(5).ToList();


            var data = trend_list.GroupBy(x => x.calculated_date.Substring(0, 8)).Select(x => x.First()).Take(5).ToList();

            return await Task.FromResult(data);
        }

        private bool IsModel(int source)
        {
            return source == (int)ModelComponentSource.model_root || source == (int)ModelComponentSource.model;
        }

        private async Task<List<ModelComponentData>> GetNodeWeaknesses(string report_guid, CalculateTreeData tree, ReportTypes report_type)
        {
            List<ModelComponentData> weaknesses_list = new List<ModelComponentData>();

            LoopTree(tree, report_type, weaknesses_list, 1);

            weaknesses_list = weaknesses_list.OrderByDescending(w => w.order).ToList();

            return await Task.FromResult(weaknesses_list);
        }

        private void LoopTree(CalculateTreeData tree, ReportTypes report_type, List<ModelComponentData> weaknesses_list, int i)
        {
            if (tree.data.model_data.father_weight == null)
            {
                tree.data.model_data.father_weight = 1;
            }

            foreach (var child in tree.children)
            {
                child.data.model_data.father_weight = tree.data.model_data.father_weight * (child.data.model_data.weight / 100);
                LoopTree(child, report_type, weaknesses_list, i + 1);
            }

            //if (((report_type == ReportTypes.Units || report_type == ReportTypes.Dates) && i > 1) || i == 3 || i == 4)
            if (i > 2)
            {
                
                if (tree.data.model_data.is_weakness)
                {
                    ModelComponentData weakness = new ModelComponentData();

                    double? score = tree.data.score_data.Sum(x => x.calculated_score) / tree.data.score_data.Count;
                    var level = tree.data.model_data.score_level;

                    weakness.model_component_guid = tree.data.model_data.model_component_guid;
                    weakness.calculated_date = "threshold";
                    weakness.order = GetWeaknessOrder(tree, score, i);
                    weakness.score = score;
                    weakness.name = tree.data.model_data.name;
                    weakness.score_level = level;
                    weakness.professional_instruction = tree.data.model_data.professional_instruction;
                    weakness.comment_list = tree.data.model_data.comment_list;

                    weaknesses_list.Add(weakness);
                }

                foreach (var score in tree.data.score_data)
                {
                    var s = tree.data.reference_score_list != null && tree.data.reference_score_list.Count > 0 ? tree.data.reference_score_list.FirstOrDefault(x => x.org_obj_guid == score.entity_guid) : null;
                     var level = s != null ? s.score_level : score.score_level;
                    var finalScore = s != null ? s.score : score.calculated_score;
                    if (IsWeakness(finalScore, tree.data.model_data.metric_measuring_unit, level.HasValue ? level.Value : 0))
                    {

                        var exist = weaknesses_list.Exists(x => x.model_component_guid == tree.data.model_data.model_component_guid && s == null);
                        if (!exist)
                        {
                            ModelComponentData weakness = new ModelComponentData();

                            weakness.model_component_guid = tree.data.model_data.model_component_guid;
                            tree.data.model_data.is_weakness = false;
                            weakness.order = GetWeaknessOrder(tree, score.calculated_score, i);
                            //weakness.org_obj_guid = score.org_obj_guid;TODO
                            //weakness.org_obj_name = score.org_obj_name;
                            weakness.score = s != null ? s.score : score.calculated_score;
                            weakness.name = s != null ? tree.data.model_data.name + " - " + weakness.org_obj_name : tree.data.model_data.name;
                            weakness.score_level = level.HasValue ? level.Value : 0;
                            weakness.professional_instruction = tree.data.model_data.professional_instruction;
                            weakness.comment_list = tree.data.model_data.comment_list;

                            weaknesses_list.Add(weakness);
                        }
                    }
                }
            }
        }

        private bool IsWeakness(double? score, int? measuring_unit, int score_level = 0)
        {
            if (score == -1 || score == -2)
            {
                return false;
            }

            if ((measuring_unit.HasValue && measuring_unit.Value != (int)MeasuringUnitType.binary && score_level < 3) ||
                (measuring_unit.HasValue && measuring_unit.Value == (int)MeasuringUnitType.binary && score_level <= 1) ||
                (!measuring_unit.HasValue && score_level < 3))
            {
                return true;
            }

            return false;
        }

        private double GetWeaknessOrder(CalculateTreeData tree, double? calculated_score, int level)
        {
            double order = 0;

            double node_score = calculated_score ?? 0;
            double father_weight = tree.data.model_data.father_weight.HasValue ? tree.data.model_data.father_weight.Value : 1;

            order = (100 - node_score) / 100 * father_weight;

            if (tree.data.model_data.is_weakness)
                order *= 100;//extra 100 to threshold                

            return order;
        }

        private static int GetScoreLevel(double? calculated_score, int? metric_measuring_unit, List<ConvertionTableData> convertion_table)
        {
            int score_level = 0;

            if (calculated_score == null)
                return score_level;

            var nodeScore = calculated_score;
            nodeScore = (int)nodeScore;
            int resultParse = 1;
            foreach (var item in convertion_table)
            {
                if (metric_measuring_unit == 4 || metric_measuring_unit == 5) // metric qualitative
                {
                    if (item.conversion_table_final_score == calculated_score)
                    {
                        int.TryParse(item.level_id.ToString(), out resultParse);
                        score_level = resultParse;
                    }
                }
                else if (metric_measuring_unit == 2) // metric binary
                {
                    if (item.conversion_table_final_score == calculated_score)
                    {
                        int.TryParse(item.level_id.ToString(), out resultParse);
                        if (resultParse == 2)
                        {
                            resultParse = 5;
                        }

                        score_level = resultParse;
                    }
                }
                else
                {
                    if (item.start_range <= nodeScore && item.end_range >= nodeScore)
                    {
                        int.TryParse(item.level_id.ToString(), out resultParse);
                        score_level = resultParse;
                    }
                }
            }

            return score_level;
        }
        private string GetPolyGonByOrgAndModelGuid(string model_guid, string org_obj_guid)
        {
            var polygonGuid = DbContext.OrgModelPolygon
                       .Where(mc => mc.ModelComponentGuid == model_guid && mc.UnitGuid == org_obj_guid).FirstOrDefault()?.PolygonGuid;

            return polygonGuid;
        }



        private List<ModelComponentData> GetNodeReportComment(CalculateTreeData tree, string report_guid, DateTime? date, List<string> comment_guid_list, bool hasReference)
        {
            List<ModelComponentData> comment = tree.GetNodeAndDescendants()
               .Where(node => node.data.model_data.model_component_guid != tree.data.model_data.model_component_guid &&
                              comment_guid_list.Contains(node.data.model_data.model_component_guid))
               .SelectMany(mc => mc.data.score_data)
               .Where(s => !string.IsNullOrEmpty(s.model_component_comment) &&
                            (!date.HasValue || string.IsNullOrEmpty(s.calculated_date) || s.calculated_date.Trim() == string.Empty || Util.ConvertStringToDate(s.calculated_date) <= date))
               .Select(s => new ModelComponentData()
               {
                   model_component_guid = s.model_component_guid,
                   name = hasReference ?
                                !string.IsNullOrEmpty(s.form_element_title) ? s.form_element_title + " - " + (s.model_component_comment + " - " + s.entity_name) : (s.model_component_comment + " - " + s.entity_name) :
                                !string.IsNullOrEmpty(s.form_element_title) ? s.form_element_title + " - " + s.model_component_comment : s.model_component_comment
               }).ToList();

            comment = comment.GroupBy(x => x.name).Select(x => x.First()).ToList();

            return comment;
        }

        private List<ModelComponentData> GetModelComponentComment(CalculateTreeData tree, bool hasReference)
        {
            //add Comments
            List<ModelComponentData> comments = tree.data.score_data.Where(s => !string.IsNullOrEmpty(s.model_component_comment))
                                                                      .Select(s => new ModelComponentData()
                                                                      {
                                                                          model_component_guid = s.model_component_guid,
                                                                          name = hasReference ?
                                                                                  !string.IsNullOrEmpty(s.form_element_title) ? s.form_element_title + " - " + (s.model_component_comment + " - " + s.entity_name) : (s.model_component_comment + " - " + s.entity_name) :
                                                                                   !string.IsNullOrEmpty(s.form_element_title) ? s.form_element_title + " - " + s.model_component_comment : s.model_component_comment
                                                                      }).ToList();

            return comments;
        }

        private List<ModelComponentData> GetCalcReportFocus(CalculateTreeData tree, string report_guid, string org_obj_guid, List<string> focus_guid_list)
        {
            List<ModelComponentData> focus = tree.GetNodeAndDescendants()
                .Where(node => focus_guid_list.Contains(node.data.model_data.model_component_guid))
                .Select(mc => new ModelComponentData()
                {
                    model_component_guid = mc.data.model_data.model_component_guid,
                    name = mc.data.model_data.name,
                    score_level = mc.data.model_data.score_level,
                    score = mc.data.score_data.Count > 0 ? mc.data.score_data[0].calculated_score : null,
                    is_precentage = !(mc.data.model_data.metric_measuring_unit.HasValue && mc.data.model_data.metric_measuring_unit.Value == (int)MeasuringUnitType.quantitative)
                }).ToList();

            return focus;
        }

        private async Task<bool> ClearPrimaryOrSecondaryReport(SavedReports saved_report, ReportView report_view)
        {
            SavedReports exist_report = null;

            if (report_view == ReportView.isPrimary)
            {
                exist_report = await DbContext.SavedReports.Where(sr => sr.IsPrimary == true && sr.UserGuid == saved_report.UserGuid).FirstOrDefaultAsync();

                if (exist_report != null)
                {
                    exist_report.IsPrimary = false;
                    DbContext.SavedReports.Update(exist_report);
                }
            }
            else if (report_view == ReportView.isSecondary)
            {
                exist_report = await DbContext.SavedReports.Where(sr => sr.IsSecondary == true && sr.UserGuid == saved_report.UserGuid).FirstOrDefaultAsync();

                if (exist_report != null)
                {
                    exist_report.IsSecondary = false;
                    DbContext.SavedReports.Update(exist_report);
                }
            }

            await DbContext.SaveChangesAsync();
            return true;
        }

        #endregion Private methods

        public async Task<List<ScoreData>> GetMetricScoreFromOutSource(string _userGuid, UnitDataInfo org_obj, List<string> activities, CalculateMCData data)
        {
            // Util.LogMessage("GetMetricScoreData", "Start!", System.Diagnostics.TraceEventType.Verbose, null);

            List<ScoreData> scores = new List<ScoreData>();

            try
            {
                //get all metric scores
                scores = (from s in DbContext.OutSourceScore
                          where s.ModelComponentGuid == data.model_component_guid &&
                               s.UserGuid == _userGuid
                          select new ScoreData
                          {
                              //org_obj_guid = org_obj.guid,
                              //org_obj_name = org_obj.name,
                              entity_guid = org_obj.guid,
                              //unit_type =TODO
                              calculated_date = Util.ConvertDateToString(s.EventDate),
                              calculated_score = !String.IsNullOrEmpty(s.Score) ? Double.Parse(s.Score) : -1,
                              original_score = !String.IsNullOrEmpty(s.Score) ? Double.Parse(s.Score) : -1,
                              model_component_comment = null,
                              activity_end_date = s.EventDate,
                              formType = s.FormType,
                              AverageScore = s.AverageScore,
                              EvaluatingCount = s.EvaluatingCount != null ? s.EvaluatingCount : 0,
                              CandidateUnit = s.CandidateUnit,
                              CandidateRole = s.CandidateRole,
                              CandidateRank = s.CandidateRank,
                              TextAnswerQuestion = s.TextAnswerQuestion,
                              TextAnswerSummary = s.TextAnswerSummary
                          })?.ToList();

                if (scores.Count > 0 && data.generate_calculate_date.HasValue)
                {
                    //get scores until generate_calculate_date
                    if (scores[0].formType == 2)
                    {
                        var myDate = new DateTime(data.generate_calculate_date.Value.Year, data.generate_calculate_date.Value.Month, data.generate_calculate_date.Value.Day);
                        scores = scores.Where(s => s.activity_end_date == myDate).ToList();
                    }
                    else
                    {
                        scores = scores.OrderByDescending(s => s.activity_end_date).ToList();
                        scores = scores.Where(s => s.activity_end_date <= data.generate_calculate_date.Value).ToList();
                    }

                    //add row with score      
                }
                scores.Add(new ScoreData()
                {
                    model_component_guid = data.model_component_guid,
                    entity_guid = org_obj.guid,
                    original_score = scores.FirstOrDefault()?.calculated_score,
                    convertion_score = scores.FirstOrDefault()?.convertion_score,
                    calculated_score = scores.FirstOrDefault()?.calculated_score,
                    calculated_date = data.generate_calculate_date.HasValue ? Util.ConvertDateToString(data.generate_calculate_date.Value) : string.Empty,
                    convertion_table = await _modelService.GetConvertionDetails(data.origin_model_component_guid),
                    AverageScore = scores.FirstOrDefault()?.AverageScore,
                    EvaluatingCount = scores.FirstOrDefault()?.EvaluatingCount != null ? scores.FirstOrDefault()?.EvaluatingCount : 0,
                    CandidateUnit = scores.FirstOrDefault()?.CandidateUnit,
                    CandidateRole = scores.FirstOrDefault()?.CandidateRole,
                    CandidateRank = scores.FirstOrDefault()?.CandidateRank,
                    TextAnswerQuestion = scores.FirstOrDefault()?.TextAnswerQuestion,
                    TextAnswerSummary = scores.FirstOrDefault()?.TextAnswerSummary
                });

                if (scores.Count > 0)
                {

                    //get score after check calender rollup, remove all scores and add the relevant
                    //List<Score_Data> score_data_list = GetScoreByCalenderRollup(scores, data, org_obj_guid, data.generate_calculate_date);

                    ScoreData score_data = scores?[0];

                    //get convertion table for metric score
                    score_data.convertion_table = await _modelService.GetConvertionDetails(data.origin_model_component_guid);

                    if (_userGuid == null)
                    {
                        if (data.metric_measuring_unit != 3 &&
                    (data.show_origion_value == null || data.show_origion_value.Value == false))
                        {
                            //if metric measuring unit not precentage and not set to origin score - change score by convertion table
                            score_data.convertion_score = score_data.convertion_table.Where(ct => ct.start_range <= score_data.original_score && ct.end_range >= score_data.original_score)
                                                                                 .Select(ct => ct.conversion_table_final_score).FirstOrDefault();
                            score_data.calculated_score = score_data.convertion_score;

                        }
                        else
                        {
                            score_data.calculated_score = score_data.original_score;
                        }
                    }
                    else
                    {
                        score_data.calculated_score = score_data.original_score;

                    }

                    //get score after decrease period
                    score_data.calculated_score = GetDecreasePeriod(data, score_data, data.generate_calculate_date);

                    scores.Clear();
                    scores.Add(score_data);
                }
            }
            catch (Exception ex)
            {
                // Util.LogMessage("GetMetricScoreData", ex.Message, System.Diagnostics.TraceEventType.Error, null);
            }

            return scores;
        }



    }
}
