using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FirstProject_API.Migrations
{
    /// <inheritdoc />
    public partial class SeedVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "CreatedDate", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "", new DateTime(2023, 5, 10, 18, 19, 7, 266, DateTimeKind.Local).AddTicks(3905), "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.", "", "Royal Villa", 5, 200.0, 440, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "", new DateTime(2023, 5, 10, 18, 19, 7, 266, DateTimeKind.Local).AddTicks(3968), "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.", "", "Beach Villa", 10, 100.0, 240, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "", new DateTime(2023, 5, 10, 18, 19, 7, 266, DateTimeKind.Local).AddTicks(3971), "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.", "", "New Villa", 12, 200.0, 340, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "", new DateTime(2023, 5, 10, 18, 19, 7, 266, DateTimeKind.Local).AddTicks(3973), "Still, the word has been around ever since ancient Roman times to mean \"country house for the elite.\" In Italian, villa means \"country house or farm.\" Most villas include a large amount of land and often barns, garages, or other outbuildings as well.", "", "Old Villa", 2, 4.0, 140, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
