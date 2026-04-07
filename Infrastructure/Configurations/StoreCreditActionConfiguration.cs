using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StoreCreditActionConfiguration : IEntityTypeConfiguration<StoreCreditAction>
    {
        public void Configure(EntityTypeBuilder<StoreCreditAction> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(s => s.EntitlementId)
                .IsRequired()
                .HasColumnName("EntitlementId");

            builder.Property(s => s.TransAmount)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("TransAmount");

            builder.Property(s => s.CashierId)
                .IsRequired()
                .HasColumnName("CashierId");

            builder.Property(s => s.TransDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("TransDate");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(s => s.Note)
                .HasColumnType("text")
                .HasColumnName("Note");

            builder.Property(s => s.ReasonId)
                .HasColumnType("integer")
                .HasColumnName("ReasonId");
            #endregion

            #region INDEXES
            builder.HasIndex(s => s.EntitlementId)
                .HasDatabaseName("IX_StoreCreditActions_EntitlementId");

            builder.HasIndex(s => s.TransDate)
                .HasDatabaseName("IX_StoreCreditActions_TransDate");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(s => s.StoreCreditUserEnt)
                .WithMany(ent => ent.StoreCreditActions)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("StoreCreditActions");
        }
    }
}
