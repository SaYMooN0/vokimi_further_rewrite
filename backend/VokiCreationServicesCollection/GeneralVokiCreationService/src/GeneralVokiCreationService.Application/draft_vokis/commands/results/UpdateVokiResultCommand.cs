using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib.general_voki.result_image;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record UpdateVokiResultCommand(
    VokiId VokiId,
    GeneralVokiResultId ResultId,
    VokiResultName NewName,
    VokiResultText NewText,
    GeneralVokiResultImageKey? NewImage
) :
    ICommand<VokiResult>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateResultTextCommandHandler : ICommandHandler<UpdateVokiResultCommand, VokiResult>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateResultTextCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiResult>> Handle(UpdateVokiResultCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithResults(command.VokiId))!;
        var res = voki.UpdateResult(
            command.ResultId, command.NewName, command.NewText, command.NewImage
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess();
    }
}