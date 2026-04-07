using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class VipActionConfiguration : IEntityTypeConfiguration<VipAction>
    {
        public void Configure(EntityTypeBuilder<VipAction> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(v => v.EntitlementId)
                .IsRequired()
                .HasColumnName("EntitlementID");

            builder.Property(v => v.CashierId)
                .IsRequired()
                .HasColumnName("CashierID");

            builder.Property(v => v.TransactionDate)
                .IsRequired()
                .HasColumnName("TransactionDate");

            builder.Property(v => v.IsValid)
                .IsRequired()
                .HasColumnName("IsValid");
            #endregion

            #region OPTIONAL PROPERTIES
            // "TransferRecieverUserID" preserves the original Access DB column spelling.
            builder.Property(v => v.TransferReceiverUserId)
                .HasColumnName("TransferRecieverUserID");
            #endregion

            #region INDEXES
            builder.HasIndex(v => v.EntitlementId)
                .HasDatabaseName("IX_30C_VipAction_EntitlementID");

            // Supports time-range reporting on VIP access events
            builder.HasIndex(v => v.TransactionDate)
                .HasDatabaseName("IX_30C_VipAction_TransactionDate");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(v => v.VipUserEnt)
                .WithMany(e => e.VipActions)
                .HasForeignKey(v => v.EntitlementId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("30C_VipAction");
        }
    }
}
