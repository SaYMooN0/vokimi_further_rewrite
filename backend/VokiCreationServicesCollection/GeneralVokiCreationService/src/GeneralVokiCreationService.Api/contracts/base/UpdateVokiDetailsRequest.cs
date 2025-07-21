using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Api.contracts.@base;

public class UpdateVokiDetailsRequest : IRequestWithValidationNeeded
{
    public string NewDescription { get; init; }
    public Language NewLanguage { get; init; }
    public bool NewIsAgeRestricted { get; init; }
    public ErrOrNothing Validate() => VokiDescription.CheckForErr(NewDescription);

    public VokiDetails ParsedDetails => new(
        VokiDescription.Create(NewDescription).AsSuccess(),
        NewIsAgeRestricted,
        NewLanguage
    );
}