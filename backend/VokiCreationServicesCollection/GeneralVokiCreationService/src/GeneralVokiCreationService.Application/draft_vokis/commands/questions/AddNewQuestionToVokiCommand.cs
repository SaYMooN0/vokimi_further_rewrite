using ApplicationShared.messaging.pipeline_behaviors;
using GeneralVokiCreationService.Application.common.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using SharedKernel.common.vokis.general_vokis;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record AddNewQuestionToVokiCommand(VokiId VokiId, GeneralVokiAnswerType AnswersType) :
    ICommand<GeneralVokiQuestionId>,   
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class AddNewQuestionToVokiCommandHandler :
    ICommandHandler<AddNewQuestionToVokiCommand, GeneralVokiQuestionId>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public AddNewQuestionToVokiCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }

    private static readonly GeneralVokiAnswerType[] SupportedTyped = [
        GeneralVokiAnswerType.TextOnly,
        GeneralVokiAnswerType.ColorOnly,
        GeneralVokiAnswerType.ColorAndText,
        GeneralVokiAnswerType.ImageOnly,
        GeneralVokiAnswerType.ImageAndText,
    ];

    public async Task<ErrOr<GeneralVokiQuestionId>> Handle(AddNewQuestionToVokiCommand command, CancellationToken ct) {
        if (!SupportedTyped.Contains(command.AnswersType)) {
            return ErrFactory.NotImplemented("Selected type is not implemented yet");
        }

        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestions(command.VokiId))!;
        var res = voki.AddNewQuestion(command.AnswersType);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokisRepository.Update(voki);
        return res.AsSuccess();
    }
}