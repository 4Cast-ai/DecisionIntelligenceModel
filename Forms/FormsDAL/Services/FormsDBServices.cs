//using AutoMapper.Configuration;
using Infrastructure.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using Infrastructure.Core;
using Model.Data;
using Model.Entities;
using Infrastructure.Helpers;

namespace FormsDal.Services
{
    public class FormsDBServices : BaseService
    {
        private readonly string strTPCConnectionWithoutDB;
        private readonly string strTPCConnectionForManager;

        public FormsDBServices()
        {
            //Logger = logger;
            strTPCConnectionWithoutDB = GeneralContext.GetConnectionString("FormsSurveyDBStart");
            strTPCConnectionForManager = GeneralContext.GetConnectionString("FormsManageDB");
        }

        private FormsDynamicDBManager GetFormsSurveyDBManager(string DynamicDBConection)
        {
            return new FormsDynamicDBManager(DynamicDBConection);
        }

        private FormsManageDBManager GetManagerDBManager(string connection)
        {
            return new FormsManageDBManager(connection);
        }

        public async Task<bool> CreateDynamicDB(string DynamicDBName)
        {
            string dynamicDBConection = strTPCConnectionWithoutDB + DynamicDBName;
            using (FormsDynamicDBManager formsSurveyDBManager = GetFormsSurveyDBManager(dynamicDBConection))
            {
                formsSurveyDBManager.CreateDB();
            }
            return true;
        }

        private FormsActivityTrace SetNewActivity(FormsDataObject eventData, int FormsDBID, string FormDBName)
        {
            DateTime todayDate = DateTime.Now;

            string todayDateStr = Util.ConvertDateToString(todayDate);

            FormsActivityTrace formsActivityTrace = new FormsActivityTrace
            {
                ActivityGuid = eventData.ActivityGuid,
                ActivityName = eventData.ActivityName,
                ActivityEndDate = eventData.EndDate,
                ActivityStartDate = eventData.StartDate,
                RecordStatusCode = 1, //TODO: Maya  להחליף לenum 
                CanSubmitOnce = eventData.CanSubmitOnce,
                IsAnonymous = eventData.IsAnonymous,
                IsLimited = eventData.IsLimited,
                FromEffectDate = todayDateStr, //TODO: set the correct field
                ToEffectDate = todayDateStr, //TODO: set the correct field
                CreationDate = todayDateStr,
                UpdateDate = todayDateStr,
                UpdateUserId = null,
                EvaluatedAndEvaluators = null, //TODO: set the correct field
                Forms = null,   //TODO: set the correct field
                FormsDBID = FormsDBID,
                FormsDBName = FormDBName
            };
            return formsActivityTrace;
        }

        public async Task<bool> CreateEvent(FormsDataObject eventData)
        {
            bool isActivityExist = false;
            using (FormsManageDBManager formsManageDBManager = GetManagerDBManager(strTPCConnectionForManager))
            {
                isActivityExist = formsManageDBManager.IsActivityExist(eventData.ActivityGuid);
            }

            if (!isActivityExist)
            {
                FormsActivityTrace formsActivity = SetNewActivity(eventData, 1, "ChangeMe");

                using (FormsManageDBManager formsManageDBManager = GetManagerDBManager(strTPCConnectionForManager))
                {
                    decimal activityKey = formsManageDBManager.SaveActivity(formsActivity);
                }

                string dynamicDBConection = strTPCConnectionWithoutDB + eventData.ActivityName;
                using (FormsDynamicDBManager formsSurveyDBManager = GetFormsSurveyDBManager(dynamicDBConection))
                {
                    formsSurveyDBManager.CreateDB();
                }
            }
           
            return true;
        }

        //public string AddScenario(ScenarioDto scenario)
        //{
        //    if (string.IsNullOrEmpty(scenario.name) == true)
        //    {
        //        Exception ex = new Exception("Scenario Name Must be Filled!");
        //        throw ex;
        //    }

        //    lock (syncScenariosObject)
        //    {
        //        long NewScenarioID = GetNewScenarioID();
        //        string db_nameNew = dbPrefix + NewScenarioID.ToString();

        //        using (TPC.DAL.DataAccessScenarioDB ScenarioDBNew = GetScenarioDB(db_nameNew))
        //        using (TPC.DAL.DataAccessManageDB ManageDB = GetManageDB())
        //        using (var transactionManage = ManageDB.contextDB.Database.BeginTransaction())
        //        //using (var transactionScenario = ScenarioDBNew.contextDB.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                ScenarioDBNew.CreateDB();

        //                TPC.DAL.ManageModels.scenario scenarioNew = new TPC.DAL.ManageModels.scenario();
        //                scenarioNew.id = (int)NewScenarioID;
        //                scenarioNew.name = scenario.name;
        //                scenarioNew.description = scenario.description;
        //                scenarioNew.guid = System.Guid.NewGuid().ToString();
        //                scenarioNew.parentguid = scenario.parentguid;
        //                scenarioNew.starttime = ConvertDateToString14(scenario.starttime);
        //                scenarioNew.htime = ConvertDateToString14(scenario.htime);
        //                scenarioNew.endtime = ConvertDateToString14(scenario.endtime);

        //                ManageDB.Add<TPC.DAL.ManageModels.scenario>(scenarioNew);
        //                //transactionScenario.Commit();
        //                transactionManage.Commit();

        //                return scenarioNew.guid;
        //            }
        //            catch (Exception ex)
        //            {
        //                //transactionScenario.Rollback();
        //                transactionManage.Rollback();
        //                throw;
        //            }
        //        }

        //    }
        //}

    }
}
