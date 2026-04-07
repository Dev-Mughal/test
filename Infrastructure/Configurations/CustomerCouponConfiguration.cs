using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CustomerCouponConfiguration : IEntityTypeConfiguration<CustomerCoupon>
    {
        public void Configure(EntityTypeBuilder<CustomerCoupon> builder)
        {
            builder.HasKey(cc => cc.Id);

            builder.Property(cc => cc.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(cc => cc.CustomerId)
                .IsRequired()
                .HasColumnName("UserID");

            builder.Property(cc => cc.CouponId)
                .IsRequired()
                .HasColumnName("CouponID");

            builder.Property(cc => cc.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");

            builder.Property(cc => cc.Status)
                .IsRequired()
                .HasColumnName("Status");

            builder.Property(cc => cc.StatusDate)
                .IsRequired()
                .HasColumnName("StatusDate");

            builder.Property(cc => cc.Created)
                .IsRequired()
                .HasColumnName("Created");

            builder.Property(cc => cc.LastUpdated)
                .IsRequired()
                .HasColumnName("LastUpdated");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(cc => cc.DateRedeemed)
                .HasColumnName("DateRedeemed");

            builder.Property(cc => cc.CashierNote)
                .HasColumnType("text")
                .HasColumnName("CashierNote");

            builder.Property(cc => cc.StatusAdminNote)
                .HasColumnType("text")
                .HasColumnName("StatusAdminNote");

            builder.Property(cc => cc.StatusUserNote)
                .HasColumnType("text")
                .HasColumnName("StatusUserNote");
            #endregion

            #region INDEXES
            builder.HasIndex(cc => new { cc.CustomerId, cc.CouponId })
                .IsUnique()
                .HasDatabaseName("UX_CouponUserEnt_UserID_CouponID");

            builder.HasIndex(cc => cc.Status)
                .HasDatabaseName("IX_CouponUserEnt_Status");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(cc => cc.Customer)
                .WithMany(c => c.CustomerCoupons)
                .HasForeignKey(cc => cc.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(cc => cc.Coupon)
                .WithMany(c => c.CustomerCoupons)
                .HasForeignKey(cc => cc.CouponId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("10B_CouponUserEnt");
        }
    }
}
