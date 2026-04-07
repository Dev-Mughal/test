using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StateConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            // Two-character postal code — unique and indexed for O(1) lookup on every signup validation
            builder.Property(s => s.Code)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Code")
                .HasMaxLength(2);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Name");

            // Region allows UI to group states (e.g., "West Coast", "Midwest")
            builder.Property(s => s.Region)
                .HasColumnType("text")
                .HasColumnName("Region");

            builder.Property(s => s.IsActive)
                .IsRequired()
                .HasDefaultValue(true)
                .HasColumnName("IsActive");

            builder.Property(s => s.CreatedOn)
                .IsRequired()
                .HasColumnName("CreatedOn");

            // Unique index on Code for fast lookups during GeoCity/GeoZipCode validation
            builder.HasIndex(s => s.Code)
                .IsUnique()
                .HasDatabaseName("IX_L52_Code");

            // Index on IsActive so we can quickly filter to active states in the dropdown
            builder.HasIndex(s => s.IsActive)
                .HasDatabaseName("IX_L52_IsActive");

            builder.ToTable("L52_Geo_States");

            #region SEEDING DATA
            var seedDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc);

            builder.HasData(
            new State { Id = 1, Code = "AL", Name = "Alabama", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 2, Code = "AK", Name = "Alaska", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 3, Code = "AZ", Name = "Arizona", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 4, Code = "AR", Name = "Arkansas", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 5, Code = "CA", Name = "California", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 6, Code = "CO", Name = "Colorado", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 7, Code = "CT", Name = "Connecticut", Region = "Northeast", IsActive = true, CreatedOn = seedDate },
            new State { Id = 8, Code = "DE", Name = "Delaware", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 9, Code = "FL", Name = "Florida", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 10, Code = "GA", Name = "Georgia", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 11, Code = "HI", Name = "Hawaii", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 12, Code = "ID", Name = "Idaho", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 13, Code = "IL", Name = "Illinois", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 14, Code = "IN", Name = "Indiana", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 15, Code = "IA", Name = "Iowa", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 16, Code = "KS", Name = "Kansas", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 17, Code = "KY", Name = "Kentucky", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 18, Code = "LA", Name = "Louisiana", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 19, Code = "ME", Name = "Maine", Region = "Northeast", IsActive = true, CreatedOn = seedDate },
            new State { Id = 20, Code = "MD", Name = "Maryland", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 21, Code = "MA", Name = "Massachusetts", Region = "Northeast", IsActive = true, CreatedOn = seedDate },
            new State { Id = 22, Code = "MI", Name = "Michigan", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 23, Code = "MN", Name = "Minnesota", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 24, Code = "MS", Name = "Mississippi", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 25, Code = "MO", Name = "Missouri", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 26, Code = "MT", Name = "Montana", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 27, Code = "NE", Name = "Nebraska", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 28, Code = "NV", Name = "Nevada", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 29, Code = "NH", Name = "New Hampshire", Region = "Northeast", IsActive = true, CreatedOn = seedDate },
            new State { Id = 30, Code = "NJ", Name = "New Jersey", Region = "Northeast", IsActive = true, CreatedOn = seedDate },
            new State { Id = 31, Code = "NM", Name = "New Mexico", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 32, Code = "NY", Name = "New York", Region = "Northeast", IsActive = true, CreatedOn = seedDate },
            new State { Id = 33, Code = "NC", Name = "North Carolina", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 34, Code = "ND", Name = "North Dakota", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 35, Code = "OH", Name = "Ohio", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 36, Code = "OK", Name = "Oklahoma", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 37, Code = "OR", Name = "Oregon", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 38, Code = "PA", Name = "Pennsylvania", Region = "Northeast", IsActive = true, CreatedOn = seedDate },
            new State { Id = 39, Code = "RI", Name = "Rhode Island", Region = "Northeast", IsActive = true, CreatedOn = seedDate },
            new State { Id = 40, Code = "SC", Name = "South Carolina", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 41, Code = "SD", Name = "South Dakota", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 42, Code = "TN", Name = "Tennessee", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 43, Code = "TX", Name = "Texas", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 44, Code = "UT", Name = "Utah", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 45, Code = "VT", Name = "Vermont", Region = "Northeast", IsActive = true, CreatedOn = seedDate },
            new State { Id = 46, Code = "VA", Name = "Virginia", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 47, Code = "WA", Name = "Washington", Region = "West", IsActive = true, CreatedOn = seedDate },
            new State { Id = 48, Code = "WV", Name = "West Virginia", Region = "South", IsActive = true, CreatedOn = seedDate },
            new State { Id = 49, Code = "WI", Name = "Wisconsin", Region = "Midwest", IsActive = true, CreatedOn = seedDate },
            new State { Id = 50, Code = "WY", Name = "Wyoming", Region = "West", IsActive = true, CreatedOn = seedDate }
            );
            #endregion
        }
    }
}
