using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddQRCodeToBIncentiveTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "40B_RaffleSchedule",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "30B_VIPUserEnt",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "20B_GiftCardUserEnt",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "12B_StampUserEnt",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "11B_PromoUserUsage",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "QRCode",
                table: "10B_CouponUserEnt",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "40B_RaffleSchedule");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "30B_VIPUserEnt");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "20B_GiftCardUserEnt");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "12B_StampUserEnt");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "11B_PromoUserUsage");

            migrationBuilder.DropColumn(
                name: "QRCode",
                table: "10B_CouponUserEnt");
        }
    }
}
