using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Infrastructure;
using Infrastructure.Core;
using Infrastructure.Helpers;
using Infrastructure.Services;
using Model.Data;
using Model.Entities;

namespace Dal.Services
{
    public class GeneralService : BaseService
    {
        #region Description

        public async Task<List<DescriptionsData>> GetDiscription(int? descriptionGuid)
        {
            List<DescriptionsData> des_list;

            if (descriptionGuid == 0)
            {
                des_list = await DbContext.Description.Select(d => new DescriptionsData() { description_guid = d.DescriptionGuid, name = d.Name ,remark = d.Remark}).ToListAsync();
            }
            else
            {
                des_list = await DbContext.Description.Where(d => d.DescriptionGuid == descriptionGuid)
                                         .Select(d => new DescriptionsData() { description_guid = d.DescriptionGuid, name = d.Name, remark = d.Remark }).ToListAsync();
            }

            return des_list;
        }

        public async Task<bool> AddDescription(Description description)
        {
            if (description.DescriptionGuid != 0)
            {
                var oldRow = await DbContext.Description.Where(ed => ed.DescriptionGuid == description.DescriptionGuid).FirstOrDefaultAsync();
                if (oldRow != null)
                {
                    oldRow.Modify = DateTime.Now;
                    oldRow.Name = description.Name;
                    oldRow.Remark = description.Remark;
                    oldRow.TypeGuid = description.TypeGuid;
                    oldRow.ModifyUserGuid = description.ModifyUserGuid;
                }
               else
                    return false;
            }
            else
            {
                var nameExist = await DbContext.Description.Where(ed => ed.Name == description.Name).FirstOrDefaultAsync();
                if (nameExist != null)
                    return false;

                description.Creator = DateTime.Now;
                description.Modify = DateTime.Now;

                DbContext.Description.Add(description);
            }

            return true;
        }

        public async Task<bool> DeleteDescription(Description description)
        {
            bool result = false;
            if (description.DescriptionGuid != 0)
            {
                DbContext.Description.Remove(description);
                result = true;
            }

            return await Task.FromResult(result);
        }

        #endregion

        #region Get

        public async Task<List<MeasuringUnitData>> GetMeasuringUnit()
        {
            List<MeasuringUnitData> results = new List<MeasuringUnitData>();
            GeneralContext.Cache.TryGetValue(Constants.MeasuringUnitT, out results);

            if (results == null)
            {
                results = await DbContext.MeasuringUnit.Select(x => new MeasuringUnitData(x)).ToListAsync();
                GeneralContext.Cache.Set(Constants.MeasuringUnitT, results, GeneralContext.CacheEntryOptions);
            }

            return results;
        }

        public async Task<List<CalenderRollupData>> GetCalenderRollup()
        {
            List<CalenderRollupData> results = new List<CalenderRollupData>();
            GeneralContext.Cache.TryGetValue(Constants.CalenderRollupT, out results);

            if (results == null)
            {
                results = await DbContext.CalenderRollup.Select(x => new CalenderRollupData(x)).ToListAsync();
                GeneralContext.Cache.Set(Constants.CalenderRollupT, results, GeneralContext.CacheEntryOptions);
            }

            return results;
        }

        public async Task<List<RollupMethodInfo>> GetRollupMethod()
        {
            List<RollupMethodInfo> results = new List<RollupMethodInfo>();
            GeneralContext.Cache.TryGetValue(Constants.RollupMethodT, out results);

            if (results == null)
            {
                results = await DbContext.RollupMethod.Select(x => new RollupMethodInfo(x)).ToListAsync();
                GeneralContext.Cache.Set(Constants.RollupMethodT, results, GeneralContext.CacheEntryOptions);
            }

            return results;
        }

        #endregion Get

        #region Maps

        public async Task<bool> LinkOrg_Model_Polygon(OrgModelPolygon data)
        {
            string currentOrgGuid = string.Empty;

            if (data == null) return await Task.FromResult(false);

            var existRow = DbContext.OrgModelPolygon
                            .Where(o => data.UnitGuid == o.UnitGuid && data.ModelComponentGuid == o.ModelComponentGuid)
                            .FirstOrDefault();

            if (existRow != null)
            {
                DbContext.OrgModelPolygon.Remove(existRow);
            }

            if (data.PolygonGuid != null)
            {
                DbContext.OrgModelPolygon.Add(data);
            }

            await DbContext.SaveChangesAsync();
            return true;

        }

        public async Task<List<Gender>> GetGenderList()
        {
            List<Gender> genderList = await DbContext.Gender.ToListAsync();
            return genderList;
        }

        public async Task<List<Status>> GetStatusList()
        {
            List<Status> statusList = await DbContext.Status.ToListAsync();
            return statusList;
        }

        public async Task<OrgModelPolygon> GetOrg_Model_Polygon(string _model_guid, string _org_obj_guid)
        {
            OrgModelPolygon Omp = await DbContext.OrgModelPolygon
                   .Where(o => _org_obj_guid == o.UnitGuid && _model_guid == o.ModelComponentGuid)
                   .FirstOrDefaultAsync();

            return Omp;
        }

