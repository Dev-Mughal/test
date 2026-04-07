using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PromoUserUsageConfiguration : IEntityTypeConfiguration<PromoUserUsage>
    {
        public void Configure(EntityTypeBuilder<PromoUserUsage> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(p => p.UserId)
                .IsRequired()
                .HasColumnName("UserID");

            builder.Property(p => p.PromotionId)
                .IsRequired()
                .HasColumnName("PromotionID");

            builder.Property(p => p.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");

            builder.Property(p => p.LastUpdated)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("LastUpdated");

            builder.Property(p => p.Created)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("Created");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(p => p.UsedDate)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("UsedDate");
            #endregion

            #region INDEXES
            // Composite unique — one usage record per customer per promotion
            builder.HasIndex(p => new { p.UserId, p.PromotionId })
                .IsUnique()
                .HasDatabaseName("UX_11B_PromoUserUsage_UserID_PromotionID");

            // Supports querying all promotions a customer has used
            builder.HasIndex(p => p.UserId)
                .HasDatabaseName("IX_11B_PromoUserUsage_UserID");

            // Supports querying all customers who used a specific promotion
            builder.HasIndex(p => p.PromotionId)
                .HasDatabaseName("IX_11B_PromoUserUsage_PromotionID");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(p => p.Customer)
                .WithMany(c => c.PromoUserUsages)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.PromoBizDef)
                .WithMany(d => d.PromoUserUsages)
                .HasForeignKey(p => p.PromotionId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("11B_PromoUserUsage");
        }
    }
}
