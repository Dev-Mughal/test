using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BizDollarUserBalanceConfiguration : IEntityTypeConfiguration<BizDollarUserBalance>
    {
        public void Configure(EntityTypeBuilder<BizDollarUserBalance> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(b => b.UserId)
                .IsRequired()
                .HasColumnName("UserId");

            builder.Property(b => b.Balance)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("Balance");

            builder.Property(b => b.CreatedChannel)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("CreatedChannel");

            builder.Property(b => b.LastUpdated)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("LastUpdated");

            builder.Property(b => b.Created)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("Created");
            #endregion

            #region INDEXES
            builder.HasIndex(b => b.UserId)
                .HasDatabaseName("IX_BizDollarUserBalances_UserId");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(b => b.Customer)
                .WithMany()
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.BizDollarActions)
                .WithOne(a => a.BizDollarUserBalance)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("BizDollarUserBalances");
        }
    }
}