        #endregion

        #region Activities

        public async Task<List<ActivityTemplateDataInfo>> Get_Org_Obj_Activity_Templates(string org_obj_guid)
        {
            List<ActivityTemplateDataInfo> activity_templates = await (from ad in DbContext.ActivityTemplateDescription
                                                                       join ed in DbContext.EntityDescription
                                                                       on ad.DescriptionGuid equals ed.DescriptionGuid
                                                                       where ed.EntityGuid == org_obj_guid && ad.ActivityTemplateGuid != null
                                                                       select new ActivityTemplateDataInfo(ad.ActivityTemplateGu)).ToListAsync();
             


            return activity_templates;
        }

        public async Task<List<ActivityDetails>> Get_Org_Obj_Activities(string org_obj_guid)
        {
            List<string> unitActivities = DbContext.ActivityEntity.Where(ae => ae.EntityGuid == org_obj_guid).Select(ae => ae.ActivityGuid).ToList();
            List<ActivityDetails> activities = await (from a in DbContext.Activity
                                                      where org_obj_guid != null ? (unitActivities.Contains(a.ActivityGuid)) : true
                                                      select new ActivityDetails
                                                      {
                                                          activity_guid = a.ActivityGuid,
                                                          name = a.Name,
                                                          description = a.Description,
                                                          start_date = Util.ConvertStringToDate(a.StartDate),
                                                          end_date = Util.ConvertStringToDate(a.EndDate),
                                                          activity_template = new ActivityTemplateDataInfo(a.ActivityTemplateGu),
                                                          anonymousEvaluation = a.AnonymousEvaluation,
                                                          users = a.Users
                                                      }).ToListAsync();


            return activities;
        }
        public async Task<List<ActivityDetails>> Get_Org_Obj_Activities_For_Filler(string org_obj_guid)
        {
            List<ActivityDetails> activities = await (from a in DbContext.Activity
                                                      //join e in DbContext.EstimatedOrganizationObject
                                                      //on a.ActivityGuid equals e.ActivityGuid
                                                      //into es
                                                      //from e in es.DefaultIfEmpty()
                                                      //where e.OrgObjEstimatedGuid == org_obj_guid

                                                      //where a.EntityGuidType == (int)EntityTypeEnum.Unit ? a.EstimateUnits.Contains(org_obj_guid) : a.EstimatePersons.Contains(org_obj_guid)TODO
                                                      select new ActivityDetails
                                                      {
                                                          activity_guid = a.ActivityGuid,
                                                          name = a.Name,
                                                          description = a.Description,
                                                          start_date = Util.ConvertStringToDate(a.StartDate),
                                                          end_date = Util.ConvertStringToDate(a.EndDate),
                                                          activity_template = new ActivityTemplateDataInfo(a.ActivityTemplateGu),
                                                          has_files = DbContext.ActivityFile.Any(af => af.ActivityGuid == a.ActivityGuid),
                                                          //EstimatePersons = a.EstimatePersons,
                                                          //EstimateUnits = a.EstimateUnits,
                                                          //estimates_org_list = DbContext.EstimatedOrganizationObject.Where(eoo => eoo.ActivityGuid == a.ActivityGuid).Select(eoo => eoo.OrgObjEstimatedGu.Name).ToList(),
                                                          form_list = (from f in DbContext.Form
                                                                           //join u in DbContext.User on f.UserInCourse equals u.UserGuid into us
                                                                           //from u in us.DefaultIfEmpty()


                                                                           //join o in DbContext.OrganizationObject on a.OrgObjGuid equals o.OrgObjGuid 
                                                                       join un in DbContext.Unit on f.EntityGuid equals un.UnitGuid into units
                                                                       from uuu in units.DefaultIfEmpty()

                                                                       join pp in DbContext.Person on f.EntityGuid equals pp.PersonGuid into persons
                                                                       from ppp in persons.DefaultIfEmpty()

                                                                       where f.ActivityGu.ActivityTemplateGuid == a.ActivityTemplateGu.ActivityTemplateGuid &&
                                                                          f.ActivityGuid == a.ActivityGuid
                                                                       select new FormDetails(f, f.FormTemplateGu, ppp != null ? ppp.FirstName + " " + ppp.LastName : uuu != null ? uuu.UnitName : null)).ToList(),
                                                          users = a.Users

                                                      }).ToListAsync();



            return activities;
        }
        //public async Task<List<ActivityDetails>> Get_Org_Obj_Activities_For_Filler(string org_obj_guid)
        //{
        //    List<ActivityDetails> activities = await (from a in DbContext.Activity
        //                                              join e in DbContext.EstimatedOrganizationObject
        //                                              on a.ActivityGuid equals e.ActivityGuid
        //                                              into es
        //                                              from e in es.DefaultIfEmpty()
        //                                              //join o in DbContext.OrganizationObject on e.OrgObjEstimatedGuid equals o.OrgObjGuid
        //                                              //into oo
        //                                              //from o in oo.DefaultIfEmpty()
        //                                              //where e.OrgObjEstimatedGuid == org_obj_guid
        //                                              group a.ActivityGroupGuid by new { a.OrgObjGuid, a.ActivityGuid, a.Name, a.Description, a.StartDate, a.EndDate, a.ActivityTemplateGu, a.Users }
        //                                              into grp
        //                                              //group o by o.OrgObjGuid into grp
        //                                              //select grp);

