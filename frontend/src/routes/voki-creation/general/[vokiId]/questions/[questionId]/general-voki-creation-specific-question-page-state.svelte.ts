import type { GeneralVokiCreationQuestionImageSet } from "./types";
import type { IVokiCreationPageState } from "../../../../voki-creation-page-context";
import type { QuestionAnswersSettings } from "./types";

export class GeneralVokiCreationSpecificQuestionPageState implements IVokiCreationPageState {
    public savedText: string = $state()!;
    public savedImageSet: GeneralVokiCreationQuestionImageSet = $state()!;
    public savedAnswerSettings: QuestionAnswersSettings = $state()!;

    public isEditingQuestionText = $state(false);
    public isEditingQuestionImages = $state(false);
    public isEditingQuestionAnswerSettings = $state(false);

    constructor(
        text: string,
        imageSet: GeneralVokiCreationQuestionImageSet,
        answerSettings: QuestionAnswersSettings
    ) {
        this.savedText = text;
        this.savedImageSet = imageSet;
        this.savedAnswerSettings = answerSettings;
    }

    get hasAnyUnsavedChanges(): boolean {
        return this.isEditingQuestionText
            || this.isEditingQuestionImages
            || this.isEditingQuestionAnswerSettings;
    }
}
