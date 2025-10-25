using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRAFFIK_API.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarModelServices_Vehicles_VehicleLicensePlate",
                table: "CarModelServices");

            migrationBuilder.DropForeignKey(
                name: "FK_CarTypeServices_Vehicles_VehicleLicensePlate",
                table: "CarTypeServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceHistories_Vehicles_VehicleLicensePlate",
                table: "ServiceHistories");

            migrationBuilder.DropIndex(
                name: "IX_ServiceHistories_VehicleLicensePlate",
                table: "ServiceHistories");

            migrationBuilder.DropIndex(
                name: "IX_CarTypeServices_VehicleLicensePlate",
                table: "CarTypeServices");

            migrationBuilder.DropIndex(
                name: "IX_CarModelServices_VehicleLicensePlate",
                table: "CarModelServices");

            migrationBuilder.DropColumn(
                name: "VehicleLicensePlate",
                table: "ServiceHistories");

            migrationBuilder.DropColumn(
                name: "VehicleLicensePlate",
                table: "CarTypeServices");

            migrationBuilder.DropColumn(
                name: "VehicleLicensePlate",
                table: "CarModelServices");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Vehicles",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Vehicles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Vehicles",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Vehicles",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Vehicles");

            migrationBuilder.AlterColumn<int>(
                name: "Year",
                table: "Vehicles",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Vehicles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Vehicles",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleLicensePlate",
                table: "ServiceHistories",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleLicensePlate",
                table: "CarTypeServices",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VehicleLicensePlate",
                table: "CarModelServices",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistories_VehicleLicensePlate",
                table: "ServiceHistories",
                column: "VehicleLicensePlate");

            migrationBuilder.CreateIndex(
                name: "IX_CarTypeServices_VehicleLicensePlate",
                table: "CarTypeServices",
                column: "VehicleLicensePlate");

            migrationBuilder.CreateIndex(
                name: "IX_CarModelServices_VehicleLicensePlate",
                table: "CarModelServices",
                column: "VehicleLicensePlate");

            migrationBuilder.AddForeignKey(
                name: "FK_CarModelServices_Vehicles_VehicleLicensePlate",
                table: "CarModelServices",
                column: "VehicleLicensePlate",
                principalTable: "Vehicles",
                principalColumn: "LicensePlate");

            migrationBuilder.AddForeignKey(
                name: "FK_CarTypeServices_Vehicles_VehicleLicensePlate",
                table: "CarTypeServices",
                column: "VehicleLicensePlate",
                principalTable: "Vehicles",
                principalColumn: "LicensePlate");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceHistories_Vehicles_VehicleLicensePlate",
                table: "ServiceHistories",
                column: "VehicleLicensePlate",
                principalTable: "Vehicles",
                principalColumn: "LicensePlate");
        }
    }
}
