
using Infrastructure.Migrations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Model.Entities;

namespace Dal.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("31_01_22_Migration")]
    public partial class _31_01_22_Migration : MigrationEnsured
    {
        protected override void Up(MigrationBuilderEnsured migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(name: "AnonymousEvaluation", table: "Activity", nullable: true, type: "boolean", defaultValue: false);
        }
    }
}
