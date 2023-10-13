using FluentValidation;
using VocaBuddy.Application.Commands;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Validation;

public class CreateNativeWordValidator : AbstractValidator<CreateNativeWordCommand>
{
    public CreateNativeWordValidator()
    {
        RuleFor(command => command.NativeWordDto.Text).NotEmpty();
        RuleForEach(command => command.NativeWordDto.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).NotEmpty();
        });

        RuleFor(command => command.NativeWordDto.Translations)
            .Must(HaveAllUniqueTranslations)
            .WithMessage("Translations must be unique.");
    }

    private bool HaveAllUniqueTranslations(List<ForeignWordDto> translations)
        => translations
            .GroupBy(translation => translation.Text)
            .All(group => group.Count() == 1);
}
