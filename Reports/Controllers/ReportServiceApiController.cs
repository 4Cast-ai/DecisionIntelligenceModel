using System;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Controllers;
using Infrastructure.Extensions;

namespace Reports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportServiceApiController : GeneralControllerBase
    {
        [HttpGet("GetFromDB")]
        public DataTable GetFromDB(string functionName,
            string FormGuid = null, string UnitGuid = null, string ModleGuid = null,
            string MetricGuid = null, string dtWhen = null)
        {
            DataSet ds = new DataSet();
            DataTable dt;

            string url = "";

            switch (functionName)
            {
                case "Units":
                case "Thresholds":
                case "Thresholds_reference":
                case "Metric_relations":
                case "Convertion_tables":
                    {
                        url += "GetDataTableByName?tableName=" + functionName;
                        break;
                    }
                case "GetFormRelevantModels":
                case "GetFormScoresHistoryByFormGuid":
                    {
                        url += functionName + "?FormGuid=" + FormGuid;
                        break;
                    }
                case "GetMetricHistory":
                    {
                        url += functionName + "?UnitGuid=" + UnitGuid + "&ModleGuid=" + ModleGuid + "&MetricGuid=" + MetricGuid + "&dtWhen=" + dtWhen;
                        break;
                    }
                default:
                    {
                        url += functionName;
                        break;
                    }
            }

            dt = DBGate.Get<DataTable>(url);
            ds.Tables.Add(dt);

            if (ds.Tables[0].Rows.Count == 0)
            {
                DataRow Dr = ds.Tables[0].NewRow();

                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    var columnType = ds.Tables[0].Columns[i].DataType.Name.ToString();
                    var propertyid = ds.Tables[0].Columns[i].ToString();

                    switch (columnType)
                    {
                        case "String":
                            Dr[propertyid] = "stam";
                            break;
                        case "Int32":
                            Dr[propertyid] = 199;
                            break;
                        case "Boolean":
                            Dr[propertyid] = false;
                            break;
                        case "Double":
                            Dr[propertyid] = 199.9;
                            break;
                        case "DateTime":
                            Dr[propertyid] = DateTime.Now;
                            break;

                        default:
                            break;
                    }
                }

                ds.Tables[0].Rows.Add(Dr);
            }

            return ds.Tables[0];
        }

        [HttpGet("InsertToDataTable")]
        public void InsertToDataTable(string functionName)
        {
            DBGate.Get<object>("" + functionName);
        }

        [HttpGet("DeleteFromDataTable")]
        public void DeleteFromDataTable(string functionName,
            string FormGuid = null, string UnitGuid = null, string ModelGuid = null,
            string MetricGuid = null, string dtWhen = null, string ThresholdGuid = null,
            string ThresholdAffectedMetricGuid = null, string UnitGuidAffecting = null)
        {
            string url = "";

            switch (functionName)
            {
                case "DeleteFormScoresByFormGuid":
                case "DeleteFormScoresHistoryByFormGuid":
                    {
                        url += functionName + "?FormGuid=" + FormGuid;
                        break;
                    }
                case "DeleteMetricHistory":
                case "DeleteStrengthWeakness":
                case "DeleteMetricHistoryReferenceByRangeDate":
                    {
                        url += functionName + "?UnitGuid=" + UnitGuid + "&ModelGuid=" + ModelGuid + "&dtWhen=" + dtWhen + "&MetricGuid=" + MetricGuid;
                        break;
                    }
                case "DeleteMetricHistoryReference":
                    {
                        url += functionName + "?UnitGuid=" + UnitGuid + "&ModelGuid=" + ModelGuid + "&dtWhen=" + dtWhen;
                        break;
                    }
                case "DeleteThresholdHistory":
                    {
                        url += functionName + "?UnitGuid=" + UnitGuid + "&ModelGuid=" + ModelGuid + "&dtWhen=" + dtWhen + "&ThresholdGuid=" + ThresholdGuid +
                            "&ThresholdAffectedMetricGuid=" + ThresholdAffectedMetricGuid + "&UnitGuidAffecting=" + UnitGuidAffecting;
                        break;
                    }
                default:
                    {
                        url += functionName;
                        break;
                    }
            }

            DBGate.Get<object>(url);
        }
    }
}
