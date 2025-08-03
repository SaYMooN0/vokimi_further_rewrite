import type { GeneralVokiAnswerType } from "$lib/ts/voki";

export type QuestionBriefInfo = {
    id: string;
    text: string;
    images: string[];
    answersType: GeneralVokiAnswerType;
    orderInVoki: number;
    shuffleAnswers: boolean;
    isMultipleChoice: boolean;
}
export type GeneralVokiTakingProcessSettings = {
    forceSequentialAnswering: boolean;
    shuffleQuestions: boolean;
}