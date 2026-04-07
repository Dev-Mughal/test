using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RaffleWinnerConfiguration : IEntityTypeConfiguration<RaffleWinner>
    {
        public void Configure(EntityTypeBuilder<RaffleWinner> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(r => r.RaffleId)
                .IsRequired()
                .HasColumnName("RaffleID");

            builder.Property(r => r.UserId)
                .IsRequired()
                .HasColumnName("UserID");

            builder.Property(r => r.StoreCreditAmount)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnName("StoreCreditAmount");

            builder.Property(r => r.GiftCardAmount)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasColumnName("GiftCardAmount");

            builder.Property(r => r.Created)
                .IsRequired()
                .HasColumnName("Created");
            #endregion

            #region INDEXES
            builder.HasIndex(r => r.RaffleId)
                .HasDatabaseName("IX_41W_RaffleWinner_RaffleID");

            builder.HasIndex(r => r.UserId)
                .HasDatabaseName("IX_41W_RaffleWinner_UserID");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(r => r.RaffleDef)
                .WithMany(d => d.RaffleWinners)
                .HasForeignKey(r => r.RaffleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("41W_RaffleWinner");
        }
    }
}
