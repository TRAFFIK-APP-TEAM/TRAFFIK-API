using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRAFFIK_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed UserRoles
            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleName" },
                values: new object[,]
                {
                    { "Admin" },
                    { "Customer" },
                    { "Employee" },
                    { "Manager" }
                });

            // Seed CarTypes
            migrationBuilder.InsertData(
                table: "CarTypes",
                columns: new[] { "Type" },
                values: new object[,]
                {
                    { "Sedan" },
                    { "SUV" },
                    { "Hatchback" },
                    { "Coupe" },
                    { "Convertible" },
                    { "Wagon" },
                    { "Truck" },
                    { "Van" },
                    { "Minivan" },
                    { "Crossover" }
                });

            // Seed ServiceCatalogs
            migrationBuilder.InsertData(
                table: "ServiceCatalogs",
                columns: new[] { "Name", "Description", "Price" },
                values: new object[,]
                {
                    { "Basic Wash", "Exterior wash with soap and water", 25.00m },
                    { "Premium Wash", "Exterior wash with wax and tire shine", 45.00m },
                    { "Full Detail", "Complete interior and exterior detailing", 120.00m },
                    { "Interior Detail", "Deep cleaning of interior surfaces", 75.00m },
                    { "Engine Cleaning", "Engine bay cleaning and degreasing", 50.00m },
                    { "Paint Correction", "Professional paint polishing and correction", 200.00m },
                    { "Ceramic Coating", "Long-lasting paint protection coating", 300.00m },
                    { "Headlight Restoration", "Restore cloudy or yellowed headlights", 80.00m },
                    { "Leather Treatment", "Clean and condition leather seats", 60.00m },
                    { "Carpet Shampoo", "Deep clean and sanitize carpets", 40.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove seed data in reverse order
            migrationBuilder.DeleteData(
                table: "ServiceCatalogs",
                keyColumn: "Name",
                keyValues: new object[]
                {
                    "Basic Wash",
                    "Premium Wash", 
                    "Full Detail",
                    "Interior Detail",
                    "Engine Cleaning",
                    "Paint Correction",
                    "Ceramic Coating",
                    "Headlight Restoration",
                    "Leather Treatment",
                    "Carpet Shampoo"
                });

            migrationBuilder.DeleteData(
                table: "CarTypes",
                keyColumn: "Type",
                keyValues: new object[]
                {
                    "Sedan",
                    "SUV",
                    "Hatchback",
                    "Coupe",
                    "Convertible",
                    "Wagon",
                    "Truck",
                    "Van",
                    "Minivan",
                    "Crossover"
                });

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumn: "RoleName",
                keyValues: new object[]
                {
                    "Admin",
                    "Customer",
                    "Employee",
                    "Manager"
                });
        }
    }
}
