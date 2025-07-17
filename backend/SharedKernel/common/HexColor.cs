using System.Text.RegularExpressions;

namespace SharedKernel.common;

public class HexColor : ValueObject
{
    private static readonly Regex HexColorRegex = new("^#[a-fA-F0-9]{6}$");
    private string Value { get; }

    public HexColor(string value) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckHexColorForErr(value));
        Value = value;
    }

    public static ErrOr<HexColor> Create(string color) {
        color = color.Trim();

        if (CheckHexColorForErr(color).IsErr(out var err)) {
            return err;
        }

        return new HexColor(color);
    }

    public static ErrOrNothing CheckHexColorForErr(string color) => HexColorRegex.IsMatch(color)
        ? ErrOrNothing.Nothing
        : ErrFactory.IncorrectFormat($"Invalid hex color format: {color}");

    public override IEnumerable<object> GetEqualityComponents() => [Value];
    public override string ToString() => Value;
}