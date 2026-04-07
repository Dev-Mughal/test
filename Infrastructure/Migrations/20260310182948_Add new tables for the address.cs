using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Addnewtablesfortheaddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "L52_Geo_States",
                columns: new[] { "ID", "Code", "CreatedOn", "IsActive", "Name", "Region" },
                values: new object[,]
                {
                    { 1L, "AL", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Alabama", "South" },
                    { 2L, "AK", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Alaska", "West" },
                    { 3L, "AZ", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Arizona", "West" },
                    { 4L, "AR", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Arkansas", "South" },
                    { 5L, "CA", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "California", "West" },
                    { 6L, "CO", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Colorado", "West" },
                    { 7L, "CT", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Connecticut", "Northeast" },
                    { 8L, "DE", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Delaware", "South" },
                    { 9L, "FL", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Florida", "South" },
                    { 10L, "GA", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Georgia", "South" },
                    { 11L, "HI", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Hawaii", "West" },
                    { 12L, "ID", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Idaho", "West" },
                    { 13L, "IL", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Illinois", "Midwest" },
                    { 14L, "IN", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Indiana", "Midwest" },
                    { 15L, "IA", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Iowa", "Midwest" },
                    { 16L, "KS", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Kansas", "Midwest" },
                    { 17L, "KY", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Kentucky", "South" },
                    { 18L, "LA", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Louisiana", "South" },
                    { 19L, "ME", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Maine", "Northeast" },
                    { 20L, "MD", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Maryland", "South" },
                    { 21L, "MA", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Massachusetts", "Northeast" },
                    { 22L, "MI", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Michigan", "Midwest" },
                    { 23L, "MN", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Minnesota", "Midwest" },
                    { 24L, "MS", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Mississippi", "South" },
                    { 25L, "MO", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Missouri", "Midwest" },
                    { 26L, "MT", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Montana", "West" },
                    { 27L, "NE", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Nebraska", "Midwest" },
                    { 28L, "NV", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Nevada", "West" },
                    { 29L, "NH", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "New Hampshire", "Northeast" },
                    { 30L, "NJ", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "New Jersey", "Northeast" },
                    { 31L, "NM", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "New Mexico", "West" },
                    { 32L, "NY", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "New York", "Northeast" },
                    { 33L, "NC", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "North Carolina", "South" },
                    { 34L, "ND", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "North Dakota", "Midwest" },
                    { 35L, "OH", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Ohio", "Midwest" },
                    { 36L, "OK", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Oklahoma", "South" },
                    { 37L, "OR", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Oregon", "West" },
                    { 38L, "PA", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Pennsylvania", "Northeast" },
                    { 39L, "RI", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Rhode Island", "Northeast" },
                    { 40L, "SC", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "South Carolina", "South" },
                    { 41L, "SD", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "South Dakota", "Midwest" },
                    { 42L, "TN", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Tennessee", "South" },
                    { 43L, "TX", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Texas", "South" },
                    { 44L, "UT", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Utah", "West" },
                    { 45L, "VT", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Vermont", "Northeast" },
                    { 46L, "VA", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Virginia", "South" },
                    { 47L, "WA", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Washington", "West" },
                    { 48L, "WV", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "West Virginia", "South" },
                    { 49L, "WI", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Wisconsin", "Midwest" },
                    { 50L, "WY", new DateTime(2026, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), true, "Wyoming", "West" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 6L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 7L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 8L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 9L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 10L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 11L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 12L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 13L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 14L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 15L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 16L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 17L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 18L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 19L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 20L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 21L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 22L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 23L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 24L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 25L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 26L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 27L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 28L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 29L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 30L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 31L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 32L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 33L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 34L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 35L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 36L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 37L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 38L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 39L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 40L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 41L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 42L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 43L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 44L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 45L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 46L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 47L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 48L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 49L);

            migrationBuilder.DeleteData(
                table: "L52_Geo_States",
                keyColumn: "ID",
                keyValue: 50L);
        }
    }
}
