using ApiShared;
using SharedKernel.errs;

namespace AlbumsService.Api.contracts;

public class UpdateVokiEntriesInAlbumsRequest : IRequestWithValidationNeeded
{
    public string[] AlbumIds { get; init; }
    public ErrOrNothing Validate() => throw new NotImplementedException();
}