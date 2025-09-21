import type { GeneralVokiAnswerType } from "$lib/ts/voki";

export type QuestionFullInfo = {
    id: string;
    text: string;
    imageSet: GeneralVokiCreationQuestionImageSet;
    answersType: GeneralVokiAnswerType;
    answers: QuestionAnswerData[];
    shuffleAnswers: boolean;
    minAnswersCount: number;
    maxAnswersCount: number;
    results: ResultIdWithName[]
}
export type QuestionAnswerData = {
    id: string;
    order: number;
    typeData: GeneralVokiCreationAnswerData;
    relatedResultIds: string[]
}

export type ResultIdWithName = {
    id: string;
    name: string;
}
export type GeneralVokiCreationQuestionImageSet = {
    width: number;
    height: number;
    keys: string[]
}
export function createEmptyGeneralVokiAnswerTypeData(type: GeneralVokiAnswerType): GeneralVokiCreationAnswerData {
    switch (type) {
        case 'TextOnly':
            return { type: 'TextOnly', relatedResultIds: [], text: '' };
        case 'ImageOnly':
            return { type: 'ImageOnly', relatedResultIds: [], image: '' };
        case 'ImageAndText':
            return { type: 'ImageAndText', relatedResultIds: [], image: '', text: '' };
        case 'ColorOnly':
            return { type: 'ColorOnly', relatedResultIds: [], color: '#bfbfbf' };
        case 'ColorAndText':
            return { type: 'ColorAndText', relatedResultIds: [], color: '#bfbfbf', text: '' };
        case 'AudioOnly':
            return { type: 'AudioOnly', relatedResultIds: [], audio: '' };
        case 'AudioAndText':
            return { type: 'AudioAndText', relatedResultIds: [], audio: '', text: '' };
    }
}

export type GeneralVokiCreationAnswerData =
    | AnswerDataTextOnly
    | AnswerDataImageOnly
    | AnswerDataImageAndText
    | AnswerDataColorOnly
    | AnswerDataColorAndText
    | AnswerDataAudioOnly
    | AnswerDataAudioAndText;

export type AnswerDataTextOnly = { type: 'TextOnly'; relatedResultIds: string[]; text: string; };
export type AnswerDataImageOnly = { type: 'ImageOnly'; relatedResultIds: string[]; image: string };
export type AnswerDataImageAndText = { type: 'ImageAndText'; relatedResultIds: string[]; image: string; text: string };
export type AnswerDataColorOnly = { type: 'ColorOnly'; relatedResultIds: string[]; color: string };
export type AnswerDataColorAndText = { type: 'ColorAndText'; relatedResultIds: string[]; color: string; text: string };
export type AnswerDataAudioOnly = { type: 'AudioOnly'; relatedResultIds: string[]; audio: string };
export type AnswerDataAudioAndText = { type: 'AudioAndText'; relatedResultIds: string[]; audio: string; text: string };