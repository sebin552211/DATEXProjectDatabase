using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATEX_ProjectDatabase.Migrations
{
    /// <inheritdoc />
    public partial class hangfire : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectManagers_ProjectManager",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectManagers_ProjectManager",
                table: "Projects",
                column: "ProjectManager",
                principalTable: "ProjectManagers",
                principalColumn: "Name",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ProjectManagers_ProjectManager",
                table: "Projects");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ProjectManagers_ProjectManager",
                table: "Projects",
                column: "ProjectManager",
                principalTable: "ProjectManagers",
                principalColumn: "Name");
        }
    }
}
