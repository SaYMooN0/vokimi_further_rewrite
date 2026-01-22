import type { GeneralVokiCreationQuestionContent, GeneralVokiCreationQuestionImageSet } from "./types";
import type { IVokiCreationPageState } from "../../../../voki-creation-page-context";
import type { QuestionAnswersSettings } from "./types";

export class GeneralVokiCreationSpecificQuestionPageState implements IVokiCreationPageState {
    public resultsIdToName: Record<string, string> = $state()!;


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
        resultsIdToName: Record<string, string>
    ) {
        this.savedText = text;
        this.savedImageSet = imageSet;
        this.savedAnswerSettings = answerSettings;
        this.savedTypeSpecificContent = typeSpecificContent;
        this.resultsIdToName = resultsIdToName;
    }

    get hasAnyUnsavedChanges(): boolean {
        return this.isEditingQuestionText
            || this.isEditingQuestionAnswerSettings
            || this.isEditingQuestionTypeSpecificContent;
    }
}
