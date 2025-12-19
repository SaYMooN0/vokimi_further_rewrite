using SharedKernel.common.vokis.tier_list_voki;

namespace VokisCatalogService.Domain.voki_aggregate.voki_types;

public sealed class TierListVoki : BaseVoki
{
    public override VokiType Type => VokiType.TierList;
    public override IVokiInteractionSettings BaseInteractionSettings => InteractionSettings;
    public TierListVokiInteractionSettings InteractionSettings { get; }
}