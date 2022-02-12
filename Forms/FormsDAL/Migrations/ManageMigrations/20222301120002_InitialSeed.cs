//using Infrastructure.Migrations;
using Model.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using FormsDal.Contexts;
using Infrastructure.Core;
using Infrastructure.Interfaces;

namespace Model.Migrations
{
    [DbContext(typeof(FormsManageDBContext))]
    [Migration("20222301120002_InitialSeed")]
    public partial class InitialSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string defaultCulture = GeneralContext.GetConfig<IAppConfig>().DefaultLocale;
            if (defaultCulture == "en")
            {
                var table = "FormsRecordStatus";
                var columns = new[] { "RecordStatusCode", "RecordStatusName" };
                var colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "Active" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "Close" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "Update" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "Cancel" });

                table = "FormsEvaluatedType";
                columns = new[] { "EvaluatedTypeCode", "EvaluatedTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "Department" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "Person" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "Learning item" });

                table = "FormsEvaluatorType";
                columns = new[] { "EvaluatorTypeCode", "EvaluatorTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "Identify" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "Anonymous" });

                table = "FormsFormElementType";
                columns = new[] { "FormElementTypeCode", "FormElementTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "Title" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "Text" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "Measure" });

                table = "FormsMeasureUnitType";
                columns = new[] { "MeasureUnitTypeCode", "MeasureUnitTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "Quantity" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "Binary" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "Percentage" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "Quality 1-5" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 5, "Quality" });

                table = "FormsObjectiveType";
                columns = new[] { "ObjectiveTypeCode", "ObjectiveTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "Performance" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "Survey" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "Evaluation 360" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "Evaluation" });
            }
            else
            {
                var table = "FormsRecordStatus";
                var columns = new[] { "RecordStatusCode", "RecordStatusName" };
                var colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "פעיל" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "סגור" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "מעודכן" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "מבוטל" });

                table = "FormsEvaluatedType";
                columns = new[] { "EvaluatedTypeCode", "EvaluatedTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "יחידה" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "אדם" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "לומדה" });

                table = "FormsEvaluatorType";
                columns = new[] { "EvaluatorTypeCode", "EvaluatorTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "מזוהה" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "אנונימי" });

                table = "FormsFormElementType";
                columns = new[] { "FormElementTypeCode", "FormElementTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "כותרת" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "טקסט" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "מדד" });

                table = "FormsMeasureUnitType";
                columns = new[] { "MeasureUnitTypeCode", "MeasureUnitTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "כמותי" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "בינארי" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "אחוזים" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "איכותי 1-5" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 5, "איכותי" });

                table = "FormsObjectiveType";
                columns = new[] { "ObjectiveTypeCode", "ObjectiveTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "כשירות" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "סקר" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "הערכה 360" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "הערכה עבור רכיב" });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //throw new NotImplementedException();
        }
    }
}


