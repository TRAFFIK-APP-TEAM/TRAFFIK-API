using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TRAFFIK_API.Migrations
{
    /// <inheritdoc />
    public partial class AddInstagramSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Seed Instagram Posts
            migrationBuilder.InsertData(
                table: "InstagramPosts",
                columns: new[] { "Id", "Caption", "MediaUrl", "MediaType", "Timestamp" },
                values: new object[,]
                {
                    { "post_001", "Fresh wash and wax for this beautiful sedan! ✨ #CarWash #TRAFFIK", "https://example.com/images/sedan-wash.jpg", "image", new DateTime(2024, 10, 23, 10, 30, 0, DateTimeKind.Utc) },
                    { "post_002", "Premium detailing service completed! This SUV is looking brand new 🚗💨", "https://example.com/images/suv-detail.jpg", "image", new DateTime(2024, 10, 22, 14, 15, 0, DateTimeKind.Utc) },
                    { "post_003", "Before and after ceramic coating application. The shine is incredible! 🌟", "https://example.com/images/ceramic-coating.jpg", "image", new DateTime(2024, 10, 21, 16, 45, 0, DateTimeKind.Utc) },
                    { "post_004", "Engine bay cleaning service - spotless results every time! 🔧", "https://example.com/images/engine-clean.jpg", "image", new DateTime(2024, 10, 20, 11, 20, 0, DateTimeKind.Utc) },
                    { "post_005", "Leather treatment and interior detailing for this luxury car 🛋️", "https://example.com/images/leather-treatment.jpg", "image", new DateTime(2024, 10, 19, 13, 10, 0, DateTimeKind.Utc) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove Instagram Posts
            migrationBuilder.DeleteData(
                table: "InstagramPosts",
                keyColumn: "Id",
                keyValues: new object[] { "post_001", "post_002", "post_003", "post_004", "post_005" });
        }
    }
}
