using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DbSeeder.seeding.newtonsoft;

public static class VokiJsonPlaceholderFiller
{
    private static readonly Regex PlaceholderRegex =
        new(@"__([A-Z0-9_]+)__", RegexOptions.Compiled);

    public static string FillPlaceholders(string json) {
        JToken jToken = JToken.Parse(json);

        Dictionary<string, string> generated = [];

        ReplaceInToken(jToken, generated);

        return jToken.ToString(Formatting.Indented);
    }

    private static void ReplaceInToken(JToken token, Dictionary<string, string> generated) {
        if (token.Type == JTokenType.Object) {
            foreach (var prop in token.Children<JProperty>()) {
                ReplaceInToken(prop.Value, generated);
            }
        }
        else if (token.Type == JTokenType.Array) {
            foreach (var item in token.Children()) {
                ReplaceInToken(item, generated);
            }
        }
        else if (token.Type == JTokenType.String) {
            string str = token.Value<string>()!;
            Match match = PlaceholderRegex.Match(str);

            if (!match.Success) {
                return;
            }

            string placeholder = match.Groups[1].Value;

            if (!generated.TryGetValue(placeholder, out string? guid)) {
                guid = Guid.CreateVersion7().ToString();
                generated[placeholder] = guid;
            }


            JToken newToken = new JObject { ["Value"] = guid };

            token.Replace(newToken);
        }
    }
}