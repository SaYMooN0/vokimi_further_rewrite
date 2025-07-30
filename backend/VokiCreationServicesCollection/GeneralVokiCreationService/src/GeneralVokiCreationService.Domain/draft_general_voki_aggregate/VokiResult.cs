using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokimiStorageKeysLib.draft_general_voki.result_image;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class VokiResult : Entity<GeneralVokiResultId>
{
    private VokiResult() { }
    public VokiResultName Name { get; private set; }
    public VokiResultText Text { get; private set; }
    public DraftGeneralVokiResultImageKey? Image { get; private set; }

    public static VokiResult CreateNew(VokiResultName name) => new() {
        Id = GeneralVokiResultId.CreateNew(),
        Name = name,
        Text = GeneralVokiPresets.GetRandomResultText(),
        Image = null
    };

    public void UpdateName(VokiResultName name) => this.Name = name;
    public void UpdateText(VokiResultText text) => this.Text = text;
    public void SetImage(DraftGeneralVokiResultImageKey image) => this.Image = image;
    public void RemoveImage() => this.Image = null;
}