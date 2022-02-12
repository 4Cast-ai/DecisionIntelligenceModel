using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Model.Entities;
using NpgsqlTypes;

namespace Model.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "Form_Template_Structure_form_template_structure_id_seq");


            migrationBuilder.CreateTable(
                name: "ActivityTemplate",
                columns: table => new
                {
                    ActivityTemplateGuid = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    CreateDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    ProfessionalRecommendations = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Activity_Template_pkey", x => x.ActivityTemplateGuid);
                });

            migrationBuilder.CreateTable(
                name: "CalenderRollup",
                columns: table => new
                {
                    CalenderRollupId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CalenderRollupName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalenderRollup", x => x.CalenderRollupId);
                });

            migrationBuilder.CreateTable(
                name: "FormElementType",
                columns: table => new
                {
                    FormElementTypeGuid = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Form_Element_Type_pkey", x => x.FormElementTypeGuid);
                });

            migrationBuilder.CreateTable(
                name: "FormStatus",
                columns: table => new
                {
                    FormStatusId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormStatus", x => x.FormStatusId);
                });

            migrationBuilder.CreateTable(
                name: "MeasuringUnit",
                columns: table => new
                {
                    MeasuringUnitId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MeasuringUnitName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeasuringUnit", x => x.MeasuringUnitId);
                });

            migrationBuilder.CreateTable(
                name: "ModelComponentSource",
                columns: table => new
                {
                    ModelComponentSourceId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelComponentSourceName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelComponentSource", x => x.ModelComponentSourceId);
                });

            migrationBuilder.CreateTable(
                name: "ModelComponentStatus",
                columns: table => new
                {
                    ModelComponentStatusId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelComponentStatusName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelComponentStatus", x => x.ModelComponentStatusId);
                });

            migrationBuilder.CreateTable(
                name: "ModelComponentType",
                columns: table => new
                {
                    TypeGuid = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeName = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Model_Component_Type_pkey", x => x.TypeGuid);
                });

            migrationBuilder.CreateTable(
                name: "PermissionTypes",
                columns: table => new
                {
                    PermissionTypeId = table.Column<int>(nullable: false),
                    PermissionTypeName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PermissionTypes_pkey", x => x.PermissionTypeId);
                });

            migrationBuilder.CreateTable(
                name: "ReportType",
                columns: table => new
                {
                    TypeId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Report_Type_pkey", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "RoleItems",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    RoleItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleId_RolItemId", x => new { x.RoleId, x.RoleItemId });
                });

            migrationBuilder.CreateTable(
                name: "RollupMethod",
                columns: table => new
                {
                    RollupMethodId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RollupMethodName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RollupMethod", x => x.RollupMethodId);
                });

            migrationBuilder.CreateTable(
                name: "SavedReportData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportGuid = table.Column<string>(maxLength: 50, nullable: false),
                    ReportData = table.Column<string>(type: "json", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedReportData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemJobTitles",
                columns: table => new
                {
                    UserAdminPermission = table.Column<int>(nullable: false),
                    JobTitleName = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("System_Job_titles_pkey", x => x.UserAdminPermission);
                });

            migrationBuilder.CreateTable(
                name: "UserType",
                columns: table => new
                {
                    UserTypeId = table.Column<int>(nullable: false),
                    TypeName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_Type_pkey", x => x.UserTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Description",
                columns: table => new
                {
                    DescriptionGuid = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Remark = table.Column<string>(maxLength: 255, nullable: true),
                    Creator = table.Column<DateTime>(nullable: true),
                    Modify = table.Column<DateTime>(nullable: true),
                    CreatorUserGuid = table.Column<string>(maxLength: 50, nullable: false),
                    ModifyUserGuid = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Description_pkey", x => x.DescriptionGuid);
                });

            migrationBuilder.CreateTable(
               name: "OrganizationType",
               columns: table => new
               {
                   OrgTypeId = table.Column<int>(nullable: false),
                   OrgTypeName = table.Column<string>(maxLength: 50, nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("OrganizationType_pkey", x => x.OrgTypeId);
               });

            migrationBuilder.CreateTable(
               name: "EntityType",
               columns: table => new
               {
                   EntityTypeId = table.Column<int>(nullable: false),
                   EntityTypeName = table.Column<string>(maxLength: 50, nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("EntityType_pkey", x => x.EntityTypeId);
               });

            migrationBuilder.CreateTable(
                name: "OrganizationObject",
                columns: table => new
                {
                    OrgObjGuid = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    Remark = table.Column<string>(maxLength: 255, nullable: true),
                    OrgObjType = table.Column<int>(nullable: true),
                    Order = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Organization_Object_pkey", x => x.OrgObjGuid);
                    table.ForeignKey(
                       name: "FK_Organization_Object_OrganizationType",
                       column: x => x.OrgObjType,
                       principalTable: "OrganizationType",
                       principalColumn: "OrgTypeId",
                       onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
               name: "TemplateSettings",
               columns: table => new
               {
                   TemplateType = table.Column<int>(nullable: false),
                   TemplateName = table.Column<string>(maxLength: 250, nullable: false),
                   ModelLevel = table.Column<int>(nullable: false),
                   NumOfChildInLevel2 = table.Column<int>(nullable: false),
                   NumOfChildInLevel3 = table.Column<int>(nullable: false),
                   NumOfChildInLevel4 = table.Column<int>(nullable: false),
               },
                constraints: table =>
                {
                    table.PrimaryKey("TemplateType_pkey", x => x.TemplateType);
                });

            migrationBuilder.CreateTable(
                name: "Form_Element",
                columns: table => new
                {
                    FormElementGuid = table.Column<string>(maxLength: 50, nullable: false),
                    FormElementType = table.Column<int>(nullable: true),
                    FormElementTitle = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Form_Element_pkey", x => x.FormElementGuid);
                    table.ForeignKey(
                        name: "FK_Form_Element_Form_Element_Type",
                        column: x => x.FormElementType,
                        principalTable: "FormElementType",
                        principalColumn: "FormElementTypeGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    PermissionTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RolePermissions_pkey", x => new { x.RoleId, x.PermissionTypeId });
                    table.ForeignKey(
                        name: "FK_PermisionTypes",
                        column: x => x.PermissionTypeId,
                        principalTable: "PermissionTypes",
                        principalColumn: "PermissionTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateTable(
            //    name: "Unit",
            //    columns: table => new
            //    {
            //        UnitGuid = table.Column<string>(maxLength: 50, nullable: false),
            //        UnitId = table.Column<double>(nullable: false),
            //        UnitName = table.Column<string>(maxLength: 255, nullable: true),
            //        UnitDescription = table.Column<string>(maxLength: 1000, nullable: true),
            //        EntityTypeGuid = table.Column<string>(maxLength: 50, nullable: true),
            //        UnitStatus = table.Column<string>(maxLength: 255, nullable: true),
            //        UnitParentGuid = table.Column<string>(maxLength: 50, nullable: true),
            //        UnitCreateDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
            //        UnitModifiedDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
            //        UnitCatalogId = table.Column<string>(maxLength: 255, nullable: true),
            //        PolygonGuid = table.Column<string>(maxLength: 50, nullable: true),
            //        IsEstimateUnit = table.Column<bool>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("Unit_pkey", x => x.UnitGuid);
            //        table.ForeignKey(
            //            name: "FK_Units_Units",
            //            column: x => x.UnitParentGuid,
            //            principalTable: "Unit",
            //            principalColumn: "UnitGuid",
            //            onDelete: ReferentialAction.Restrict);
            migrationBuilder.CreateTable(
            name: "Unit",
            columns: table => new
            {
                SerialNum = table.Column<int>(nullable: false).Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                UnitGuid = table.Column<string>(maxLength: 50, nullable: false),
                UnitName = table.Column<string>(maxLength: 255, nullable: false),
                Order = table.Column<int>(nullable: true),
                ParentUnitGuid = table.Column<string>(maxLength: 50, nullable: true),
                ManagerUnitGuid = table.Column<string>(maxLength: 50, nullable: true),
                DefaultModelGuid = table.Column<string>(maxLength: 50, nullable: true),
                //DescriptionsGuid = table.Column<int[]>(nullable: true),
                //ActivityTemplates = table.Column<string[]>(nullable: true),

            },
            constraints: table =>
            {
                table.PrimaryKey("PK_UnitGuid", x => x.UnitGuid);
                table.ForeignKey(
                    name: "FK_Unit_Parent_Unit_Guid",
                    column: x => x.ParentUnitGuid,
                    principalTable: "Unit",
                    principalColumn: "UnitGuid",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                  name: "FK_Unit_Default_Model_Guid",
                  column: x => x.DefaultModelGuid,
                  principalTable: "ModelComponent",
                  principalColumn: "ModelComponentGuid",
                  onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                name: "FK_Unit_Manager_Unit_Guid",
                column: x => x.ManagerUnitGuid,
                principalTable: "Person",
                principalColumn: "PersonGuid",
                onDelete: ReferentialAction.Restrict);
                //table.ForeignKey(
                //  name: "FK_Unit_Description_Guid",
                //  column: x => x.DescriptionGuid,
                //  principalTable: "Description",
                //  principalColumn: "DescriptionGuid",
                //  onDelete: ReferentialAction.Restrict);
            });
            migrationBuilder.CreateTable(
           name: "Person",
           columns: table => new
           {
               PersonGuid = table.Column<string>(maxLength: 50, nullable: false),
               FirstName = table.Column<string>(maxLength: 255, nullable: false),
               LastName = table.Column<string>(maxLength: 255, nullable: false),
               Id = table.Column<string>(maxLength: 50, nullable: false),
               PersonNumber = table.Column<int>(nullable: false),
               UnitGuid = table.Column<string>(maxLength: 50, nullable: false),
               DirectManagerGuid = table.Column<string>(maxLength: 50, nullable: true),
               professionalManagerGuid = table.Column<string>(maxLength: 50, nullable: true),
               JobtitleGuid = table.Column<string>(maxLength: 50, nullable: true),
               //Descriptions = table.Column<int[]>(nullable: true),
               Gender = table.Column<int>(nullable: false),
               BeginningOfWork = table.Column<DateTime>(nullable: false),
               Email1 = table.Column<string>(maxLength: 255, nullable: false),
               Email2 = table.Column<string>(maxLength: 255, nullable: true),
               Phone1 = table.Column<string>(maxLength: 255, nullable: false),
               Phone2 = table.Column<string>(maxLength: 255, nullable: true),
               Street = table.Column<string>(maxLength: 255, nullable: true),
               City = table.Column<string>(maxLength: 255, nullable: true),
               State = table.Column<int>(nullable: true),
               Country = table.Column<int>(nullable: true),
               ZipCode = table.Column<string>(maxLength: 255, nullable: true),
               DateOfBirth = table.Column<DateTime>(nullable: false),
               Status = table.Column<int>(nullable: false),
               ChildrenNum = table.Column<int>(nullable: false),
               Degree = table.Column<string>(maxLength: 255, nullable: true),
               Institution = table.Column<string>(maxLength: 255, nullable: true),
               Profession = table.Column<string>(maxLength: 255, nullable: true),
               Car = table.Column<bool>(nullable: true),
               Manufactor = table.Column<string>(maxLength: 255, nullable: true),
               PlateNum = table.Column<string>(maxLength: 50, nullable: true),
               EducationFund = table.Column<bool>(nullable: true),
               LastSalaryUpdate = table.Column<DateTime>(nullable: true),
               Files = table.Column<Byte[][]>(nullable: true),
               Photo = table.Column<Byte[]>(nullable: true),
               //ActivityTemplates = table.Column<string[]>(nullable: true),

           },
           constraints: table =>
           {
               table.PrimaryKey("PK_PersonGuid", x => x.PersonGuid);
               table.ForeignKey(
                   name: "FK_Person_Unit_Guid",
                   column: x => x.UnitGuid,
                   principalTable: "Unit",
                   principalColumn: "UnitGuid",
                   onDelete: ReferentialAction.Restrict);
               table.ForeignKey(
                 name: "FK_Person_Direct_Manager_Guid",
                 column: x => x.DirectManagerGuid,
                 principalTable: "Person",
                 principalColumn: "PersonGuid",
                 onDelete: ReferentialAction.Restrict);
               table.ForeignKey(
                 name: "FK_Person_professional_Manager_Guid",
                 column: x => x.professionalManagerGuid,
                 principalTable: "Person",
                 principalColumn: "PersonGuid",
                 onDelete: ReferentialAction.Restrict);
               table.ForeignKey(
                name: "FK_Person_Job_title_Guid",
                column: x => x.JobtitleGuid,
                principalTable: "ModelComponentGuid",
                principalColumn: "ModelComponent",
                onDelete: ReferentialAction.Restrict);
               table.ForeignKey(
                name: "FK_Person_Gender",
                column: x => x.Gender,
                principalTable: "Gender",
                principalColumn: "GenderId",
                onDelete: ReferentialAction.Restrict);
               table.ForeignKey(
               name: "FK_Person_Status",
               column: x => x.Status,
               principalTable: "Status",
               principalColumn: "StatusId",
               onDelete: ReferentialAction.Restrict);
           });


            migrationBuilder.CreateTable(
             name: "Gender",
             columns: table => new
             {
                 GenderId = table.Column<int>(nullable: false),
                 GenderName = table.Column<string>(maxLength: 50, nullable: false)
             },
             constraints: table =>
             {
                 table.PrimaryKey("PK_GenderId", x => x.GenderId);
             });

            migrationBuilder.CreateTable(
             name: "Status",
             columns: table => new
             {
                 StatusId = table.Column<int>(nullable: false),
                 StatusName = table.Column<string>(maxLength: 50, nullable: false)
             },
             constraints: table =>
             {
                 table.PrimaryKey("PK_StatusId", x => x.StatusId);
             });
            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    ActivityGuid = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Name1 = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 1000, nullable: true),
                    StartDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    EndDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    ActivityTemplateGuid = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    //OrgObjGuid = table.Column<string>(maxLength: 50, nullable: true),
                    ActivityGroupGuid = table.Column<string>(maxLength: 50, nullable: true),
                    Users = table.Column<string[]>(nullable: true),
                    GuidType = table.Column<int>(nullable: true),
                    Guid = table.Column<string>(nullable: true),
                    EstimateUnits = table.Column<string[]>(nullable: true),
                    EstimatePersons = table.Column<string[]>(nullable: true),

                },
                constraints: table =>
                {
                    table.PrimaryKey("Activity_pkey", x => x.ActivityGuid);
                    table.ForeignKey(
                        name: "FK_Activities_Activity_template",
                        column: x => x.ActivityTemplateGuid,
                        principalTable: "ActivityTemplate",
                        principalColumn: "ActivityTemplateGuid",
                        onDelete: ReferentialAction.Restrict);
                    //table.ForeignKey(
                    //    name: "FK_Activity_Organization_Object",
                    //    column: x => x.OrgObjGuid,
                    //    principalTable: "OrganizationObject",
                    //    principalColumn: "OrgObjGuid",
                    //    onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                       name: "FK_Activity_Guid_Type",
                       column: x => x.GuidType,
                       principalTable: "EntityType",
                       principalColumn: "EntityTypeId",
                       onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                  name: "MultiActivity",
                  columns: table => new
                  {
                      ActivityGroupGuid = table.Column<string>(nullable: false),
                      //OrgObjGuidList = table.Column<string>(nullable: false),
                      Units = table.Column<string[]>(nullable: true),
                      Persons = table.Column<string[]>(nullable: true),
                      Name = table.Column<string>(nullable: false),
                      Description = table.Column<string>(nullable: true),
                      ActivityTemplateGuid = table.Column<string>(nullable: false),
                      StartDate = table.Column<string>(maxLength: 14, nullable: false),
                      EndDate = table.Column<string>(maxLength: 14, nullable: false),
                      //EstimatesGuids = table.Column<string>(nullable: true),
                      //EstimatesNames = table.Column<string>(nullable: true),
                      EstimateUnits = table.Column<string[]>(nullable: true),
                      EstimatePersons = table.Column<string[]>(nullable: true),

                  },
                  constraints: table =>
                  {
                      table.PrimaryKey("MultiActivity_pkey", x => x.ActivityGroupGuid);
                  });

            //migrationBuilder.CreateTable(
            //    name: "OrganizationStructure",
            //    columns: table => new
            //    {
            //        OrgObjGuid = table.Column<string>(maxLength: 50, nullable: false),
            //        OrgObjParentGuid = table.Column<string>(maxLength: 50, nullable: true),
            //        PermissionUnits = table.Column<Guid[]>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("Organization_Structure_pkey", x => x.OrgObjGuid);
            //        table.ForeignKey(
            //            name: "FK_Organization_Structure_Organization_Object",
            //            column: x => x.OrgObjGuid,
            //            principalTable: "OrganizationObject",
            //            principalColumn: "OrgObjGuid",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Organization_Structure_Organization_Object1",
            //            column: x => x.OrgObjParentGuid,
            //            principalTable: "OrganizationObject",
            //            principalColumn: "OrgObjGuid",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "EstimatedOrganizationObject",
            //    columns: table => new
            //    {
            //        OrgObjGuid = table.Column<string>(maxLength: 50, nullable: false),
            //        ActivityGuid = table.Column<string>(maxLength: 50, nullable: false),
            //        OrgObjEstimatedGuid = table.Column<string>(maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("Estimated_Organization_Object_pkey", x => new { x.OrgObjGuid, x.ActivityGuid, x.OrgObjEstimatedGuid });
            //        table.ForeignKey(
            //            name: "FK_Estimated_Organization_Object_Activity",
            //            column: x => x.ActivityGuid,
            //            principalTable: "Activity",
            //            principalColumn: "ActivityGuid",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Estimated_Organization_Object_Organization_Object1",
            //            column: x => x.OrgObjEstimatedGuid,
            //            principalTable: "OrganizationObject",
            //            principalColumn: "OrgObjGuid",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Estimated_Organization_Object_Organization_Object",
            //            column: x => x.OrgObjGuid,
            //            principalTable: "OrganizationObject",
            //            principalColumn: "OrgObjGuid",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateTable(
                name: "CalculateScore",
                columns: table => new
                {
                    ScoreId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: true),
                    FormElementGuid = table.Column<string>(maxLength: 50, nullable: true),
                    //OrgObjGuid = table.Column<string>(maxLength: 50, nullable: true),
                    GuidType = table.Column<int>(nullable: true),
                    Guid = table.Column<string>(nullable: true),
                    ActivityGuid = table.Column<string>(maxLength: 50, nullable: true),
                    OriginalScore = table.Column<double>(nullable: true),
                    ConvertionScore = table.Column<double>(nullable: true),
                    CalculatedScore = table.Column<double>(nullable: true),
                    CalculatedDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    ModelComponentComment = table.Column<string>(type: "character varying", nullable: true),
                    FormGuid = table.Column<string>(maxLength: 50, nullable: true),
                    ReportGuid = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CalculateScore_pkey", x => x.ScoreId);
                    table.ForeignKey(
                        name: "FK_Calculate_Score_Activity",
                        column: x => x.ActivityGuid,
                        principalTable: "Activity",
                        principalColumn: "ActivityGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Calculate_Score_Form_Element",
                        column: x => x.FormElementGuid,
                        principalTable: "Form_Element",
                        principalColumn: "FormElementGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                     name: "FK_Calculate_Score_Guid_Type",
                     column: x => x.GuidType,
                     principalTable: "EntityType",
                     principalColumn: "EntityTypeId",
                     onDelete: ReferentialAction.Restrict);
                    //table.ForeignKey(
                    //    name: "FK_Calculate_Score_Organization_Object",
                    //    column: x => x.OrgObjGuid,
                    //    principalTable: "OrganizationObject",
                    //    principalColumn: "OrgObjGuid",
                    //    onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Form",
                columns: table => new
                {
                    FormGuid = table.Column<string>(maxLength: 50, nullable: false),
                    FormTemplateGuid = table.Column<string>(maxLength: 50, nullable: true),
                    ActivityGuid = table.Column<string>(maxLength: 50, nullable: true),
                    ApproveUserGuid = table.Column<string>(maxLength: 50, nullable: true),
                    ApproveDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    Status = table.Column<int>(nullable: true),
                    //OrgObjGuid = table.Column<string>(maxLength: 50, nullable: true),
                    GuidType = table.Column<int>(nullable: true),
                    Guid = table.Column<string>(nullable: true),
                    UserInCourse = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Form_pkey", x => x.FormGuid);
                    table.ForeignKey(
                        name: "FK_Forms_Activities",
                        column: x => x.ActivityGuid,
                        principalTable: "Activity",
                        principalColumn: "ActivityGuid",
                        onDelete: ReferentialAction.Restrict);
                    //table.ForeignKey(
                    //    name: "FK_Form_Organization_Object",
                    //    column: x => x.OrgObjGuid,
                    //    principalTable: "OrganizationObject",
                    //    principalColumn: "OrgObjGuid",
                    //    onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Form_Form_Status",
                        column: x => x.Status,
                        principalTable: "FormStatus",
                        principalColumn: "FormStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                       name: "FK_User_binding",
                       column: x => x.UserInCourse,
                       principalTable: "User",
                       principalColumn: "UserGuid",
                       onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                    name: "FK_User_Guid_Type",
                    column: x => x.GuidType,
                    principalTable: "EntityType",
                    principalColumn: "EntityTypeId",
                    onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Score",
                columns: table => new
                {
                    ScoreId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: true),
                    FormElementGuid = table.Column<string>(maxLength: 50, nullable: true),
                    //OrgObjGuid = table.Column<string>(maxLength: 50, nullable: true),
                    EntityType = table.Column<int>(nullable: true),
                    UnitGuid = table.Column<string>(nullable: true),
                    ActivityGuid = table.Column<string>(maxLength: 50, nullable: true),
                    OriginalScore = table.Column<double>(nullable: true),
                    ConvertionScore = table.Column<double>(nullable: true),
                    ModelComponentComment = table.Column<string>(type: "character varying", nullable: true),
                    Status = table.Column<int>(nullable: true),
                    FormGuid = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score", x => x.ScoreId);
                    table.ForeignKey(
                        name: "FK_Score_Activity",
                        column: x => x.ActivityGuid,
                        principalTable: "Activity",
                        principalColumn: "ActivityGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Score_Form_Element",
                        column: x => x.FormElementGuid,
                        principalTable: "Form_Element",
                        principalColumn: "FormElementGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Score_Form",
                        column: x => x.FormGuid,
                        principalTable: "Form",
                        principalColumn: "FormGuid",
                        onDelete: ReferentialAction.Restrict);
                    //table.ForeignKey(
                    //    name: "FK_Score_Organization_Object",
                    //    column: x => x.OrgObjGuid,
                    //    principalTable: "OrganizationObject",
                    //    principalColumn: "OrgObjGuid",
                    //    onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Score_Form_Status",
                        column: x => x.Status,
                        principalTable: "FormStatus",
                        principalColumn: "FormStatusId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                     name: "FK_Score_Guid_Type",
                     column: x => x.EntityType,
                     principalTable: "EntityType",
                     principalColumn: "EntityTypeId",
                     onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AtInFt",
                columns: table => new
                {
                    ActivityTemplateGuid = table.Column<string>(maxLength: 50, nullable: false),
                    FormTemplateGuid = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("AtInFt_pkey", x => new { x.ActivityTemplateGuid, x.FormTemplateGuid });
                    table.ForeignKey(
                        name: "FK_AtInFt_ActivityTemplate",
                        column: x => x.ActivityTemplateGuid,
                        principalTable: "ActivityTemplate",
                        principalColumn: "ActivityTemplateGuid",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
                      name: "OrganizationObjectConnection",
                      columns: table => new
                      {
                          OrgObjConnGuid = table.Column<int>(nullable: false)
                              .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                          OrgObjGuid = table.Column<string>(maxLength: 50, nullable: false),
                          DescriptionGuid = table.Column<int?>(nullable: true),
                          //ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: true),
                          ActivityTemplateGuid = table.Column<string>(maxLength: 50, nullable: true)
                      },
                      constraints: table =>
                      {
                          table.PrimaryKey("Organization_Object_Connection_pkey", x => x.OrgObjConnGuid);
                          table.ForeignKey(
                              name: "FK_Organization_Object_Connection_Activity_template",
                              column: x => x.ActivityTemplateGuid,
                              principalTable: "ActivityTemplate",
                              principalColumn: "ActivityTemplateGu",
                              onDelete: ReferentialAction.Restrict);
                          table.ForeignKey(
                              name: "FK_Organization_Object_Connection_Description",
                              column: x => x.DescriptionGuid,
                              principalTable: "Description",
                              principalColumn: "DescriptionGu",
                              onDelete: ReferentialAction.Restrict);
                          table.ForeignKey(
                              name: "FK_Organization_Object_Connection_Organization_Object",
                              column: x => x.OrgObjGuid,
                              principalTable: "OrganizationObject",
                              principalColumn: "OrgObjGu",
                              onDelete: ReferentialAction.Restrict);
                      });


            migrationBuilder.CreateTable(
                name: "ModelComponent",
                columns: table => new
                {
                    ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    ProfessionalInstruction = table.Column<string>(maxLength: 255, nullable: true),
                    ModelDescriptionText = table.Column<string>(maxLength: 255, nullable: true),
                    Source = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: true),
                    ModelComponentOrder = table.Column<int>(nullable: true),
                    Weight = table.Column<double>(nullable: false),
                    CreateDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: false),
                    ModifiedDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: false),
                    ModifiedUserGuid = table.Column<string>(maxLength: 50, nullable: true),
                    MetricRequired = table.Column<bool>(nullable: true),
                    MetricMeasuringUnit = table.Column<int>(nullable: true),
                    MetricRollupMethod = table.Column<int>(nullable: true),
                    MetricCalenderRollup = table.Column<int>(nullable: true),
                    MetricFormula = table.Column<string>(maxLength: 255, nullable: true),
                    MetricIsVisible = table.Column<bool>(nullable: true),
                    MetricNotDisplayIfIrrelevant = table.Column<bool>(nullable: true),
                    MetricExpiredPeriod = table.Column<string>(maxLength: 25, nullable: true),
                    MetricExpiredPeriodSecondary = table.Column<string>(maxLength: 25, nullable: true),
                    MetricCommentObligationLevel = table.Column<double>(nullable: true),
                    MetricGradualDecreasePrecent = table.Column<double>(nullable: true),
                    MetricGradualDecreasePeriod = table.Column<double>(nullable: true),
                    MetricMinimumFeeds = table.Column<double>(nullable: true),
                    ShowOrigionValue = table.Column<bool>(nullable: true),
                    MetricSource = table.Column<int>(nullable: true),
                    TemplateType = table.Column<int>(nullable: true),
                    CalcAsSum = table.Column<bool>(nullable: true),
                    GroupChildren = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Model_Component_pkey", x => x.ModelComponentGuid);
                    table.ForeignKey(
                        name: "FK_Model_Component_Calender_Rollup",
                        column: x => x.MetricCalenderRollup,
                        principalTable: "CalenderRollup",
                        principalColumn: "CalenderRollupId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Model_Component_Measuring_Unit",
                        column: x => x.MetricMeasuringUnit,
                        principalTable: "MeasuringUnit",
                        principalColumn: "MeasuringUnitId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Model_Component_Rollup_Method",
                        column: x => x.MetricRollupMethod,
                        principalTable: "RollupMethod",
                        principalColumn: "RollupMethodId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Model_Component_Model_Component_Source1",
                        column: x => x.MetricSource,
                        principalTable: "ModelComponentSource",
                        principalColumn: "ModelComponentSourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Model_Component_Model_Component_Source",
                        column: x => x.Source,
                        principalTable: "ModelComponentSource",
                        principalColumn: "ModelComponentSourceId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Model_Component_Model_Component_Status",
                        column: x => x.Status,
                        principalTable: "ModelComponentStatus",
                        principalColumn: "ModelComponentStatusId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ConvertionTable",
                columns: table => new
                {
                    ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: false),
                    LevelId = table.Column<double>(nullable: false),
                    StartRange = table.Column<double>(nullable: true),
                    EndRange = table.Column<double>(nullable: true),
                    ConversionTableModifiedDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    ConversionTableStatus = table.Column<string>(maxLength: 255, nullable: true),
                    ConversionTableCreateDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    StartRangeScoreDisplayed = table.Column<double>(nullable: true),
                    EndRangeScoreDisplayed = table.Column<double>(nullable: true),
                    ConversionTableScoreOrder = table.Column<string>(maxLength: 255, nullable: true),
                    ConversionTableFinalScore = table.Column<double>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Convertion_table_pkey", x => new { x.ModelComponentGuid, x.LevelId });
                    table.ForeignKey(
                        name: "FK_Convertion_tables_Model_Component",
                        column: x => x.ModelComponentGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormElementConnection",
                columns: table => new
                {
                    FormElementGuid = table.Column<string>(maxLength: 50, nullable: false),
                    ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: false),
                    Order = table.Column<int>(defaultValue: 1, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Form_Element_Connection_pkey", x => new { x.FormElementGuid, x.ModelComponentGuid });
                    table.ForeignKey(
                        name: "FK_Form_Element_Connection_Form_Element",
                        column: x => x.FormElementGuid,
                        principalTable: "Form_Element",
                        principalColumn: "FormElementGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Form_Element_Connection_Model_Component",
                        column: x => x.ModelComponentGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
               name: "OrganizationUnion",
               columns: table => new
               {
                   OrganizationUnionId = table.Column<int>(nullable: false)
                   .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                   OrganizationUnionGuid = table.Column<string>(nullable: false),
                   OrgObjGuid = table.Column<string>(nullable: true),
                   ParentOrgObjGuid = table.Column<string>(nullable: true),
                   Order = table.Column<int>(nullable: false),
                   Name = table.Column<string>(nullable: true),
                   Description = table.Column<string>(nullable: true),
                   ModifiedDate = table.Column<DateTime>(nullable: true)
               },
                constraints: table =>
                {
                    table.PrimaryKey("OrganizationUnion_pkey", x => x.OrganizationUnionId);
                    table.ForeignKey(
                       name: "FK_OrganizationUnion1_OrganizationObject",
                       column: x => x.OrgObjGuid,
                       principalTable: "OrganizationObject",
                       principalColumn: "OrgObjGuid",
                       onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                     name: "FK_OrganizationUnion2_OrganizationObject",
                     column: x => x.ParentOrgObjGuid,
                     principalTable: "OrganizationObject",
                     principalColumn: "OrgObjGuid",
                     onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModelDescription",
                columns: table => new
                {
                    ModelDescriptionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: false),
                    EntityDescriptionGuid = table.Column<int>(nullable: true),
                    DescriptionGuid = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelDescription", x => x.ModelDescriptionId);
                    table.ForeignKey(
                        name: "FK_Model_Description_Description",
                        column: x => x.DescriptionGuid,
                        principalTable: "Description",
                        principalColumn: "DescriptionGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Model_Description_Models",
                        column: x => x.ModelComponentGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ModelStructure",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: false),
                    ModelComponentParentGuid = table.Column<string>(maxLength: 50, nullable: false),
                    ModelComponentOrigionGuid = table.Column<string>(maxLength: 50, nullable: true),
                    ModelComponentType = table.Column<int>(nullable: true),
                    AllOrigins = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelStructure", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Model_Structure_Model_Component",
                        column: x => x.ModelComponentGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Model_Structure_Model_Component2",
                        column: x => x.ModelComponentOrigionGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Model_Structure_Model_Component1",
                        column: x => x.ModelComponentParentGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Model_Structure_Model_Component_Type",
                        column: x => x.ModelComponentType,
                        principalTable: "ModelComponentType",
                        principalColumn: "TypeGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrgModelPolygon",
                columns: table => new
                {
                    OrgObjGuid = table.Column<string>(maxLength: 50, nullable: false),
                    ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: false),
                    PolygonGuid = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Org_Model_Polygon_pkey", x => new { x.OrgObjGuid, x.ModelComponentGuid, x.PolygonGuid });
                    table.ForeignKey(
                        name: "model_component_guid_pkey",
                        column: x => x.ModelComponentGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "org_obj_guid_pkey",
                        column: x => x.OrgObjGuid,
                        principalTable: "OrganizationObject",
                        principalColumn: "OrgObjGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OutSourceScore",
                columns: table => new
                {
                    UserGuid = table.Column<string>(type: "character varying", nullable: false),
                    ModelComponentGuid = table.Column<string>(type: "character varying", nullable: false),
                    EventDate = table.Column<DateTime>(type: "date", nullable: false),
                    Score = table.Column<string>(type: "character varying", nullable: false),
                    FormType = table.Column<int>(type: "integer", nullable: false),
                    AverageScore = table.Column<string>(type: "character varying", nullable: true),
                    EvaluatingCount = table.Column<int>(type: "integer", nullable: true),
                    CandidateUnit = table.Column<string>(type: "character varying", nullable: true),
                    CandidateRank = table.Column<string>(type: "character varying", nullable: true),
                    CandidateRole = table.Column<string>(type: "character varying", nullable: true),
                    TextAnswerQuestion = table.Column<string>(type: "character varying", nullable: true),
                    TextAnswerSummary = table.Column<string>(type: "character varying", nullable: true)

                },
                constraints: table =>
                {
                    table.PrimaryKey("OutSource_Score_pkey", x => new { x.UserGuid, x.ModelComponentGuid, x.EventDate });
                    table.ForeignKey(
                        name: "FK_OutSourceScore_ModelComponent_ModelComponentGuid",
                        column: x => x.ModelComponentGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThresholdLevels",
                columns: table => new
                {
                    LevelId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ThresholdLevels_pkey", x => x.LevelId);
                });

            migrationBuilder.CreateTable(
               name: "ThresholdOriginCondition",
               columns: table => new
               {
                   OriginConditionId = table.Column<int>(nullable: false),
                   Name = table.Column<string>(maxLength: 50, nullable: false)
               },
                constraints: table =>
                {
                    table.PrimaryKey("ThresholdOriginCondition_pkey", x => x.OriginConditionId);
                });

            migrationBuilder.CreateTable(
                name: "ThresholdsDestinationCondition",
                columns: table => new
                {
                    DestinationConditionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ThresholdsDestinationCondition_pkey", x => x.DestinationConditionId);
                });

            migrationBuilder.CreateTable(
                name: "Threshold",
                columns: table => new
                {
                    ThresholdGuid = table.Column<string>(maxLength: 50, nullable: false),
                    CreateDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: false),
                    ModifiedDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    ModelComponentOriginGuid = table.Column<string>(maxLength: 50, nullable: false),
                    OriginCondition = table.Column<int>(nullable: false),
                    OriginScore = table.Column<double>(nullable: true),
                    OriginLevel = table.Column<int>(nullable: true),
                    IsOriginLevel = table.Column<bool>(nullable: false),
                    ModelComponentDestinationGuid = table.Column<string>(maxLength: 50, nullable: false),
                    DestinationCondition = table.Column<int>(nullable: false),
                    DestinationScore = table.Column<double>(nullable: true),
                    DestinationLevel = table.Column<int>(nullable: true),
                    IsDestinationLevel = table.Column<bool>(nullable: false),
                    AutoMessage = table.Column<string>(maxLength: 1000, nullable: true),
                    FreeMessage = table.Column<string>(maxLength: 1000, nullable: true),
                    Recommendations = table.Column<string>(maxLength: 1000, nullable: true),
                    Explanations = table.Column<string>(maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Threshold_pkey", x => x.ThresholdGuid);
                    table.ForeignKey(
                        name: "FK_Threshold_Model_Component_Origin",
                        column: x => x.ModelComponentOriginGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                       name: "FK_Threshold_Model_Component_Destination",
                       column: x => x.ModelComponentDestinationGuid,
                       principalTable: "ModelComponent",
                       principalColumn: "ModelComponentGuid",
                       onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                       name: "FK_Thresholds_Condition_Origin",
                       column: x => x.OriginCondition,
                       principalTable: "ThresholdOriginCondition",
                       principalColumn: "OriginConditionId",
                       onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                       name: "FK_Thresholds_Condition_Destination",
                       column: x => x.DestinationCondition,
                       principalTable: "ThresholdsDestinationCondition",
                       principalColumn: "DestinationConditionId",
                       onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormTemplateStructure",
                columns: table => new
                {
                    FormTemplateStructureId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FormTemplateGuid = table.Column<string>(maxLength: 50, nullable: false),
                    ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: true),
                    FormElementGuid = table.Column<string>(maxLength: 50, nullable: true),
                    Order = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FormTemplateStructure", x => x.FormTemplateStructureId);
                    table.ForeignKey(
                        name: "FK_Form_Template_Structure_Form_Element",
                        column: x => x.FormElementGuid,
                        principalTable: "Form_Element",
                        principalColumn: "FormElementGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Form_Template_Structure_Model_Component",
                        column: x => x.ModelComponentGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavedReportConnection",
                columns: table => new
                {
                    ConnectionId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReportGuid = table.Column<string>(maxLength: 50, nullable: true),
                    OrgObjGuid = table.Column<string>(maxLength: 50, nullable: true),
                    Focus = table.Column<string>(maxLength: 50, nullable: true),
                    Comment = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Saved_Report_Connection_pkey", x => x.ConnectionId);
                    table.ForeignKey(
                        name: "FK_Saved_Report_Connection_Model_Component1",
                        column: x => x.Comment,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Saved_Report_Connection_Model_Component",
                        column: x => x.Focus,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Saved_Report_Connection_Organization_Object",
                        column: x => x.OrgObjGuid,
                        principalTable: "OrganizationObject",
                        principalColumn: "OrgObjGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SavedReports",
                columns: table => new
                {
                    ReportGuid = table.Column<string>(maxLength: 50, nullable: false),
                    UserGuid = table.Column<string>(maxLength: 50, nullable: false),
                    ModelComponentGuid = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    CalculatedDates = table.Column<string>(type: "character varying", nullable: false),
                    IsWatch = table.Column<bool>(nullable: false),
                    IsPrimary = table.Column<bool>(nullable: false),
                    IsSecondary = table.Column<bool>(nullable: false),
                    ReportType = table.Column<int>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    TemplateType = table.Column<int>(nullable: true, defaultValue: 0),
                    IsDefined = table.Column<bool>(nullable: true),
                    CandidateUserGuid = table.Column<string>(maxLength: 50, nullable: true),
                    UnionGuid = table.Column<string>(maxLength: 50, defaultValue: "1", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Saved_reports_pkey", x => x.ReportGuid);
                    table.ForeignKey(
                        name: "FK_Saved_reports_Model_Component",
                        column: x => x.ModelComponentGuid,
                        principalTable: "ModelComponent",
                        principalColumn: "ModelComponentGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Saved_reports_Report_Type",
                        column: x => x.ReportType,
                        principalTable: "ReportType",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<int>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 50, nullable: false),
                    Description = table.Column<string>(maxLength: 255, nullable: false),
                    OrgObjGuid = table.Column<string>(maxLength: 50, nullable: false),
                    UpdateDate = table.Column<string>(maxLength: 14, nullable: false),
                    UpdateUserId = table.Column<string>(maxLength: 50, nullable: false),
                    Status = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                    table.ForeignKey(
                        name: "FK_Users_Organization_Object",
                        column: x => x.OrgObjGuid,
                        principalTable: "OrganizationObject",
                        principalColumn: "OrgObjGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserGuid = table.Column<string>(maxLength: 50, nullable: false),
                    UserId = table.Column<string>(maxLength: 50, nullable: false),
                    UserName = table.Column<string>(maxLength: 255, nullable: true),
                    UserPassword = table.Column<string>(maxLength: 255, nullable: true),
                    UserFirstName = table.Column<string>(maxLength: 255, nullable: true),
                    UserLastName = table.Column<string>(maxLength: 255, nullable: true),
                    UserBusinessPhone = table.Column<string>(maxLength: 255, nullable: true),
                    UserMobilePhone = table.Column<string>(maxLength: 255, nullable: true),
                    UserNotes = table.Column<string>(maxLength: 255, nullable: true),
                    UserModifiedDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    UserStatus = table.Column<string>(maxLength: 255, nullable: true),
                    JobTitleGuid = table.Column<string>(maxLength: 50, nullable: true),
                    OrgObjGuid = table.Column<string>(maxLength: 50, nullable: true),
                    UserAdminPermission = table.Column<int>(nullable: false),
                    UserCreateDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    UserEmail = table.Column<string>(maxLength: 255, nullable: true),
                    RoleId = table.Column<int>(nullable: false),
                    UserType = table.Column<int>(nullable: false, defaultValueSql: "1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("User_pkey", x => x.UserGuid);
                    table.ForeignKey(
                        name: "FK_Users_Organization_Object",
                        column: x => x.OrgObjGuid,
                        principalTable: "OrganizationObject",
                        principalColumn: "OrgObjGuid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Roles_Id",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserType_User",
                        column: x => x.UserType,
                        principalTable: "UserType",
                        principalColumn: "UserTypeId",
                        onDelete: ReferentialAction.Restrict);
                });
            migrationBuilder.CreateTable(
               name: "UserPreference",
               columns: table => new
               {
                   UserGuid = table.Column<string>(maxLength: 50, nullable: false),
                   UserTheme = table.Column<int>(nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("UserPreference_pkey", x => x.UserGuid);
                   table.ForeignKey(
                       name: "FK_UserPreference_User",
                       column: x => x.UserGuid,
                       principalTable: "User",
                       principalColumn: "UserGuid",
                       onDelete: ReferentialAction.Restrict);
               });

            migrationBuilder.CreateTable(
             name: "SavedReportsConnectionInterface",
             columns: table => new
             {
                 ReportId = table.Column<int>(nullable: false)
                 .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                 ReportGuid = table.Column<string>(maxLength: 50, nullable: true, type: "character varying"),
                 ReportGuidInterFace = table.Column<string>(maxLength: 50, nullable: true, type: "character varying"),
             },
             constraints: table =>
             {
                 table.PrimaryKey("SavedReportsConnectionInterface_pkey", x => x.ReportId);

             });

            migrationBuilder.CreateTable(
                name: "Candidate",
                columns: table => new
                {
                    UserGuid = table.Column<string>(maxLength: 50, nullable: false),
                    IdValue = table.Column<string>(maxLength: 50, nullable: false),
                    Gender = table.Column<int>(nullable: false),
                    Birthdate = table.Column<DateTime>(type: "date", nullable: true),
                    PersonalNumber = table.Column<int>(nullable: true),
                    EnrichmentId = table.Column<string>(type: "character varying", nullable: true),
                    RankId = table.Column<int>(nullable: true),
                    StandardRank = table.Column<int>(nullable: true),
                    CorpId = table.Column<int>(nullable: true),
                    AreaCode = table.Column<string>(type: "character varying", nullable: true),
                    Kaba = table.Column<double>(nullable: true),
                    Dapar = table.Column<double>(nullable: true),
                    ServiceType = table.Column<int>(nullable: true),
                    Job = table.Column<int>(nullable: true),
                    MinuyText = table.Column<string>(type: "character varying", nullable: true),
                    Slabs = table.Column<string>(type: "character varying", nullable: true),
                    CourseOfStudy1 = table.Column<int>(nullable: true),
                    CourseOfStudy2 = table.Column<int>(nullable: true),
                    ReadyKidum = table.Column<string>(type: "character varying", nullable: true),
                    ObviousGood = table.Column<string>(type: "character varying", nullable: true),
                    SuccessMeasur = table.Column<int>(nullable: true),
                    ZgrantDate = table.Column<DateTime>(type: "date", nullable: true),
                    Zdecoration = table.Column<int>(nullable: true),
                    StartDateUnit = table.Column<DateTime>(type: "date", nullable: true),
                    EndDateUnit = table.Column<DateTime>(type: "date", nullable: true),
                    StartDateRank = table.Column<DateTime>(type: "date", nullable: true),
                    EndDateRank = table.Column<DateTime>(type: "date", nullable: true),
                    Photo = table.Column<byte[]>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Candidate_pkey", x => x.UserGuid);
                    table.ForeignKey(
                        name: "FK_Candidate_User",
                        column: x => x.UserGuid,
                        principalTable: "User",
                        principalColumn: "UserGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FormTemplate",
                columns: table => new
                {
                    FormTemplateGuid = table.Column<string>(maxLength: 50, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    ModifiedDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    CreateDate = table.Column<string>(fixedLength: true, maxLength: 14, nullable: true),
                    CreatorUserGuid = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Form_Template_pkey", x => x.FormTemplateGuid);
                    table.ForeignKey(
                        name: "FK_Form_templates_Users",
                        column: x => x.CreatorUserGuid,
                        principalTable: "User",
                        principalColumn: "UserGuid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
               name: "PersonalUnit",
               columns: table => new
               {
                   PersonalUnitGuid = table.Column<string>(nullable: false),
                   OrgObjGuid = table.Column<string>(nullable: false),
                   IdentityNumber = table.Column<string>(nullable: true),
                   FirstName = table.Column<string>(nullable: true),
                   LastName = table.Column<string>(nullable: true),
                   RoleName = table.Column<string>(nullable: true),
                   Shift = table.Column<string>(nullable: true),
                   Rank = table.Column<string>(nullable: true),
                   MilitaryProfession = table.Column<string>(nullable: true),
                   Profile = table.Column<int>(nullable: true),
                   BirthDate = table.Column<DateTime>(nullable: true),
                   RegularReleaseDate = table.Column<DateTime>(nullable: true),
                   ReserveReleaseDate = table.Column<DateTime>(nullable: true),
                   ShamapOneYear = table.Column<int>(nullable: true),
                   ShamapThreeYear = table.Column<int>(nullable: true),
                   RoleNameRank = table.Column<string>(nullable: true),
                   Certification = table.Column<bool>(nullable: true),
                   CertificationType = table.Column<string>(nullable: true),
                   RefreshingValid = table.Column<bool>(nullable: true),
                   RefreshingLastDate = table.Column<DateTime>(nullable: true),
                   Address = table.Column<string>(nullable: true),
                   PhoneNumber = table.Column<string>(nullable: true),
                   MobileNumber = table.Column<string>(nullable: true),
                   EmailAddress = table.Column<string>(nullable: true),
                   CivilJob = table.Column<string>(nullable: true)
               },
                constraints: table =>
                {
                    table.PrimaryKey("PersonalUnit_pkey", x => x.PersonalUnitGuid);
                    table.ForeignKey(
                       name: "FK_PersonalUnit_OrganizationObject",
                       column: x => x.OrgObjGuid,
                       principalTable: "OrganizationObject",
                       principalColumn: "OrgObjGuid",
                       onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
         name: "ActivityFile",
         columns: table => new
         {
             ActivityFileGuid = table.Column<string>(nullable: false),
             ActivityGuid = table.Column<string>(nullable: false),
             FileName = table.Column<string>(nullable: false),
             Content = table.Column<string>(nullable: false)
         },
          constraints: table =>
          {
              table.PrimaryKey("ActivityFile_pkey", x => x.ActivityFileGuid);
              table.ForeignKey(
                 name: "FK_ActivityFile_Activity",
                 column: x => x.ActivityGuid,
                 principalTable: "Activity",
                 principalColumn: "ActivityGuid",
                 onDelete: ReferentialAction.Restrict);
          });



            migrationBuilder.CreateIndex(
                name: "IX_Activity_ActivityTemplateGuid",
                table: "Activity",
                column: "ActivityTemplateGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_OrgObjGuid",
                table: "Activity",
                column: "OrgObjGuid");

            migrationBuilder.CreateIndex(
                name: "IX_AtInFt_FormTemplateGuid",
                table: "AtInFt",
                column: "FormTemplateGuid");

            migrationBuilder.CreateIndex(
                name: "IX_CalculateScore_ActivityGuid",
                table: "CalculateScore",
                column: "ActivityGuid");

            migrationBuilder.CreateIndex(
                name: "IX_CalculateScore_FormElementGuid",
                table: "CalculateScore",
                column: "FormElementGuid");

            migrationBuilder.CreateIndex(
                name: "IX_CalculateScore_FormGuid",
                table: "CalculateScore",
                column: "FormGuid");

            migrationBuilder.CreateIndex(
                name: "IX_CalculateScore_ModelComponentGuid",
                table: "CalculateScore",
                column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_CalculateScore_OrgObjGuid",
                table: "CalculateScore",
                column: "OrgObjGuid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_EstimatedOrganizationObject_ActivityGuid",
            //    table: "EstimatedOrganizationObject",
            //    column: "ActivityGuid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_EstimatedOrganizationObject_OrgObjEstimatedGuid",
            //    table: "EstimatedOrganizationObject",
            //    column: "OrgObjEstimatedGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Form_ActivityGuid",
                table: "Form",
                column: "ActivityGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Form_ApproveUserGuid",
                table: "Form",
                column: "ApproveUserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Form_FormTemplateGuid",
                table: "Form",
                column: "FormTemplateGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Form_OrgObjGuid",
                table: "Form",
                column: "OrgObjGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Form_Status",
                table: "Form",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Form_Element_FormElementType",
                table: "Form_Element",
                column: "FormElementType");

            migrationBuilder.CreateIndex(
                name: "IX_FormElementConnection_ModelComponentGuid",
                table: "FormElementConnection",
                column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_FormTemplate_CreatorUserGuid",
                table: "FormTemplate",
                column: "CreatorUserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_FormTemplateStructure_FormElementGuid",
                table: "FormTemplateStructure",
                column: "FormElementGuid");

            migrationBuilder.CreateIndex(
                name: "IX_FormTemplateStructure_FormTemplateGuid",
                table: "FormTemplateStructure",
                column: "FormTemplateGuid");

            migrationBuilder.CreateIndex(
                name: "IX_FormTemplateStructure_ModelComponentGuid",
                table: "FormTemplateStructure",
                column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelComponent_MetricCalenderRollup",
                table: "ModelComponent",
                column: "MetricCalenderRollup");

            migrationBuilder.CreateIndex(
                name: "IX_ModelComponent_MetricMeasuringUnit",
                table: "ModelComponent",
                column: "MetricMeasuringUnit");

            migrationBuilder.CreateIndex(
                name: "IX_ModelComponent_MetricRollupMethod",
                table: "ModelComponent",
                column: "MetricRollupMethod");

            migrationBuilder.CreateIndex(
                name: "IX_ModelComponent_MetricSource",
                table: "ModelComponent",
                column: "MetricSource");

            migrationBuilder.CreateIndex(
                name: "IX_ModelComponent_ModifiedUserGuid",
                table: "ModelComponent",
                column: "ModifiedUserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelComponent_Source",
                table: "ModelComponent",
                column: "Source");

            migrationBuilder.CreateIndex(
                name: "IX_ModelComponent_Status",
                table: "ModelComponent",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_ModelDescription_DescriptionGuid",
                table: "ModelDescription",
                column: "DescriptionGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelDescription_ModelComponentGuid",
                table: "ModelDescription",
                column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStructure_ModelComponentGuid",
                table: "ModelStructure",
                column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStructure_ModelComponentOrigionGuid",
                table: "ModelStructure",
                column: "ModelComponentOrigionGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStructure_ModelComponentParentGuid",
                table: "ModelStructure",
                column: "ModelComponentParentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_ModelStructure_ModelComponentType",
                table: "ModelStructure",
                column: "ModelComponentType");

            migrationBuilder.CreateIndex(
                   name: "IX_OrganizationObjectConnection_ActivityTemplateGuid",
                   table: "OrganizationObjectConnection",
                   column: "ActivityTemplateGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationObjectConnection_DescriptionGuid",
                table: "OrganizationObjectConnection",
                column: "DescriptionGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationObjectConnection_ModelComponentGuid",
                table: "OrganizationObjectConnection",
                column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationObjectConnection_OrgObjGuid",
                table: "OrganizationObjectConnection",
                column: "OrgObjGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationStructure_OrgObjParentGuid",
                table: "OrganizationStructure",
                column: "OrgObjParentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrganizationStructure_OrgParentGuid",
                table: "OrganizationStructure",
                column: "OrgParentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OrgModelPolygon_ModelComponentGuid",
                table: "OrgModelPolygon",
                column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_OutSourceScore_ModelComponentGuid",
                table: "OutSourceScore",
                column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionTypeId",
                table: "RolePermissions",
                column: "PermissionTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_OrgObjGuid",
                table: "Roles",
                column: "OrgObjGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UpdateUserId",
                table: "Roles",
                column: "UpdateUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SavedReportConnection_Comment",
                table: "SavedReportConnection",
                column: "Comment");

            migrationBuilder.CreateIndex(
                name: "IX_SavedReportConnection_Focus",
                table: "SavedReportConnection",
                column: "Focus");

            migrationBuilder.CreateIndex(
                name: "IX_SavedReportConnection_OrgObjGuid",
                table: "SavedReportConnection",
                column: "OrgObjGuid");

            migrationBuilder.CreateIndex(
                name: "IX_SavedReportConnection_ReportGuid",
                table: "SavedReportConnection",
                column: "ReportGuid");

            migrationBuilder.CreateIndex(
                name: "IX_SavedReports_ModelComponentGuid",
                table: "SavedReports",
                column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_SavedReports_ReportType",
                table: "SavedReports",
                column: "ReportType");

            migrationBuilder.CreateIndex(
                name: "IX_SavedReports_UserGuid",
                table: "SavedReports",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Score_ActivityGuid",
                table: "Score",
                column: "ActivityGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Score_FormElementGuid",
                table: "Score",
                column: "FormElementGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Score_FormGuid",
                table: "Score",
                column: "FormGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Score_ModelComponentGuid",
                table: "Score",
                column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Score_OrgObjGuid",
                table: "Score",
                column: "OrgObjGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Score_Status",
                table: "Score",
                column: "Status");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Threshold_ModelComponentGuid",
            //    table: "Threshold",
            //    column: "ModelComponentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_UnitParentGuid",
                table: "Unit",
                column: "UnitParentGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Unit_EntityTypeGuid",
                table: "Unit",
                column: "EntityTypeGuid");

            migrationBuilder.CreateIndex(
                name: "IX_User_OrgObjGuid",
                table: "User",
                column: "OrgObjGuid");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleId",
                table: "User",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_User_UserType",
                table: "User",
                column: "UserType");

            migrationBuilder.AddForeignKey(
                name: "FK_Calculate_Score_Form",
                table: "CalculateScore",
                column: "FormGuid",
                principalTable: "Form",
                principalColumn: "FormGuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Calculate_Score_Model_Component",
                table: "CalculateScore",
                column: "ModelComponentGuid",
                principalTable: "ModelComponent",
                principalColumn: "ModelComponentGuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Form_templates",
                table: "Form",
                column: "FormTemplateGuid",
                principalTable: "FormTemplate",
                principalColumn: "FormTemplateGuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Forms_Users",
                table: "Form",
                column: "ApproveUserGuid",
                principalTable: "User",
                principalColumn: "UserGuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Score_Model_Component",
                table: "Score",
                column: "ModelComponentGuid",
                principalTable: "ModelComponent",
                principalColumn: "ModelComponentGuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AT_In_FT_Form_Template",
                table: "AtInFt",
                column: "FormTemplateGuid",
                principalTable: "FormTemplate",
                principalColumn: "FormTemplateGuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Model_Component_User",
                table: "ModelComponent",
                column: "ModifiedUserGuid",
                principalTable: "User",
                principalColumn: "UserGuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Form_Template_Structure_Form_Template",
                table: "FormTemplateStructure",
                column: "FormTemplateGuid",
                principalTable: "FormTemplate",
                principalColumn: "FormTemplateGuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Saved_Report_Connection_Saved_reports",
                table: "SavedReportConnection",
                column: "ReportGuid",
                principalTable: "SavedReports",
                principalColumn: "ReportGuid",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Saved_reports_User",
                table: "SavedReports",
                column: "UserGuid",
                principalTable: "User",
                principalColumn: "UserGuid",
                onDelete: ReferentialAction.Restrict);
            migrationBuilder.Sql("INSERT INTO public.Score('ScoreId')VALUES(1)");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Users",
            //    table: "Roles",
            //    column: "UpdateUserId",
            //    principalTable: "User",
            //    principalColumn: "UserGuid",
            //    onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.Sql(
            //@"CREATE TYPE EntityType AS (
            //        type           integer ,
            //        guid    character varying(50) 

            //    );");



            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organization_Object",
                table: "Roles");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Organization_Object",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_Users",
                table: "Roles");

            migrationBuilder.DropTable(
                name: "AtInFt");

            migrationBuilder.DropTable(
                name: "CalculateScore");

            migrationBuilder.DropTable(
                name: "Candidate");

            migrationBuilder.DropTable(
                name: "ConvertionTable");

            //migrationBuilder.DropTable(
            //    name: "EstimatedOrganizationObject");

            migrationBuilder.DropTable(
                name: "FormElementConnection");

            migrationBuilder.DropTable(
                name: "FormTemplateStructure");

            migrationBuilder.DropTable(
                name: "ModelDescription");

            migrationBuilder.DropTable(
                name: "ModelStructure");

            migrationBuilder.DropTable(
                        name: "OrganizationObjectConnection");

            //migrationBuilder.DropTable(
            //    name: "OrganizationStructure");

            migrationBuilder.DropTable(
                name: "OrgModelPolygon");

            migrationBuilder.DropTable(
                name: "OutSourceScore");

            migrationBuilder.DropTable(
                name: "RoleItems");

            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "SavedReportConnection");

            migrationBuilder.DropTable(
                name: "SavedReportData");

            migrationBuilder.DropTable(
                name: "Score");

            migrationBuilder.DropTable(
                name: "SystemJobTitles");

            migrationBuilder.DropTable(
                name: "Threshold");

            migrationBuilder.DropTable(
                name: "ThresholdLevels");

            migrationBuilder.DropTable(
              name: "ThresholdOriginCondition");

            migrationBuilder.DropTable(
                name: "ThresholdsDestinationCondition");

            migrationBuilder.DropTable(
                name: "ModelComponentType");

            migrationBuilder.DropTable(
                name: "Description");

            migrationBuilder.DropTable(
                name: "PermissionTypes");

            migrationBuilder.DropTable(
                name: "SavedReports");

            migrationBuilder.DropTable(
                name: "Form_Element");

            migrationBuilder.DropTable(
                name: "Form");

            migrationBuilder.DropTable(
                name: "Unit");

            migrationBuilder.DropTable(
                name: "ModelComponent");

            migrationBuilder.DropTable(
                name: "ReportType");

            migrationBuilder.DropTable(
                name: "FormElementType");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "FormTemplate");

            migrationBuilder.DropTable(
                name: "FormStatus");

            migrationBuilder.DropTable(
                name: "CalenderRollup");

            migrationBuilder.DropTable(
                name: "MeasuringUnit");

            migrationBuilder.DropTable(
                name: "RollupMethod");

            migrationBuilder.DropTable(
                name: "ModelComponentSource");

            migrationBuilder.DropTable(
                name: "ModelComponentStatus");

            migrationBuilder.DropTable(
                name: "ActivityTemplate");

            migrationBuilder.DropTable(
                name: "OrganizationObject");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "UserType");

            migrationBuilder.DropSequence(
                name: "Form_Template_Structure_form_template_structure_id_seq");
        }

    }
}


//Expamles:
//migrationBuilder.DropColumn(name: "Name1", table: "Activity");
//migrationBuilder.AddColumn<string>(name: "Name1", table: "Activity", nullable: true);
//migration.RenameTable(name: "AspNetUsers", newName: "Users");
//migration.DropTable("Member");
//migrationBuilder.RenameColumn(name: "PhoneNumber", table: "AspNetUsers", newName: "Phonenumber");
//migration.DropColumn(name: "MemberId", table: "Registration");
//migrationBuilder.AlterColumn<DateTime>(name: "LastUpdated", table: "ProductCategories", nullable: false, defaultValueSql: "GetDate()");
//migrationBuilder.DropForeignKey(name: "FK_Translation_Content_ContentId", table: "Translation");
//migrationBuilder.DropPrimaryKey(name: "PK_Content", table: "Content");
//migrationBuilder.DropColumn(name: "Id", table: "Content");
//migrationBuilder.AddColumn<Guid>(name: "ContentGuid", table: "Content", nullable: false, defaultValueSql: "newsequentialid()");
//migrationBuilder.AddPrimaryKey(name: "PK_Content", table: "Content", column: "ContentGuid");
//migrationBuilder.AddForeignKey(
//    name: "FK_Translation_Content_ContentContentGuid",
//    table: "Translation",
//    column: "ContentContentGuid",
//    principalTable: "Content",
//    principalColumn: "ContentGuid",
//    onDelete: ReferentialAction.Restrict);
//migrationBuilder.CreateTable(
//    name: "SavedReportsConnectionInterface",
//    columns: table => new
//    {
//        ReportId = table.Column<int>(nullable: false)
//            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
//        ReportGuid = table.Column<string>(maxLength: 50, nullable: true),
//        ReportGuidInterFace = table.Column<string>(maxLength: 50, nullable: true),
//    },
//    constraints: table =>
//    {
//        table.PrimaryKey("SavedReportsConnectionInterface_pkey", x => new { x.ReportGuid, x.ReportGuidInterFace });
//        //table.ForeignKey(
//        //    name: "FK_SavedReportConnectionInterface_Saved_reports",
//        //    column: x => new { x.ReportGuidInterFace, x.ReportGuid },
//        //    principalTable: "SavedReports",
//        //    principalColumn: "ReportGuid",
//        //    onDelete: ReferentialAction.Cascade);
//    });
