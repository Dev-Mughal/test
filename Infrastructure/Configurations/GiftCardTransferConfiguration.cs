using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class GiftCardTransferConfiguration : IEntityTypeConfiguration<GiftCardTransfer>
    {
        public void Configure(EntityTypeBuilder<GiftCardTransfer> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(g => g.SenderEntitlementId)
                .IsRequired()
                .HasColumnName("SenderEntitlementID");

            // "RecieverEntitlementID" preserves the original Access DB column spelling.
            builder.Property(g => g.ReceiverEntitlementId)
                .IsRequired()
                .HasColumnName("RecieverEntitlementID");

            builder.Property(g => g.Reason)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Reason");

            builder.Property(g => g.CashierId)
                .IsRequired()
                .HasColumnName("CashierID");
            #endregion

            #region INDEXES
            builder.HasIndex(g => g.SenderEntitlementId)
                .HasDatabaseName("IX_20T_GiftCardTransfer_SenderEntitlementID");

            builder.HasIndex(g => g.ReceiverEntitlementId)
                .HasDatabaseName("IX_20T_GiftCardTransfer_RecieverEntitlementID");
            #endregion

            #region RELATIONSHIPS
            // Restrict delete on both FKs — cannot delete an entitlement that has transfer records.
            builder.HasOne(g => g.SenderEntitlement)
                .WithMany()
                .HasForeignKey(g => g.SenderEntitlementId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(g => g.ReceiverEntitlement)
                .WithMany()
                .HasForeignKey(g => g.ReceiverEntitlementId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            builder.ToTable("20T_GiftCardTransfer");
        }
    }
}
