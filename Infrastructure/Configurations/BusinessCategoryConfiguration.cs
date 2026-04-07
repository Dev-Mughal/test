using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BusinessCategoryConfiguration : IEntityTypeConfiguration<BusinessCategory>
    {
        public void Configure(EntityTypeBuilder<BusinessCategory> builder)
        {
            builder.HasKey(bc => bc.CategoryId);

            builder.Property(bc => bc.CategoryId)
                .HasColumnName("CategoryID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(bc => bc.CategoryName)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("CategoryName");
            builder.Property(bc => bc.CreatedOn)
                .IsRequired()
                .HasColumnName("CreatedOn");
            builder.Property(bc => bc.CategorySlug)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("CategorySlug");
            builder.Property(bc => bc.IsActive)
                .IsRequired()
                .HasColumnName("IsActive");
            builder.Property(bc => bc.DisplayOrder)
                .IsRequired()
                .HasColumnName("DisplayOrder");
            builder.Property(bc => bc.DisplayColumn)
                .IsRequired()
                .HasColumnName("DisplayColumn");
            #endregion

            #region INDEXES
            builder.HasIndex(bc => bc.CategoryName)
                .IsUnique()
                .HasDatabaseName("IX_BusinessCategory_CategoryName");

            // Composite index covering the grouped active-category lookup query
            // (WHERE IsActive=true ORDER BY DisplayColumn, DisplayOrder)
            builder.HasIndex(bc => new { bc.IsActive, bc.DisplayColumn, bc.DisplayOrder })
                .HasDatabaseName("IX_BusinessCategory_IsActive_DisplayColumn_DisplayOrder");
            #endregion

            #region RELATIONSHIPS
            builder.HasMany(bc => bc.Businesses)
                .WithOne(b => b.Category)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            builder.ToTable("B04_Business_Category");

            #region SEEDING DATA
            var seedDate = new DateTime(2026, 7, 11, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
                new BusinessCategory { CategoryId = 1,  CategoryName = "Restaurant & Bar",              CategorySlug = "restaurant-bar",                IsActive = true, DisplayColumn = 1, DisplayOrder = 1, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 2,  CategoryName = "Desert and Drinks",             CategorySlug = "desert-and-drinks",             IsActive = true, DisplayColumn = 1, DisplayOrder = 2, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 3,  CategoryName = "Entertainment",                 CategorySlug = "entertainment",                 IsActive = true, DisplayColumn = 1, DisplayOrder = 3, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 4,  CategoryName = "Beauty and Health",             CategorySlug = "beauty-and-health",             IsActive = true, DisplayColumn = 1, DisplayOrder = 4, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 5,  CategoryName = "Hotel & Travel",                CategorySlug = "hotel-travel",                  IsActive = true, DisplayColumn = 1, DisplayOrder = 5, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 6,  CategoryName = "For Kids",                      CategorySlug = "for-kids",                      IsActive = true, DisplayColumn = 1, DisplayOrder = 6, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 7,  CategoryName = "For Pets",                      CategorySlug = "for-pets",                      IsActive = true, DisplayColumn = 1, DisplayOrder = 7, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 16, CategoryName = "Everything Else",               CategorySlug = "everything-else",               IsActive = true, DisplayColumn = 1, DisplayOrder = 8, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 8,  CategoryName = "Automotive Services",           CategorySlug = "automotive-services",           IsActive = true, DisplayColumn = 2, DisplayOrder = 1, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 9,  CategoryName = "Cleaning",                      CategorySlug = "cleaning",                      IsActive = true, DisplayColumn = 2, DisplayOrder = 2, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 10, CategoryName = "Home Services",                 CategorySlug = "home-services",                 IsActive = true, DisplayColumn = 2, DisplayOrder = 3, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 11, CategoryName = "Gym & Fitness",                 CategorySlug = "gym-fitness",                   IsActive = true, DisplayColumn = 2, DisplayOrder = 4, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 12, CategoryName = "Real Estate",                   CategorySlug = "real-estate",                   IsActive = true, DisplayColumn = 2, DisplayOrder = 5, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 13, CategoryName = "Legal",                         CategorySlug = "legal",                         IsActive = true, DisplayColumn = 2, DisplayOrder = 6, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 14, CategoryName = "Medical Services",              CategorySlug = "medical-services",              IsActive = true, DisplayColumn = 2, DisplayOrder = 7, CreatedOn = seedDate },
                new BusinessCategory { CategoryId = 15, CategoryName = "Professional Services (other)", CategorySlug = "professional-services-other",   IsActive = true, DisplayColumn = 2, DisplayOrder = 8, CreatedOn = seedDate }
            );
            #endregion
        }
    }
}
