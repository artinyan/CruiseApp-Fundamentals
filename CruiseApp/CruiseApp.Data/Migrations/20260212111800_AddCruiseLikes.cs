using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CruiseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCruiseLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CruiseLike",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User who liked the cruise."),
                    CruiseId = table.Column<int>(type: "int", nullable: false, comment: "Cruise which is liked from the user.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CruiseLike", x => new { x.UserId, x.CruiseId });
                    table.ForeignKey(
                        name: "FK_CruiseLike_Cruises_CruiseId",
                        column: x => x.CruiseId,
                        principalTable: "Cruises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents likes for a cruise.");

            migrationBuilder.CreateIndex(
                name: "IX_CruiseLike_CruiseId",
                table: "CruiseLike",
                column: "CruiseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CruiseLike");
        }
    }
}
