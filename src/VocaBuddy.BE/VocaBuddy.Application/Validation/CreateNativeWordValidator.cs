using FluentValidation;

namespace VocaBuddy.Application.Validation;

public class CreateNativeWordValidator : AbstractValidator<CreateNativeWordCommand>
{
    public CreateNativeWordValidator()
    {
        RuleFor(command => command.NativeWorld.Text).NotEmpty();
        RuleFor(command => command.NativeWorld.Text).MaximumLength(ValidationConstants.MaxWordLength);
        RuleForEach(command => command.NativeWorld.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).NotEmpty();
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).MaximumLength(ValidationConstants.MaxWordLength);
        });

        RuleFor(command => command.NativeWorld.Translations)
            .Must(translations => translations.Select(translation => translation.Text).AllUnique())
            .WithMessage("Translations must be unique.");
    }
}
