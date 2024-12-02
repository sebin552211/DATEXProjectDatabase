using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATEX_ProjectDatabase.Migrations
{
    /// <inheritdoc />
    public partial class PMrectification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectManagers_Projects_ProjectId",
                table: "ProjectManagers");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectManagers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectManagers_Projects_ProjectId",
                table: "ProjectManagers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectManagers_Projects_ProjectId",
                table: "ProjectManagers");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectId",
                table: "ProjectManagers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectManagers_Projects_ProjectId",
                table: "ProjectManagers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "ProjectId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
