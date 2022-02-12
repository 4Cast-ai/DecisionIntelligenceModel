using Infrastructure;
using Infrastructure.Controllers;
using Infrastructure.Core;
using Infrastructure.Extensions;
using Model.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Spire.Xls;
using System;
using System.Collections.Generic;
//using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using ReportData = Model.Data.ReportData;

namespace Reports.Controllers
{
    [Route("api/ReportApi/[controller]")]
    [ApiController]
    public class ReportController : GeneralControllerBase
    {
        private readonly Serilog.ILogger _logger = GeneralContext.GetService<Serilog.ILogger>();

        [HttpPost("BuildCalculateTreeReports")]
        public async Task<object> BuildCalculateTreeReports(ReportData report_data)
        {
            int count = 0;
            List<string> _candidates = report_data.candidatesList;
            List<CalculateTreeData> mytrees = new List<CalculateTreeData>();

            CalculateData calcItm;
            DateTime? calc_date = null;

            if (report_data.calculated_date_list.Count > 0)
            {
                calc_date = report_data.calculated_date_list[0];
            }

            if (report_data.org_obj_list.Count > 1 && _candidates.Count == 0)
            {
                foreach (UnitDataInfo org_obj in report_data.org_obj_list)
                {
                    List<string> candidate = new List<string>();
                    var candidateGuid = _candidates.Count > 0 ? _candidates[0]?.Split("@")[0] : null;
                    if (candidateGuid != null)
                    {
                        candidate.Add(candidateGuid);
                    }

                    calcItm = new CalculateData(report_data, org_obj, calc_date, candidate);
                    CalculateTreeData tree = await BuildOneTree(calcItm);
                    mytrees.Add(tree);
                }
            }
            else if (report_data.calculated_date_list.Count > 1 && _candidates.Count == 0)
            {
                foreach (DateTime calculated_date in report_data.calculated_date_list)
                {
                    List<string> candidate = new List<string>();
                    var candidateGuid = _candidates.Count > 0 ? _candidates[0]?.Split("@")[0] : null;
                    if (candidateGuid != null)
                    {
                        candidate.Add(candidateGuid);

                    }

                    calcItm = new CalculateData(report_data, report_data.org_obj_list[0], calculated_date, candidate);
                    CalculateTreeData tree = await BuildOneTree(calcItm);
                    mytrees.Add(tree);
                }
            }
            else if (_candidates.Count > 0)
            {

                int mycase = report_data.org_obj_list.Count > 1 ? 1 : report_data.calculated_date_list.Count > 1 ? 2 : 0;

                if (mycase != 0)
                {
                    switch (mycase)
                    {
                        case 1://for Units Compare
                            foreach (UnitDataInfo org_obj in report_data.org_obj_list)
                            {
                                foreach (string candItm in _candidates)
                                {
                                    var unitLink = candItm.Split("@")[1];
                                    var candidateGuid = candItm.Split("@")[0];
                                    if (unitLink == org_obj.guid)
                                    {
                                        List<string> candidate = new List<string>();
                                        candidate.Add(candidateGuid);
                                        calcItm = new CalculateData(report_data, org_obj, calc_date, candidate);
                                        CalculateTreeData tree = await BuildOneTree(calcItm);
                                        mytrees.Add(tree);

                                        candidate.Clear();
                                        count++;
                                    }

                                }

                            }
                            break;
                        case 2://for Dates Compare
                            foreach (var calc_dateItem in report_data.calculated_date_list)
                            {

                                var candidateGuid = _candidates[0]?.Split("@")[0];

                                List<string> candidate = new List<string>();
                                candidate.Add(candidateGuid);
                                calcItm = new CalculateData(report_data, report_data.org_obj_list[0], calc_dateItem, candidate);
                                CalculateTreeData tree = await BuildOneTree(calcItm);
                                mytrees.Add(tree);

                                candidate.Clear();
                                count++;
                            }
                            break;
                    }


                }
                else
                {
                    foreach (string candItm in _candidates)
                    {
                        List<string> candidate = new List<string>();
                        var candidateGuid = candItm.Split("@")[0];
                        candidate.Add(candidateGuid);
                        calcItm = new CalculateData(report_data, report_data.org_obj_list[0], calc_date, candidate);
                        CalculateTreeData tree = await BuildOneTree(calcItm);
                        mytrees.Add(tree);

                        candidate.Clear();
                    }
                }

            }

            else
            {
                List<string> candidate = new List<string>();
                var candidateGuid = _candidates.Count > 0 ? _candidates[0]?.Split("@")[0] : null;
                if (candidateGuid != null)
                {
                    candidate.Add(candidateGuid);

                }

                calcItm = new CalculateData(report_data, report_data.org_obj_list[0], calc_date, candidate);
                CalculateTreeData tree = await BuildOneTree(calcItm);
                mytrees.Add(tree);
                candidate.Clear();

            }

            return mytrees;
        }

        [HttpPost("SaveReportDataAsJson")]
        public async Task<string> SaveReportDataAsJson(dynamic _data)
        {
            string url = "report/SaveReportDataAsJson/";
            var result = await DBGate.PostAsync<string>(url, (object)_data);
            return result;
        }

