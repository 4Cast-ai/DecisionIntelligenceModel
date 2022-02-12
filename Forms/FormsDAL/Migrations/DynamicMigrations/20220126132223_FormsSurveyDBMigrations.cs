using FormsDal.Contexts;
//using Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Model.Migrations
{
    [DbContext(typeof(FormsDynamicDBContext))]
    [Migration("20220126132223_FormsSurveyDBMigrations")]
    public partial class FormsSurveyDBMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "FormsScore",
               columns: table => new
               {
                   ScoreId = table.Column<int>(type: "integer", nullable: false)
                       .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                   ModelComponentGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                   FormElementGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                   EntityGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                   EntityGuidType = table.Column<int>(type: "integer", nullable: false),
                   ActivityGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                   OriginalScore = table.Column<double>(type: "double precision", nullable: true),
                   ConvertionScore = table.Column<double>(type: "double precision", nullable: true),
                   ModelComponentComment = table.Column<string>(type: "character varying", nullable: true),
                   Status = table.Column<int>(type: "integer", nullable: true),
                   FormGuid = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("FormsScore_pkey", x => x.ScoreId);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                           name: "FormsScore");
        }
    }
}
