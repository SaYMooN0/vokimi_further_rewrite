namespace SharedKernel.common.vokis.tier_list_voki;

public class TierListVokiInteractionSettings : ValueObject, IVokiInteractionSettings
{
    public bool SignedInOnlyTaking { get; }
    public override IEnumerable<object> GetEqualityComponents() => [SignedInOnlyTaking];
    public TierListVokiInteractionSettings(bool signedInOnlyTaking) {
        SignedInOnlyTaking = signedInOnlyTaking;
    }
}