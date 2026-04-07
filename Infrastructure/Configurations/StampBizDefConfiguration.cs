using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StampBizDefConfiguration : IEntityTypeConfiguration<StampBizDef>
    {
        public void Configure(EntityTypeBuilder<StampBizDef> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(s => s.BusinessId)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("BusinessID");

            builder.Property(s => s.RewardDesc)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("RewardDesc");

            builder.Property(s => s.StampGoal)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("StampGoal");

            builder.Property(s => s.QRCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("QRCode");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(s => s.GoalReachedMessage)
                .HasColumnType("text")
                .HasColumnName("GoalReachedMessage");

            builder.Property(s => s.FinePrint)
                .HasColumnType("text")
                .HasColumnName("FinePrint");

            builder.Property(s => s.AdminNote)
                .HasColumnType("text")
                .HasColumnName("AdminNote");

            builder.Property(s => s.CashierPOSMessage)
                .HasColumnType("text")
                .HasColumnName("CashierPOSMessage");

            builder.Property(s => s.MaxStampPerDay)
                .HasColumnName("MaxStampPerDay");

            // QRCode stores the BZ-{tableCode}-{id} code string.
            // No IsRequired — nullable to support two-step save.
            builder.Property(s => s.QRCode)
                .HasColumnType("text")
                .HasColumnName("QRCode");
            #endregion

            #region INDEXES
            builder.HasIndex(s => s.BusinessId)
                .HasDatabaseName("IX_12A_StampBizDef_BusinessID");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(s => s.Business)
                .WithMany(b => b.StampBizDefs)
                .HasForeignKey(s => s.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("12A_StampBizDef");
        }
    }
}
