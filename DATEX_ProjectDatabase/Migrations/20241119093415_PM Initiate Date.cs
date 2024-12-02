using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATEX_ProjectDatabase.Migrations
{
    /// <inheritdoc />
    public partial class PMInitiateDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PMInitiateDate",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PMInitiateDate",
                table: "Projects");
        }
    }
}
