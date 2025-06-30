using AuthService.Application.unconfirmed_users.commands;
using SharedKernel.auth;

namespace AuthService.Api.endpoints;

public static class RootHandlers
{
    internal static void MapRootHandlers(this IEndpointRouteBuilder endpoints) {
        // endpoints.MapPost("/ping", PingAuth);
        endpoints.MapPost("/sign-up", RegisterUser)
            .WithRequestValidation<RegisterUserRequest>();
        // endpoints.MapPost("/login", LoginUser)
        //     .WithRequestValidation<LoginUserRequest>();
        // endpoints.MapPost("/confirm-registration", ConfirmUserRegistration)
        //     .WithRequestValidation<ConfirmRegistrationRequest>();
        // endpoints.MapPost("/logout", LogOutUser);
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