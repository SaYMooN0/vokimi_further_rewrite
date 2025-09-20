using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;

namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public sealed class SessionWithFreeAnswering : BaseVokiTakingSession
{
    private SessionWithFreeAnswering() { }

    public override bool IsWithForceSequentialAnswering => false;

    public ImmutableArray<SessionWithFreeAnsweringAnsweredQuestion> AnsweredQuestions { get; private set; }

    public ErrOrNothing SaveAnswers( /*new list*/) {
        if (VokiTaker is null) {
            return ErrFactory.AuthRequired();
        }
        throw new NotImplementedException();
    }

    private SessionWithFreeAnswering(
        VokiTakingSessionId id, VokiId vokiId, AppUserId? vokiTaker, DateTime startTime,
        ImmutableArray<TakingSessionExpectedQuestion> questions
    ) : base(id, vokiId, vokiTaker, startTime, questions) { }

    public static SessionWithFreeAnswering Create(
        VokiId vokiId, AppUserId? vokiTaker, DateTime startTime,
        IEnumerable<VokiQuestion> vokiQuestions, bool shuffleQuestions
    ) {
        VokiQuestion[] ordered = (
            shuffleQuestions
                ? vokiQuestions.OrderBy(_ => Guid.NewGuid())
                : vokiQuestions.OrderBy(q => q.OrderInVoki)
        ).ToArray();

        List<TakingSessionExpectedQuestion> sessionQuestion = new(ordered.Length);
        for (ushort i = 0; i < ordered.Length; i++) {
            var question = ordered[i];
            sessionQuestion.Add(new TakingSessionExpectedQuestion(
                question.Id,
                OrderInVokiTaking: i,
                question.AnswersCountLimit.MinAnswers,
                question.AnswersCountLimit.MaxAnswers,
                question.Answers.Select(a => a.Id).ToImmutableHashSet()
            ));
        }

        return new SessionWithFreeAnswering(
            VokiTakingSessionId.CreateNew(), vokiId, vokiTaker, startTime,
            sessionQuestion.ToImmutableArray()
        );
    }

    public ErrOrNothing ValidateChosenAnswers(
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> chosenAnswers
    ) {
        var errs = ErrOrNothing.Nothing;

        if (chosenAnswers is null) {
            errs.AddNext(ErrFactory.NoValue.Common("Chosen answers are missing"));
            return errs;
        }

        ImmutableDictionary<GeneralVokiQuestionId, TakingSessionExpectedQuestion> expectedById = Questions
            .ToImmutableDictionary(q => q.QuestionId, q => q);

        foreach (var providedQuestionId in chosenAnswers.Keys) {
            if (!expectedById.ContainsKey(providedQuestionId)) {
                errs.AddNext(ErrFactory.IncorrectFormat(
                    "You answered a question that is not part of this test"
                ));
            }
        }

        foreach (var kv in expectedById) {
            var question = kv.Value;
            chosenAnswers.TryGetValue(kv.Key, out var providedSet);

            errs.AddNextIfErr(ValidateSingleQuestionAnswers(question, providedSet));
        }

        return errs;
    }

    public ImmutableArray<VokiTakenQuestionDetails> GatherQuestionDetails(
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> chosenAnswers
    ) {
        ImmutableDictionary<GeneralVokiQuestionId, ushort> questionIdToOrder = Questions
            .ToImmutableDictionary(q => q.QuestionId, q => q.OrderInVokiTaking);

        return chosenAnswers
            .Select(qToAnswers => new VokiTakenQuestionDetails(
                qToAnswers.Key,
                qToAnswers.Value,
                questionIdToOrder[qToAnswers.Key]
            ))
            .ToImmutableArray();
    }
}

public record SessionWithFreeAnsweringAnsweredQuestion(
    GeneralVokiQuestionId QuestionId,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswerIds
);