import type { IVokiCreationPageState } from "../../../../voki-creation-page-context";

export class GeneralVokiCreationSpecificQuestionPageState implements IVokiCreationPageState {
    get hasAnyUnsavedChanges(): boolean {
        return false;
    }
}
