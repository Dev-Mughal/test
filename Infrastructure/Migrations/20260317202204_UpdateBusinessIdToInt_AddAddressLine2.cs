using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBusinessIdToInt_AddAddressLine2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "B02_Business_User",
                type: "int4",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "B01_Business_Profile",
                type: "int4",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine2",
                table: "B01_Business_Profile",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "40A_RaffleDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "30B_VIPUserEnt",
                type: "int4",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "30A_VIPBizDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "20A_GiftCardBizDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "12A_StampBizDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "11A_PromoBizDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "10A_Coupon_BizDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine2",
                table: "B01_Business_Profile");

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "B02_Business_User",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "B01_Business_Profile",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "40A_RaffleDef",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "30B_VIPUserEnt",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "30A_VIPBizDef",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "20A_GiftCardBizDef",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "12A_StampBizDef",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "11A_PromoBizDef",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "10A_Coupon_BizDef",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");
        }
    }
}
