using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StorePointActionConfiguration : IEntityTypeConfiguration<StorePointAction>
    {
        public void Configure(EntityTypeBuilder<StorePointAction> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(s => s.EntitlementId)
                .IsRequired()
                .HasColumnName("EntitlementId");

            builder.Property(s => s.PointAmount)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("PointAmount");

            builder.Property(s => s.CashierId)
                .IsRequired()
                .HasColumnName("CashierId");

            builder.Property(s => s.TransactionDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("TransactionDate");

            builder.Property(s => s.IsTransfer)
                .IsRequired()
                .HasColumnType("boolean")
                .HasColumnName("IsTransfer");
            #endregion

            #region INDEXES
            builder.HasIndex(s => s.EntitlementId)
                .HasDatabaseName("IX_StorePointActions_EntitlementId");

            builder.HasIndex(s => s.TransactionDate)
                .HasDatabaseName("IX_StorePointActions_TransactionDate");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(s => s.StorePointUserEnt)
                .WithMany(ent => ent.StorePointActions)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("StorePointActions");
        }
    }
}
