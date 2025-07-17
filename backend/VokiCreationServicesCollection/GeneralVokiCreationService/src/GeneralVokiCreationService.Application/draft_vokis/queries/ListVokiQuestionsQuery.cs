using GeneralVokiCreationService.Domain.common.interfaces.repositories;
using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace GeneralVokiCreationService.Application.draft_vokis.queries;

public sealed record ListVokiQuestionsQuery(VokiId VokiId) :
    IQuery<ImmutableArray<VokiQuestion>>,
    IWithVokiAccessValidationStep;

internal sealed class ListVokiQuestionsQueryHandler 
    : IQueryHandler<ListVokiQuestionsQuery, ImmutableArray<VokiQuestion>>
{
    private readonly IDraftGeneralVokiRepository _draftGeneralVokiRepository;

    public ListVokiQuestionsQueryHandler(IDraftGeneralVokiRepository draftGeneralVokiRepository) {
        _draftGeneralVokiRepository = draftGeneralVokiRepository;
    }

    public async Task<ErrOr<ImmutableArray<VokiQuestion>>> Handle(ListVokiQuestionsQuery query, CancellationToken ct) {
        DraftGeneralVoki voki = (await _draftGeneralVokiRepository.GetWithQuestionsAsNoTracking(query.VokiId))!;
        return voki.Questions;
    }
}