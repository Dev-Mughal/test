using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RaffleDefConfiguration : IEntityTypeConfiguration<RaffleDef>
    {
        public void Configure(EntityTypeBuilder<RaffleDef> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(r => r.BusinessId)
                .IsRequired()
                .HasColumnName("BusinessID");

            builder.Property(r => r.Enabled)
                .IsRequired()
                .HasColumnName("Enabled");

            builder.Property(r => r.Name)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Name");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(r => r.MinimumEntry)
                .HasColumnName("MinimumEntry");

            builder.Property(r => r.GiftCardValue)
                .HasPrecision(18, 2)
                .HasColumnName("GiftCardValue");

            builder.Property(r => r.StoreCreditValue)
                .HasPrecision(18, 2)
                .HasColumnName("StoreCreditValue");

            builder.Property(r => r.CustomPrize)
                .HasColumnType("text")
                .HasColumnName("CustomPrize");

            builder.Property(r => r.CustomPrizeValue)
                .HasPrecision(18, 2)
                .HasColumnName("CustomPrizeValue");

            builder.Property(r => r.ScheduleType)
                .HasColumnName("ScheduleType");

            // Column names preserve original Access DB naming (prefixed and spaced).
            builder.Property(r => r.DrawingDayOfWeek)
                .HasColumnName("2_Day of the week");

            builder.Property(r => r.DrawingMonthDay)
                .HasColumnName("3_DrawingMonthDay");

            builder.Property(r => r.DateOfDrawing)
                .HasColumnName("4_DateOfDrawing");

            builder.Property(r => r.DrawingTime)
                .HasColumnName("DrawingTime");

            builder.Property(r => r.LastUpdateTime)
                .HasColumnName("LastUpdateTime");

            builder.Property(r => r.TicketUsageType)
                .HasColumnName("TicketUsageType");

            builder.Property(r => r.PreviousLastDaysToUse)
                .HasColumnName("PreviousLastDaysToUse");

            // QRCode stores the BZ-{tableCode}-{id} code string.
            // No IsRequired — nullable to support two-step save.
            builder.Property(r => r.QRCode)
                .HasColumnType("text")
                .HasColumnName("QRCode");
            #endregion

            #region INDEXES
            builder.HasIndex(r => r.BusinessId)
                .HasDatabaseName("IX_40A_RaffleDef_BusinessID");

            // Quickly find all active raffles across businesses
            builder.HasIndex(r => r.Enabled)
                .HasDatabaseName("IX_40A_RaffleDef_Enabled");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(r => r.Business)
                .WithMany(b => b.RaffleDefs)
                .HasForeignKey(r => r.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("40A_RaffleDef");
        }
    }
}
