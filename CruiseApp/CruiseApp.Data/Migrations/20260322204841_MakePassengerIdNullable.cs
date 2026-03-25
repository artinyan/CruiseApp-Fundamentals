using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CruiseApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakePassengerIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationPassengers_Passengers_PassengerId",
                table: "ReservationPassengers");

            migrationBuilder.AlterColumn<int>(
                name: "PassengerId",
                table: "ReservationPassengers",
                type: "int",
                nullable: true,
                comment: "Id of the Passenger",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id of the Passenger");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationPassengers_Passengers_PassengerId",
                table: "ReservationPassengers",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationPassengers_Passengers_PassengerId",
                table: "ReservationPassengers");

            migrationBuilder.AlterColumn<int>(
                name: "PassengerId",
                table: "ReservationPassengers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Id of the Passenger",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldComment: "Id of the Passenger");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationPassengers_Passengers_PassengerId",
                table: "ReservationPassengers",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
