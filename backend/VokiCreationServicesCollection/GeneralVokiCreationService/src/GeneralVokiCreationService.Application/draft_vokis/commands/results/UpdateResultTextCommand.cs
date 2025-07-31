using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record UpdateResultTextCommand(
    VokiId VokiId,
    GeneralVokiResultId ResultId,
    VokiResultText NewResultText
) :
    ICommand<VokiResultText>,
    IWithVokiAccessValidationStep;

internal sealed class UpdateResultTextCommandHandler : ICommandHandler<UpdateResultTextCommand, VokiResultText>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public UpdateResultTextCommandHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<VokiResultText>> Handle(UpdateResultTextCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithResults(command.VokiId))!;
        var res = voki.UpdateResultText(command.ResultId, command.NewResultText);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return res.AsSuccess().Text;
    }
}