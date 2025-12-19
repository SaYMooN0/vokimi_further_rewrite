namespace SharedKernel.common.vokis.scoring_voki;

public class ScoringVokiInteractionSettings : ValueObject, IVokiInteractionSettings
{
    public bool SignedInOnlyTaking { get; }
    public override IEnumerable<object> GetEqualityComponents() => [SignedInOnlyTaking];

    public ScoringVokiInteractionSettings(bool signedInOnlyTaking) {
        SignedInOnlyTaking = signedInOnlyTaking;
    }
}