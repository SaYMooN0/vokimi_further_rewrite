using SharedKernel.common.vokis.scoring_voki;

namespace VokisCatalogService.Domain.voki_aggregate.voki_types;

public sealed class ScoringVoki : BaseVoki
{
    public override VokiType Type => VokiType.Scoring;
    public override IVokiInteractionSettings BaseInteractionSettings => InteractionSettings;
    public ScoringVokiInteractionSettings InteractionSettings { get; }
}