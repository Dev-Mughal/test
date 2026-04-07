using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate_v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "B04_Business_Category",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CategorySlug = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    DisplayColumn = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B04_Business_Category", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "B05_Coupon_Type",
                columns: table => new
                {
                    CouponTypeID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TypeDescription = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TypeCode = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B05_Coupon_Type", x => x.CouponTypeID);
                });

            migrationBuilder.CreateTable(
                name: "Business_Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "B01_Business_Profile",
                columns: table => new
                {
                    BusinessID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    BusinessURL = table.Column<string>(type: "text", nullable: false),
                    BusinessEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BusinessPhone = table.Column<string>(type: "text", nullable: false),
                    CountryCode = table.Column<int>(type: "integer", nullable: false),
                    StreetAddress = table.Column<string>(type: "text", nullable: false),
                    City = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<string>(type: "text", nullable: false),
                    ZipCode = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Longitude = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<string>(type: "text", nullable: false),
                    BusinessImageUrl = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CategoryID = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B01_Business_Profile", x => x.BusinessID);
                    table.ForeignKey(
                        name: "FK_B01_Business_Profile_B04_Business_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "B04_Business_Category",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Business_Roles_Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_Roles_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Business_Roles_Claims_Business_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Business_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "B02_Business_User",
                columns: table => new
                {
                    UserID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeZone = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RefreshToken = table.Column<string>(type: "text", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    BusinessID = table.Column<int>(type: "integer", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B02_Business_User", x => x.UserID);
                    table.ForeignKey(
                        name: "FK_B02_Business_User_B01_Business_Profile_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "B04_Coupon",
                columns: table => new
                {
                    CouponID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessID = table.Column<int>(type: "integer", nullable: false),
                    CouponTypeID = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Picture = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    QRCode = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TrackCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    StartDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B04_Coupon", x => x.CouponID);
                    table.ForeignKey(
                        name: "FK_B04_Coupon_B01_Business_Profile_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_B04_Coupon_B05_Coupon_Type_CouponTypeID",
                        column: x => x.CouponTypeID,
                        principalTable: "B05_Coupon_Type",
                        principalColumn: "CouponTypeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Business_User_Claims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_User_Claims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Business_User_Claims_B02_Business_User_UserId",
                        column: x => x.UserId,
                        principalTable: "B02_Business_User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Business_User_Logins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_User_Logins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_Business_User_Logins_B02_Business_User_UserId",
                        column: x => x.UserId,
                        principalTable: "B02_Business_User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Business_User_Roles",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_User_Roles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Business_User_Roles_B02_Business_User_UserId",
                        column: x => x.UserId,
                        principalTable: "B02_Business_User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Business_User_Roles_Business_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Business_Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Business_User_Tokens",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Business_User_Tokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_Business_User_Tokens_B02_Business_User_UserId",
                        column: x => x.UserId,
                        principalTable: "B02_Business_User",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "B04_Business_Category",
                columns: new[] { "CategoryID", "CategoryName", "CategorySlug", "CreatedOn", "DisplayColumn", "DisplayOrder", "IsActive" },
                values: new object[,]
                {
                    { 1, "Restaurant & Bar", "restaurant-bar", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1, true },
                    { 2, "Desert and Drinks", "desert-and-drinks", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, 2, true },
                    { 3, "Entertainment", "entertainment", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, 3, true },
                    { 4, "Beauty and Health", "beauty-and-health", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, 4, true },
                    { 5, "Hotel & Travel", "hotel-travel", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, 5, true },
                    { 6, "For Kids", "for-kids", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, 6, true },
                    { 7, "For Pets", "for-pets", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 1, 7, true },
                    { 8, "Automotive Services", "automotive-services", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, 1, true },
                    { 9, "Cleaning", "cleaning", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, 2, true },
                    { 10, "Home Services", "home-services", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, 3, true },
                    { 11, "Gym & Fitness", "gym-fitness", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, 4, true },
                    { 12, "Real Estate", "real-estate", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, 5, true },
                    { 13, "Legal", "legal", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, 6, true },
                    { 14, "Medical Services", "medical-services", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, 7, true },
                    { 15, "Professional Services (other)", "professional-services-other", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), 2, 8, true }
                });

            migrationBuilder.InsertData(
                table: "B05_Coupon_Type",
                columns: new[] { "CouponTypeID", "CreatedOn", "IsActive", "TypeCode", "TypeDescription" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), true, "B", "BizyPop Dollars" },
                    { 2, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), true, "C", "Coupon" },
                    { 3, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), true, "S", "Stamp" },
                    { 4, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), true, "M", "Promotions" },
                    { 5, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), true, "G", "Gift Card" },
                    { 6, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), true, "R", "Store Credit" },
                    { 7, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), true, "P", "Store Point" },
                    { 8, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), true, "V", "VIP Access" },
                    { 9, new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), true, "I", "Check-In" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Business_BusinessEmail",
                table: "B01_Business_Profile",
                column: "BusinessEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Business_BusinessName",
                table: "B01_Business_Profile",
                column: "BusinessName");

            migrationBuilder.CreateIndex(
                name: "IX_Business_BusinessPhone",
                table: "B01_Business_Profile",
                column: "BusinessPhone");

            migrationBuilder.CreateIndex(
                name: "IX_Business_CategoryID",
                table: "B01_Business_Profile",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "B02_Business_User",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUser_BusinessID",
                table: "B02_Business_User",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessUser_Email",
                table: "B02_Business_User",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "B02_Business_User",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCategory_CategoryName",
                table: "B04_Business_Category",
                column: "CategoryName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCategory_DisplayColumn",
                table: "B04_Business_Category",
                column: "DisplayColumn");

            migrationBuilder.CreateIndex(
                name: "IX_BusinessCategory_DisplayOrder",
                table: "B04_Business_Category",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_BusinessID",
                table: "B04_Coupon",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_CouponTypeID",
                table: "B04_Coupon",
                column: "CouponTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_IsActive",
                table: "B04_Coupon",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_IsFeatured",
                table: "B04_Coupon",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_QRCode",
                table: "B04_Coupon",
                column: "QRCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Coupon_TrackCode",
                table: "B04_Coupon",
                column: "TrackCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CouponType_IsActive",
                table: "B05_Coupon_Type",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_CouponType_TypeCode",
                table: "B05_Coupon_Type",
                column: "TypeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Business_Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Business_Roles_Claims_RoleId",
                table: "Business_Roles_Claims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_User_Claims_UserId",
                table: "Business_User_Claims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_User_Logins_UserId",
                table: "Business_User_Logins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Business_User_Roles_RoleId",
                table: "Business_User_Roles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "B04_Coupon");

            migrationBuilder.DropTable(
                name: "Business_Roles_Claims");

            migrationBuilder.DropTable(
                name: "Business_User_Claims");

            migrationBuilder.DropTable(
                name: "Business_User_Logins");

            migrationBuilder.DropTable(
                name: "Business_User_Roles");

            migrationBuilder.DropTable(
                name: "Business_User_Tokens");

            migrationBuilder.DropTable(
                name: "B05_Coupon_Type");

            migrationBuilder.DropTable(
                name: "Business_Roles");

            migrationBuilder.DropTable(
                name: "B02_Business_User");

            migrationBuilder.DropTable(
                name: "B01_Business_Profile");

            migrationBuilder.DropTable(
                name: "B04_Business_Category");
        }
    }
}
