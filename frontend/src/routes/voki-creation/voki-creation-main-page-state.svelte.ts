import type { VokiDetails } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { IVokiCreationPageState } from "./voki-creation-page-context";

export abstract class VokiCreationMainPageState implements IVokiCreationPageState {
    protected readonly savedName: string;
    protected readonly savedCover: string;
    public savedTags: Set<string>;
    protected readonly savedDetails: VokiDetails;

    currentName: string;
    currentCover: string;
    currentDetails: VokiDetails;

    constructor(
        name: string,
        cover: string,
        tags: string[],
        details: VokiDetails,
    ) {
        this.savedName = $state<string>(name);
        this.savedCover = $state<string>(cover);
        this.savedTags = $state(new Set());
        this.savedDetails = $state<VokiDetails>(details);

        this.currentName = $state<string>(name);
        this.currentCover = $state<string>(cover);
        this.currentDetails = $state<VokiDetails>(details);
    }
    abstract get hasAnyUnsavedChanges(): boolean;

    protected detailsAreUnsaved(): boolean {
        return this.currentDetails!.description !== this.savedDetails.description
            || this.currentDetails!.language !== this.savedDetails.language
            || this.currentDetails!.hasMatureContent !== this.savedDetails.hasMatureContent;
    }
}
