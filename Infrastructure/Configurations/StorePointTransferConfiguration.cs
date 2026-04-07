using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StorePointTransferConfiguration : IEntityTypeConfiguration<StorePointTransfer>
    {
        public void Configure(EntityTypeBuilder<StorePointTransfer> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(s => s.SenderEntitlementId)
                .IsRequired()
                .HasColumnName("SenderEntitlementId");

            builder.Property(s => s.ReceiverEntitlementId)
                .IsRequired()
                .HasColumnName("ReceiverEntitlementId");

            builder.Property(s => s.Reason)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Reason");

            builder.Property(s => s.CashierId)
                .IsRequired()
                .HasColumnName("CashierId");
            #endregion

            #region INDEXES
            builder.HasIndex(s => s.SenderEntitlementId)
                .HasDatabaseName("IX_StorePointTransfers_SenderEntitlementId");

            builder.HasIndex(s => s.ReceiverEntitlementId)
                .HasDatabaseName("IX_StorePointTransfers_ReceiverEntitlementId");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(s => s.SenderEntitlement)
                .WithMany()
                .HasForeignKey(s => s.SenderEntitlementId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.ReceiverEntitlement)
                .WithMany()
                .HasForeignKey(s => s.ReceiverEntitlementId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("22T_StorePointTransfer");
        }
    }
}
