using GeneralVokiCreationService.Domain.draft_general_voki_aggregate.results;

namespace GeneralVokiCreationService.Api.contracts.results;

public class UpdateResultNameRequest: IRequestWithValidationNeeded 
{
    public string NewName { get; init; }
    public ErrOrNothing Validate() => VokiResultName.CheckForErr(NewName);
    public VokiResultName ParsedName => VokiResultName.Create(NewName).AsSuccess();
}