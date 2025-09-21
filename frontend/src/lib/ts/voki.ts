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


export type PublishedVokiBriefInfo = {
    id: string;
    type: VokiType;
    name: string;
    cover: string;
    primaryAuthorId: string;
    coAuthorIds: string[];
    hasMatureContent: boolean;
    language: Language;
	authenticatedOnlyTaking: boolean;
};