using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class VipUserEntConfiguration : IEntityTypeConfiguration<VipUserEnt>
    {
        public void Configure(EntityTypeBuilder<VipUserEnt> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(v => v.UserId)
                .IsRequired()
                .HasColumnName("UserID");

            builder.Property(v => v.BusinessId)
                .IsRequired()
                .HasColumnName("BusinessID");

            builder.Property(v => v.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");

            builder.Property(v => v.Status)
                .IsRequired()
                .HasColumnName("Status");

            builder.Property(v => v.LastUpdated)
                .IsRequired()
                .HasColumnName("LastUpdated");

            builder.Property(v => v.Created)
                .IsRequired()
                .HasColumnName("Created");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(v => v.StatusDate)
                .HasColumnName("StatusDate");

            builder.Property(v => v.StatusNote)
                .HasColumnType("text")
                .HasColumnName("StatusNote");

            builder.Property(v => v.CashierNote)
                .HasColumnType("text")
                .HasColumnName("CashierNote");

            builder.Property(v => v.StartDay)
                .HasColumnName("StartDay");

            builder.Property(v => v.EndDay)
                .HasColumnName("EndDay");

            builder.Property(v => v.DailyStartHour)
                .HasColumnName("DailyStartHour");

            builder.Property(v => v.DailyEndHour)
                .HasColumnName("DailyEndHour");
            #endregion

            #region INDEXES
            // Composite unique — one VIP membership per customer per business
            builder.HasIndex(v => new { v.UserId, v.BusinessId })
                .IsUnique()
                .HasDatabaseName("UX_30B_VIPUserEnt_UserID_BusinessID");

            // Supports filtering active VIP members by status
            builder.HasIndex(v => v.Status)
                .HasDatabaseName("IX_30B_VIPUserEnt_Status");

            builder.HasIndex(v => v.BusinessId)
                .HasDatabaseName("IX_30B_VIPUserEnt_BusinessID");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(v => v.Customer)
                .WithMany()
                .HasForeignKey(v => v.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(v => v.Business)
                .WithMany()
                .HasForeignKey(v => v.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("30B_VIPUserEnt");
        }
    }
}
