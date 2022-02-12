using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Model.Data;
using Model.Entities;
using Microsoft.AspNetCore.Http;

namespace FillerDal.Services
{
    public class ActivityService : BaseService
    {
        private FormService _formService => this.GetChildService<FormService>();

        public async Task<List<MultiActivityData>> GetMultiActivities()
        {
            var activities = await (from a in DbContext.MultiActivity
                                    join at in DbContext.ActivityTemplate on a.ActivityTemplateGuid equals at.ActivityTemplateGuid
                                    //orderby a.Name
                                    select new MultiActivityData(a, at)).ToListAsync();

            return activities;
        }
        
         public async Task<List<MultiActivityData>> GetUnitsMultiActivities(string orgObjGuid)
        {
            var activities = await (from a in DbContext.MultiActivity
                                    join at in DbContext.ActivityTemplate on a.ActivityTemplateGuid equals at.ActivityTemplateGuid
                                    where a.EstimatesGuids.Contains(orgObjGuid)
            //orderby a.Name
            select new MultiActivityData(a, at)).ToListAsync();

            return activities;
        }
        public async Task<List<ActivityDetails>> GetMultiActivityItems(string activity_group_guid)
        {
            var activities = await DbContext.Activity
                    .Where(a => a.ActivityGroupGuid == activity_group_guid)
                    //.OrderBy(x=>x.Name)
                    .Select(a => new ActivityDetails(
                        a,
                        a.ActivityTemplateGu
                        //a.EstimatedOrganizationObject.Select(e => e.OrgObjEstimatedGu.OrgObjGuid + ";" + e.OrgObjEstimatedGu.Name).ToList()
                        ))
                    .ToListAsync();

            return activities;
        }

        public async Task<ActivityDetails> GetActivity(string activity_guid)
        {
            var activity = await DbContext.Activity
                    .Where(a => a.ActivityGuid == activity_guid)
                    .Select(a => new ActivityDetails(
                        a,
                        a.ActivityTemplateGu
                        //a.EstimatedOrganizationObject.Select(e => e.OrgObjEstimatedGuid).ToList()
                        
                    )).FirstOrDefaultAsync();

            return activity;
        }

        public async Task<string> SaveMultiActivity(MultiActivity data)
        {
            var existing = DbContext.MultiActivity.Find(data.ActivityGroupGuid);
            if (existing != null)
            {
                existing.Name = data.Name;
                existing.Description = data.Description;
                existing.ActivityTemplateGuid = data.ActivityTemplateGuid;
                existing.StartDate = data.StartDate;
                existing.EndDate = data.EndDate;
                existing.EstimatesGuids = data.EstimatesGuids;
                existing.EstimatesNames = data.EstimatesNames;
                existing.OrgObjGuidList = data.OrgObjGuidList;

                DbContext.MultiActivity.Update(existing);
            }
            else
            {
                DbContext.MultiActivity.Add(data);
            }
            DbContext.SaveChanges();
            return data.ActivityGroupGuid;
        }

        public async Task<bool> DeleteMultiActivityData(string activity_group_guid)
        {
            var exist = DbContext.MultiActivity.Find(activity_group_guid);
            DbContext.Remove(exist);
            DbContext.SaveChanges();
            return true;
        }

        public async Task<string> SaveActivity(string unit_guid, int unitType, ActivityDetails activity_data)
        {
            string guid;

            if (activity_data.activity_guid == null)
                guid = await CreateActivity(unit_guid, unitType, activity_data);
            else
                guid = await UpdateActivity(unit_guid, unitType, activity_data); 
               

            return guid;
        }

