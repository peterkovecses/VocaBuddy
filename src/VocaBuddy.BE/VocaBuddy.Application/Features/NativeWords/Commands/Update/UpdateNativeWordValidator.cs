﻿namespace VocaBuddy.Application.Features.NativeWords.Commands.Update;

public class UpdateNativeWordValidator : AbstractValidator<UpdateNativeWordCommand>
{
    public UpdateNativeWordValidator()
    {
        RuleFor(command => command.NativeWord.Text).NotNull();
        RuleFor(command => command.NativeWord.Text).NotEmpty();
        RuleFor(command => command.NativeWord.Text).MaximumLength(ValidationConstants.MaxWordLength);
        RuleForEach(command => command.NativeWord.Translations).ChildRules(translationRule =>
        {
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).NotNull();
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).NotEmpty();
            translationRule.RuleFor(foreignWordDto => foreignWordDto.Text).MaximumLength(ValidationConstants.MaxWordLength);
        });

        RuleFor(command => command.NativeWord.Translations)
            .NotEmpty()
            .WithMessage("At least one translation is required.")
            .Must(translations => translations.Select(translation => translation.Text).AllUnique())
            .WithMessage("Translations must be unique.");
    }
}
