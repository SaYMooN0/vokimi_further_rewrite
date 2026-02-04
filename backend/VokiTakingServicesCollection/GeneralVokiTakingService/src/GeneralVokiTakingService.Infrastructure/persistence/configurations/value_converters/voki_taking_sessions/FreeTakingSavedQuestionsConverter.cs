using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiTakingService.Infrastructure.persistence.configurations.value_converters.voki_taking_sessions;

public class FreeTakingSavedQuestionsConverter
    : ValueConverter<ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>, string[]>
{
    private const char QuestionSeparator = ':';
    private const char AnswersSeparator = ',';

    public FreeTakingSavedQuestionsConverter() : base(
        d => ToStringArray(d),
        s => FromStringArray(s)
    ) { }

    private static string[] ToStringArray(
        ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> dictionary
    ) =>
        dictionary
            .Select(kvp => {
                var answerIds = string.Join(AnswersSeparator, kvp.Value.Select(a => a.ToString()));
                return $"{kvp.Key}{QuestionSeparator}{answerIds}";
            })
            .ToArray();

    private static ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> FromStringArray(
        string[] strings
    ) {
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> dictionary = [];

        foreach (var str in strings) {
            var parts = str.Split(QuestionSeparator);
            if (parts.Length != 2) {
                if (!str.Contains(QuestionSeparator)) continue;
            }

            var qId = new GeneralVokiQuestionId(new Guid(parts[0]));
            var answerIds = parts.Length > 1 && !string.IsNullOrEmpty(parts[1])
                ? parts[1].Split(AnswersSeparator).Select(a => new GeneralVokiAnswerId(new Guid(a))).ToImmutableHashSet()
                : ImmutableHashSet<GeneralVokiAnswerId>.Empty;

            dictionary[qId] = answerIds;
        }

        return dictionary.ToImmutableDictionary();
    }
}

public class FreeTakingSavedQuestionsComparer : ValueComparer<
    ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>>
{
    public FreeTakingSavedQuestionsComparer() : base(
        (a, b) => AreEqual(a, b),
        (a) => GetHashCodeSafe(a),
        (a) => SnapshotSafe(a)
    ) { }

    private static bool AreEqual(
        ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>? d1,
        ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>? d2
    ) {
        if (d1 is null && d2 is null) {
            return true;
        }

        if (d1 is null || d2 is null) {
            return false;
        }

        if (d1.Count != d2.Count) {
            return false;
        }

        foreach (var kvp in d1) {
            if (!d2.TryGetValue(kvp.Key, out var val2)) {
                return false;
            }

            if (!kvp.Value.SetEquals(val2)) {
                return false;
            }
        }

        return true;
    }

    private static int GetHashCodeSafe(ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> d) {
        if (d is null) {
            return 0;
        }

        int hash = 17;
        foreach (var kvp in d.OrderBy(k => k.Key.Value)) {
            hash = hash * 31 + kvp.Key.GetHashCode();
            int answersHash = 0;
            foreach (var a in kvp.Value) answersHash ^= a.GetHashCode();
            hash = hash * 31 + answersHash;
        }

        return hash;
    }

    private static ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> SnapshotSafe(
        ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> d
    ) => d is null
        ? ImmutableDictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>.Empty
        : d.ToImmutableDictionary(k => k.Key, k => k.Value);
}