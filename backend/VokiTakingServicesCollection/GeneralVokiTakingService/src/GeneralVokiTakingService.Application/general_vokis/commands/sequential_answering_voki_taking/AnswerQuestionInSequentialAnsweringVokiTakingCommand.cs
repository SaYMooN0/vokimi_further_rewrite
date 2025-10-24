using GeneralVokiTakingService.Application.common.repositories;
using GeneralVokiTakingService.Application.common.repositories.taking_sessions;
using GeneralVokiTakingService.Domain.common;
using GeneralVokiTakingService.Domain.common.dtos;
using GeneralVokiTakingService.Domain.general_voki_aggregate;
using GeneralVokiTakingService.Domain.voki_taking_session_aggregate;
using SharedKernel;
using SharedKernel.auth;

namespace GeneralVokiTakingService.Application.general_vokis.commands.sequential_answering_voki_taking;

public sealed record AnswerQuestionInSequentialAnsweringVokiTakingCommand(
    VokiId VokiId,
    VokiTakingSessionId SessionId,
    GeneralVokiQuestionId QuestionId,
    ClientServerTimePairDto ShownAt,
    DateTime ClientQuestionAnsweredAt,
    ushort QuestionOrderInVokiTaking,
    ImmutableHashSet<GeneralVokiAnswerId> ChosenAnswers
) : ICommand<VokiTakingQuestionData>;

internal sealed class AnswerQuestionInSequentialAnsweringVokiTakingCommandHandler :
    ICommandHandler<AnswerQuestionInSequentialAnsweringVokiTakingCommand, VokiTakingQuestionData>
{
    private readonly ISessionsWithSequentialAnsweringRepository _sessionsWithSequentialAnsweringRepository;
    private readonly IGeneralVokisRepository _generalVokisRepository;
    private readonly IUserContext _userContext;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AnswerQuestionInSequentialAnsweringVokiTakingCommandHandler(
        ISessionsWithSequentialAnsweringRepository sessionsWithSequentialAnsweringRepository,
        IGeneralVokisRepository generalVokisRepository, IUserContext userContext, IDateTimeProvider dateTimeProvider
    ) {
        _sessionsWithSequentialAnsweringRepository = sessionsWithSequentialAnsweringRepository;
        _generalVokisRepository = generalVokisRepository;
        _userContext = userContext;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<ErrOr<VokiTakingQuestionData>> Handle(
        AnswerQuestionInSequentialAnsweringVokiTakingCommand command, CancellationToken ct
    ) {
        GeneralVoki? voki = await _generalVokisRepository.GetWithQuestionAnswersAsNoTracking(command.VokiId, ct);
        if (voki is null) {
            return ErrFactory.NotFound.Voki("Cannot answer the question because requested Voki does not exist");
        }

        SessionWithSequentialAnswering? session = await _sessionsWithSequentialAnsweringRepository.GetById(command.SessionId, ct);
        if (session is null) {
            return ErrFactory.NotFound.Common(
                "Cannot answer the question because Voki taking session with sequential answering was not found "
            );
        }

        var answeringResult = session.AnswerQuestionAndGetNext(
            command.VokiId,
            shownAt: command.ShownAt,
            currentTime: _dateTimeProvider.UtcNow,
            clientQuestionAnsweredAt: command.ClientQuestionAnsweredAt,
            questionId: command.QuestionId,
            questionOrderInVokiTaking: command.QuestionOrderInVokiTaking,
            chosenAnswers: command.ChosenAnswers
        );
        if (answeringResult.IsErr(out var err)) {
            return err;
        }

        (GeneralVokiQuestionId nextQuestionId, ushort orderInVokiTaking) = answeringResult.AsSuccess();
        var nextQuestion = voki.Questions.FirstOrDefault(q => q.Id == nextQuestionId);
        if (nextQuestion is null) {
            return ErrFactory.NotFound.VokiContent("Expected next question was not found in Voki");
        }

        await _sessionsWithSequentialAnsweringRepository.Update(session, ct);

        return VokiTakingQuestionData.Create(nextQuestion, orderInVokiTaking);
    }
}