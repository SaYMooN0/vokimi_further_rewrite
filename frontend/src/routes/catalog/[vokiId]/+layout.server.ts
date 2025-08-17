import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { Language } from "$lib/ts/language";
import type { VokiType } from "$lib/ts/voki";
import type { LayoutServerLoad } from "./$types";

export const load: LayoutServerLoad = async ({ params, fetch }) => {
    return {
        response: await ApiVokisCatalog.serverFetchJsonResponse<VokiOverviewInfo>(
            fetch, `/vokis/${params.vokiId}/overview`, { method: 'GET' }
        ),
        vokiId: params.vokiId
    };
}

type VokiOverviewInfo = {
    id: string;
    type: VokiType;
    name: string;
    cover: string;
    primaryAuthorId: string;
    coAuthorIds: string[];
    description: string;
    isAgeRestricted: boolean;
    language: Language;
    tags: string[];
    ratingsCount: number;
    commentsCount: number;
};
