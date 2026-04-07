using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Logging.Configurations
{
    public class AppLogEntryConfiguration : IEntityTypeConfiguration<AppLogEntry>
    {
        public void Configure(EntityTypeBuilder<AppLogEntry> builder)
        {
            builder.ToTable("L01_AppLog");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedOnAdd();

            builder.Property(x => x.CreatedAtUtc)
                .HasColumnName("CreatedAtUtc")
                .HasColumnType("timestamp with time zone")
                .IsRequired();

            builder.Property(x => x.Level)
                .HasColumnName("Level")
                .HasColumnType("smallint")
                .IsRequired();

            builder.Property(x => x.Message)
                .HasColumnName("Message")
                .HasColumnType("text")
                .IsRequired();

            builder.Property(x => x.Category)
                .HasColumnName("Category")
                .HasColumnType("text");

            builder.Property(x => x.EventName)
                .HasColumnName("EventName")
                .HasColumnType("text");

            builder.Property(x => x.CorrelationId)
                .HasColumnName("CorrelationId")
                .HasColumnType("text");

            builder.Property(x => x.RequestPath)
                .HasColumnName("RequestPath")
                .HasColumnType("text");

            builder.Property(x => x.HttpMethod)
                .HasColumnName("HttpMethod")
                .HasColumnType("text");

            builder.Property(x => x.ExceptionType)
                .HasColumnName("ExceptionType")
                .HasColumnType("text");

            builder.Property(x => x.ExceptionMessage)
                .HasColumnName("ExceptionMessage")
                .HasColumnType("text");

            builder.Property(x => x.StackTrace)
                .HasColumnName("StackTrace")
                .HasColumnType("text");

            builder.Property(x => x.Metadata)
                .HasColumnName("Metadata")
                .HasColumnType("jsonb");

            builder.Property(x => x.Data)
                .HasColumnName("Data")
                .HasColumnType("jsonb");

            builder.HasIndex(x => x.CreatedAtUtc)
                .HasDatabaseName("IX_L01_AppLog_CreatedAtUtc");

            builder.HasIndex(x => new { x.Level, x.CreatedAtUtc })
                .HasDatabaseName("IX_L01_AppLog_Level_CreatedAtUtc");

            builder.HasIndex(x => x.CorrelationId)
                .HasDatabaseName("IX_L01_AppLog_CorrelationId");
        }
    }
}
