using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BusinessUserConfiguration : IEntityTypeConfiguration<BusinessUser>
    {
        public void Configure(EntityTypeBuilder<BusinessUser> builder)
        {
            builder.Property(bu => bu.Id)
                .HasColumnName("UserID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(bu => bu.FirstName)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("FirstName");

            builder.Property(bu => bu.LastName)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("LastName");

            builder.Property(bu => bu.CreatedOn)
                .IsRequired()
                .HasColumnName("CreatedOn");

            builder.Property(bu => bu.TimeZone)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("TimeZone");

            builder.Property(bu => bu.RefreshToken)
                .HasColumnType("text")
                .HasColumnName("RefreshToken");

            builder.Property(bu => bu.RefreshTokenExpiryTime)
                .HasColumnName("RefreshTokenExpiryTime");
            #endregion

            #region INDEXES
            builder.HasIndex(bu => bu.Email)
                .HasDatabaseName("IX_BusinessUser_Email");
            #endregion

            #region RELATIONSHIPS
            builder.HasMany(bu => bu.BusinessUserBusinesses)
                .WithOne(bub => bub.BusinessUser)
                .HasForeignKey(bub => bub.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("B02_Business_User");
        }
    }
}

