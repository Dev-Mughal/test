using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGeoLocationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Clear all existing business rows (and their dependents via CASCADE) so the
            // new NOT NULL FK columns State_City_ID / State_City_Zip_ID can be added safely.
            // This is safe in development; production deployments must migrate data first.
            migrationBuilder.Sql(@"TRUNCATE TABLE ""B01_Business_Profile"" CASCADE;");

            migrationBuilder.DropColumn(
                name: "City",
                table: "B01_Business_Profile");

            migrationBuilder.DropColumn(
                name: "State",
                table: "B01_Business_Profile");

            migrationBuilder.DropColumn(
                name: "ZipCode",
                table: "B01_Business_Profile");

            migrationBuilder.AddColumn<long>(
                name: "State_City_ID",
                table: "B01_Business_Profile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "State_City_Zip_ID",
                table: "B01_Business_Profile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "L50_Geo_Cities",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    UserInput = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_L50_Geo_Cities", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "L51_Geo_ZipCodes",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    UserInput = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_L51_Geo_ZipCodes", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Business_StateCityID",
                table: "B01_Business_Profile",
                column: "State_City_ID");

            migrationBuilder.CreateIndex(
                name: "IX_Business_StateCityZipID",
                table: "B01_Business_Profile",
                column: "State_City_Zip_ID");

            migrationBuilder.CreateIndex(
                name: "IX_L50_City_State",
                table: "L50_Geo_Cities",
                columns: new[] { "City", "State" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_L51_City_State_ZipCode",
                table: "L51_Geo_ZipCodes",
                columns: new[] { "City", "State", "ZipCode" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_B01_Business_Profile_L50_Geo_Cities_State_City_ID",
                table: "B01_Business_Profile",
                column: "State_City_ID",
                principalTable: "L50_Geo_Cities",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_B01_Business_Profile_L51_Geo_ZipCodes_State_City_Zip_ID",
                table: "B01_Business_Profile",
                column: "State_City_Zip_ID",
                principalTable: "L51_Geo_ZipCodes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_B01_Business_Profile_L50_Geo_Cities_State_City_ID",
                table: "B01_Business_Profile");

            migrationBuilder.DropForeignKey(
                name: "FK_B01_Business_Profile_L51_Geo_ZipCodes_State_City_Zip_ID",
                table: "B01_Business_Profile");

            migrationBuilder.DropTable(
                name: "L50_Geo_Cities");

            migrationBuilder.DropTable(
                name: "L51_Geo_ZipCodes");

            migrationBuilder.DropIndex(
                name: "IX_Business_StateCityID",
                table: "B01_Business_Profile");

            migrationBuilder.DropIndex(
                name: "IX_Business_StateCityZipID",
                table: "B01_Business_Profile");

            migrationBuilder.DropColumn(
                name: "State_City_ID",
                table: "B01_Business_Profile");

            migrationBuilder.DropColumn(
                name: "State_City_Zip_ID",
                table: "B01_Business_Profile");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "B01_Business_Profile",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "B01_Business_Profile",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ZipCode",
                table: "B01_Business_Profile",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
