using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class StoreCreditReasonConfiguration : IEntityTypeConfiguration<StoreCreditReason>
    {
        public void Configure(EntityTypeBuilder<StoreCreditReason> builder)
        {
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(s => s.ReasonDescription)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("Reason Description");

            #region SEED DATA
            builder.HasData(
                new StoreCreditReason { Id = 1, ReasonDescription = "Customer Service Issue" },
                new StoreCreditReason { Id = 2, ReasonDescription = "Quality Issue" },
                new StoreCreditReason { Id = 3, ReasonDescription = "Friend" },
                new StoreCreditReason { Id = 4, ReasonDescription = "Family" }
            );
            #endregion

            builder.ToTable("21LK1_StoreCreditReason");
        }
    }
}
