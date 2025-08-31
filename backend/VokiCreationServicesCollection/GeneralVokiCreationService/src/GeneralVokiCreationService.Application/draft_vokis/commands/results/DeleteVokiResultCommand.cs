﻿using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record DeleteVokiResultCommand(VokiId VokiId, GeneralVokiResultId ResultId) :
    ICommand,
    IWithVokiAccessValidationStep;

internal sealed class DeleteVokiResultCommandHandler : ICommandHandler<DeleteVokiResultCommand>
{
    private readonly IDraftGeneralVokisRepository _draftGeneralVokisRepository;

    public DeleteVokiResultCommandHandler(IDraftGeneralVokisRepository draftGeneralVokisRepository) {
        _draftGeneralVokisRepository = draftGeneralVokisRepository;
    }


    public async Task<ErrOrNothing> Handle(
        DeleteVokiResultCommand command, CancellationToken ct
    ) {
        DraftGeneralVoki voki = (await _draftGeneralVokisRepository.GetWithQuestionAnswersAndResults(command.VokiId))!;
        bool wasDeleted = voki.DeleteResult(command.ResultId);
        if (wasDeleted) {
            await _draftGeneralVokisRepository.Update(voki);
        }
        return ErrOrNothing.Nothing;
    }
}