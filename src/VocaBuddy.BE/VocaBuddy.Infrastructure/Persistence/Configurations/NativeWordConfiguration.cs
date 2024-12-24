namespace VocaBuddy.Infrastructure.Persistence.Configurations;

public class NativeWordConfiguration : IEntityTypeConfiguration<NativeWord>
{
    public void Configure(EntityTypeBuilder<NativeWord> builder)
    {
        // Primary key
        builder.HasKey(nativeWord => nativeWord.Id);

        // Text property configuration
        builder.Property(nativeWord => nativeWord.Text)
            .IsRequired()
            .HasMaxLength(ValidationConstants.MaxWordLength)
            .IsUnicode(true);

        // Unique constraint on Name property
        builder.HasIndex(nativeWord => new { nativeWord.UserId, nativeWord.Text })
            .IsUnique()
            .IsClustered(false);

        // UserId property configuration
        builder.Property(nativeWord => nativeWord.UserId)
            .IsRequired()
            .HasMaxLength(36)
            .IsUnicode(false);
        
        // MistakeCount property configuration
        builder.ToTable(t =>
        {
            t.HasCheckConstraint("CK_NativeWord_MistakeCount_NonNegative", "[MistakeCount] >= 0");
        });

        // Non-clustered index on CreatedUtc property
        builder.HasIndex(nativeWord => nativeWord.CreatedUtc)
            .IsUnique(false)
            .IsClustered(false);

        // CreatedUtc property configuration
        builder.Property(nativeWord => nativeWord.CreatedUtc)
            .IsRequired();

        // Non-clustered index on UpdatedUtc property
        builder.HasIndex(nativeWord => nativeWord.UpdatedUtc)
            .IsUnique(false)
            .IsClustered(false);

        // UpdatedUtc property configuration
        builder.Property(nativeWord => nativeWord.UpdatedUtc)
            .IsRequired();
    }
}
