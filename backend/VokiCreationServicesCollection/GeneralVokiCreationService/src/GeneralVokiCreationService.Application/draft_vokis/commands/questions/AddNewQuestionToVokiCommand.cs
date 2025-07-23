using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record AddNewQuestionToVokiCommand(VokiId VokiId, GeneralVokiAnswerType AnswersType) :
    ICommand<GeneralVokiQuestionId>,
    IWithVokiAccessValidationStep;

internal sealed class AddNewQuestionToVokiCommandHandler :
    ICommandHandler<AddNewQuestionToVokiCommand, GeneralVokiQuestionId>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public AddNewQuestionToVokiCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    private static readonly GeneralVokiAnswerType[] SupportedTyped = [GeneralVokiAnswerType.TextOnly];

    public async Task<ErrOr<GeneralVokiQuestionId>> Handle(AddNewQuestionToVokiCommand command, CancellationToken ct) {
        if (!SupportedTyped.Contains(command.AnswersType)) {
            return ErrFactory.NotImplemented("Selected type is not implemented yet");
        }

        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestions(command.VokiId))!;
        GeneralVokiQuestionId questionId = voki.AddNewQuestion(command.AnswersType);
        await _draftGeneralVokiRepository.Update(voki);
        return questionId;
    }
}