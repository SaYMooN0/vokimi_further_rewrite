import type { Err } from "$lib/ts/err";
import type { Language } from "$lib/ts/language";
import type { VokiType } from "$lib/ts/voki-type";

export interface VokiItemViewOkStateProps {
    vokiId: string;
    type: VokiType;
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
}

export interface VokiItemViewErrStateProps {
    vokiId?: string;
    errs: Err[];
}
