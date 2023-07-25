using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VocaBuddy.Domain.Entities;

namespace VocaBuddy.Infrastructure.Persistence.Configurations;

public class ForeignWordConfiguration : IEntityTypeConfiguration<ForeignWord>
{
    public void Configure(EntityTypeBuilder<ForeignWord> builder)
    {
        // Primary key
        builder.HasKey(foreignWord => foreignWord.Id);

        // Word Name property configuration
        builder.Property(foreignWord => foreignWord.Text)
            .IsRequired()
            .HasMaxLength(30)
            .IsUnicode(true);

        // NativeWordId property configuration
        builder.Property(foreignWord => foreignWord.NativeWordId)
            .IsRequired();

        // NativeWord foreign key relationship configuration
        builder
            .HasOne(foreignWord => foreignWord.NativeWord).WithMany(nativeWord => nativeWord.Translations)
            .HasForeignKey(foreignWord => foreignWord.NativeWordId).HasPrincipalKey(nativeWord => nativeWord.Id)
            .OnDelete(DeleteBehavior.Cascade); // Cascade delete, i.e. when a NativeWord is deleted, all its related ForeignWords are deleted too
    }
}
