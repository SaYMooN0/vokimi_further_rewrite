using VokisCatalogService.Application.vokis.queries;

namespace VokisCatalogService.Api.contracts;

public record class UserTakenVokiIdsResponse(
    Dictionary<string, DateTime> VokiIdWithLastTakenDate
) : ICreatableResponse<VokiIdWithLastTakenDateDto[]>
{
    public static ICreatableResponse<VokiIdWithLastTakenDateDto[]> Create(VokiIdWithLastTakenDateDto[] vokis) =>
        new UserTakenVokiIdsResponse(
            vokis.ToDictionary(v => v.VokiId.ToString(), v => v.Date)
        );
}