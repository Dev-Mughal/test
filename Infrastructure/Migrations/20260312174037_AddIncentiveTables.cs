using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddIncentiveTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "11A_PromoBizDef",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessID = table.Column<long>(type: "bigint", nullable: false),
                    PromotionDesc = table.Column<string>(name: "Promotion Desc", type: "text", nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    TrackCode = table.Column<string>(type: "text", nullable: false),
                    FinePrint = table.Column<string>(type: "text", nullable: true),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AdminNote = table.Column<string>(type: "text", nullable: true),
                    CashierPOSMessage = table.Column<string>(type: "text", nullable: true),
                    VoidedReason = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_11A_PromoBizDef", x => x.ID);
                    table.ForeignKey(
                        name: "FK_11A_PromoBizDef_B01_Business_Profile_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "12A_StampBizDef",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessID = table.Column<long>(type: "bigint", nullable: false),
                    RewardDesc = table.Column<string>(type: "text", nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    TrackCode = table.Column<string>(type: "text", nullable: false),
                    StampGoal = table.Column<int>(type: "integer", nullable: false),
                    StampCount = table.Column<int>(type: "integer", nullable: false),
                    GoalReachedMessage = table.Column<string>(type: "text", nullable: true),
                    FinePrint = table.Column<string>(type: "text", nullable: true),
                    AdminNote = table.Column<string>(type: "text", nullable: true),
                    CashierPOSMessage = table.Column<string>(type: "text", nullable: true),
                    MaxStampPerDay = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_12A_StampBizDef", x => x.ID);
                    table.ForeignKey(
                        name: "FK_12A_StampBizDef_B01_Business_Profile_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "20A_GiftCardBizDef",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessID = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    TrackCode = table.Column<string>(type: "text", nullable: false),
                    MarketingText = table.Column<string>(type: "text", nullable: true),
                    FinePrint = table.Column<string>(type: "text", nullable: true),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AdminNote = table.Column<string>(type: "text", nullable: true),
                    CashierPOSMessage = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StatusNote = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_20A_GiftCardBizDef", x => x.ID);
                    table.ForeignKey(
                        name: "FK_20A_GiftCardBizDef_B01_Business_Profile_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "30A_VIPBizDef",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessID = table.Column<long>(type: "bigint", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    TrackCode = table.Column<string>(type: "text", nullable: false),
                    DesignData = table.Column<string>(type: "text", nullable: true),
                    FinePrint = table.Column<string>(type: "text", nullable: true),
                    DefaultStartDay = table.Column<int>(type: "integer", nullable: true),
                    DefaultEndDay = table.Column<int>(type: "integer", nullable: true),
                    DefaultDailyStartHour = table.Column<int>(type: "integer", nullable: true),
                    DefaultDailyEndHour = table.Column<int>(type: "integer", nullable: true),
                    Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AdminNote = table.Column<string>(type: "text", nullable: true),
                    CashierPOSMessage = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_30A_VIPBizDef", x => x.ID);
                    table.ForeignKey(
                        name: "FK_30A_VIPBizDef_B01_Business_Profile_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "30B_VIPUserEnt",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    BusinessID = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    StatusDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StatusNote = table.Column<string>(type: "text", nullable: true),
                    CashierNote = table.Column<string>(type: "text", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StartDay = table.Column<int>(type: "integer", nullable: true),
                    EndDay = table.Column<int>(type: "integer", nullable: true),
                    DailyStartHour = table.Column<int>(type: "integer", nullable: true),
                    DailyEndHour = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_30B_VIPUserEnt", x => x.ID);
                    table.ForeignKey(
                        name: "FK_30B_VIPUserEnt_B01_Business_Profile_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_30B_VIPUserEnt_C01_Customer_UserID",
                        column: x => x.UserID,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "40A_RaffleDef",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    BusinessID = table.Column<long>(type: "bigint", nullable: false),
                    Enabled = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    QRCode = table.Column<string>(type: "text", nullable: false),
                    TrackCode = table.Column<string>(type: "text", nullable: false),
                    MinimumEntry = table.Column<int>(type: "integer", nullable: true),
                    GiftCardValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    StoreCreditValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    CustomPrize = table.Column<string>(type: "text", nullable: true),
                    CustomPrizeValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: true),
                    ScheduleType = table.Column<int>(type: "integer", nullable: true),
                    _2_Dayoftheweek = table.Column<int>(name: "2_Day of the week", type: "integer", nullable: true),
                    _3_DrawingMonthDay = table.Column<int>(name: "3_DrawingMonthDay", type: "integer", nullable: true),
                    _4_DateOfDrawing = table.Column<DateTime>(name: "4_DateOfDrawing", type: "timestamp with time zone", nullable: true),
                    DrawingTime = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    LastUpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TicketUsageType = table.Column<int>(type: "integer", nullable: true),
                    PreviousLastDaysToUse = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_40A_RaffleDef", x => x.ID);
                    table.ForeignKey(
                        name: "FK_40A_RaffleDef_B01_Business_Profile_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "B01_Business_Profile",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "11B_PromoUserUsage",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    PromotionID = table.Column<long>(type: "bigint", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_11B_PromoUserUsage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_11B_PromoUserUsage_11A_PromoBizDef_PromotionID",
                        column: x => x.PromotionID,
                        principalTable: "11A_PromoBizDef",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_11B_PromoUserUsage_C01_Customer_UserID",
                        column: x => x.UserID,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "12B_StampUserEnt",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    StampID = table.Column<long>(type: "bigint", nullable: false),
                    RedeemedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StampCount = table.Column<int>(type: "integer", nullable: false),
                    StampGoal = table.Column<int>(type: "integer", nullable: false),
                    CashierNote = table.Column<string>(type: "text", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_12B_StampUserEnt", x => x.ID);
                    table.ForeignKey(
                        name: "FK_12B_StampUserEnt_12A_StampBizDef_StampID",
                        column: x => x.StampID,
                        principalTable: "12A_StampBizDef",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_12B_StampUserEnt_C01_Customer_UserID",
                        column: x => x.UserID,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "20B_GiftCardUserEnt",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    GiftCardID = table.Column<long>(type: "bigint", nullable: false),
                    GiftCardBalance = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    GiftCardValue = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CashierNote = table.Column<string>(type: "text", nullable: true),
                    LastUpdated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_20B_GiftCardUserEnt", x => x.ID);
                    table.ForeignKey(
                        name: "FK_20B_GiftCardUserEnt_20A_GiftCardBizDef_GiftCardID",
                        column: x => x.GiftCardID,
                        principalTable: "20A_GiftCardBizDef",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_20B_GiftCardUserEnt_C01_Customer_UserID",
                        column: x => x.UserID,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "30C_VipAction",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntitlementID = table.Column<long>(type: "bigint", nullable: false),
                    CashierID = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransferRecieverUserID = table.Column<long>(type: "bigint", nullable: true),
                    IsValid = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_30C_VipAction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_30C_VipAction_30B_VIPUserEnt_EntitlementID",
                        column: x => x.EntitlementID,
                        principalTable: "30B_VIPUserEnt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "30T_VipTransfer",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderEntitlementID = table.Column<long>(type: "bigint", nullable: false),
                    RecieverEntitlementID = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    CashierID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_30T_VipTransfer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_30T_VipTransfer_30B_VIPUserEnt_RecieverEntitlementID",
                        column: x => x.RecieverEntitlementID,
                        principalTable: "30B_VIPUserEnt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_30T_VipTransfer_30B_VIPUserEnt_SenderEntitlementID",
                        column: x => x.SenderEntitlementID,
                        principalTable: "30B_VIPUserEnt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "40B_RaffleSchedule",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RaffleID = table.Column<long>(type: "bigint", nullable: false),
                    DateOfDrawing = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessingStartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessingEndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_40B_RaffleSchedule", x => x.ID);
                    table.ForeignKey(
                        name: "FK_40B_RaffleSchedule_40A_RaffleDef_RaffleID",
                        column: x => x.RaffleID,
                        principalTable: "40A_RaffleDef",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "41C_RaffleTicket",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RaffleID = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreationCode = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_41C_RaffleTicket", x => x.ID);
                    table.ForeignKey(
                        name: "FK_41C_RaffleTicket_40A_RaffleDef_RaffleID",
                        column: x => x.RaffleID,
                        principalTable: "40A_RaffleDef",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "41W_RaffleWinner",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RaffleID = table.Column<long>(type: "bigint", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    StoreCreditAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    GiftCardAmount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_41W_RaffleWinner", x => x.ID);
                    table.ForeignKey(
                        name: "FK_41W_RaffleWinner_40A_RaffleDef_RaffleID",
                        column: x => x.RaffleID,
                        principalTable: "40A_RaffleDef",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_41W_RaffleWinner_C01_Customer_UserID",
                        column: x => x.UserID,
                        principalTable: "C01_Customer",
                        principalColumn: "CustomerID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "12C_StampAction",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntitlementID = table.Column<long>(type: "bigint", nullable: false),
                    CashierID = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    IsVoided = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_12C_StampAction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_12C_StampAction_12B_StampUserEnt_EntitlementID",
                        column: x => x.EntitlementID,
                        principalTable: "12B_StampUserEnt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "12V_StampVoidLog",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntitlmentID = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    CashierID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_12V_StampVoidLog", x => x.ID);
                    table.ForeignKey(
                        name: "FK_12V_StampVoidLog_12B_StampUserEnt_EntitlmentID",
                        column: x => x.EntitlmentID,
                        principalTable: "12B_StampUserEnt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "20C_GiftCardAction",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EntitlementID = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    CashierID = table.Column<long>(type: "bigint", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserID = table.Column<long>(type: "bigint", nullable: false),
                    TransferRecieverUserID = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_20C_GiftCardAction", x => x.ID);
                    table.ForeignKey(
                        name: "FK_20C_GiftCardAction_20B_GiftCardUserEnt_EntitlementID",
                        column: x => x.EntitlementID,
                        principalTable: "20B_GiftCardUserEnt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "20T_GiftCardTransfer",
                columns: table => new
                {
                    ID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderEntitlementID = table.Column<long>(type: "bigint", nullable: false),
                    RecieverEntitlementID = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "text", nullable: false),
                    CashierID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_20T_GiftCardTransfer", x => x.ID);
                    table.ForeignKey(
                        name: "FK_20T_GiftCardTransfer_20B_GiftCardUserEnt_RecieverEntitlemen~",
                        column: x => x.RecieverEntitlementID,
                        principalTable: "20B_GiftCardUserEnt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_20T_GiftCardTransfer_20B_GiftCardUserEnt_SenderEntitlementID",
                        column: x => x.SenderEntitlementID,
                        principalTable: "20B_GiftCardUserEnt",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_11A_PromoBizDef_BusinessID",
                table: "11A_PromoBizDef",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_11A_PromoBizDef_DateRange",
                table: "11A_PromoBizDef",
                columns: new[] { "StartDate", "ExpirationDate" });

            migrationBuilder.CreateIndex(
                name: "IX_11B_PromoUserUsage_PromotionID",
                table: "11B_PromoUserUsage",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_11B_PromoUserUsage_UserID",
                table: "11B_PromoUserUsage",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "UX_11B_PromoUserUsage_UserID_PromotionID",
                table: "11B_PromoUserUsage",
                columns: new[] { "UserID", "PromotionID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_12A_StampBizDef_BusinessID",
                table: "12A_StampBizDef",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_12B_StampUserEnt_StampID",
                table: "12B_StampUserEnt",
                column: "StampID");

            migrationBuilder.CreateIndex(
                name: "IX_12B_StampUserEnt_Status",
                table: "12B_StampUserEnt",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UX_12B_StampUserEnt_UserID_StampID",
                table: "12B_StampUserEnt",
                columns: new[] { "UserID", "StampID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_12C_StampAction_EntitlementID",
                table: "12C_StampAction",
                column: "EntitlementID");

            migrationBuilder.CreateIndex(
                name: "IX_12C_StampAction_TransactionDate",
                table: "12C_StampAction",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_12V_StampVoidLog_EntitlementID",
                table: "12V_StampVoidLog",
                column: "EntitlmentID");

            migrationBuilder.CreateIndex(
                name: "IX_20A_GiftCardBizDef_BusinessID",
                table: "20A_GiftCardBizDef",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_20A_GiftCardBizDef_Status",
                table: "20A_GiftCardBizDef",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_20B_GiftCardUserEnt_GiftCardID",
                table: "20B_GiftCardUserEnt",
                column: "GiftCardID");

            migrationBuilder.CreateIndex(
                name: "IX_20B_GiftCardUserEnt_UserID",
                table: "20B_GiftCardUserEnt",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_20C_GiftCardAction_EntitlementID",
                table: "20C_GiftCardAction",
                column: "EntitlementID");

            migrationBuilder.CreateIndex(
                name: "IX_20C_GiftCardAction_TransactionDate",
                table: "20C_GiftCardAction",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_20T_GiftCardTransfer_RecieverEntitlementID",
                table: "20T_GiftCardTransfer",
                column: "RecieverEntitlementID");

            migrationBuilder.CreateIndex(
                name: "IX_20T_GiftCardTransfer_SenderEntitlementID",
                table: "20T_GiftCardTransfer",
                column: "SenderEntitlementID");

            migrationBuilder.CreateIndex(
                name: "UX_30A_VIPBizDef_BusinessID",
                table: "30A_VIPBizDef",
                column: "BusinessID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_30B_VIPUserEnt_BusinessID",
                table: "30B_VIPUserEnt",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_30B_VIPUserEnt_Status",
                table: "30B_VIPUserEnt",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "UX_30B_VIPUserEnt_UserID_BusinessID",
                table: "30B_VIPUserEnt",
                columns: new[] { "UserID", "BusinessID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_30C_VipAction_EntitlementID",
                table: "30C_VipAction",
                column: "EntitlementID");

            migrationBuilder.CreateIndex(
                name: "IX_30C_VipAction_TransactionDate",
                table: "30C_VipAction",
                column: "TransactionDate");

            migrationBuilder.CreateIndex(
                name: "IX_30T_VipTransfer_RecieverEntitlementID",
                table: "30T_VipTransfer",
                column: "RecieverEntitlementID");

            migrationBuilder.CreateIndex(
                name: "IX_30T_VipTransfer_SenderEntitlementID",
                table: "30T_VipTransfer",
                column: "SenderEntitlementID");

            migrationBuilder.CreateIndex(
                name: "IX_40A_RaffleDef_BusinessID",
                table: "40A_RaffleDef",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_40A_RaffleDef_Enabled",
                table: "40A_RaffleDef",
                column: "Enabled");

            migrationBuilder.CreateIndex(
                name: "IX_40B_RaffleSchedule_DateOfDrawing",
                table: "40B_RaffleSchedule",
                column: "DateOfDrawing");

            migrationBuilder.CreateIndex(
                name: "IX_40B_RaffleSchedule_RaffleID",
                table: "40B_RaffleSchedule",
                column: "RaffleID");

            migrationBuilder.CreateIndex(
                name: "IX_41C_RaffleTicket_RaffleID",
                table: "41C_RaffleTicket",
                column: "RaffleID");

            migrationBuilder.CreateIndex(
                name: "UX_41C_RaffleTicket_CreationCode",
                table: "41C_RaffleTicket",
                column: "CreationCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_41W_RaffleWinner_RaffleID",
                table: "41W_RaffleWinner",
                column: "RaffleID");

            migrationBuilder.CreateIndex(
                name: "IX_41W_RaffleWinner_UserID",
                table: "41W_RaffleWinner",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "11B_PromoUserUsage");

            migrationBuilder.DropTable(
                name: "12C_StampAction");

            migrationBuilder.DropTable(
                name: "12V_StampVoidLog");

            migrationBuilder.DropTable(
                name: "20C_GiftCardAction");

            migrationBuilder.DropTable(
                name: "20T_GiftCardTransfer");

            migrationBuilder.DropTable(
                name: "30A_VIPBizDef");

            migrationBuilder.DropTable(
                name: "30C_VipAction");

            migrationBuilder.DropTable(
                name: "30T_VipTransfer");

            migrationBuilder.DropTable(
                name: "40B_RaffleSchedule");

            migrationBuilder.DropTable(
                name: "41C_RaffleTicket");

            migrationBuilder.DropTable(
                name: "41W_RaffleWinner");

            migrationBuilder.DropTable(
                name: "11A_PromoBizDef");

            migrationBuilder.DropTable(
                name: "12B_StampUserEnt");

            migrationBuilder.DropTable(
                name: "20B_GiftCardUserEnt");

            migrationBuilder.DropTable(
                name: "30B_VIPUserEnt");

            migrationBuilder.DropTable(
                name: "40A_RaffleDef");

            migrationBuilder.DropTable(
                name: "12A_StampBizDef");

            migrationBuilder.DropTable(
                name: "20A_GiftCardBizDef");
        }
    }
}
