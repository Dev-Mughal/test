using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StampActionConfiguration : IEntityTypeConfiguration<StampAction>
    {
        public void Configure(EntityTypeBuilder<StampAction> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(s => s.EntitlementId)
                .IsRequired()
                .HasColumnName("EntitlementID");

            builder.Property(s => s.CashierId)
                .IsRequired()
                .HasColumnName("CashierID");

            builder.Property(s => s.TransactionDate)
                .IsRequired()
                .HasColumnName("TransactionDate");

            builder.Property(s => s.IsVoided)
                .IsRequired()
                .HasColumnName("IsVoided");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(s => s.Note)
                .HasColumnType("text")
                .HasColumnName("Note");
            #endregion

            #region INDEXES
            builder.HasIndex(s => s.EntitlementId)
                .HasDatabaseName("IX_12C_StampAction_EntitlementID");

            // Supports time-range reporting on stamp transactions
            builder.HasIndex(s => s.TransactionDate)
                .HasDatabaseName("IX_12C_StampAction_TransactionDate");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(s => s.StampUserEnt)
                .WithMany(e => e.StampActions)
                .HasForeignKey(s => s.EntitlementId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("12C_StampAction");
        }
    }
}
