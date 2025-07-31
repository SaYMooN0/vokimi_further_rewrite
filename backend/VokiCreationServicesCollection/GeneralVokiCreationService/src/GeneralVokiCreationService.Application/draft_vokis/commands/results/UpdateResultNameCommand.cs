using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record UpdateResultNameCommand(
    VokiId VokiId,
    GeneralVokiResultId ResultId,
    VokiResultName NewResultName
) :
    ICommand<VokiResultName>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateResultNameCommandHandler : ICommandHandler<UpdateResultNameCommand, VokiResultName>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateResultNameCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiResultName>> Handle(UpdateResultNameCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithResults(command.VokiId))!;
        var res = voki.UpdateResultName(command.ResultId, command.NewResultName);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess().Name;
    }
}