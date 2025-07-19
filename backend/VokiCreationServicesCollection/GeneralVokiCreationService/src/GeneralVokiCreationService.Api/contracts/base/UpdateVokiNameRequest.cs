using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Api.contracts.main_info;

public class UpdateVokiNameRequest : IRequestWithValidationNeeded
{
    public string NewName { get; init; }
    public ErrOrNothing Validate() => VokiName.CheckForErr(NewName);

    public VokiName ParsedVokiName => new(NewName);
}