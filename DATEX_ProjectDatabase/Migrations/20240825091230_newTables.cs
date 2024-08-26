using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATEX_ProjectDatabase.Migrations
{
    /// <inheritdoc />
    public partial class newTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectManagers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectManagers_ProjectId",
                table: "ProjectManagers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectManagers_Projects_ProjectId",
                table: "ProjectManagers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectManagers_Projects_ProjectId",
                table: "ProjectManagers");

            migrationBuilder.DropIndex(
                name: "IX_ProjectManagers_ProjectId",
                table: "ProjectManagers");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectId",
                table: "ProjectManagers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
