using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class GiftCardActionConfiguration : IEntityTypeConfiguration<GiftCardAction>
    {
        public void Configure(EntityTypeBuilder<GiftCardAction> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(g => g.EntitlementId)
                .IsRequired()
                .HasColumnName("EntitlementID");

            builder.Property(g => g.Amount)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("Amount");

            builder.Property(g => g.CashierId)
                .IsRequired()
                .HasColumnName("CashierID");

            builder.Property(g => g.TransactionDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("TransactionDate");

            builder.Property(g => g.UserId)
                .IsRequired()
                .HasColumnName("UserID");
            #endregion

            #region OPTIONAL PROPERTIES
            // "TransferRecieverUserID" preserves the original Access DB column spelling.
            builder.Property(g => g.TransferReceiverUserId)
                .HasColumnName("TransferRecieverUserID");
            #endregion

            #region INDEXES
            builder.HasIndex(g => g.EntitlementId)
                .HasDatabaseName("IX_20C_GiftCardAction_EntitlementID");

            // Date-range reporting on gift card transactions
            builder.HasIndex(g => g.TransactionDate)
                .HasDatabaseName("IX_20C_GiftCardAction_TransactionDate");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(g => g.GiftCardUserEnt)
                .WithMany(e => e.GiftCardActions)
                .HasForeignKey(g => g.EntitlementId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("20C_GiftCardAction");
        }
    }
}
