using AlbumsService.Application.app_users.queries;
using AlbumsService.Application.common.repositories;
using ApiShared;

namespace AlbumsService.Api.contracts;

public record class AllAlbumsPreviewResponse(
    AutoAlbumsAppearanceResponse AutoAlbumsAppearance,
    VokiAlbumPreviewResponse[] Albums
) : ICreatableResponse<ListAllUserAlbumsPreviewQueryResult>
{
    public static ICreatableResponse<ListAllUserAlbumsPreviewQueryResult> Create(
        ListAllUserAlbumsPreviewQueryResult res
    ) => new AllAlbumsPreviewResponse(
        AutoAlbumsAppearanceResponse.FromUserAppearance(res.AutoAlbumsAppearance),
        res.Albums.Select(VokiAlbumPreviewResponse.FromAlbum).ToArray()
    );
}

public record VokiAlbumPreviewResponse(
    string Id,
    string Name,
    string Icon,
    string MainColor,
    string SecondaryColor,
    int VokisCount
)
{
    public static VokiAlbumPreviewResponse FromAlbum(VokiAlbumPreviewDto a) => new(
        a.Id.ToString(),
        a.Name.ToString(),
        a.Icon.ToString(),
        a.MainColor.ToString(),
        a.SecondaryColor.ToString(),
        a.VokiIdsCount
    );
}