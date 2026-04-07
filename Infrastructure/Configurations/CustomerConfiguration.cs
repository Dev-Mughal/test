using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.CustomerId)
                .HasColumnName("CustomerID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(c => c.FirstName)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("FirstName");

            builder.Property(c => c.LastName)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("LastName");

            builder.Property(c => c.Email)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Email");

            builder.Property(c => c.PasswordHash)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("PasswordHash");

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("IsActive");

            builder.Property(c => c.TimeZone)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("TimeZone");

            builder.Property(c => c.CreatedAt)
                .IsRequired()
                .HasColumnName("CreatedAt");

            builder.Property(c => c.UpdatedAt)
                .IsRequired()
                .HasColumnName("UpdatedAt");
            #endregion

            #region OPTIONAL PROPERTIES
            builder.Property(c => c.ProfilePhotoUrl)
                .HasColumnType("text")
                .HasColumnName("ProfilePhotoUrl");

            builder.Property(c => c.RefreshToken)
                .HasColumnType("text")
                .HasColumnName("RefreshToken");

            builder.Property(c => c.RefreshTokenExpiryTime)
                .HasColumnName("RefreshTokenExpiryTime");
            #endregion

            #region INDEXES
            builder.HasIndex(c => c.Email)
                .IsUnique()
                .HasDatabaseName("UX_Customer_Email");

            builder.HasIndex(c => c.IsActive)
                .HasDatabaseName("IX_Customer_IsActive");

            builder.HasIndex(c => c.CreatedAt)
                .HasDatabaseName("IX_Customer_CreatedAt");
            #endregion

            builder.ToTable("C01_Customer");
        }
    }
}
