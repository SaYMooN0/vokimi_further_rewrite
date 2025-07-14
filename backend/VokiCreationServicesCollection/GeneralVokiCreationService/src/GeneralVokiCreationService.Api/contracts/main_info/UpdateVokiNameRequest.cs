using SharedKernel.common.vokis;

namespace GeneralVokiCreationService.Api.contracts.main_info;

public class UpdateVokiNameRequest : IRequestWithValidationNeeded
{
    public string NewVokiName { get; init; }
    public ErrOrNothing Validate() => VokiName.CheckForErr(NewVokiName);

    public VokiName ParsedVokiName => new(NewVokiName);
}