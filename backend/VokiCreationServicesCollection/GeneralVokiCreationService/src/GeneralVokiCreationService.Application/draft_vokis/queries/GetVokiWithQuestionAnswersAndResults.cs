using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record GetVokiWithQuestionAnswersAndResults(VokiId VokiId) :
    IQuery<DraftGeneralVoki>,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiWithQuestionAnswersAndResultsHandler : IQueryHandler<GetVokiWithQuestionAnswersAndResults, DraftGeneralVoki>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public GetVokiWithQuestionAnswersAndResultsHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }
    public async Task<ErrOr<DraftGeneralVoki>> Handle(GetVokiWithQuestionAnswersAndResults query, CancellationToken ct) {
        return (await _draftGeneralVokiRepository.GetWithQuestionAnswersAndResultsAsNoTracking(query.VokiId))!; 
    }
}