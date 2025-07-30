﻿using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;

public sealed class VokiResultText : ValueObject
{
    private readonly string _value;

    public const int MinLength = 10;
    public const int MaxLength = 500;

    private VokiResultText(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckForErr(value));
        _value = value;
    }

    public static ErrOr<VokiResultText> Create(string value) =>
        CheckForErr(value).IsErr(out var err) ? err : new VokiResultText(value);

    public static ErrOrNothing CheckForErr(string text) {
        if (string.IsNullOrWhiteSpace(text)) {
            return ErrFactory.NoValue.Common("Result text cannot be empty");
        }

        ErrOrNothing errs = ErrOrNothing.Nothing;

        if (text.Length < MinLength) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Result text must be at least {MinLength} characters",
                $"Provided text is {text.Length} characters"
            ));
        }
        else if (text.Length > MaxLength) {
            errs.AddNext(ErrFactory.ValueOutOfRange(
                $"Result text must not exceed {MaxLength} characters",
                $"Provided text is {text.Length} characters"
            ));
        }

        if (char.IsWhiteSpace(text[0])) {
            errs.AddNext(ErrFactory.IncorrectFormat("Result text cannot start with a space"));
        }

        if (char.IsWhiteSpace(text[^1])) {
            errs.AddNext(ErrFactory.IncorrectFormat("Result text cannot end with a space"));
        }

        return errs;
    }

    public override string ToString() => _value;

    public override IEnumerable<object> GetEqualityComponents() => [_value];
}
