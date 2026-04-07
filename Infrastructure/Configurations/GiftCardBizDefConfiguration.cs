using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class GiftCardBizDefConfiguration : IEntityTypeConfiguration<GiftCardBizDef>
    {
        public void Configure(EntityTypeBuilder<GiftCardBizDef> builder)
        {
            builder.HasKey(g => g.Id);

            builder.Property(g => g.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(g => g.BusinessId)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("BusinessID");

            builder.Property(g => g.Title)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Title");

            builder.Property(g => g.Status)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("Status");

            builder.Property(g => g.GiftCardValue)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("GiftCardValue");

            builder.Property(g => g.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(g => g.MarketingText)
                .HasColumnType("text")
                .HasColumnName("MarketingText");

            builder.Property(g => g.FinePrint)
                .HasColumnType("text")
                .HasColumnName("FinePrint");

            builder.Property(g => g.Expiration)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("Expiration");

            builder.Property(g => g.AdminNote)
                .HasColumnType("text")
                .HasColumnName("AdminNote");

            builder.Property(g => g.CashierPOSMessage)
                .HasColumnType("text")
                .HasColumnName("CashierPOSMessage");

            builder.Property(g => g.StatusDate)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("StatusDate");

            builder.Property(g => g.StatusNote)
                .HasColumnType("text")
                .HasColumnName("StatusNote");

            // QRCode stores the BZ-{tableCode}-{id} code string.
            // No IsRequired — nullable to support two-step save.
            builder.Property(g => g.QRCode)
                .HasColumnType("text")
                .HasColumnName("QRCode");
            #endregion

            #region INDEXES
            builder.HasIndex(g => g.BusinessId)
                .HasDatabaseName("IX_20A_GiftCardBizDef_BusinessID");

            // Supports filtering active/available gift card programs by status
            builder.HasIndex(g => g.Status)
                .HasDatabaseName("IX_20A_GiftCardBizDef_Status");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(g => g.Business)
                .WithMany(b => b.GiftCardBizDefs)
                .HasForeignKey(g => g.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("20A_GiftCardBizDef");
        }
    }
}
