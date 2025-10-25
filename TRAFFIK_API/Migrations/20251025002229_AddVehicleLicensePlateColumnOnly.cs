using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRAFFIK_API.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleLicensePlateColumnOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VehicleLicensePlate",
                table: "Bookings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VehicleLicensePlate",
                table: "Bookings");
        }
    }
}
