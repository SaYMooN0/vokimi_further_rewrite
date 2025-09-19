namespace SharedKernel.common.vokis.general_vokis;

public enum GeneralVokiResultsVisibility
{
    Anyone,
    AfterTaking,
    OnlyReceived
}
public static class GeneralVokiResultsVisibilityExtensions
{
    public static T Match<T>(
        this GeneralVokiResultsVisibility visibility,
        Func<T> anyone,
        Func<T> afterTaking,
        Func<T> onlyReceived
    ) => visibility switch {
        GeneralVokiResultsVisibility.Anyone => anyone(),
        GeneralVokiResultsVisibility.AfterTaking => afterTaking(),
        GeneralVokiResultsVisibility.OnlyReceived => onlyReceived(),
        _ => throw new ArgumentOutOfRangeException(nameof(visibility), visibility, null)
    };
}