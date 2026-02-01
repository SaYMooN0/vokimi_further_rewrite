import type { GeneralVokiQuestionContentType } from "$lib/ts/voki";

export type GeneralVokiTakingData = {
    id: string;
    vokiName: string;
    forceSequentialAnswering: boolean;
    questions: GeneralVokiTakingQuestionData[];
    sessionId: string;
    startedAt: Date;
    totalQuestionsCount: number;
}
export type GeneralVokiTakingQuestionData = {
    id: string;
    text: string;
    imageKeys: string[];
    imagesAspectRatio: number;
    answerType: GeneralVokiQuestionContentType;
    orderInVokiTaking: number;
    minAnswersCount: number;
    maxAnswersCount: number;
}
export type GeneralVokiTakingAnswerData = {
    id: string;
}

export type GeneralVokiAnswerTextOnly = { text: string; };
export type GeneralVokiAnswerImageOnly = { image: string; };
export type GeneralVokiAnswerImageAndText = { image: string; text: string; };
export type GeneralVokiAnswerColorOnly = { color: string; };
export type GeneralVokiAnswerColorAndText = { color: string; text: string; };
export type GeneralVokiAnswerAudioOnly = { audio: string; };
export type GeneralVokiAnswerAudioAndText = { audio: string; text: string; };

export type GeneralVokiAnswerTypeData =
    | GeneralVokiAnswerTextOnly
    | GeneralVokiAnswerImageOnly
    | GeneralVokiAnswerImageAndText
    | GeneralVokiAnswerColorOnly
    | GeneralVokiAnswerColorAndText
    | GeneralVokiAnswerAudioOnly
    | GeneralVokiAnswerAudioAndText;
