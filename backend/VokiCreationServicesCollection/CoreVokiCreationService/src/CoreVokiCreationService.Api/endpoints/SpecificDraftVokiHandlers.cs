using CoreVokiCreationService.Api.contracts;

namespace CoreVokiCreationService.Api.endpoints;

public static class SpecificDraftVokiHandlers
{
    internal static void MapSpecificDraftVokiHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/draft-vokis/{draft-voki-id}/");

        group.WithGroupAuthenticationRequired();

        // group.MapPut("/invite-co-author", InviteCoAuthor)
        //     .WithRequestValidation<InviteCoAuthorRequest>();
        // group.MapGet("/view-data", GetVokiViewData); 
        // group.MapGet("/view-data-for-co-author-invited", GetVokiViewDataForCoAuthorInvitedUser); ?
    }
}