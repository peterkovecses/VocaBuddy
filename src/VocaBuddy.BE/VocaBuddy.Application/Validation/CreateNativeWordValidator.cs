using FluentValidation;
using VocaBuddy.Application.Commands;
using VocaBuddy.Shared;
using VocaBuddy.Shared.Extensions;

namespace VocaBuddy.Application.Validation;

public class CreateNativeWordValidator : AbstractValidator<CreateNativeWordCommand>
{
    public CreateNativeWordValidator()
    {
        RuleFor(command => command.NativeWorld.Text).NotEmpty();
        RuleFor(command => command.NativeWorld.Text).MaximumLength(Constants.MaxWordLength);
        RuleForEach(command => command.NativeWorld.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).NotEmpty();
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).MaximumLength(Constants.MaxWordLength);
        });

        RuleFor(command => command.NativeWorld.Translations)
            .Must(translations => translations.Select(translation => translation.Text).AllUnique())
            .WithMessage("Translations must be unique.");
    }
}
