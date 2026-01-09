using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taken_record_aggregate;
using SharedKernel;
using SharedKernel.user_ctx;

namespace GeneralVokiTakingService.Domain.voki_taking_session_aggregate;

public sealed class SessionWithFreeAnswering : BaseVokiTakingSession
{
    private SessionWithFreeAnswering() { }

    public override bool IsWithForceSequentialAnswering => false;

    public ErrOr<VokiTakingSessionFinishedDto> FinishAndReceiveResult(
        DateTime currentTime,
        ClientServerTimePairDto sessionStartTime,
        DateTime clientFinishedTime,
        AuthenticatedUserCtx? authenticatedUserContext,
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>> chosenAnswers,
        Func<Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>,
            ErrOr<GeneralVokiResultId>> getResultAccordingToAnswers
    ) {
        if (
            ValidateStartAndFinishTime(
                currentTime: currentTime,
                sessionStartTime: sessionStartTime,
                clientFinishTime: clientFinishedTime
            ).IsErr(out var err)
        ) {
            return err;
        }


        if (ValidateVokiTaker(authenticatedUserContext, out var vokiTakerId).IsErr(out err)) {
            return err;
        }

        if (ValidateChosenAnswers(chosenAnswers).IsErr(out err)) {
            return err;
        }

        ErrOr<GeneralVokiResultId> resOrErr = getResultAccordingToAnswers(chosenAnswers);
        if (resOrErr.IsErr(out err)) {
            return err;
        }

        ImmutableDictionary<GeneralVokiQuestionId, ushort> questionIdToOrder = Questions
            .ToImmutableDictionary(q => q.QuestionId, q => q.OrderInVokiTaking);

        ImmutableArray<VokiTakenQuestionDetails> questionDetails = chosenAnswers
            .Select(qToAnswers => new VokiTakenQuestionDetails(
                qToAnswers.Key,
                qToAnswers.Value,
                questionIdToOrder[qToAnswers.Key]
            ))
            .ToImmutableArray();

        return new VokiTakingSessionFinishedDto(
            VokiId,
            vokiTakerId,
            sessionStartTime.Client,
            clientFinishedTime,
            WasSessionWithForcedSequentialOrder: false,
            ReceivedResultId: resOrErr.AsSuccess(),
            questionDetails
        );
    }

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
            VokiQuestion question = ordered[i];
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

    private ErrOrNothing ValidateChosenAnswers(
        Dictionary<GeneralVokiQuestionId, ImmutableHashSet<GeneralVokiAnswerId>>? chosenAnswers
    ) {
        ErrOrNothing errs = ErrOrNothing.Nothing;

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
}

public record SessionWithFreeAnsweringAnsweredQuestion(
    GeneralVokiQuestionId QuestionId,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswerIds
);