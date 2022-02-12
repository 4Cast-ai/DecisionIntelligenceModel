using Infrastructure.Helpers;
using Infrastructure.Services;
using Model.Data;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FormElementType = Model.Data.FormElementType;

namespace Dal.Services
{
    public class FormTemplateService : BaseService
    {
        private ActivityService _activityService => this.GetChildService<ActivityService>();
        private FormService _formService => this.GetChildService<FormService>();
        private ModelService _modelService => this.GetChildService<ModelService>();

        #region Get

        public Task<List<FormTemplateDataInfo>> GetAllFormTemplates()
        {
            List<FormTemplateDataInfo> form_templates_list = new List<FormTemplateDataInfo>();

            form_templates_list = DbContext.FormTemplate.OrderBy(ft => ft.Name).Select(ft => new FormTemplateDataInfo()
            {
                form_template_guid = ft.FormTemplateGuid,
                name = ft.Name,
                description = ft.Description,
                modified_date = !string.IsNullOrEmpty(ft.ModifiedDate) ? ft.ModifiedDate : string.Empty,
                create_date = ft.CreateDate,
                creator_user_guid = ft.CreatorUserGuid
            }).ToList();

            return Task.FromResult(form_templates_list);
        }

        public async Task<FormTemplateData> GetFormTemplateDetails(string form_template_guid)
        {
            FormTemplateData form_Data = new FormTemplateData();
            var ft = DbContext.FormTemplate.FirstOrDefault(ft => ft.FormTemplateGuid == form_template_guid);
            if (ft != null)
            {
                form_Data.form_template = new FormTemplateDataInfo(ft);
                form_Data.form_items_list = await GetFormItemsList(form_template_guid);
                form_Data.activities = await DbContext.AtInFt.Where(aif => aif.FormTemplateGuid == form_template_guid)
                                            .Select(aif => aif.ActivityTemplateGuid).ToListAsync();
            }
            return form_Data;
        }

        public async Task<List<FormItemData>> GetFormItemsList(string form_template_guid, string form_guid = null)
        {
            List<FormItemData> form_items_list = new List<FormItemData>();

            form_items_list = (from fts in DbContext.FormTemplateStructure
                               join m in DbContext.ModelComponent on fts.ModelComponentGuid equals m.ModelComponentGuid into mlist
                               from mc in mlist.DefaultIfEmpty()
                               where fts.FormTemplateGuid == form_template_guid
                               select new FormItemData()
                               {
                                   order = fts.Order,
                                   model_component_guid = fts.ModelComponentGuid ?? null,
                                   title = fts.ModelComponentGuid != null ? fts.ModelComponentGu.Name : fts.FormElementGu != null ? fts.FormElementGu.FormElementTitle : null,
                                   metric_status = fts.ModelComponentGuid != null ? fts.ModelComponentGu.Status : null,
                                   metric_required = fts.ModelComponentGuid != null ? fts.ModelComponentGu.MetricRequired : null,
                                   metric_measuring_unit = fts.ModelComponentGuid != null ? fts.ModelComponentGu.MetricMeasuringUnit : null,
                                   metric_rollup_method = fts.ModelComponentGuid != null ? fts.ModelComponentGu.MetricRollupMethod : null,
                                   metric_not_display_if_irrelevant = fts.ModelComponentGuid != null ? fts.ModelComponentGu.MetricNotDisplayIfIrrelevant : null,
                                   metric_show_origion_value = fts.ModelComponentGuid != null ? fts.ModelComponentGu.ShowOrigionValue : null,
                                   professional_instruction = fts.ModelComponentGuid != null ? fts.ModelComponentGu.ProfessionalInstruction : null,
                                   form_element_guid = fts.FormElementGuid != null ? fts.FormElementGuid : null,
                                   form_element_type = fts.FormElementGuid != null ? fts.FormElementGu.FormElementType : null,
                                   connected_model_guid = fts.FormElementGuid != null ? DbContext.FormElementConnection.Where(fec => fec.FormElementGuid == fts.FormElementGuid)
                                                       .OrderBy(x => x.Order).Select(fec => fec.ModelComponentGuid).ToList() : null,
                                   source = mc != null ? mc.Source : 0
                               })
                .OrderBy(x => x.order).ToList();


            if (!string.IsNullOrEmpty(form_guid))
            {
                //get score and convertion table
                foreach (FormItemData form_item in form_items_list)
                {
                    Score score = DbContext.Score.FirstOrDefault(s => ((s.ModelComponentGuid != null && s.ModelComponentGuid == form_item.model_component_guid) ||
                                        (s.FormElementGuid != null && s.FormElementGuid == form_item.form_element_guid)) && s.FormGuid == form_guid);
                    if (score != null)
                    {
                        form_item.score = score.OriginalScore != -1 ? score.OriginalScore : null;
                        form_item.comment = score.ModelComponentComment ?? string.Empty;

                        if (form_item.score == -2)//irrelevant
                        {
                            form_item.metric_not_display_if_irrelevant = true;
                            form_item.score = null;
                            form_item.metric_form_irrelevant = true;
                        }
                        else
                        {
                            form_item.metric_form_irrelevant = false;

                        }

                    }

                    form_item.showConverTableFlag = false;
                    form_item.convertion_table = await _modelService.GetConvertionDetails(form_item.model_component_guid);
                }
            }

            return form_items_list;
        }

