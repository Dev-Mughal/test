using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class BizDollarCreatedChannelConfiguration : IEntityTypeConfiguration<BizDollarCreatedChannel>
    {
        public void Configure(EntityTypeBuilder<BizDollarCreatedChannel> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(b => b.ChannelCode)
                .IsRequired()
                .HasColumnType("integer")
                .HasColumnName("ChannelCD");

            builder.Property(b => b.ChannelDescription)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("ChannelDescription");

            #region SEED DATA
            builder.HasData(
                new BizDollarCreatedChannel { Id = 1, ChannelCode = 0, ChannelDescription = "New member reward" }
            );
            #endregion

            builder.ToTable("01LK1_BizDollerCreatedChannel");
        }
    }
}
