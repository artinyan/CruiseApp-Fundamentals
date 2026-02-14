using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CruiseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCruiseDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "Cruises",
                comment: "Represents cruise in the system.",
                oldComment: "Repersents cruise in the system.");

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "Cruises",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "The route of the cruise");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LastDay",
                table: "Cruises",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "Disembarkation day of the cruise");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FirstDay",
                table: "Cruises",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date",
                oldComment: "Embarkation day of the cruise");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cruises",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Primary key for Cruise.")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cruises",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                comment: "Optional cruise description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cruises");

            migrationBuilder.AlterTable(
                name: "Cruises",
                comment: "Repersents cruise in the system.",
                oldComment: "Represents cruise in the system.");

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "Cruises",
                type: "int",
                nullable: false,
                comment: "The route of the cruise",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "LastDay",
                table: "Cruises",
                type: "date",
                nullable: false,
                comment: "Disembarkation day of the cruise",
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "FirstDay",
                table: "Cruises",
                type: "date",
                nullable: false,
                comment: "Embarkation day of the cruise",
                oldClrType: typeof(DateOnly),
                oldType: "date");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Cruises",
                type: "int",
                nullable: false,
                comment: "Primary key for Cruise.",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
