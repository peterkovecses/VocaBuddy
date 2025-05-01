namespace VocaBuddy.Application.Features.NativeWords.Commands.Create;

public class CreateNativeWordValidator : AbstractValidator<CreateNativeWordCommand>
{
    public CreateNativeWordValidator()
    {
        RuleFor(command => command.NativeWord.Text)
            .NotNull()
            .NotEmpty()
            .MaximumLength(ValidationConstants.MaxWordLength);
        
        RuleForEach(command => command.NativeWord.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text)
                .NotNull()
                .NotEmpty()
                .MaximumLength(ValidationConstants.MaxWordLength);
        });

        RuleFor(command => command.NativeWord.Translations)
            .NotEmpty()
            .WithMessage("At least one translation is required.")
            .Must(translations => translations.Select(translation => translation.Text).AllUnique())
            .WithMessage("Translations must be unique.");
    }
}
