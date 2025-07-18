using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record AddNewQuestionToVokiCommand(VokiId VokiId, GeneralVokiAnswerType AnswersType) :
    ICommand<VokiQuestion>,
    IWithVokiAccessValidationStep;

internal sealed class AddNewQuestionToVokiCommandHandler :
    ICommandHandler<AddNewQuestionToVokiCommand, VokiQuestion>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public AddNewQuestionToVokiCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiQuestion>> Handle(AddNewQuestionToVokiCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestions(command.VokiId))!;
        var question = voki.AddNewQuestion(command.AnswersType);
        await _draftGeneralVokiRepository.Update(voki);
        return question;
    }
}