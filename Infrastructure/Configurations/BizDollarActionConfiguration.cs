using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BizDollarActionConfiguration : IEntityTypeConfiguration<BizDollarAction>
    {
        public void Configure(EntityTypeBuilder<BizDollarAction> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(b => b.EntitlementId)
                .IsRequired()
                .HasColumnName("EntitlementId");

            builder.Property(b => b.BusinessId)
                .IsRequired()
                .HasColumnType("int4")
                .HasColumnName("BusinessId");

            builder.Property(b => b.Amount)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("Amount");

            builder.Property(b => b.CashierId)
                .IsRequired()
                .HasColumnName("CashierId");

            builder.Property(b => b.TransactionDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("TransactionDate");

            builder.Property(b => b.UserId)
                .IsRequired()
                .HasColumnName("UserId");
            #endregion

            #region INDEXES
            builder.HasIndex(b => b.EntitlementId)
                .HasDatabaseName("IX_BizDollarActions_EntitlementId");

            builder.HasIndex(b => b.BusinessId)
                .HasDatabaseName("IX_BizDollarActions_BusinessId");

            builder.HasIndex(b => b.TransactionDate)
                .HasDatabaseName("IX_BizDollarActions_TransactionDate");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(b => b.BizDollarUserBalance)
                .WithMany(balance => balance.BizDollarActions)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(b => b.Business)
                .WithMany()
                .HasForeignKey(b => b.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("BizDollarActions");
        }
    }
}
