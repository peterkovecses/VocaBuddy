﻿using FluentValidation;
using VocaBuddy.Application.Commands;
using VocaBuddy.Shared;
using VocaBuddy.Shared.Extensions;

namespace VocaBuddy.Application.Validation;

public class UpdateNativeWordValidator : AbstractValidator<UpdateNativeWordCommand>
{
    public UpdateNativeWordValidator()
    {
        RuleFor(command => command.NativeWordDto.Text).NotEmpty();
        RuleFor(command => command.NativeWordDto.Text).MaximumLength(Constants.MaxWordLength);
        RuleForEach(command => command.NativeWordDto.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).NotEmpty();
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).MaximumLength(Constants.MaxWordLength);
        });

        RuleFor(command => command.NativeWordDto.Translations)
            .Must(translations => translations.Select(translation => translation.Text).AllUnique())
            .WithMessage("Translations must be unique.");
    }
}
