using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removingTrckCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Business_BusinessEmail",
                table: "B01_Business_Profile");

            migrationBuilder.CreateIndex(
                name: "IX_Business_BusinessEmail",
                table: "B01_Business_Profile",
                column: "BusinessEmail");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Business_BusinessEmail",
                table: "B01_Business_Profile");

            migrationBuilder.CreateIndex(
                name: "IX_Business_BusinessEmail",
                table: "B01_Business_Profile",
                column: "BusinessEmail",
                unique: true);
        }
    }
}
