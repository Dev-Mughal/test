using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class intialdatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Coupon_BusinessID",
                table: "B04_Coupon");

            migrationBuilder.DropIndex(
                name: "IX_Coupon_IsActive",
                table: "B04_Coupon");

            migrationBuilder.DropIndex(
                name: "IX_Coupon_IsFeatured",
                table: "B04_Coupon");

            migrationBuilder.DropIndex(
                name: "IX_BusinessCategory_DisplayColumn",
                table: "B04_Business_Category");

            migrationBuilder.DropIndex(
                name: "IX_BusinessCategory_DisplayOrder",
                table: "B04_Business_Category");

            migrationBuilder.DropIndex(
                name: "IX_Business_CategoryID",
                table: "B01_Business_Profile");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Business_User_Tokens",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "Business_User_Roles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Business_User_Roles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Business_User_Logins",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Business_User_Claims",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "RoleId",
                table: "Business_Roles_Claims",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "Id",
                table: "Business_Roles",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "TypeDescription",
                table: "B05_Coupon_Type",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TypeCode",
                table: "B05_Coupon_Type",
                type: "character(1)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<string>(
                name: "TrackCode",
                table: "B04_Coupon",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "B04_Coupon",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "QRCode",
                table: "B04_Coupon",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "B04_Coupon",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "B04_Coupon",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "CouponID",
                table: "B04_Coupon",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<short>(
                name: "DisplayOrder",
                table: "B04_Business_Category",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<short>(
                name: "DisplayColumn",
                table: "B04_Business_Category",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "B04_Business_Category",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "TimeZone",
                table: "B02_Business_User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "B02_Business_User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "B02_Business_User",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "B02_Business_User",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<long>(
                name: "UserID",
                table: "B02_Business_User",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.Sql(
                @"ALTER TABLE ""B01_Business_Profile"" ALTER COLUMN ""Longitude"" TYPE double precision USING ""Longitude""::double precision;");

            migrationBuilder.Sql(
                @"ALTER TABLE ""B01_Business_Profile"" ALTER COLUMN ""Latitude"" TYPE double precision USING ""Latitude""::double precision;");

            migrationBuilder.AlterColumn<short>(
                name: "CountryCode",
                table: "B01_Business_Profile",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "BusinessName",
                table: "B01_Business_Profile",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "BusinessImageUrl",
                table: "B01_Business_Profile",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BusinessEmail",
                table: "B01_Business_Profile",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<long>(
                name: "BusinessID",
                table: "B01_Business_Profile",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 1,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)1, (short)1 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 2,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)1, (short)2 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 3,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)1, (short)3 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 4,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)1, (short)4 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 5,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)1, (short)5 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 6,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)1, (short)6 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 7,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)1, (short)7 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 8,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)2, (short)1 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 9,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)2, (short)2 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 10,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)2, (short)3 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 11,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)2, (short)4 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 12,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)2, (short)5 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 13,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)2, (short)6 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 14,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)2, (short)7 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 15,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { (short)2, (short)8 });

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_BusinessID_CreatedOn",
                table: "B04_Coupon",
                columns: new[] { "BusinessID", "CreatedOn" },
                descending: new[] { false, true });

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_DateRange",
                table: "B04_Coupon",
                columns: new[] { "StartDateTime", "EndDateTime" });

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_IsActive_IsFeatured",
                table: "B04_Coupon",
                columns: new[] { "IsActive", "IsFeatured" });

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCategory_IsActive_DisplayColumn_DisplayOrder",
                table: "B04_Business_Category",
                columns: new[] { "IsActive", "DisplayColumn", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Business_CategoryID_CreatedOn",
                table: "B01_Business_Profile",
                columns: new[] { "CategoryID", "CreatedOn" },
                descending: new[] { false, true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Coupon_BusinessID_CreatedOn",
                table: "B04_Coupon");

            migrationBuilder.DropIndex(
                name: "IX_Coupon_DateRange",
                table: "B04_Coupon");

            migrationBuilder.DropIndex(
                name: "IX_Coupon_IsActive_IsFeatured",
                table: "B04_Coupon");

            migrationBuilder.DropIndex(
                name: "IX_BusinessCategory_IsActive_DisplayColumn_DisplayOrder",
                table: "B04_Business_Category");

            migrationBuilder.DropIndex(
                name: "IX_Business_CategoryID_CreatedOn",
                table: "B01_Business_Profile");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Business_User_Tokens",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Business_User_Roles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Business_User_Roles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Business_User_Logins",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Business_User_Claims",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Business_Roles_Claims",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Business_Roles",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<string>(
                name: "TypeDescription",
                table: "B05_Coupon_Type",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "TypeCode",
                table: "B05_Coupon_Type",
                type: "character varying(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(1)");

            migrationBuilder.AlterColumn<string>(
                name: "TrackCode",
                table: "B04_Coupon",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "B04_Coupon",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "QRCode",
                table: "B04_Coupon",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Picture",
                table: "B04_Coupon",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "B04_Coupon",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "CouponID",
                table: "B04_Coupon",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "DisplayOrder",
                table: "B04_Business_Category",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<int>(
                name: "DisplayColumn",
                table: "B04_Business_Category",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "CategoryName",
                table: "B04_Business_Category",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "TimeZone",
                table: "B02_Business_User",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "B02_Business_User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "B02_Business_User",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "B02_Business_User",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "B02_Business_User",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.Sql(
                @"ALTER TABLE ""B01_Business_Profile"" ALTER COLUMN ""Longitude"" TYPE text USING ""Longitude""::text;");

            migrationBuilder.Sql(
                @"ALTER TABLE ""B01_Business_Profile"" ALTER COLUMN ""Latitude"" TYPE text USING ""Latitude""::text;");

            migrationBuilder.AlterColumn<int>(
                name: "CountryCode",
                table: "B01_Business_Profile",
                type: "integer",
                nullable: false,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<string>(
                name: "BusinessName",
                table: "B01_Business_Profile",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "BusinessImageUrl",
                table: "B01_Business_Profile",
                type: "character varying(300)",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BusinessEmail",
                table: "B01_Business_Profile",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "B01_Business_Profile",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 1,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 2,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 1, 2 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 3,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 1, 3 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 4,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 1, 4 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 5,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 1, 5 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 6,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 1, 6 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 7,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 1, 7 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 8,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 2, 1 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 9,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 2, 2 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 10,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 2, 3 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 11,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 2, 4 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 12,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 2, 5 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 13,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 2, 6 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 14,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 2, 7 });

            migrationBuilder.UpdateData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 15,
                columns: new[] { "DisplayColumn", "DisplayOrder" },
                values: new object[] { 2, 8 });

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_BusinessID",
                table: "B04_Coupon",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_IsActive",
                table: "B04_Coupon",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_IsFeatured",
                table: "B04_Coupon",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCategory_DisplayColumn",
                table: "B04_Business_Category",
                column: "DisplayColumn");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCategory_DisplayOrder",
                table: "B04_Business_Category",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Business_CategoryID",
                table: "B01_Business_Profile",
                column: "CategoryID");
        }
    }
}
