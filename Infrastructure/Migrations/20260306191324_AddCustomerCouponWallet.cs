using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerCouponWallet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "10B_CouponUserEnt",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    CouponID = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateRedeemed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CashierNote = table.Column<string>(type: "text", nullable: true),
                    StatusAdminNote = table.Column<string>(type: "text", nullable: true),
                    StatusUserNote = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_10B_CouponUserEnt", x => x.ID);
                    table.ForeignKey(
                        name: "FK_10B_CouponUserEnt_B04_Coupon_CouponID",
                        column: x => x.CouponID,
                        principalTable: "B04_Coupon",
                        principalColumn: "CouponID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_10B_CouponUserEnt_C01_Customer_UserID",
                        column: x => x.UserID,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_10B_CouponUserEnt_CouponID",
                table: "10B_CouponUserEnt",
                column: "CouponID");

            migrationBuilder.CreateIndex(
                name: "IX_CouponUserEnt_Status",
                table: "10B_CouponUserEnt",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_CouponUserEnt_UserID_CouponID",
                table: "10B_CouponUserEnt",
                columns: new[] { "UserID", "CouponID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "10B_CouponUserEnt");
        }
    }
}