        public async Task<string> CreateActivity(string unit_guid,int unitType, ActivityDetails activity_data)
        {
            string guid = string.Empty;

            //save activity
            Activity activity = new Activity
            {
                ActivityGuid = Util.CreateGuid(),
                ActivityGroupGuid = activity_data.activity_group_guid,
                CreateDate = Util.ConvertDateToString(DateTime.Now),
                //OrgObjGuid = org_obj_guid,
                UnitGuid = unit_guid,
                UnitType = unitType,
                Name = activity_data.name,
                Description = activity_data.description,
                Users = activity_data.users,
                EstimatePersons = activity_data.EstimatePersons,
                EstimateUnits = activity_data.EstimateUnits,    

            };

            DateTime sd = new DateTime(activity_data.start_date.Year, activity_data.start_date.Month, activity_data.start_date.Day, 0, 0, 0);
            activity.StartDate = Util.ConvertDateToString(sd);
            DateTime ed = new DateTime(activity_data.end_date.Year, activity_data.end_date.Month, activity_data.end_date.Day, 0, 0, 0);
            activity.EndDate = Util.ConvertDateToString(ed);
            activity.ActivityTemplateGuid = activity_data.activity_template.activity_template_guid;

            DbContext.Activity.Add(activity);
            await DbContext.SaveChangesAsync();

            ////save estimates organization connection
            //foreach (string estimates_org in activity_data.estimates_org_list)
            //{
            //    EstimatedOrganizationObject estimated_org_obj = new EstimatedOrganizationObject();
            //    //estimated_org_obj.OrgObjGuid = org_obj_guid;TODO
            //    estimated_org_obj.ActivityGuid = activity.ActivityGuid;
            //    estimated_org_obj.OrgObjEstimatedGuid = estimates_org;

            //    DbContext.EstimatedOrganizationObject.Add(estimated_org_obj);
            //}

            await DbContext.SaveChangesAsync();

            //create forms by form template
            List<string> form_template_list = DbContext.AtInFt
                .Where(atift => atift.ActivityTemplateGuid == activity.ActivityTemplateGuid)
                .Select(atift => atift.FormTemplateGuid).ToList();

            foreach (var form_template_guid in form_template_list)
            {
                if (activity.Users == null )
                {
                    await _formService.CreateForm(activity, form_template_guid);
                }
                else
                {
                    foreach (var user in activity.Users)
                    {
                        string userIncourse = user;
                        await _formService.CreateForm(activity, form_template_guid, userIncourse);
                    }
                }
               
            }

            guid = activity.ActivityGuid;
            return guid;
        }

        public async Task<string> UpdateActivity(string unit_guid, int unitType, ActivityDetails activity_data)
        {
            string guid = string.Empty;

            //update activity
            Activity activity = DbContext.Activity.Where(a => a.ActivityGuid == activity_data.activity_guid).FirstOrDefault();

            //activity.OrgObjGuid = org_obj_guid;
            activity.UnitGuid = unit_guid;  
            activity.UnitType = unitType;   
            activity.Name = activity_data.name;
            activity.Description = activity_data.description;
            DateTime sd = new DateTime(activity_data.start_date.Year, activity_data.start_date.Month, activity_data.start_date.Day, 0, 0, 0);
            activity.StartDate = Util.ConvertDateToString(sd);
            DateTime ed = new DateTime(activity_data.end_date.Year, activity_data.end_date.Month, activity_data.end_date.Day, 0, 0, 0);
            activity.EndDate = Util.ConvertDateToString(ed);
            activity.EstimateUnits = activity_data.EstimateUnits;
            activity.EstimatePersons = activity_data.EstimatePersons;   

            DbContext.Activity.Update(activity);
            DbContext.SaveChanges();

            //remove estimates organization connection
            //var estimated_orgs_list = DbContext.EstimatedOrganizationObject.Where(eoo => eoo.ActivityGuid == activity.ActivityGuid).ToList();
            //DbContext.EstimatedOrganizationObject.RemoveRange(estimated_orgs_list);
            //DbContext.SaveChanges();

            ////save estimates organization connection
            //foreach (string estimates_org in activity_data.estimates_org_list)
            //{
            //    EstimatedOrganizationObject estimated_org_obj = new EstimatedOrganizationObject();
            //    //estimated_org_obj.OrgObjGuid = org_obj_guid;
            //    estimated_org_obj.ActivityGuid = activity.ActivityGuid;
            //    estimated_org_obj.OrgObjEstimatedGuid = estimates_org;

            //    DbContext.EstimatedOrganizationObject.Add(estimated_org_obj);
            //}

            await DbContext.SaveChangesAsync();

            guid = activity.ActivityGuid;
            return guid;
        }

        public async Task<bool> DeleteItemFromMultiActivity(string orgObjGuid, string activityGroupGuid)
        {
            //var existActivity = DbContext.Activity.FirstOrDefault(a => a.OrgObjGuid == orgObjGuid && a.ActivityGroupGuid == activityGroupGuid);
            var existActivity = DbContext.Activity.FirstOrDefault(a => a.UnitGuid == orgObjGuid && a.ActivityGroupGuid == activityGroupGuid);
            if (existActivity != null)
            {
                await DeleteActivity(existActivity.ActivityGuid);
            }

            return true;
        }

