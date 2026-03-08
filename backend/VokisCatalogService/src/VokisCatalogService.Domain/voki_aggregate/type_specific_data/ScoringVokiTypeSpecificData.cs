namespace VokisCatalogService.Domain.voki_aggregate.type_specific_data;

public sealed class ScoringVokiTypeSpecificData : BaseVokiTypeSpecificData
{
    private ScoringVokiTypeSpecificData() { }
    public override IEnumerable<object> GetEqualityComponents() => [];
}
