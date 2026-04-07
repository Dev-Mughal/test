using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStateTable_L52_Geo_States : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "L52_Geo_States",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "text", maxLength: 2, nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_L52_Geo_States", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_L52_Code",
                table: "L52_Geo_States",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_L52_IsActive",
                table: "L52_Geo_States",
                column: "IsActive");

            // Seed data is now managed by StateConfiguration.HasData() in the fluent API
            // This keeps seeding logic centralized in the entity configuration files
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "L52_Geo_States");
        }
    }
}
