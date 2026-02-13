using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CruiseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCruiseLikes2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CruiseLike_Cruises_CruiseId",
                table: "CruiseLike");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CruiseLike",
                table: "CruiseLike");

            migrationBuilder.RenameTable(
                name: "CruiseLike",
                newName: "CruiseLikes");

            migrationBuilder.RenameIndex(
                name: "IX_CruiseLike_CruiseId",
                table: "CruiseLikes",
                newName: "IX_CruiseLikes_CruiseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CruiseLikes",
                table: "CruiseLikes",
                columns: new[] { "UserId", "CruiseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CruiseLikes_Cruises_CruiseId",
                table: "CruiseLikes",
                column: "CruiseId",
                principalTable: "Cruises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CruiseLikes_Cruises_CruiseId",
                table: "CruiseLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CruiseLikes",
                table: "CruiseLikes");

            migrationBuilder.RenameTable(
                name: "CruiseLikes",
                newName: "CruiseLike");

            migrationBuilder.RenameIndex(
                name: "IX_CruiseLikes_CruiseId",
                table: "CruiseLike",
                newName: "IX_CruiseLike_CruiseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CruiseLike",
                table: "CruiseLike",
                columns: new[] { "UserId", "CruiseId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CruiseLike_Cruises_CruiseId",
                table: "CruiseLike",
                column: "CruiseId",
                principalTable: "Cruises",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
