using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Model.Data;
using Model.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Activity = Model.Entities.Activity;
using FormElementType = Model.Data.FormElementType;
using FormStatus = Model.Data.FormStatus;
using System;

namespace Dal.Services
{
    public class FormService : BaseService
    {
        private ModelService _modelService => this.GetChildService<ModelService>();
        private ActivityService _activityService => this.GetChildService<ActivityService>();
        private FormTemplateService _formTemplateService => this.GetChildService<FormTemplateService>();

        public async Task<FormData> GetFormDetails(string form_guid)/*TODO*/
        {
            //var form = DbContext.Form
            //    .Include(x => x.FormTemplateGu).Include(x => x.OrgObjGu)
            //    .FirstOrDefault(f => f.FormGuid == form_guid);

            //TODO למחוק את זה קוד זמני בלבד בגלל הסטטוס 4 שנמחק

            //var form = DbContext.Form.Where(x => x.Status != 4)
            // .Include(x => x.FormTemplateGu).Include(x => x.OrgObjGu)
            // .FirstOrDefault(f => f.FormGuid == form_guid);

            var form = (from f in DbContext.Form

                        join un in DbContext.Unit on f.EntityGuid equals un.UnitGuid into units
                        from uuu in units.DefaultIfEmpty()

                        join pp in DbContext.Person on f.EntityGuid equals pp.PersonGuid into persons
                        from ppp in persons.DefaultIfEmpty()


                        //join o in DbContext.OrganizationObject on f.ApproveUserGuid equals o.OrgObjGuid into app
                        //from a in app
                        where f.Status != 4
                        && f.FormGuid == form_guid
                        select new FormDetails
                        {
                            form_guid = f.FormGuid,
                            form_template_guid = f.FormTemplateGuid,
                            activity_guid = f.ActivityGuid,
                            name = f.FormTemplateGu.Name,
                            approve_user_guid = f.ApproveUserGuid,
                            approve_user_name = f.ApproveUserGuid !=null ? f.ApproveUserGu.UserFirstName + " " + f.ApproveUserGu.UserLastName: null,
                            approve_date = f.ApproveDate,
                            status = f.Status,
                            //org_obj_guid = f.OrgObjGuid,
                            //org_obj_name = f.OrgObjGu.Name
                            entity_guid = f.EntityGuid,
                            entity_name = uuu == null ? ppp.FirstName + " " + ppp.LastName : uuu.UnitName,
                            entity_type = f.EntityType                            /*MyProperty =*/


                        }).FirstOrDefault();

            if (form == null) return null;

            FormDetails form_details = Mapper.Map<FormDetails>(form); //new FormDetails(formDetails, formDetails.FormTemplateGu, formDetails.OrgObjGu);

            ActivityDetails activity_details = await _activityService.GetActivity(form_details.activity_guid);
            List<FormItemData> form_items = await _formTemplateService.GetFormItemsList(form_details.form_template_guid, form_guid);

            List<FormGroupData> res = new List<FormGroupData>();
            List<FormGroupData> res2 = new List<FormGroupData>();
            int sidx = 0;
            int eidx = 0;
            int list_length = form_items.Count;

            for (int i = 0; i < list_length; i++)
            {
                if (form_items[i].form_element_type.HasValue && form_items[i].form_element_type.Value == (int)FormElementType.Title || form_items[i].source == 6 || form_items[i].source == 7)
                {
                    sidx = i + 1;
                    var resList = form_items.GetRange(sidx, list_length - sidx);
                    int resIdx = -1;
                    if (form_items[i].form_element_type.HasValue && form_items[i].form_element_type.Value == (int)FormElementType.Title || form_items[i].source == 7)
                    {
                        resIdx = resList.FindIndex(x => x.form_element_type.HasValue && (x.form_element_type.Value == (int)FormElementType.Title || x.source == 7)) ;
                    }
                    //else if(form_items[i].source == 7) 
                    //{
                    //    resIdx = resList.FindIndex(x => x.source == 7);
                    //}
                    else
                    {
                        resIdx = resList.FindIndex(x =>  x.source == 6 );
                    }
                    eidx = resIdx == -1 ? list_length : (sidx + resIdx);
                    List<FormItemData> items = form_items.GetRange(sidx, eidx - sidx);

                    int items_length = items.Count;
                    int sidx2 = 0;
                    int eidx2 = 0;
                    int increasement = 0;
                    for (int j=0;j< items_length; j++)
                    {
                        if (items[j].form_element_type.HasValue && items[j].form_element_type.Value == (int)FormElementType.Title || items[j].source == 6 || items[j].source == 7)
                        {
                            if(items[j].children ==null)
                                items[j].children = new List<FormItemData>();
                            sidx2 = j + 1;
                            var resList2 = items.GetRange(sidx2, items_length - sidx2);
                            int resIdx2 = -1;
                            if (items[j].form_element_type.HasValue && items[j].form_element_type.Value == (int)FormElementType.Title || items[j].source == 7 )
                            {
                                resIdx2 = resList2.FindIndex(x => x.form_element_type.HasValue && (x.form_element_type.Value == (int)FormElementType.Title || x.source == 7));
                            }
                            //else if (items[j].source == 7)
                            //{
                            //    resIdx2 = resList2.FindIndex(x => x.source == 7);
                            //}
                            else
                            {
                                resIdx2 = resList2.FindIndex(x => x.source == 6 );
                            }
                            eidx2 = resIdx2 == -1 ? items_length : (sidx2 + resIdx2);

                            var newChilds = items.GetRange(sidx2, eidx2 - sidx2);
                            items[j].children.AddRange(newChilds);
                            items.RemoveRange(sidx2, eidx2 - sidx2);
                            items_length -= newChilds.Count;
                            sidx2 = eidx2;
                          
                        }
                        //else
                        //{
                        //    items[j].children.AddRange(items.GetRange(j, 1));
                        //}
                    }

                    res.Add(new FormGroupData(form_items[i], items));
                    sidx = eidx;
                    i = eidx - 1;
                }
                else
                {
                    res.Add(new FormGroupData(form_items[i], form_items.GetRange(i, 1)));
                }
            }

            FormData form_Data = new FormData() { form_details = form_details, activity_details = activity_details, form_items = res };

            

            return form_Data;
        }

