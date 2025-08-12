using VokiCreationServicesLib.Application;

namespace GeneralVokiCreationService.Api.contracts.voki;

public record class VokiSuccessfullyPublishedResponse(
    string Id,
    string Cover,
    string Name
)
{
    public static VokiSuccessfullyPublishedResponse Create(VokiSuccessfullyPublishedResult result) => new(
        result.Id.ToString(),
        result.Cover.ToString(),
        result.Name.ToString()
    );
}