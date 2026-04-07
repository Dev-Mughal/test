using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameCouponTable_B04_To_10A_BizDef : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_10B_CouponUserEnt_B04_Coupon_CouponID",
                table: "10B_CouponUserEnt");

            migrationBuilder.DropForeignKey(
                name: "FK_B04_Coupon_B01_Business_Profile_BusinessID",
                table: "B04_Coupon");

            migrationBuilder.DropForeignKey(
                name: "FK_B04_Coupon_B05_Coupon_Type_CouponTypeID",
                table: "B04_Coupon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_B04_Coupon",
                table: "B04_Coupon");

            migrationBuilder.DropIndex(
                name: "IX_Coupon_CouponTypeID",
                table: "B04_Coupon");

            migrationBuilder.DropColumn(
                name: "CouponTypeID",
                table: "B04_Coupon");

            migrationBuilder.RenameTable(
                name: "B04_Coupon",
                newName: "10A_Coupon_BizDef");

            migrationBuilder.AddPrimaryKey(
                name: "PK_10A_Coupon_BizDef",
                table: "10A_Coupon_BizDef",
                column: "CouponID");

            migrationBuilder.AddForeignKey(
                name: "FK_10A_Coupon_BizDef_B01_Business_Profile_BusinessID",
                table: "10A_Coupon_BizDef",
                column: "BusinessID",
                principalTable: "B01_Business_Profile",
                principalColumn: "BusinessID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_10B_CouponUserEnt_10A_Coupon_BizDef_CouponID",
                table: "10B_CouponUserEnt",
                column: "CouponID",
                principalTable: "10A_Coupon_BizDef",
                principalColumn: "CouponID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_10A_Coupon_BizDef_B01_Business_Profile_BusinessID",
                table: "10A_Coupon_BizDef");

            migrationBuilder.DropForeignKey(
                name: "FK_10B_CouponUserEnt_10A_Coupon_BizDef_CouponID",
                table: "10B_CouponUserEnt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_10A_Coupon_BizDef",
                table: "10A_Coupon_BizDef");

            migrationBuilder.RenameTable(
                name: "10A_Coupon_BizDef",
                newName: "B04_Coupon");

            migrationBuilder.AddColumn<int>(
                name: "CouponTypeID",
                table: "B04_Coupon",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_B04_Coupon",
                table: "B04_Coupon",
                column: "CouponID");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_CouponTypeID",
                table: "B04_Coupon",
                column: "CouponTypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_10B_CouponUserEnt_B04_Coupon_CouponID",
                table: "10B_CouponUserEnt",
                column: "CouponID",
                principalTable: "B04_Coupon",
                principalColumn: "CouponID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_B04_Coupon_B01_Business_Profile_BusinessID",
                table: "B04_Coupon",
                column: "BusinessID",
                principalTable: "B01_Business_Profile",
                principalColumn: "BusinessID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_B04_Coupon_B05_Coupon_Type_CouponTypeID",
                table: "B04_Coupon",
                column: "CouponTypeID",
                principalTable: "B05_Coupon_Type",
                principalColumn: "CouponTypeID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
