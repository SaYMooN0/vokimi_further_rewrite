import { ApiVokisCatalog } from "$lib/ts/backend-communication/backend-services";
import type { Language } from "$lib/ts/language";
import type { VokiType } from "$lib/ts/voki";
import type { ServerLoad } from "@sveltejs/kit";

export const load: ServerLoad = async ({ params, fetch }) => {
    const response = await ApiVokisCatalog.serverFetchJsonResponse<VokiOverviewInfo>(
        fetch, `/vokis/${params.vokiId}/overview`, { method: 'GET' }
    );
    return { ...response, vokiId: params.vokiId };
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
