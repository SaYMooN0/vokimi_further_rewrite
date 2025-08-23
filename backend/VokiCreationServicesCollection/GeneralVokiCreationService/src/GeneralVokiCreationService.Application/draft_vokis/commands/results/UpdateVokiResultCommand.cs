using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib.concrete_keys.general_voki;
using VokimiStorageKeysLib.temp_keys;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record UpdateVokiResultCommand(
    VokiId VokiId,
    GeneralVokiResultId ResultId,
    VokiResultName NewName,
    VokiResultText NewText,
    TempImageKey? NewImage
) :
    ICommand<VokiResult>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateResultTextCommandHandler : ICommandHandler<UpdateVokiResultCommand, VokiResult>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public UpdateResultTextCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<VokiResult>> Handle(UpdateVokiResultCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithResults(command.VokiId))!;
        GeneralVokiResultImageKey? newResultImage = null;

        if (command.NewImage is not null) {
            newResultImage = GeneralVokiResultImageKey.CreateForResult(
                command.VokiId, command.ResultId, command.NewImage.Extension
            );
            var copyingRes = await _mainStorageBucket.CopyVokiResultImageFromTempToStandard(
                command.NewImage, newResultImage
            );
            if (copyingRes.IsErr(out var copyingErr)) {
                return ErrFactory.Unspecified(
                    "Couldn't update voki result because of image problems", copyingErr.Message
                );
            }
        }

        var res = voki.UpdateResult(
            command.ResultId, command.NewName, command.NewText, newResultImage
        );
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess();
    }
}