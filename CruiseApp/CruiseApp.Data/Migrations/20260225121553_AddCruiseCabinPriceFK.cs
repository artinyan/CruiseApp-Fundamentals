using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CruiseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCruiseCabinPriceFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_CruiseCabinPrices_Cruises_CruiseId",
                table: "CruiseCabinPrices",
                column: "CruiseId",
                principalTable: "Cruises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CruiseCabinPrices_Cruises_CruiseId",
                table: "CruiseCabinPrices");
        }
    }
}
