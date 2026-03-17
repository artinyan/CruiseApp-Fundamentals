using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CruiseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddCabinLayouts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DeckPlanImage",
                table: "Decks",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CabinLayouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary key for the Cabin layout.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeckId = table.Column<int>(type: "int", nullable: false, comment: "The Deck of the Cabin."),
                    CabinId = table.Column<int>(type: "int", nullable: false, comment: "The Cabin."),
                    PosX = table.Column<int>(type: "int", nullable: false, comment: "X coordinate in pixels relative to the top-left corner of the deck image."),
                    PosY = table.Column<int>(type: "int", nullable: false, comment: "Y coordinate in pixels relative to the top-left corner of the deck image.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabinLayouts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabinLayouts_Cabins_CabinId",
                        column: x => x.CabinId,
                        principalTable: "Cabins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CabinLayouts_Decks_DeckId",
                        column: x => x.DeckId,
                        principalTable: "Decks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Represents Cabin layout of Deck of Ship in the system.");

            migrationBuilder.CreateIndex(
                name: "IX_CabinLayouts_CabinId",
                table: "CabinLayouts",
                column: "CabinId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CabinLayouts_DeckId_CabinId",
                table: "CabinLayouts",
                columns: new[] { "DeckId", "CabinId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CabinLayouts_DeckId_PosX_PosY",
                table: "CabinLayouts",
                columns: new[] { "DeckId", "PosX", "PosY" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CabinLayouts");

            migrationBuilder.DropColumn(
                name: "DeckPlanImage",
                table: "Decks");
        }
    }
}
