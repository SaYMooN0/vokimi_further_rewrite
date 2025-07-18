using System.Text.RegularExpressions;
using SharedKernel.errs.utils;

namespace VokimiStorageKeysLib;

public class KeyTemplateParser
{
    private const int MaxStrLength = 60;

    private static readonly Regex PlaceholderRegex = new(@"\{(\w+)(?::(\w+))?\}");

    private readonly Regex _regex;
    private readonly string[] _placeholders;
    private readonly ImmutableDictionary<string, string> _typesByPlaceholder;
    private readonly ImmutableHashSet<string> _allowedExtensions;

    private static readonly Dictionary<string, Func<string, bool>> SupportedPlaceholderTypes = new() {
        { "id", val => Guid.TryParse(val, out _) },
        { "str", val => !string.IsNullOrWhiteSpace(val) && val.Length <= MaxStrLength }
    };

    public KeyTemplateParser(string template, ImmutableHashSet<string> allowedExtensions) {
        List<string> placeholders = [];
        Dictionary<string, string> typesByPlaceholder = [];
        if (allowedExtensions is null || allowedExtensions.Count == 0) {
            throw new ArgumentException("Allowed extensions must be non-empty");
        }

        _allowedExtensions = allowedExtensions.Select(e => e.TrimStart('.').ToLowerInvariant()).ToImmutableHashSet();

        string pattern = "^" + PlaceholderRegex.Replace(template, match => {
            var name = match.Groups[1].Value;
            var type = match.Groups[2].Success ? match.Groups[2].Value : "str";

            placeholders.Add(name);
            typesByPlaceholder[name] = type;

            return $"(?<{name}>{type switch {
                "id" => @"[0-9a-fA-F\-]{36}",
                "str" => $@"[^/{{}}]{{1,{MaxStrLength}}}",
                _ => throw new NotSupportedException($"Unsupported placeholder type: {type}")
            }})";
        });

        pattern += @"\.(?<ext>\w+)$";
        pattern += "$";

        _placeholders = placeholders.ToArray();
        _typesByPlaceholder = typesByPlaceholder.ToImmutableDictionary();
        _regex = new Regex(pattern, RegexOptions.Compiled);
    }

    public ErrOr<Dictionary<string, string>> TryParse(string value) {
        Dictionary<string, string> result = [];

        var match = _regex.Match(value);
        if (!match.Success) {
            return ErrFactory.IncorrectFormat("Key does not match expected format", value);
        }

        foreach (var name in _placeholders) {
            var val = match.Groups[name].Value;

            if (!_typesByPlaceholder.TryGetValue(name, out var type)) {
                UnexpectedBehaviourException.ThrowErr(ErrFactory.Unspecified($"No type found for placeholder: {name}"));
            }

            if (!SupportedPlaceholderTypes.TryGetValue(type, out var validator)) {
                return ErrFactory.IncorrectFormat($"Unsupported placeholder type: '{type}' in key");
            }

            if (!validator(val)) {
                return ErrFactory.IncorrectFormat($"Invalid value '{val}' for placeholder '{name}' of type '{type}'");
            }

            result[name] = val;
        }

        var ext = match.Groups["ext"].Value.ToLowerInvariant();
        if (!_allowedExtensions.Contains(ext)) {
            return ErrFactory.IncorrectFormat(
                $"Extension '.{ext}' is not allowed", $"Allowed: {string.Join(", ", _allowedExtensions)}"
            );
        }

        return result;
    }
}