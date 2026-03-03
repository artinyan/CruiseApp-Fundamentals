using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CruiseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCascadeDeleteToCruiseCabinPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CruiseCabinPrices_Cruises_CruiseId",
                table: "CruiseCabinPrices");

            migrationBuilder.AddForeignKey(
                name: "FK_CruiseCabinPrices_Cruises_CruiseId",
                table: "CruiseCabinPrices",
                column: "CruiseId",
                principalTable: "Cruises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CruiseCabinPrices_Cruises_CruiseId",
                table: "CruiseCabinPrices");

            migrationBuilder.AddForeignKey(
                name: "FK_CruiseCabinPrices_Cruises_CruiseId",
                table: "CruiseCabinPrices",
                column: "CruiseId",
                principalTable: "Cruises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
