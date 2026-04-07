using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIncentiveUpdatesAndLookupTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_12V_StampVoidLog_12B_StampUserEnt_EntitlmentID",
                table: "12V_StampVoidLog");

            migrationBuilder.DropColumn(
                name: "StampCount",
                table: "12A_StampBizDef");

            migrationBuilder.RenameColumn(
                name: "EntitlmentID",
                table: "12V_StampVoidLog",
                newName: "EntitlementId");

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "30B_VIPUserEnt",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "30A_VIPBizDef",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "30A_VIPBizDef",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "20C_GiftCardAction",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "GiftCardValue",
                table: "20B_GiftCardUserEnt",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<int>(
                name: "GiftCardBalance",
                table: "20B_GiftCardUserEnt",
                type: "integer",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AddColumn<long>(
                name: "CustomerId",
                table: "20B_GiftCardUserEnt",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "20A_GiftCardBizDef",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AddColumn<int>(
                name: "GiftCardValue",
                table: "20A_GiftCardBizDef",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "20A_GiftCardBizDef",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "12A_StampBizDef",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "12A_StampBizDef",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "11A_PromoBizDef",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "11A_PromoBizDef",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "10A_Coupon_BizDef",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int4");

            migrationBuilder.CreateTable(
                name: "01LK1_BizDollerCreatedChannel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ChannelCD = table.Column<int>(type: "integer", nullable: false),
                    ChannelDescription = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_01LK1_BizDollerCreatedChannel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "21LK1_StoreCreditReason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ReasonDescription = table.Column<string>(name: "Reason Description", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_21LK1_StoreCreditReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BizDollarUserBalances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Balance = table.Column<int>(type: "integer", nullable: false),
                    CreatedChannel = table.Column<int>(type: "integer", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizDollarUserBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BizDollarUserBalances_C01_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID");
                    table.ForeignKey(
                        name: "FK_BizDollarUserBalances_C01_Customer_UserId",
                        column: x => x.UserId,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreCreditBizDefs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessId = table.Column<int>(type: "int4", nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    AdminNote = table.Column<string>(type: "text", nullable: true),
                    CashierPOSMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCreditBizDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCreditBizDefs_B01_Business_Profile_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorePointsBizDefs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessId = table.Column<int>(type: "int4", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    PhotoUrl = table.Column<string>(type: "text", nullable: true),
                    FinePrint = table.Column<string>(type: "text", nullable: true),
                    AdminNote = table.Column<string>(type: "text", nullable: true),
                    CashierPOSMessage = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StatusNote = table.Column<string>(type: "text", nullable: true),
                    DollarPointRatio = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorePointsBizDefs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorePointsBizDefs_B01_Business_Profile_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BizDollarActions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntitlementId = table.Column<long>(type: "bigint", nullable: false),
                    BusinessId = table.Column<int>(type: "int4", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    CashierId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BizDollarUserBalanceId = table.Column<long>(type: "bigint", nullable: false),
                    BusinessId1 = table.Column<int>(type: "int4", nullable: true),
                    CustomerId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BizDollarActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BizDollarActions_B01_Business_Profile_BusinessId",
                        column: x => x.BusinessId,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BizDollarActions_B01_Business_Profile_BusinessId1",
                        column: x => x.BusinessId1,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID");
                    table.ForeignKey(
                        name: "FK_BizDollarActions_BizDollarUserBalances_BizDollarUserBalance~",
                        column: x => x.BizDollarUserBalanceId,
                        principalTable: "BizDollarUserBalances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BizDollarActions_C01_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID");
                });

            migrationBuilder.CreateTable(
                name: "StoreCreditUserEnts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    StoreCredId = table.Column<long>(type: "bigint", nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    StoreCreditBalance = table.Column<int>(type: "integer", nullable: false),
                    CashierNote = table.Column<string>(type: "text", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    StoreCreditBizDefId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCreditUserEnts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCreditUserEnts_C01_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StoreCreditUserEnts_StoreCreditBizDefs_StoreCreditBizDefId",
                        column: x => x.StoreCreditBizDefId,
                        principalTable: "StoreCreditBizDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorePointUserEnts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    StorePointId = table.Column<long>(type: "bigint", nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    StorePointTotal = table.Column<int>(type: "integer", nullable: false),
                    CashierNote = table.Column<string>(type: "text", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CustomerId = table.Column<long>(type: "bigint", nullable: false),
                    StorePointsBizDefId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorePointUserEnts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorePointUserEnts_C01_Customer_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StorePointUserEnts_StorePointsBizDefs_StorePointsBizDefId",
                        column: x => x.StorePointsBizDefId,
                        principalTable: "StorePointsBizDefs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StoreCreditActions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntitlementId = table.Column<long>(type: "bigint", nullable: false),
                    TransAmount = table.Column<int>(type: "integer", nullable: false),
                    CashierId = table.Column<long>(type: "bigint", nullable: false),
                    TransDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ReasonId = table.Column<int>(type: "integer", nullable: true),
                    StoreCreditUserEntId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreCreditActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StoreCreditActions_StoreCreditUserEnts_StoreCreditUserEntId",
                        column: x => x.StoreCreditUserEntId,
                        principalTable: "StoreCreditUserEnts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "22T_StorePointTransfer",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderEntitlementId = table.Column<long>(type: "bigint", nullable: false),
                    ReceiverEntitlementId = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    CashierId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_22T_StorePointTransfer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_22T_StorePointTransfer_StorePointUserEnts_ReceiverEntitleme~",
                        column: x => x.ReceiverEntitlementId,
                        principalTable: "StorePointUserEnts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_22T_StorePointTransfer_StorePointUserEnts_SenderEntitlement~",
                        column: x => x.SenderEntitlementId,
                        principalTable: "StorePointUserEnts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StorePointActions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntitlementId = table.Column<long>(type: "bigint", nullable: false),
                    PointAmount = table.Column<int>(type: "integer", nullable: false),
                    CashierId = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsTransfer = table.Column<bool>(type: "boolean", nullable: false),
                    StorePointUserEntId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StorePointActions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StorePointActions_StorePointUserEnts_StorePointUserEntId",
                        column: x => x.StorePointUserEntId,
                        principalTable: "StorePointUserEnts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "01LK1_BizDollerCreatedChannel",
                columns: new[] { "Id", "ChannelCD", "ChannelDescription" },
                values: new object[] { 1, 0, "New member reward" });

            migrationBuilder.InsertData(
                table: "21LK1_StoreCreditReason",
                columns: new[] { "Id", "Reason Description" },
                values: new object[,]
                {
                    { 1, "Customer Service Issue" },
                    { 2, "Quality Issue" },
                    { 3, "Friend" },
                    { 4, "Family" }
                });

            migrationBuilder.InsertData(
                table: "B04_Business_Category",
                columns: new[] { "CategoryID", "CategoryName", "CategorySlug", "CreatedOn", "DisplayColumn", "DisplayOrder", "IsActive" },
                values: new object[] { 16, "Everything Else", "everything-else", new DateTime(2026, 7, 11, 0, 0, 0, 0, DateTimeKind.Utc), (short)1, (short)8, true });

            migrationBuilder.CreateIndex(
                name: "IX_30B_VIPUserEnt_CustomerId",
                table: "30B_VIPUserEnt",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_20B_GiftCardUserEnt_CustomerId",
                table: "20B_GiftCardUserEnt",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePointTransfers_ReceiverEntitlementId",
                table: "22T_StorePointTransfer",
                column: "ReceiverEntitlementId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePointTransfers_SenderEntitlementId",
                table: "22T_StorePointTransfer",
                column: "SenderEntitlementId");

            migrationBuilder.CreateIndex(
                name: "IX_BizDollarActions_BizDollarUserBalanceId",
                table: "BizDollarActions",
                column: "BizDollarUserBalanceId");

            migrationBuilder.CreateIndex(
                name: "IX_BizDollarActions_BusinessId",
                table: "BizDollarActions",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_BizDollarActions_BusinessId1",
                table: "BizDollarActions",
                column: "BusinessId1");

            migrationBuilder.CreateIndex(
                name: "IX_BizDollarActions_CustomerId",
                table: "BizDollarActions",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BizDollarActions_EntitlementId",
                table: "BizDollarActions",
                column: "EntitlementId");

            migrationBuilder.CreateIndex(
                name: "IX_BizDollarActions_TransactionDate",
                table: "BizDollarActions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_BizDollarUserBalances_CustomerId",
                table: "BizDollarUserBalances",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_BizDollarUserBalances_UserId",
                table: "BizDollarUserBalances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCreditActions_EntitlementId",
                table: "StoreCreditActions",
                column: "EntitlementId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCreditActions_StoreCreditUserEntId",
                table: "StoreCreditActions",
                column: "StoreCreditUserEntId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCreditActions_TransDate",
                table: "StoreCreditActions",
                column: "TransDate");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCreditBizDefs_BusinessId",
                table: "StoreCreditBizDefs",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCreditUserEnts_CustomerId",
                table: "StoreCreditUserEnts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreCreditUserEnts_StoreCreditBizDefId",
                table: "StoreCreditUserEnts",
                column: "StoreCreditBizDefId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePointActions_EntitlementId",
                table: "StorePointActions",
                column: "EntitlementId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePointActions_StorePointUserEntId",
                table: "StorePointActions",
                column: "StorePointUserEntId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePointActions_TransactionDate",
                table: "StorePointActions",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_StorePointsBizDefs_BusinessId",
                table: "StorePointsBizDefs",
                column: "BusinessId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePointUserEnts_CustomerId",
                table: "StorePointUserEnts",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_StorePointUserEnts_StorePointsBizDefId",
                table: "StorePointUserEnts",
                column: "StorePointsBizDefId");

            migrationBuilder.AddForeignKey(
                name: "FK_12V_StampVoidLog_12B_StampUserEnt_EntitlementId",
                table: "12V_StampVoidLog",
                column: "EntitlementId",
                principalTable: "12B_StampUserEnt",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_20B_GiftCardUserEnt_C01_Customer_CustomerId",
                table: "20B_GiftCardUserEnt",
                column: "CustomerId",
                principalTable: "C01_Customer",
                principalColumn: "CustomerID");

            migrationBuilder.AddForeignKey(
                name: "FK_30B_VIPUserEnt_C01_Customer_CustomerId",
                table: "30B_VIPUserEnt",
                column: "CustomerId",
                principalTable: "C01_Customer",
                principalColumn: "CustomerID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_12V_StampVoidLog_12B_StampUserEnt_EntitlementId",
                table: "12V_StampVoidLog");

            migrationBuilder.DropForeignKey(
                name: "FK_20B_GiftCardUserEnt_C01_Customer_CustomerId",
                table: "20B_GiftCardUserEnt");

            migrationBuilder.DropForeignKey(
                name: "FK_30B_VIPUserEnt_C01_Customer_CustomerId",
                table: "30B_VIPUserEnt");

            migrationBuilder.DropTable(
                name: "01LK1_BizDollerCreatedChannel");

            migrationBuilder.DropTable(
                name: "21LK1_StoreCreditReason");

            migrationBuilder.DropTable(
                name: "22T_StorePointTransfer");

            migrationBuilder.DropTable(
                name: "BizDollarActions");

            migrationBuilder.DropTable(
                name: "StoreCreditActions");

            migrationBuilder.DropTable(
                name: "StorePointActions");

            migrationBuilder.DropTable(
                name: "BizDollarUserBalances");

            migrationBuilder.DropTable(
                name: "StoreCreditUserEnts");

            migrationBuilder.DropTable(
                name: "StorePointUserEnts");

            migrationBuilder.DropTable(
                name: "StoreCreditBizDefs");

            migrationBuilder.DropTable(
                name: "StorePointsBizDefs");

            migrationBuilder.DropIndex(
                name: "IX_30B_VIPUserEnt_CustomerId",
                table: "30B_VIPUserEnt");

            migrationBuilder.DropIndex(
                name: "IX_20B_GiftCardUserEnt_CustomerId",
                table: "20B_GiftCardUserEnt");

            migrationBuilder.DeleteData(
                table: "B04_Business_Category",
                keyColumn: "CategoryID",
                keyValue: 16);

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "30B_VIPUserEnt");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "30A_VIPBizDef");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "20B_GiftCardUserEnt");

            migrationBuilder.DropColumn(
                name: "GiftCardValue",
                table: "20A_GiftCardBizDef");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "20A_GiftCardBizDef");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "12A_StampBizDef");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "11A_PromoBizDef");

            migrationBuilder.RenameColumn(
                name: "EntitlementId",
                table: "12V_StampVoidLog",
                newName: "EntitlmentID");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "30A_VIPBizDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "20C_GiftCardAction",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "GiftCardValue",
                table: "20B_GiftCardUserEnt",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<decimal>(
                name: "GiftCardBalance",
                table: "20B_GiftCardUserEnt",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "20A_GiftCardBizDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "12A_StampBizDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "StampCount",
                table: "12A_StampBizDef",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "11A_PromoBizDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "BusinessID",
                table: "10A_Coupon_BizDef",
                type: "int4",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_12V_StampVoidLog_12B_StampUserEnt_EntitlmentID",
                table: "12V_StampVoidLog",
                column: "EntitlmentID",
                principalTable: "12B_StampUserEnt",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
