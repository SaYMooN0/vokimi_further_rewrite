using ApplicationShared.messaging;
using ApplicationShared.messaging.pipeline_behaviors;
using SharedKernel.common.vokis;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using VokiCreationServicesLib.Application.common;
using VokiCreationServicesLib.Application.pipeline_behaviors;

namespace VokiCreationServicesLib.Application.draft_vokis.queries;


public sealed record GetVokiNameQuery(VokiId VokiId) :
    IQuery<VokiName>,
    IWithAuthCheckStep,
    IWithVokiAccessValidationStep;

internal sealed class GetVokiNameQueryHandler : IQueryHandler<GetVokiNameQuery, VokiName>
{
    private readonly IDraftVokiRepository _draftVokiRepository;
    public GetVokiNameQueryHandler(IDraftVokiRepository draftVokiRepository) {
        _draftVokiRepository = draftVokiRepository;
    }

    public async Task<ErrOr<VokiName>> Handle(GetVokiNameQuery query, CancellationToken ct) {
        return (await _draftVokiRepository.GetByIdAsNoTracking(query.VokiId, ct))!.Name; 
    }
}