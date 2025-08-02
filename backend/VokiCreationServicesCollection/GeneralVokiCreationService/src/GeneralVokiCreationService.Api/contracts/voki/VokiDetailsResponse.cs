using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.voki;

public record class VokiDetailsResponse(
    string Description,
    Language Language,
    bool IsAgeRestricted
)
{
    public static VokiDetailsResponse FromDetails(VokiDetails details) => new(
        details.Description.ToString(),
        details.Language,
        details.IsAgeRestricted
    );
}