        public async Task<bool> DeleteActivity(string activity_guid)
        {
            //get activity
            var activity = DbContext.Activity
               .Where(a => a.ActivityGuid == activity_guid).FirstOrDefault();

            //remove estimates organization connection
            //var estimatedOrgs = DbContext.EstimatedOrganizationObject.Where(eoo => eoo.ActivityGuid == activity_guid);
            //DbContext.EstimatedOrganizationObject.RemoveRange(estimatedOrgs);
            //await DbContext.SaveChangesAsync();

            //remove activity files
            var activityFiles = DbContext.ActivityFile.Where(eoo => eoo.ActivityGuid == activity_guid);
            DbContext.ActivityFile.RemoveRange(activityFiles);
            await DbContext.SaveChangesAsync();

            //remove scores
            var scores = DbContext.Score.Where(eoo => eoo.ActivityGuid == activity_guid);
            DbContext.Score.RemoveRange(scores);
            await DbContext.SaveChangesAsync();

            //remove calculated scores
            var calcScores = DbContext.CalculateScore.Where(eoo => eoo.ActivityGuid == activity_guid);
            DbContext.CalculateScore.RemoveRange(calcScores);
            await DbContext.SaveChangesAsync();

            //remove forms
            var forms = DbContext.Form.Where(f => f.ActivityGuid == activity_guid).ToList();
            if (forms.Any())
                await DeleteForms(forms);

            //remove activity
            if (activity != null)
                DbContext.Activity.Remove(activity);

            var saveChangesCount = await DbContext.SaveChangesAsync();
            return saveChangesCount > 0;
        }

        public async Task<bool> DeleteMultiActivity(string org_obj_guid, string activity_group_guid)
        {
            bool result = true;

            //var activity = DbContext.Activity.FirstOrDefault(a => a.ActivityGroupGuid == activity_group_guid && a.OrgObjGuid == org_obj_guid);TODO
            var activity = DbContext.Activity.FirstOrDefault(a => a.ActivityGroupGuid == activity_group_guid && a.UnitGuid == org_obj_guid);
            if (activity != null && activity.ActivityGuid != null)
            {
                result = await DeleteActivity(activity.ActivityGuid);
            }

            return result;
        }

        public async Task DeleteForms(List<Form> forms)
        {
            //loop all forms and get scores
            foreach (var form in forms)
            {
                List<CalculateScore> calculated_scores = DbContext.CalculateScore.Where(s => s.FormGuid == form.FormGuid).ToList();
                DbContext.CalculateScore.RemoveRange(calculated_scores);

                List<Score> scores = DbContext.Score.Where(s => s.FormGuid == form.FormGuid).ToList();
                DbContext.Score.RemoveRange(scores);
            }

            //remove forms
            DbContext.Form.RemoveRange(forms);
            await DbContext.SaveChangesAsync();
        }

        public async Task<string> SaveActivityFile(string activityGuid, string fileName, string content)
        {
            ActivityFile activityFile = new ActivityFile();
            activityFile.ActivityFileGuid = Util.CreateGuid();
            activityFile.ActivityGuid = activityGuid;
            activityFile.FileName = fileName;
            activityFile.Content = content;

            DbContext.ActivityFile.Add(activityFile);
            await DbContext.SaveChangesAsync();

            return activityFile.ActivityFileGuid;
        }

        public async Task<List<ActivityFile>> GetActivityFiles(string activityGuid)
        {
            var result = DbContext.ActivityFile.Where(af => af.ActivityGuid == activityGuid).ToList();
            return result;
        }

        public async Task<bool> DeleteActivityFile(string[] activityFileGuids)
        {
            ActivityFile activityFile = null;
            foreach (var afg in activityFileGuids)
            {
                activityFile = DbContext.ActivityFile.Find(afg);
                DbContext.ActivityFile.Remove(activityFile);
            }

            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<PersonalUnit>> GetPersonalUnit(string orgObjGuid)
        {
            var result = DbContext.PersonalUnit.Where(af => af.OrgObjGuid == orgObjGuid).ToList();
            return result;
        }
    }
}

