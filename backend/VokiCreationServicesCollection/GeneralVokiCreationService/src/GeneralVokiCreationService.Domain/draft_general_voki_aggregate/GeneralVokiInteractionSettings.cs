using SharedKernel.common.vokis.general_vokis;
using SharedKernel.exceptions;
using VokiCreationServicesLib.Domain.draft_voki_aggregate;

namespace GeneralVokiCreationService.Domain.draft_general_voki_aggregate;

public class GeneralVokiInteractionSettings : ValueObject, IVokiInteractionSettings
{
    public bool AuthenticatedOnlyTaking { get; }
    public GeneralVokiResultsVisibility ResultsVisibility { get; }
    public override IEnumerable<object> GetEqualityComponents() => [AuthenticatedOnlyTaking, ResultsVisibility];

    private GeneralVokiInteractionSettings(
        bool authenticatedOnlyTaking,
        GeneralVokiResultsVisibility resultsVisibility
    ) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckResultsVisibilityForErr(
            authenticatedOnlyTaking, resultsVisibility
        ));
        AuthenticatedOnlyTaking = authenticatedOnlyTaking;
        ResultsVisibility = resultsVisibility;
    }

    public static ErrOrNothing CheckResultsVisibilityForErr(
        bool authOnlyAccess,
        GeneralVokiResultsVisibility visibility
    ) => (authOnlyAccess, visibility) switch {
        (_, GeneralVokiResultsVisibility.Anyone)
            or (authOnlyAccess: true, _) => ErrOrNothing.Nothing,


        (authOnlyAccess: false, GeneralVokiResultsVisibility.AfterTaking)
            or (authOnlyAccess: false, GeneralVokiResultsVisibility.OnlyReceived) => ErrFactory.Conflict(
                "This results visibility option cannot be used if the voki can be taken without signing in",
                "Allow to Voki taking for authenticated only users or set results visibility to Anyone"
            ),

        _ => throw new Exception(
            $"Unhandled case in {nameof(CheckResultsVisibilityForErr)} in the {nameof(GeneralVokiInteractionSettings)}. {nameof(authOnlyAccess)}: {authOnlyAccess}, {nameof(visibility)}: {visibility}")
    };

    public static GeneralVokiInteractionSettings Default => new(false, GeneralVokiResultsVisibility.Anyone);

    public static ErrOr<GeneralVokiInteractionSettings> Create(bool authOnlyAccess,
        GeneralVokiResultsVisibility visibility) =>
        CheckResultsVisibilityForErr(authOnlyAccess, visibility).IsErr(out var err)
            ? err
            : new GeneralVokiInteractionSettings(authOnlyAccess, visibility);
}