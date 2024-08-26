using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATEX_ProjectDatabase.Migrations
{
    /// <inheritdoc />
    public partial class newTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectManagers_Projects_ProjectId",
                table: "ProjectManagers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectManagers_ProjectId",
                table: "ProjectManagers");

            migrationBuilder.DropColumn(
                name: "ProjectCode",
                table: "ProjectManagers");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "ProjectManagers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectManagers",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProjectCode",
                table: "ProjectManagers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagers_ProjectId",
                table: "ProjectManagers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectManagers_Projects_ProjectId",
                table: "ProjectManagers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");
        }
    }
}
