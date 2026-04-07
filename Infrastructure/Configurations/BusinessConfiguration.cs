using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BusinessConfiguration : IEntityTypeConfiguration<Business>
    {
        public void Configure(EntityTypeBuilder<Business> builder)
        {
            builder.HasKey(b => b.BusinessId);

            builder.Property(b => b.BusinessId)
                .HasColumnName("BusinessID")
                .HasColumnType("int4")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(b => b.BusinessName)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("BusinessName");
            builder.Property(b => b.BusinessEmail)
                .HasColumnType("text")
                .HasColumnName("BusinessEmail");
            builder.Property(b => b.BusinessURL)
                .HasColumnType("text")
                .HasColumnName("BusinessURL");
            builder.Property(b => b.BusinessPhone)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("BusinessPhone");
            builder.Property(b => b.CountryCode)
                .IsRequired()
                .HasColumnName("CountryCode");
            builder.Property(b => b.StreetAddress)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("StreetAddress");
            builder.Property(b => b.AddressLine2)
                .HasColumnType("text")
                .HasColumnName("AddressLine2");
            builder.Property(b => b.Country)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Country");
            builder.Property(b => b.Longitude)
                .IsRequired()
                .HasColumnName("Longitude");
            builder.Property(b => b.Latitude)
                .IsRequired()
                .HasColumnName("Latitude");
            builder.Property(b => b.CreatedOn)
                .IsRequired()
                .HasColumnName("CreatedOn");
            builder.Property(b => b.CategoryId)
                .IsRequired()
                .HasColumnName("CategoryID");
            // Geo lookup FKs — stored as IDs to allow index-only city/zip searches
            // on B01 without a JOIN (per geo-search Jira).
            builder.Property(b => b.StateCityId)
                .IsRequired()
                .HasColumnName("State_City_ID");
            builder.Property(b => b.StateCityZipId)
                .IsRequired()
                .HasColumnName("State_City_Zip_ID");
            builder.Property(b => b.BusinessImageUrl)
                .HasColumnType("text")
                .HasColumnName("BusinessImageUrl");
            #endregion

            #region INDEXES
            builder.HasIndex(b => b.BusinessEmail)
                .HasDatabaseName("IX_Business_BusinessEmail");
            builder.HasIndex(b => b.BusinessPhone)
                .HasDatabaseName("IX_Business_BusinessPhone");
            builder.HasIndex(b => b.BusinessName)
                .HasDatabaseName("IX_Business_BusinessName");
            // StateCityId index enables O(1) city/state search on B01 (no JOIN needed)
            builder.HasIndex(b => b.StateCityId)
                .HasDatabaseName("IX_Business_StateCityID");
            // StateCityZipId index enables O(1) zip-code search on B01 (no JOIN needed)
            builder.HasIndex(b => b.StateCityZipId)
                .HasDatabaseName("IX_Business_StateCityZipID");
            // Composite index covering GetBusinessesByCategory with descending date ordering
            builder.HasIndex(b => new { b.CategoryId, b.CreatedOn })
                .IsDescending(false, true)
                .HasDatabaseName("IX_Business_CategoryID_CreatedOn");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(b => b.Category)
                .WithMany(c => c.Businesses)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(b => b.Coupons)
                .WithOne(c => c.Business)
                .HasForeignKey(c => c.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(b => b.BusinessUserBusinesses)
                .WithOne(bub => bub.Business)
                .HasForeignKey(bub => bub.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            // GeoCity (L50) — one city/state record can be shared by many businesses
            builder.HasOne(b => b.GeoCity)
                .WithMany(g => g.Businesses)
                .HasForeignKey(b => b.StateCityId)
                .OnDelete(DeleteBehavior.Restrict);

            // GeoZipCode (L51) — one zip record can be shared by many businesses
            builder.HasOne(b => b.GeoZipCode)
                .WithMany(g => g.Businesses)
                .HasForeignKey(b => b.StateCityZipId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            builder.ToTable("B01_Business_Profile");
        }
    }
}

