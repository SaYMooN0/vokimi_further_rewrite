using AlbumsService.Application.common.repositories;
using AlbumsService.Application.voki_albums.queries;
using AlbumsService.Domain.app_user_aggregate;
using AlbumsService.Domain.voki_album_aggregate;
using ApiShared;

namespace AlbumsService.Api.contracts;

public record class AllAlbumsPreviewResponse(
    AutoAlbumsColorsPairResponse TakenVokisAlbums,
    AutoAlbumsColorsPairResponse RatedVokisAlbums,
    AutoAlbumsColorsPairResponse CommentedVokisAlbums,
    VokiAlbumPreviewResponse[] Albums
) : ICreatableResponse<GetAllUserAlbumsPreviewQueryResult>
{
    public static ICreatableResponse<GetAllUserAlbumsPreviewQueryResult> Create(GetAllUserAlbumsPreviewQueryResult res)
        => new AllAlbumsPreviewResponse(
            AutoAlbumsColorsPairResponse.TakenVokisAlbums(res.AutoAlbumsAppearance),
            AutoAlbumsColorsPairResponse.RatedVokisAlbums(res.AutoAlbumsAppearance),
            AutoAlbumsColorsPairResponse.CommentedVokisAlbums(res.AutoAlbumsAppearance),
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

public record AutoAlbumsColorsPairResponse(string MainColor, string SecondaryColor)
{
    public static AutoAlbumsColorsPairResponse TakenVokisAlbums(UserAutoAlbumsAppearance data) =>
        new(data.TakenMainColor.ToString(), data.TakenSecondaryColor.ToString());

    public static AutoAlbumsColorsPairResponse RatedVokisAlbums(UserAutoAlbumsAppearance data) =>
        new(data.RatedMainColor.ToString(), data.RatedSecondaryColor.ToString());

    public static AutoAlbumsColorsPairResponse CommentedVokisAlbums(UserAutoAlbumsAppearance data) =>
        new(data.CommentedMainColor.ToString(), data.CommentedSecondaryColor.ToString());
}