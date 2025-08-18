namespace VokimiStorageKeysLib;

public class TempKey
{
    private TempKey(string randomPart, string extension) {
        FullValue = "temp/" + randomPart + extension;
        Extension = extension;
    }

    public string FullValue { get; }
    public string Extension { get; }

    public static TempKey Create(string extension) => new(
        Guid.NewGuid().ToString(),
        extension
    );
}