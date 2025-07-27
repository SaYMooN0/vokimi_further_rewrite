import type { GeneralVokiAnswerTypeData, GeneralVokiAnswerType } from "$lib/ts/voki";

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
export type QuestionFullInfo = {
    id: string;
    text: string;
    images: string[];
    answersType: GeneralVokiAnswerType;
    answers: QuestionAnswerData[];
    shuffleAnswers: boolean;
    minAnswersCount: number;
    maxAnswersCount: number;
}
export type QuestionAnswerData = {
    id: string;
    orderInQuestion: number;
    typeData: GeneralVokiAnswerTypeData;
}