        #endregion Get

        #region Save And Update

        public async Task<string> SaveFormTemplate(FormTemplate form_template, List<FormItemData> form_items_list, List<string> activities_list)
        {
            string guid;

            if (form_template.FormTemplateGuid == null)
                guid = await CreateFormTemplate(form_template, form_items_list, activities_list);
            else
                guid = await UpdateFormTemplate(form_template, form_items_list, activities_list);

            return guid;
        }

        public async Task<string> CreateFormTemplate(FormTemplate form_template, List<FormItemData> form_items_list, List<string> activities_list)
        {
            string guid;

            //save form_template to Form_Template
            form_template.FormTemplateGuid = Util.CreateGuid().ToString();
            //form_template.creator_user_guid = "92abb1cc525840aaa085e4fad18e4cf6";

            string now = Util.ConvertDateToString(DateTime.Now);
            form_template.CreateDate = now;
            form_template.ModifiedDate = now;

            await DbContext.FormTemplate.AddAsync(form_template);
            await DbContext.SaveChangesAsync();

            //save form_items_list to Form_Template_Structure
            await SaveFormTemplateStructure(form_template, form_items_list);

            //save activities to AT_In_FT (activity template in form template)
            await SaveFormTemplatesActivities(form_template, activities_list, "Create");

            guid = form_template.FormTemplateGuid;
            return guid;
        }

        public async Task<string> UpdateFormTemplate(FormTemplate form_template, List<FormItemData> form_items_list, List<string> activities_list)
        {
            string guid = null;
            
            //update Form_Template
            FormTemplate existing_form_template = DbContext.FormTemplate.FirstOrDefault(ft => ft.FormTemplateGuid == form_template.FormTemplateGuid);
            
            if (existing_form_template != null)
            {
                existing_form_template.Name = form_template.Name;
                existing_form_template.Description = form_template.Description;

                string now = Util.ConvertDateToString(DateTime.Now);
                //existing_form_template.CreateDate = now;
                existing_form_template.ModifiedDate = now;

                DbContext.FormTemplate.Update(existing_form_template);
                DbContext.SaveChanges();


                //save form_items_list to Form_Template_Structure
                await SaveFormTemplateStructure(form_template, form_items_list);

                //save activities to AT_In_FT (activity template in form template)
                await SaveFormTemplatesActivities(existing_form_template, activities_list, "Update");

                guid = existing_form_template.FormTemplateGuid;
            }

            return guid;
        }

        public async Task SaveFormTemplateStructure(FormTemplate form_template, List<FormItemData> form_items_list)
        {
            //get all deleted form items
            var deleted_form_item_list = DbContext.FormTemplateStructure
                                            .Where(fts => fts.FormTemplateGuid == form_template.FormTemplateGuid &&
                                                  ((fts.ModelComponentGuid != null && !form_items_list.Select(x => x.model_component_guid).Contains(fts.ModelComponentGuid)) ||
                                                  (fts.FormElementGuid != null && !form_items_list.Select(x => x.form_element_guid).Contains(fts.FormElementGuid)))).ToList();

            //loop of deleted items and remove from db
            foreach (var deleted_form_item in deleted_form_item_list)
            {
                await DeleteFormTemplateStructure(deleted_form_item);
            }

            //save to Form_Template_Structure
            foreach (var form_item in form_items_list)
            {
                var existing_form_structure = DbContext.FormTemplateStructure
                                                .FirstOrDefault(fts => fts.FormTemplateGuid == form_template.FormTemplateGuid &&
                                                      ((form_item.model_component_guid != null && fts.ModelComponentGuid == form_item.model_component_guid) ||
                                                      (form_item.form_element_guid != null && fts.FormElementGuid == form_item.form_element_guid)));

                if (existing_form_structure == null)
                {
                    await CreateFormTemplateStructure(form_item, form_template);
                }
                else
                {
                    await UpdateFormTemplateStructure(existing_form_structure, form_item);
                }
            }
        }

