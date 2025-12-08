using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.voki;

public record class VokiNameResponse(
    string VokiId,
    string VokiName
) : ICreatableResponse<DraftGeneralVoki>

{
    public static ICreatableResponse<DraftGeneralVoki> Create(DraftGeneralVoki voki) => new VokiNameResponse(
        voki.Id.ToString(),
        voki.Name.ToString()
    );
}