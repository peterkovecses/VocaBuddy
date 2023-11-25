using FluentValidation;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Extensions;
using VocaBuddy.Shared;

namespace VocaBuddy.Application.Validation;

public class CreateNativeWordValidator : AbstractValidator<CreateNativeWordCommand>
{
    public CreateNativeWordValidator()
    {
        RuleFor(command => command.NativeWordDto.Text).NotEmpty();
        RuleFor(command => command.NativeWordDto.Text).MaximumLength(Constants.MaxWordLenth);
        RuleForEach(command => command.NativeWordDto.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).NotEmpty();
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).MaximumLength(Constants.MaxWordLenth);
        });

        RuleFor(command => command.NativeWordDto.Translations)
            .Must(translations => translations.Select(translation => translation.Text).AllUnique())
            .WithMessage("Translations must be unique.");
    }
}
