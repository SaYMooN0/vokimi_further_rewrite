import type { GeneralVokiResultsVisibility } from "$lib/ts/voki";

export type GeneralVokiInteractionSettings = {
    signedInOnlyTaking : boolean;
    resultsVisibility : GeneralVokiResultsVisibility;
    showResultsDistribution : boolean;
}
