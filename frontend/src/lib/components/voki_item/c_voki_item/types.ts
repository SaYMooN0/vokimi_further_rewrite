import type { Err } from "$lib/ts/err";
import type { Language } from "$lib/ts/language";

export interface VokiItemViewOkStateProps {
    vokiId: string;
    voki: {
        name: string;
        cover: string;
        primaryAuthorId: string;
        coAuthorIds: string[];
    };
    onMoreBtnClick: (mEvent: MouseEvent) => void;
    link: string;
    flags?: {
        language: Language;
        hasMatureContent: boolean;
        authenticatedOnlyTaking: boolean;
    };
    publicationDate?: Date;
}

export interface VokiItemViewErrStateProps {
    vokiId: string;
    errs: Err[];
}
