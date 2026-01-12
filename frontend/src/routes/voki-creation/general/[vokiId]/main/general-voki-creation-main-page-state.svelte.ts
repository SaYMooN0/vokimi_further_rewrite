import type { VokiDetails } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { GeneralVokiResultsVisibility } from "$lib/ts/voki";
import { VokiCreationMainPageState } from "../../../voki-creation-main-page-state.svelte";
import type { GeneralVokiInteractionSettings } from "./types";

export class GeneralVokiCreationMainPageState extends VokiCreationMainPageState {
    public currentInteractionSettings: GeneralVokiInteractionSettings;
    public savedInteractionSettings: GeneralVokiInteractionSettings;

    constructor(
        name: string,
        cover: string,
        tags: string[],
        details: VokiDetails,
        interactionSettings: GeneralVokiInteractionSettings,
    ) {
        super(name, cover, tags, details);
        this.currentInteractionSettings = $state(interactionSettings);
        this.savedInteractionSettings = $state(interactionSettings);
    }
    get hasAnyUnsavedChanges(): boolean {
        return this.detailsAreUnsaved()
            || this.interactionSettingsAreUnsaved();
    }
    interactionSettingsAreUnsaved(): boolean {
        return this.currentInteractionSettings!.signedInOnlyTaking !== this.savedInteractionSettings!.signedInOnlyTaking
            || this.currentInteractionSettings!.resultsVisibility !== this.savedInteractionSettings!.resultsVisibility
            || this.currentInteractionSettings!.showResultsDistribution !== this.savedInteractionSettings!.showResultsDistribution;
    }

}