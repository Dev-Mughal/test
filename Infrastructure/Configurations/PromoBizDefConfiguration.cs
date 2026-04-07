using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class PromoBizDefConfiguration : IEntityTypeConfiguration<PromoBizDef>
    {
        public void Configure(EntityTypeBuilder<PromoBizDef> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(p => p.BusinessId)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("BusinessID");

            builder.Property(p => p.PromotionDesc)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Promotion Desc");

            builder.Property(p => p.StartDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("StartDate");

            builder.Property(p => p.ExpirationDate)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("ExpirationDate");

            builder.Property(p => p.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(p => p.FinePrint)
                .HasColumnType("text")
                .HasColumnName("FinePrint");

            builder.Property(p => p.AdminNote)
                .HasColumnType("text")
                .HasColumnName("AdminNote");

            builder.Property(p => p.CashierPOSMessage)
                .HasColumnType("text")
                .HasColumnName("CashierPOSMessage");

            builder.Property(p => p.VoidedReason)
                .HasColumnType("text")
                .HasColumnName("VoidedReason");

            // QRCode stores the BZ-{tableCode}-{id} code string.
            // No IsRequired — nullable to support two-step save pattern.
            builder.Property(p => p.QRCode)
                .HasColumnType("text")
                .HasColumnName("QRCode");
            #endregion

            #region INDEXES
            // FK index for joining promotions to a business
            builder.HasIndex(p => p.BusinessId)
                .HasDatabaseName("IX_11A_PromoBizDef_BusinessID");

            // Date-range index for finding active promotions efficiently
            builder.HasIndex(p => new { p.StartDate, p.ExpirationDate })
                .HasDatabaseName("IX_11A_PromoBizDef_DateRange");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(p => p.Business)
                .WithMany(b => b.PromoBizDefs)
                .HasForeignKey(p => p.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("11A_PromoBizDef");
        }
    }
}
