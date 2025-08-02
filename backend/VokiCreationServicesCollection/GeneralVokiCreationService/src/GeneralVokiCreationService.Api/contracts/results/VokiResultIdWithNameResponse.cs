using GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.results;

public record class VokiResultIdWithNameResponse(string Id, string Name)
{
    public static VokiResultIdWithNameResponse Create(VokiResult result) => new(
        result.Id.ToString(),
        result.Name.ToString()
    );
}