using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace VocaBuddy.Infrastructure.Persistence.Configurations;

public class NativeWordConfiguration : IEntityTypeConfiguration<NativeWord>
{
    public void Configure(EntityTypeBuilder<NativeWord> builder)
    {
        // Primary key
        builder.HasKey(newWord => newWord.Id);

        // Text property configuration
        builder.Property(newWord => newWord.Text)
            .IsRequired()
            .HasMaxLength(ValidationConstants.MaxWordLength)
            .IsUnicode(true);

        // Unique constraint on Name property
        builder.HasIndex(nw => new { nw.UserId, nw.Text })
            .IsUnique()
            .IsClustered(false);

        // UserId property configuration
        builder.Property(newWord => newWord.UserId)
            .IsRequired()
            .HasMaxLength(36)
            .IsUnicode(false);

        // Non-clustered index on CreatedUtc property
        builder.HasIndex(newWord => newWord.CreatedUtc)
            .IsUnique(false)
            .IsClustered(false);

        // CreatedUtc property configuration
        builder.Property(newWord => newWord.CreatedUtc)
            .IsRequired();

        // Non-clustered index on UpdatedUtc property
        builder.HasIndex(newWord => newWord.UpdatedUtc)
            .IsUnique(false)
            .IsClustered(false);

        // UpdatedUtc property configuration
        builder.Property(newWord => newWord.UpdatedUtc)
            .IsRequired();
    }
}
