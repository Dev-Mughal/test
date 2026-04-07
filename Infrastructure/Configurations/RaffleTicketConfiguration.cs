using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RaffleTicketConfiguration : IEntityTypeConfiguration<RaffleTicket>
    {
        public void Configure(EntityTypeBuilder<RaffleTicket> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .HasColumnName("ID")
                .ValueGeneratedOnAdd();

            #region REQUIRED PROPERTIES
            builder.Property(r => r.RaffleId)
                .IsRequired()
                .HasColumnName("RaffleID");

            builder.Property(r => r.CreatedDateTime)
                .IsRequired()
                .HasColumnName("CreatedDateTime");

            builder.Property(r => r.CreationCode)
                .IsRequired()
                .HasColumnType("text")
                .HasColumnName("CreationCode");
            #endregion

            #region INDEXES
            builder.HasIndex(r => r.RaffleId)
                .HasDatabaseName("IX_41C_RaffleTicket_RaffleID");

            // Unique code per ticket — used for lottery draw selection
            builder.HasIndex(r => r.CreationCode)
                .IsUnique()
                .HasDatabaseName("UX_41C_RaffleTicket_CreationCode");
            #endregion

            #region RELATIONSHIPS
            builder.HasOne(r => r.RaffleDef)
                .WithMany(d => d.RaffleTickets)
                .HasForeignKey(r => r.RaffleId)
                .OnDelete(DeleteBehavior.Cascade);
            #endregion

            builder.ToTable("41C_RaffleTicket");
        }
    }
}
