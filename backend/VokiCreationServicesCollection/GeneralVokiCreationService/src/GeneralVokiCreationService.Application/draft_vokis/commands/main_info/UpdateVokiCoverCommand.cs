using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;
using VokimiStorageKeysLib.draft_voki_cover;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.main_info;

public sealed record UpdateVokiCoverCommand(
    VokiId VokiId,
    Stream FileStream,
    string FileName,
    string FileContentType
) :
    ICommand<DraftVokiCoverKey>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateVokiCoverCommandHandler :
    ICommandHandler<UpdateVokiCoverCommand, DraftVokiCoverKey>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IMainStorageBucket _mainStorageBucket;

    public UpdateVokiCoverCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository,
        IMainStorageBucket mainStorageBucket
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _mainStorageBucket = mainStorageBucket;
    }

    public async Task<ErrOr<DraftVokiCoverKey>> Handle(UpdateVokiCoverCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetById(command.VokiId))!;
        var uploadingRes = await _mainStorageBucket.UploadDraftVokiCover(
            command.VokiId, command.FileStream, command.FileName, command.FileContentType
        );
        if (uploadingRes.IsErr(out var err)) {
            return err;
        }
        var result = voki.UpdateCover(uploadingRes.AsSuccess());
        if (result.IsErr(out  err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return voki.Cover;
    }
}