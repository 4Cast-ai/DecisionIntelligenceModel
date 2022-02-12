using Infrastructure.Migrations;
using Model.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Model.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20202312120002_InitialSeed")]
    public partial class InitialSeed : MigrationEnsured
    {
        //only for example
        //private List<User> _users; 

        protected override void Init(MigrationBuilderEnsured migrationBuilder, IServiceProvider serviceProvider)
        {
            //only for example
            //using var dbContext = services.GetService<Context>();
        }

        protected override void Up(MigrationBuilderEnsured migrationBuilder)
        {
            
            if (this.DefaultCulture == "en")
            {
                var table = "ThresholdLevels";
                var columns = new[] { "LevelId", "Name" };
                var colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "Not enough" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Low" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Medium" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "Good" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "High" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "One level" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "Two levels" });

                table = "ThresholdOriginCondition";
                columns = new[] { "OriginConditionId", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "Lower than" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Bigger than" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Equal" });

                table = "ThresholdsDestinationCondition";
                columns = new[] { "DestinationConditionId", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "Will not exceed" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Will not be less than" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Decrease by" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "Increase by" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "Will go down to" });

               
                table = "EntityType";
                columns = new[] { "EntityTypeId", "EntityTypeName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "Unit" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Person" });

                table = "SystemJobTitles";
                columns = new[] { "UserAdminPermission", "JobTitleName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "PermissionManager" });   //"מנהל הרשאות"
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Employee" });            //"עובד" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Tutor" });               //"חונך" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "ManDown" });             //"איש מטה" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "Director" });            //"מנהל" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "ClientExpert" });        //"מומחה לקוח" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "ContentManager" });      //"מנהל תוכן" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, "SystemDirector" });      //"מנהל מערכת" });

                table = "PermissionTypes";
                columns = new[] { "PermissionTypeId", "PermissionTypeName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 0, "BlockedAll" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "View" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Edit" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "Create" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, "AllowedAll" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 100, "Open" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 101, "Open_Home" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 200, "Activity" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 201, "New_Activity" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 202, "Edit_Activity" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 203, "Erase_Activity" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 204, "Improve_Form" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 205, "Import_Units_Participents" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 300, "Filler" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 400, "Report" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 401, "New_Report" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 402, "Open_OrganizationReport" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 403, "Open_OrganizationStructure" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 500, "Expert" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 501, "Open_ExpertSystem" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 502, "Open_Units" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 503, "Open_BuildModel" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 504, "Open_BuildForm" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 505, "Open_BuildActivity" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 506, "Open_Map" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 601, "Open_Permissions" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 700, "Interface" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 701, "Import_Data" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 702, "EMS" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 703, "EMS_ShowXYAll" });

                table = "UserType";
                columns = new[] { "UserTypeId", "TypeName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "User" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "HR" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "UserHR" });

                table = "MeasuringUnit";
                columns = new[] { "MeasuringUnitId", "MeasuringUnitName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "quantitative" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "binary" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "percentage" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "qualitative" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "qualitative(1-7)" });

                table = "FormStatus";
                columns = new[] { "FormStatusId", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "Empty" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Draft" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Published" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "Authorized" });

                table = "ActivityStatus";
                columns = new[] { "ActivityStatusId", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "Draft" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "InProcess" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Ended" });

                table = "FormElementType";
                columns = new[] { "FormElementTypeGuid", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "Title" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Text" });

                table = "RollupMethod";
                columns = new[] { "RollupMethodId", "RollupMethodName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "formula" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Weighted_Average" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "minimum" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "maximum" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, "average" });

                table = "ReportType";
                columns = new[] { "TypeId", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "Regular" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Units" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Dates" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "Map" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "Activites" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "4cast&Interface" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "Interface" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, "Cluster" });

                table = "ModelComponentType";
                columns = new[] { "TypeGuid", "TypeName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "הפניה" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "העתקה ללא מקור" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "העתקה כולל מקור" });

                table = "ModelComponentStatus";
                columns = new[] { "ModelComponentStatusId", "ModelComponentStatusName" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "draft" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "active" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "edit" });

                table = "ModelComponentSource";
                columns = new[] { "ModelComponentSourceId", "ModelComponentSourceName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "form" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "data_source" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "calculated" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "metric" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "model" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "model_root" });

                table = "CalenderRollup";
                columns = new[] { "CalenderRollupId", "CalenderRollupName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "cumulative" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "last_set" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "smallest" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "last" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "biggest" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "average" });

              
                colTypes = new string[] { "integer", "integer" };
                table = "RolePermissions";
                columns = new[] { "RoleId", "PermissionTypeId" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, 200 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, 400 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, 500 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, 701 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, 702 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 402 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 403 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 500 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 601 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 205 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 701 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 703 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 5, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 402 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 501 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 502 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 503 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 504 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 505 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 506 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 8, 8 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 9, 0 });


                table = "RoleItems";
                columns = new[] { "RoleId", "RoleItemId" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, 1 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, 4 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, 1 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, 4 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, 6 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, 0 });

                table = "TemplateSettings";
                columns = new[] { "TemplateType", "TemplateName", "ModelLevel", "NumOfChildInLevel2", "NumOfChildInLevel3", "NumOfChildInLevel4" };
                colTypes = new string[] { "integer", "character varying", "integer", "integer", "integer", "integer" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "template1", 3, 21, 2, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "template2", 3, 2, 16, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "template3", 3, 2, 20, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "template4", 4, 2, 4, 5 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "template5", 3, 12, 2, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "template6", 4, 4, 2, 4 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "template7", 3, 11, 2, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, "template8", 4, 3, 2, 5 });

                table = "Unit";
                columns = new[] { "SerialNum", "UnitGuid", "UnitName", "Order", "ParentUnitGuid", "ManagerUnitGuid", "DefaultModelGuid" };
                colTypes = new string[] { "integer", "character varying", "character varying", "integer", "character varying", "character varying", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 0, "aaaabbbbccccddddeeeeffffgggghhhh", "4Cast", 1, null, null, null });

                table = "Roles";
                columns = new[] { "RoleId", "RoleName", "Description", "UnitGuid", "UpdateDate", "UpdateUserId", "Status" };
                colTypes = new string[] { "integer", "character varying", "character varying", "character varying", "character varying", "character varying", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 0, "None", "אין הרשאות", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "PermissionManager", "מנהל הרשאות", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "ClientUser", "משתמש לקוח", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "UserEval", "משתמש הערכה", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "ClientManager", "מנהל לקוח", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, "SystemManager", "מנהל מערכת", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 9, "Anonymous", "משתמש אנונימי ", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate" });

                

                table = "Gender";
                columns = new[] { "GenderId", "GenderName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "male" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "female" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "other" });

                table = "Status";
                columns = new[] { "StatusId", "StatusName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "single" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "married" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "divorcee" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "widower" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "separated" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "publicly known" });

                table = "User";
                columns = new[] { "UserGuid", "UserId", "UserName", "UserPassword", "UserFirstName", "UserLastName", "UserBusinessPhone", "UserMobilePhone", "UserNotes", "UserModifiedDate", "UserStatus", "JobTitleGuid", "UnitGuid", "UserAdminPermission", "UserCreateDate", "UserEmail", "RoleId", "UserType" };
                colTypes = new string[] { "character varying", "character varying", "character varying", "character varying", "character varying", "character varying", "character varying", "character varying", "character varying", "character", "character varying", "character varying", "character varying", "integer", "character", "character varying", "integer", "integer" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { "00001111222233334444555566667777", "111111", "4cast", "t+IdgWKthwS8I2j0Mu1V9g==", "4cast", "user", "", "", "", "", "activate", "", "aaaabbbbccccddddeeeeffffgggghhhh", 8, "20200110123930", "", 8, 1 });

                table = "UserPreference";
                columns = new[] { "UserGuid", "UserTheme" };
                colTypes = new string[] { "character varying", "integer" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { "00001111222233334444555566667777", 1 });
            }
            else
            {
                var table = "ThresholdLevels";
                var columns = new[] { "LevelId", "Name" };
                var colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "בלתי מספקת" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "נמוכה" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "בינונית" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "טובה" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 5, "טובה מאוד" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, "רמה אחת" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 7, "שתי רמות" });

                table = "ThresholdOriginCondition";
                columns = new[] { "OriginConditionId", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "קטנ/ה מ" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "גדולה מ" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "שווה בדיוק" });

                table = "ThresholdsDestinationCondition";
                columns = new[] { "DestinationConditionId", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "לא תעלה על" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "לא תרד מ" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "תרד ב" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "תעלה ב" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 5, "תרד ל" });

               
                table = "EntityType";
                columns = new[] { "EntityTypeId", "EntityTypeName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "יחידה" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "אדם" });

                table = "SystemJobTitles";
                columns = new[] { "UserAdminPermission", "JobTitleName" };

                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "מנהל הרשאות" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "עובד" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "חונך" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "איש מטה" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 5, "מנהל" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, "מומחה לקוח" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 7, "מנהל תוכן" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 8, "מנהל מערכת" });

                table = "PermissionTypes";
                columns = new[] { "PermissionTypeId", "PermissionTypeName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 0, "BlockedAll" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "View" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Edit" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "Create" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, "AllowedAll" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 100, "Open" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 101, "Open_Home" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 200, "Activity" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 201, "New_Activity" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 202, "Edit_Activity" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 203, "Erase_Activity" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 204, "Improve_Form" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 205, "Import_Units_Participents" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 300, "Filler" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 400, "Report" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 401, "New_Report" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 402, "Open_OrganizationReport" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 403, "Open_OrganizationStructure" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 500, "Expert" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 501, "Open_ExpertSystem" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 502, "Open_Units" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 503, "Open_BuildModel" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 504, "Open_BuildForm" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 505, "Open_BuildActivity" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 506, "Open_Map" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 601, "Open_Permissions" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 700, "Interface" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 701, "Import_Data" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 702, "EMS" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 703, "EMS_ShowXYAll" });

                table = "UserType";
                columns = new[] { "UserTypeId", "TypeName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "User" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "HR" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "UserHR" });

                table = "MeasuringUnit";
                columns = new[] { "MeasuringUnitId", "MeasuringUnitName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "quantitative" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "binary" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "percentage" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "qualitative" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "qualitative(1-7)" });

                table = "FormStatus";
                columns = new[] { "FormStatusId", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "Empty" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Draft" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Published" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "Authorized" });

                table = "ActivityStatus";
                columns = new[] { "ActivityStatusId", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "טיוטה" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "בתהליך" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "הסתיים" });

                table = "FormElementType";
                columns = new[] { "FormElementTypeGuid", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "כותרת" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "שדה טקסט" });

                table = "RollupMethod";
                columns = new[] { "RollupMethodId", "RollupMethodName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "formula" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Weighted_Average" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "minimum" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "maximum" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, "average" });

                table = "ReportType";
                columns = new[] { "TypeId", "Name" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "Regular" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "Units" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "Dates" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "Map" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "Activites" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "4cast&Interface" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "Interface" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, "Cluster" });

                table = "ModelComponentType";
                columns = new[] { "TypeGuid", "TypeName" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "הפניה" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "העתקה ללא מקור" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "העתקה כולל מקור" });

                table = "ModelComponentStatus";
                columns = new[] { "ModelComponentStatusId", "ModelComponentStatusName" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "draft" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "active" });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "edit" });

                table = "ModelComponentSource";
                columns = new[] { "ModelComponentSourceId", "ModelComponentSourceName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "form" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "data_source" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "calculated" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "metric" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "model" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "model_root" });

                table = "CalenderRollup";
                columns = new[] { "CalenderRollupId", "CalenderRollupName" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "cumulative" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "last_set" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "smallest" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "last" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "biggest" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 7, "average" });


                
                table = "RoleItems";
                columns = new[] { "RoleId", "RoleItemId" };
                colTypes = new string[] { "integer", "integer" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, 1 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, 4 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, 1 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, 4 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, 6 });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, 0 });

                table = "RolePermissions";
                columns = new[] { "RoleId", "PermissionTypeId" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, 200 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, 400 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, 500 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, 701 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, 702 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 402 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 403 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 500 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 601 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 205 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 701 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, 703 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 5, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 402 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 501 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 502 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 503 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 504 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 505 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, 506 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 8, 8 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 9, 0 });

                colTypes = new string[] { "integer", "character varying", "integer", "integer", "integer", "integer" };
                table = "TemplateSettings";
                columns = new[] { "TemplateType", "TemplateName", "ModelLevel", "NumOfChildInLevel2", "NumOfChildInLevel3", "NumOfChildInLevel4" };
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 1, "template1", 3, 21, 2, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 2, "template2", 3, 2, 16, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 3, "template3", 3, 2, 20, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 4, "template4", 4, 2, 4, 5 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 5, "template5", 3, 12, 2, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 6, "template6", 4, 4, 2, 4 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 7, "template7", 3, 11, 2, 0 });
                migrationBuilder.InsertData(table, columns, colTypes, values: new object[] { 8, "template8", 4, 3, 2, 4 });

                table = "Unit";
                columns = new[] { "SerialNum", "UnitGuid", "UnitName", "Order", "ParentUnitGuid", "ManagerUnitGuid", "DefaultModelGuid"};
                colTypes = new string[] { "integer", "character varying", "character varying", "integer", "character varying", "character varying", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] {0, "aaaabbbbccccddddeeeeffffgggghhhh", "4Cast", 1, null, null, null});

                              
                table = "Roles";
                columns = new[] { "RoleId", "RoleName", "Description", "UnitGuid", "UpdateDate", "UpdateUserId", "Status", "UserGuid" };
                colTypes = new string[] { "integer", "character varying", "character varying", "character varying", "character varying", "character varying", "character varying", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 0, "None", "אין הרשאות", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate", null });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "PermissionManager", "מנהל הרשאות", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate", null });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "ClientUser", "משתמש לקוח", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate", null });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "UserEval", "משתמש הערכה", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate", null });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "ClientManager", "מנהל לקוח", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate", null });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 8, "SystemManager", "מנהל מערכת", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate", null });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 9, "Anonymous", "משתמש אנונימי ", "aaaabbbbccccddddeeeeffffgggghhhh", "20200101120000", "00001111222233334444555566667777", "activate", null });

               

                table = "Gender";
                columns = new[] { "GenderId", "GenderName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] {1, "זכר" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "נקבה" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "אחר" });

                table = "Status";
                columns = new[] { "StatusId", "StatusName" };
                colTypes = new string[] { "integer", "character varying" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 1, "רווק/ה" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 2, "נשוי/אה" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 3, "גרוש/ה" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 4, "אלמן/ה" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 5, "פרוד/ה" });
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { 6, "ידוע/ה בציבור" });

                table = "User";
                columns = new[] { "UserGuid", "UserId", "UserName", "UserPassword", "UserFirstName", "UserLastName", "UserBusinessPhone", "UserMobilePhone", "UserNotes", "UserModifiedDate", "UserStatus", "JobTitleGuid", "UnitGuid", "UserAdminPermission", "UserCreateDate", "UserEmail", "RoleId", "UserType" };
                colTypes = new string[] { "character varying", "character varying", "character varying", "character varying", "character varying", "character varying", "character varying", "character varying", "character varying", "character", "character varying", "character varying", "character varying", "integer", "character", "character varying", "integer", "integer" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { "00001111222233334444555566667777", "111111", "4cast", "t+IdgWKthwS8I2j0Mu1V9g==", "4cast", "user", "", "", "", "", "activate", "", "aaaabbbbccccddddeeeeffffgggghhhh", 8, "20200110123930", "", 8, 1 });


                table = "UserPreference";
                columns = new[] { "UserGuid", "UserTheme" };
                colTypes = new string[] { "character varying", "integer" };
                migrationBuilder.InsertData(table, columns, colTypes, new object[] { "00001111222233334444555566667777", 1 });

            }
        }

        protected override void Down(MigrationBuilderEnsured migrationBuilder)
        {
            //throw new NotImplementedException();
        }
    }
}


