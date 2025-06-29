using AuthService.Application.unconfirmed_users.commands;

namespace AuthService.Api.endpoints;

public static class RootHandlers
{
    internal static void MapRootHandlers(this IEndpointRouteBuilder endpoints) {
        endpoints.MapPost("/sign-up", RegisterUser)
            .WithRequestValidation<RegisterUserRequest>();
    }

    private static async Task<IResult> RegisterUser(
        HttpContext httpContext, CancellationToken ct, ICommandHandler<RegisterUserCommand> handler
    ) {
        var request = httpContext.GetValidatedRequest<RegisterUserRequest>();

        RegisterUserCommand command = new(request.ParsedUsername, request.ParsedEmail, request.Password);
        var result = await handler.Handle(command, ct);

        return CustomResults.FromErrOrNothing(result, Results.Created);
    }

}