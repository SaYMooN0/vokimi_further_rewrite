namespace VokisCatalogService.Domain.voki_aggregate;

public record class VokiDetails(
    string Description,
    bool IsAgeRestricted,
    Language Language
);