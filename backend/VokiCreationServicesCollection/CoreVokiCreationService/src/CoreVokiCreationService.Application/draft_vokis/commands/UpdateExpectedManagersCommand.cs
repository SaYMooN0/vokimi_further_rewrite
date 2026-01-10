using ApplicationShared.messaging.pipeline_behaviors;
using CoreVokiCreationService.Application.common.repositories;
using CoreVokiCreationService.Application.pipeline_behaviors;
using CoreVokiCreationService.Domain.draft_voki_aggregate;

namespace CoreVokiCreationService.Application.draft_vokis.commands;

public sealed record UpdateExpectedManagersCommand(
    VokiId VokiId,
    VokiExpectedManagersSetting NewSettingValue
) :
    ICommand<VokiExpectedManagersSetting>,
    IWithAuthCheckStep,
    IWithVokiPrimaryAuthorValidationStep;

internal sealed class UpdateExpectedManagersCommandHandler
    : ICommandHandler<UpdateExpectedManagersCommand, VokiExpectedManagersSetting>
{
    private readonly IDraftVokiRepository _draftVokiRepository;

    public UpdateExpectedManagersCommandHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }


    public async Task<ErrOr<VokiExpectedManagersSetting>> Handle(UpdateExpectedManagersCommand command, CancellationToken ct) {
        DraftVoki voki = (await _draftVokiRepository.GetByIdForUpdate(command.VokiId, ct))!;
        var res = voki.UpdateExpectedManagers(command.NewSettingValue);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftVokiRepository.Update(voki, ct);
        return voki.ExpectedManagers;
    }
}