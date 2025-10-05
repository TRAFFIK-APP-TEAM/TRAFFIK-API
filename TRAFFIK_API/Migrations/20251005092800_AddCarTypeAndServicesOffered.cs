using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TRAFFIK_API.Migrations
{
    /// <inheritdoc />
    public partial class AddCarTypeAndServicesOffered : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarTypeId",
                table: "ServiceCatalogs",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CarTypeId",
                table: "CarModels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "BookingStages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UpdatedByUserId",
                table: "BookingStages",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ServiceCatalogId",
                table: "Bookings",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CarModelServices",
                columns: table => new
                {
                    CarModelId = table.Column<int>(type: "integer", nullable: false),
                    ServiceCatalogId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarModelServices", x => new { x.CarModelId, x.ServiceCatalogId });
                    table.ForeignKey(
                        name: "FK_CarModelServices_CarModels_CarModelId",
                        column: x => x.CarModelId,
                        principalTable: "CarModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarModelServices_ServiceCatalogs_ServiceCatalogId",
                        column: x => x.ServiceCatalogId,
                        principalTable: "ServiceCatalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ServiceHistories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CarModelId = table.Column<int>(type: "integer", nullable: false),
                    ServiceCatalogId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceHistories_CarModels_CarModelId",
                        column: x => x.CarModelId,
                        principalTable: "CarModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceHistories_ServiceCatalogs_ServiceCatalogId",
                        column: x => x.ServiceCatalogId,
                        principalTable: "ServiceCatalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServiceHistories_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarTypeServices",
                columns: table => new
                {
                    CarTypeId = table.Column<int>(type: "integer", nullable: false),
                    ServiceCatalogId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarTypeServices", x => new { x.CarTypeId, x.ServiceCatalogId });
                    table.ForeignKey(
                        name: "FK_CarTypeServices_CarTypes_CarTypeId",
                        column: x => x.CarTypeId,
                        principalTable: "CarTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarTypeServices_ServiceCatalogs_ServiceCatalogId",
                        column: x => x.ServiceCatalogId,
                        principalTable: "ServiceCatalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceCatalogs_CarTypeId",
                table: "ServiceCatalogs",
                column: "CarTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Rewards_UserId",
                table: "Rewards",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookingId",
                table: "Reviews",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_BookingId",
                table: "Payments",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_BookingId",
                table: "Notifications",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_CarTypeId",
                table: "CarModels",
                column: "CarTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarModels_UserId",
                table: "CarModels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingStages_BookingId",
                table: "BookingStages",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_BookingStages_UpdatedByUserId",
                table: "BookingStages",
                column: "UpdatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_CarModelId",
                table: "Bookings",
                column: "CarModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_ServiceCatalogId",
                table: "Bookings",
                column: "ServiceCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CarModelServices_ServiceCatalogId",
                table: "CarModelServices",
                column: "ServiceCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_CarTypeServices_ServiceCatalogId",
                table: "CarTypeServices",
                column: "ServiceCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistories_CarModelId",
                table: "ServiceHistories",
                column: "CarModelId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistories_ServiceCatalogId",
                table: "ServiceHistories",
                column: "ServiceCatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceHistories_UserId",
                table: "ServiceHistories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_CarModels_CarModelId",
                table: "Bookings",
                column: "CarModelId",
                principalTable: "CarModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_ServiceCatalogs_ServiceCatalogId",
                table: "Bookings",
                column: "ServiceCatalogId",
                principalTable: "ServiceCatalogs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingStages_Bookings_BookingId",
                table: "BookingStages",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookingStages_Users_UpdatedByUserId",
                table: "BookingStages",
                column: "UpdatedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_CarTypes_CarTypeId",
                table: "CarModels",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarModels_Users_UserId",
                table: "CarModels",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Bookings_BookingId",
                table: "Notifications",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Bookings_BookingId",
                table: "Payments",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Bookings_BookingId",
                table: "Reviews",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rewards_Users_UserId",
                table: "Rewards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceCatalogs_CarTypes_CarTypeId",
                table: "ServiceCatalogs",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_CarModels_CarModelId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_ServiceCatalogs_ServiceCatalogId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Users_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingStages_Bookings_BookingId",
                table: "BookingStages");

            migrationBuilder.DropForeignKey(
                name: "FK_BookingStages_Users_UpdatedByUserId",
                table: "BookingStages");

            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_CarTypes_CarTypeId",
                table: "CarModels");

            migrationBuilder.DropForeignKey(
                name: "FK_CarModels_Users_UserId",
                table: "CarModels");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Bookings_BookingId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Users_UserId",
                table: "Notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Bookings_BookingId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Bookings_BookingId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_UserId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Rewards_Users_UserId",
                table: "Rewards");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceCatalogs_CarTypes_CarTypeId",
                table: "ServiceCatalogs");

            migrationBuilder.DropTable(
                name: "CarModelServices");

            migrationBuilder.DropTable(
                name: "CarTypeServices");

            migrationBuilder.DropTable(
                name: "ServiceHistories");

            migrationBuilder.DropTable(
                name: "CarTypes");

            migrationBuilder.DropIndex(
                name: "IX_ServiceCatalogs_CarTypeId",
                table: "ServiceCatalogs");

            migrationBuilder.DropIndex(
                name: "IX_Rewards_UserId",
                table: "Rewards");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_BookingId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Payments_BookingId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_BookingId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_CarTypeId",
                table: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_CarModels_UserId",
                table: "CarModels");

            migrationBuilder.DropIndex(
                name: "IX_BookingStages_BookingId",
                table: "BookingStages");

            migrationBuilder.DropIndex(
                name: "IX_BookingStages_UpdatedByUserId",
                table: "BookingStages");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_CarModelId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_ServiceCatalogId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "CarTypeId",
                table: "ServiceCatalogs");

            migrationBuilder.DropColumn(
                name: "CarTypeId",
                table: "CarModels");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "BookingStages");

            migrationBuilder.DropColumn(
                name: "UpdatedByUserId",
                table: "BookingStages");

            migrationBuilder.DropColumn(
                name: "ServiceCatalogId",
                table: "Bookings");
        }
    }
}
