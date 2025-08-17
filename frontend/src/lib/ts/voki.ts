import type { Language } from "./language";

export type VokiType = 'General' | 'Scoring' | 'TierList';

export type GeneralVokiAnswerType =
    | "TextOnly"
    | "ImageOnly"
    | "ImageAndText"
    | "ColorOnly"
    | "ColorAndText"
    | "AudioOnly"
    | "AudioAndText";

export type GeneralVokiAnswerTypeData =
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



export type PublishedVokiBriefInfo = {
    id: string;
    type: VokiType;
    name: string;
    cover: string;
    primaryAuthorId: string;
    coAuthorIds: string[];
    isAgeRestricted: boolean;
    language: Language;
};