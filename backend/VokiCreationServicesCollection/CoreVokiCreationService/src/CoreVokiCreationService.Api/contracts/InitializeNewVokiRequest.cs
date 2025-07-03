using SharedKernel.common.vokis;

namespace CoreVokiCreationService.Api.contracts;

public class InitializeNewVokiRequest : IRequestWithValidationNeeded
{
    public string NewVokiName { get; init; }
    public VokiType VokiType { get; init; }
    public ErrOrNothing Validate() => VokiName.CheckForErr(NewVokiName);

    public VokiName ParseVokiName => VokiName.Create(NewVokiName).AsSuccess();
}