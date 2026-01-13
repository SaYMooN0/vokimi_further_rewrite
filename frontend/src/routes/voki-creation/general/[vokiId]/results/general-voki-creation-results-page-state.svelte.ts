import type { IVokiCreationPageState } from "../../../voki-creation-page-context";

export class GeneralVokiCreationResultsPageState implements IVokiCreationPageState {
    get hasAnyUnsavedChanges(): boolean {
        return true;
    }
}