        //                                              select new ActivityDetails
        //                                              {
        //                                                  activity_guid = grp.Key.ActivityGuid,
        //                                                  name = grp.Key.Name,
        //                                                  description = grp.Key.Description,
        //                                                  start_date = Util.ConvertStringToDate(grp.Key.StartDate),
        //                                                  end_date = Util.ConvertStringToDate(grp.Key.EndDate),
        //                                                  activity_template = new ActivityTemplateDataInfo(grp.Key.ActivityTemplateGu),
        //                                                  has_files = DbContext.ActivityFile.Any(af => af.ActivityGuid == grp.Key.ActivityGuid),
        //                                                  estimates_org_list = DbContext.EstimatedOrganizationObject.Where(eoo => eoo.ActivityGuid == grp.Key.ActivityGuid).Select(eoo => eoo.OrgObjEstimatedGu.Name).ToList(),
        //                                                  form_list = (from f in DbContext.Form



        //                                                               join o in DbContext.OrganizationObject on grp.Key.OrgObjGuid equals o.OrgObjGuid
        //                                                               where f.ActivityGu.ActivityTemplateGuid == grp.Key.ActivityTemplateGu.ActivityTemplateGuid &&
        //                                                                  f.ActivityGuid == grp.Key.ActivityGuid
        //                                                               select new FormDetails(f, f.FormTemplateGu, o)).ToList(),
        //                                                  users = grp.Key.Users

        //                                                  //activity_guid = a.ActivityGuid,
        //                                                  //name = a.Name,
        //                                                  //description = a.Description,
        //                                                  //start_date = Util.ConvertStringToDate(a.StartDate),
        //                                                  //end_date = Util.ConvertStringToDate(a.EndDate),
        //                                                  //activity_template = new ActivityTemplateDataInfo(a.ActivityTemplateGu),
        //                                                  //has_files = DbContext.ActivityFile.Any(af => af.ActivityGuid == a.ActivityGuid),
        //                                                  //estimates_org_list = DbContext.EstimatedOrganizationObject.Where(eoo => eoo.ActivityGuid == a.ActivityGuid).Select(eoo => eoo.OrgObjEstimatedGu.Name).ToList(),
        //                                                  //form_list = (from f in DbContext.Form
        //                                                  //                 //join u in DbContext.User on f.UserInCourse equals u.UserGuid into us
        //                                                  //                 //from u in us.DefaultIfEmpty()


        //                                                  //             join o in DbContext.OrganizationObject on a.OrgObjGuid equals o.OrgObjGuid
        //                                                  //             where f.ActivityGu.ActivityTemplateGuid == a.ActivityTemplateGu.ActivityTemplateGuid &&
        //                                                  //                f.ActivityGuid == a.ActivityGuid
        //                                                  //             select new FormDetails(f, f.FormTemplateGu, o)).ToList(),
        //                                                  //users = a.Users

        //                                              }).ToListAsync();



        //    return activities;
        //}
        //public async Task<List<FormData>> Get_Forms_Data_For_Filler(string org_obj_guid)
        //{
        //    List<FormData> forms = await (
        //                                   //from f in DbContext.Form
        //                                   //join a in DbContext.Activity
        //                                   //on f.ActivityGuid equals a.ActivityGuid
        //                                   ////where f.OrgObjGuid == org_obj_guid
        //                                   //join e in DbContext.EstimatedOrganizationObject
        //                                   //on a.ActivityGuid equals e.ActivityGuid into es
        //                                   //from e in es.DefaultIfEmpty()
        //                                   //where e.OrgObjEstimatedGuid == org_obj_guid
        //                                   //&& a.OrgObjGuid == org_obj_guid
        //                                  from f in DbContext.Form
        //                                  join a in DbContext.Activity
        //                                  on f.ActivityGuid equals a.ActivityGuid
        //                                  where a.UnitGuid == org_obj_guid
        //                                  //join e in DbContext.EstimatedOrganizationObject
        //                                  //on a.ActivityGuid equals e.ActivityGuid into es
        //                                  //from e in es.DefaultIfEmpty()
        //                                  //where e.OrgObjEstimatedGuid == org_obj_guid


        //                                  select new FormData
        //                                  {
        //                                        form_details = new FormDetails(f,f.FormTemplateGu, f.OrgObjGu, null),
        //                                        activity_details = new ActivityDetails(a,a.ActivityTemplateGu)
        //                                  }
        //        ).ToListAsync();



        //    return forms;
        //}
        #endregion Activities
    }
}
