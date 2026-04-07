using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeCustomerCouponIndexUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CouponUserEnt_UserID_CouponID",
                table: "10B_CouponUserEnt");

            migrationBuilder.CreateIndex(
                name: "UX_CouponUserEnt_UserID_CouponID",
                table: "10B_CouponUserEnt",
                columns: new[] { "UserID", "CouponID" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UX_CouponUserEnt_UserID_CouponID",
                table: "10B_CouponUserEnt");

            migrationBuilder.CreateIndex(
                name: "IX_CouponUserEnt_UserID_CouponID",
                table: "10B_CouponUserEnt",
                columns: new[] { "UserID", "CouponID" });
        }
    }
}
