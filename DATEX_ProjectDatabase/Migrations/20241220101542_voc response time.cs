using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATEX_ProjectDatabase.Migrations
{
    /// <inheritdoc />
    public partial class vocresponsetime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VOCFeedbackReceivedDate",
                table: "VocAnalyses",
                newName: "Response_Completion_Time");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Response_Completion_Time",
                table: "VocAnalyses",
                newName: "VOCFeedbackReceivedDate");
        }
    }
}