        public async Task<bool> SaveFormScore(FormData form_data)
        {
            bool is_new;
            Score score = null;

            //loop on all form items
            foreach (FormGroupData form_group in form_data.form_items)
            {
                if (form_group.form_element_type == null)
                {
                    //var item
                    FormItemData item_details = Mapper.Map<FormItemData>(form_group);
                    await SaveItemScore(form_data, item_details);
                }
                foreach (FormItemData form_item in form_group.items)
                {
                    await  SaveItemScore(form_data, form_item);
                    if (form_item.children != null)
                    {
                        foreach (FormItemData child in form_item.children)
                        {
                            await SaveItemScore(form_data, child);
                        }
                    }
                   
                    //is_new = false;

                        //score = DbContext.Score.Where(s => ((s.ModelComponentGuid != null && s.ModelComponentGuid == form_item.model_component_guid) ||
                        //                         (s.FormElementGuid != null && s.FormElementGuid == form_item.form_element_guid)) &&
                        //                          s.OrgObjGuid == form_data.form_details.org_obj_guid &&
                        //                          s.ActivityGuid == form_data.activity_details.activity_guid &&
                        //                          s.FormGuid == form_data.form_details.form_guid).FirstOrDefault();
                        //if (score == null)
                        //{
                        //    is_new = true;
                        //    score = new Score
                        //    {
                        //        OrgObjGuid = form_data.form_details.org_obj_guid,
                        //        ActivityGuid = form_data.activity_details.activity_guid,
                        //        FormGuid = form_data.form_details.form_guid,
                        //        ModelComponentGuid = form_item.model_component_guid,
                        //        FormElementGuid = form_item.form_element_guid
                        //    };
                        //}

                        //score.Status = form_data.form_details.status;
                        //score.ModelComponentComment = form_item.comment;
                        //score.ModelComponentGuid = form_item.model_component_guid;
                        //score.FormElementGuid = form_item.form_element_guid;

                        //if (form_item.metric_form_irrelevant.HasValue && form_item.metric_form_irrelevant.Value)
                        //{
                        //    score.OriginalScore = -2;
                        //    score.ConvertionScore = null;
                        //}
                        //else if (form_item.score == -1 && form_item.metric_measuring_unit == null)//model
                        //{
                        //    score.OriginalScore = null;
                        //    score.ConvertionScore = null;
                        //}
                        //else if (form_item.score == null && form_item.metric_measuring_unit != null)
                        //{
                        //    score.OriginalScore = -1;
                        //    score.ConvertionScore = null;
                        //}
                        //else
                        //{

                        //    var convertion_table = await _modelService.GetConvertionDetails(form_item.model_component_guid);

                        //    score.OriginalScore = form_item.score;
                        //    score.ConvertionScore = convertion_table
                        //            .Where(ct => ct.start_range <= score.OriginalScore && ct.end_range >= score.OriginalScore)
                        //            .Select(ct => ct.conversion_table_final_score).FirstOrDefault();
                        //}

                        //if (is_new)
                        //{
                        //    await DbContext.Score.AddAsync(score);
                        //}
                        //else
                        //{
                        //    DbContext.Score.Update(score);
                        //}
                        //await DbContext.SaveChangesAsync();
                }
            }

            //update form status
            Form form = DbContext.Form.Where(f => f.FormGuid == form_data.form_details.form_guid).FirstOrDefault();
            if (form != null && form.Status != form_data.form_details.status)
            {
                form.Status = form_data.form_details.status;
                
            }
            if (form_data.form_details.status == 3)
            {
                form.ApproveUserGuid = form_data.form_details.approve_user_guid;
                form.ApproveDate = Util.ConvertDateToString(DateTime.Now);
            }
            DbContext.Form.Update(form);
            var saveChangesCount = await DbContext.SaveChangesAsync();
            return saveChangesCount > 0;
        }

