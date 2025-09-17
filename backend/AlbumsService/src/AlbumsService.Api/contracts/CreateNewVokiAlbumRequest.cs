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

    public AlbumName ParsedName => AlbumName.Create(Name).AsSuccess();
    public AlbumIcon ParsedIcon => AlbumIcon.Create(Icon).AsSuccess();
    public HexColor ParsedMainColor=> HexColor.Create(MainColor).AsSuccess();
    public HexColor ParsedSecondColor => HexColor.Create(SecondColor).AsSuccess();
}