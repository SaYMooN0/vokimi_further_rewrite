using VokimiStorageKeysLib.draft_general_voki.result_image;

namespace GeneralVokiCreationService.Api.contracts.results;

public class UpdateResultImagesRequest : IRequestWithValidationNeeded
{
    public string? Image { get; init; }

    public ErrOrNothing Validate() {
        if (Image is null) {
            ImageKey = null;
            return ErrOrNothing.Nothing;
        }

        var creationRes = DraftGeneralVokiResultImageKey.FromString(Image);
        if (creationRes.IsErr()) {
            return ErrFactory.IncorrectFormat(
                "Value fot the image key is provided but the value is invalid",
                $"Fix the value or remove it completely. Provided value: {Image} "
            );
        }

        ImageKey = creationRes.AsSuccess();
        return ErrOrNothing.Nothing;
    }

    public DraftGeneralVokiResultImageKey? ImageKey { get; private set; }
}