using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorEcommerce.Server.Migrations
{
    /// <inheritdoc />
    public partial class ProductSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "This is the description for product 1. Make the description interesting with at least 1000 characters. and make sure that the description is long enough to wrap to the next line.", "https://via.placeholder.com/250", 9.99m, "Product 1" },
                    { 2, "This is the description for product 2. Make the description interesting with at least 1000 characters. and make sure that the description is long enough to wrap to the next line.", "https://via.placeholder.com/250", 19.99m, "Product 2" },
                    { 3, "This is the description for product 3. Make the description interesting with at least 1000 characters. and make sure that the description is long enough to wrap to the next line.", "https://via.placeholder.com/250", 29.99m, "Product 3" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
