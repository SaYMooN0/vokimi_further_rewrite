import type { Err } from "./err";
import type { Language } from "./language";
import type { VokiType } from "./voki-type";


export type GeneralVokiQuestionContentType =
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
    managerIds: string[];
    hasMatureContent: boolean;
    language: Language;
    signedInOnlyTaking: boolean;
    publicationDate: Date;
};
export type PublishedVokiViewState =
    | { state: "loading", vokiId: string }
    | { state: "ok"; data: PublishedVokiBriefInfo }
    | { state: "errs"; errs: Err[], vokiId: string };

export type VokiRatingValue = 1 | 2 | 3 | 4 | 5;

export namespace VokiUtils {
    export function canUserManageVoki(voki: PublishedVokiBriefInfo, signedInUserId: string): boolean {
        console.log(voki, signedInUserId);
        return voki.primaryAuthorId === signedInUserId || voki.managerIds.includes(signedInUserId);
    }
}