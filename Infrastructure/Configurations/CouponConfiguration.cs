using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("CouponID")
                .ValueGeneratedOnAdd();

            #region PROPERTIES
            builder.Property(c => c.BusinessId)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("BusinessID");

            builder.Property(c => c.Title)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Title");

            builder.Property(c => c.Description)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Description");

            builder.Property(c => c.PhotoUrl)
                .HasColumnType("text")
                .HasColumnName("Picture");

            // QRCode populated in a second SaveChangesAsync after the PK is known.
            // No IsRequired — nullable column allows the initial insert without it.
            builder.Property(c => c.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");

            builder.Property(c => c.StartDateTime)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("StartDateTime");

            builder.Property(c => c.EndDateTime)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("EndDateTime");

            builder.Property(c => c.ExpirationTime)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("ExpirationTime");

            builder.Property(c => c.CreatedOn)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("CreatedOn");

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasColumnType("boolean")
                .HasColumnName("IsActive");

            builder.Property(c => c.IsFeatured)
                .IsRequired()
                .HasColumnType("boolean")
                .HasColumnName("IsFeatured");
            #endregion

            #region INDEXES
            // Composite index for paginated coupon listing per business ordered by date
            builder.HasIndex(c => new { c.BusinessId, c.CreatedOn })
                .IsDescending(false, true)
                .HasDatabaseName("IX_Coupon_BusinessID_CreatedOn");

            // Composite index covering active/featured status filters
            builder.HasIndex(c => new { c.IsActive, c.IsFeatured })
                .HasDatabaseName("IX_Coupon_IsActive_IsFeatured");

            builder.HasIndex(c => c.QRCode)
                .IsUnique()
                .HasDatabaseName("IX_Coupon_QRCode");

            // Index supporting date-range queries for active/expiring coupons
            builder.HasIndex(c => new { c.StartDateTime, c.EndDateTime })
                .HasDatabaseName("IX_Coupon_DateRange");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(c => c.Business)
                .WithMany(b => b.Coupons)
                .HasForeignKey(c => c.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("10A_Coupon_BizDef");
        }
    }
}

