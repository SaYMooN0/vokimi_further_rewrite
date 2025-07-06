using Microsoft.AspNetCore.Mvc;
using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Api.endpoints;

public static class SpecificVokiHandlers
{
    internal static void MapSpecificVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/draft-vokis/{draft-voki-id}/");

        group.WithGroupAuthenticationRequired();

        // group.MapPut("/invite-co-author", InviteCoAuthor)
        //     .WithRequestValidation<InviteCoAuthorRequest>();
        group.MapGet("/brief-info", GetVokiBriefData);
        // group.MapGet("/view-data-for-co-author-invited", GetVokiViewDataForCoAuthorInvitedUser); 
    }

    private static async Task<IResult> GetVokiBriefData(
    ) {
        return Results.Json(new {
            Id = "dsm",
            Name = Guid.NewGuid().ToString(),
            PrimaryAuthorId = Guid.NewGuid().ToString(),
            Type = VokiType.General
        });
    }
}