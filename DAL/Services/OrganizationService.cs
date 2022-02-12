using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Core;
using Infrastructure.Helpers;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Services;
using Model.Data;
using Model.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Dal.Services
{
    public class OrganizationService : BaseService
    {
        private ActivityService _activityService => this.GetChildService<ActivityService>();
        private ReportService _reportService => this.GetChildService<ReportService>();
        private ModelService _modelService => this.GetChildService<ModelService>();
        private UserService _userService => this.GetChildService<UserService>();

        private readonly ICacheService _cacheService = GeneralContext.GetService<ICacheService>();

        private readonly IAppConfig _config = GeneralContext.GetConfig<IAppConfig>();

      

        //public async Task<OrgObjTree> GetOrganizationTree(OrgObjTree[] flatTree = null, OrgObjTree tree = null, string parent_guid = null)
        //{
        //    try
        //    {
        //        if (parent_guid == null)
        //        {
        //            flatTree = await GetOrganizationFlatlList(new string[] { null });
        //            tree = flatTree.FirstOrDefault(x => x.parent_guid == parent_guid);
        //        }
        //        if (tree == null)
        //        {
        //            return tree;
        //        }
        //        OrgObjTree[] children = flatTree.Where(x => x.parent_guid == tree.guid).ToArray();

        //        Parallel.ForEach(children, async child =>
        //        {
        //            await GetOrganizationTree(flatTree, child, tree.guid);
        //        });

        //        tree.children = children.ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
        //    }

        //    _cacheService.RemoveKey("/api/dalapi/organization/updateorganizationtreeconnections$orgobjtree");
        //    return tree;
        //}

        
        public async Task<List<UnitData>> GetUnit(string unitGuid)
        {
            try
            {

                //var x = from u in DbContext.Unit
                //        join o in DbContext.o
                //UnitData result = await DbContext.Unit.Where(x => x.UnitGuid == unitGuid).FirstOrDefaultAsync();
                List<UnitData> result = await (from u in DbContext.Unit

                                         join up in DbContext.Unit on u.ParentUnitGuid equals up.UnitGuid into uparent
                                         from upa in uparent.DefaultIfEmpty()

                                         join p in DbContext.Person on u.ManagerUnitGuid equals p.PersonGuid into mngr  
                                         from mp in mngr.DefaultIfEmpty()

                                         join m in DbContext.ModelComponent on u.DefaultModelGuid equals m.ModelComponentGuid into mco 
                                         from mc in mco.DefaultIfEmpty()


                                         where unitGuid != null ? u.UnitGuid == unitGuid : true

                                         select new UnitData
                                         {
                                            SerialNum = u.SerialNum,
                                            UnitGuid = u.UnitGuid,
                                            UnitName = u.UnitName,
                                            Order = u.Order,    
                                            ParentUnitGuid = u.ParentUnitGuid,
                                            ParentUnitName = upa.UnitName,
                                            ManagerUnitPersonGuid = u.ManagerUnitGuid,
                                            ManagerUnitPersonName = mp != null ? mp.FirstName + " " + mp.LastName : null,
                                            DefaultModelGuid = u.DefaultModelGuid,
                                            DefaultModelName = mc !=null ? mc.Name : null,

                                             DescriptionsData = (from ed in DbContext.EntityDescription
                                                                 join d in DbContext.Description on ed.DescriptionGuid equals d.DescriptionGuid
                                                                where  ed.EntityGuid == u.UnitGuid
                                                                 select new DescriptionsData
                                                             {
                                                                 description_guid = d.DescriptionGuid,
                                                                 name = d.Name,
                                                             }).ToArray()

                                         }                                            
                                         ).OrderBy(u => u.SerialNum).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
                return null;
            }          
        }

        public async Task<List<UnitData>> GetUnitByAT(string activityTemplateGuid)
        {
            var atDescriptions = DbContext.ActivityTemplateDescription.Where(atd => atd.ActivityTemplateGuid == activityTemplateGuid).Select(atd => atd.DescriptionGuid).ToList();
            List<UnitData> result =  await (from ed in DbContext.EntityDescription
                                     join u in DbContext.Unit on ed.EntityGuid equals u.UnitGuid
                                     where atDescriptions.Contains( ed.DescriptionGuid)
                                     select new UnitData()
                {
                    SerialNum = u.SerialNum,
                    UnitGuid = u.UnitGuid,
                    UnitName = u.UnitName,
                    Order = u.Order,
                    ParentUnitGuid = u.ParentUnitGuid,
                    ManagerUnitPersonGuid = u.ManagerUnitGuid,
                    DefaultModelGuid = u.DefaultModelGuid
                }).Distinct().OrderBy(u=>u.SerialNum).ToListAsync();

            return result;

        }

        public async Task<List<PersonData>> GetUnitPeoples(string unitGuid)
        {
            List<PersonData> result = await (from p in DbContext.Person
                                             where p.UnitGuid == unitGuid
                                             select new PersonData()
                                             {
                                                PersonGuid = p.PersonGuid,
                                                FirstName = p.FirstName,
                                                LastName = p.LastName,
                                                DirectManagerGuid = p.DirectManagerGuid,
                                                DirectManagerName = p.DirectManagerGu.FirstName + " " + p.DirectManagerGu.LastName,
                                                professionalManagerGuid = p.professionalManagerGuid,
                                                professionalManagerName = p.professionalManagerGu.FirstName + " " + p.professionalManagerGu.LastName,
                                             }).ToListAsync();

            return result;

        }

        //public async Task<List<UnitData>> GetUnitsList()
        //{
        //    try
        //    {
        //        List<UnitData> result  = await (from u in DbContext.Unit


        //                                                        join up in DbContext.Unit on u.ParentUnitGuid equals up.UnitGuid into uparent
        //                                                        from upa in uparent.DefaultIfEmpty()

        //                                                        join p in DbContext.Person on u.ManagerUnitGuid equals p.PersonGuid into mngr
        //                                                        from mp in mngr.DefaultIfEmpty()

        //                                                        join m in DbContext.ModelComponent on u.DefaultModelGuid equals m.ModelComponentGuid into mco
        //                                                        from mc in mco.DefaultIfEmpty()


        //                                                        select new UnitData
        //                                                        {
        //                                                            UnitGuid = u.UnitGuid,
        //                                                            UnitName = u.UnitName,
        //                                                            Order = u.Order,
        //                                                            ParentUnitGuid = u.ParentUnitGuid,
        //                                                            ParentUnitName = upa.UnitName,
        //                                                            ManagerUnitPersonName = mp != null ? mp.FirstName + " " + mp.LastName : null,
        //                                                            DefaultModelGuid = u.DefaultModelGuid,
        //                                                            DefaultModelName = mc != null ? mc.Name : null,
        //                                                            Descriptions = (from d in DbContext.Description
        //                                                                            where u.DescriptionsGuid.Contains(d.DescriptionGuid)
        //                                                                            select new DescriptionsData
        //                                                                            {
        //                                                                                description_guid = d.DescriptionGuid,
        //                                                                                name = d.Name,
        //                                                                            }).ToArray()

        //                                                        }
        //                                 ).ToListAsync();
        //        return result;


        //    }
        //    catch (Exception ex)
        //    {
        //        Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
        //        return null;
        //    }
        //}

        public async Task<List<PersonData>> GetPerson(string personGuid)
        {
            try
            {
                //UnitData result = await DbContext.Unit.Where(x => x.UnitGuid == unitGuid).FirstOrDefaultAsync();
                List<PersonData> result = await (from p in DbContext.Person

                                           join p1 in DbContext.Person on p.DirectManagerGuid equals p1.PersonGuid into directorManager
                                           from dm in directorManager.DefaultIfEmpty()

                                           join p2 in DbContext.Person on p.professionalManagerGuid equals p2.PersonGuid into professionalManager
                                           from pm in professionalManager.DefaultIfEmpty()

                                           join u in DbContext.Unit on p.UnitGuid equals u.UnitGuid into units
                                           from un in units.DefaultIfEmpty()

                                          join mun in DbContext.Unit on p.PersonGuid equals mun.ManagerUnitGuid into munits
                                          from mu in munits.DefaultIfEmpty()

                                           join m in DbContext.ModelComponent on p.JobtitleGuid equals m.ModelComponentGuid into jtitles
                                           from jt in jtitles.DefaultIfEmpty()

                                           join s in DbContext.Status on p.Status equals s.StatusId into statuses
                                           from st in statuses.DefaultIfEmpty()

                                           join g in DbContext.Gender on p.Gender equals g.GenderId into genderes
                                           from ge in genderes.DefaultIfEmpty()

                                           where personGuid != null ? p.PersonGuid == personGuid : true


                                           select new PersonData
                                           {
                                               PersonGuid = p.PersonGuid,
                                               FirstName = p.FirstName,
                                               LastName = p.LastName,
                                               Id = p.Id,
                                               PersonNumber = p.PersonNumber,
                                               UnitGuid = p.UnitGuid,
                                               UnitName = un.UnitName,
                                               DirectManagerGuid = p.DirectManagerGuid,
                                               DirectManagerName = dm.FirstName + " " + dm.LastName,
                                               professionalManagerGuid = p.professionalManagerGuid,
                                               professionalManagerName = pm.FirstName + " " + pm.LastName,
                                               JobtitleGuid = p.JobtitleGuid,
                                               JobtitleName = jt.Name,
                                               Gender = p.Gender,
                                               GenderName = ge.GenderName,
                                               BeginningOfWork = p.BeginningOfWork, 
                                               Email1 = p.Email1,
                                               Email2 = p.Email2,   
                                               Phone1 = p.Phone1,
                                               Phone2 = p.Phone2,
                                               Street = p.Street,
                                               City = p.City,
                                               ManagedUnitGuid = mu.UnitGuid,
                                               ManagedUnitName = mu.UnitName,
                                               State = p.State,
                                               //StateName TODO
                                               Country = p.Country,
                                               //CountryName TODO
                                               ZipCode = p.ZipCode, 
                                               DateOfBirth = p.DateOfBirth, 
                                               StatusGuid = p.Status,
                                               StatusName = st.StatusName,
                                               ChildrenNum = p.ChildrenNum,
                                               Degree = p.Degree,
                                               Institution = p.Institution, 
                                               Profession = p.Profession,
                                               Car = p.Car,
                                               Manufactor = p.Manufactor,
                                               PlateNum = p.PlateNum,
                                               EducationFund = p.EducationFund,
                                               LastSalaryUpdate = p.LastSalaryUpdate,
                                               Files = p.Files,
                                               Photo=p.Photo,
                                               //Descriptions = (from d in DbContext.Description
                                               //                where p.Descriptions.Contains(d.DescriptionGuid)
                                               //                select new DescriptionsData
                                               //                {
                                               //                    description_guid = d.DescriptionGuid,
                                               //                    name = d.Name,
                                               //                }).ToArray()
                                               DescriptionsData = (from ed in DbContext.EntityDescription
                                                                    join d in DbContext.Description on ed.DescriptionGuid equals d.DescriptionGuid
                                                               where p.PersonGuid == ed.EntityGuid
                                                               select new DescriptionsData
                                                               {
                                                                   description_guid = d.DescriptionGuid,
                                                                   name = d.Name,
                                                               }).ToArray()

                                           }
                                         ).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
                return null;
            }
        }
        //public async Task<OrgObjTree[]> GetOrganizationFlatlList(string[] orgChildGuids, OrgObjTree[] totalResults = null)
        //{
        //    totalResults = totalResults ?? new OrgObjTree[] { };

        //    OrgObjTree[] children = await (from os in DbContext.OrganizationStructure
        //           where orgChildGuids.Contains(os.OrgObjParentGuid)
        //           join oo in DbContext.OrganizationObject on os.OrgObjGuid equals oo.OrgObjGuid
        //           orderby oo.Order
        //           select new OrgObjTree()
        //           {
        //               guid = oo.OrgObjGuid,
        //               name = oo.Name,
        //               remark = oo.Remark,
        //               org_type = oo.OrgObjType,
        //               order = oo.Order,
        //               parent_guid = os.OrgObjParentGuid,
        //               permission_units = os.PermissionUnits,

        //           }).ToArrayAsync();

        //    if (children == null || !children.Any())
        //        return totalResults;

        //    totalResults = totalResults.Concat(children).ToArray();
        //    return await GetOrganizationFlatlList(children.Select(x => x.guid).ToArray(), totalResults);
        //}

        public async Task<EntityConnection> GetOrgObjConnection(string orgObjGuid)
        {
            EntityConnection connection = new EntityConnection() { entity_guid = orgObjGuid };
            //connection.activities = await DbContext.EntityActivityTemplate.Where(x => x.EntityGuid == orgObjGuid).Select(x => new ActivityTemplateDataInfo(x.ActivityTemplateGu)).ToArrayAsync();
            connection.descriptions = await DbContext.EntityDescription.Where(x => x.EntityGuid == orgObjGuid).Select(x => new DescriptionsData() { description_guid = x.DescriptionGu.DescriptionGuid, name = x.DescriptionGu.Name }).ToArrayAsync();

            return connection;
        }

        //public async Task<OrganizationObjectData> UpdateOrganizationTreeConnections(OrganizationObjectData tree = null, OrganizationObjectConnection[] allConnections = null)
        //{
        //    if (tree == null)
        //    {
        //        OrgObjTree tmpTree = null;
        //        allConnections = await DbContext.OrganizationObjectConnection.Include(a => a.ActivityTemplateGu).Include(a => a.ModelComponentGu).Include(a => a.DescriptionGu).ToArrayAsync();
        //        string _cacheKey = _cacheService.CreateCacheKey(this.HttpContext.Request.Path, "OrganizationObjectData");
        //        bool _cacheKeyExists = _cacheService.ContainsKey(_cacheKey);
        //        if (_cacheKeyExists)
        //        {
        //            var actionResult = await _cacheService.GetAsync<ApiCacheItem>(_cacheKey);
        //            tmpTree = (OrgObjTree)JsonConvert.DeserializeObject(actionResult.Data);
        //        }
        //        else
        //        {
        //            tmpTree = await GetOrganizationTree();
        //        }

        //        tree = Mapper.Map<OrganizationObjectData>(tmpTree);
        //        if (tree == null)
        //            return null;
        //    }

        //    Parallel.ForEach(tree.children, async child =>
        //    {
        //        await UpdateOrganizationTreeConnections(child, allConnections);

        //        child.activities = allConnections.Where(c => c.OrgObjGuid == child.guid && c.ActivityTemplateGuid != null).Select(c => new ActivityTemplateDataInfo(c.ActivityTemplateGu)).ToList();
        //        child.descriptions = allConnections.Where(c => c.OrgObjGuid == child.guid && c.DescriptionGuid != null).Select(c => new DescriptionsData() { description_guid = c.DescriptionGu.DescriptionGuid,/* type_guid = c.DescriptionGu.TypeGuid,*/ name = c.DescriptionGu.Name }).ToList();
        //        child.models = allConnections.Where(c => c.OrgObjGuid == child.guid && c.ModelComponentGuid != null).Select(c => new ModelData(c.ModelComponentGu)).ToList();
        //    });

        //    return tree;
        //}

        //public async Task<string> SaveOrganizationObject(string parent_guid, OrganizationObjectData organization_object)
        //{
        //    string guid = string.Empty;

        //    //if organization_object.org_obj_guid not exist
        //    if (organization_object != null && string.IsNullOrEmpty(organization_object.guid))
        //    {
        //        //create new organization_object
        //        guid = await CreateOrganizationObject(parent_guid, organization_object);
        //    }
        //    else
        //    {
        //        //update existing organization_object
        //        guid = await UpdateOrganizationObject(organization_object);
        //    }

        //    if (organization_object.children != null && organization_object.children.Count > 0)
        //    {
        //        foreach (var child in organization_object.children)
        //        {
        //            await SaveOrganizationObject(organization_object.guid, child);
        //        }
        //    }

        //    return guid;
        //}
        public async Task<string> SaveUnit(UnitData unit_object)
        {
            string guid = string.Empty;

            //if organization_object.org_obj_guid not exist
            if (unit_object != null && string.IsNullOrEmpty(unit_object.UnitGuid))
            {
                //create new organization_object
                guid = await CreateUnit(unit_object);
            }
            else
            {
                //update existing organization_object
                guid = await UpdateUnit(unit_object);
            }

            //if (organization_object.children != null && organization_object.children.Count > 0)
            //{
            //    foreach (var child in organization_object.children)
            //    {
            //        await SaveOrganizationObject(organization_object.guid, child);
            //    }
            //}

            return guid;
        }

        public async Task<string> CreateUnit(UnitData unit_object)
        {
            string guid = string.Empty;

            unit_object.UnitGuid = Util.CreateGuid().ToString();//Create new guid

            //Save to OrganizationObject table
            Unit unit_obj = new Unit();
            unit_obj.UnitGuid = unit_object.UnitGuid;
            unit_obj.UnitName = unit_object.UnitName;
            unit_obj.Order = DbContext.Unit.Where(os => os.ParentUnitGuid == unit_object.ParentUnitGuid).Count() + 1;
            unit_obj.ParentUnitGuid = unit_object.ParentUnitGuid;
            unit_obj.DefaultModelGuid = unit_object.DefaultModelGuid;
            DbContext.Unit.Add(unit_obj);
            await DbContext.SaveChangesAsync();
            await CreateEntityDescriptions(unit_object.UnitGuid, unit_object.DescriptionsGuids);
            guid = unit_object.UnitGuid;

            return guid;
        }
        public async Task<string> UpdateUnit(UnitData unit_object)
        {
            //Get Organization_Object row by org_guid
            var org_obj = DbContext.Unit.Where(o => o.UnitGuid == unit_object.UnitGuid).FirstOrDefault();

            if (org_obj != null)
            {
                //Update Organization_Object
                org_obj.UnitName = unit_object.UnitName;
                org_obj.UnitGuid = unit_object.UnitGuid;
                org_obj.UnitName = unit_object.UnitName;
                org_obj.Order = unit_object.Order;
                org_obj.ParentUnitGuid = unit_object.ParentUnitGuid;
                org_obj.ManagerUnitGuid = unit_object.ManagerUnitPersonGuid;    
                org_obj.DefaultModelGuid = unit_object.DefaultModelGuid;


       //org_obj.OrgObjType = organization_object.org_type ?? null;

                DbContext.Unit.Update(org_obj);
                await DbContext.SaveChangesAsync();

                //Delete all Organization_Object_Connection for org_obj_guid
                await DeleteEntityDescription(unit_object.UnitGuid);

                //Create Organization_Object_Connection
                await CreateEntityDescriptions(unit_object.UnitGuid , unit_object.DescriptionsGuids);

                string guid = unit_object.UnitGuid;
                return guid;
            }
            else
            {
                return null;
            }
        }
        public async Task<string> SavePerson(PersonData person)
        {
            try
            {
                string guid = string.Empty;

                if (person != null && string.IsNullOrEmpty(person.PersonGuid))
                {

                    guid = await CreatePesron(person);
                }
                else
                {

                    guid = await UpdatePerson(person);
                }

                return guid;
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
                return null;
            }
            
        }
        public async Task<string> CreatePesron(PersonData person)
        {
            try
            {
                int save_changes = 0;
                string guid = string.Empty;
                person.PersonGuid = Util.CreateGuid().ToString();//Create new guid

                Person p = new Person();
                p.PersonGuid = person.PersonGuid;
                p.FirstName = person.FirstName;
                p.LastName = person.LastName;
                p.Id = person.Id;
                p.PersonNumber = person.PersonNumber;
                p.UnitGuid = person.UnitGuid;
                p.DirectManagerGuid = person.DirectManagerGuid;
                p.professionalManagerGuid = person.professionalManagerGuid;
                p.JobtitleGuid = person.JobtitleGuid;
                p.Gender = person.Gender;
                p.BeginningOfWork = person.BeginningOfWork;
                p.Email1 = person.Email1;
                p.Email2 = person.Email2;
                p.Phone1 = person.Phone1;
                p.Phone2 = person.Phone2;
                p.Street = person.Street;
                p.City = person.City;
                p.State = person.State;
                p.Country = person.Country;
                p.ZipCode = person.ZipCode;
                p.DateOfBirth = person.DateOfBirth;
                p.Status = person.StatusGuid;
                p.ChildrenNum = person.ChildrenNum;
                p.Degree = person.Degree;
                p.Institution = person.Institution;
                p.Profession = person.Profession;
                p.Car = person.Car;
                p.Manufactor = person.Manufactor;
                p.PlateNum = person.PlateNum;
                p.EducationFund = person.EducationFund;
                p.LastSalaryUpdate = person.LastSalaryUpdate;
                p.Files = person.Files;
                p.Photo = person.Photo;
                await DbContext.Person.AddAsync(p);
                save_changes = await DbContext.SaveChangesAsync();
                if (save_changes > 0)
                {
                    await CreateEntityDescriptions(person.PersonGuid, person.DescriptionsGuids);
                    guid = person.PersonGuid;
                    return guid;
                }
                return null;

            }
            catch (Exception ex)
            {

                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
                return null;
            }
           
        }
        public async Task<string> UpdatePerson(PersonData person)
        {
            try
            {
                //Get Organization_Object row by org_guid
                var p = DbContext.Person.Where(p => p.PersonGuid == person.PersonGuid).FirstOrDefault();

                if (p != null)
                {
                    p.PersonGuid = person.PersonGuid;
                    p.FirstName = person.FirstName;
                    p.LastName = person.LastName;
                    p.Id = person.Id;
                    p.PersonNumber = person.PersonNumber;
                    p.UnitGuid = person.UnitGuid;
                    p.DirectManagerGuid = person.DirectManagerGuid;
                    p.professionalManagerGuid = person.professionalManagerGuid;
                    p.JobtitleGuid = person.JobtitleGuid;
                    p.Gender = person.Gender;
                    p.BeginningOfWork = person.BeginningOfWork;
                    p.Email1 = person.Email1;
                    p.Email2 = person.Email2;
                    p.Phone1 = person.Phone1;
                    p.Phone2 = person.Phone2;
                    p.Street = person.Street;
                    p.City = person.City;
                    p.State = person.State;
                    p.Country = person.Country;
                    p.ZipCode = person.ZipCode;
                    p.DateOfBirth = person.DateOfBirth;
                    p.Status = person.StatusGuid;
                    p.ChildrenNum = person.ChildrenNum;
                    p.Degree = person.Degree;
                    p.Institution = person.Institution;
                    p.Profession = person.Profession;
                    p.Car = person.Car;
                    p.Manufactor = person.Manufactor;
                    p.PlateNum = person.PlateNum;
                    p.EducationFund = person.EducationFund;
                    p.LastSalaryUpdate = person.LastSalaryUpdate;
                    p.Files = person.Files;
                    p.Photo = person.Photo;

                    DbContext.Person.Update(p);
                    await DbContext.SaveChangesAsync();

                    //Delete all Organization_Object_Connection for org_obj_guid
                    await DeleteEntityDescription(person.UnitGuid);

                    //Create Organization_Object_Connection
                    await CreateEntityDescriptions(person.PersonGuid, person.DescriptionsGuids);

                    string guid = person.PersonGuid;
                    return guid;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
                return null;
            }
            
        }
        //public async Task<string> CreateOrganizationObject(string parent_guid, OrganizationObjectData organization_object)
        //{
        //    string guid = string.Empty;

        //    string eventId = organization_object.guid;

        //    organization_object.guid = Util.CreateGuid().ToString();//Create new guid

        //    //Save to OrganizationObject table
        //    OrganizationObject org_obj = new OrganizationObject();
        //    org_obj.OrgObjGuid = organization_object.guid;
        //    org_obj.Name = organization_object.name;
        //    org_obj.Remark = organization_object.remark;
        //    org_obj.OrgObjType = organization_object.org_type ?? null;
        //    org_obj.Order = DbContext.OrganizationStructure.Where(os => os.OrgObjParentGuid == parent_guid).Count() + 1;

        //    DbContext.OrganizationObject.Add(org_obj);
        //    await DbContext.SaveChangesAsync();

        //    //Save to OrganizationStructure
        //    OrganizationStructure org_obj_structure = new OrganizationStructure();
        //    org_obj_structure.OrgObjGuid = organization_object.guid;
        //    org_obj_structure.OrgObjParentGuid = parent_guid;

        //    DbContext.OrganizationStructure.Add(org_obj_structure);
        //    await DbContext.SaveChangesAsync();

        //    //Save to Organization_Object_Connection
        //    await CreateOrganizationObjectConnection(organization_object);

        //    guid = organization_object.guid;
        //    organization_object.guid += "@" + eventId;

        //    return guid;
        //}

        //public async Task<string> UpdateOrganizationObject(OrganizationObjectData organization_object)
        //{
        //    //Get Organization_Object row by org_guid
        //    var org_obj = DbContext.OrganizationObject.Where(o => o.OrgObjGuid == organization_object.guid).FirstOrDefault();

        //    if (org_obj != null)
        //    {
        //        //Update Organization_Object
        //        org_obj.Name = organization_object.name;
        //        org_obj.Remark = organization_object.remark;
        //        org_obj.OrgObjType = organization_object.org_type ?? null;

        //        DbContext.OrganizationObject.Update(org_obj);
        //        await DbContext.SaveChangesAsync();

        //        //Delete all Organization_Object_Connection for org_obj_guid
        //        await DeleteOrganizationObjectConnection(organization_object.guid);

        //        //Create Organization_Object_Connection
        //        await CreateOrganizationObjectConnection(organization_object);

        //        string guid = organization_object.guid;
        //        return guid;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public async Task<bool> CreateEntityDescriptions(string guid , int[] descriptionsGuids)
        {
            
            if (descriptionsGuids != null)
            {
                foreach (var desc_guid in descriptionsGuids)
                {
                    EntityDescription ed = new EntityDescription();
                    ed.EntityGuid = guid;
                    ed.DescriptionGuid = desc_guid;
                    DbContext.EntityDescription.Add(ed);    
                }
                return await DbContext.SaveChangesAsync() > 0;  
            }
            return false;
        }
        //public async Task<bool> CreateOrganizationObjectConnection(OrganizationObjectData organization_object)
        //{
        //    int saveChangesCount = 0;
        //    OrganizationObjectConnection org_obj_conn;

        //    //Add descriptions to Organization_Object_Connection
        //    if (organization_object.descriptions != null && organization_object.descriptions.Count > 0)
        //    {
        //        foreach (var description in organization_object.descriptions)
        //        {
        //            org_obj_conn = new OrganizationObjectConnection();
        //            org_obj_conn.OrgObjGuid = organization_object.guid;
        //            org_obj_conn.DescriptionGuid = description.description_guid;
        //            DbContext.OrganizationObjectConnection.Add(org_obj_conn);
        //        }

        //        saveChangesCount += await DbContext.SaveChangesAsync();
        //    }

        //    //Add models to Organization_Object_Connection
        //    if (organization_object.models != null && organization_object.models.Count > 0)
        //    {
        //        foreach (var model_component in organization_object.models)
        //        {
        //            org_obj_conn = new OrganizationObjectConnection();
        //            org_obj_conn.OrgObjGuid = organization_object.guid;
        //            //org_obj_conn.ModelComponentGuid = model_component.model_component_guid;
        //            DbContext.OrganizationObjectConnection.Add(org_obj_conn);
        //        }

        //        saveChangesCount += await DbContext.SaveChangesAsync();
        //    }

        //    //Add activities to Organization_Object_Connection
        //    if (organization_object.activities != null && organization_object.activities.Count > 0)
        //    {
        //        foreach (var activity in organization_object.activities)
        //        {
        //            org_obj_conn = new OrganizationObjectConnection();
        //            org_obj_conn.OrgObjGuid = organization_object.guid;
        //            org_obj_conn.ActivityTemplateGuid = activity.activity_template_guid;
        //            DbContext.OrganizationObjectConnection.Add(org_obj_conn);
        //        }

        //        saveChangesCount += DbContext.SaveChanges();
        //    }

        //    return saveChangesCount > 0;
        //}

        //public async Task<bool> DeleteOrganizationObjectConnection(string org_obj_guid)TODO
        //{
        //    string guid = string.Empty;
        //    ////Get descriptions from Organization_Object_Connection by org_obj_guid
        //    //var descriptions = DbContext.OrganizationObjectConnection.Where(ooc => ooc.OrgObjGuid == org_obj_guid && ooc.DescriptionGuid != null);
        //    ////Get models from Organization_Object_Connection by org_obj_guid
        //    //var models = DbContext.OrganizationObjectConnection.Where(ooc => ooc.OrgObjGuid == org_obj_guid && ooc.ModelComponentGuid != null);
        //    ////Get activities from Organization_Object_Connection by org_obj_guid
        //    //var activities = DbContext.OrganizationObjectConnection.Where(ooc => ooc.OrgObjGuid == org_obj_guid && ooc.ActivityTemplateGuid != null);

        //    var connections = DbContext.OrganizationObjectConnection.Where(ooc => ooc.OrgObjGuid == org_obj_guid);

        //    //Append all items for deleting
        //    //List<OrganizationObjectConnection> org_obj_conn = new List<OrganizationObjectConnection>();
        //    //org_obj_conn.AddRange(descriptions);
        //    //org_obj_conn.AddRange(models);
        //    //org_obj_conn.AddRange(activities);

        //    //foreach (var conn in org_obj_conn)
        //    //{
        //    //    DbContext.OrganizationObjectConnection.Remove(conn);
        //    //}

        //    DbContext.OrganizationObjectConnection.RemoveRange(connections);

        //    int saveChangeCount = await DbContext.SaveChangesAsync();
        //    return saveChangeCount > 0;
        //}
        public async Task<bool> DeleteEntityDescription(string org_obj_guid)
        {
           string guid = string.Empty;
           var connections = DbContext.EntityDescription.Where(ooc => ooc.EntityGuid == org_obj_guid);
           DbContext.EntityDescription.RemoveRange(connections);
           int saveChangeCount = await DbContext.SaveChangesAsync();
           return saveChangeCount > 0;
        }
    //public async Task<bool> DeleteOrganizationOrObject(List<string> organization_guid_list)
    //{
    //    foreach (string org_obj_guid in organization_guid_list)
    //    {
    //        var org_obj = DbContext.OrganizationObject.Where(oo => oo.OrgObjGuid == org_obj_guid).FirstOrDefault();
    //        var org_obj_structure = DbContext.OrganizationStructure.Where(os => os.OrgObjGuid == org_obj_guid).FirstOrDefault();

    //        if (org_obj != null)
    //        {
    //            //get organization_object children
    //            List<string> children = DbContext.OrganizationObject
    //                                    .Where(oo => oo.OrganizationStructureOrgObjGu.OrgObjParentGuid == org_obj_guid)
    //                                    .Select(oo => oo.OrgObjGuid).ToList();

    //            if (children != null && children.Count > 0)
    //            {
    //                //pass children to DeleteOrganizationOrObject recursive 
    //                await DeleteOrganizationOrObject(children);
    //            }

    //            //delete from Organization_Structure
    //            if (org_obj_structure != null)
    //            {
    //                DbContext.OrganizationStructure.Remove(org_obj_structure);
    //                await DbContext.SaveChangesAsync();

    //            }

    //            //delete from Organization_Object_Connection
    //            await DeleteOrganizationObjectConnection(org_obj_guid);
    //            await DbContext.SaveChangesAsync();


    //            //delete from Estimated_Organization_Object
    //            //var estimated_org_obj_list = DbContext.EstimatedOrganizationObject.Where(os => os.OrgObjGuid == org_obj_guid || os.OrgObjEstimatedGuid == org_obj_guid).ToList();
    //            //DbContext.EstimatedOrganizationObject.RemoveRange(estimated_org_obj_list);
    //            //await DbContext.SaveChangesAsync();

    //            //delete from Activity/Form/Score/Calculate_Score
    //            var activities_list = DbContext.Activity.Where(a => a.UnitGuid == org_obj_guid).ToList();
    //             foreach (var activity in activities_list)
    //             {
    //                 await _activityService.DeleteActivity(activity.ActivityGuid);
    //             }

    //             //delete from Saved_Report_Connection
    //             var saved_report_connection_list = DbContext.SavedReportConnection.Where(src => src.OrgObjGuid == org_obj_guid).ToList();
    //            foreach (var saved_report_connection in saved_report_connection_list)
    //            {
    //                //delete from Saved_reports
    //                await _reportService.DeleteSavedReport(saved_report_connection.ReportGuid);
    //            }

    //            DbContext.SaveChanges();

    //            //delete from Score
    //            var score_list = DbContext.Score.Where(s => s.OrgObjGuid == org_obj_guid).ToList();
    //            DbContext.Score.RemoveRange(score_list);
    //            DbContext.SaveChanges();

    //            //delete from Calculate_Score
    //            var calculate_score_list = DbContext.CalculateScore.Where(cs => cs.OrgObjGuid == org_obj_guid).ToList();
    //            DbContext.CalculateScore.RemoveRange(calculate_score_list);
    //            DbContext.SaveChanges();

    //            //get deleted Users
    //            var user_list = DbContext.User.Where(u => u.OrgObjGuid == org_obj_guid).ToList();
    //            var userListGuids = user_list.Select(x => x.UserGuid);

    //            //delete from candidates
    //            var relatedCandidates = DbContext.Candidate.Where(c => userListGuids.Contains(c.UserGuid));
    //            DbContext.Candidate.RemoveRange(relatedCandidates);
    //            DbContext.SaveChanges();

    //            //delete from Users
    //            await _userService.DeleteUsers(user_list.Select(x => x.UserGuid));
    //            //DbContext.User.RemoveRange(user_list);
    //            //DbContext.SaveChanges();

    //            //delete from Organization_Union
    //            var organization_Union = DbContext.OrganizationUnion.Where(x => x.OrgObjGuid == org_obj_guid);
    //            DbContext.OrganizationUnion.RemoveRange(organization_Union);
    //            DbContext.SaveChanges();

    //            //delete from OrgModelPolygon
    //            var orgModelPolygon = DbContext.OrgModelPolygon.Where(x => x.OrgObjGuid == org_obj_guid);
    //            DbContext.OrgModelPolygon.RemoveRange(orgModelPolygon);
    //            DbContext.SaveChanges();

    //            //delete from PersonalUnit
    //            var personalUnit = DbContext.PersonalUnit.Where(x => x.OrgObjGuid == org_obj_guid);
    //            DbContext.PersonalUnit.RemoveRange(personalUnit);
    //            DbContext.SaveChanges();

    //            //delete Organization_Object
    //            DbContext.OrganizationObject.Remove(org_obj);
    //            DbContext.SaveChanges();
    //        }
    //    }

    //    return true;
    //}

    public async Task<bool> DeleteUnit(List<string> units_guid_list)
        {
            try
            {
                if (units_guid_list != null)
                {
               
                   
                        foreach (string unit_guid in units_guid_list)
                        {
                            var unit_obj = DbContext.Unit.Where(oo => oo.UnitGuid == unit_guid).FirstOrDefault();

                            if (unit_obj != null)

                            {   //get unit_object children
                                List<string> children = DbContext.Unit.Where(x => x.ParentUnitGuid == unit_guid).Select(x => x.UnitGuid).ToList();

                                if (children != null && children.Count > 0)
                                {
                                    //pass children to DeleteOrganizationOrObject recursive 
                                    await DeleteUnit(children);
                                }
                                //List<Activity> A_Estimated_Unit = DbContext.Activity.Where(x => x.EstimateUnits.Contains(unit_guid)).ToList();
                                //foreach (var item in A_Estimated_Unit)
                                //{
                                //    item.EstimateUnits = item.EstimateUnits.Where(e => e != unit_guid).ToArray();
                                //    await DbContext.SaveChangesAsync();
                                //}

                                //delete from Activity/Form/Score/Calculate_Score
                                //TODO
                                //var activities_list = DbContext.Activity.Where(a => a.EntityGuid == unit_guid).ToList();
                                //foreach (var activity in activities_list)
                                //{
                                //    await _activityService.DeleteActivity(activity.ActivityGuid);
                                //}

                                //delete from Saved_Report_Connection
                                //TODO
                                //var saved_report_connection_list = DbContext.SavedReportConnection.Where(src => src.OrgObjGuid == org_obj_guid).ToList();
                                //foreach (var saved_report_connection in saved_report_connection_list)
                                //{
                                //    //delete from Saved_reports
                                //    await _reportService.DeleteSavedReport(saved_report_connection.ReportGuid);
                                //}

                                //DbContext.SaveChanges();

                                //delete from Score
                                //var score_list = DbContext.Score.Where(s => s.OrgObjGuid == org_obj_guid).ToList();
                                //DbContext.Score.RemoveRange(score_list);
                                //DbContext.SaveChanges();

                                //delete from Calculate_Score
                                //var calculate_score_list = DbContext.CalculateScore.Where(cs => cs.OrgObjGuid == org_obj_guid).ToList();
                                //DbContext.CalculateScore.RemoveRange(calculate_score_list);
                                //DbContext.SaveChanges();

                                //get deleted Users
                                //var user_list = DbContext.User.Where(u => u.OrgObjGuid == org_obj_guid).ToList();
                                //var userListGuids = user_list.Select(x => x.UserGuid);

                                //delete from candidates
                                //var relatedCandidates = DbContext.Candidate.Where(c => userListGuids.Contains(c.UserGuid));
                                //DbContext.Candidate.RemoveRange(relatedCandidates);
                                //DbContext.SaveChanges();

                                //delete from Users
                                //await _userService.DeleteUsers(user_list.Select(x => x.UserGuid));
                                //DbContext.User.RemoveRange(user_list);
                                //DbContext.SaveChanges();

                                //delete from Organization_Union
                                //var organization_Union = DbContext.OrganizationUnion.Where(x => x.OrgObjGuid == org_obj_guid);
                                //DbContext.OrganizationUnion.RemoveRange(organization_Union);
                                //DbContext.SaveChanges();

                                //delete from OrgModelPolygon
                                //var orgModelPolygon = DbContext.OrgModelPolygon.Where(x => x.OrgObjGuid == org_obj_guid);
                                //DbContext.OrgModelPolygon.RemoveRange(orgModelPolygon);
                                //DbContext.SaveChanges();

                                //delete from PersonalUnit
                                //var personalUnit = DbContext.PersonalUnit.Where(x => x.OrgObjGuid == org_obj_guid);
                                //DbContext.PersonalUnit.RemoveRange(personalUnit);
                                //DbContext.SaveChanges();

                                await DeleteEntityDescription(unit_guid);
                                //delete Organization_Object
                                DbContext.Unit.Remove(unit_obj);
                                DbContext.SaveChanges();
                            }
                        }
                         return true;

                }
                else
                {
                    return false;
                }
            }
            
           
           
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
                return false;
            }
        }

        public async Task<bool> DeletePerson(List<string> persons_guid_list)
        {
            try
            {
                if (persons_guid_list != null)
                {
                    foreach (string person_guid in persons_guid_list)
                    {
                        var person = await DbContext.Person.Where(oo => oo.PersonGuid == person_guid).FirstOrDefaultAsync();


                        if (person != null)

                        {
                            List<Person> p = await DbContext.Person.Where(p => p.DirectManagerGuid == person_guid).ToListAsync();
                            if (p != null && p.Count() > 0)
                            {
                                foreach (var item in p)
                                {
                                    item.DirectManagerGuid = null;

                                }
                                DbContext.Person.UpdateRange(p);
                            }

                            List<Person> p2 = await DbContext.Person.Where(p => p.professionalManagerGuid == person_guid).ToListAsync();
                            if (p2 != null && p2.Count() > 0)
                            {
                                foreach (var item in p2)
                                {
                                    item.professionalManagerGuid = null;
                                }
                                DbContext.Person.UpdateRange(p2);
                            }

                            List<Unit> u = await DbContext.Unit.Where(u => u.ManagerUnitGuid == person_guid).ToListAsync();
                            if (u != null && u.Count() > 0)
                            {
                                foreach (var item in u)
                                {
                                    item.ManagerUnitGuid = null;
                                }
                                DbContext.Unit.UpdateRange(u);
                            }

                            //List<Activity> A_Estimated_Person = DbContext.Activity.Where(x => x.EstimatePersons.Contains(person_guid)).ToList();
                            //foreach (var item in A_Estimated_Person)
                            //{
                            //    item.EstimatePersons = item.EstimatePersons.Where(e => e != person_guid).ToArray();
                            //    await DbContext.SaveChangesAsync();
                            //}

                            //delete from Activity/Form/Score/Calculate_Score
                            // TODO activityconections
                            //var activities_list = DbContext.Activity.Where(a => a.EntityGuid == person_guid).ToList();
                            //foreach (var activity in activities_list)
                            //{
                            //    await _activityService.DeleteActivity(activity.ActivityGuid);
                            //}
                            await DeleteEntityDescription(person_guid);

                            DbContext.Person.Remove(person);
                            DbContext.SaveChanges();
                        }
                    }

                    return true;
                }

                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Logger.Error($"ex:{ex.InnerException?.Message ?? ex.Message }");
                return false;
            }
        }
        //public async Task<bool> DragAndDrop(string dest_org_guid, string drag_org_guid, List<string> org_children_guid_list)
        //{
        //    if (!string.IsNullOrEmpty(dest_org_guid) && !string.IsNullOrEmpty(drag_org_guid))
        //    {
        //        //change hierarchy

        //        //get drag item from structure
        //        OrganizationStructure org_structure = DbContext.OrganizationStructure.Where(o => o.OrgObjGuid == drag_org_guid).FirstOrDefault();

        //        //get destination item from organization or organization_object
        //        var org_obj = DbContext.OrganizationObject.Where(oo => oo.OrgObjGuid == dest_org_guid).FirstOrDefault();

        //        if (org_obj != null)
        //        {
        //            org_structure.OrgParentGuid = null;
        //            org_structure.OrgObjParentGuid = dest_org_guid;
        //        }

        //        DbContext.OrganizationStructure.Update(org_structure);
        //        await DbContext.SaveChangesAsync();
        //    }

        //    for (int i = 0; i < org_children_guid_list.Count; i++)
        //    {
        //        var org_obj = DbContext.OrganizationObject.Where(oo => oo.OrgObjGuid == org_children_guid_list[i]).FirstOrDefault();

        //        if (org_obj != null)
        //        {
        //            org_obj.Order = i + 1;
        //            DbContext.OrganizationObject.Update(org_obj);
        //            await DbContext.SaveChangesAsync();
        //        }
        //    }

        //    return true;
        //}

        //public async Task<bool> ApplyProperties(OrganizationObjectData obj, List<string> paste_guid_list)
        //{
        //    //update organization_object
        //    string objGuid = await UpdateOrganizationObject(obj);
        //    if (objGuid != null)
        //    {
        //        foreach (string guid in paste_guid_list)
        //        {
        //            OrganizationObject copiedObj = DbContext.OrganizationObject.Where(oo => oo.OrgObjGuid == guid).FirstOrDefault();
        //            if (copiedObj != null)
        //            {
        //                //delete existing connections
        //                await DeleteOrganizationObjectConnection(guid);

        //                //add connections
        //                obj.guid = guid;
        //                await CreateOrganizationObjectConnection(obj);
        //            }
        //        }
        //    }

        //    return objGuid != null;
        //}

        //public async Task<List<NewOrg>> GetSaveOrgObjList(OrganizationObjectData orgObj, string parentGuid, bool isRec = false, int order = 1, string duplicate = null, int countDuplicate = 0, List<NewOrg> res = null)
        //{
        //    if (res == null)
        //    {
        //        res = new List<NewOrg>();
        //    }

        //    if (duplicate != null)
        //    {
        //        orgObj.name = orgObj.name + "_" + duplicate + "_" + (countDuplicate + 1).ToString();
        //    }

        //    orgObj.guid = Util.CreateGuid().ToString();//Create new guid

        //    OrganizationObject new_org_obj = new OrganizationObject();
        //    new_org_obj.OrgObjGuid = orgObj.guid;
        //    new_org_obj.Name = orgObj.name;
        //    new_org_obj.Remark = orgObj.remark;
        //    new_org_obj.OrgObjType = orgObj.org_type ?? null;
        //    new_org_obj.Order = duplicate != null ? orgObj.order : order;

        //    OrganizationStructure org_obj_structure = new OrganizationStructure();
        //    org_obj_structure.OrgObjGuid = orgObj.guid;
        //    org_obj_structure.OrgObjParentGuid = parentGuid;

        //    List<OrganizationObjectConnection> orgConnections = new List<OrganizationObjectConnection>();
        //    OrganizationObjectConnection org_obj_conn;

        //    #region connections
        //    if (orgObj.descriptions != null && orgObj.descriptions.Count > 0)
        //    {
        //        foreach (var description in orgObj.descriptions)
        //        {
        //            org_obj_conn = new OrganizationObjectConnection();
        //            org_obj_conn.OrgObjGuid = orgObj.guid;
        //            org_obj_conn.DescriptionGuid = description.description_guid;
        //            orgConnections.Add(org_obj_conn);
        //        }
        //    }

        //    if (orgObj.models != null && orgObj.models.Count > 0)
        //    {
        //        foreach (var model_component in orgObj.models)
        //        {
        //            org_obj_conn = new OrganizationObjectConnection();
        //            org_obj_conn.OrgObjGuid = orgObj.guid;
        //            org_obj_conn.ModelComponentGuid = model_component.model_component_guid;
        //            orgConnections.Add(org_obj_conn);
        //        }
        //    }

        //    if (orgObj.activities != null && orgObj.activities.Count > 0)
        //    {
        //        foreach (var activity in orgObj.activities)
        //        {
        //            org_obj_conn = new OrganizationObjectConnection();
        //            org_obj_conn.OrgObjGuid = orgObj.guid;
        //            org_obj_conn.ActivityTemplateGuid = activity.activity_template_guid;
        //            orgConnections.Add(org_obj_conn);
        //        }
        //    }
        //    #endregion connections

        //    var newOrg = new NewOrg() { orgObj = new_org_obj, orgStructure = org_obj_structure, orgConnection = orgConnections.ToArray() };
        //    res.Add(newOrg);

        //    if (isRec)
        //    {
        //        //Parallel.ForEach(orgObj.children, async (child, state, index) =>
        //        //{
        //        //    child.parent_guid = orgObj.guid;
        //        //    await GetSaveOrgObjList(child, orgObj.guid, isRec, ((int)index) + 1, duplicate, countDuplicate, res);
        //        //});

        //        int i = 0;
        //        foreach (var child in orgObj.children)
        //        {
        //            child.parent_guid = orgObj.guid;
        //            await GetSaveOrgObjList(child, orgObj.guid, isRec, ++i, duplicate, countDuplicate, res);
        //        }
        //    }

        //    return await Task.FromResult(res);
        //}

        //public async Task<string> DuplicateOrganizationObject(OrganizationObjectData organization_object, bool isRec)
        //{
        //    string guid = string.Empty;

        //    string duplicate = _config.DefaultLocale == "en" ? "duplicate" : "משוכפל";
        //    var countDuplicate = DbContext.OrganizationObject
        //                              .Where(oo => oo.Name.Contains(organization_object.name + "_" + duplicate)).Count();

        //    int order = DbContext.OrganizationStructure.Where(os => os.OrgObjParentGuid == organization_object.parent_guid).Count() + 1;

        //    IEnumerable<NewOrg> newOrgs = await GetSaveOrgObjList(organization_object, organization_object.parent_guid, isRec, order, duplicate, countDuplicate);

        //    var allOrgObj = newOrgs.Select(x => x.orgObj);
        //    DbContext.OrganizationObject.AddRange(allOrgObj);
        //    await DbContext.SaveChangesAsync();
        //    this.CommitTransaction();

        //    var allOrgObjConnections = newOrgs.SelectMany(x => x.orgConnection.Select(y=>y));
        //    DbContext.OrganizationObjectConnection.AddRange(allOrgObjConnections);
        //    await DbContext.SaveChangesAsync();

        //    var allOrgObjStructure = newOrgs.Select(x => x.orgStructure);
        //    DbContext.OrganizationStructure.AddRange(allOrgObjStructure);
        //    await DbContext.SaveChangesAsync();

        //    //var countDuplicate = DbContext.OrganizationObject
        //    //                      .Where(oo => oo.Name.Contains(organization_object.name + "_" + duplicate)).Count();

        //    //organization_object.name = organization_object.name + "_" + duplicate + "_" + (countDuplicate + 1).ToString();
        //    //guid = await CreateOrganizationObject(organization_object.parent_guid, organization_object);

        //    //if (isRec)
        //    //{
        //    //    foreach (var child in organization_object.children)
        //    //    {
        //    //        child.parent_guid = guid;
        //    //        await DuplicateOrganizationObject(child, isRec);
        //    //    }
        //    //}
        //    var baseOrg = newOrgs.FirstOrDefault();
        //    if (baseOrg != null)
        //        guid = baseOrg.orgObj.OrgObjGuid;
        //    return guid;
        //}

        //public async Task<string[]> GetOrgObjChildren(string[] org_obj_list, string unionGuid = "1", string[] total_list = null)
        //{
        //    total_list = total_list ?? new string[] { };

        //    string[] children = null;
        //    if (unionGuid == "1")
        //    {
        //        children = await DbContext.OrganizationStructure.Where(os => org_obj_list.Contains(os.OrgObjParentGuid))
        //                                        .Select(os => os.OrgObjGuid).ToArrayAsync();
        //    }
        //    else
        //    {
        //        children = await DbContext.OrganizationUnion.Where(os => os.OrganizationUnionGuid == unionGuid && org_obj_list.Contains(os.ParentOrgObjGuid))
        //                                        .Select(os => os.OrgObjGuid).ToArrayAsync();
        //    }

        //    if (children == null || !children.Any())
        //        return total_list;

        //    total_list = total_list.Concat(children).ToArray();
        //    return await GetOrgObjChildren(children.ToArray(), unionGuid, total_list);
        //}

        //public async Task<List<string>> GetOrgModels(string orgObjGuid)
        //{
        //    string[] childsModel;
        //    var ModelComponentList = (from ooc in DbContext.OrganizationObjectConnection
        //                          where ooc.OrgObjGuid == orgObjGuid && ooc.ModelComponentGuid != null
        //                          select ooc.ModelComponentGuid).ToList();

        //    int count = ModelComponentList.Count;
        //    for (int i = 0; i < count; i++)
        //    {
        //        childsModel = await _modelService.GetModelList(new string[] { ModelComponentList[i] });
        //        ModelComponentList.AddRange(childsModel);
        //    }

        //    return ModelComponentList;
        //}

        //public async Task<List<OrgData>> GetOrgObjChildrenStructure(OrganizationStructure org_obj, string unionGuid, bool add_existing, List<OrgData> org_obj_list = null)
        //{
        //    List<string> ModelComponentList;

        //    if (org_obj_list == null)
        //    {
        //        org_obj_list = new List<OrgData>();
        //        if (add_existing)
        //        {
        //            ModelComponentList = await GetOrgModels(org_obj.OrgObjGuid);
        //            org_obj_list.Add(new OrgData(org_obj, ModelComponentList));
        //        }
        //    }

        //    List<OrganizationStructure> org_obj_children_guid = new List<OrganizationStructure>();
        //    if (unionGuid == "1")
        //    {
        //        org_obj_children_guid = DbContext.OrganizationStructure.Where(os => os.OrgObjParentGuid == org_obj.OrgObjGuid).Include(oo => oo.OrgObjGu).OrderBy(os => os.OrgObjGu.Order).ToList();
        //    }
        //    else
        //    {
        //        org_obj_children_guid = DbContext.OrganizationUnion.Where(os => os.OrganizationUnionGuid == unionGuid && os.ParentOrgObjGuid == org_obj.OrgObjGuid).OrderBy(os => os.Order).Select(oo => new OrganizationStructure() { OrgObjGuid = oo.OrgObjGuid, OrgObjGu = oo.OrgObjGu, OrgObjParentGuid = oo.ParentOrgObjGuid, OrgObjParentGu = oo.ParentOrgObjGu }).Distinct().ToList();
        //    }

        //    foreach (var org_obj_child in org_obj_children_guid)
        //    {
        //        ModelComponentList = ModelComponentList = await GetOrgModels(org_obj_child.OrgObjGuid);
        //        org_obj_list.Add(new OrgData(org_obj_child, ModelComponentList));
        //        await GetOrgObjChildrenStructure(org_obj_child, unionGuid, false, org_obj_list);
        //    }

        //    return org_obj_list;
        //}

        //public async Task<List<OrgData>> GetSubOrgObjChildren(OrgData org_obj, List<OrgData> allOrgs, bool add_existing, List<OrgData> org_obj_list = null)
        //{
        //    if (org_obj_list == null)
        //    {
        //        org_obj_list = new List<OrgData>();
        //        if (add_existing)
        //        {
        //            org_obj_list.Add(org_obj);
        //        }
        //    }

        //    var org_obj_children_guid = allOrgs.Where(os => os.OrganizationStructure.OrgObjParentGuid == org_obj.OrganizationStructure.OrgObjGu.OrgObjGuid).AsEnumerable();

        //    foreach (var org_obj_child in org_obj_children_guid)
        //    {
        //        org_obj_list.Add(org_obj_child);
        //        await GetSubOrgObjChildren(org_obj_child, allOrgs, false, org_obj_list);
        //    }

        //    return org_obj_list;
        //}

        //public async Task<Dictionary<string, List<OrgModels>>> GetChildOrgModels(string org_obj_guid, string unionGuid = "1")
        //{
        //    Dictionary<string, List<OrgModels>> result = new Dictionary<string, List<OrgModels>>();
        //    List<OrgData> childs;
        //    List<OrgModels> orgModels;
        //    OrgModels d;

        //    var org_obj = DbContext.OrganizationStructure.Where(os => os.OrgObjGuid == org_obj_guid).Include(os => os.OrgObjGu).FirstOrDefault();
        //    var orgObjChildrenStructure = await GetOrgObjChildrenStructure(org_obj, unionGuid, true);


        //    foreach (var org in orgObjChildrenStructure)
        //    {
        //        childs = await GetSubOrgObjChildren(org, orgObjChildrenStructure, true);

        //        orgModels = new List<OrgModels>();
        //        foreach (var c in childs)
        //        {
        //            d = new OrgModels()
        //            {
        //                OrgObjParentGuid = c.OrganizationStructure.OrgObjParentGuid,
        //                OrgObjGuid = c.OrganizationStructure.OrgObjGu.OrgObjGuid,
        //                OrgObjName = c.OrganizationStructure.OrgObjGu.Name,
        //                OrgOrder = c.OrganizationStructure.OrgObjGu.Order.HasValue ? c.OrganizationStructure.OrgObjGu.Order.Value : 0,
        //                OrgObjType = c.OrganizationStructure.OrgObjGu.OrgObjType,
        //                ModelComponentList = c.ModelComponentList
        //            };

        //            orgModels.Add(d);
        //        }

        //        result.Add(org.OrganizationStructure.OrgObjGu.OrgObjGuid, orgModels);
        //    }

        //    return result;
        //}

        //public async Task<string> GetOrgName(string orgObjGuid)
        //{
        //    string orgName = DbContext.OrganizationObject.Find(orgObjGuid).Name;
        //    return await Task.FromResult(orgName);
        //}

        public async Task<List<UnitDataInfo>> GetOrgByModel(Dictionary<string, List<OrgModels>> orgModelsDict, string orgObjGuid, string modelComponentGuid)
        {
            try
            {
                List<OrgModels> orgModels;
                List<UnitDataInfo> data = null;
                orgModelsDict.TryGetValue(orgObjGuid, out orgModels);

                if (orgModels != null && orgModels.Count > 0)
                {
                    data = orgModels.Where(om => om.ModelComponentList.Contains(modelComponentGuid))
                                        .Select(om => new UnitDataInfo() { guid = om.OrgObjGuid, name = om.OrgObjName, order = om.OrgOrder, org_type = om.OrgObjType }).ToList();
                }

                return await Task.FromResult(data);
            }
            catch(Exception ex)
            {
                
            }

            return null;
        }

        public Task<Unit> GetUnitByGuid(string unitGuid)
        {
            return DbContext.Unit.Where(u => u.UnitGuid == unitGuid).FirstOrDefaultAsync();
        }

        //public async Task<HttpStatusCode> UpdatePermissionUnits(Guid ownerUnit, string[] units)
        //{
        //    var org = this.DbContext.OrganizationStructure.FirstOrDefault(r => r.OrgObjGuid == ownerUnit.ToString().Replace("-",""));
        //    if (org != null)
        //    {
        //        org.PermissionUnits = units.Any() ? units.Select(x => Guid.Parse(x)).ToArray() : null;
        //        return await this.DbContext.SaveChangesAsync() > 0
        //            ? HttpStatusCode.OK : HttpStatusCode.NotModified;
        //    }
        //    return HttpStatusCode.NotFound;
        //}

        //public async Task<int> ImportPersonalUnit(List<PersonalUnit> allPersonalUnits)
        //{
        //    List<string> allUnits = allPersonalUnits.Select(x => x.OrgObjGuid).Distinct().ToList();

        //    //delete personalUnits that connect to units that exist in current csv file
        //    DbContext.PersonalUnit.RemoveRange(DbContext.PersonalUnit.Where(x=> allUnits.Contains(x.OrgObjGuid)));
        //    DbContext.SaveChanges();

        //    DbContext.PersonalUnit.AddRange(allPersonalUnits);
        //    DbContext.SaveChanges();

        //    //add model
        //    string modelGuid = Constants.PersonnelModelGuid;
        //    await CreatePersonnelModel(modelGuid);

        //    //connect model to units if not exist
        //    for (int i = 0; i < allUnits.Count; i++)
        //    {
        //        OrganizationObjectConnection ooc = new OrganizationObjectConnection();
        //        ooc.OrgObjGuid = allUnits[i];
        //        ooc.ModelComponentGuid = modelGuid;
        //        var exist = DbContext.OrganizationObjectConnection.FirstOrDefault(x => x.OrgObjGuid == ooc.OrgObjGuid && x.ModelComponentGuid == ooc.ModelComponentGuid);
        //        if (exist == null)
        //        {
        //            DbContext.OrganizationObjectConnection.Add(ooc);
        //            DbContext.SaveChanges();
        //        }
        //    }

        //    //add score
        //    //await SaveModelScores(allPersonalUnits); TODO

        //    return allPersonalUnits.Count();
        //}

        public async Task CreatePersonnelModel(string modelGuid)
        {
            //נתוני כוח אדם
            ModelComponent model_component = new ModelComponent();
            model_component.ModelComponentGuid = modelGuid;
            model_component.Name = "נתוני כח אדם";
            model_component.Source = 7;
            model_component.Status = 2;
            model_component.Weight = 100;
            model_component.CreateDate = Util.ConvertDateToString(DateTime.Now);
            model_component.ModifiedDate = Util.ConvertDateToString(DateTime.Now);
            model_component.MetricRequired = false;
            model_component.MetricRollupMethod = 3;
            model_component.MetricIsVisible = true;
            model_component.MetricNotDisplayIfIrrelevant = false;
            model_component.MetricExpiredPeriod = "m12";
            model_component.MetricCommentObligationLevel = 0;
            model_component.MetricGradualDecreasePrecent = 0;
            model_component.MetricGradualDecreasePeriod = 0;
            model_component.MetricMinimumFeeds = 0;
            model_component.ShowOrigionValue = false;
            model_component.TemplateType = 0;
            model_component.CalcAsSum = false;
            model_component.GroupChildren = false;

            string rootGuid = null;
            await CreateDynamicModel(model_component, rootGuid, 3);

            rootGuid = model_component.ModelComponentGuid;

            //תקן
            model_component = new ModelComponent();
            model_component.ModelComponentGuid = Constants.RequiredModelGuid;
            model_component.Name = "תקן";
            model_component.Source = 5;
            model_component.Status = 2;
            model_component.ModelComponentOrder = 1;
            model_component.Weight = 25;
            model_component.CreateDate = Util.ConvertDateToString(DateTime.Now);
            model_component.ModifiedDate = Util.ConvertDateToString(DateTime.Now);
            model_component.MetricRequired = false;
            model_component.MetricMeasuringUnit = 1;
            model_component.MetricCalenderRollup = 2;
            model_component.MetricIsVisible = true;
            model_component.MetricNotDisplayIfIrrelevant = false;
            model_component.MetricExpiredPeriod = "m12";
            model_component.MetricCommentObligationLevel = 0;
            model_component.MetricGradualDecreasePrecent = 0;
            model_component.MetricGradualDecreasePeriod = 0;
            model_component.MetricMinimumFeeds = 0;
            model_component.ShowOrigionValue = true;
            model_component.MetricSource = 4;
            model_component.TemplateType = 0;
            model_component.CalcAsSum = true;
            model_component.GroupChildren = false;

            await CreateDynamicModel(model_component, rootGuid, 1);

            //מצבה
            model_component = new ModelComponent();
            model_component.ModelComponentGuid = Constants.ActualModelGuid;
            model_component.Name = "מצבה";
            model_component.Source = 5;
            model_component.Status = 2;
            model_component.ModelComponentOrder = 2;
            model_component.Weight = 25;
            model_component.CreateDate = Util.ConvertDateToString(DateTime.Now);
            model_component.ModifiedDate = Util.ConvertDateToString(DateTime.Now);
            model_component.MetricRequired = false;
            model_component.MetricMeasuringUnit = 1;
            model_component.MetricCalenderRollup = 2;
            model_component.MetricIsVisible = true;
            model_component.MetricNotDisplayIfIrrelevant = false;
            model_component.MetricExpiredPeriod = "m12";
            model_component.MetricCommentObligationLevel = 0;
            model_component.MetricGradualDecreasePrecent = 0;
            model_component.MetricGradualDecreasePeriod = 0;
            model_component.MetricMinimumFeeds = 0;
            model_component.ShowOrigionValue = true;
            model_component.MetricSource = 4;
            model_component.TemplateType = 0;
            model_component.CalcAsSum = true;
            model_component.GroupChildren = false;

            await CreateDynamicModel(model_component, rootGuid, 1);

            //אחוז מוסמכים
            model_component = new ModelComponent();
            model_component.ModelComponentGuid = Constants.CertificatedModelGuid;
            model_component.Name = "אחוז מוסמכים";
            model_component.Source = 5;
            model_component.Status = 2;
            model_component.ModelComponentOrder = 3;
            model_component.Weight = 25;
            model_component.CreateDate = Util.ConvertDateToString(DateTime.Now);
            model_component.ModifiedDate = Util.ConvertDateToString(DateTime.Now);
            model_component.MetricRequired = false;
            model_component.MetricMeasuringUnit = 3;
            model_component.MetricCalenderRollup = 2;
            model_component.MetricIsVisible = true;
            model_component.MetricNotDisplayIfIrrelevant = false;
            model_component.MetricExpiredPeriod = "m12";
            model_component.MetricCommentObligationLevel = 0;
            model_component.MetricGradualDecreasePrecent = 0;
            model_component.MetricGradualDecreasePeriod = 0;
            model_component.MetricMinimumFeeds = 0;
            model_component.ShowOrigionValue = false;
            model_component.MetricSource = 4;
            model_component.TemplateType = 0;
            model_component.CalcAsSum = false;
            model_component.GroupChildren = false;

            await CreateDynamicModel(model_component, rootGuid, 1);

            //אחוז מרועננים
            model_component = new ModelComponent();
            model_component.ModelComponentGuid = Constants.RefreshedModelGuid;
            model_component.Name = "אחוז מרועננים";
            model_component.Source = 5;
            model_component.Status = 2;
            model_component.ModelComponentOrder = 4;
            model_component.Weight = 25;
            model_component.CreateDate = Util.ConvertDateToString(DateTime.Now);
            model_component.ModifiedDate = Util.ConvertDateToString(DateTime.Now);
            model_component.MetricRequired = false;
            model_component.MetricMeasuringUnit = 3;
            model_component.MetricCalenderRollup = 2;
            model_component.MetricIsVisible = true;
            model_component.MetricNotDisplayIfIrrelevant = false;
            model_component.MetricExpiredPeriod = "m12";
            model_component.MetricCommentObligationLevel = 0;
            model_component.MetricGradualDecreasePrecent = 0;
            model_component.MetricGradualDecreasePeriod = 0;
            model_component.MetricMinimumFeeds = 0;
            model_component.ShowOrigionValue = false;
            model_component.MetricSource = 4;
            model_component.TemplateType = 0;
            model_component.CalcAsSum = false;
            model_component.GroupChildren = false;

            await CreateDynamicModel(model_component, rootGuid, 1);

        }

        public async Task CreateDynamicModel(ModelComponent model_component, string rootGuid, int measuringUnit)
        {
            bool exsit = DbContext.ModelComponent.Any(mc => mc.ModelComponentGuid == model_component.ModelComponentGuid);

            if (!exsit)
            {
                //model component
                DbContext.ModelComponent.Add(model_component);
                DbContext.SaveChanges();

                if (!string.IsNullOrEmpty(rootGuid))
                {
                    //model structure
                    ModelStructure model_structure = new ModelStructure();
                    model_structure.ModelComponentGuid = model_component.ModelComponentGuid;
                    model_structure.ModelComponentParentGuid = rootGuid;

                    DbContext.ModelStructure.Add(model_structure);
                    DbContext.SaveChanges();
                }

                //convertion table
                List<ConvertionTableData> convertion_data = new List<ConvertionTableData>();
                convertion_data = _modelService.GetDefaultConvertionTable(measuringUnit, model_component.ModelComponentGuid);
                List<ConvertionTable> data = Mapper.Map<List<ConvertionTable>>(convertion_data);
                //List<ConvertionTable> data = convertion_data.Select(x => new Model.Entities.ConvertionTable()
                //{
                //    ModelComponentGuid = x.model_component_guid,
                //    LevelId = x.level_id,
                //    StartRange = x.start_range,
                //    EndRange = x.end_range,
                //    ConversionTableModifiedDate = x.conversion_table_modified_date,
                //    ConversionTableStatus = x.conversion_table_status,
                //    ConversionTableCreateDate = x.conversion_table_create_date,
                //    StartRangeScoreDisplayed = x.start_range_score_displayed,
                //    EndRangeScoreDisplayed = x.end_range_score_displayed,
                //    ConversionTableScoreOrder = x.conversion_table_score_order,
                //    ConversionTableFinalScore = x.conversion_table_final_score
                //}).ToList();

                DbContext.ConvertionTable.AddRange(data);
                DbContext.SaveChanges();
            }
        }
        //TODO
        //public async Task SaveModelScores(List<PersonalUnit> allPersonalUnits)
        //{
        //    //remove exist scores
        //    List<string> allGuids = new List<string> { Constants.RequiredModelGuid, Constants.ActualModelGuid, Constants.CertificatedModelGuid, Constants.RefreshedModelGuid };
        //    var allOrgs = allPersonalUnits.Select(x => x.OrgObjGuid);
        //    var existScores = DbContext.Score.Where(s => allGuids.Contains(s.ModelComponentGuid) && allOrgs.Contains(s.OrgObjGuid));
        //    DbContext.Score.RemoveRange(existScores);
        //    DbContext.SaveChanges();

        //    List<ConvertionTableData> precentageConvertionTable = _modelService.GetDefaultConvertionTable(3, null);
        //    int required, actual, refreshed, refreshedPrecent, certificated, certificatedPrecent;
        //    Score score;

        //    var allUnits = allPersonalUnits.GroupBy(x => x.OrgObjGuid);

        //    foreach (var unit in allUnits)
        //    {
        //        required = unit.Where(x => !string.IsNullOrEmpty(x.RoleName)).Count();
        //        actual = unit.Where(x => !string.IsNullOrEmpty(x.IdentityNumber)).Count();
        //        refreshed = unit.Where(x => x.RefreshingValid == true).Count();
        //        refreshedPrecent = actual > 0 ? refreshed * 100 / actual : 0;
        //        certificated = unit.Where(x => x.Certification == true).Count();
        //        certificatedPrecent = actual > 0 ? certificated * 100 / actual : 0;

        //        score = new Score
        //        {
        //            ModelComponentGuid = Constants.RequiredModelGuid,
        //            OrgObjGuid = unit.Key,
        //            OriginalScore = required,
        //            ConvertionScore = required,
        //            Status = 4,
        //        };
        //        DbContext.Score.Add(score);
        //        DbContext.SaveChanges();

        //        score = new Score
        //        {
        //            ModelComponentGuid = Constants.ActualModelGuid,
        //            //OrgObjGuid = unit.Key,TODO
        //            OriginalScore = actual,
        //            ConvertionScore = actual,
        //            Status = 4,
        //        };
        //        DbContext.Score.Add(score);
        //        DbContext.SaveChanges();

        //        score = new Score
        //        {
        //            ModelComponentGuid = Constants.CertificatedModelGuid,
        //            //OrgObjGuid = unit.Key,TODO
        //            OriginalScore = certificatedPrecent,
        //            ConvertionScore = precentageConvertionTable.Where(ct => ct.start_range <= certificatedPrecent && ct.end_range >= certificatedPrecent)
        //                                                           .Select(ct => ct.conversion_table_final_score).FirstOrDefault(),
        //            Status = 4,
        //        };
        //        DbContext.Score.Add(score);
        //        DbContext.SaveChanges();

        //        score = new Score
        //        {
        //            ModelComponentGuid = Constants.RefreshedModelGuid,
        //            OrgObjGuid = unit.Key,
        //            OriginalScore = refreshedPrecent,
        //            ConvertionScore = precentageConvertionTable.Where(ct => ct.start_range <= refreshedPrecent && ct.end_range >= refreshedPrecent)
        //                                                           .Select(ct => ct.conversion_table_final_score).FirstOrDefault(),
        //            Status = 4,
        //        };
        //        DbContext.Score.Add(score);
        //        DbContext.SaveChanges();
        //    }
        //}

        public async Task<List<OrganizationUnion>> GetOrganizationUnion()
        {
            var data = DbContext.OrganizationUnion.Where(ou => ou.OrgObjGuid == null && ou.ParentOrgObjGuid == null).ToList();
            return await Task.FromResult(data);
        }

        public async Task<OrganizationUnionTreeData> GetOrganizationUnionDetails(string organizationUnionGuid)
        {
            OrganizationUnionTreeData tree = new OrganizationUnionTreeData();
            OrganizationUnion selectedOrgainzationUnion = DbContext.OrganizationUnion.FirstOrDefault(ou => ou.OrganizationUnionGuid == organizationUnionGuid && ou.OrgObjGuid != null && ou.ParentOrgObjGuid == null);
            
            if (selectedOrgainzationUnion != null)
            {
                tree.data = Mapper.Map<OrganizationUnionData>(selectedOrgainzationUnion);
                await BuildOrganizationUnionTree(tree);
            }
            return tree;
        }

        public async Task BuildOrganizationUnionTree(OrganizationUnionTreeData tree)
        {
            List<OrganizationUnionData> children = Mapper.Map<List<OrganizationUnionData>>(DbContext.OrganizationUnion.Where(ou => ou.OrganizationUnionGuid == tree.data.OrganizationUnionGuid && ou.ParentOrgObjGuid == tree.data.OrgObjGuid).ToList());

            if (children == null || !children.Any())
                return;

            tree.children = new List<OrganizationUnionTreeData>();
            OrganizationUnionTreeData c;
            foreach (var child in children)
            {
                c = new OrganizationUnionTreeData() { data = child };
                tree.children.Add(c);
                await BuildOrganizationUnionTree(c);
            }
        }

        public async Task<bool> DeleteOrganizationUnion(string OrganizationUnionGuid)
        {
            var deletedOU = DbContext.OrganizationUnion.Where(ou => ou.OrganizationUnionGuid == OrganizationUnionGuid);
            DbContext.OrganizationUnion.RemoveRange(deletedOU);
            await DbContext.SaveChangesAsync();
            return true;
        }

        public async Task<string> SaveOrganizationUnion(OrganizationUnionData ouData, string ouJsonTree)
        {
            string guid = string.Empty;

            if (ouData.OrganizationUnionGuid == null)
            {
                guid = await CreateOrganizationUnion(ouData, ouJsonTree);
            }
            else
            {
                guid = await UpdateOrganizationUnion(ouData, ouJsonTree);
            }

            return guid;
        }

        public async Task<string> CreateOrganizationUnion(OrganizationUnionData ouData, string ouJsonTree)
        {
            string guid = string.Empty;

            int order = DbContext.OrganizationUnion.Where(ou => ou.OrgObjGuid == null && ou.ParentOrgObjGuid == null).Count() + 1;

            OrganizationUnion ou = new OrganizationUnion();
            ou.OrganizationUnionGuid = Util.CreateGuid().ToString();
            ou.Name = ouData.Name;
            ou.Description = ouData.Description;
            ou.Order = order;
            ou.ModifiedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            DbContext.OrganizationUnion.Add(ou);
            DbContext.SaveChanges();

            BaseTree tree = JsonConvert.DeserializeObject<BaseTree>(ouJsonTree);

            //add organization tree
            await AddOUTree(ou.OrganizationUnionGuid, tree);

            guid = ou.OrganizationUnionGuid;
            return guid;
        }

        public async Task<string> UpdateOrganizationUnion(OrganizationUnionData ouData, string ouJsonTree)
        {
            string guid = string.Empty;

            OrganizationUnion existOU = DbContext.OrganizationUnion.FirstOrDefault(ou => ou.OrganizationUnionGuid == ouData.OrganizationUnionGuid && ou.OrgObjGuid == null && ou.ParentOrgObjGuid == null);
            
            if (existOU != null)
            {
                existOU.Name = ouData.Name;
                existOU.Description = ouData.Description;
                existOU.ModifiedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                DbContext.OrganizationUnion.Update(existOU);
                await DbContext.SaveChangesAsync();

                //remove existing organization tree
                var deletedOT = DbContext.OrganizationUnion.Where(ou => ou.OrganizationUnionGuid == ouData.OrganizationUnionGuid && ou.OrgObjGuid != null);
                DbContext.OrganizationUnion.RemoveRange(deletedOT);
                DbContext.SaveChanges();

                BaseTree tree = JsonConvert.DeserializeObject<BaseTree>(ouJsonTree);

                //add organization tree
                await AddOUTree(ouData.OrganizationUnionGuid, tree);
            }

            guid = ouData.OrganizationUnionGuid;
            return guid;
        }

        public async Task AddOUTree(string organizationUnionGuid, BaseTree ouJsonTree, string parentGuid = null, int order = 1)
        {
            OrganizationUnion ou = new OrganizationUnion();
            ou.OrganizationUnionGuid = organizationUnionGuid;
            ou.OrgObjGuid = ouJsonTree.data;
            ou.ParentOrgObjGuid = parentGuid;
            ou.Name = ouJsonTree.label;
            ou.Order = order;

            DbContext.OrganizationUnion.Add(ou);
            DbContext.SaveChanges();

            if (ouJsonTree.children != null)
            {
                for (int i = 0; i < ouJsonTree.children.Count; i++)
                {
                    await AddOUTree(organizationUnionGuid, ouJsonTree.children[i], ouJsonTree.data, i + 1);
                }
            }
        }

       
    }

    public class BaseTree
    {
        public string data { get; set; }
        public string label { get; set; }
        public List<BaseTree> children { get; set; }
    }

    //public class NewOrg
    //{
    //    public OrganizationObject orgObj { get; set; }
    //    //public OrganizationStructure orgStructure { get; set; }
    //    //public OrganizationObjectConnection[] orgConnection { get; set; }
    //}
}
