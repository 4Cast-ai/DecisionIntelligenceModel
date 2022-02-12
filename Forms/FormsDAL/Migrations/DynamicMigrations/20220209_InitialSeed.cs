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
    [DbContext(typeof(FormsDynamicDBContext))]
    [Migration("20220209_InitialSeed")]
    public partial class DynamicInitialSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string defaultCulture = GeneralContext.GetConfig<IAppConfig>().DefaultLocale;
            if (defaultCulture == "en")
            {
                var table = "DynamicRecordStatus";
                var columns = new[] { "RecordStatusCode", "RecordStatusName" };
                var colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "Active" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "Close" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "Update" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "Cancel" });

                table = "DynamicFormStatus";
                columns = new[] { "FormStatusCode", "FormStatusName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "Pending" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "In Progress" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "Completed" });

                table = "DynamicEntityType";
                columns = new[] { "EntityTypeCode", "EntityTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "Department" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "Person" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "Learning item" });
            }
            else
            {
                var table = "DynamicRecordStatus";
                var columns = new[] { "RecordStatusCode", "RecordStatusName" };
                var colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "פעיל" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "סגור" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "מעודכן" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "מבוטל" });

                table = "DynamicFormStatus";
                columns = new[] { "FormStatusCode", "FormStatusName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "חדש" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "בתהליך" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "הושלם" });

                table = "DynamicEntityType";
                columns = new[] { "EntityTypeCode", "EntityTypeName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "יחידה" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "אדם" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "לומדה" });
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //throw new NotImplementedException();
        }
    }
}


