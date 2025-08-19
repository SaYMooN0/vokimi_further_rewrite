using System.Text.RegularExpressions;
using SharedKernel.errs.utils;

namespace VokimiStorageKeysLib;

public sealed class KeyTemplateParser
{
    private readonly Regex _regex;
    private readonly ImmutableDictionary<string, PlaceholderType> _typesByPlaceholder;
    private readonly ImmutableHashSet<string> _allowedExtensions;

    public KeyTemplateParser(string template, ImmutableHashSet<string> allowedExtensions) {
        if (allowedExtensions is null || allowedExtensions.Count == 0) {
            throw new ArgumentException("Allowed extensions must be non-empty");
        }

        _allowedExtensions = allowedExtensions
            .Select(e => e.TrimStart('.').ToLowerInvariant())
            .ToImmutableHashSet();

        Dictionary<string, PlaceholderType> typesByPlaceholderName = new(StringComparer.Ordinal);

        string pattern = "^" + PlaceholderRegex.Replace(template, match => {
            string placeholderName = match.Groups[1].Value;
            string placeholderType = match.Groups[2].Success ? match.Groups[2].Value : "str";

            if (!SupportedPlaceholderTypes.TryGetValue(placeholderType, out var typeDef)) {
                throw new NotSupportedException($"Unsupported placeholder type: {placeholderType}");
            }

            if (typesByPlaceholderName.ContainsKey(placeholderName)) {
                throw new NotSupportedException($"Duplicate placeholder '{placeholderName}' is not allowed");
            }

            typesByPlaceholderName[placeholderName] = typeDef;

            var body = typeDef.BodyRegex.ToString();
            return $"(?<{placeholderName}>{body})";
        });

        // file extension
        pattern += @"\.(?<ext>\w+)$";

        _typesByPlaceholder = typesByPlaceholderName.ToImmutableDictionary();
        _regex = new Regex(pattern, DefaultRegexOptions);
    }

    private const RegexOptions DefaultRegexOptions = RegexOptions.Compiled |
                                                     RegexOptions.CultureInvariant |
                                                     RegexOptions.ExplicitCapture;


    private static readonly Regex PlaceholderRegex = new(@"<(\w+)(?::(\w+))?>", DefaultRegexOptions);

    private sealed record PlaceholderType(
        string Name,
        Regex BodyRegex,
        Func<string, bool> Validator
    );

    private const int MaxStrLength = 60;

    private static readonly IReadOnlyDictionary<string, PlaceholderType> SupportedPlaceholderTypes =
        new Dictionary<string, PlaceholderType>(StringComparer.OrdinalIgnoreCase) {
            ["id"] = new(Name: "id", new Regex(@"[0-9a-fA-F\-]{36}", DefaultRegexOptions),
                s => Guid.TryParse(s, out _)
            ),
            ["str"] = new(
                Name: "str", new Regex($@"[^/<>]{{1,{MaxStrLength}}}", DefaultRegexOptions),
                s => !string.IsNullOrWhiteSpace(s) && s.Length <= MaxStrLength
            ),
        };


    public ErrOr<Dictionary<string, string>> TryParse(string value) {
        var match = _regex.Match(value);
        if (!match.Success) {
            return ErrFactory.IncorrectFormat("Key does not match expected format", value);
        }

        Dictionary<string, string> result = new(_typesByPlaceholder.Count, StringComparer.Ordinal);

        foreach (var (name, typeDef) in _typesByPlaceholder) {
            var placeholderValue = match.Groups[name].Value;
            if (!typeDef.Validator(placeholderValue)) {
                return ErrFactory.IncorrectFormat(
                    $"Invalid value '{placeholderValue}' for placeholder '{name}' of type '{typeDef.Name}'"
                );
            }

            result[name] = placeholderValue;
        }

        string ext = match.Groups["ext"].Value.ToLowerInvariant();
        if (!_allowedExtensions.Contains(ext))
            return ErrFactory.IncorrectFormat(
                $"Extension '.{ext}' is not allowed", $"Allowed: {string.Join(", ", _allowedExtensions)}"
            );

        return result;
    }
}