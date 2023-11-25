using FluentValidation;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Extensions;

namespace VocaBuddy.Application.Validation;

public class UpdateNativeWordValidator : AbstractValidator<UpdateNativeWordCommand>
{
    public UpdateNativeWordValidator()
    {
        RuleFor(command => command.NativeWordDto.Text).NotEmpty();
        RuleFor(command => command.NativeWordDto.Text)
            .MaximumLength(Constants.MaxWordLenth)
            .WithMessage(Constants.AboveMaxWordLengthMessage);
        RuleForEach(command => command.NativeWordDto.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).NotEmpty();
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text)
                .MaximumLength(Constants.MaxWordLenth)
                .WithMessage(Constants.AboveMaxWordLengthMessage);
        });

        RuleFor(command => command.NativeWordDto.Translations)
            .Must(translations => translations.Select(translation => translation.Text).AllUnique())
            .WithMessage("Translations must be unique.");
    }
}
