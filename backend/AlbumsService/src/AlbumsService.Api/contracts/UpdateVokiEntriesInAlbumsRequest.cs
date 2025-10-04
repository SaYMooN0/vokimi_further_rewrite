using ApiShared;
using SharedKernel.domain.ids;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace AlbumsService.Api.contracts;

public class UpdateVokiEntriesInAlbumsRequest : IRequestWithValidationNeeded
{
    private const int MaxCount = 200;
    public Dictionary<string, bool> AlbumIdToEntry { get; init; }

    public ErrOrNothing Validate() {
        if (AlbumIdToEntry is null || AlbumIdToEntry.Count == 0) {
            return ErrFactory.NoValue.Common("Voki albums entry is not provided");
        }

        if (AlbumIdToEntry.Count >= MaxCount) {
            return ErrFactory.NoValue.Common("Voki albums entry is too large");
        }

        ParsedAlbumIdToEntry = AlbumIdToEntry
            .Where(kvp => Guid.TryParse(kvp.Key, out _))
            .ToDictionary(kvp => new VokiAlbumId(new(kvp.Key)), kvp => kvp.Value);
        if (ParsedAlbumIdToEntry.Count != AlbumIdToEntry.Count) {
            return ErrFactory.IncorrectFormat("Some of provided Album ids are incorrect");
        }

        return ErrOrNothing.Nothing;
    }

    public Dictionary<VokiAlbumId, bool> ParsedAlbumIdToEntry { get; private set; }
}