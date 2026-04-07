using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class VipBizDefConfiguration : IEntityTypeConfiguration<VipBizDef>
    {
        public void Configure(EntityTypeBuilder<VipBizDef> builder)
        {
            builder.HasKey(v => v.Id);

            builder.Property(v => v.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(v => v.BusinessId)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("BusinessID");

            builder.Property(v => v.Description)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Description");

            builder.Property(v => v.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(v => v.DesignData)
                .HasColumnType("text")
                .HasColumnName("DesignData");

            builder.Property(v => v.FinePrint)
                .HasColumnType("text")
                .HasColumnName("FinePrint");

            builder.Property(v => v.DefaultStartDay)
                .HasColumnType("integer")
                .HasColumnName("DefaultStartDay");

            builder.Property(v => v.DefaultEndDay)
                .HasColumnType("integer")
                .HasColumnName("DefaultEndDay");

            builder.Property(v => v.DefaultDailyStartHour)
                .HasColumnType("integer")
                .HasColumnName("DefaultDailyStartHour");

            builder.Property(v => v.DefaultDailyEndHour)
                .HasColumnType("integer")
                .HasColumnName("DefaultDailyEndHour");

            builder.Property(v => v.Expiration)
                .HasColumnType("timestamp with time zone")
                .HasColumnName("Expiration");

            builder.Property(v => v.AdminNote)
                .HasColumnType("text")
                .HasColumnName("AdminNote");

            builder.Property(v => v.CashierPOSMessage)
                .HasColumnType("text")
                .HasColumnName("CashierPOSMessage");

            // QRCode stores the BZ-{tableCode}-{id} code string.
            // No IsRequired — nullable to support two-step save.
            builder.Property(v => v.QRCode)
                .HasColumnType("text")
                .HasColumnName("QRCode");
            #endregion

            #region INDEXES
            // One VIP definition per business — unique index enforces this constraint.
            builder.HasIndex(v => v.BusinessId)
                .IsUnique()
                .HasDatabaseName("UX_30A_VIPBizDef_BusinessID");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(v => v.Business)
                .WithMany(b => b.VipBizDefs)
                .HasForeignKey(v => v.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("30A_VIPBizDef");
        }
    }
}
