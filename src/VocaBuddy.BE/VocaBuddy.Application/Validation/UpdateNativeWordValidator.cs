﻿using FluentValidation;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Extensions;

namespace VocaBuddy.Application.Validation;

public class UpdateNativeWordValidator : AbstractValidator<UpdateNativeWordCommand>
{
    public UpdateNativeWordValidator()
    {
        RuleFor(command => command.NativeWordDto.Text).NotEmpty();
        RuleForEach(command => command.NativeWordDto.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).NotEmpty();
        });

        RuleFor(command => command.NativeWordDto.Translations)
            .Must(translations => translations.AllTranslationsAreUnique())
            .WithMessage("Translations must be unique.");
    }
}
