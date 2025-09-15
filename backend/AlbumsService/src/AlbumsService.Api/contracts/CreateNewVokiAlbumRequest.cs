using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;
using SharedKernel.common;
using SharedKernel.errs;

namespace AlbumsService.Api.contracts;

public class CreateNewVokiAlbumRequest : IRequestWithValidationNeeded
{
    private string Name { get; init; }
    private string Icon { get; init; }
    private string MainColor { get; init; }
    private string SecondColor { get; init; }

    public ErrOrNothing Validate() =>
        AlbumName.CheckForErr(Name)
            .WithNextIfErr(AlbumIcon.CheckForErr(Icon))
            .WithNextIfErr(HexColor.CheckHexColorForErr(MainColor))
            .WithNextIfErr(HexColor.CheckHexColorForErr(SecondColor));

    public AlbumName ParsedName { get; private set; }
    public AlbumIcon ParsedIcon { get; private set; }
    public HexColor ParsedMainColor { get; private set; }
    public HexColor ParsedSecondColor { get; private set; }
}