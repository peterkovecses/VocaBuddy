using FluentValidation;
using VocaBuddy.Application.Commands;

namespace VocaBuddy.Application.Validation;

public class InsertNativeWordValidator : AbstractValidator<InsertNativeWordCommand>
{
    public InsertNativeWordValidator()
    {
        RuleFor(command => command.NativeWordDto.Text).NotEmpty();
        RuleForEach(command => command.NativeWordDto.Translations).ChildRules(translationRule => {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).NotEmpty();
        });
    }
}
