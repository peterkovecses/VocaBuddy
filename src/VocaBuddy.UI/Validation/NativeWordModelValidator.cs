namespace VocaBuddy.UI.Validation;

public class NativeWordModelValidator : AbstractValidator<INativeWordDto>
{
    public NativeWordModelValidator()
    {
        RuleFor(word => word.Text)
            .NotEmpty()
            .WithMessage("The field is required")
            .MaximumLength(ValidationConstants.MaxWordLength)
            .WithMessage(ValidationConstants.AboveMaxLengthMessage);
        
        RuleFor(word => word.Translations)
            .NotEmpty()
            .WithMessage("At least one translation is required.")
            .Must(translations => translations.Select(translation => translation.Text).AllUnique())
            .WithMessage("Translations must be unique.");
        
        RuleForEach(word => word.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text)
                .NotEmpty()
                .WithMessage("The field is required")
                .MaximumLength(ValidationConstants.MaxWordLength)
                .WithMessage(ValidationConstants.AboveMaxLengthMessage);
        });
    }
}
