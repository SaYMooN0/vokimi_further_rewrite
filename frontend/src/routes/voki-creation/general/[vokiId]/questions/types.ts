import type { AnswerTypeData, GeneralVokiAnswerType } from "$lib/ts/voki";

export type QuestionBriefData = {
    id: string;
    text: string;
    images: string[];
    answersType: GeneralVokiAnswerType;
    orderInVoki: number;
    isMultipleChoice: boolean;
}
export type GeneralVokiTakingProcessSettings = {
    forceSequentialAnswering: boolean;
    shuffleQuestions: boolean;
}
export type QuestionFullData = {
    id: string;
    text: string;
    images: string[];
    answersType: GeneralVokiAnswerType;
    answers: QuestionAnswerData[];
    minAnswersCount: number;
    maxAnswersCount: number;
}
export type QuestionAnswerData = {
    id: string;
    orderInQuestion: number;
    typeData: AnswerTypeData;
}
