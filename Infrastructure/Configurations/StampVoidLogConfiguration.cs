using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StampVoidLogConfiguration : IEntityTypeConfiguration<StampVoidLog>
    {
        public void Configure(EntityTypeBuilder<StampVoidLog> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(s => s.EntitlementId)
                .IsRequired()
                .HasColumnName("EntitlementId");

            builder.Property(s => s.Reason)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Reason");

            builder.Property(s => s.CashierId)
                .IsRequired()
                .HasColumnName("CashierID");
            #endregion

            #region INDEXES
            builder.HasIndex(s => s.EntitlementId)
                .HasDatabaseName("IX_12V_StampVoidLog_EntitlementID");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(s => s.StampUserEnt)
                .WithMany(e => e.StampVoidLogs)
                .HasForeignKey(s => s.EntitlementId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("12V_StampVoidLog");
        }
    }
}
