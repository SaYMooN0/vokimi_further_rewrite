﻿using GeneralVokiCreationService.Application.common.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.questions;

public sealed record DeleteVokiQuestionCommand(VokiId VokiId, GeneralVokiQuestionId QuestionId) :
    ICommand<ImmutableArray<VokiQuestion>>,
    IWithVokiAccessValidationStep;

internal sealed class DeleteVokiQuestionCommandHandler :
    ICommandHandler<DeleteVokiQuestionCommand, ImmutableArray<VokiQuestion>>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public DeleteVokiQuestionCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }


    public async Task<ErrOr<ImmutableArray<VokiQuestion>>> Handle(
        DeleteVokiQuestionCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestions(command.VokiId))!;
        bool wasDeleted = voki.DeleteQuestion(command.QuestionId);
        if (wasDeleted) {
            await _draftGeneralVokisRepository.Update(voki);
        }

        return voki.Questions;
    }
}