        public async Task CreateFormTemplateStructure(FormItemData form_item, FormTemplate form_template)
        {
            FormTemplateStructure form_structure = new FormTemplateStructure
            {
                FormTemplateGuid = form_template.FormTemplateGuid,
                Order = form_item.order
            };

            if (form_item.model_component_guid != null)
            {
                form_structure.ModelComponentGuid = form_item.model_component_guid;
            }
            else if (form_item.form_element_type.HasValue)
            {
                //create Form_Element from type title
                FormElement form_element = new FormElement();
                form_element.FormElementGuid = Util.CreateGuid().ToString();

                switch (form_item.form_element_type.Value)
                {
                    case (int)FormElementType.Title:
                        {
                            form_element.FormElementTitle = form_item.title;
                            form_element.FormElementType = (int)FormElementType.Title;

                            DbContext.FormElement.Add(form_element);
                            DbContext.SaveChanges();
                            break;
                        }
                    case (int)FormElementType.TextArea:
                        {
                            form_element.FormElementTitle = form_item.title;
                            form_element.FormElementType = (int)FormElementType.TextArea;

                            DbContext.FormElement.Add(form_element);
                            DbContext.SaveChanges();

                            int i = 1;
                            foreach (string connected_model_guid in form_item.connected_model_guid)
                            {
                                FormElementConnection element_connected = new FormElementConnection();
                                element_connected.FormElementGuid = form_element.FormElementGuid;
                                element_connected.ModelComponentGuid = connected_model_guid;
                                element_connected.Order = i++;

                                DbContext.FormElementConnection.Add(element_connected);
                                DbContext.SaveChanges();
                            }

                            break;
                        }
                }

                //set form_element_guid to new form element created
                form_structure.FormElementGuid = form_element.FormElementGuid;
            }

            DbContext.FormTemplateStructure.Add(form_structure);
            await DbContext.SaveChangesAsync();
        }

        public async Task UpdateFormTemplateStructure(FormTemplateStructure existing_form_structure, FormItemData form_item)
        {
            existing_form_structure.Order = form_item.order;

            if (form_item.form_element_type.HasValue)
            {
                //get Form_Element from type title
                FormElement form_element = DbContext.FormElement.Where(fe => fe.FormElementGuid == form_item.form_element_guid).FirstOrDefault();

                switch (form_item.form_element_type.Value)
                {
                    case (int)FormElementType.Title:
                        {
                            form_element.FormElementTitle = form_item.title;

                            DbContext.FormElement.Update(form_element);
                            DbContext.SaveChanges();
                            break;
                        }
                    case (int)FormElementType.TextArea:
                        {
                            form_element.FormElementTitle = form_item.title;

                            DbContext.FormElement.Update(form_element);
                            DbContext.SaveChanges();

                            //remove existing form element connection to this form_element_guid
                            var form_element_connection_list = DbContext.FormElementConnection.Where(fec => fec.FormElementGuid == form_element.FormElementGuid).ToList();
                            DbContext.FormElementConnection.RemoveRange(form_element_connection_list);
                            DbContext.SaveChanges();

                            ////add all form element connection from form_item.connected_model_guid
                            int i = 1;
                            foreach (string connected_model_guid in form_item.connected_model_guid)
                            {
                                FormElementConnection element_connected = new FormElementConnection();
                                element_connected.FormElementGuid = form_element.FormElementGuid;
                                element_connected.ModelComponentGuid = connected_model_guid;
                                element_connected.Order = i++;

                                DbContext.FormElementConnection.Add(element_connected);
                                DbContext.SaveChanges();
                            }

                            break;
                        }
                }
            }

            DbContext.FormTemplateStructure.Update(existing_form_structure);
            await DbContext.SaveChangesAsync();
        }

