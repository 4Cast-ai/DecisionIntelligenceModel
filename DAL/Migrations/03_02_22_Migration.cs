
using Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Model.Entities;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Dal.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("03_02_22_Migration")]
    public partial class _03_02_22_Migration : MigrationEnsured
    {
        protected override void Up(MigrationBuilderEnsured migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityStatus",
                columns: table => new
                {
                    ActivityStatusId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityStatus", x => x.ActivityStatusId);
                });

            migrationBuilder.AddColumn<bool>(name: "Status", table: "Activity", nullable: true, type: "integer");

            migrationBuilder.AddForeignKey(name: "FK_Activity_ActivityStatus",
                                            table: "Activity",
                                            column: "Status",
                                            principalTable: "ActivityStatus",
                                            principalColumn: "ActivityStatusId",
                                            onDelete: ReferentialAction.SetNull);
        }
    }
}
