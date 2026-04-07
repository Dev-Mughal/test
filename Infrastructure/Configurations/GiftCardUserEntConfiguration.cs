using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class GiftCardUserEntConfiguration : IEntityTypeConfiguration<GiftCardUserEnt>
    {
        public void Configure(EntityTypeBuilder<GiftCardUserEnt> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(g => g.UserId)
                .IsRequired()
                .HasColumnName("UserID");

            builder.Property(g => g.GiftCardId)
                .IsRequired()
                .HasColumnName("GiftCardID");

            builder.Property(g => g.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");

            builder.Property(g => g.GiftCardBalance)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("GiftCardBalance");

            builder.Property(g => g.GiftCardValue)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("GiftCardValue");

            builder.Property(g => g.LastUpdated)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("LastUpdated");

            builder.Property(g => g.Created)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("Created");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(g => g.CashierNote)
                .HasColumnType("text")
                .HasColumnName("CashierNote");
            #endregion

            #region INDEXES
            builder.HasIndex(g => g.UserId)
                .HasDatabaseName("IX_20B_GiftCardUserEnt_UserID");

            builder.HasIndex(g => g.GiftCardId)
                .HasDatabaseName("IX_20B_GiftCardUserEnt_GiftCardID");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(g => g.Customer)
                .WithMany()
                .HasForeignKey(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(g => g.GiftCardBizDef)
                .WithMany(d => d.GiftCardUserEnts)
                .HasForeignKey(g => g.GiftCardId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("20B_GiftCardUserEnt");
        }
    }
}
