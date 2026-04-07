using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RaffleScheduleConfiguration : IEntityTypeConfiguration<RaffleSchedule>
    {
        public void Configure(EntityTypeBuilder<RaffleSchedule> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(r => r.RaffleId)
                .IsRequired()
                .HasColumnName("RaffleID");

            builder.Property(r => r.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");

            builder.Property(r => r.DateOfDrawing)
                .IsRequired()
                .HasColumnName("DateOfDrawing");

            builder.Property(r => r.ProcessingStartDate)
                .IsRequired()
                .HasColumnName("ProcessingStartDate");

            builder.Property(r => r.ProcessingEndDate)
                .IsRequired()
                .HasColumnName("ProcessingEndDate");
            #endregion

            #region INDEXES
            builder.HasIndex(r => r.RaffleId)
                .HasDatabaseName("IX_40B_RaffleSchedule_RaffleID");

            // Supports scheduling queries for upcoming drawings
            builder.HasIndex(r => r.DateOfDrawing)
                .HasDatabaseName("IX_40B_RaffleSchedule_DateOfDrawing");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(r => r.RaffleDef)
                .WithMany(d => d.RaffleSchedules)
                .HasForeignKey(r => r.RaffleId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("40B_RaffleSchedule");
        }
    }
}
