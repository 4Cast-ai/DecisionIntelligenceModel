//using AutoMapper.Configuration;
using Infrastructure.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System;
using Infrastructure.Core;

namespace FormsDal.Services
{
    public class FormsDBServices : BaseService
    {
        private readonly string strTPCConnectionWithoutDB;

        public FormsDBServices()
        {
            //Logger = logger;
            strTPCConnectionWithoutDB = GeneralContext.GetConnectionString("FormsSurveyDBStart");
        }

        private FormsDynamicDBManager GetFormsSurveyDBManager(string DynamicDBConection)
        {
            return new FormsDynamicDBManager(DynamicDBConection);
        }

        public async Task<bool> CreateFormsSurveyDB(string DynamicDBName)
        {
            string dynamicDBConection = strTPCConnectionWithoutDB + DynamicDBName;
            using (FormsDynamicDBManager formsSurveyDBManager = GetFormsSurveyDBManager(dynamicDBConection))
            {
                formsSurveyDBManager.CreateDB();
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
