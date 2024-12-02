using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATEX_ProjectDatabase.Migrations
{
    /// <inheritdoc />
    public partial class PMName : Migration
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
                name: "ProjectId",
                table: "ProjectManagers");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectManager",
                table: "Projects",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProjectManagers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_ProjectManagers_Name",
                table: "ProjectManagers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManager",
                table: "Projects",
                column: "ProjectManager");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectManagers_ProjectManager",
                table: "Projects",
                column: "ProjectManager",
                principalTable: "ProjectManagers",
                principalColumn: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectManagers_ProjectManager",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ProjectManager",
                table: "Projects");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_ProjectManagers_Name",
                table: "ProjectManagers");

            migrationBuilder.AlterColumn<string>(
                name: "ProjectManager",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "ProjectManagers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "ProjectId",
                table: "ProjectManagers",
                type: "int",
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
