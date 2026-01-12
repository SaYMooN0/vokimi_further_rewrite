import type { IVokiCreationPageState } from "../../../ivoki-creation-page-state";

export class GeneralVokiCreationAuthorsPageState implements IVokiCreationPageState {
    get hasAnyUnsavedChanges(): boolean {
        return false;
    }
}
