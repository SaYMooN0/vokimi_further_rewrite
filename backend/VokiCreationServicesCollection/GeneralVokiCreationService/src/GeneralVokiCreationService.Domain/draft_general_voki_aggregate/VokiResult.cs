using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;
using VokimiStorageKeysLib.draft_general_voki.result_image;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class VokiResult : Entity<GeneralVokiResultId>
{
    private VokiResult() { }
    public VokiResultName Name { get; private set; }
    public VokiResultText Text { get; private set; }
    public DraftGeneralVokiResultImageKey? Image { get; private set; }
    public DateTime? CreationDate { get; }

    private VokiResult(VokiResultName name, DateTime dateTime) {
        Id = GeneralVokiResultId.CreateNew();
        Name = name;
        Text = GeneralVokiPresets.GetRandomResultText();
        Image = null;
        CreationDate = dateTime;
    }

    public static VokiResult CreateNew(VokiResultName name, DateTime dateTime) => new(name, dateTime);

    public void UpdateName(VokiResultName name) => this.Name = name;
    public void UpdateText(VokiResultText text) => this.Text = text;

    public ErrOrNothing SetImage(DraftGeneralVokiResultImageKey key) {
        if (key.ResultId != Id) {
            return ErrFactory.Conflict(
                "Provided image does not belong to the specified result",
                $"Result id: {Id}, key: {key}"
            );
        }

        Image = key;
        return ErrOrNothing.Nothing;
    }

    public void RemoveImage() => this.Image = null;
}