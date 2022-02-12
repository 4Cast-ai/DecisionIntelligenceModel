using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Model.Migrations
{
    public partial class FormsDynamicDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DynamicForm",
                columns: table => new
                {
                    FormRecordId = table.Column<int>(type: "integer", nullable: false),
                    ActivityGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FormGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EvaluatedGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EvaluatorGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreationDate = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    UpdateDate = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Form_pkey", x => x.FormRecordId);
                });

            migrationBuilder.CreateTable(
                name: "DynamicFormComponent",
                columns: table => new
                {
                    FormComponentRecordId = table.Column<int>(type: "integer", nullable: false),
                    FormRecordId = table.Column<int>(type: "integer", nullable: false),
                    ModelComponentGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Score = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Comment = table.Column<string>(type: "character varying", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("FormComponent_pkey", x => x.FormComponentRecordId);
                    table.ForeignKey(
                        name: "FormRecordId_FK",
                        column: x => x.FormRecordId,
                        principalTable: "DynamicForm",
                        principalColumn: "FormRecordId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DynamicFormComponent_FormRecordId",
                table: "DynamicFormComponent",
                column: "FormRecordId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DynamicFormComponent");

            migrationBuilder.DropTable(
                name: "DynamicForm");
        }
    }
}
