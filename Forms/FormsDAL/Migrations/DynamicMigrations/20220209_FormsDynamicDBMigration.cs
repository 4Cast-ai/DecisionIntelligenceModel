using FormsDal.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Model.Migrations
{
    [DbContext(typeof(FormsDynamicDBContext))]
    [Migration("20220209_FormsDynamicDBMigration")]
    public partial class FormsDynamicDBMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "ActivityTraceIdSeq",
                startValue: 3L);

            migrationBuilder.CreateTable(
                name: "DynamicEntityType",
                columns: table => new
                {
                    EntityTypeCode = table.Column<int>(type: "integer", nullable: false),
                    EntityTypeName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("EvaluatedType_pkey", x => x.EntityTypeCode);
                });

            migrationBuilder.CreateTable(
                name: "DynamicFormStatus",
                columns: table => new
                {
                    FormStatusCode = table.Column<int>(type: "integer", nullable: false),
                    FormStatusName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FormStatus_pkey", x => x.FormStatusCode);
                });

            migrationBuilder.CreateTable(
                name: "DynamicRecordStatus",
                columns: table => new
                {
                    RecordStatusCode = table.Column<int>(type: "integer", nullable: false),
                    RecordStatusName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RecordStatus_pkey", x => x.RecordStatusCode);
                });

            migrationBuilder.CreateTable(
                name: "DynamicForm",
                columns: table => new
                {
                    FormGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FormStatus = table.Column<int>(type: "integer", nullable: false),
                    ActivityGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EvaluatorGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EvaluatorType = table.Column<int>(type: "integer", nullable: false),
                    EvaluatedGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EvaluatedType = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    UpdateDate = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    LastUpdateUserGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FormTemplateGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Form_pkey", x => x.FormGuid);
                    table.ForeignKey(
                        name: "EvaluatedType",
                        column: x => x.EvaluatedType,
                        principalTable: "DynamicEntityType",
                        principalColumn: "EntityTypeCode");
                    table.ForeignKey(
                        name: "EvluatorTypeFK",
                        column: x => x.EvaluatorType,
                        principalTable: "DynamicEntityType",
                        principalColumn: "EntityTypeCode");
                    table.ForeignKey(
                        name: "FormStatus_FK",
                        column: x => x.FormStatus,
                        principalTable: "DynamicFormStatus",
                        principalColumn: "FormStatusCode");
                });

            migrationBuilder.CreateTable(
                name: "DynamicActivityTrace",
                columns: table => new
                {
                    ActivityGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ActivityName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ActivityStartDate = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    ActivityEndDate = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    IsLimited = table.Column<bool>(type: "boolean", nullable: false),
                    CanSubmitOnce = table.Column<bool>(name: "CanSubmitOnce ", type: "boolean", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "boolean", nullable: false),
                    CreationDate = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    UpdateDate = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    FromEffectDate = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    ToEffectDate = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: true),
                    RecordStatusCode = table.Column<int>(type: "integer", nullable: false),
                    UpdateUserId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EvaluatedAndEvaluators = table.Column<string>(type: "json", nullable: true),
                    Forms = table.Column<string>(type: "json", nullable: true),
                    FormsDBID = table.Column<int>(type: "integer", nullable: true),
                    FormsDBName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ActivityTrace_PK", x => x.ActivityGuid);
                    table.ForeignKey(
                        name: "RecordStatusCode_FK",
                        column: x => x.RecordStatusCode,
                        principalTable: "DynamicRecordStatus",
                        principalColumn: "RecordStatusCode");
                });

            migrationBuilder.CreateTable(
                name: "DynamicScores",
                columns: table => new
                {
                    DynamicScoresID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'', '1', '', '99999999', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    FormGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ModelComponentGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Comment = table.Column<string>(type: "character varying", nullable: true),
                    Score = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FormComponent_pkey", x => x.DynamicScoresID);
                    table.ForeignKey(
                        name: "FormGuid_FK",
                        column: x => x.FormGuid,
                        principalTable: "DynamicForm",
                        principalColumn: "FormGuid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FormsActivityTrace_RecordStatusCode",
                table: "DynamicActivityTrace",
                column: "RecordStatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicForm_EvaluatedType",
                table: "DynamicForm",
                column: "EvaluatedType");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicForm_EvaluatorType",
                table: "DynamicForm",
                column: "EvaluatorType");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicForm_FormStatus",
                table: "DynamicForm",
                column: "FormStatus");

            migrationBuilder.CreateIndex(
                name: "IX_DynamicScores_FormGuid",
                table: "DynamicScores",
                column: "FormGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicActivityTrace");

            migrationBuilder.DropTable(
                name: "DynamicScores");

            migrationBuilder.DropTable(
                name: "DynamicRecordStatus");

            migrationBuilder.DropTable(
                name: "DynamicForm");

            migrationBuilder.DropTable(
                name: "DynamicEntityType");

            migrationBuilder.DropTable(
                name: "DynamicFormStatus");

            migrationBuilder.DropSequence(
                name: "ActivityTraceIdSeq");
        }
    }
}