        public async Task SaveFormTemplatesActivities(FormTemplate form_template, List<string> activities_list, string action)
        {
            activities_list = activities_list.Distinct().ToList();
            var removedAT = DbContext.AtInFt.Where(x => x.FormTemplateGuid == form_template.FormTemplateGuid && !activities_list.Contains(x.ActivityTemplateGuid)).Select(x=>x.ActivityTemplateGuid).ToList();

            //remove connection to form template guid
            var atInft = DbContext.AtInFt.Where(x => x.FormTemplateGuid == form_template.FormTemplateGuid);
            DbContext.AtInFt.RemoveRange(atInft);
            await DbContext.SaveChangesAsync();

            //add activities_list
            AtInFt activity_in_form;
            foreach (string activity_template_guid in activities_list)
            {
                activity_in_form = new AtInFt();
                activity_in_form.FormTemplateGuid = form_template.FormTemplateGuid;
                activity_in_form.ActivityTemplateGuid = activity_template_guid;

                DbContext.AtInFt.Add(activity_in_form);
                await DbContext.SaveChangesAsync();

                //get all activities and add form
                List<Activity> activities = DbContext.Activity.Where(a => a.ActivityTemplateGuid == activity_template_guid).ToList();

                foreach (var activity in activities)
                {
                    if (activity.Users == null)
                    {
                        await _formService.CreateForm(activity, form_template.FormTemplateGuid);
                    }
                    else
                    {
                        foreach (var user in activity.Users)
                        {
                            string userIncourse = user;
                            await _formService.CreateForm(activity, form_template.FormTemplateGuid, userIncourse);
                        }
                    }

                    //await _formService.CreateForm(activity, form_template.FormTemplateGuid);
                }

                var deletedForms = DbContext.Form.Where(f => f.FormTemplateGuid == form_template.FormTemplateGuid && removedAT.Contains(f.ActivityGu.ActivityTemplateGuid)).ToList();
                await _activityService.DeleteForms(deletedForms);
            }
        }

        #endregion Save And Update

        #region Delete

        public async Task<bool> DeleteFormTemplate(string form_tempate_guid)
        {
            var deleted_form_template = DbContext.FormTemplate.Where(ft => ft.FormTemplateGuid == form_tempate_guid).FirstOrDefault();

            //get all forms by form template
            List<Form> forms = DbContext.Form.Where(f => f.FormTemplateGuid == form_tempate_guid).ToList();

            //remove form template connected forms
            await _activityService.DeleteForms(forms);

            //remove activity template connection
            var connected_activities_templates = DbContext.AtInFt.Where(x => x.FormTemplateGuid == form_tempate_guid);
            DbContext.AtInFt.RemoveRange(connected_activities_templates);

            var form_template_structure_list = DbContext.FormTemplateStructure.Where(fts => fts.FormTemplateGuid == form_tempate_guid).ToList();

            foreach (var form_template_structure in form_template_structure_list)
            {
                await DeleteFormTemplateStructure(form_template_structure);
            }

            DbContext.FormTemplate.Remove(deleted_form_template);
            var saveChangesCount = await DbContext.SaveChangesAsync();

            return saveChangesCount > 0;
        }

        public async Task DeleteFormTemplateStructure(FormTemplateStructure deleted_form_item)
        {
            DbContext.FormTemplateStructure.Remove(deleted_form_item);

            if (deleted_form_item.FormElementGuid != null)
            {
                var form_element_connection_list = DbContext.FormElementConnection.Where(fec => fec.FormElementGuid == deleted_form_item.FormElementGuid).ToList();
                DbContext.FormElementConnection.RemoveRange(form_element_connection_list);

                var scores = DbContext.Score.Where(s => s.FormElementGuid == deleted_form_item.FormElementGuid);
                DbContext.Score.RemoveRange(scores);

                var form_element = DbContext.FormElement.Where(fe => fe.FormElementGuid == deleted_form_item.FormElementGuid).FirstOrDefault();
                DbContext.FormElement.Remove(form_element);
            }

            await DbContext.SaveChangesAsync();
        }

        #endregion Delete
    }
}
