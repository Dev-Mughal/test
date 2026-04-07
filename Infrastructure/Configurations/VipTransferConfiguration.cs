using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class VipTransferConfiguration : IEntityTypeConfiguration<VipTransfer>
    {
        public void Configure(EntityTypeBuilder<VipTransfer> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(v => v.SenderEntitlementId)
                .IsRequired()
                .HasColumnName("SenderEntitlementID");

            // "RecieverEntitlementID" preserves the original Access DB column spelling.
            builder.Property(v => v.ReceiverEntitlementId)
                .IsRequired()
                .HasColumnName("RecieverEntitlementID");

            builder.Property(v => v.Reason)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Reason");

            builder.Property(v => v.CashierId)
                .IsRequired()
                .HasColumnName("CashierID");
            #endregion

            #region INDEXES
            builder.HasIndex(v => v.SenderEntitlementId)
                .HasDatabaseName("IX_30T_VipTransfer_SenderEntitlementID");

            builder.HasIndex(v => v.ReceiverEntitlementId)
                .HasDatabaseName("IX_30T_VipTransfer_RecieverEntitlementID");
            #endregion

            #region RELATIONSHIPS
            // Restrict delete on both FKs — cannot delete a VIP entitlement that has transfer records.
            builder.HasOne(v => v.SenderEntitlement)
                .WithMany()
                .HasForeignKey(v => v.SenderEntitlementId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(v => v.ReceiverEntitlement)
                .WithMany()
                .HasForeignKey(v => v.ReceiverEntitlementId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            builder.ToTable("30T_VipTransfer");
        }
    }
}
