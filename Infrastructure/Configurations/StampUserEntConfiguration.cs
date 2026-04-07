using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StampUserEntConfiguration : IEntityTypeConfiguration<StampUserEnt>
    {
        public void Configure(EntityTypeBuilder<StampUserEnt> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(s => s.Status)
                .IsRequired()
                .HasColumnName("Status");

            builder.Property(s => s.UserId)
                .IsRequired()
                .HasColumnName("UserID");

            builder.Property(s => s.StampId)
                .IsRequired()
                .HasColumnName("StampID");

            builder.Property(s => s.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");

            builder.Property(s => s.StampCount)
                .IsRequired()
                .HasColumnName("StampCount");

            builder.Property(s => s.StampGoal)
                .IsRequired()
                .HasColumnName("StampGoal");

            builder.Property(s => s.LastUpdated)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("LastUpdated");

            builder.Property(s => s.Created)
                .IsRequired()
                .HasColumnType("timestamp with time zone")
                .HasColumnName("Created");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(s => s.RedeemedDate)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("RedeemedDate");

            builder.Property(s => s.CashierNote)
                .HasColumnType("text")
                .HasColumnName("CashierNote");
            #endregion

            #region INDEXES
            // Composite unique — one stamp card per customer per stamp program
            builder.HasIndex(s => new { s.UserId, s.StampId })
                .IsUnique()
                .HasDatabaseName("UX_12B_StampUserEnt_UserID_StampID");

            // Supports filtering by status (e.g. ReadyToUse cards)
            builder.HasIndex(s => s.Status)
                .HasDatabaseName("IX_12B_StampUserEnt_Status");

            builder.HasIndex(s => s.StampId)
                .HasDatabaseName("IX_12B_StampUserEnt_StampID");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(s => s.Customer)
                .WithMany(c => c.StampUserEnts)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(s => s.StampBizDef)
                .WithMany(d => d.StampUserEnts)
                .HasForeignKey(s => s.StampId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("12B_StampUserEnt");
        }
    }
}
