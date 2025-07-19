export type VokiType = 'General' | 'Scoring' | 'TierList';

export type GeneralVokiAnswerType =
    | "TextOnly"
    | "ImageOnly"
    | "ImageAndText"
    | "ColorOnly"
    | "ColorAndText"
    | "AudioOnly"
    | "AudioAndText";

export type AnswerTypeData =
    | AnswerDataTextOnly
    | AnswerDataImageOnly
    | AnswerDataImageAndText
    | AnswerDataColorOnly
    | AnswerDataColorAndText
    | AnswerDataAudioOnly
    | AnswerDataAudioAndText;

export type AnswerDataTextOnly = { text: string };
export type AnswerDataImageOnly = { image: string };
export type AnswerDataImageAndText = { image: string; text: string };
export type AnswerDataColorOnly = { color: string };
export type AnswerDataColorAndText = { color: string; text: string };
export type AnswerDataAudioOnly = { audio: string };
export type AnswerDataAudioAndText = { audio: string; text: string };
