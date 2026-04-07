using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CouponTypeConfiguration : IEntityTypeConfiguration<IncentiveType>
    {
        public void Configure(EntityTypeBuilder<IncentiveType> builder)
        {
            builder.HasKey(ct => ct.Id);

            builder.Property(ct => ct.Id)
                .HasColumnName("CouponTypeID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(ct => ct.TypeDescription)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("TypeDescription");

            builder.Property(ct => ct.TypeCode)
                .IsRequired()
                .HasColumnType("character(1)")
                .HasColumnName("TypeCode");

            builder.Property(ct => ct.IsActive)
                .IsRequired()
                .HasColumnName("IsActive");

            builder.Property(ct => ct.CreatedOn)
                .IsRequired()
                .HasColumnName("CreatedOn");
            #endregion

            #region INDEXES
            builder.HasIndex(ct => ct.TypeCode)
                .IsUnique()
                .HasDatabaseName("IX_CouponType_TypeCode");
            builder.HasIndex(ct => ct.IsActive)
                .HasDatabaseName("IX_CouponType_IsActive");
            #endregion

            builder.ToTable("B05_Coupon_Type");

            #region SEEDING DATA
            var seedDate = new DateTime(2026, 7, 11, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new IncentiveType { Id = 1, TypeDescription = "BizyPop Dollars", TypeCode = "B", IsActive = true, CreatedOn = seedDate },
                new IncentiveType { Id = 2, TypeDescription = "Coupon",          TypeCode = "C", IsActive = true, CreatedOn = seedDate },
                new IncentiveType { Id = 3, TypeDescription = "Stamp",           TypeCode = "S", IsActive = true, CreatedOn = seedDate },
                new IncentiveType { Id = 4, TypeDescription = "Promotions",      TypeCode = "M", IsActive = true, CreatedOn = seedDate },
                new IncentiveType { Id = 5, TypeDescription = "Gift Card",       TypeCode = "G", IsActive = true, CreatedOn = seedDate },
                new IncentiveType { Id = 6, TypeDescription = "Store Credit",    TypeCode = "R", IsActive = true, CreatedOn = seedDate },
                new IncentiveType { Id = 7, TypeDescription = "Store Point",     TypeCode = "P", IsActive = true, CreatedOn = seedDate },
                new IncentiveType { Id = 8, TypeDescription = "VIP Access",      TypeCode = "V", IsActive = true, CreatedOn = seedDate },
                new IncentiveType { Id = 9, TypeDescription = "Check-In",        TypeCode = "I", IsActive = true, CreatedOn = seedDate }
            );
            #endregion
        }
    }
}
