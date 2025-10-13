﻿using UserProfilesService.Api.contracts;
using UserProfilesService.Application.app_users.commands;
using UserProfilesService.Application.app_users.queries;
using UserProfilesService.Domain.app_user_aggregate;

namespace UserProfilesService.Api.endpoints;

internal static class RootHandlers
{
    internal static void MapRootHandlers(this IEndpointRouteBuilder endpoints) {
        var group = endpoints.MapGroup("/");

        group.MapGet("/basic-setup-info", GetUserBasicSetupInfo)
            .WithAuthenticationRequired();

        group.MapPost("/save-basic-setup", SaveBasicProfileSetup)
            .WithRequestValidation<SaveBasicProfileSetupRequest>()
            .WithAuthenticationRequired();
    }

    private static async Task<IResult> GetUserBasicSetupInfo(
        CancellationToken ct, HttpContext httpContext,
        IQueryHandler<GetCurrentUserQuery, AppUser> handler
    ) {
        GetCurrentUserQuery query = new();
        var result = await handler.Handle(query, ct);

        return CustomResults.FromErrOrToJson<AppUser, UserBasicProfileSetupInfoResponse>(result);
    }

    private static async Task<IResult> SaveBasicProfileSetup(
        HttpContext httpContext, CancellationToken ct,
        ICommandHandler<SaveBasicProfileSetupCommand> handler
    ) {
        var request = httpContext.GetValidatedRequest<SaveBasicProfileSetupRequest>();

        SaveBasicProfileSetupCommand command = new();
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, () => Results.Ok());
    }
}