using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CruiseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationsAndPrices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CabinType",
                table: "Cabins",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Defines type of the Cabin.");

            migrationBuilder.CreateTable(
                name: "CabinReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary key for the CabinReservation.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CruiseId = table.Column<int>(type: "int", nullable: false, comment: "CruiseId"),
                    CabinId = table.Column<int>(type: "int", nullable: false, comment: "CabinId"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "UserId"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "ReservationStatus"),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "CreatedOn"),
                    CabinType = table.Column<int>(type: "int", nullable: false, comment: "Cabin type at the time of reservation (snapshot)"),
                    PassengersCount = table.Column<int>(type: "int", nullable: false, comment: "Numbers of the passengers in the cabin"),
                    PricePaid = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "The amount paid for this reservation"),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false, comment: "Indicates whether the reservation has been paid")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabinReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabinReservations_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CabinReservations_Cabins_CabinId",
                        column: x => x.CabinId,
                        principalTable: "Cabins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CabinReservations_Cruises_CruiseId",
                        column: x => x.CruiseId,
                        principalTable: "Cruises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents CabinReservation");

            migrationBuilder.CreateTable(
                name: "CruiseCabinPrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary key for the CabinCruisePrice.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CruiseId = table.Column<int>(type: "int", nullable: false, comment: "Cruise for which this price applies"),
                    CabinType = table.Column<int>(type: "int", nullable: false, comment: "Type of the Cabin"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Price for the cabin type on this cruise")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CruiseCabinPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary key for the Passenger.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, comment: "Gender of the Passenger"),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: false, comment: "Date of Birth of the Passenger"),
                    Nationality = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Nationality of the Passenger"),
                    PassportNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Passport Number of the Passenger"),
                    PassportExpirationDate = table.Column<DateOnly>(type: "date", nullable: false, comment: "Passport Exparation Date of the Passenger"),
                    PassportIssuingCountry = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Passport Issuing Country of the Passenger")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                },
                comment: "Represents CHECK-IN DATA");

            migrationBuilder.CreateTable(
                name: "ReservationPassengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Primary key for the ReservationPassenger.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CabinReservationId = table.Column<int>(type: "int", nullable: false, comment: "CabinReservationId"),
                    PassengerId = table.Column<int>(type: "int", nullable: false, comment: "Id of the Passenger"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "First Name of the Passenger"),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "Last Name of the Passenger"),
                    PassengerOrder = table.Column<int>(type: "int", nullable: false, comment: "Order of the assenger in the cabin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationPassengers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationPassengers_CabinReservations_CabinReservationId",
                        column: x => x.CabinReservationId,
                        principalTable: "CabinReservations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationPassengers_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents ReservationPassenger");

            migrationBuilder.CreateIndex(
                name: "IX_CabinReservations_CabinId",
                table: "CabinReservations",
                column: "CabinId");

            migrationBuilder.CreateIndex(
                name: "IX_CabinReservations_CruiseId_CabinId_UserId",
                table: "CabinReservations",
                columns: new[] { "CruiseId", "CabinId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CabinReservations_UserId",
                table: "CabinReservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CruiseCabinPrices_CruiseId_CabinType",
                table: "CruiseCabinPrices",
                columns: new[] { "CruiseId", "CabinType" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReservationPassengers_CabinReservationId_PassengerOrder",
                table: "ReservationPassengers",
                columns: new[] { "CabinReservationId", "PassengerOrder" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReservationPassengers_PassengerId",
                table: "ReservationPassengers",
                column: "PassengerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CruiseCabinPrices");

            migrationBuilder.DropTable(
                name: "ReservationPassengers");

            migrationBuilder.DropTable(
                name: "CabinReservations");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropColumn(
                name: "CabinType",
                table: "Cabins");
        }
    }
}
