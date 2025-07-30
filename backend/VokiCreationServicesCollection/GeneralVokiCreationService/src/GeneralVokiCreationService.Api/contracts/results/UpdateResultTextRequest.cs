using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;

namespace GeneralVokiCreationService.Api.contracts.results;

public class UpdateResultTextRequest: IRequestWithValidationNeeded 
{
    public string NewText { get; init; }
    public ErrOrNothing Validate() => VokiResultText.CheckForErr(NewText);
    public VokiResultText ParsedText => VokiResultText.Create(NewText).AsSuccess();
}