        [HttpGet("GetReportByGuid")]
        public async Task<List<string>> GetReportByGuid([FromQuery] string report_guid)
        {
            string url = $"report/GetReportByGuid?report_guid={report_guid}";
            var result = await DBGate.GetAsync<List<string>>(url);
            return result;
        }

        //[HttpGet("GetUserPrimaryReport")]
        //public async Task<string> GetUserPrimaryReport(string user_guid)
        //{
        //    string url = $"report/GetUserPrimaryReport?user_guid={user_guid}";
        //    var result = await DBGate.GetAsync<string>(url);
        //    return result;
        //}

        //[HttpGet("GetUserHybridReports")]
        //public async Task<List<string>> GetUserHybridReports(string user_guid)
        //{
        //    string url = $"report/GetUserHybridReports?user_guid={user_guid}";
        //    List<string> result = await DBGate.GetAsync<List<string>>(url);
        //    return result;
        //}


        [HttpGet("GetUserSecondaryReport")]
        public async Task<string> GetUserSecondaryReport(string user_guid)
        {
            string url = $"report/GetUserSecondaryReport?user_guid={user_guid}";
            var result = await DBGate.GetAsync<string>(url);
            return result;
        }

        [HttpGet("GetUserWatchReports")]
        public async Task<List<WatchData>> GetUserWatchReports(string user_guid)
        {
            string url = $"Report/GetUserWatchReports?user_guid={user_guid}";
            var result = await DBGate.GetAsync<List<WatchData>>(url);
            return result;
        }

        [HttpGet("GetUserReports")]
        public async Task<List<SavedReportDataInfo>> GetUserReports(string user_guid)
        {
            string url = $"report/GetUserReports?user_guid={user_guid}";
            var result = await DBGate.GetAsync<List<SavedReportDataInfo>>(url);
            return result;
        }

        //[HttpGet("GetSavedReport")]
        //public async Task<SavedReportDataInfo> GetSavedReport(string report_guid)
        //{
        //    string url = $"report/GetSavedReport?report_guid={report_guid}";
        //    var result = await DBGate.GetAsync<SavedReportDataInfo>(url);
        //    return result;
        //}

        [HttpPost("SaveReport")]
        public async Task<string> SaveReport(ReportData data)
        {
            string url = "report/SaveReport/";
            var result = await DBGate.PostAsync<string>(url, data);
            return result;
        }

        [HttpGet("ToggleReportViewType")]
        public async Task<bool> ToggleReportViewType(string report_guid, ReportView exist_report_view, int order)
        {
            string url = $"report/ToggleReportViewType?report_guid={report_guid}&exist_report_view={exist_report_view}&order={order}";
            var result = await DBGate.GetAsync<bool>(url);
            return result;
        }

        [HttpGet("DeleteSavedReport")]
        public async Task<bool> DeleteSavedReport(string report_guid)
        {
            string url = $"report/DeleteSavedReport?report_guid={report_guid}";
            var result = await DBGate.GetAsync<bool>(url);
            return result;
        }

        [HttpPost("GetOrgObjModels")]
        public async Task<List<ModelData>> GetOrgObjModels(string[] guids)
        {
            string url = "Report/GetOrgObjModels/";
            var result = await DBGate.PostAsync<List<ModelData>>(url, guids);
            return result;
        }

