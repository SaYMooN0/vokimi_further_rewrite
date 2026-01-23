import type { GeneralVokiCreationQuestionContent, GeneralVokiCreationQuestionImageSet } from "./types";
import type { IVokiCreationPageState } from "../../../../voki-creation-page-context";
import type { QuestionAnswersSettings } from "./types";
import { ApiVokiCreationGeneral } from "$lib/ts/backend-communication/voki-creation-backend-service";

export class GeneralVokiCreationSpecificQuestionPageState implements IVokiCreationPageState {
    public resultsIdToName: Record<string, string> = $state()!;
    public readonly vokiId: string;

    public savedText: string = $state()!;
    public savedImageSet: GeneralVokiCreationQuestionImageSet = $state()!;
    public savedAnswerSettings: QuestionAnswersSettings = $state()!;
    public savedTypeSpecificContent: GeneralVokiCreationQuestionContent = $state()!;

    public isEditingQuestionText = $state(false);
    public isEditingQuestionAnswerSettings = $state(false);
    public isEditingQuestionTypeSpecificContent = $state(false);

    constructor(
        text: string,
        imageSet: GeneralVokiCreationQuestionImageSet,
        answerSettings: QuestionAnswersSettings,
        typeSpecificContent: GeneralVokiCreationQuestionContent,
        vokiId: string,
        resultsIdToName: Record<string, string>,
    ) {
        this.savedText = text;
        this.savedImageSet = imageSet;
        this.savedAnswerSettings = answerSettings;
        this.savedTypeSpecificContent = typeSpecificContent;
        this.vokiId = vokiId;
        this.resultsIdToName = resultsIdToName;
    }

    get hasAnyUnsavedChanges(): boolean {
        return this.isEditingQuestionText
            || this.isEditingQuestionAnswerSettings
            || this.isEditingQuestionTypeSpecificContent;
    }
    async fetchResultNames() {
        // allResults = {state: 'loading'};
        const response = await ApiVokiCreationGeneral.fetchJsonResponse<{
            results: Record<string, string>;
        }>(`/vokis/${this.vokiId}/results/ids-names`, { method: 'GET' });

        if (response.isSuccess) {
            this.resultsIdToName = response.data.results;
        } else {
            // errs = response.errs;
        }
    }
}
