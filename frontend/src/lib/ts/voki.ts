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

export type AnswerDataTextOnly = { answerType: 'TextOnly', text: string };
export type AnswerDataImageOnly = { answerType: 'ImageOnly', image: string };
export type AnswerDataImageAndText = { answerType: 'ImageAndText', image: string; text: string };
export type AnswerDataColorOnly = { answerType: 'ColorOnly', color: string };
export type AnswerDataColorAndText = { answerType: 'ColorAndText', color: string; text: string };
export type AnswerDataAudioOnly = { answerType: 'AudioOnly', audio: string };
export type AnswerDataAudioAndText = { answerType: 'AudioAndText', audio: string; text: string };

