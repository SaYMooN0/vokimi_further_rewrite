using SharedKernel.exceptions;

namespace GeneralVokiCreationService.Api.extensions;

public static class HttpContextExtensions
{
  
    public static GeneralVokiQuestionId GetQuestionIdFromRoute(this HttpContext context) {
        var questionIdString = context.Request.RouteValues["questionId"]?.ToString() ?? "";
        if (!Guid.TryParse(questionIdString, out var guid)) {
            UnexpectedBehaviourException.ThrowErr(ErrFactory.IncorrectFormat(
                "Invalid question id",
                "Couldn't parse question id from route"
            ));
        }

        return new GeneralVokiQuestionId(guid);
    }
}