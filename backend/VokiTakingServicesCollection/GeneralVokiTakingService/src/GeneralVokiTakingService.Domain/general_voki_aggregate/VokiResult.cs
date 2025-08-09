using VokimiStorageKeysLib.published_general_voki.result_image;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public class VokiResult : Entity<GeneralVokiResultId>
{
    private VokiResult() { }
    public string Name { get; }
    public string Text { get; }
    public PublishedGeneralVokiResultImageKey? Image { get; }

    public VokiResult(GeneralVokiResultId id, string name, string text, PublishedGeneralVokiResultImageKey? image) {
        Id = id;
        Name = name;
        Text = text;
        Image = image;
    }
}