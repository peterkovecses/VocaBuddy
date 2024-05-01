using FluentValidation;

namespace VocaBuddy.UI.Validation;

public class NativeWordCreateUpdateModelValidator : AbstractValidator<CompactNativeWordDto>
{
    private readonly string AboveMaxLengthMessage = $"The number of characters in the word must not exceed {Constants.MaxWordLength}";

    public NativeWordCreateUpdateModelValidator()
    {
        RuleFor(word => word.Text)
            .NotEmpty()
            .WithMessage("The field is required");
        
        RuleFor(word => word.Text)
            .MaximumLength(Constants.MaxWordLength)
            .WithMessage(AboveMaxLengthMessage);
        
        RuleForEach(word => word.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text)
                .NotEmpty()
                .WithMessage("The field is required");

            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text)
                .MaximumLength(Constants.MaxWordLength)
                .WithMessage(AboveMaxLengthMessage);
        });

        RuleFor(word => word.Translations)
            .Must(translations => translations.Select(translation => translation.Text).AllUnique())
            .WithMessage("Translations must be unique.");
    }
}
