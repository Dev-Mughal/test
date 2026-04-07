using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BusinessUserBusinessConfiguration : IEntityTypeConfiguration<BusinessUserBusiness>
    {
        public void Configure(EntityTypeBuilder<BusinessUserBusiness> builder)
        {
            builder.HasKey(bub => bub.Id);

            builder.Property(bub => bub.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            builder.Property(bub => bub.BusinessId)
                .IsRequired()
                .HasColumnName("BusinessID");

            builder.Property(bub => bub.UserId)
                .IsRequired()
                .HasColumnName("UserID");

            builder.Property(bub => bub.IsDefault)
                .HasColumnName("IsDefault");

            builder.HasIndex(bub => new { bub.BusinessId, bub.UserId })
                .IsUnique()
                .HasDatabaseName("UX_B03_BusinessUserLink_BusinessID_UserID");

            builder.HasIndex(bub => new { bub.UserId, bub.IsDefault })
                .HasDatabaseName("IX_B03_BusinessUserLink_UserID_IsDefault");

            builder.HasOne(bub => bub.Business)
                .WithMany(b => b.BusinessUserBusinesses)
                .HasForeignKey(bub => bub.BusinessId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(bub => bub.BusinessUser)
                .WithMany(bu => bu.BusinessUserBusinesses)
                .HasForeignKey(bub => bub.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("B03_BusinessUserLink");
        }
    }
}
