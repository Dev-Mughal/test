using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class BizyPopDbContext(DbContextOptions<BizyPopDbContext> options)
        : IdentityDbContext<BusinessUser, IdentityRole<long>, long>(options)
    {
        public virtual DbSet<BusinessCategory> BusinessCategories { get; set; }
        public virtual DbSet<Business> Businesses { get; set; }
        public virtual DbSet<BusinessUser> BusinessUsers { get; set; }
        public virtual DbSet<BusinessUserBusiness> BusinessUserBusinesses { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<IncentiveType> IncentiveTypes { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerCoupon> CustomerCoupons { get; set; }
        public virtual DbSet<GeoCity> GeoCities { get; set; }
        public virtual DbSet<GeoZipCode> GeoZipCodes { get; set; }
        public virtual DbSet<State> States { get; set; }

        // BizyPop Dollars
        public virtual DbSet<BizDollarCreatedChannel> BizDollarCreatedChannels { get; set; }
        public virtual DbSet<BizDollarUserBalance> BizDollarUserBalances { get; set; }
        public virtual DbSet<BizDollarAction> BizDollarActions { get; set; }

        // Promotions
        public virtual DbSet<PromoBizDef> PromoBizDefs { get; set; }
        public virtual DbSet<PromoUserUsage> PromoUserUsages { get; set; }

        // Stamp Cards
        public virtual DbSet<StampBizDef> StampBizDefs { get; set; }
        public virtual DbSet<StampUserEnt> StampUserEnts { get; set; }
        public virtual DbSet<StampAction> StampActions { get; set; }
        public virtual DbSet<StampVoidLog> StampVoidLogs { get; set; }

        // Gift Cards
        public virtual DbSet<GiftCardBizDef> GiftCardBizDefs { get; set; }
        public virtual DbSet<GiftCardUserEnt> GiftCardUserEnts { get; set; }
        public virtual DbSet<GiftCardAction> GiftCardActions { get; set; }
        public virtual DbSet<GiftCardTransfer> GiftCardTransfers { get; set; }

        // Store Credit
        public virtual DbSet<StoreCreditReason> StoreCreditReasons { get; set; }
        public virtual DbSet<StoreCreditBizDef> StoreCreditBizDefs { get; set; }
        public virtual DbSet<StoreCreditUserEnt> StoreCreditUserEnts { get; set; }
        public virtual DbSet<StoreCreditAction> StoreCreditActions { get; set; }

        // Store Points
        public virtual DbSet<StorePointsBizDef> StorePointsBizDefs { get; set; }
        public virtual DbSet<StorePointUserEnt> StorePointUserEnts { get; set; }
        public virtual DbSet<StorePointAction> StorePointActions { get; set; }
        public virtual DbSet<StorePointTransfer> StorePointTransfers { get; set; }

        // VIP Access
        public virtual DbSet<VipBizDef> VipBizDefs { get; set; }
        public virtual DbSet<VipUserEnt> VipUserEnts { get; set; }
        public virtual DbSet<VipAction> VipActions { get; set; }
        public virtual DbSet<VipTransfer> VipTransfers { get; set; }

        // Raffles
        public virtual DbSet<RaffleDef> RaffleDefs { get; set; }
        public virtual DbSet<RaffleSchedule> RaffleSchedules { get; set; }
        public virtual DbSet<RaffleTicket> RaffleTickets { get; set; }
        public virtual DbSet<RaffleWinner> RaffleWinners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Custom Identity table names
            modelBuilder.Entity<IdentityUserClaim<long>>().ToTable("Business_User_Claims");
            modelBuilder.Entity<IdentityUserLogin<long>>().ToTable("Business_User_Logins");
            modelBuilder.Entity<IdentityUserToken<long>>().ToTable("Business_User_Tokens");
            modelBuilder.Entity<IdentityUserRole<long>>().ToTable("Business_User_Roles");
            modelBuilder.Entity<IdentityRole<long>>().ToTable("Business_Roles");
            modelBuilder.Entity<IdentityRoleClaim<long>>().ToTable("Business_Roles_Claims");

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(BizyPopDbContext).Assembly,
                type => type.Namespace == "Infrastructure.Configurations");
        }
    }
}

