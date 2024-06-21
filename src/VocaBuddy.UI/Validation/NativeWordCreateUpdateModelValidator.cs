using FluentValidation;

namespace VocaBuddy.UI.Validation;

public class NativeWordCreateUpdateModelValidator : AbstractValidator<CompactNativeWordDto>
{
    public NativeWordCreateUpdateModelValidator()
    {
        RuleFor(word => word.Text)
            .NotEmpty()
            .WithMessage("The field is required");
        
        RuleFor(word => word.Text)
            .MaximumLength(ValidationConstants.MaxWordLength)
            .WithMessage(ValidationConstants.AboveMaxLengthMessage);
        
        RuleForEach(word => word.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text)
                .NotEmpty()
                .WithMessage("The field is required");

            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text)
                .MaximumLength(ValidationConstants.MaxWordLength)
                .WithMessage(ValidationConstants.AboveMaxLengthMessage);
        });

        RuleFor(word => word.Translations)
            .Must(translations => translations.Select(translation => translation.Text).AllUnique())
            .WithMessage("Translations must be unique.");
    }
}
