using Infrastructure.Helpers;
using Infrastructure.Services;
using Model.Data;
using Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Services
{
    public class ActivityTemplateService : BaseService
    {
        private ActivityService _activityService => this.GetChildService<ActivityService>();
        private FormService _formService => this.GetChildService<FormService>();
        private ModelService _modelService => this.GetChildService<ModelService>();

        public async Task<string> SaveActivityTemplate(ActivityTemplateData activity_template_data)
        {
            string guid;

            if (activity_template_data.activity_template.activity_template_guid == null)
                guid = await CreateActivityTemplate(activity_template_data);
            else
                guid = await UpdateActivityTemplate(activity_template_data);

            return guid;
        }

        public async Task<string> CreateActivityTemplate(ActivityTemplateData activity_template_data)
        {
            //save activity template
            //Activity_Template activity_template = Mapper.Map<Activity_Template>(activity_template_data.activity_template);
            ActivityTemplate activity_template = new ActivityTemplate();
            activity_template.ActivityTemplateGuid = Util.CreateGuid().ToString();
            activity_template.Name = activity_template_data.activity_template.name;
            activity_template.Description = activity_template_data.activity_template.description;
            activity_template.EntityType = activity_template_data.activity_template.entity_type;
            activity_template.ProfessionalRecommendations = activity_template_data.activity_template.professional_recommendations;

            activity_template.ActivityTemplateGuid = Util.CreateGuid().ToString();
            activity_template.CreateDate = Util.ConvertDateToString(DateTime.Now);
            activity_template.WithinTimeRange = activity_template_data.activity_template.within_time_range;
            activity_template.SubmitOnlyOnce = activity_template_data.activity_template.submit_only_once;

            await DbContext.ActivityTemplate.AddAsync(activity_template);

         
            //save form template connection
            foreach (int desc_guid in activity_template_data.connected_descriptions)
            {
                ActivityTemplateDescription activity_template_description = new ActivityTemplateDescription();
                activity_template_description.DescriptionGuid = desc_guid;
                activity_template_description.ActivityTemplateGuid = activity_template.ActivityTemplateGuid;
                DbContext.ActivityTemplateDescription.Add(activity_template_description);

            }
            foreach (string form_guid in activity_template_data.connected_form_templates)
            {
                AtInFt at_in_ft = new AtInFt();
                at_in_ft.ActivityTemplateGuid = activity_template.ActivityTemplateGuid;
                at_in_ft.FormTemplateGuid = form_guid;

                DbContext.AtInFt.Add(at_in_ft);
            }

            int saveChangeCount = await DbContext.SaveChangesAsync();

            string guid = saveChangeCount > 0 ? activity_template.ActivityTemplateGuid : string.Empty;

            return guid;
        }

        public async Task<string> UpdateActivityTemplate(ActivityTemplateData activity_template_data)
        {
            string guid = string.Empty;

            //update activity template
            ActivityTemplate activity_template = DbContext.ActivityTemplate.Find(activity_template_data.activity_template.activity_template_guid);

            activity_template.Name = activity_template_data.activity_template.name;
            activity_template.Description = activity_template_data.activity_template.description;
            activity_template.ProfessionalRecommendations = activity_template_data.activity_template.professional_recommendations;
            activity_template.WithinTimeRange = activity_template_data.activity_template.within_time_range;
            activity_template.SubmitOnlyOnce = activity_template_data.activity_template.submit_only_once;

            DbContext.ActivityTemplate.Update(activity_template);

            //remove activitytamplate descriptions
            var desc = DbContext.ActivityTemplateDescription.Where(ooc => ooc.ActivityTemplateGuid == activity_template.ActivityTemplateGuid);
            DbContext.ActivityTemplateDescription.RemoveRange(desc);

            //save organization connection
            foreach (int desc_guid in activity_template_data.connected_descriptions)
            {
                ActivityTemplateDescription activity_template_description = new ActivityTemplateDescription();
                activity_template_description.ActivityTemplateGuid = activity_template.ActivityTemplateGuid;
                activity_template_description.DescriptionGuid = desc_guid;

                DbContext.ActivityTemplateDescription.Add(activity_template_description);
            }

            var removedFT = DbContext.AtInFt.Where(x => x.ActivityTemplateGuid == activity_template.ActivityTemplateGuid && !activity_template_data.connected_form_templates.Contains(x.FormTemplateGuid)).Select(x => x.FormTemplateGuid).ToList();

            //remove form template connection
            var connected_form_templates = DbContext.AtInFt.Where(x => x.ActivityTemplateGuid == activity_template.ActivityTemplateGuid);
            DbContext.AtInFt.RemoveRange(connected_form_templates);
            await DbContext.SaveChangesAsync();

            AtInFt at_in_ft;

            //save form template connection
            foreach (string form_guid in activity_template_data.connected_form_templates)
            {
                at_in_ft = new AtInFt();
                at_in_ft.ActivityTemplateGuid = activity_template.ActivityTemplateGuid;
                at_in_ft.FormTemplateGuid = form_guid;

                DbContext.AtInFt.Add(at_in_ft);
                await DbContext.SaveChangesAsync();
            }

            foreach (var formTemplateGuid in activity_template_data.connected_form_templates)
            {
                var activities = DbContext.Activity.Where(a => a.ActivityTemplateGuid == activity_template.ActivityTemplateGuid).ToList();

                foreach (var activity in activities)
                {
                    if (activity.Users == null)
                    {
                        await _formService.CreateForm(activity, formTemplateGuid);
                    }
                    else
                    {
                        foreach (var user in activity.Users)
                        {
                            string userIncourse = user;
                            await _formService.CreateForm(activity, formTemplateGuid, userIncourse);
                        }
                    }
                    //await _formService.CreateForm(activity, formTemplateGuid);
                }
            }

            var deletedForms = DbContext.Form.Where(f => removedFT.Contains(f.FormTemplateGuid)).ToList();
            await _activityService.DeleteForms(deletedForms);

            guid = activity_template.ActivityTemplateGuid;

            return guid;
        }

        public Task<List<ActivityTemplateDataInfo>> GetActivityTemplates()
        {
            List<ActivityTemplateDataInfo> activity_templates_list = new List<ActivityTemplateDataInfo>();

            activity_templates_list = DbContext.ActivityTemplate.OrderBy(at => at.Name).Select(at => new ActivityTemplateDataInfo()
            {
                activity_template_guid = at.ActivityTemplateGuid,
                name = at.Name,
                description = at.Description,
                create_date = !string.IsNullOrEmpty(at.CreateDate) ? at.CreateDate : string.Empty,
                professional_recommendations = at.ProfessionalRecommendations,
                entity_type = at.EntityType,
                //is_poll = at.IsPoll
            }).ToList();

            return Task.FromResult(activity_templates_list);
        }

        public Task<ActivityTemplateData> GetActivityTemplateDetails(string activity_template_guid)
        {
            ActivityTemplateData activity_template_data = new ActivityTemplateData
            {
                activity_template = DbContext.ActivityTemplate
                                        .Where(at => at.ActivityTemplateGuid == activity_template_guid)
                                        .Select(at => new ActivityTemplateDataInfo(at)).FirstOrDefault(),

                connected_orgs = (from ad in DbContext.ActivityTemplateDescription
                                 join ed in DbContext.EntityDescription
                                 on ad.DescriptionGuid equals ed.DescriptionGuid
                                 where ad.ActivityTemplateGuid == activity_template_guid
                                 select ed.EntityGuid).ToList(),

                connected_form_templates = DbContext.AtInFt
                                        .Where(aif => aif.ActivityTemplateGuid == activity_template_guid)
                                        .Select(aif => aif.FormTemplateGuid).ToList(),


                 connected_descriptions = (from ad in DbContext.ActivityTemplateDescription
                                        where ad.ActivityTemplateGuid == activity_template_guid
                                        select ad.DescriptionGuid).ToList(),
            };

            return Task.FromResult(activity_template_data);
        }
        public async Task<bool> DeleteActivityTemplate(string activity_template_guid)
        {
            //remove organization connection
            var connected_orgs_list = DbContext.ActivityTemplateDescription.Where(ooc => ooc.ActivityTemplateGuid == activity_template_guid).ToList();
            DbContext.ActivityTemplateDescription.RemoveRange(connected_orgs_list);

            //remove form template connection
            var connected_form_templates = DbContext.AtInFt.Where(ooc => ooc.ActivityTemplateGuid == activity_template_guid).ToList();
            DbContext.AtInFt.RemoveRange(connected_form_templates);

            //get activity template activities
            List<Activity> activities = DbContext.Activity.Where(a => a.ActivityTemplateGuid == activity_template_guid).ToList();
            foreach (var activity in activities)
            {
                await _activityService.DeleteActivity(activity.ActivityGuid);
            }

            //remove activity template
            var activity_template = DbContext.ActivityTemplate.Where(at => at.ActivityTemplateGuid == activity_template_guid)?.FirstOrDefault();
            if (activity_template != null)
                DbContext.ActivityTemplate.Remove(activity_template);

            var saveChangesCount = await DbContext.SaveChangesAsync();
            return saveChangesCount > 0;
        }

        public async Task<List<object>> GetActivityTemplateFormTemplates(string activity_template_guid)
        {
            var res = DbContext.AtInFt.Where(aif => aif.ActivityTemplateGuid == activity_template_guid)
                                      .Select(aif => aif.FormTemplateGu).ToList<object>();

            return await Task.FromResult(res);
        }

        public async Task<List<FormItemDataMulti>> GetOrganizationsFormScores(string formTemplateGuid, string activityGroupGuid, string[] orgList)
        {
            var form_items_list = DbContext.FormTemplateStructure
                .Where(fts => fts.FormTemplateGuid == formTemplateGuid)
                .Select(fts => new FormItemDataMulti
                {
                    order = fts.Order,
                    model_component_guid = fts.ModelComponentGuid ?? null,
                    title = fts.ModelComponentGuid != null ? fts.ModelComponentGu.Name : fts.FormElementGu != null ? fts.FormElementGu.FormElementTitle : null,
                    metric_status = fts.ModelComponentGuid != null ? fts.ModelComponentGu.Status : null,
                    metric_required = fts.ModelComponentGuid != null ? fts.ModelComponentGu.MetricRequired : null,
                    metric_measuring_unit = fts.ModelComponentGuid != null ? fts.ModelComponentGu.MetricMeasuringUnit : null,
                    metric_not_display_if_irrelevant = fts.ModelComponentGuid != null ? fts.ModelComponentGu.MetricNotDisplayIfIrrelevant : null,
                    metric_show_origion_value = fts.ModelComponentGuid != null ? fts.ModelComponentGu.ShowOrigionValue : null,
                    professional_instruction = fts.ModelComponentGuid != null ? fts.ModelComponentGu.ProfessionalInstruction : null,
                    form_element_guid = fts.FormElementGuid != null ? fts.FormElementGuid : null,
                    form_element_type = fts.FormElementGuid != null ? fts.FormElementGu.FormElementType : null,
                    connected_model_guid = fts.FormElementGuid != null ? DbContext.FormElementConnection.Where(fec => fec.FormElementGuid == fts.FormElementGuid)
                                        .OrderBy(x => x.Order).Select(fec => fec.ModelComponentGuid).ToList() : null,
                    convertion_table = Mapper.Map<List<ConvertionTableData>>(DbContext.ConvertionTable.Where(x => x.ModelComponentGuid == fts.ModelComponentGuid).ToList())
                }).OrderBy(x => x.order).ToList();

            Dictionary<string, List<FormSsoreBaseData>> all_scores = new Dictionary<string, List<FormSsoreBaseData>>();

            foreach (var org in orgList)
            {
                var res = (from s in DbContext.Score
                           join f in DbContext.Form on s.FormGuid equals f.FormGuid
                           join a in DbContext.Activity on s.ActivityGuid equals a.ActivityGuid
                           where f.FormTemplateGuid == formTemplateGuid &&
                           //a.ActivityGroupGuid == activityGroupGuid && TODO activityconections
                           s.EntityGuid == org
                           select new FormSsoreBaseData
                           {
                               entity_guid = f.EntityGuid,
                               entity_type = f.EntityType,
                               model_component_guid = s.ModelComponentGuid,
                               original_score = s.OriginalScore,
                               model_component_comment = s.ModelComponentComment,
                               status = s.Status,
                               form_guid = f.FormGuid,
                               activity_guid = a.ActivityGuid
                           }).ToList();

                all_scores.Add(org, res);
            }

            foreach (var form_item in form_items_list)
            {
                foreach (var org in orgList)
                {
                    var orgScores = all_scores.GetValueOrDefault(org);
                    var modelScore = orgScores.FirstOrDefault(s => s.model_component_guid == form_item.model_component_guid);
                    if (modelScore != null)
                    {
                        modelScore.original_score = modelScore.original_score != -1 ? modelScore.original_score : null;
                        modelScore.model_component_comment = modelScore.model_component_comment ?? string.Empty;

                        if (modelScore.original_score == -2)//irrelevant
                        {
                            modelScore.original_score = null;
                            modelScore.metric_form_irrelevant = true;
                        }
                    }
                    else
                    {
                        //not fill
                        modelScore = new FormSsoreBaseData();
                        modelScore.entity_guid = org;
                        modelScore.model_component_guid = form_item.model_component_guid;

                        var form = (from f in DbContext.Form
                                    join a in DbContext.Activity on f.ActivityGuid equals a.ActivityGuid
                                    where f.FormTemplateGuid == formTemplateGuid &&
                                    //a.ActivityGroupGuid == activityGroupGuid && TODO activityconections
                                    f.EntityGuid == org
                                    select f).FirstOrDefault();

                        if (form != null)
                        {
                            modelScore.form_guid = form.FormGuid;
                            modelScore.activity_guid = form.ActivityGuid;
                        }
                        modelScore.status = 1;
                    }

                    form_item.orgs_scores.Add(modelScore);
                }
            }

            return await Task.FromResult(form_items_list);
        }

        public async Task<bool> SaveOrganizationsFormScores(List<FormItemDataMulti> formItemsScores)
        {
            bool is_new;
            Score score = null;
            List<string> formsGuids = new List<string>();
            int? status = 1;

            foreach (var form_item in formItemsScores)
            {
                foreach (var orgScore in form_item.orgs_scores)
                {
                    is_new = false;

                    score = DbContext.Score.Where(s => ((s.ModelComponentGuid != null && s.ModelComponentGuid == form_item.model_component_guid) ||
                                             (s.FormElementGuid != null && s.FormElementGuid == form_item.form_element_guid)) &&
                                              s.EntityGuid == orgScore.entity_guid &&
                                              s.ActivityGuid == orgScore.activity_guid &&
                                              s.FormGuid == orgScore.form_guid).FirstOrDefault();
                    if (score == null)
                    {
                        is_new = true;
                        score = new Score
                        {
                            EntityGuid = orgScore.entity_guid,
                            ActivityGuid = orgScore.activity_guid,
                            FormGuid = orgScore.form_guid,
                            ModelComponentGuid = form_item.model_component_guid,
                            FormElementGuid = form_item.form_element_guid
                        };
                    }

                    score.Status = orgScore.status;
                    score.ModelComponentComment = orgScore.model_component_comment;
                    score.ModelComponentGuid = form_item.model_component_guid;
                    score.FormElementGuid = form_item.form_element_guid;

                    if (orgScore.metric_form_irrelevant)
                    {
                        score.OriginalScore = -2;
                        score.ConvertionScore = null;
                    }
                    else if (orgScore.original_score == -1 && form_item.metric_measuring_unit == null)//model
                    {
                        score.OriginalScore = null;
                        score.ConvertionScore = null;
                    }
                    else if (orgScore.original_score == null && form_item.metric_measuring_unit != null)
                    {
                        score.OriginalScore = -1;
                        score.ConvertionScore = null;
                    }
                    else
                    {

                        var convertion_table = await _modelService.GetConvertionDetails(form_item.model_component_guid);

                        score.OriginalScore = orgScore.original_score;
                        score.ConvertionScore = convertion_table
                                .Where(ct => ct.start_range <= score.OriginalScore && ct.end_range >= score.OriginalScore)
                                .Select(ct => ct.conversion_table_final_score).FirstOrDefault();
                    }

                    if (is_new)
                    {
                        await DbContext.Score.AddAsync(score);
                    }
                    else
                    {
                        DbContext.Score.Update(score);
                    }
                    await DbContext.SaveChangesAsync();

                    status = score.Status;
                    if (!formsGuids.Contains(score.FormGuid))
                        formsGuids.Add(score.FormGuid);
                }
            }

            foreach (var formGuid in formsGuids)
            {
                //update form status
                Form form = DbContext.Form.Where(f => f.FormGuid == formGuid).FirstOrDefault();
                if (form != null && form.Status != status)
                {
                    form.Status = status;
                    DbContext.Form.Update(form);
                }

                await DbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}
