using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Api.extensions;

public static class HttpContextExtensions
{
    public static GeneralVokiQuestionId GetQuestionIdFromRoute(this HttpContext context) {
        var idString = context.Request.RouteValues["questionId"]?.ToString() ?? "";
        if (!Guid.TryParse(idString, out var guid)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                    "Invalid question id",
                    $"'{idString}' is not a valid ${nameof(GeneralVokiQuestionId)}"
                ),
                userMessage: "Invalid question id. Couldn't parse question id from route"
            );
        }

        return new GeneralVokiQuestionId(guid);
    }
}