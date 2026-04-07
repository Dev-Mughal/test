using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RefactorBusinessUserManyToManyLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "B03_BusinessUserLink",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessID = table.Column<int>(type: "int4", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B03_BusinessUserLink", x => x.ID);
                    table.ForeignKey(
                        name: "FK_B03_BusinessUserLink_B01_Business_Profile_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_B03_BusinessUserLink_B02_Business_User_UserID",
                        column: x => x.UserID,
                        principalTable: "B02_Business_User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_B03_BusinessUserLink_UserID_IsDefault",
                table: "B03_BusinessUserLink",
                columns: new[] { "UserID", "IsDefault" });

            migrationBuilder.CreateIndex(
                name: "UX_B03_BusinessUserLink_BusinessID_UserID",
                table: "B03_BusinessUserLink",
                columns: new[] { "BusinessID", "UserID" },
                unique: true);

            migrationBuilder.Sql(
                """
                INSERT INTO "B03_BusinessUserLink" ("BusinessID", "UserID", "IsDefault")
                SELECT bu."BusinessID", bu."UserID", TRUE
                FROM "B02_Business_User" bu;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_B02_Business_User_B01_Business_Profile_BusinessID",
                table: "B02_Business_User");

            migrationBuilder.DropIndex(
                name: "IX_BusinessUser_BusinessID",
                table: "B02_Business_User");

            migrationBuilder.DropColumn(
                name: "BusinessID",
                table: "B02_Business_User");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "B03_BusinessUserLink");

            migrationBuilder.AddColumn<int>(
                name: "BusinessID",
                table: "B02_Business_User",
                type: "int4",
                nullable: true);

            migrationBuilder.Sql(
                """
                UPDATE bu
                SET "BusinessID" = src."BusinessID"
                FROM "B02_Business_User" bu
                JOIN (
                    SELECT DISTINCT ON (link."UserID")
                        link."UserID",
                        link."BusinessID"
                    FROM "B03_BusinessUserLink" link
                    ORDER BY link."UserID", (link."IsDefault" = TRUE) DESC, link."ID"
                ) src ON src."UserID" = bu."UserID";
                """);

            migrationBuilder.Sql(
                """
                UPDATE "B02_Business_User"
                SET "BusinessID" = (
                    SELECT "BusinessID"
                    FROM "B01_Business_Profile"
                    ORDER BY "BusinessID"
                    LIMIT 1
                )
                WHERE "BusinessID" IS NULL;
                """);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "B02_Business_User",
                type: "int4",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUser_BusinessID",
                table: "B02_Business_User",
                column: "BusinessID");

            migrationBuilder.AddForeignKey(
                name: "FK_B02_Business_User_B01_Business_Profile_BusinessID",
                table: "B02_Business_User",
                column: "BusinessID",
                principalTable: "B01_Business_Profile",
                principalColumn: "BusinessID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
