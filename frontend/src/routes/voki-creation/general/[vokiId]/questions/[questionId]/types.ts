import type { GeneralVokiAnswerType, GeneralVokiAnswerTypeData } from "$lib/ts/voki";

export type QuestionFullInfo = {
    id: string;
    text: string;
    images: string[];
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
    typeData: GeneralVokiAnswerTypeData;
    relatedResultIds: string[]
}

export type ResultIdWithName = {
    id: string;
    name: string;
}

export function createEmptyGeneralVokiAnswerTypeData(type: GeneralVokiAnswerType): GeneralVokiAnswerTypeData {
    switch (type) {
        case 'TextOnly':
            return { type: 'TextOnly', relatedResultIds: [], text: '' };
        case 'ImageOnly':
            return { type: 'ImageOnly', relatedResultIds: [], image: '' };
        case 'ImageAndText':
            return { type: 'ImageAndText', relatedResultIds: [], image: '', text: '' };
        case 'ColorOnly':
            return { type: 'ColorOnly', relatedResultIds: [], color: '' };
        case 'ColorAndText':
            return { type: 'ColorAndText', relatedResultIds: [], color: '', text: '' };
        case 'AudioOnly':
            return { type: 'AudioOnly', relatedResultIds: [], audio: '' };
        case 'AudioAndText':
            return { type: 'AudioAndText', relatedResultIds: [], audio: '', text: '' };
    }
}