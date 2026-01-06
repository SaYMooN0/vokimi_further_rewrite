namespace ApiShared.EfCore;

internal sealed class DisableConsistencyFilterMetadata
{
    public static readonly DisableConsistencyFilterMetadata Instance = new();
    private DisableConsistencyFilterMetadata() { }
}