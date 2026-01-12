import type { IVokiCreationPageState } from "../../../ivoki-creation-page-state";

export class GeneralVokiCreationQuestionsPageState implements IVokiCreationPageState {
    get hasAnyUnsavedChanges(): boolean {
        return false;
    }
}
