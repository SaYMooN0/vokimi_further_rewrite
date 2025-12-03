import type { Err } from "./err";
import type { Language } from "./language";
import type { VokiType } from "./voki-type";


export type GeneralVokiAnswerType =
    | "TextOnly"
    | "ImageOnly"
    | "ImageAndText"
    | "ColorOnly"
    | "ColorAndText"
    | "AudioOnly"
    | "AudioAndText";

export type GeneralVokiResultsVisibility =
    | "Anyone"
    | "AfterTaking"
    | "OnlyReceived";

export type PublishedVokiBriefInfo = {
    id: string;
    type: VokiType;
    name: string;
    cover: string;
    primaryAuthorId: string;
    coAuthorIds: string[];
    hasMatureContent: boolean;
    language: Language;
    signedInOnlyTaking: boolean;
    publicationDate: Date;
};
export type PublishedVokiViewState =
    | { state: "loading", vokiId: string }
    | { state: "ok"; data: PublishedVokiBriefInfo }
    | { state: "errs"; errs: Err[], vokiId: string };