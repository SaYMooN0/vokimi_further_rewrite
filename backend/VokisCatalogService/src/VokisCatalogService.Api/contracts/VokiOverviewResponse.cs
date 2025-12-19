using System.Text.Json.Serialization;
using SharedKernel.common.vokis;
using VokisCatalogService.Domain.voki_aggregate;

namespace VokisCatalogService.Api.contracts;

public record class VokiOverviewResponse(
    string Id,
    VokiType Type,
    string Name,
    string Cover,
    string PrimaryAuthorId,
    string[] CoAuthorIds,
    string[] ManagerIds,
    Language Language,
    string Description,
    string[] Tags,
    bool HasMatureContent,
    DateTime PublicationDate,
    uint RatingsCount,
    uint CommentsCount,
    bool SignedInOnlyTaking,
    VokiOverviewResponse.VokiTypeWithSpecificDataResponse TypeWithSpecificData
)
{
    public static VokiOverviewResponse FromBaseVoki(BaseVoki v) => new(
        v.Id.ToString(),
        v.Type,
        v.Name.ToString(),
        v.Cover.ToString(),
        PrimaryAuthorId: v.PrimaryAuthorId.ToString(),
        CoAuthorIds: v.CoAuthorIds.Select(id => id.ToString()).ToArray(),
        ManagerIds: v.ManagersSet.ToArray().Select(id => id.ToString()).ToArray(),
        v.Details.Language,
        v.Details.Description,
        v.Tags.Select(t => t.ToString()).ToArray(),
        v.Details.HasMatureContent,
        v.PublicationDate,
        v.RatingsCount,
        v.CommentsCount,
        v.BaseInteractionSettings.SignedInOnlyTaking,
        CreateTypeSpecificData(v)
    );

    private static VokiTypeWithSpecificDataResponse CreateTypeSpecificData(
        BaseVoki v
    ) => v.MatchOnType<VokiTypeWithSpecificDataResponse>(
        (g) => new GeneralVokiTypeWithSpecificDataResponse(
            ForceSequentialAnswering: false,
            ShuffleQuestions: false,
            AnyAudioAnswers: g.AnyAudios
        ),
        (t) => new TierListVokiTypeWithSpecificDataResponse(),
        (s) => new ScoringVokiTypeWithSpecificDataResponse()
    );

    [JsonDerivedType(typeof(GeneralVokiTypeWithSpecificDataResponse), nameof(GeneralVokiTypeWithSpecificDataResponse))]
    [JsonDerivedType(typeof(TierListVokiTypeWithSpecificDataResponse), nameof(TierListVokiTypeWithSpecificDataResponse))]
    [JsonDerivedType(typeof(ScoringVokiTypeWithSpecificDataResponse), nameof(ScoringVokiTypeWithSpecificDataResponse))]
    public abstract record VokiTypeWithSpecificDataResponse;


    public record GeneralVokiTypeWithSpecificDataResponse(
        bool ForceSequentialAnswering,
        bool ShuffleQuestions,
        bool AnyAudioAnswers
    ) : VokiTypeWithSpecificDataResponse();

    public record TierListVokiTypeWithSpecificDataResponse() : VokiTypeWithSpecificDataResponse();

    public record ScoringVokiTypeWithSpecificDataResponse() : VokiTypeWithSpecificDataResponse();
}