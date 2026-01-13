import type { VokiDetails } from "$lib/ts/backend-communication/voki-creation-backend-service";
import type { IVokiCreationPageState } from "./voki-creation-page-context";

export abstract class VokiCreationMainPageState implements IVokiCreationPageState {
    public savedName: string;
    public savedCover: string;
    public savedTags: Set<string>;
    public savedDetails: VokiDetails;
    constructor(
        name: string,
        cover: string,
        tags: string[],
        details: VokiDetails,
    ) {
        this.savedName = $state<string>(name);
        this.savedCover = $state<string>(cover);
        this.savedTags = $state(new Set(tags));
        this.savedDetails = $state<VokiDetails>(details);
    }
    public isNameEditing = $state<boolean>(false);
    public isDetailsEditing = $state<boolean>(false);

    abstract get hasAnyUnsavedChanges(): boolean;
}
