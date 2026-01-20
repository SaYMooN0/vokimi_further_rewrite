import type { IVokiCreationPageState } from "../../../voki-creation-page-context";

export class GeneralVokiCreationAllQuestionsPageState implements IVokiCreationPageState {
    get hasAnyUnsavedChanges(): boolean {
        return false;
    }
    // maxQuestionsCount
}