        int row = 3;
        [HttpPost("ExportModelToExcel")]
        public async Task<IActionResult> ExportModelToExcel([FromQuery] string filename, [FromBody] ModelComponentTreeData tree)
        {
            Workbook workbook = new Workbook();

            var measuringUnitTypes = GetMeasuringUnit();
            var calnderRollupTypes = GetCalenderRollup();
            var rollupMethodTypes = GetRollupMethod();

            #region WorkBook

            string language = GeneralContext.HttpContext.Request.Headers["Accept-Language"];
            bool isRightToLeft = !string.IsNullOrEmpty(language) && language.StartsWith("he-IL") ? true : false;
            workbook.IsRightToLeft = isRightToLeft;
            Worksheet sheet = workbook.Worksheets[0];

            sheet.Name = filename;
            sheet.PageSetup.IsSummaryRowBelow = false;
            sheet.PageSetup.CenterVertically = true;

            sheet.Range["A1"].ColumnWidth = 35;
            sheet.Range["B1"].ColumnWidth = 20;
            sheet.Range["H1"].ColumnWidth = 35;
            sheet.Range["C1"].ColumnWidth = 20;
            sheet.Range["D1"].ColumnWidth = 20;
            sheet.Range["E1"].ColumnWidth = 20;
            sheet.Range["H1"].Merge();

            sheet.Range["A1"].Style.Font.IsBold = true;
            sheet.Range["B1"].Style.Font.IsBold = true;
            sheet.Range["C1"].Style.Font.IsBold = true;
            sheet.Range["D1"].Style.Font.IsBold = true;
            sheet.Range["E1"].Style.Font.IsBold = true;
            sheet.Range["F1"].Style.Font.IsBold = true;
            sheet.Range["G1"].Style.Font.IsBold = true;
            sheet.Range["H1"].Style.Font.IsBold = true;

            sheet.Range["A1"].Text = "Center";
            sheet.Range["B1"].Text = "Center";
            sheet.Range["C1"].Text = "Center";
            sheet.Range["D1"].Text = "Center";
            sheet.Range["E1"].Text = "Center";
            sheet.Range["F1"].Text = "Center";
            sheet.Range["G1"].Text = "Center";
            sheet.Range["H1"].Text = "Center";

            //sheet.Range["A1"].Style.Color = Color.Gray;
            //sheet.Range["B1"].Style.Color = Color.Gray;
            //sheet.Range["C1"].Style.Color = Color.Gray;
            //sheet.Range["D1"].Style.Color = Color.Gray;
            //sheet.Range["E1"].Style.Color = Color.Gray;
            //sheet.Range["F1"].Style.Color = Color.Gray;
            //sheet.Range["G1"].Style.Color = Color.Gray;
            //sheet.Range["H1"].Style.Color = Color.Gray;

            sheet.Range["A1"].Value = GeneralContext.Localizer["IndexName"];
            sheet.Range["B1"].Value = GeneralContext.Localizer["Type"];
            sheet.Range["C1"].Value = GeneralContext.Localizer["CalculationRule"];
            sheet.Range["D1"].Value = GeneralContext.Localizer["MeasuringUnit"];
            sheet.Range["E1"].Value = GeneralContext.Localizer["HowCalculate"];
            sheet.Range["F1"].Value = GeneralContext.Localizer["Weight"];
            sheet.Range["G1"].Value = GeneralContext.Localizer["Source"];
            sheet.Range["H1"].Value = GeneralContext.Localizer["ProfessionalRecommendations"];

            var root = tree;

            sheet.Range["A2"].Value = root.data.name;
            sheet.Range["B2"].Value = SetSourceValue(root.data.source);
            if (rollupMethodTypes != null)
                sheet.Range["C2"].Value = rollupMethodTypes.Where(x => x.rollup_method_id == root.data.metric_rollup_method).Select(x => x.rollup_method_name).FirstOrDefault().ToString();
            sheet.Range["D2"].Value = "";
            sheet.Range["E2"].Value = "";
            sheet.Range["F2"].Value = root.data.weight.ToString();
            sheet.Range["G2"].Value = root.data.model_component_type != null ? root.data.model_component_type == 1 ? "הפניה" : "" : ""; //TODO use resources
            sheet.Range["H2"].Value = root.data.professional_instruction;

            for (int i = 1; i < 3; i++)
            {
                sheet.Range["A" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["B" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["C" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["D" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["E" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["F" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["G" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["H" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
            }

            int childsCount = 0;
            GetChildsCountModel(root, ref childsCount);
            if (childsCount > 0)
                sheet.GroupByRows(row, (row + childsCount - 1), false);

            #endregion WorkBook

            UpdateSheetModel(root, ref sheet);

            workbook.Worksheets[1].Remove();
            workbook.Worksheets[1].Remove();
            sheet.AllocatedRange.AutoFitColumns();

            filename = filename.Replace("\"", "''").Replace(":", " ").Replace(";", " ");
            string now = DateTime.Now.ToLongTimeString().Replace(":", "-");

            MemoryStream streamResult = new MemoryStream();
            workbook.Version = ExcelVersion.Version2013;
            workbook.SaveToStream(streamResult);

            var result = this.File(
                fileContents: streamResult.ToArray(),
                contentType: MediaTypeNames.Application.Octet,
                fileDownloadName: $"{filename}-{now}.xlsx"
            );
            return await Task.FromResult(result);
        }

        [HttpPost("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel([FromQuery] string filename, [FromBody] CalculateTreeData tree)
        {
            Workbook workbook = new Workbook();

            var measuringUnitTypes = GetMeasuringUnit();

            #region WorkBook

            string language = GeneralContext.HttpContext.Request.Headers["Accept-Language"];
            bool isRightToLeft = !string.IsNullOrEmpty(language) && language.StartsWith("he-IL") ? true : false;
            workbook.IsRightToLeft = isRightToLeft;

            Worksheet sheet = workbook.Worksheets[0];
            sheet.Name = filename.Length > 32 ? filename.Substring(0, 31) : filename;

            sheet.PageSetup.IsSummaryRowBelow = false;

            sheet.Range["A1"].ColumnWidth = 35;
            sheet.Range["B1"].ColumnWidth = 20;

            sheet.Range["A1"].Style.Font.IsBold = true;
            sheet.Range["B1"].Style.Font.IsBold = true;
            sheet.Range["C1"].Style.Font.IsBold = true;
            sheet.Range["D1"].Style.Font.IsBold = true;
            sheet.Range["E1"].Style.Font.IsBold = true;


            //sheet.Range["A1"].Style.Color = Color.Gray;
            //sheet.Range["B1"].Style.Color = Color.Gray;
            //sheet.Range["C1"].Style.Color = Color.Gray;
            //sheet.Range["D1"].Style.Color = Color.Gray;
            //sheet.Range["E1"].Style.Color = Color.Gray;

            for (int i = 1; i < 3; i++)
            {
                sheet.Range["A" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["B" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["C" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["D" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
                sheet.Range["E" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
            }

            sheet.Range["A1"].Value = GeneralContext.Localizer["IndexName"];
            sheet.Range["B1"].Value = GeneralContext.Localizer["Type"];
            sheet.Range["C1"].Value = GeneralContext.Localizer["Source"];
            sheet.Range["D1"].Value = GeneralContext.Localizer["Weight"];
            sheet.Range["E1"].Value = GeneralContext.Localizer["Score"];

            var root = tree;

            sheet.Range["A2"].Value = root.data.model_data.name;
            if (root.data.model_data.metric_measuring_unit != null)
            {
                sheet.Range["B2"].Value = measuringUnitTypes.Where(x => x.measuring_unit_id == root.data.model_data.metric_measuring_unit).Select(x => x.measuring_unit_name).FirstOrDefault();
            }
            else
            {
                sheet.Range["B2"].Value = SetSourceValue(root.data.model_data.source);
            }

            sheet.Range["C2"].Value = root.data.reference_score_list != null ? root.data.reference_score_list.Count() > 0 ? "הפניה" : "" : ""; //TODO use resources
            sheet.Range["D2"].Value = root.data.model_data.weight.ToString();
            sheet.Range["E2"].Value = root.data.score_data[0].calculated_score.ToString();

            int childsCount = 0;
            GetChildsCount(root, ref childsCount);
            if (childsCount > 0)
                sheet.GroupByRows(row, (row + childsCount - 1), false);

            #endregion WorkBook

            UpdateSheet(root, ref sheet);

            workbook.Worksheets[1].Remove();
            workbook.Worksheets[1].Remove();
            sheet.AllocatedRange.AutoFitColumns();

            filename = filename.Replace("\"", "''").Replace(":", " ").Replace(";", " ");
            string now = DateTime.Now.ToLongTimeString().Replace(":", "-");

            MemoryStream streamResult = new MemoryStream();
            workbook.Version = ExcelVersion.Version2013;
            workbook.SaveToStream(streamResult);

            var result = this.File(
                fileContents: streamResult.ToArray(),
                contentType: MediaTypeNames.Application.Octet,
                fileDownloadName: $"{filename}-{now}.xlsx"
            );
            return await Task.FromResult(result);
        }

        //[HttpPost("ExportUnitTreeToExcel")]
        //public async Task<IActionResult> ExportUnitTreeToExcel([FromQuery] string filename, [FromBody] OrganizationObjectData tree)
        //{
        //    Workbook workbook = new Workbook();

        //    #region WorkBook

        //    string language = GeneralContext.HttpContext.Request.Headers["Accept-Language"];
        //    bool isRightToLeft = !string.IsNullOrEmpty(language) && language.StartsWith("he-IL") ? true : false;
        //    workbook.IsRightToLeft = isRightToLeft;

        //    Worksheet sheet = workbook.Worksheets[0];
        //    sheet.Name = filename;

        //    sheet.PageSetup.IsSummaryRowBelow = false;

        //    sheet.Range["A1"].ColumnWidth = 35;
        //    sheet.Range["B1"].ColumnWidth = 20;

        //    sheet.Range["A1"].Style.Font.IsBold = true;
        //    sheet.Range["B1"].Style.Font.IsBold = true;

        //    //sheet.Range["A1"].Style.Color = Color.Gray;
        //    //sheet.Range["B1"].Style.Color = Color.Gray;

        //    for (int i = 1; i < 3; i++)
        //    {
        //        sheet.Range["A" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
        //        sheet.Range["B" + i].Style.HorizontalAlignment = HorizontalAlignType.Center;
        //    }

        //    sheet.Range["A1"].Value = "UnitName";
        //    sheet.Range["B1"].Value = "UnitGuid";

        //    var root = tree;

        //    sheet.Range["A2"].Value = root.name;
        //    sheet.Range["B2"].Value = root.guid;

        //    int childsCount = 0;
        //    GetChildsCount(root, ref childsCount);
        //    if (childsCount > 0)
        //        sheet.GroupByRows(row, (row + childsCount - 1), false);

        //    #endregion WorkBook

        //    UpdateSheet(root, ref sheet);

        //    workbook.Worksheets[1].Remove();
        //    workbook.Worksheets[1].Remove();
        //    sheet.AllocatedRange.AutoFitColumns();

        //    filename = filename.Replace("\"", "''").Replace(":", " ").Replace(";", " ");
        //    string now = DateTime.Now.ToLongTimeString().Replace(":", "-");

        //    MemoryStream streamResult = new MemoryStream();
        //    workbook.Version = ExcelVersion.Version2013;
        //    workbook.SaveToStream(streamResult);

        //    var result = this.File(
        //        fileContents: streamResult.ToArray(),
        //        contentType: MediaTypeNames.Application.Octet,
        //        fileDownloadName: $"{filename}-{now}.xlsx"
        //    );
        //    return await Task.FromResult(result);
        //}


        [HttpGet("WMSCapabilitiesRetrieve")]
        public async Task<WMSCapabilities> WMSCapabilitiesRetrieve([FromQuery] string WMSGeoserverUrl)
        {
            WMSCapabilities Capabilities = new WMSCapabilities();
            Capabilities.Layers = new List<CustomImageInfo>();
            if (string.IsNullOrEmpty(WMSGeoserverUrl))
            {
                return null;
            }
            string result = string.Empty;
            string BaseUrlFormat = "{0}://{1}:{2}/";
            var client = new HttpClient();
            client.Timeout = new TimeSpan(0, 0, 15);
            HttpResponseMessage response = null;
            try
            {
                Uri u = new Uri(WMSGeoserverUrl);
                string BaseUrl = string.Format(CultureInfo.InvariantCulture, BaseUrlFormat, u.Scheme, u.Host, u.Port);
                string ReqUrl = u.PathAndQuery + "?request=getCapabilities";
                client.BaseAddress = new Uri(BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
                response = await client.GetAsync(ReqUrl);
                if (response.IsSuccessStatusCode == false) return null;
                using (HttpContent content = response.Content)
                {
                    result = await content.ReadAsStringAsync();
                }
                client.Dispose();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(result);
                XmlNodeList CapabilitiesNodes = doc.GetElementsByTagName("WMS_Capabilities");
                XmlNode CapabilitiesNode = CapabilitiesNodes[0];
                Capabilities.Version = CapabilitiesNode.Attributes["version"].Value;
                XmlNodeList Layers = doc.GetElementsByTagName("Layer");
                foreach (XmlNode layer in Layers)
                {
                    foreach (XmlNode ch in layer.ChildNodes)
                    {
                        if (ch.Name == "Layer")
                        {
                            CustomImageInfo ImageInfo = new CustomImageInfo();
                            foreach (XmlNode l in ch.ChildNodes)
                            {
                                if (l.Name == "Name")
                                {
                                    ImageInfo.MapName = l.InnerText;
                                }
                                if (l.Name == "EX_GeographicBoundingBox")
                                {
                                    foreach (XmlNode b in l.ChildNodes)
                                    {
                                        if (b.Name == "westBoundLongitude")
                                        {
                                            double.TryParse(b.InnerText, out ImageInfo.MinX);
                                        }
                                        else if (b.Name == "eastBoundLongitude")
                                        {
                                            double.TryParse(b.InnerText, out ImageInfo.MaxX);
                                        }
                                        else if (b.Name == "southBoundLatitude")
                                        {
                                            double.TryParse(b.InnerText, out ImageInfo.MinY);
                                        }
                                        else if (b.Name == "northBoundLatitude")
                                        {
                                            double.TryParse(b.InnerText, out ImageInfo.MaxY);
                                        }
                                    }
                                }
                            }
                            Capabilities.Layers.Add(ImageInfo);
                        }
                    }
                }
                return Capabilities;
            }
            catch (Exception ex)
            {
                Capabilities.Error = ex;
                return Capabilities;
            }
        }


        #region Private methods

        private void UpdateSheetModel(ModelComponentTreeData root, ref Worksheet sheet)
        {
            var measuringUnitTypes = GetMeasuringUnit();
            var calnderRollupTypes = GetCalenderRollup();
            var rollupMethodTypes = GetRollupMethod();

            if (root.children != null && root.children.Count > 0)
            {
                foreach (var child in root.children)
                {
                    sheet.Range["A" + row].Value = child.data.name;
                    sheet.Range["B" + row].Value = SetSourceValue(child.data.source);

                    if (measuringUnitTypes != null)
                        if (child.data.source == 5)
                        {
                            sheet.Range["D" + row].Value = measuringUnitTypes.Where(x => x.measuring_unit_id == child.data.metric_measuring_unit).FirstOrDefault()?.measuring_unit_name;
                            sheet.Range["E" + row].Value = calnderRollupTypes.Where(x => x.calender_rollup_id == child.data.metric_calender_rollup).Select(x => x.calender_rollup_name).FirstOrDefault()?.ToString();
                        }
                        else if ((child.data.source == 7 || child.data.source == 6) && rollupMethodTypes != null)
                        {
                            sheet.Range["C" + row].Value = rollupMethodTypes.Where(x => x.rollup_method_id == child.data.metric_rollup_method).Select(x => x.rollup_method_name).FirstOrDefault().ToString();
                        }

                    sheet.Range["G" + row].Value = child.data.model_component_type != null ? child.data.model_component_type == 1 ? "הפניה" : "" : ""; //TODO use resources
                    sheet.Range["F" + row].Value = child.data.weight.ToString();
                    sheet.Range["H" + row].Value = child.data.professional_instruction;

                    sheet.Range["A" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["B" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["C" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["D" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["E" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["F" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["G" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["H" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;

                    int childsCount = 0;
                    GetChildsCountModel(child, ref childsCount);
                    if (childsCount > 0)
                        sheet.GroupByRows(row + 1, row + childsCount, false);

                    row += 1;
                    UpdateSheetModel(child, ref sheet);
                }
            }
        }

        private void UpdateSheet(CalculateTreeData root, ref Worksheet sheet)
        {
            var measuringUnitTypes = GetMeasuringUnit();

            if (root.children != null && root.children.Count > 0)
            {
                foreach (var child in root.children)
                {
                    sheet.Range["A" + row].Value = child.data.model_data.name;
                    if (child.data.model_data.source == 5)//if its metric
                    {
                        sheet.Range["B" + row].Value = measuringUnitTypes.Where(x => x.measuring_unit_id == child.data.model_data.metric_measuring_unit).FirstOrDefault()?.measuring_unit_name;
                    }
                    else
                    {
                        sheet.Range["B" + row].Value = SetSourceValue(child.data.model_data.source);
                    }

                    sheet.Range["C" + row].Value = child.data.reference_score_list != null ? child.data.reference_score_list.Count() > 0 ? "הפניה" : "" : ""; //TODO use resources
                    sheet.Range["D" + row].Value = child.data.model_data.weight.ToString();
                    sheet.Range["E" + row].Value = child.data.score_data[0].calculated_score.ToString();

                    sheet.Range["A" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["B" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["C" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["D" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                    sheet.Range["E" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;

                    int childsCount = 0;
                    GetChildsCount(child, ref childsCount);
                    if (childsCount > 0)
                        sheet.GroupByRows(row + 1, row + childsCount, false);

                    row += 1;
                    UpdateSheet(child, ref sheet);
                }
            }
        }

        //private void UpdateSheet(OrganizationObjectData root, ref Worksheet sheet)
        //{
        //    if (root.children != null && root.children.Count > 0)
        //    {
        //        foreach (var child in root.children)
        //        {
        //            sheet.Range["A" + row].Value = child.name;
        //            sheet.Range["B" + row].Value = child.guid;
                    
        //            sheet.Range["A" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
        //            sheet.Range["B" + row].Style.HorizontalAlignment = HorizontalAlignType.Center;
                   
        //            int childsCount = 0;
        //            GetChildsCount(child, ref childsCount);
        //            if (childsCount > 0)
        //                sheet.GroupByRows(row + 1, row + childsCount, false);

        //            row += 1;
        //            UpdateSheet(child, ref sheet);
        //        }
        //    }
        //}

        private void GetChildsCountModel(ModelComponentTreeData root, ref int count)
        {
            if (root.children != null)
            {
                foreach (var child in root.children)
                {
                    count += 1;
                    GetChildsCountModel(child, ref count);
                }
            }

        }

        private void GetChildsCount(CalculateTreeData root, ref int count)
        {
            foreach (var child in root.children)
            {
                count += 1;
                GetChildsCount(child, ref count);
            }
        }

        //private void GetChildsCount(OrganizationObjectData root, ref int count)
        //{
        //    foreach (var child in root.children)
        //    {
        //        count += 1;
        //        GetChildsCount(child, ref count);
        //    }
        //}

        private string SetSourceValue(int _source)
        {
            switch (_source)
            {
                case 5:
                    return "מדד";
                case 6:
                    return "מודל בסיס";
                case 7:
                    return "מודל מבצעי";
                default:
                    return null;

            }
        }

        private List<MeasuringUnitData> GetMeasuringUnit()
        {
            List<MeasuringUnitData> results = new List<MeasuringUnitData>();
            GeneralContext.Cache.TryGetValue(Constants.MeasuringUnitT, out results);

            if (results == null)
            {
                results = DBGate.Get<List<MeasuringUnitData>>("General/GetMeasuring");
                GeneralContext.Cache.Set(Constants.MeasuringUnitT, results, GeneralContext.CacheEntryOptions);
            }

            return results;
        }

        private List<CalenderRollupData> GetCalenderRollup()
        {
            List<CalenderRollupData> results = new List<CalenderRollupData>();
            GeneralContext.Cache.TryGetValue(Constants.CalenderRollupT, out results);

            if (results == null)
            {
                results = DBGate.Get<List<CalenderRollupData>>("General/GetCalender_Rollup");
                GeneralContext.Cache.Set(Constants.CalenderRollupT, results, GeneralContext.CacheEntryOptions);
            }

            return results;
        }

        private List<RollupMethodInfo> GetRollupMethod()
        {
            List<RollupMethodInfo> results = new List<RollupMethodInfo>();
            GeneralContext.Cache.TryGetValue(Constants.RollupMethodT, out results);

            if (results == null)
            {
                results = DBGate.Get<List<RollupMethodInfo>>("General/GetRollup_Method");
                GeneralContext.Cache.Set(Constants.RollupMethodT, results, GeneralContext.CacheEntryOptions);
            }

            return results;
        }

        private async Task<CalculateTreeData> BuildOneTree(CalculateData calcItm)
        {
            bool outSoureFlag = false;
            if (calcItm.candidatesList != null && calcItm.candidatesList.Count > 0)
            {
                outSoureFlag = true;
            }

            if (calcItm.org_obj.name == null)
            {
                calcItm.org_obj.name = await DBGate.GetAsync<string>("organization/GetOrgName?orgObjGuid=" + calcItm.org_obj.guid);
            }

            Dictionary<string, CalculateTreeData> flatModel = await DBGate.GetAsync<Dictionary<string, CalculateTreeData>>("model/GetFlatModel?model_component_guid=" + calcItm.model_component_guid + "&org_obj_guid=" + calcItm.org_obj.guid);

            //build and calc childs
            await BuildAndCalcRef(flatModel, calcItm);

            //build tree for calc with metric score
            Dictionary<string, CalculateTreeData> modelWithScores = await DBGate.PostStreamAsync<Dictionary<string, CalculateTreeData>>("report/FillTreeScores/", 
                new { item1 = calcItm, item2 = flatModel }, CustomMediaTypeNames.FormDataCompress);

            if (modelWithScores == null)
                return null;

            //calc model score
            //CalculateTreeData calcTree = await CalcApi.PostStreamAsync<CalculateTreeData>("RunCalculateReport2/", new { item1 = outSoureFlag, item2 = firstTree });
            CalculateTreeData calcTree = await CalcApi.PostStreamAsync<CalculateTreeData>("RunCalc/", new { item1 = outSoureFlag, item2 = modelWithScores }, CustomMediaTypeNames.FormDataCompress);

            if (calcTree == null)
                return null;
            else
                calcTree.candidateID = calcItm.candidateID;

            //update tree data
            CalculateTreeData updateTree = await DBGate.PostStreamAsync<CalculateTreeData>("report/UpdateCalculateTree/", new { Tree = calcTree, Data = calcItm }, CustomMediaTypeNames.FormDataCompress);

            if (updateTree == null)
                return null;

            _logger.Information("Start Task SaveCalculateTree");
            //save to Calculated_Score
            Task.Run(() => DBGate.PostStreamAsync<bool>("report/SaveCalculateTree/",new { Tree = updateTree }, CustomMediaTypeNames.FormDataCompress));
            //await DBGate.PostStreamAsync<bool>("report/SaveCalculateTree/", new { Tree = updateTree }, CustomMediaTypeNames.FormDataCompress);

            //try to wait after run task for linux
            Task.Delay(2000).Wait();
            _logger.Information("End Task SaveCalculateTree");

            return updateTree;
        }

        private static readonly object lockObject = new object();
        private async Task BuildAndCalcRef(Dictionary<string, CalculateTreeData> flatModel, CalculateData calcItm)
        {
            var flatM = new List<string>();
            Dictionary<string, List<OrgModels>> orgModels = await DBGate.GetAsync<Dictionary<string, List<OrgModels>>>("organization/GetChildOrgModels?org_obj_guid=" + calcItm.org_obj.guid + "&unionGuid=" + calcItm.org_obj.union_guid);
            Dictionary<string, List<CalculateTreeData>> dictOrgSavedTree = new Dictionary<string, List<CalculateTreeData>>();

            Dictionary<string, CalculateTreeData> copyFlatModel = new Dictionary<string, CalculateTreeData>();

            foreach (var elt in flatModel)
            {
                copyFlatModel.Add(elt.Key, (CalculateTreeData)elt.Value.Clone());
            }

            //build and calc childs
            foreach (var node in flatModel)
            {
                if (node.Value.data.model_data.is_reference)
                {
                    //get org child
                    List<UnitDataInfo> org_obj_children = await GetOrgByModel(orgModels, calcItm.org_obj.guid, node.Value.data.model_data.base_origin_model_component_guid);

                    List<OrgObjScoreData> ref_scores = new List<OrgObjScoreData>();
                    if (org_obj_children.Count > 0)
                    {
                        double sum = 0;
                        int count = org_obj_children.Count;

                        //foreach (var org in org_obj_children)
                        Task task = Task.Run(() =>
                        {
                            Parallel.ForEach(org_obj_children, // source collection
                                         () => 0, // method to initialize the local variable
                                         (org, loop, sum) => // method invoked by the loop on each iteration
                                         {
                            CalculateTreeData calc_t = null;
                            CalculateTreeData exist_calc_t = GetExistingRefCalc(dictOrgSavedTree, node.Key, org.guid);
                            if (exist_calc_t == null)
                            {
                                CalculateData refCalcItm = new CalculateData(calcItm);
                                refCalcItm.org_obj = org;
                                refCalcItm.org_obj.union_guid = calcItm.org_obj.union_guid;
                                refCalcItm.model_component_guid = node.Key;

                                    //build tree ref    
                                    Dictionary<string, CalculateTreeData> flatRefModel = DBGate.PostStreamAsync<Dictionary<string, CalculateTreeData>>("report/FillTreeScoresRef/", new { refCalcItm, orgModels, copyFlatModel }, CustomMediaTypeNames.FormDataCompress).GetAwaiter().GetResult();

                                    //calc tree ref
                                    //calc_t = CalcApi.PostStreamAsync<CalculateTreeData>("RunCalculateReportRef2", new { treeRef }, CustomMediaTypeNames.FormDataCompress).GetAwaiter().GetResult();
                                    calc_t = CalcApi.PostStreamAsync<CalculateTreeData>("RunCalcRef", new { flatRefModel }, CustomMediaTypeNames.FormDataCompress).GetAwaiter().GetResult();
                                    //calc_t = DBGate.PostStreamAsync<CalculateTreeData>("report/UpdateCalculateTree/", new { Tree = calc_t_1, Data = refCalcItm }, CustomMediaTypeNames.FormDataCompress).GetAwaiter().GetResult(); ;

                                    if (calc_t != null)
                                {
                                    lock (dictOrgSavedTree)
                                    {
                                        if (dictOrgSavedTree.ContainsKey(org.guid))
                                        {
                                            dictOrgSavedTree[org.guid].Add(calc_t);
                                        }
                                        else
                                        {
                                            List<CalculateTreeData> orgTrees = new List<CalculateTreeData>();
                                            orgTrees.Add(calc_t);
                                            dictOrgSavedTree.Add(org.guid, orgTrees);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                calc_t = exist_calc_t;
                            }

                            if (calc_t.data.score_data[0].calculated_score != -1 && calc_t.data.score_data[0].calculated_score != -2)
                            {
                                lock (lockObject)
                                {
                                    sum += calc_t.data.score_data[0].calculated_score.HasValue ? Convert.ToInt32(calc_t.data.score_data[0].calculated_score.Value) : 0;
                                }
                            }
                            else
                            {
                                lock (lockObject)
                                {
                                    count -= 1;
                                }
                            }

                            lock (ref_scores)
                            {
                                CalculateTreeData copyTree = (CalculateTreeData)calc_t.Clone();
                                copyTree.data.score_data = new List<ScoreData>();
                                foreach (var itemS in calc_t.data.score_data)
                                {
                                    copyTree.data.score_data.Add(new ScoreData(itemS));
                                }
                                ref_scores.Add(new OrgObjScoreData(org.parent_guid, org.guid, org.name, org.order.HasValue ? org.order.Value : 0, copyTree.data.score_data[0].calculated_score, 0, copyTree));
                            }

                            if (calc_t.data.score_data.Count > 0)
                            {
                                lock (node.Value.data.score_data)
                                {
                                    node.Value.data.score_data.AddRange(calc_t.data.score_data.GetRange(0, calc_t.data.score_data.Count));
                                }
                            }

                            return sum; // value to be passed to next iteration //subtotal
                        },
                                     // Method to be executed when each partition has completed.
                                     // finalResult is the final value of subtotal for a particular partition.
                                     (finalResult) =>
                                     {
                                         InterlockedDoubleAdd(ref sum, finalResult);
                                     }
                                     );
                        });

                        await task;

                        node.Value.data.reference_score_list = ref_scores.OrderBy(rs => rs.order).ToList();

                        //if (node.Value.data.reference_score_list != null)
                        //{
                        //    foreach (var refS in node.Value.data.reference_score_list)
                        //    {
                        //        if (refS.tree != null)
                        //        {
                        //            foreach (var r in refS.tree.data.model_data.threshold_list)
                        //            {
                        //                if (r.IsActivated)
                        //                {
                        //                    r.FreeMessage = r.FreeMessage + "-" + r.org_obj_name;
                        //                    node.Value.data.model_data.threshold_list.Add(r);
                        //                }
                        //            }
                        //        }
                        //    }
                        //}

                        double? score = sum;

                        if (!node.Value.data.model_data.calcAsSum.HasValue || !node.Value.data.model_data.calcAsSum.Value)
                        {
                            score /= count;
                        }

                        if (count == 0)
                            score = -1;

                        node.Value.data.score_data[0].original_score = node.Value.data.score_data[0].convertion_score = node.Value.data.score_data[0].calculated_score = score;
                    }
                    else
                    {
                        //no score
                        if (node.Value.data.score_data.Count > 0)
                            node.Value.data.score_data[0].original_score = node.Value.data.score_data[0].convertion_score = node.Value.data.score_data[0].calculated_score = -1;

                        node.Value.data.model_data.score_level = 0;
                    }
                }
            }
        }

        private async Task<List<UnitDataInfo>> GetOrgByModel(Dictionary<string, List<OrgModels>> orgModelsDict, string orgObjGuid, string modelComponentGuid)
        {
            try
            {
                List<OrgModels> orgModels;
                List<UnitDataInfo> data = null;
                orgModelsDict.TryGetValue(orgObjGuid, out orgModels);

                if (orgModels != null && orgModels.Count > 0)
                {
                    data = orgModels.Where(om => om.ModelComponentList.Contains(modelComponentGuid))
                                        .Select(om => new UnitDataInfo() { parent_guid = om.OrgObjParentGuid, guid = om.OrgObjGuid, name = om.OrgObjName, order = om.OrgOrder, org_type = om.OrgObjType }).ToList();
                }

                return await Task.FromResult(data);
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        private CalculateTreeData GetExistingRefCalc(Dictionary<string, List<CalculateTreeData>> dictOrgSavedTree, string modelComponentGuid, string orgGuid)
        {
            CalculateTreeData res;
            List<CalculateTreeData> orgTrees;
            dictOrgSavedTree.TryGetValue(orgGuid, out orgTrees);

            if (orgTrees != null && orgTrees.Count > 0)
            {
                foreach (var tree in orgTrees)
                {
                    res = tree.GetNodeAndDescendants().FirstOrDefault(node => node.data.model_data.model_component_guid == modelComponentGuid);
                    if (res != null)
                        return res;
                }
            }

            return null;
        }

        private double InterlockedDoubleAdd(ref double location1, double value)
        {
            double newCurrentValue = location1; // non-volatile read, so may be stale
            while (true)
            {
                double currentValue = newCurrentValue;
                double newValue = currentValue + value;
                newCurrentValue = Interlocked.CompareExchange(ref location1, newValue, currentValue);
                if (newCurrentValue == currentValue)
                    return newValue;
            }
        }

        #endregion Private methods

        #region InterFace
        [HttpPost("GetCandidateByOrgGuid")]
        public async Task<List<UserDetails>> GetCandidateByOrgGuid([FromBody] object org_obj_guid_list)
        {
            var results = await DBGate.PostAsync<List<UserDetails>>("Report/GetCandidateByOrgGuid", org_obj_guid_list);
            return results;
        }

        [HttpPost("GetOrgObjModelsByUserHR")]
        public async Task<List<ModelData>> GetOrgObjModelsByUserHR([FromBody] object candidates)
        {
            var results = await DBGate.PostAsync<List<ModelData>>("Report/GetOrgObjModelsByUserHR", candidates);
            return results;
        }

        #endregion
    }
}
