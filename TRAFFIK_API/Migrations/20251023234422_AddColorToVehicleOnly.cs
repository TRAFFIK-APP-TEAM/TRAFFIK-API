using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRAFFIK_API.Migrations
{
    /// <inheritdoc />
    public partial class AddColorToVehicleOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarTypes_CarTypeId",
                table: "CarModels");

            migrationBuilder.DropForeignKey(
                name: "FK_CarTypeServices_CarTypes_CarTypeId",
                table: "CarTypeServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCatalogs_CarTypes_CarTypeId",
                table: "ServiceCatalogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarTypes",
                table: "CarTypes");

            migrationBuilder.RenameTable(
                name: "CarTypes",
                newName: "VehicleTypes");

            migrationBuilder.AddColumn<string>(
                name: "VehicleLicensePlate",
                table: "Bookings",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleTypes",
                table: "VehicleTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_VehicleTypes_CarTypeId",
                table: "CarModels",
                column: "CarTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarTypeServices_VehicleTypes_CarTypeId",
                table: "CarTypeServices",
                column: "CarTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCatalogs_VehicleTypes_CarTypeId",
                table: "ServiceCatalogs",
                column: "CarTypeId",
                principalTable: "VehicleTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_VehicleTypes_CarTypeId",
                table: "CarModels");

            migrationBuilder.DropForeignKey(
                name: "FK_CarTypeServices_VehicleTypes_CarTypeId",
                table: "CarTypeServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCatalogs_VehicleTypes_CarTypeId",
                table: "ServiceCatalogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleTypes",
                table: "VehicleTypes");

            migrationBuilder.DropColumn(
                name: "VehicleLicensePlate",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "VehicleTypes",
                newName: "CarTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarTypes",
                table: "CarTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarTypes_CarTypeId",
                table: "CarModels",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarTypeServices_CarTypes_CarTypeId",
                table: "CarTypeServices",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCatalogs_CarTypes_CarTypeId",
                table: "ServiceCatalogs",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id");
        }
    }
}
