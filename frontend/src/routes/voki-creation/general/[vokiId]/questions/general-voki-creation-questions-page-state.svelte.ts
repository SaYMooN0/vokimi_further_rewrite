import type { IVokiCreationPageState } from "../../../voki-creation-page-context";

export class GeneralVokiCreationQuestionsPageState implements IVokiCreationPageState {
    get hasAnyUnsavedChanges(): boolean {
        return false;
    }
}
