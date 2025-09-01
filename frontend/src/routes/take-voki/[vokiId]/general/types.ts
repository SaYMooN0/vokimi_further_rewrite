import type { GeneralVokiAnswerType } from "$lib/ts/voki";

export type GeneralVokiTakingData = {
    id: string;
    forceSequentialAnswering: boolean;
    questions: GeneralVokiTakingQuestionData[];
    takingSessionId: string;
    startedAd: Date;
}
export type GeneralVokiTakingQuestionData = {
    id: string;
    text: string;
    imageKeys: string[];
    imagesAspectRatio: number;
    answerType: GeneralVokiAnswerType;
    orderInVokiTaking: number;
    minAnswersCount: number;
    maxAnswersCount: number;
    answers: GeneralVokiTakingAnswerData[]
}
export type GeneralVokiTakingAnswerData = {
    id: string;
    typeData: GeneralVokiAnswerTypeData;
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
export type GeneralVokiTakingResultData = {

}