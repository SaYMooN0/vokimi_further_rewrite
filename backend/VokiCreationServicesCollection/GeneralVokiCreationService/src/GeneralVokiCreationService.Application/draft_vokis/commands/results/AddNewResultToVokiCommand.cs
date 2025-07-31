using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using SharedKernel;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.commands.results;

public sealed record AddNewResultToVokiCommand(VokiId VokiId, VokiResultName ResultName) :
    ICommand<ImmutableArray<VokiResult>>,
    IWithVokiAccessValidationStep;

internal sealed class AddNewResultToVokiCommandHandler :
    ICommandHandler<AddNewResultToVokiCommand, ImmutableArray<VokiResult>>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;
    private readonly IDateTimeProvider _dateTimeProvider;

    public AddNewResultToVokiCommandHandler(
        IDraftGeneralVokiRepository draftGeneralVokiRepository, IDateTimeProvider dateTimeProvider
    ) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
        _dateTimeProvider = dateTimeProvider;
    }


    public async Task<ErrOr<ImmutableArray<VokiResult>>>
        Handle(AddNewResultToVokiCommand command, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithResults(command.VokiId))!;
        var res = voki.AddNewResult(command.ResultName, _dateTimeProvider);
        if (res.IsErr(out var err)) {
            return err;
        }

        await _draftGeneralVokiRepository.Update(voki);
        return voki.Results;
    }
}