        private async Task<bool> SaveItemScore(FormData form_data,FormItemData form_item)
        {

            bool is_new;
            Score score = null;
            is_new = false;

            score = DbContext.Score.Where(s => ((s.ModelComponentGuid != null && s.ModelComponentGuid == form_item.model_component_guid) ||
                                     (s.FormElementGuid != null && s.FormElementGuid == form_item.form_element_guid)) &&
                                      s.EntityGuid == form_data.form_details.entity_guid &&
                                      s.ActivityGuid == form_data.activity_details.activity_guid &&
                                      s.FormGuid == form_data.form_details.form_guid).FirstOrDefault();
            if (score == null)
            {
                is_new = true;
                score = new Score
                {
                    EntityGuid = form_data.form_details.entity_guid,
                    EntityType = form_data.form_details.entity_type,
                    ActivityGuid = form_data.activity_details.activity_guid,
                    FormGuid = form_data.form_details.form_guid,
                    ModelComponentGuid = form_item.model_component_guid,
                    FormElementGuid = form_item.form_element_guid
                };
            }

            score.Status = form_data.form_details.status;
            score.ModelComponentComment = form_item.comment;
            score.ModelComponentGuid = form_item.model_component_guid;
            score.FormElementGuid = form_item.form_element_guid;

            if (form_item.metric_form_irrelevant.HasValue && form_item.metric_form_irrelevant.Value)
            {
                score.OriginalScore = -2;
                score.ConvertionScore = null;
            }
            else if (form_item.score == -1 && form_item.metric_measuring_unit == null)//model
            {
                score.OriginalScore = null;
                score.ConvertionScore = null;
            }
            else if (form_item.score == null && form_item.metric_measuring_unit != null)
            {
                score.OriginalScore = -1;
                score.ConvertionScore = null;
            }
            else
            {

                var convertion_table = await _modelService.GetConvertionDetails(form_item.model_component_guid);

                score.OriginalScore = form_item.score;
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
            return true;
        }
        public async Task CreateForm(Activity activity, string form_template_guid,string user = null)
        {
            bool existingActivityForm = DbContext.Form.Any(f => f.FormTemplateGuid == form_template_guid && f.ActivityGuid == activity.ActivityGuid &&  f.UserInCourse == user );

            if (existingActivityForm)
                return;
           
            Form form = new Form
            {
                FormGuid = Util.CreateGuid().ToString(),
                FormTemplateGuid = form_template_guid,
                ActivityGuid = activity.ActivityGuid,
                Status = (int)FormStatus.empty,
                //OrgObjGuid = activity.OrgObjGuid,TODO
                UserInCourse = user,

            };

            await DbContext.Form.AddAsync(form);
            await DbContext.SaveChangesAsync();
        }
    }
}
