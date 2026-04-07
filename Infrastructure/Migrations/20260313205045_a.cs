using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class a : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Coupon_TrackCode",
                table: "10A_Coupon_BizDef");

            migrationBuilder.DropColumn(
                name: "TrackCode",
                table: "40A_RaffleDef");

            migrationBuilder.DropColumn(
                name: "TrackCode",
                table: "30A_VIPBizDef");

            migrationBuilder.DropColumn(
                name: "TrackCode",
                table: "20A_GiftCardBizDef");

            migrationBuilder.DropColumn(
                name: "TrackCode",
                table: "12A_StampBizDef");

            migrationBuilder.DropColumn(
                name: "TrackCode",
                table: "11A_PromoBizDef");

            migrationBuilder.DropColumn(
                name: "TrackCode",
                table: "10A_Coupon_BizDef");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrackCode",
                table: "40A_RaffleDef",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrackCode",
                table: "30A_VIPBizDef",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrackCode",
                table: "20A_GiftCardBizDef",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrackCode",
                table: "12A_StampBizDef",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrackCode",
                table: "11A_PromoBizDef",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrackCode",
                table: "10A_Coupon_BizDef",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_TrackCode",
                table: "10A_Coupon_BizDef",
                column: "TrackCode",
                unique: true);
        }
    }
}
