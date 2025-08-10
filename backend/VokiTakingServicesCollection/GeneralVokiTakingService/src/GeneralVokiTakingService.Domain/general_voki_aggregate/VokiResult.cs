using VokimiStorageKeysLib.general_voki.answer_image;
using VokimiStorageKeysLib.general_voki.result_image;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public class VokiResult : Entity<GeneralVokiResultId>
{
    private VokiResult() { }
    public string Name { get; }
    public string Text { get; }
    public GeneralVokiResultImageKey? Image { get; }

    public VokiResult(GeneralVokiResultId id, string name, string text, GeneralVokiResultImageKey? image) {
        Id = id;
        Name = name;
        Text = text;
        Image = image;
    }
}