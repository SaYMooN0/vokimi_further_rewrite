﻿using SharedKernel.common.vokis.general_vokis;
using SharedKernel.exceptions;
using VokiTakingServicesLib.Domain.base_voki_aggregate;

namespace GeneralVokiTakingService.Domain.general_voki_aggregate;

public class GeneralVokiInteractionSettings : ValueObject, IVokiInteractionSettings
{
    public bool SignedInOnlyTaking { get; }
    public GeneralVokiResultsVisibility ResultsVisibility { get; }
    public bool ShowResultsDistribution { get; }


    public override IEnumerable<object> GetEqualityComponents() =>
        [SignedInOnlyTaking, ResultsVisibility, ShowResultsDistribution];

    private GeneralVokiInteractionSettings(
        bool signedInOnlyTaking,
        GeneralVokiResultsVisibility resultsVisibility,
        bool showResultsDistribution
    ) {
        InvalidConstructorArgumentException.ThrowIfErr(this, CheckResultsVisibilityForErr(
            signedInOnlyTaking, resultsVisibility
        ));
        SignedInOnlyTaking = signedInOnlyTaking;
        ResultsVisibility = resultsVisibility;
        ShowResultsDistribution = showResultsDistribution;
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

    public static GeneralVokiInteractionSettings Default => new(
        false,
        GeneralVokiResultsVisibility.Anyone,
        true
    );

    public static ErrOr<GeneralVokiInteractionSettings> Create(
        bool authOnlyAccess,
        GeneralVokiResultsVisibility visibility,
        bool showResultsDistribution
    ) =>
        CheckResultsVisibilityForErr(authOnlyAccess, visibility).IsErr(out var err)
            ? err
            : new GeneralVokiInteractionSettings(authOnlyAccess, visibility, showResultsDistribution);
}