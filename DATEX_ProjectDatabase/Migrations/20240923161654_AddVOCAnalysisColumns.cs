using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DATEX_ProjectDatabase.Migrations
{
    /// <inheritdoc />
    public partial class AddVOCAnalysisColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PMInitiateDate",
                table: "VocAnalyses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "VOCFeedbackReceivedDate",
                table: "VocAnalyses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "VOCRemarks",
                table: "VocAnalyses",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PMInitiateDate",
                table: "VocAnalyses");

            migrationBuilder.DropColumn(
                name: "VOCFeedbackReceivedDate",
                table: "VocAnalyses");

            migrationBuilder.DropColumn(
                name: "VOCRemarks",
                table: "VocAnalyses");
        }
    }
}
